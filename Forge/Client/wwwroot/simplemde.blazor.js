var simplemdeInstances = new Map();

if (!window.simplemdeBlazor) {
    window.simplemdeBlazor = {};
}
window.simplemdeBlazor = {
    /**
     * Create a new Uppy instance
     * @param {string} target
     */
    create: function (dotnetHelper, target, readOnly, renderMarkdown, onChange) {
        let simplemde = new SimpleMDE({
            element: $(target)[0],
            previewRender: function (plainText) {
                let markdown = dotnetHelper.invokeMethod(renderMarkdown, plainText);
                return markdown;
            },
        });
        simplemde.codemirror.on("change", function () {
            let value = simplemde.value();
            dotnetHelper.invokeMethodAsync(onChange, value);
        });
        simplemde.codemirror.options.readOnly = readOnly;

        simplemdeInstances.set(target, simplemde)
    },
    value: function (target, value) {
        if (typeof (value) != "undefined") {
            // Setting the value
            let simplemde = simplemdeInstances.get(target);
            if (typeof (simplemde) != "undefined") {
                simplemde.value(value);   
            }
        }
        else {
            // Reading the value
            let simplemde = simplemdeInstances.get(target);
            if (typeof (simplemde) != "undefined") {
                return simplemde.value();
            }
        }

        return undefined;
    },
    remove: function (target, removeFromDOM) {
        let simplemde = simplemdeInstances.get(target);
        if (typeof (simplemde) != "undefined") {
            console.log('JS -> Removing SimpleMDE');
            simplemde.toTextArea();

            if (removeFromDOM) {
                let wrapperSelector = target + "_wrapper";
                console.log('JS -> Removing SimpleMDE from DOM with' + wrapperSelector);
                $(wrapperSelector).remove();
            }

            simplemdeInstances.delete(target);
        }
    }
};