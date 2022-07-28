(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('addEditRestaurantController', addEditRestaurantController);
    /* @ngInject */
    function addEditRestaurantController($rootScope, $http, $scope, $uibModalInstance, $filter, company, viewMode, SweetAlert) {
        $scope.PhotoPath = 'api/Upload/UploadFile?uploadFile=Attachment';
        $scope.viewMode = viewMode;
        if (company == null) {
            $scope.company = {
                address: {}
            };
        }
        else {
            $scope.company = angular.copy(company);
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
                setTimeout(function () { if (typeof addthis !== 'undefined') { addthis.layers.refresh(); } }, 500);
            }, function (response) { });

        $scope.ok = function () {
            if ($scope.company.id == undefined) {
                $scope.company.registrationTypeId = 2;
                $http.post($rootScope.app.httpSource + 'api/Restaurant/CreateRestaurant', $scope.company)
                    .then(function (resp) {
                        SweetAlert.swal('Restaurant', 'Restaurant Added Successfully', "success");
                        $uibModalInstance.close(resp.data);
                    }, function (response) { });
            }
            else {
                $http.post($rootScope.app.httpSource + 'api/Restaurant/UpdateRestaurant', $scope.company)
                    .then(function (response) {
                        SweetAlert.swal('Restaurant', 'Restaurant Updated Successfully', "success");
                        $uibModalInstance.close($scope.company);
                    }, function (response) { });
            }
        };

        $scope.closeModal = function () {
            $uibModalInstance.dismiss('cancel');
        };

    }

    addEditRestaurantController.$inject = ['$rootScope', '$http', '$scope', '$uibModalInstance', '$filter', 'company', 'viewMode', 'SweetAlert'];
})();