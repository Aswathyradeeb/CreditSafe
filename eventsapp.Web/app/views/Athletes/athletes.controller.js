(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('AthletesController', AthletesController);

    AthletesController.$inject = ['$rootScope', '$scope', 'UserProfile', '$filter', '$compile', '$http', '$uibModal', '$state', '$stateParams', 'SweetAlert', '$window'];
    function AthletesController($rootScope, $scope, UserProfile, $filter, $compile, $http, $uibModal, $state, $stateParams, SweetAlert, $window) {
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

        vm.getAllAthletes = function () {
            $http.post($rootScope.app.httpSource + 'api/Athlete/GetAthletes', vm.params)
                .then(function (resp) {
                    vm.Athletes = resp.data.content;
                    vm.pager = resp.data;
                },
                    function (response) { });

        }

        vm.searchAthlete = function () {
            vm.params = {
                filterParams: (vm.filterParams === undefined ? null : vm.filterParams),
                page: 1,
                pageSize: 12,
                Searchtext: vm.userSearch
            };
            vm.getAllAthletes();

        }

        vm.getAll = function () {
            vm.userSearch = null;
            vm.params = {
                filterParams: (vm.filterParams === undefined ? null : vm.filterParams),
                page: 1,
                pageSize: 12
            };
            vm.getAllAthletes();
        }

        vm.getAllAthletes();

        vm.setPage = function (page) {
            vm.params = {
                filterParams: (vm.filterParams === undefined ? null : vm.filterParams),
                page: page,
                pageSize: vm.pager.pageSize
            };
            vm.getAllAthletes();
        };

        vm.addathlete = function (size) {
            debugger;
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Athletes/AddEditAthlete/addEditAthlete.html',
                controller: 'addEditAthleteController',
                size: size,
                keyboard: false,
                backdrop: 'static',
                resolve: {
                    athlete: function () {
                        return null;
                    },
                    viewMode: false
                }
            });

            modalInstance.result.then(function (company) {
                vm.setPage(1);
                //vm.getAllAthletes();
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
                //vm.getAllAthletes();
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
                //vm.getAllAthletes();
            }, function () {
            });
        }

        $scope.manageGuest = function (athlete) {
            $state.go('app.guests', { "id": athlete.id }, { reload: true });
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

        vm.exportAllQRImages = function () {
            $http.get($rootScope.app.httpSource + 'api/Upload/ZipFiles')
                .then(function (resp) {
                    debugger;
                    window.open(resp.data.httpPath, '_blank');
                },
                    function (response) {
                        debugger;
                    });
        };
    }
})();
