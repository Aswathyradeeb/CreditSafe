/**=========================================================
 * Module: LoginController.js
 * Controller for the Chat APP 
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('SetPasswordController', SetPasswordController);
    /* @ngInject */
    function SetPasswordController($rootScope, $scope, $http, $stateParams, $state, SweetAlert, $filter) {
        var vm = this;
        debugger;
        vm.passwordObj = { "newPassword": '', "oldPassword": '', "userId": 0 };
        vm.passwordObj.userId = $stateParams.userId;
        //vm.passwordObj.newPassword = $stateParams.code;

        vm.setPassword = function () {
            $http.post($rootScope.app.httpSource + 'api/Account/SetUserPassword', vm.passwordObj)
                .then(function (response) { 
                    if (response.data == "PasswordSet") {
                        SweetAlert.swal($filter('translate')('recover.reset'), $filter('translate')('recover.resetComplete'), "success");
                        $state.go('page.login');
                    }
                    else
                        alert('failed');
                },
                    function (response) { // optional
                        alert('failed');
                    });
        }
    }

    SetPasswordController.$inject = ['$rootScope', '$scope', '$http', '$stateParams', '$state', 'SweetAlert', '$filter'];

})();