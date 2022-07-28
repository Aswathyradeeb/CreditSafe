(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('editPersonController', editPersonController);
    /* @ngInject */
    function editPersonController($rootScope, $scope, $http, $uibModalInstance, $filter, person, viewMode, SweetAlert) {
        debugger;
        $scope.PhotoPath = 'api/Upload/UploadFile?uploadFile=Attachment';
        $scope.viewMode = viewMode;
        if (person !== null) {
            $scope.personObj = angular.copy(person);
        }
        else {
            $scope.personObj = {};
        }

        $scope.ok = function () {
            if ($scope.personObj.id == undefined) {
                $http.post($rootScope.app.httpSource + 'api/Speaker/CreateSpeaker', $scope.personObj)
                    .then(function (response) {
                        SweetAlert.swal($filter('translate')('event.speaker'), $filter('translate')('person.personAdded'), "success");
                        $uibModalInstance.close($scope.personObj);
                    }, function (response) { });
            }
            else {
                $http.post($rootScope.app.httpSource + 'api/Speaker/UpdateSpeaker', $scope.personObj)
                    .then(function (response) {
                        SweetAlert.swal($filter('translate')('event.speaker'), $filter('translate')('person.personEdited'), "success");
                        $uibModalInstance.close($scope.personObj);
                    }, function (response) { });
            }
        };

        $scope.closeModal = function () {
            $uibModalInstance.dismiss('cancel');
        };

    }

    editPersonController.$inject = ['$rootScope', '$scope', '$http', '$uibModalInstance', '$filter', 'person', 'viewMode','SweetAlert'];
})();