if("undefined" !== typeof app){
    app.factory('Helper', function (CONFIG) {

        return {
            fixUrlAPI: function (apiName, variables) {
                var url = apiName;
                if (Array.isArray(variables)) {
                    for (var i = 1; i <= variables.length; i++) {
                        url = url.replace("$" + i, variables[i - 1]);
                    }
                }
                return url;
            },
            getAPIEndpoint: function (path, isAPI) {
                var index = path.indexOf("/");
                if (index !== 0)
                    path = "/" + path;
                return isAPI ? CONFIG.apiEndpoint.replace(/^\/|\/$/g, '') + path : CONFIG.endpoint.replace(/^\/|\/$/g, '') + path;
            },
            toRealObject: function (object) {
                return Object.assign({}, object);
            }
        };
    });
}