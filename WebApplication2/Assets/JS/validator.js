var validator = function () { };

var warningText = [];

checkTextExist = function (text) {
    for (var t in warningText)
    {
        if (warningText[t] === text)
            return true;
    }
    return false;
}

removeTextExist = function (text) {
    for (var i = 0; i < warningText.length; i++)
    {
        if (warningText[i] === text)
        {
            warningText.splice(i, 1);
        }
    }
}

validator.prototype.showWarning = function (selector, childSelector, text) {
    if (selector !== null && selector !== "" && typeof selector !== "undefined") {
        if (text !== "") {
            if (checkTextExist(text) === false)
            {
                warningText.push(text);
                $("<p class='text-danger' id='" + childSelector + "' style='margin : 0px; padding : 0px;'>" + text + "</p>").appendTo(selector);
            }
        }
    }
};

validator.prototype.hideWarning = function (selector, childSelector, text) {
    if (selector !== null && selector !== "" && typeof selector !== "undefined") {
        $(selector + " p#" + childSelector).remove();
        removeTextExist(text);
    }
};