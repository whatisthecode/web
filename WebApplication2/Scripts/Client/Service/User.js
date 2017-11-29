if ("undefined" !== typeof app) {
    app.factory('User', function (API, Helper, Oauth2) {
        var apiName = {
            "setPassword": "cccount/setPassword"
        };
        return {
            setPassword: function (data, success, fail) {
                var headers = {
                    'Content-Type': "application/json"
                };
                API.request(false, "post", apiName.setPassword, headers, data).then(function (result) {
                    success(result.data);
                    fail(null);
                }, function (error) {
                    success(null);
                    fail(error);
                });
            }
        };
}