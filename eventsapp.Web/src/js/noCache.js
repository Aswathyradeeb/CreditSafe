function NoneCahedURL(__URL) {
    try {
        var lastPart = __URL.substr(__URL.lastIndexOf('/') + 1);
        if (lastPart.indexOf("?") == -1 && (lastPart.indexOf(".html") >= 0 || lastPart.indexOf(".js") >= 0)) {
            if (document.URL.indexOf("localhost") >= 0) {
                var __d = new Date();
                __URL = __URL + '?' + __d.getMinutes() + __d.getSeconds() + __d.getMilliseconds();
            } else {
                __URL = __URL + '?v.0.02';
            }
        }
    }
    catch (e) { }
    return __URL;
}