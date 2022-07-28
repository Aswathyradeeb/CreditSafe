(function () {
    'use strict';

    angular
        .module('eventsapp')
        .directive('uiSelectRequired', Directive);

    function Directive($rootScope) {
        return {
            require: 'ngModel',
            link: function (scope, elm, attrs, ctrl) {
                ctrl.$validators.uiSelectRequired = function (modelValue, viewValue) {
                    return modelValue && modelValue.length;
                };
            }
        };
    }
})();