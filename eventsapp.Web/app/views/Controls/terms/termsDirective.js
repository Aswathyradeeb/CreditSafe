/**=========================================================
 * Module: profileAddress
 * Reuse cases of address in user profile page
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('eventsapp')
        .directive('terms', terms)


    terms.$inject = ['$rootScope', '$http', '$filter','$uibModal']
    function terms($rootScope, $http, $filter, $uibModal) {
        return {
            restrict: 'E',
            scope: {
                termsControl: "=ngModel"
            },
            templateUrl: '/app/views/Controls/terms/termsDirectiveTemplate.html',
            link: link
        };

        function link(scope, element, attrs) {
            var unwatch = scope.$watch('termsControl', function (newVal, oldVal) {
                if (newVal) {
                    init();
                    unwatch();
                }
                else {
                    // gridTable();
                }
            });
            function init() {
                scope.termsControl.isAgreed = false;
            scope.openTerms = function (size) {

                var modalInstance = $uibModal.open({
                    templateUrl: '/terms.html',
                    controller: ModalInstanceCtrl,
                    size: size
                });

                var state = $('#modal-state');
                modalInstance.result.then(function () {
                    state.text('Modal dismissed with OK status');
                }, function () {
                    state.text('Modal dismissed with Cancel status');
                });
            };



            // Please note that $uibModalInstance represents a modal window (instance) dependency.
            // It is not the same as the $uibModal service used above.

            var ModalInstanceCtrl = function ($scope, $uibModalInstance) {

                $scope.ok = function () {
                    $uibModalInstance.close('closed');
                };

                $scope.cancel = function () {
                    $uibModalInstance.dismiss('cancel');
                };
            };
            ModalInstanceCtrl.$inject = ['$scope', '$uibModalInstance'];
        }



        }

    }


})();
