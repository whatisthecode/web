if ("undefined" !== typeof app) {
    app.controller("SetPasswordController", function ($scope, User) {
        $scope.data = {
            "email" : "",
            "newPassword": "",
            "confirmPassword" : ""
        }

        $scope.confirmPassword = function (password, confirmPassword) {
            console.log("a");
            if (password === confirmPassword)
            {
                return true;
            }
            else
            {
                return false
            }
        }

        $scope.setPassword = function () {
            User.setPassword(data, function (data) {
                if (data)
                {
                    console.log(data);
                }
            }, function (err) {
                if (err.status == 400)
                {
                    console.log(err);
                }
            })
        }
    });
}