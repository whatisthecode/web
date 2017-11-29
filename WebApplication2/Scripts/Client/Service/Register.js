if ("undefined" !== typeof app) {
    app.factory('Register', function (API, Helper) {
        var apiName = {
            "register": "account/register",
        };
        return {
            register: function (userInfo, success, fail) {
                API.request(false, "post", apiName.register).then(function (result) {
                    success(result.data);
                    fail(null);
                }, function (error) {
                    success(null);
                    fail(error);
                });
            }
        }
    })
}