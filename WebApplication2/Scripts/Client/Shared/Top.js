if ("undefined" !== typeof app) {
    app.controller("TopController", function ($scope, Helper, Login) {
        console.log("this is top");
        $scope.login = function () {
            if (!Helper.notEmpty($scope.email)) {
                validator.prototype.showWarning("#errors", "emptyEmail", "Chưa nhập email");
                return;
            }
            else
            {
                validator.prototype.hideWarning("#errors", "emptyEmail", "Chưa nhập email");
            }

            if (!Helper.notEmpty($scope.password)) {
                validator.prototype.showWarning("#errors", "emptyPassword", "Chưa nhập mật khẩu");
                return;
            }
            else {
                validator.prototype.hideWarning("#errors", "emptyPassword", "Chưa nhập mật khẩu");
            }

            Login.login($scope.email, $scope.password, function (response) {
                if (response) {
                    localStorage.setItem("userData", JSON.stringify(response.results));
                    validator.prototype.hideWarning("#errors", "loginFail", "Sai tên đăng nhập hoặc mật khẩu");
                }
            }, function (err) {
                if (err) {
                    switch (err.code) {
                        case "404":
                            validator.prototype.showWarning("#errors", "loginFail", "Sai tên đăng nhập hoặc mật khẩu");
                            break;
                        default:
                            console.log(err);
                            break;
                    }
                }
            });
        };
    });
}