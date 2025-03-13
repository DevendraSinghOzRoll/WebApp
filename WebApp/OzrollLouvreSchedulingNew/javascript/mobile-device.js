var isMobile = {
    Android: function() {
        return navigator.userAgent.match(/Android/i) ? true : false;
    },
    BlackBerry: function() {
        return navigator.userAgent.match(/BlackBerry/i) ? true : false;
    },
    iOS: function() {
        return navigator.userAgent.match(/iPhone|iPad|iPod/i) ? true : false;
    },
    iOS6Above: function() {
        if (isMobile.iOS())
            return navigator.userAgent.match(/OS 10|[6-9](_\d)?(_\d)? like Mac OS X/i) ? true : false;
        else
            return false;
    },
    iOS6Below: function() {
        if (isMobile.iOS())
            return navigator.userAgent.match(/OS [1-6](_\d)?(_\d)? like Mac OS X/i) ? true : false;
        else
            return false;
    },
    Windows: function() {
        return navigator.userAgent.match(/IEMobile/i) ? true : false;
    },
    any: function() {
        return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Windows());
    }
};