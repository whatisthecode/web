if ("undefined" !== typeof app) {
    app.factory('ProductDetail', function (API, Helper, Oauth2) {
        var apiName = {
            "getProductDetailById": "api/product/$1"
        };
        return {
            getProductDetail: function (productId, success, fail) {
                var headers = {
                    'Content-Type': "application/json"
                };
                var api = Helper.fixUrlAPI(apiName.getProductDetailById, [productId]);
                API.request(false, "get", api, headers, null).then(function (result) {
                    success(result.data);
                    fail(null);
                }, function (error) {
                    success(null);
                    fail(error);
                });
            }
        };
    });
}