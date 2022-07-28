(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('VouchersController', VouchersController);

    VouchersController.$inject = ['$rootScope', '$scope', 'UserProfile', '$filter', '$compile', '$http', '$uibModal', '$state', '$stateParams', 'SweetAlert', '$window'];
    function VouchersController($rootScope, $scope, UserProfile, $filter, $compile, $http, $uibModal, $state, $stateParams, SweetAlert, $window) {
        debugger;
        var vm = this;

        vm.filterParams = { events: [] };
        vm.params = {
            filterParams: (vm.filterParams === undefined ? null : vm.filterParams),
            page: 1,
            pageSize: 12
        };
        vm.getAthleteVoucher = function () {
            debugger;
            $http.post($rootScope.app.httpSource + 'api/Athlete/GetAthleteVoucher', vm.params)
                .then(function (resp) {
                    vm.AthletesVocher = resp.data.content;
                    vm.pager = resp.data;
                },
                    function (response) { });

        }
      
        vm.pager = {};
        vm.getAthleteVoucher();
        vm.setPage = function (page) {
            vm.params = {
                filterParams: (vm.filterParams === undefined ? null : vm.filterParams),
                page: page,
                pageSize: vm.pager.pageSize
            };
          
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


        $scope.shareVoucher = function (size, voucher) {
            debugger;
            $http.get($rootScope.app.httpSource + 'api/Athlete/GetVoucherShareCount?voucherId=' + voucher.id)
                .then(function (resp) {
                    debugger;
                    var result = resp.data;
                    if (result < 2) {

                        var modalInstance = $uibModal.open({
                            templateUrl: 'app/views/Vouchers/shareVouchers/shareVoucher.html',
                            controller: 'shareVoucherController',
                            size: size,
                            keyboard: false,
                            backdrop: 'static',
                            resolve: {

                                viewMode: false,
                                voucherId: voucher.id
                            }
                        });

                        modalInstance.result.then(function (athlete) {
                            vm.setPage(1);
                            vm.getAthleteVoucher();
                        }, function () {
                        });
                    }

                    else {
                        SweetAlert.swal($filter('translate')('athlete.voucher'), $filter('translate')('athlete.limitReached'), "error");
                        $uibModalInstance.close($scope.athlete);
                    }

                },
                    function (response) { });

          
        }



    }
})();
