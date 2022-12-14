/**=========================================================
 * Module: RoutesRun
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('eventsapp')
        .run(appRun);
    /* @ngInject */
    function appRun($rootScope, $window, $state, $http) {

        // Hook not found
        $rootScope.$on('$stateNotFound',
            function (event, unfoundState, fromState, fromParams) {
                console.log(unfoundState.to); // "lazy.state"
                console.log(unfoundState.toParams); // {a:1, b:2}
                console.log(unfoundState.options); // {inherit:false} + default options
            });

        // Hook success
        $rootScope.$on('$stateChangeSuccess',
            function (event, toState, toParams, fromState, fromParams) {
                // success here
                // display new view from top
                $window.scrollTo(0, 0);
            });

        $rootScope.$on('$stateChangeStart', function (event, newUrl, toParams, fromState, fromParams) {
           
        });

        $rootScope.$on('unauthorized', function () {
           
        });

    }
    appRun.$inject = ['$rootScope', '$window', '$state', '$http'];

})();

