if ("undefined" !== typeof app) {
    app.controller("SetPasswordController", function ($scope, User, Helper) {
        $scope.data = {
            "email" : "",
            "newPassword": "",
            "confirmPassword" : ""
        }

        $scope.setPassword = function () {
            if (!Helper.validateEmail) {
                return;
            }
            if (!Helper.validateConfirmPassword($scope.newPassword, $scope.confirmPassword))
            {
                return;
            }
            User.setPassword($scope.data, function (reponse) {
                if (reponse)
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