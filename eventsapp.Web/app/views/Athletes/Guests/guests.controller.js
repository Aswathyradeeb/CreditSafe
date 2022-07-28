(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('GuestsController', GuestsController);

    GuestsController.$inject = ['$rootScope', '$scope', 'UserProfile', '$filter', '$compile', '$http', '$uibModal', '$state', '$stateParams', 'SweetAlert', '$window'];
    function GuestsController($rootScope, $scope, UserProfile, $filter, $compile, $http, $uibModal, $state, $stateParams, SweetAlert, $window) {
        debugger;
        var vm = this;
        var athleteId = $stateParams.id;
        vm.filterParams = { events: [] };
       
        vm.getGuestsOfAthlete = function () {
            vm.params = {
                filterParams: (vm.filterParams === undefined ? null : vm.filterParams),
                page: 1,
                pageSize: 12,
                searchText: $stateParams.id
            };
            $http.post($rootScope.app.httpSource + 'api/Athlete/GetGuestsOfAthlete', vm.params)
                .then(function (resp) {
                    vm.Athletes = resp.data.content;
                    vm.pager = resp.data;
                },
                    function (response) { });

        }
      
        vm.pager = {};
        vm.getGuestsOfAthlete();

        vm.setPage = function (page) {
            vm.params = {
                filterParams: (vm.filterParams === undefined ? null : vm.filterParams),
                page: page,
                pageSize: vm.pager.pageSize,
                searchText: $stateParams.id
            };
            vm.getGuestsOfAthlete();
        };
        vm.searchPST = function () {
            debugger;
            vm.filterParams = {
                events: [],
                searchText: vm.userSearch
            };
            vm.params = {
                filterParams: (vm.filterParams === undefined ? null : vm.filterParams),
                page: 1,
                pageSize:12,
                searchText: $stateParams.id,
                sortBy: vm.userSearch
            };
            $http.post($rootScope.app.httpSource + 'api/Athlete/GetGuestsOfAthlete', vm.params)
                .then(function (resp) {
                    vm.Athletes = resp.data.content;
                    vm.pager = resp.data;
                },
                    function (response) { });

        }
        vm.addGuest = function (size) {
            debugger;
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Athletes/AddEditAthlete/addEditAthlete.html',
                controller: 'addEditAthleteController',
                size: size,
                keyboard: false,
                backdrop: 'static',
                resolve: {
                    athlete: function () {
                        return {
                            address: {},
                            isActive: true,
                            guestOf: $stateParams.id,
                            registrationTypeId: 3
                        };
                    },
                    viewMode: false
                }
            });

            modalInstance.result.then(function (company) {
                vm.setPage(1);
                //vm.params = {
                //    filterParams: (vm.filterParams === undefined ? null : vm.filterParams),
                //    page: 1,
                //    pageSize: 12,
                //    searchText: $stateParams.id
                //};
                //vm.getGuestsOfAthlete();
            }, function () {
                //state.text('Modal dismissed with Cancel status');
            });
        };

        $scope.view = function (size, athlete) {
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Athletes/AddEditAthlete/addEditAthlete.html',
                controller: 'addEditAthleteController',
                size: size,
                keyboard: false,
                backdrop: 'static',
                resolve: {
                    athlete: function () {
                        return athlete;
                    },
                    viewMode: true
                }
            });

            modalInstance.result.then(function (athlete) {
                vm.setPage(1);
            }, function () {
            });
        }

        $scope.edit = function (size, athlete) {
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Athletes/AddEditAthlete/addEditAthlete.html',
                controller: 'addEditAthleteController',
                size: size,
                keyboard: false,
                backdrop: 'static',
                resolve: {
                    athlete: function () {
                        return athlete;
                    },
                    viewMode: false
                }
            });

            modalInstance.result.then(function (athlete) {
                vm.setPage(1);
                //vm.params = {
                //    filterParams: (vm.filterParams === undefined ? null : vm.filterParams),
                //    page: 1,
                //    pageSize: 12,
                //    searchText: $stateParams.id
                //};
                //vm.getGuestsOfAthlete();
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
                        $http.get($rootScope.app.httpSource + 'api/Company/DeleteCompany?id=' + companyId)
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
