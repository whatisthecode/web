if ("undefined" !== typeof app) {
    app.controller("LoginController", function ($scope, Login, Helper) {
        $scope.login = function () {
            if (Helper.isNotEmpty($scope.email)) {
                dasdasdas;
            }
            if (!$scope.password) {
                dsadsa;
            }
            Login.login($scope.username, $scope.password, function (data) {
                if (data) {
                    console.log(data);
                }
            }, function (err) {
                if (err.status === 400) {
                    console.log("Sai tên đăng nhập hoặc mật khẩu");
                }
            });
        };
    });
}