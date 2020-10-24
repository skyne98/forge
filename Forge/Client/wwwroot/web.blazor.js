if (!window.webBlazor) {
    window.webBlazor = {};
}
window.webBlazor = {
    copyTextToClipboard: function (text, callbackTarget, callback, callbackError) {
        navigator.clipboard.writeText(text).then(function () {
            callbackTarget.invokeMethod(callback);
        })
        .catch(function (error) {
            callbackTarget.invokeMethod(callbackError, error);
        });
    }
};