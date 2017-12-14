if ("undefined" !== typeof app) {
    app.factory('User', function (API, Helper, Oauth2) {
        var apiName = {
            "setPassword": "api/account/setPassword",
            "getUserInfo": "api/account/UserInfo"
        };
        return {
            setPassword: function (data, success, fail) {
                var reqData = {
                    "email": data.email,
                    "newPassword": data.newPassword,
                    "confirmPassword": data.confirmPassword
                };
                var headers = {
                    'Content-Type': "application/json"
                };
                API.request(false, "post", apiName.setPassword, headers, reqData).then(function (result) {
                    success(result.data);
                    fail(null);
                }, function (error) {
                    success(null);
                    fail(error);
                });
            },
            getUserInfo: function (success, fail)
            {
                var headers = {
                    'Content-Type': "application/json"
                };
                API.request(true, "get", apiName.getUserInfo, headers, null).then(function (result) {
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