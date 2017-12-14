if ("undefined" !== typeof app) {
    app.factory('API', function (CONFIG, Oauth2, Helper, $http, $httpParamSerializer) {
        return {
            request: function (isAuthenticated, method, apiEndpoint, headers = null, data = null, isAPI = false) {
                if (!headers)
                    headers = {};
                //if (isAuthenticated) {
                //    if (Oauth2.isExpired) {
                //        Oauth2.clear();
                //        location.href = "./login";
                //        return null;
                //    }
                //    headers['Authorization'] = "Bearer " + Oauth2.token.access_token;
                //}
                var token = localStorage.getItem("token");
                if (token)
                    headers['Authorization'] = "Bearer " + token;
                else
                    location.href = "./login";
                if (!headers['Content-Type'] && method !== "head" && method !== "options")
                    headers['Content-Type'] = undefined;
                var realApiEndpoint = Helper.getAPIEndpoint(apiEndpoint, isAPI);

                var transformRequest = angular.identity;
                if (headers["Content-Type"]) {
                    if (headers["Content-Type"] === "application/x-www-form-urlencoded")
                        transformRequest = $httpParamSerializer;
                }

                var req = {
                    method: method,
                    url: realApiEndpoint,
                    transformRequest: transformRequest,
                    headers: headers,
                    data: data || undefined
                };
                return $http(req);
            }
        };
    });
}