(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('CitiesController', CitiesController);

    CitiesController.$inject = ['$rootScope', '$scope', '$filter', '$compile', '$http', '$uibModal', '$state', '$stateParams', 'SweetAlert', '$window'];
    function CitiesController($rootScope, $scope, $filter, $compile, $http, $uibModal, $state, $stateParams, SweetAlert, $window) {
        debugger;
        var vm = this;
        vm.userSearch = null;
        vm.filterParams = { events: [] };
        vm.pager = {};
        vm.params = {
            filterParams: (vm.filterParams === undefined ? null : vm.filterParams),
            page: 1,
            pageSize: 12
        };

     
        vm.searchCity = function () {
            vm.params = {
                filterParams: (vm.filterParams === undefined ? null : vm.filterParams),
                page: 1,
                pageSize: 12,
                Searchtext: vm.userSearch
            };

            $http.post($rootScope.app.httpSource + 'api/City/GetCities', vm.params)
                .then(function (resp) {
                    vm.cities = resp.data.content;
                    vm.pager = resp.data;
                },
                    function (response) { });
        }

        vm.getAllcities = function () {
            $http.post($rootScope.app.httpSource + 'api/City/GetCities', vm.params)
                .then(function (resp) {
                    vm.cities = resp.data.content;
                    vm.pager = resp.data;
                },
                    function (response) { });

        }
        vm.getAll = function () {
            vm.params = {
                filterParams: (vm.filterParams === undefined ? null : vm.filterParams),
                page: 1,
                pageSize: 12
            };
            $http.post($rootScope.app.httpSource + 'api/City/GetCities', vm.params)
                .then(function (resp) {
                    vm.cities = resp.data.content;
                    vm.pager = resp.data;
                },
                    function (response) { });

        }
     
        vm.setPage = function (page) {
            vm.params = {
                filterParams: (vm.filterParams === undefined ? null : vm.filterParams),
                page: page,
                pageSize: vm.pager.pageSize
            };
            vm.getAllcities();
        };
        vm.getAllcities();
        vm.addcity = function (size) {
            debugger;
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/cities/AddEditCities/addEditcities.html',
                controller: 'addEditCitiesController',
                size: size,
                keyboard: false,
                backdrop: 'static',
                resolve: {
                    city: function () {
                        return null;
                    },
                    viewMode: false
                }
            });

            modalInstance.result.then(function (company) {
                vm.setPage(1);
                //vm.getAllcitys();
            }, function () {
                //state.text('Modal dismissed with Cancel status');
            });
        };

        vm.deleteCity = function (cityId) {
            debugger;

            var translate = $filter('translate');

            SweetAlert.swal({
                title: 'Delete',
                text: 'Are you Sure You Want To Delete?',
                type: "warning",
                showCancelButton: true,
                showLoaderOnConfirm: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: translate('general.delete'),
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        //delete
                        $http.get($rootScope.app.httpSource + 'api/City/DeleteCity?id=' + cityId)
                            .then(function (resp) {

                                SweetAlert.swal('Delete', 'Deleted Successfully', "success");
                                window.location.reload();
                            },
                                function (response) { });

                    } else {

                    }
                });
        };
      
        $scope.edit = function (size, city) {
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/cities/AddEditCities/addEditcities.html',
                controller: 'addEditCitiesController',
                size: size,
                keyboard: false,
                backdrop: 'static',
                resolve: {
                    city: function () {
                        return city;
                    },
                    viewMode: true
                }
            });

            modalInstance.result.then(function (city) {
                vm.setPage(1);
                //vm.getAllcitys();
            }, function () {
            });
        }

     
    }
})();
