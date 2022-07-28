(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('personController', personController);
    /* @ngInject */
    function personController($rootScope, $scope, $http, $uibModalInstance, $filter, eventId, eventSpeaker, personTypeId, SweetAlert) {
        debugger;
        $scope.PhotoPath = 'api/Upload/UploadFile?uploadFile=Attachment';

        if (eventSpeaker !== null) {
            $scope.eventSpeaker = angular.copy(eventSpeaker);
        }
        else {
            $scope.eventSpeaker = {
                person: {},
                eventId: eventId,
                personTypeId: personTypeId
            };
        }
        $scope.ok = function () {
            if (!$scope.eventSpeaker.id) {
                $http.post($rootScope.app.httpSource + 'api/EventPerson/CreateEventPerson', $scope.eventSpeaker)
                    .then(function (response) {
                        SweetAlert.swal($filter('translate')('event.speaker'), $filter('translate')('person.personAdded'), "success");
                        $scope.eventSpeaker = response.data.result;
                        $uibModalInstance.close($scope.eventSpeaker);
                    }, function (response) {
                        if (response.data.innerException.innerException.innerException.exceptionMessage == "Person Already Exists With This Email") {
                            SweetAlert.swal($filter('translate')('event.speaker'), $filter('translate')('person.personExistsEmail'), "error");
                        }
                        if (response.data.message) {
                            SweetAlert.swal($filter('translate')('general.error'), $filter('translate')('general.errorException'), "error");
                        }
                    });
            } else {
                $http.post($rootScope.app.httpSource + 'api/EventPerson/UpdateEventPerson', $scope.eventSpeaker)
                    .then(function (response) {
                        SweetAlert.swal($filter('translate')('event.speaker'), $filter('translate')('person.personEdited'), "success");
                        $scope.eventSpeaker = response.data;
                        $uibModalInstance.close($scope.eventSpeaker);
                    }, function (response) {
                        if (response.data.innerException.innerException.innerException.exceptionMessage == "Person Already Exists With This Email") {
                            SweetAlert.swal($filter('translate')('event.speaker'), $filter('translate')('person.personExistsEmail'), "error");
                        }
                        if (response.data.message) {
                            SweetAlert.swal($filter('translate')('general.error'), $filter('translate')('general.errorException'), "error");
                        }
                    });
            }

        };

        $scope.closeModal = function () {
            $uibModalInstance.dismiss('cancel');
        };

    }

    personController.$inject = ['$rootScope', '$scope', '$http', '$uibModalInstance', '$filter', 'eventId', 'eventSpeaker', 'personTypeId', 'SweetAlert'];
})();