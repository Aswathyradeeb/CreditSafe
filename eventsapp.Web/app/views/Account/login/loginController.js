/**=========================================================
 * Module: LoginController.js
 * Controller for the Chat APP 
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('LoginController', LoginController);
    /* @ngInject */
    function LoginController($rootScope, $scope, LoginService, UserProfile, $state, SweetAlert, $http, $filter, RegisterService, rememberMeService) {
        var vm = this;

        vm.responseData = "";
        vm.userName = "";
        vm.userEmail = "";
        vm.userPassword = "";
        vm.accessToken = "";
        vm.refreshToken = "";
        vm.invalidCredential = false;
        vm.emailConfirmed = true;
        $rootScope.userPreferredLanguages = [];
        $rootScope.englishPreferredLanguage = false;
        $rootScope.arabicPreferredLanguage = false;

        vm.login = function () {
            var userLogin = {
                grant_type: 'password',
                userName: vm.userEmail,
                password: vm.userPassword
            };
            vm.responseData = '';
            var loginResult = LoginService.login(userLogin);
            vm.isBusy = true;

            loginResult.then(
                function (resp) {
                    debugger;
                    console.log(resp.data.userId);
                    vm.userName = resp.data.userName;
                    vm.isBusy = false;
                    console.log(resp);
                    UserProfile.setProfile(resp.data.userName, resp.data.access_token, true, resp.data.firstName, resp.data.lastName, resp.data.emailConfirmed, resp.data.phoneNumber,
                        resp.data.phoneNumberConfirmed, resp.data.roleName, resp.data.userId, resp.data.companyId, resp.data.eventId, resp.data.registrationTypeId);

                    $http.get($rootScope.app.httpSource + 'api/User/UserPreferredLanguages?userId=' + parseInt(resp.data.userId))
                        .then(function (response) {
                            for (var i = 0; i < response.data.length; i++) {
                                $rootScope.userPreferredLanguages.push(response.data[i].language);
                                if (response.data[i].language.nameEn === 'English') {
                                    $rootScope.englishPreferredLanguage = true;
                                } else if (response.data[i].language.nameEn === 'Arabic') {
                                    $rootScope.arabicPreferredLanguage = true;
                                }
                            }
                        }, function (response) { });

                    if (resp.data.roleName === 1) {
                        $rootScope.AdminMode = true;
                        $rootScope.app.sidebar.isOffscreen = false;
                    }
                    if (resp.data.roleName === 2) {
                        $rootScope.AdminMode = false;
                        $rootScope.app.sidebar.isOffscreen = false;
                    }
                    $state.go('app.home', {}, { reload: true });
                    //else
                    //{
                    //    debugger;
                    //    RegisterService.setRegisteredUser("asd", resp.data.phoneNumber);
                    //    $state.go('page.confirmPhone', null, { reload: true });
                    //}

                }, function (response) {
                    vm.isBusy = false;
                    if (response.data.error == "invalid_grant") {
                        vm.emailConfirmed = true;
                        vm.invalidCredential = true;
                        vm.userLocked = false;
                        vm.pending_approval = false;
                    }
                    if (response.data.error == "email_not_confirmed") {
                        vm.emailConfirmed = false;
                        vm.invalidCredential = false;
                        vm.userLocked = false;
                        vm.pending_approval = false;
                    }
                    if (response.data.error == "user_locked") {
                        vm.userLocked = true;
                        vm.invalidCredential = false;
                        vm.emailConfirmed = true;
                        vm.pending_approval = false;
                    }
                    if (response.data.error == "pending_approval") {
                        vm.emailConfirmed = true;
                        vm.invalidCredential = false;
                        vm.userLocked = false;
                        vm.pending_approval = true;
                    }
                    vm.responseData = response.statusText + " : \r\n";
                    if (response.data.error) {
                        vm.responseData += response.data.error_description;
                    }
                });
        };

        vm.resendConfirmEmail = function () {
            vm.account = {};
            vm.account.email = vm.userEmail;
            vm.isBusy = true;

            $http.post($rootScope.app.httpSource + 'api/Account/ResendConfirmEmail', vm.account)
                .then(function (response) {
                    SweetAlert.swal($filter('translate')('general.ok'), $filter('translate')('register.successfulEmailConfirm'), "success");
                    vm.emailConfirmed = true;
                    vm.invalidCredential = false;
                    vm.isBusy = false;
                },
                    function (response) { // optional
                        vm.isBusy = false;
                    });
        };


        vm.rememberMe = function () {
            debugger;

            if (vm.remember) {
                rememberMeService('userEmail', vm.userEmail);
                rememberMeService('userPassword', vm.userPassword);
            } else {
                rememberMeService('userEmail', '');
                rememberMeService('userPassword', '');
            }
        };

        if (rememberMeService('userEmail') && rememberMeService('userPassword')) {
            vm.userEmail = rememberMeService('userEmail');
            vm.userPassword = rememberMeService('userPassword');
            vm.remember = true;
        }

    }
    LoginController.$inject = ['$rootScope', '$scope', 'LoginService', 'UserProfile', '$state', 'SweetAlert', '$http', '$filter', 'RegisterService', 'rememberMeService'];

})();