var app = (function () {
    return {
        getUrlHash: function () {
            console.log('getUrlHash called');
            console.log(window.location.hash);
            console.log(window.location.href);
            return window.location.hash;
        }
    };
}());