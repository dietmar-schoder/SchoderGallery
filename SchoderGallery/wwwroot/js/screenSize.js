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

    window.addEventListener('resize', () => {
        clearTimeout(resizeTimeout);

        resizeTimeout = setTimeout(() => {
            dotNetObject.invokeMethodAsync('OnResize', createScreenSize());
        }, 1);
    });
};