if ("undefined" !== typeof app) {
    app.factory('API', function (CONFIG, Oauth2, Helper, $http, $httpParamSerializer) {
        return {
            request: function (isAuthenticated, method, apiEndpoint, headers = null, data = null, isAPI = false) {
                if (!headers)
                    headers = {};
                var token = localStorage.getItem("token").toString();
                console.log(token);
                if (isAuthenticated) {
                    //if (Oauth2.isExpired) {
                    //    Oauth2.clear();
                    //    location.href = "./login";
                    //    return null;
                    //}
                    //headers['Authorization'] = "Bearer " + Oauth2.token.access_token;
                    if (token)
                        headers['Authorization'] = "Bearer " + token;
                    else {
                        var returnUrl = encodeURIComponent(location.origin + location.pathname);
                        location.href = "./login?returnUrl=" + returnUrl;     
                    }
                }
                if (!headers['Content-Type'] && method !== "head" && method !== "options") {
                    headers['Content-Type'] = undefined;
                    data = JSON.stringify(data);
                }
                var realApiEndpoint = Helper.getAPIEndpoint(apiEndpoint, isAPI);

                var req = {
                    method: method,
                    url: realApiEndpoint,
                    headers: headers,
                    data: data || undefined
                };
                if (headers["Content-Type"]) {
                    if (headers["Content-Type"] === "application/x-www-form-urlencoded")
                        req.transformRequest = $httpParamSerializer;
                }

                return $http(req);
            }
        };
    });
}