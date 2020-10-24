var uppyInstances = new Map();

if (!window.uppyBlazor) {
    window.uppyBlazor = {};
}
window.uppyBlazor = {
    /**
     * Create a new Uppy instance
     * @param {string} target
     * @param {boolean} inline
     */
    create: function (dotnetHelper, target, inline, fileAdded, fileRemoved) {
        console.log(target, document.querySelector(target));

        let uppy = Uppy.Core({
            restrictions: {
                maxNumberOfFiles: 1,
                allowedFileTypes: ['image/*']
            }
        });
        uppy.use(Uppy.Dashboard, {
            inline: inline,
            target: target,
            hideUploadButton: true,
            proudlyDisplayPoweredByUppy: false
        });
        /*
        uppy.use(Uppy.ImageEditor, {
            target: Uppy.Dashboard,
            quality: 0.8
        })
        */

        uppy.on('file-added', (file) => {
            console.log(dotnetHelper);
            const toBase64 = file => new Promise((resolve, reject) => {
                const reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = () => resolve(reader.result);
                reader.onerror = error => reject(error);
            });

            toBase64(file.data).then(base64 => {
                console.log('Added file', file.name, 'with type', file.type);
                dotnetHelper.invokeMethod(fileAdded, file.name, file.type, base64);
            });
        });
        uppy.on('file-removed', (file) => {
            console.log('Removed file', file);
            dotnetHelper.invokeMethod(fileRemoved, file.name);
        });

        uppyInstances.set(target, uppy);
    },
    remove: function (target) {
        $(document).ready(function () {
            let uppy = uppyInstances.get(target);
            if (typeof (uppy) != "undefined") {
                uppy.close();

                let wrapperSelector = target;
                console.log('JS -> Removing uppy ' + wrapperSelector);
                $(wrapperSelector).remove();

                uppyInstances.delete(target);
            }
        });
    }
};