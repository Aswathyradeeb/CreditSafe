/**=========================================================
 * Module: LoginController.js
 * Controller for the Chat APP 
 =========================================================*/


(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('VerifyVoucher', VerifyVoucher);

    /* @ngInject */
    VerifyVoucher.$inject = ['$rootScope', '$scope', '$http', '$stateParams', '$state', 'UserProfile', '$filter', 'SweetAlert', 'moment'];

    function VerifyVoucher($rootScope, $scope, $http, $stateParams, $state, UserProfile, $filter, SweetAlert, moment) {
        var vm = this;
        vm.isvalid = false;
        //$http.get($rootScope.app.httpSource + 'api/Athlete/GetAllVouchers')
        //    .then(function (response) {
        //        debugger;
        //        vm.vouchers = response.data;
        //    }, function (response) { });


        $http.get($rootScope.app.httpSource + 'api/User/VerifyVoucher?userId=' + $stateParams.id + '&eventId=' + $stateParams.eventId)
            .then(function (response) {
                if (response.data == null) {
                    vm.isvalid = false;
                    SweetAlert.swal($filter('translate')('general.error'), $filter('translate')('User not found, Cannot Verify User'), "error");

                }
                else if (response.data.isActive == false) {
                    vm.isvalid = false;
                    SweetAlert.swal($filter('translate')('general.error'), $filter('translate')('User not Active, Cannot Verify User'), "error");

                }
                else if (response.data.usedDrinkVouchers == response.data.assignedDrinkVouchers && response.data.assignedFoodVouchers == response.data.usedFoodVouchers) {
                    vm.isvalid = false;
                    SweetAlert.swal($filter('translate')('general.error'), $filter('translate')('Limit Reached, Cannot use voucher'), "error");

                }
                else {
                    debugger;
                    vm.user = response.data;
                    vm.today = moment().format('YYYY-MM-DD');
                    vm.startDate = moment.utc(vm.user.event.startDate).format('YYYY-MM-DD');
                    vm.endDate = moment.utc(vm.user.event.endDate).format('YYYY-MM-DD');
                    vm.isvalid = true;
                    vm.beverageCount = vm.user.usedDrinkVouchers;
                    vm.foodCount = vm.user.usedFoodVouchers;
                    if (vm.user.event.deletedOn) {
                        SweetAlert.swal($filter('translate')('general.error'), $filter('translate')('Event has been deleted. Kindly contact Admin!'), "error");
                        vm.isvalid = false;
                    }
                    else if ((moment(vm.today).isBefore(vm.startDate) || moment(vm.today).isAfter(vm.endDate))) {
                        SweetAlert.swal($filter('translate')('general.error'), $filter('translate')('Event is not valid for today. Kindly contact Admin!'), "error");
                        vm.isvalid = false;
                    }
                    //else if ((moment(vm.today).isBefore(vm.startDate) || !moment(vm.today).isSame(vm.startDate)) ||
                    //    (moment(vm.today).isAfter(vm.endDate) || !moment(vm.today).isSame(vm.endDate))) {
                    //    SweetAlert.swal($filter('translate')('general.error'), $filter('translate')('Event is not valid for today. Kindly contact Admin!'), "error");
                    //    vm.isvalid = false;
                    //}
                }


            }, function (response) { });

        vm.numberOnly = function () {
            var count = vm.user.assignedDrinkVouchers - (vm.user.usedDrinkVouchers + vm.beverageCount);
            if (count < 0)
                vm.user.usedDrinkVouchers = 0;
            count = vm.user.assignedFoodVouchers - (vm.user.usedFoodVouchers + vm.foodCount);
            if (count < 0)
                vm.user.usedFoodVouchers = 0;
        };

        vm.updateProfile = function () {
            vm.isBusy = true;
            var count = vm.user.assignedDrinkVouchers - (vm.user.usedDrinkVouchers + vm.beverageCount);
            if (count < 0) {
                SweetAlert.swal($filter('translate')('general.error'), $filter('translate')('Limit exceeded,cannot update'), "error");
                return;
            }
            count = vm.user.assignedFoodVouchers - (vm.user.usedFoodVouchers + vm.foodCount);
            if (count < 0) {
                SweetAlert.swal($filter('translate')('general.error'), $filter('translate')('Limit exceeded,cannot update'), "error");
                return;
            }
            $http.post($rootScope.app.httpSource + 'api/User/UpdateUserProfile', vm.user)
                .then(function (response) {
                    SweetAlert.swal($filter('translate')('general.congrats'), $filter('translate')('Updated succesfully'), "success");
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

        };

    }


})();














