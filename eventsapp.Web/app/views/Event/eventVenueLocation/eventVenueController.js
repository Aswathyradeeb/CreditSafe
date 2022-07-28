(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('eventVenueController', eventVenueController);
    /* @ngInject */
    function eventVenueController($rootScope, $scope, $uibModalInstance, $filter, location) {

        if (location) {
            $scope.location = angular.copy(location);
        } else {
            $scope.location = {
                address: {}
            };
        }
        $scope.ok = function () {
            $uibModalInstance.close($scope.location);
        };

        $scope.closeModal = function () {
            $uibModalInstance.dismiss('cancel');
        };

    }

    eventVenueController.$inject = ['$rootScope', '$scope', '$uibModalInstance', '$filter', 'location'];
})();