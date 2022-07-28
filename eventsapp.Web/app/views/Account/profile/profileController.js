/**=========================================================
 * Module: LoginController.js
 * Controller for the Chat APP 
 =========================================================*/


(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('ProfileController', ProfileController);

    /* @ngInject */
    function ProfileController($rootScope, $scope, $http, $stateParams, $state, UserProfile, DTOptionsBuilder, DTColumnBuilder, $compile, $filter, $uibModal,
        editableOptions, editableThemes, SweetAlert, $timeout, FileUploader) {
        var vm = this;
        //vm.user = UserProfile.getProfile();
        //vm.id = vm.user.userId;
        vm.isBusy = false;
        vm.preferredLanguagesList = [];
        vm.showRequiredLanguageError = false;

        $http.get($rootScope.app.httpSource + 'api/Lookup/GetLanguages')
            .then(function (response) {
                vm.languages = response.data;
            });

        $http.get($rootScope.app.httpSource + 'api/User/Get')
            .then(function (response) {
                vm.user = response.data;

                vm.preferredLanguagesList = [];
                for (var i = 0; i < vm.user.preferredLanguages.length; i++) {
                    vm.preferredLanguagesList.push(vm.user.preferredLanguages[i].language);
                }
            }, function (response) { });

        vm.addCompany = function () {
            if (vm.companyCode) {
                $http.post($rootScope.app.httpSource + 'api/User/AddCompany?companyCode=' + vm.companyCode)
                    .then(function (response) {
                        SweetAlert.swal($filter('translate')('general.congrats'), $filter('translate')('general.companyCodeAdded'), "success");
                        $state.go('app.profile');
                    },
                        function (response) { // optional
                            if (response.data.exceptionMessage == 'InvalidCompanyCode') {
                                SweetAlert.swal($filter('translate')('general.error'), $filter('translate')('general.invalidCompanyCode'), "error");
                            }
                            if (response.data.exceptionMessage == 'CompanyAlreadyExists') {
                                SweetAlert.swal($filter('translate')('general.error'), $filter('translate')('general.companyAlreadyExists'), "error");
                            }
                            vm.isBusy = false;
                        });
            }
        };

        vm.isBusy = false;
        vm.updateProfile = function () {
            vm.isBusy = true;
            if (!vm.preferredLanguagesList.length) {
                vm.showRequiredLanguageError = true;
                return;
            } else {
                vm.showRequiredLanguageError = false;
                vm.user.preferredLanguages = [];
                for (var i = 0; i < vm.preferredLanguagesList.length; i++) {
                    var preferredLanguage = {
                        userId: parseInt(vm.user.userId),
                        languageId: vm.preferredLanguagesList[i].id
                    };
                    vm.user.preferredLanguages.push(preferredLanguage);
                }
                $http.post($rootScope.app.httpSource + 'api/User/UpdateUserProfile', vm.user)
                    .then(function (response) {
                        SweetAlert.swal($filter('translate')('general.congrats'), $filter('translate')('Update successfully'), "success");
                        vm.isBusy = false;
                        window.location.reload(true);
                        //$state.go('app.dashboard');
                    },
                        function (response) { // optional
                            vm.isBusy = false;
                            if (response.data.modelState.invalidDataExceptionBase[0] !== undefined) {
                                vm.emailAlreadyTaken = true;
                            }
                            vcRecaptchaService.reload(vm.widgetId);
                            
                        });
            }
        };
        vm.validMobileNumber = function () {
            vm.validMobile = true;
        };

        vm.invalidMobileNumber = function () {
            vm.validMobile = false;
        };

        vm.changePass = function () {
            if (vm.openPasswordModelPopup) { return; }
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Account/changePassword/changePassword.html',
                controller: 'changePassController',
                size: 'md',
                backdrop: 'static',
                keyboard: false,
                resolve: {
                    user: function () {
                        return vm.user;
                    }
                }
            });

            vm.openPasswordModelPopup = true;

            modalInstance.result.then(function (user) {
                vm.openPasswordModelPopup = false;
            }, function () {
                //state.text('Modal dismissed with Cancel status');
                vm.openPasswordModelPopup = false;
            });
        };
    }
    ProfileController.$inject = ['$rootScope', '$scope', '$http', '$stateParams', '$state', 'UserProfile', 'DTOptionsBuilder', 'DTColumnBuilder', '$compile', '$filter', '$uibModal',
        'editableOptions', 'editableThemes', 'SweetAlert', '$timeout', 'FileUploader'];

})();














