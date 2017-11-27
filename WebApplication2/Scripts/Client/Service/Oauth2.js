if ("undefined" !== typeof app) {
    app.factory('Oauth2', function (CONFIG, $cookies) {
        var userInfo = $cookies.get("userInfo");
        var token = $cookies.get("token");
        return {
            setUserInfo: function (userInfo) {
                $cookies.set("userInfo", userInfo);
                userInfo = $cookies.get("userInfo");
            },
            setToken: function (token) {
                $cookies.set("token", token);
                token = $cookies.get("token");
            },
            clear: function () {
                token = null;
                userInfo = null;
                $cookies.remove("userInfo");
                $cookies.remove("token");
            },
            isExpired: function () {
                if (this.token.expiredIn) {
                    var expiredTime = new Date(token.expiredTime);
                    var now = Date.now();
                    if (expiredTime <= now) {
                        return true;
                    }
                    return false;
                }
                return true;
            }
        };
    });
}