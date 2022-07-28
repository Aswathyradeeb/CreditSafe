(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('searchCompanyController', searchCompanyController);

    searchCompanyController.$inject = ['$rootScope', '$scope', 'UserProfile', '$filter', '$compile', '$http', '$uibModalInstance', '$state', 'SweetAlert', '$window', 'eventId', 'eventCompanies', 'companyTypeId','eventPackages'];
    function searchCompanyController($rootScope, $scope, UserProfile, $filter, $compile, $http, $uibModalInstance, $state, SweetAlert, $window, eventId, eventCompanies, companyTypeId, eventPackages) {
        debugger;
        $scope.eventCompanies = angular.copy(eventCompanies);
        $scope.companyTypeId = companyTypeId;
        $scope.sponserPackages = angular.copy(eventPackages);
       
        $http.get($rootScope.app.httpSource + 'api/Company/GetCompanies')
            .then(function (resp) {
                debugger;
                $scope.companies = resp.data;
                $scope.eventCompanies.forEach( function (value, key1) {
                    $scope.companies.forEach(function (val, key) {
                        if (val.id == value.companyId) {
                            $scope.companies[key].packageId = $scope.eventCompanies[key1].packageId;
                            $scope.companies[key].standNumber = $scope.eventCompanies[key1].standNumber;
                            $scope.companies[key].standLocation = $scope.eventCompanies[key1].standLocation;
                        }
                    });
                });

            }, function (response) { });

        $scope.companyExists = function (checkboxValue) {
            if (checkboxValue.id) {
                var index = $scope.eventCompanies.indexOf($filter('filter')($scope.eventCompanies, { company: { id: checkboxValue.id } }, true)[0]);
                if (index != -1) {
                    return true;
                }
                return false;
            }
            else {
                var index = $scope.eventCompanies.indexOf($filter('filter')($scope.eventCompanies, { company: { id: checkboxValue } }, true)[0]);
                if (index != -1) {
                    return true;
                }
                return false;
            }
            return false;
        };
       
        $scope.companychecked = function (checkboxValue) {
            debugger;
            if (checkboxValue.nameEn) {
                var eventCompany = {
                    company: checkboxValue,
                    eventId: eventId,
                    companyTypeId: $scope.companyTypeId,
                    companyId: checkboxValue.id,
                    packageId: checkboxValue.packageId,
                    id: 0,
                    standNumber: checkboxValue.standNumber,
                    standLocation: checkboxValue.standLocation
                };
                $scope.eventCompanies.push(eventCompany);
            }
            else {
                var index = $scope.eventCompanies.indexOf($filter('filter')($scope.eventCompanies, { company: { id: checkboxValue } }, true)[0]);
                $scope.eventCompanies.splice(index, 1);
            }
        };

        $scope.ok = function () {
            debugger;
            $uibModalInstance.close($scope.eventCompanies);
        };

        $scope.closeModal = function () {
            $uibModalInstance.dismiss('cancel');
        };
    }
})();
