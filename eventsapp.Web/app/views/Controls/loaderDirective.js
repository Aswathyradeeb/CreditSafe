(function () {
    'use strict';

    angular
        .module('eventsapp')
        .directive('spinLoader', loaderDirective);

    loaderDirective.$inject = ['$http'];

    function loaderDirective($http) {
        return {
            restrict: 'A',
            scope: {
                isLoadingRoot: '=isLoading'
            },
            link: function (scope, elm) {
                scope.isLoading = function () {
                    scope.isLoadingRoot = $http.pendingRequests.length > 0;
                    return scope.isLoadingRoot;
                };

                scope.$watch(scope.isLoading, function (v) {
                    if (v) {
                        elm.removeClass('display-none');
                    } else {
                        elm.addClass('display-none');
                    }
                });
            }
        };
    }
})();