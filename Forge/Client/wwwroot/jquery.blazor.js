if (!window.jqueryBlazor) {
    window.jqueryBlazor = {};
}
window.jqueryBlazor = {
    hide: function (selector, duration) {
        $(selector).hide(duration);
    },
    show: function (selector, duration) {
        $(selector).show(duration);
    },
    toggle: function (selector, duration) {
        console.log("JS -> Toggle(", selector, ",", duration, ")");
        $(selector).toggle(duration);
    }
};