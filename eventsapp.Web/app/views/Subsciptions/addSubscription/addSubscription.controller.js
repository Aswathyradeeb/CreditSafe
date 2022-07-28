(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('addSubscriptionController', addSubscriptionController);

    addSubscriptionController.$inject = ['$rootScope', '$scope', 'UserProfile', '$filter', '$compile', '$http', '$uibModalInstance', '$state', 'SweetAlert', '$window'];
    function addSubscriptionController($rootScope, $scope, UserProfile, $filter, $compile, $http, $uibModalInstance, $state, SweetAlert, $window) {
        debugger;
        $scope.ok = function () {
            if ($scope.eventPackageId == undefined || $scope.eventPackageId == null) {
                SweetAlert.swal("Package Selection", "Please Select the Package", "error");
                return;
            }
            $http.post($rootScope.app.httpSource + 'api/User/CreateSubscription?eventPackageId=' + $scope.eventPackageId)
                .then(function (resp) {
                    SweetAlert.swal("Package Selection", "Please Added Succesfully, Kindly proceed to payment", "success");

                    $http.get($rootScope.app.httpSource + 'api/Payment/MakePayment?subscriptionId=' + resp.data.message)
                        .then(function (resp) {
                            window.location = resp.data;
                        });
                });
        };

        $scope.closeModal = function () {
            $uibModalInstance.dismiss('cancel');
        };
    }
})();
