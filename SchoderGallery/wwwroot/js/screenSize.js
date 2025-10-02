window.getScreenSize = () => {
    return {
        width: document.documentElement.clientWidth,
        height: document.documentElement.clientHeight
    };
};

window.initResizeHandler = (dotNetObject) => {
    let resizeTimeout;

    window.addEventListener('resize', () => {
        clearTimeout(resizeTimeout);

        resizeTimeout = setTimeout(() => {
            const size = {
                width: document.documentElement.clientWidth,
                height: document.documentElement.clientHeight
            };
            dotNetObject.invokeMethodAsync('OnResize', size);
        }, 20);
    });
};