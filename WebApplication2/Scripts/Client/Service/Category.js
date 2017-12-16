if ("undefined" !== typeof app) {
    app.factory('Category', function (API, Helper, Oauth2) {
        var apiName = {
            "getAllCategory": "api/category",
            "getCategoryByType": "api/category-type/$1"
        };
        return {
            getAllCategory: function (success, fail) {
                var headers = {
                    'Content-Type': "application/json"
                };
                API.request(false, "get", apiName.getAllCategory, headers, null).then(function (result) {
                    success(result.data);
                    fail(null);
                }, function (error) {
                    success(null);
                    fail(error);
                });
            },
            getCategoryByType: function (id, success, fail)
            {
                var api = Helper.fixUrlAPI(apiName.getCategoryByType, [id]);
                var headers = {
                    'Content-Type': "application/json"
                };
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