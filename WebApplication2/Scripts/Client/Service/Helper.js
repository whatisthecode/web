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
            },
            validateEmail: function (email) {
                if (notEmpty(email) == true)
                {
                    var pattern = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;
                    if (pattern.test(value))
                        return true;
                    else
                        return false;
                }
            },
            validateConfirmPassword: function (password1, password2)
            {
                if (notEmpty(password1) == true && notEmpty(password2) == true)
                {
                    if (password1 === password2)
                        return true;
                    else
                        return false;
                }
            },
            notEmpty: function (value) {
                if (value == "" || value == null || typeof (value) == "undefined")
                    return false;
                else
                    return true;
            },
            isNumber: function (value)
            {
                if (typeof (value) == "number")
                    return true;
                else
                    return false;
            },
            lengthOfString: function (max = 10, min = 1, value)
            {
                if (value.length <= max && value.length >= min)
                    return true;
                else
                    return false;
            },
            isValidNumber: function (value)
            {
                if (notEmpty(value) == true)
                {
                    var pattern = /^\d+$/;
                    if (pattern.test(value))
                        return true;
                    else
                        return false;
                }
            }
        };
    });
}