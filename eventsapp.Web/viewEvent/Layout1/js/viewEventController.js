(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('viewEventController', viewEventController);

    function viewEventController($rootScope, $scope, $http, $stateParams, $state, UserProfile, DTOptionsBuilder, DTColumnBuilder, $filter, $compile, $uibModal, SweetAlert, $timeout, $sce) {
        var vm = this;
        vm.shown = true;
        var url = window.location.href;
        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        }
        var id = url.split('=').pop()
        vm.localizeString = $filter('localizeString');
        if (id != undefined && id != '') {
            $http.get($rootScope.app.httpSource + 'api/Event/GetEventById?id=' + id)
                .then(function (resp) {
                    vm.event = resp.data;
                    vm.event.disableRegister = false;
                    if (vm.event.eventAddresses != null) {
                        for (var index in vm.event.eventAddresses) {
                            if (vm.event.eventAddresses[index].address != null) {
                                vm.event.eventAddresses[index].locationURL = 'https://maps.google.com/maps?q=' + vm.event.eventAddresses[index].address.lat + ',' +
                                    vm.event.eventAddresses[index].address.lng + '&z=15&output=embed';
                            }
                        }
                    }
                    setTimeout(function () { loadMainScript(); }, 1000);
                    if (new Date(vm.event.endDate) < new Date() || vm.event.startDate > new Date()) {
                        vm.event.disableRegister = true;
                    }
                },
                    function (response) {
                    });
        }
    }

    viewEventController.$inject = ['$rootScope', '$scope', '$http', '$stateParams', '$state', 'UserProfile', 'DTOptionsBuilder', 'DTColumnBuilder', '$filter', '$compile', '$uibModal', 'SweetAlert', '$timeout', '$sce'];

})();