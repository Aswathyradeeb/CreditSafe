(function () {
    'use strict';

    angular.module('eventsapp')
        .controller('changePassController', changePassController);

    function changePassController($rootScope, $http, $scope, $uibModalInstance, $filter, $timeout, FileUploader, SweetAlert, user) {
        debugger;
        $scope.userDetails = angular.copy(user);
        $scope.passwordObj = {};
        $scope.passwordObj.userId = user.id;
        $scope.passwordObj.email = user.email;
        $scope.ok = function () {
            if ($scope.passwordObj.newPassword != $scope.passwordObj.confirmPassword) {
                SweetAlert.swal($filter('translate')('register.changePassword'), $filter('translate')('recover.passwordNotMatch'), "error");
                return;
            }
            $http.post($rootScope.app.httpSource + 'api/Account/ResetPasswordAdmin', $scope.passwordObj)
                .then(function (response) {
                    if (response.data == "PasswordReset") {
                        SweetAlert.swal($filter('translate')('register.changePassword'), $filter('translate')('recover.resetComplete'), "success");
                        $uibModalInstance.close($scope.passwordObj);
                    }
                    else
                        SweetAlert.swal($filter('translate')('register.changePassword'), $filter('translate')('recover.requestFailed'), "error");
                },
                    function (response) { // optional
                        SweetAlert.swal($filter('translate')('register.changePassword'), $filter('translate')('recover.requestFailed'), "error");
                    });
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };

        changePassController.$inject = ['$rootScope', '$http', '$scope', '$uibModalInstance', '$filter', '$timeout', 'FileUploader', 'SweetAlert', 'user'];
    }
})();