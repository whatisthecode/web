if ("undefined" !== typeof app) {
    app.controller("SetPasswordController", function ($scope, $http) {
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
            var req = {
                method: 'POST',
                url: 'http://localhost:54962/api/Account/SetPassword',
                headers: {
                    'Content-Type': "application/json"
                },
                data: $scope.data
            }
            $http(req).then(function (response) {
                console.log(response);
            }, function (response) {
                console.log(response);
            });
        }
    });
}