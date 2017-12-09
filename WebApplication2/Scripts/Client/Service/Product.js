if ("undefined" !== typeof app) {
    app.factory('Product', function (API, Helper, Oauth2) {
        var apiName = {
            "getProduct": "api/product/"
        };
        return {
            getProduct: function (success, fail) {
                var headers = {
                    'Content-Type': "application/json"
                };
                API.request(false, "get", apiName.getProduct, headers, null).then(function (result) {
                    success(result.data.results.results);
                    fail(null);
                }, function (error) {
                    success(null);
                    fail(error);
                });
            }
        };
    });
}