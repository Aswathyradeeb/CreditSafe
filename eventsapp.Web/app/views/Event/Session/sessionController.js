(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('sessionController', sessionController);
    /* @ngInject */
    function sessionController($rootScope, $scope, $uibModalInstance, $filter, sessionObj, $http) {

        if (sessionObj != undefined) {
            $scope.session = angular.copy(sessionObj);
        } else {
            $scope.session = {};
        }

        $scope.ok = function () {
            $uibModalInstance.close($scope.session);
        };

        $scope.closeModal = function () {
            $uibModalInstance.dismiss('cancel');
        };

    }

    sessionController.$inject = ['$rootScope', '$scope', '$uibModalInstance', '$filter', 'sessionObj', '$http'];
})();