(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('companyController', companyController);
    /* @ngInject */
    function companyController($rootScope, $http, $scope, $uibModalInstance, $filter, eventId, companyTypeId, eventCompany, sponserPackages, SweetAlert) {
        debugger;
        $scope.sponserPackages = sponserPackages;

        $http.get($rootScope.app.httpSource + 'api/SponserTypes/Get')
            .then(function (response) {
                $scope.SponserTypes = response.data;
            }, function (response) { });
         

        $scope.companyTypeId = companyTypeId;
        $scope.PhotoPath = 'api/Upload/UploadFile?uploadFile=Attachment';

        if (eventCompany == null) {
            $scope.eventCompany = {
                company: {
                    address: {}
                },
                eventId: eventId,
                companyTypeId: companyTypeId,
            };
        }
        else {
            $scope.eventCompany = angular.copy(eventCompany);
        }

        $scope.ok = function () {
            debugger;
            if (!$scope.eventCompany.id) {
                $http.post($rootScope.app.httpSource + 'api/EventCompany/CreateEventCompany', $scope.eventCompany)
                    .then(function (response) {
                        $scope.eventCompany = response.data;
                        SweetAlert.swal($filter('translate')('company.company'), $filter('translate')('company.companyAdded'), "success");
                        $uibModalInstance.close($scope.eventCompany.result);
                    }, function (response) {
                            if (response.data.innerException.innerException.innerException.exceptionMessage == "Company Already Exists") {
                                SweetAlert.swal($filter('translate')('company.company'), $filter('translate')('company.companyExistsEmail'), "error");
                            }
                            if (response.data.message) {
                                SweetAlert.swal($filter('translate')('general.error'), $filter('translate')('general.errorException'), "error");
                            }
                        });
            } else {
                $http.post($rootScope.app.httpSource + 'api/EventCompany/UpdateEventCompany', $scope.eventCompany)
                    .then(function (response) {
                        $scope.eventCompany = response.data;
                        SweetAlert.swal($filter('translate')('company.company'), $filter('translate')('company.companyEdited'), "success");
                        $uibModalInstance.close($scope.eventCompany);
                    }, function (response) {
                            if (response.data.innerException.innerException.innerException.exceptionMessage == "Company Already Exists") {
                                SweetAlert.swal($filter('translate')('company.company'), $filter('translate')('company.companyExistsEmail'), "error");
                            }
                            if (response.data.message) {
                                SweetAlert.swal($filter('translate')('general.error'), $filter('translate')('general.errorException'), "error");
                            }
                        });
            }
        };

        $scope.closeModal = function () {
            $uibModalInstance.dismiss('cancel');
        };

    }

    companyController.$inject = ['$rootScope', '$http', '$scope', '$uibModalInstance', '$filter', 'eventId', 'companyTypeId', 'eventCompany', 'sponserPackages','SweetAlert'];
})();