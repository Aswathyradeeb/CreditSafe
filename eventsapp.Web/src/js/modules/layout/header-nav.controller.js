/**=========================================================
 * Module: HeaderNavController
 * Controls the header navigation
 =========================================================*/

(function() {
    'use strict';

    angular
        .module('eventsapp')
        .controller('HeaderNavController', HeaderNavController);
    /* @ngInject */    
    function HeaderNavController($rootScope, LoginService, $state, UserProfile) {
      var vm = this;
      vm.headerMenuCollapsed = true;
      vm.user = UserProfile.getProfile();

      vm.toggleHeaderMenu = function() {
        vm.headerMenuCollapsed = !vm.headerMenuCollapsed;
      };

      vm.logOut = function () {
          LoginService.logout();
          $state.go('page.login')
      };

      // Adjustment on route changes
      $rootScope.$on('$stateChangeStart', function(event, toState, toParams, fromState, fromParams) {
        vm.headerMenuCollapsed = true;
      });

    }
    HeaderNavController.$inject = ['$rootScope', 'LoginService', '$state', 'UserProfile'];

})();
