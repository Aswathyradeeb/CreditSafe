(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('searchSpeakerController', searchSpeakerController);

    searchSpeakerController.$inject = ['$rootScope', '$scope', 'UserProfile', '$filter', '$compile', '$http', '$uibModalInstance', '$state', 'SweetAlert', '$window', 'eventId', 'eventPersons', 'personTypeId'];
    function searchSpeakerController($rootScope, $scope, UserProfile, $filter, $compile, $http, $uibModalInstance, $state, SweetAlert, $window, eventId, eventPersons, personTypeId) {
        debugger;
        $scope.eventPersons = angular.copy(eventPersons);
        $http.get($rootScope.app.httpSource + 'api/Speaker/GetSpeakers')
            .then(function (resp) {
                $scope.speakers = resp.data;
            }, function (response) { });

        $scope.SpeakerExists = function (checkboxValue) {
            if (checkboxValue.id) {
                var index = $scope.eventPersons.indexOf($filter('filter')($scope.eventPersons, { person: { id: checkboxValue.id } }, true)[0]);
                if (index != -1) {
                    return true;
                }
                return false;
            }
            else {
                var index = $scope.eventPersons.indexOf($filter('filter')($scope.eventPersons, { person: { id: checkboxValue } }, true)[0]);
                if (index != -1) {
                    return true;
                }
                return false;
            }
            return false;
        };

        $scope.Speakerchecked = function (checkboxValue) {
            debugger;
            if (checkboxValue.nameEn) {
                var eventSpeaker = {
                    eventId: eventId,
                    person: checkboxValue,
                    personId: checkboxValue.id,
                    personTypeId: personTypeId
                }
                $scope.eventPersons.push(eventSpeaker);
            }
            else {
                var index = $scope.eventPersons.indexOf($filter('filter')($scope.eventPersons, { person: { id: checkboxValue } }, true)[0]);
                $scope.eventPersons.splice(index, 1);
            }
        };

        $scope.ok = function () {
            $uibModalInstance.close($scope.eventPersons);
        };

        $scope.closeModal = function () {
            $uibModalInstance.dismiss('cancel');
        };
    }
})();
