 

/**=========================================================
 * Module: HeaderNavController
 * Controls the header navigation
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('SidebarController', SidebarController);
    /* @ngInject */
    function SidebarController($rootScope, $state, $stateParams, $http) {
        var vm = this;

            vm.AdminMode = false;
            $rootScope.app.sidebar.isOffscreen = false;
        
        setTimeout(function () {
            //init side bar - left
            $.Sidemenu.init();
            //init fullscreen
            $.FullScreen.init();
            //init fullscreen
            $.Components.init();
        },1000);
    }
    SidebarController.$inject = ['$rootScope', '$state', '$stateParams', '$http'];

})();
