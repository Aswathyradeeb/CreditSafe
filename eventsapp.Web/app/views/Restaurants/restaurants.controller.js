(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('RestaurantsController', RestaurantsController);

    RestaurantsController.$inject = ['$rootScope', '$scope', 'UserProfile', '$filter', '$compile', '$http', '$uibModal', '$state', '$stateParams', 'SweetAlert', '$window'];
    function RestaurantsController($rootScope, $scope, UserProfile, $filter, $compile, $http, $uibModal, $state, $stateParams, SweetAlert, $window) {
        debugger;
        var vm = this;
         vm.isExist = false;
        vm.filterParams = { events: [] };
        vm.params = {
            filterParams: (vm.filterParams === undefined ? null : vm.filterParams),
            page: 1,
            pageSize: 12
        };

        vm.getAllRestaurants = function () {
            $http.post($rootScope.app.httpSource + 'api/Restaurant/GetAllRestaurants', vm.params)
                .then(function (resp) {
                    vm.Restaurants = resp.data.content;
                    if (vm.Restaurants.length==0)
                        vm.isExist = false;
                    else
                        vm.isExist = true;
                    vm.pager = resp.data;
                },
                    function (response) { });
        }
        vm.getAllRestaurants();
        vm.pager = {};

        vm.setPage = function (page) {
            vm.params = {
                filterParams: (vm.filterParams === undefined ? null : vm.filterParams),
                page: page,
                pageSize: vm.pager.pageSize
            };
            vm.getAllRestaurants();
        };

        vm.addRestaurant = function (size) {
            debugger;
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Restaurants/AddEditRestaurant/addEditRestaurant.html',
                controller: 'addEditRestaurantController',
                size: size,
                keyboard: false,
                backdrop: 'static',
                resolve: {
                    company: function () {
                        return {
                            address: {}
                        };
                    },
                    viewMode: false
                }
            });

            modalInstance.result.then(function (company) {
                vm.setPage(1);
                //vm.getAllRestaurants();
            }, function () {
                //state.text('Modal dismissed with Cancel status');
            });
        };

        $scope.view = function (size, company) {
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Restaurants/AddEditRestaurant/addEditRestaurant.html',
                controller: 'addEditRestaurantController',
                size: size,
                keyboard: false,
                backdrop: 'static',
                resolve: {
                    company: function () {
                        return company;
                    },
                    viewMode: true
                }
            });

            modalInstance.result.then(function (company) {
                vm.setPage(1);
            }, function () {
            });
        }

        $scope.edit = function (size, company) {
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Restaurants/AddEditRestaurant/addEditRestaurant.html',
                controller: 'addEditRestaurantController',
                size: size,
                keyboard: false,
                backdrop: 'static',
                resolve: {
                    company: function () {
                        return company;
                    },
                    viewMode: false
                }
            });

            modalInstance.result.then(function (company) {
                vm.setPage(1);
                //vm.getAllRestaurants();
            }, function () {
            });
        }

        vm.deleteSponser = function (companyId) {
            debugger;

            var index;
            var tempStore;
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
                        $http.get($rootScope.app.httpSource + 'api/Restaurant/DeleteRestaurant?id=' + companyId)
                            .then(function (resp) {

                                SweetAlert.swal('Delete', 'Deleted Successfully', "success");
                                window.location.reload();
                            },
                                function (response) { });

                    } else {

                        SweetAlert.swal(translate('general.restoreBtn'), translate('general.restoreMessage'), "success");
                        vm.testReportDt.dtInstance.rerender();
                    }
                });
        };
    }
})();
