function createScreenSize() {
    return {
        width: document.documentElement.clientWidth,
        height: document.documentElement.clientHeight
    };
}

window.getScreenSize = () => {
    return createScreenSize();
};

window.initResizeHandler = (dotNetObject, interval) => {
    let resizeTimeout;

    if (window._resizeIntervalId) {
        clearInterval(window._resizeIntervalId);
        window._resizeIntervalId = null;
    }

    window.removeEventListener('resize', window._resizeHandler);

    window._resizeHandler = () => {
        clearTimeout(resizeTimeout);
        resizeTimeout = setTimeout(() => {
            dotNetObject.invokeMethodAsync('OnResizeAsync', createScreenSize());
        }, 200);
    };

    window.addEventListener('resize', window._resizeHandler);

    if (interval > 0) {
        window._resizeIntervalId = setInterval(() => {
            dotNetObject.invokeMethodAsync('OnResizeAsync', createScreenSize());
        }, interval);
    }
};