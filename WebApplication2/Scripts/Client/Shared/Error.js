if ("undefined" !== typeof app) {
    app.controller("ErrorController", function ($scope, $location) {
        console.log("aaaaaaaaaaaa");
        location.reload();
    });
}