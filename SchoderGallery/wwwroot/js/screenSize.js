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

window.centerSpinner = () => {
    const spinner = document.getElementById('spinner');
    if (!spinner) return;

    const width = spinner.offsetWidth;
    const height = spinner.offsetHeight;

    const viewportWidth = document.documentElement.clientWidth;
    const viewportHeight = document.documentElement.clientHeight;

    spinner.style.left = `${(viewportWidth - width) / 2}px`;
    spinner.style.top = `${(viewportHeight - height) / 2}px`;
};

window.addEventListener('load', window.centerSpinner);