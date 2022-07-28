 

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
    function SidebarController($rootScope, LoginService, $state, $stateParams, UserProfile, $http) {
        var vm = this;

        vm.user = UserProfile.getProfile();
        if (vm.user.roleName == 1) {
            vm.AdminMode = true;
            $rootScope.app.sidebar.isOffscreen = false;
        }
        if (vm.user.roleName == 2) {
            vm.AdminMode = false;
            $rootScope.app.sidebar.isOffscreen = false;
        }
        setTimeout(function () {
            //init side bar - left
            $.Sidemenu.init();
            //init fullscreen
            $.FullScreen.init();
            //init fullscreen
            $.Components.init();
        },1000);
    }
    SidebarController.$inject = ['$rootScope', 'LoginService', '$state', '$stateParams', 'UserProfile', '$http'];

})();
