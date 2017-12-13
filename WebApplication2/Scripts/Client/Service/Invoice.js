if ("undefined" !== typeof app) {
    app.factory('Invoice', function (API, Helper, Oauth2) {
        var apiName = {
            "createInvoice": "api/invoice"
        };
        return {
            createInvoice: function (req, success, fail) {
                var data = Helper.toRealObject(req);
                var headers = {
                    'Content-Type': "application/json"
                };
                API.request(false, "post", apiName.createInvoice, headers, data).then(function (result) {
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