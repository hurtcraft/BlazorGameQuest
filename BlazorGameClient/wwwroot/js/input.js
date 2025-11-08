window.inputManager = {
    pressedKeys: new Set(),
    init: function (dotnetObj) {
        window.addEventListener("keydown", (e) => {
            this.pressedKeys.add(e.code);
            dotnetObj.invokeMethodAsync("OnKeyDown", e.code);
        });
        window.addEventListener("keyup", (e) => {
            this.pressedKeys.delete(e.code);
            dotnetObj.invokeMethodAsync("OnKeyUp", e.code);
        });
    },
    isPressed: function (key) {
        return this.pressedKeys.has(key);
    }
};