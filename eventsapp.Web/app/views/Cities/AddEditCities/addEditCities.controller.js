(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('addEditCitiesController', addEditCitiesController);
    /* @ngInject */
    function addEditCitiesController($rootScope, $http, $scope, $uibModalInstance, $filter, city, viewMode, SweetAlert) {
      
        debugger;
        $scope.viewMode = viewMode;
        if (city == null) {
            $scope.city = {
                id: null,
                isActive: true
            };
        }
        else {
            debugger;
            $scope.city = angular.copy(city);
        }
        $scope.filterParams = { countries: [] };

      

        $scope.params = {
            filterParams: ($scope.filterParams === undefined ? null : $scope.filterParams),
            page: 1,
            pageSize: 9
        };
        $http.post($rootScope.app.httpSource + 'api/City/GetCountries', $scope.params)
            .then(function (resp) {
                $scope.countries = resp.data.content;
                //setTimeout(function () { if (typeof addthis !== 'undefined') { addthis.layers.refresh(); } }, 500);
            }, function (response) { });
     
       
        $scope.ok = function () {
            if ($scope.city.id == undefined) {
              
                $http.post($rootScope.app.httpSource + 'api/City/CreateUpdateCity', $scope.city)
                    .then(function (resp) {
                        SweetAlert.swal('User', $filter('translate')('city.cityRegistered'), "success");
                        $uibModalInstance.close(resp.data);
                    }, function (response) {
                            
                        SweetAlert.swal('Error', 'Error in Saving! Try Again', "error");
                    });
            }
            else {
                $http.post($rootScope.app.httpSource + 'api/City/CreateUpdateCity', $scope.city)
                    .then(function (response) {
                        SweetAlert.swal('Success', 'City Updated Successfully', "success");
                        $uibModalInstance.close($scope.city);
                    }, function (response) { });
            }
        };

        $scope.closeModal = function () {
            $uibModalInstance.dismiss('cancel');
        };

    }

    addEditCitiesController.$inject = ['$rootScope', '$http', '$scope', '$uibModalInstance', '$filter', 'city', 'viewMode', 'SweetAlert'];
})();