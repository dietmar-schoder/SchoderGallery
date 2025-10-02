window.getScreenSize = () => {
    return {
        width: document.documentElement.clientWidth - 4,
        height: document.documentElement.clientHeight - 4
    };
};