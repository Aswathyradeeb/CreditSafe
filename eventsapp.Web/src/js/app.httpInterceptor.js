(function () {
    'use strict';

    angular
        .module('eventsapp')
        .factory('httpInterceptor', interceptor);

    interceptor.$inject = ['$q', 'toaster', '$rootScope'];

    function interceptor($q, toaster, $rootScope) {
        return {
            //// Use when request sent 
            //'request': function (config) {
            //    toaster.pop({
            //        type: 'success',
            //        title: "Success",
            //        body: "Request Successfully",
            //        showCloseButton: true,
            //        timeOut: 0,
            //        extendedTimeOut: 0
            //    });
            //    return config;
            //},

            //// Use when request sent 
            //'requestError': function (config) {
            //toaster.pop({
            //    type: 'error',
            //    title: config.statusText,
            //    body: config.data.message,
            //    showCloseButton: true,
            //    timeOut: 0,
            //    extendedTimeOut: 0
            //});
            //    return config;
            //},

            //// Use when response recieved        
            //'response': function (response) {
            //    // do something on response success
            //    toaster.pop({
            //        type: 'success',
            //        title: "Success",
            //        body: "Response Recieved Successfully",
            //        showCloseButton: true,
            //        timeOut: 100,
            //        extendedTimeOut: 100
            //    });

            //    return response;
            //},

            // This is the responseError interceptor
            responseError: function (rejection) {
                if (rejection.status == 401 && rejection.data.message == "Authorization has been denied for this request.") { // assuming that any code over 399 is an error
                    toaster.pop({
                        type: 'error',
                        title: "Internal Server Error",
                        body: rejection.data.message,
                        showCloseButton: true,
                        timeOut: 100,
                        extendedTimeOut: 100
                    });
                    
                    $rootScope.$emit("unauthorized");
                    $q.reject(rejection);
                };
                throw rejection;
            }
        };
    }
})();