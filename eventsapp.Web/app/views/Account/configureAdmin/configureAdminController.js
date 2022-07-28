(function () {
    'use strict';

    angular.module('eventsapp')
        .controller('configureAdminController', configureAdminController);

    function configureAdminController($rootScope, $http, $scope, $uibModalInstance, $filter, $timeout, FileUploader, SweetAlert, user) {
        debugger;
        $scope.userDetails = angular.copy(user);
        $scope.userDetailsObj = {};
        $scope.userDetailsObj.userId = user.id;
        $scope.userDetailsObj.email = user.email;
        $scope.userDetailsObj.noOfEvents = user.noOfEvents;
        $scope.noOfEvents = [1, 5, 10, 15];
        $scope.userDetailsObj.isApproved = user.isApproved;
        $scope.ok = function () {
            $http.post($rootScope.app.httpSource + 'api/Account/ConfigureAdmin', $scope.userDetailsObj)
                .then(function (response) { 
                    SweetAlert.swal("Succesful", "Admin Configured Succesfully", "success");
                    $uibModalInstance.dismiss('cancel');
                },
                function (response) { // optional 
                });
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

        configureAdminController.$inject = ['$rootScope', '$http', '$scope', '$uibModalInstance', '$filter', '$timeout', 'FileUploader', 'SweetAlert', 'user'];
    }
})();