var pixiInstances = new Map();
var pixiInstaceDatas = new Map();

if (!window.pixiBlazor) {
    window.pixiBlazor = {};
}
window.pixiBlazor = {
    /**
     * Create a new Pixi instance
     * @param {string} target
     */
    create: function (dotnetHelper, target, onUpdate) {
        $(document).ready(function () {
            /* Greetings */
            let type = "WebGL"
            if (!PIXI.utils.isWebGLSupported()) {
                type = "canvas"
            }

            PIXI.utils.sayHello(type);

            /* Initialize */
            let canvas = $(target)[0];
            let app = new PIXI.Application({
                view: canvas,
                resizeTo: canvas,
                antialias: true,
                transparent: false
            });

            /* Initialize the FPS counter */
            let showCounter = true;

            if (showCounter) {
                let stats = new Stats();
                stats.showPanel(0); // 0: fps, 1: ms, 2: mb, 3+: custom
                canvas.parentElement.appendChild(stats.dom);

                app.ticker.add((delta) => {
                    stats.begin();
                    dotnetHelper.invokeMethod(onUpdate, delta, app.ticker.elapsedMS / 1000)
                    stats.end();
                });
            }
            else {
                app.ticker.add((delta) => dotnetHelper.invokeMethod(onUpdate, delta, app.ticker.elapsedMS / 1000));
            }

            pixiInstances.set(target, app);
            pixiInstaceDatas.set(target, { lastId: 0, displayObjects: new Map(), containers: new Map(), sprites: new Map() });
        });
    },
    remove: function (target) {
        $(document).ready(function () {
            let pixi = pixiInstances.get(target);
            if (typeof (pixi) != "undefined") {
                pixi.destroy();
                pixiInstances.delete(target);
                pixiInstaceDatas.delete(target);
            }
        });
    },

    start: function (target) {
        let pixi = pixiInstances.get(target);
        if (typeof (pixi) != "undefined") {
            pixi.start();
        }
    },
    stop: function (target) {
        let pixi = pixiInstances.get(target);
        if (typeof (pixi) != "undefined") {
            pixi.stop();
        }
    },
    /* Screen */
    screenGetWidth: function (target) {
        let pixi = pixiInstances.get(target);
        if (typeof (pixi) != "undefined") {
            return `[${pixi.screen.width}, ${pixi.screen.height}]`;
        }
    },

    /* Stage */
    sprite: function (target, uri) {
        let pixi = pixiInstances.get(target);
        let pixiData = pixiInstaceDatas.get(target);
        if (typeof (pixi) != "undefined" && typeof (pixiData) != "undefined") {
            let id = pixiData.lastId + 1;
            pixiData.lastId = id;
            let sprite = PIXI.Sprite.from(uri);
            pixiData.displayObjects.set(id, sprite);
            pixiData.containers.set(id, sprite);
            pixiData.sprites.set(id, sprite);

            return id;
        }
    },
    container: function (target) {
        let pixi = pixiInstances.get(target);
        let pixiData = pixiInstaceDatas.get(target);
        if (typeof (pixi) != "undefined" && typeof (pixiData) != "undefined") {
            let id = pixiData.lastId + 1;
            pixiData.lastId = id;
            let container = new PIXI.Container();
            pixiData.displayObjects.set(id, container);
            pixiData.containers.set(id, container);

            return id;
        }
    },
    addDisplayObjectToStage: function (target, id) {
        let pixi = pixiInstances.get(target);
        let pixiData = pixiInstaceDatas.get(target);
        if (typeof (pixi) != "undefined" && typeof (pixiData) != "undefined") {
            let displayObject = pixiData.displayObjects.get(id);
            pixi.stage.addChild(displayObject);
        }
    },
    removeDisplayObjectFromStage: function (target, id) {
        let pixi = pixiInstances.get(target);
        let pixiData = pixiInstaceDatas.get(target);
        if (typeof (pixi) != "undefined" && typeof (pixiData) != "undefined") {
            let displayObject = pixiData.displayObjects.get(id);
            pixi.stage.removeChild(displayObject);
        }
    },

    /* Display object */
    addDisplayObjectToContainer: function (target, id, containerId) {
        let pixi = pixiInstances.get(target);
        let pixiData = pixiInstaceDatas.get(target);
        if (typeof (pixi) != "undefined" && typeof (pixiData) != "undefined") {
            let displayObject = pixiData.displayObjects.get(id);
            let container = pixiData.containers.get(containerId);
            container.addChild(displayObject);
        }
    },
    removeDisplayObjectFromContainer: function (target, id, containerId) {
        let pixi = pixiInstances.get(target);
        let pixiData = pixiInstaceDatas.get(target);
        if (typeof (pixi) != "undefined" && typeof (pixiData) != "undefined") {
            let displayObject = pixiData.displayObjects.get(id);
            let container = pixiData.containers.get(containerId);
            container.removeChild(displayObject);
        }
    },
    getDisplayObjectMember: function (target, id, memberPath) {
        let pixi = pixiInstances.get(target);
        let pixiData = pixiInstaceDatas.get(target);
        if (typeof (pixi) != "undefined" && typeof (pixiData) != "undefined") {
            let displayObject = pixiData.displayObjects.get(id);
            let result = displayObject;
            for (let member of memberPath) {
                result = result[member];
            }

            if (result == displayObject)
                return undefined;
            else
                return result;
        }
    },
    setDisplayObjectMember: function (target, id, memberPath, value) {
        let pixi = pixiInstances.get(target);
        let pixiData = pixiInstaceDatas.get(target);
        if (typeof (pixi) != "undefined" && typeof (pixiData) != "undefined") {
            let displayObject = pixiData.displayObjects.get(id);
            let result = displayObject;
            let depth = 1;
            for (let member of memberPath) {
                if (depth == memberPath.length) {
                    result[member] = value;
                    break;
                }
                else
                    result = result[member];

                depth += 1;
            }
        }
    },

    /* Container */
    getContainerMember: function (target, id, memberPath) {
        let pixi = pixiInstances.get(target);
        let pixiData = pixiInstaceDatas.get(target);
        if (typeof (pixi) != "undefined" && typeof (pixiData) != "undefined") {
            let displayObject = pixiData.containers.get(id);
            let result = displayObject;
            for (let member of memberPath) {
                result = result[member];
            }

            if (result == displayObject)
                return undefined;
            else
                return result;
        }
    },
    setContainerMember: function (target, id, memberPath, value) {
        let pixi = pixiInstances.get(target);
        let pixiData = pixiInstaceDatas.get(target);
        if (typeof (pixi) != "undefined" && typeof (pixiData) != "undefined") {
            let displayObject = pixiData.containers.get(id);
            let result = displayObject;
            let depth = 1;
            for (let member of memberPath) {
                if (depth == memberPath.length) {
                    result[member] = value;
                    break;
                }
                else
                    result = result[member];

                depth += 1;
            }
        }
    },

    /* Sprite */
    getSpriteMember: function (target, id, memberPath) {
        let pixi = pixiInstances.get(target);
        let pixiData = pixiInstaceDatas.get(target);
        if (typeof (pixi) != "undefined" && typeof (pixiData) != "undefined") {
            let displayObject = pixiData.sprites.get(id);
            let result = displayObject;
            for (let member of memberPath) {
                result = result[member];
            }

            if (result == displayObject)
                return undefined;
            else
                return result;
        }
    },
    setSpriteMember: function (target, id, memberPath, value) {
        let pixi = pixiInstances.get(target);
        let pixiData = pixiInstaceDatas.get(target);
        if (typeof (pixi) != "undefined" && typeof (pixiData) != "undefined") {
            let displayObject = pixiData.sprites.get(id);
            let result = displayObject;
            let depth = 1;
            for (let member of memberPath) {
                if (depth == memberPath.length) {
                    result[member] = value;
                    break;
                }
                else
                    result = result[member];

                depth += 1;
            }
        }
    }
};