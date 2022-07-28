(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('shareVoucherController', shareVoucherController);
    /* @ngInject */
    function shareVoucherController($rootScope, $http, $scope, $uibModalInstance, $filter, viewMode, voucherId, SweetAlert) {
        $scope.PhotoPath = 'api/Upload/UploadFile?uploadFile=Attachment';
        $scope.viewMode = viewMode;
        $scope.voucherId = voucherId;
        $scope.aVouchers = []
       
        $scope.athlete = {
                   user: [],
                    athleteVouchers: [] ,
                    aVouchers:[]
                };
            
        
        $http.get($rootScope.app.httpSource + 'api/Athlete/GetAllAthletesGuest')
            .then(function (response) {
                debugger;
                $scope.guests = response.data;
            }, function (response) { });

        $http.get($rootScope.app.httpSource + 'api/Athlete/GetAllVouchers')
            .then(function (response) {
                debugger;
                $scope.vouchers = response.data;
            }, function (response) { });
        $scope.ok = function () {
            if ($scope.athlete.user.id == undefined || $scope.athlete.user == []) {
                debugger
                $scope.athlete.password = "Abc@123";
                $scope.athlete.confirmPassword = "Abc@123";
                    var obj = {};
                    obj.id = 0;
                    obj.userId = 0;
                    obj.voucherId = $scope.voucherId;
                    $scope.athlete.athleteVouchers.push(obj);
                
                $scope.athlete.registrationTypeId = 2;
                    $http.post($rootScope.app.httpSource + 'api/Account/Register', $scope.athlete)
                    .then(function (resp) {
                        SweetAlert.swal($filter('translate')('athlete.hifive'), $filter('translate')('athlete.voucherShared'), "success");
                        $uibModalInstance.close(resp.data);
                    }, function (response) { });
            }
            else {
                $http.get($rootScope.app.httpSource + 'api/Athlete/ShareGuestVoucher?guestId=' + $scope.athlete.user.id + '&voucherId=' + $scope.voucherId)

                    .then(function (response) {
                        debugger;
                        if (response.data.result == 1) {
                            SweetAlert.swal($filter('translate')('athlete.hifive'), $filter('translate')('athlete.voucherShared'), "success");
                            $uibModalInstance.close($scope.athlete);
                        }
                        else {
                            SweetAlert.swal($filter('translate')('athlete.hifive'), $filter('translate')('athlete.alreadyShared'), "error");
                            $uibModalInstance.close($scope.athlete);
                        }
                    }, function (response) { });
            }
        };

        $scope.closeModal = function () {
            $uibModalInstance.dismiss('cancel');
        };
        $scope.addGuest = function () {
            $scope.viewMode = true;
        };
    }

    shareVoucherController.$inject = ['$rootScope', '$http', '$scope', '$uibModalInstance', '$filter', 'viewMode', 'voucherId', 'SweetAlert'];
})();