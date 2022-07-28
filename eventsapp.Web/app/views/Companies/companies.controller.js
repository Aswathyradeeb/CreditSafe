(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('companiesController', companiesController);

    companiesController.$inject = ['$rootScope', '$scope', 'UserProfile', '$filter', '$compile', '$http', '$uibModal', '$state', '$stateParams', 'SweetAlert', '$window'];
    function companiesController($rootScope, $scope, UserProfile, $filter, $compile, $http, $uibModal, $state, $stateParams, SweetAlert, $window) {
        debugger;
        var vm = this;

        vm.filterParams = { events: [] };
        vm.params = {
            filterParams: (vm.filterParams === undefined ? null : vm.filterParams),
            page: 1,
            pageSize: 12
        };

        $http.post($rootScope.app.httpSource + 'api/Company/GetAllCompanies', vm.params)
            .then(function (resp) {
                vm.companies = resp.data.content;
                vm.pager = resp.data;
            },
                function (response) { });

        vm.pager = {};
        vm.setPage = function (page) {
            vm.params = {
                filterParams: (vm.filterParams === undefined ? null : vm.filterParams),
                page: page,
                pageSize: vm.pager.pageSize
            };
            $http.post($rootScope.app.httpSource + 'api/Company/GetAllCompanies', vm.params)
                .then(function (resp) {
                    vm.companies = resp.data.content;
                    vm.pager = resp.data;
                },
                    function (response) { });
        };

        vm.addCompany = function (size) {
            debugger;
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Company/company.html',
                controller: 'editCompanyController',
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
            }, function () {
                //state.text('Modal dismissed with Cancel status');
            });
        };
        $scope.view = function (size, company) {
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Company/company.html',
                controller: 'editCompanyController',
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
                templateUrl: 'app/views/Company/company.html',
                controller: 'editCompanyController',
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
