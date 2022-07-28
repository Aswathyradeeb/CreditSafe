(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('addEditAthleteController', addEditAthleteController);
    /* @ngInject */
    function addEditAthleteController($rootScope, $http, $scope, $uibModalInstance, $filter, athlete, viewMode, SweetAlert) {
        $scope.PhotoPath = 'api/Upload/UploadFile?uploadFile=Attachment';
        $scope.viewMode = viewMode;
        $scope.aVouchers = []
        if (athlete == null) {
           
            $scope.athlete = {
                address: {},
               athleteVouchers: {} 
            };
        }
        else {
            $scope.athlete = angular.copy(athlete);
           
            if ($scope.athlete.athleteVouchers) {
                $scope.athlete.aVouchers = []
                for (var i = 0; i < $scope.athlete.athleteVouchers.length; i++) {
                    var obj = {};
                    obj.nameAr = $scope.athlete.athleteVouchers[i].voucher.nameAr;
                    obj.nameEn = $scope.athlete.athleteVouchers[i].voucher.nameEn;
                    obj.id = $scope.athlete.athleteVouchers[i].voucherId;
                    $scope.athlete.aVouchers.push(obj)
                }
            }
           
            else
                $scope.athlete = {
                    athleteVouchers: [] ,
                    aVouchers:[]
                };
            
        }
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
                $scope.athleteVouchers = {};
                for (var i = 0; i < $scope.athlete.aVouchers.length; i++) {
                    var obj = {};
                    obj.id = i;
                    obj.userId = i;
                    obj.voucherId = $scope.athlete.aVouchers[i].id;
                    $scope.athlete.athleteVouchers.push(obj);
                }
                $scope.athlete.registrationTypeId = 1;
                    $http.post($rootScope.app.httpSource + 'api/Account/Register', $scope.athlete)
                    .then(function (resp) {
                        SweetAlert.swal($filter('translate')('company.company'), $filter('translate')('athlete.athleteRegistered'), "success");
                        $uibModalInstance.close(resp.data);
                    }, function (response) { });
            }
            else {
                $http.post($rootScope.app.httpSource + 'api/Account/UpdateProfile', $scope.athlete)
                    .then(function (response) {
                        SweetAlert.swal($filter('translate')('company.company'), $filter('translate')('company.athleteUpdated'), "success");
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