function createScreenSize() {
    return {
        width: document.documentElement.clientWidth,
        height: document.documentElement.clientHeight
    };
}

window.getScreenSize = () => {
    return createScreenSize();
};

window.initResizeHandler = (dotNetObject) => {
    let resizeTimeout;

    window.removeEventListener('resize', window._resizeHandler);

    window._resizeHandler = () => {
        clearTimeout(resizeTimeout);
        resizeTimeout = setTimeout(() => {
            dotNetObject.invokeMethodAsync('OnResizeAsync', createScreenSize());
        }, 200);
    };

    window.addEventListener('resize', window._resizeHandler);
};
