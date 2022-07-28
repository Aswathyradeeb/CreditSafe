
(function () {
    'use strict';

    angular.module('eventsapp').filter('localizeString', ['$rootScope', function ($rootScope) {
        return function (input, englishField, arabicField) {
            if (englishField == undefined) {
                englishField = 'nameEn';
                arabicField = 'nameAr';
            }
            if (input !== undefined && input !== null) {
                if ($rootScope.app.layout.isRTL) {
                    return input[arabicField];
                }
                else {
                    return input[englishField];
                }
            }
        };
    }]);

    angular.module('eventsapp').filter('localizeDescString', ['$rootScope', function ($rootScope) {
        return function (input) {
            if (input !== undefined && input !== null) {
                if ($rootScope.app.layout.isRTL) {
                    return input.descAr;
                }
                else {
                    return input.descEn;
                }
            }
        };
    }]);

    angular.module('eventsapp').filter('localizeStringWithProperty', ['$rootScope', function ($rootScope) {
        return function (input, property) {
            if (input !== undefined && input !== null && property !== undefined && property !== null) {
                if ($rootScope.app.layout.isRTL) {
                    for (var key in input) {
                        if (input.hasOwnProperty(key) && key.indexOf(property) !== -1) {
                            return input[property + 'Ar'] != null ? input[property + 'Ar'] : '';
                        }
                    }
                }
                else {
                    for (var key in input) {
                        if (input.hasOwnProperty(key) && key.indexOf(property) !== -1) {
                            return input[property + 'En'] != null ? input[property + 'En'] : '';
                        }
                    }
                }
            }
        };
    }]);

})();