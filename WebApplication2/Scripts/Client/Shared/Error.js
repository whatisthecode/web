if ("undefined" !== typeof app) {
    app.controller("ErrorController", function ($scope, $location) {
        location.reload();
    });
}