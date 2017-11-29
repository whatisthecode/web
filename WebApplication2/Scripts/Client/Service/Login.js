if ("undefined" !== typeof app) {
    app.factory('Login', function (API, Helper, Oauth2) {
        var apiName = {
            "login": "token"
        };
        return {
            login: function (username, password, success, fail) {
                var data = {
                    username: username,
                    password: password,
                    grant_type: "password"
                };
                var headers = {
                    'Content-Type': "application/x-www-form-urlencoded"
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