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
    create: function (dotnetHelper, target, inline, width, height, fileAdded, fileRemoved) {
        console.log(target, document.querySelector(target));

        if (width == 0)
            width = undefined;
        if (height == 0)
            height = undefined;

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
            proudlyDisplayPoweredByUppy: false,
            width: width,
            height: height
        });
        /*
        uppy.use(Uppy.Url, {
            target: Uppy.Dashboard,
            companionUrl: 'https://companion.uppy.io/',
            locale: {}
        })
        */
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
    },
    isModalOpen: function (target) {
        let uppy = uppyInstances.get(target);
        if (typeof (uppy) != "undefined") {
            return uppy.getPlugin('Dashboard').isModalOpen();
        }
    },
    open: function (target) {
        let uppy = uppyInstances.get(target);
        if (typeof (uppy) != "undefined") {
            uppy.getPlugin('Dashboard').openModal();
        }
    },
    close: function (target) {
        let uppy = uppyInstances.get(target);
        if (typeof (uppy) != "undefined") {
            uppy.getPlugin('Dashboard').closeModal()
        }
    },
};