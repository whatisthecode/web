if ("undefined" !== typeof app) {
    app.factory('Login', function (API, Helper, Oauth2) {
        var apiName = {
            "login": "api/account/login"
        };
        return {
            login: function (email, password, success, fail) {
                var data = {
                    email: email,
                    password: password
                };
                var headers = {
                    'Content-Type': "application/json"
                };
                API.request(false, "post", apiName.login, headers, data).then(function (result) {
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