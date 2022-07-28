/**=========================================================
 * Module: LoginController.js
 * Controller for the Chat APP 
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('ConfirmEmailController', ConfirmEmailController);
    /* @ngInject */
    function ConfirmEmailController($rootScope, $filter, $http, $stateParams, $state, SweetAlert) {
        var vm = this;

        $http.get($rootScope.app.httpSource + 'api/Account/ConfirmEmail?userId=' + $stateParams.userId + '&code='+$stateParams.code)
            .then(function (response) {
                console.log(response);
                
                if (response.data === "ConfirmEmail") {
                    SweetAlert.swal($filter('translate')('general.ok'), $filter('translate')('register.mailconfirmed'), "success");
                    $state.go('page.login');
                }
                else {
                    alert('failed');
                }
            },
            function (response) { });

    }

    ConfirmEmailController.$inject = ['$rootScope', '$filter', '$http', '$stateParams', '$state', 'SweetAlert'];

})();