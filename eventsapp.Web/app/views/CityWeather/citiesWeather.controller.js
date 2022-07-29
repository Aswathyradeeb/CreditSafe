(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('CitiesWeatherController', CitiesWeatherController);

    CitiesWeatherController.$inject = ['$rootScope', '$scope', '$filter', '$compile', '$http', '$uibModal', '$state', '$stateParams', 'SweetAlert', '$window'];
    function CitiesWeatherController($rootScope, $scope, $filter, $compile, $http, $uibModal, $state, $stateParams, SweetAlert, $window) {
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

            $http.post($rootScope.app.httpSource + 'api/City/GetCityWeather', vm.params)
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
       
     
    }
})();
