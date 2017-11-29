if ("undefined" !== typeof app) {
    app.controller("LoginController", function ($scope, Login) {
        $scope.login = function () {
            if (!$scope.username) {

            }
            if (!$scope.password) {

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