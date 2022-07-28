(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('addquestionsController', addquestionsController);

    addquestionsController.$inject = ['$rootScope', '$scope', 'UserProfile', '$filter', '$compile', '$http', '$state', 'SweetAlert', '$uibModalInstance', 'addqstns'];
    function addquestionsController($rootScope, $scope, UserProfile, $filter, $compile, $http, $state, SweetAlert, $uibModalInstance, addqstns) {
        var vm = this;
        $scope.user = UserProfile.getProfile();
        if (addqstns != null) {
            $scope.addqstns = addqstns;
        }
        else {
            $scope.addqstns = {
                userId: $scope.user.userId
            };
        }
        $scope.Init = function () {
            $scope.question = "";
            $scope.questionAr = 0;
            $scope.event = {
                EventId: 0,
                eventSpeakers: []
            };
            $scope.eventSpeaker = {
                speakerId: 0,
                person: {}
            };
            //$scope.isBusy = true;
            debugger;
            $http.get($rootScope.app.httpSource + 'api/Event/GetEventsLookUp')
                .then(function (response) {

                    $scope.events = response.data;

                },
                    function (response) { });
        };
        $scope.Init();
        $scope.savequestion = function () {
            $http.post($rootScope.app.httpSource + 'api/Event/Questions', $scope.addqstns)
                .then(function (response) {
                    debugger;
                    $scope.isBusy = false;
                    console.log(response);
                    $scope.event = response.data;
                    SweetAlert.swal($filter('translate')('general.ok'), $filter('translate')('general.questionAddedSuccess'), "success");
                    $uibModalInstance.dismiss('cancel');
                    $state.go("app.questionare");
                },
                    function (response) {
                        $scope.isBusy = false;
                    });
        };

        $scope.closeModal = function () {
            $uibModalInstance.dismiss('cancel');
            // angular.element('.modal').css('display', 'none');
        };

        $scope.onSelectedEvent = function (selectedItem) {
            $scope.addqstns.EventId = selectedItem.id;
            $http.get($rootScope.app.httpSource + 'api/Speaker/GetSpeakersByEventId?eventId=' + selectedItem.id)
                .then(function (response) {
                    $scope.eventSpeaker = response.data;
                    console.log($scope.eventSpeaker);
                },
                    function (response) {
                    });
        };


    }
})();