(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('addEditAthleteController', addEditAthleteController);
    /* @ngInject */
    function addEditAthleteController($rootScope, $http, $scope, $uibModalInstance, $filter, athlete, viewMode, SweetAlert) {
        $scope.PhotoPath = 'api/Upload/UploadFile?uploadFile=Attachment';
        debugger;
        $scope.isGuest = false;
        $scope.viewMode = viewMode;
        $scope.guestId = 0;
        //$scope.registrationTypeId = 0;
        $scope.aVouchers = []
        if (athlete == null) {
            $scope.athlete = {
                address: {},
                athleteVouchers: {},
                registrationTypeId: 5,
                isActive: true
            };
        }
        else {
            debugger;
            $scope.athlete = angular.copy(athlete);
            $scope.guestId = $scope.athlete.guestOf;
            $scope.registrationTypeId = $scope.athlete.registrationTypeId;
            if ($scope.athlete.registrationTypeId == 3)
                $scope.isGuest = true;
        }
        $scope.filterParams = { events: [] };

        $scope.params = {
            filterParams: ($scope.filterParams === undefined ? null : $scope.filterParams),
            page: 1,
            pageSize: 9
        };

        $http.post($rootScope.app.httpSource + 'api/Event/GetAllEvents', $scope.params)
            .then(function (resp) {
                $scope.events = resp.data.content;
                //setTimeout(function () { if (typeof addthis !== 'undefined') { addthis.layers.refresh(); } }, 500);
            }, function (response) { });

        $http.get($rootScope.app.httpSource + 'api/Athlete/GetAllVouchers')
            .then(function (response) {
                debugger;
                $scope.vouchers = response.data;
            }, function (response) { });

        $scope.ok = function () {
            if ($scope.athlete.id == undefined) {
                debugger
                $scope.athlete.password = "Abc@123";
                $scope.athlete.confirmPassword = "Abc@123";
              
                if ($scope.registrationTypeId != undefined) {

                    $scope.athlete.registrationTypeId = $scope.registrationTypeId;
                    $scope.athlete.guestOf = $scope.guestId;
                }

                $http.post($rootScope.app.httpSource + 'api/Account/Register', $scope.athlete)
                    .then(function (resp) {
                        SweetAlert.swal('User', $filter('translate')('athlete.athleteRegistered'), "success");
                        $uibModalInstance.close(resp.data);
                    }, function (response) {
                            if (response.data.modelState.invalidDataExceptionBase && response.data.modelState.invalidDataExceptionBase[0] !== undefined) {
                                SweetAlert.swal($filter('translate')('general.error'), $filter('translate')('register.emailTaken'), "error");
                            }
                            else {
                                SweetAlert.swal($filter('translate')('general.error'), $filter('translate')('register.failure'), "error");
                            }
                    });
            }
            else {
                $http.post($rootScope.app.httpSource + 'api/Account/UpdateProfile', $scope.athlete)
                    .then(function (response) {
                        SweetAlert.swal('Success', 'User Updated Successfully', "success");
                        $uibModalInstance.close($scope.athlete);
                    }, function (response) { });
            }
        };

        $scope.closeModal = function () {
            $uibModalInstance.dismiss('cancel');
        };

    }

    addEditAthleteController.$inject = ['$rootScope', '$http', '$scope', '$uibModalInstance', '$filter', 'athlete', 'viewMode', 'SweetAlert'];
})();