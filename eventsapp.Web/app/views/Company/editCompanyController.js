(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('editCompanyController', editCompanyController);
    /* @ngInject */
    function editCompanyController($rootScope, $http, $scope, $uibModalInstance, $filter, company, viewMode, SweetAlert) {
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

        $scope.ok = function () {
            if ($scope.company.id == undefined) {
                $http.post($rootScope.app.httpSource + 'api/Company/CreateCompany', $scope.company)
                    .then(function (resp) {
                        SweetAlert.swal($filter('translate')('company.company'), $filter('translate')('company.companyAdded'), "success");
                        $uibModalInstance.close(resp.data);
                    }, function (response) { });
            }
            else {
                $http.post($rootScope.app.httpSource + 'api/Company/UpdateCompany', $scope.company)
                    .then(function (response) {
                        SweetAlert.swal($filter('translate')('company.company'), $filter('translate')('company.companyEdited'), "success");
                        $uibModalInstance.close($scope.company);
                    }, function (response) { });
            }
        };

        $scope.closeModal = function () {
            $uibModalInstance.dismiss('cancel');
        };

    }

    editCompanyController.$inject = ['$rootScope', '$http', '$scope', '$uibModalInstance', '$filter', 'company', 'viewMode','SweetAlert'];
})();