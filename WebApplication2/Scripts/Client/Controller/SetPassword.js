if ("undefined" !== typeof app) {
    app.controller("SetPasswordController", function ($scope, CONFIG, User, Helper) {
        $scope.data = {
            "email": "",
            "newPassword": "",
            "confirmPassword": ""
        };

        $scope.disabled = true;

        $scope.validateForm = function () {
            var validate = true;
            if (!Helper.validateEmail($scope.data.email)) {
                validator.prototype.showWarning("#email", "validEmail", "Email Không hợp lệ");
                return validate = false;
            } else {
                validator.prototype.hideWarning("#email", "validEmail", "Email không hợp lệ");
            }

            if (!Helper.lengthOfString(20, 6, $scope.data.newPassword)) {
                validator.prototype.showWarning("#password", "newPassword", "Mật khẩu ít nhất 6 ký tự");
                return validate = false;
            } else {
                validator.prototype.hideWarning("#password", "newPassword", "Mật khẩu ít nhất 6 ký tự");
            }

            if (!Helper.lengthOfString(20, 6, $scope.data.confirmPassword)) {
                validator.prototype.showWarning("#confirmPassword", "confirmPassword1", "Mật khẩu ít nhất 6 ký tự");
                return validate = false;
            } else {
                validator.prototype.hideWarning("#confirmPassword", "confirmPassword1", "Mật khẩu ít nhất 6 ký tự");
            }

            if (!Helper.validateConfirmPassword($scope.data.newPassword, $scope.data.confirmPassword)) {
                validator.prototype.showWarning("#confirmPassword", "confirmPassword2", "Mật khẩu xác nhận không chính xác");
                return validate = false;
            } else {
                validator.prototype.hideWarning("#confirmPassword", "confirmPassword2", "Mật khẩu xác nhận không chính xác");

            }
            if (validate === true)
            {
                $scope.disabled = false;
            }
            else
            {
                $scope.disabled = true;
            }
            return validate;
        }

        $scope.setPassword = function () {
            User.setPassword($scope.data, function (reponse) {
                if (reponse) {
                    console.log(response);
                }
            }, function (err) {
                if (err.status === 400) {
                    console.log(err);
                }
            });
        };
    });
}