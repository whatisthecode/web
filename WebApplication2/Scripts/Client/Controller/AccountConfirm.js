if ("undefined" !== typeof app) {
    app.controller("AccountConfirmController", function ($scope) {
        setTimeout(function () {
            window.location.href = "/";
        }, 3000)
    });
}