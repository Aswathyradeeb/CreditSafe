/**=========================================================
 * Module: LoginController.js
 * Controller for the Chat APP 
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('QuestionareController', QuestionareController);

    /* @ngInject */
    function QuestionareController($rootScope, UserProfile, $scope, $http, $filter, $uibModal, $state, SweetAlert) {
        var vm = this;
        vm.user = UserProfile.getProfile();
        vm.event = {};
        vm.questionsDt = {};
        vm.filterParams = {};

        vm.questionsDt.open = function (size) {
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Questions/page.addQuestions.html',
                controller: 'addquestionsController',
                size: size,
                keyboard: false,
                backdrop: 'static',
                resolve: {
                    addqstns: function () {
                        return null;
                    }
                }
            });
            modalInstance.result.then(function () {
                $state.go($state.current, {}, { reload: true });
            }, function () { });
        };

        vm.questionsDt.edit = function (size, questionId) {
            vm.question = $filter('filter')(vm.questions, { id: questionId })[0];
            if (vm.question.answer != null) {
                return;
            }
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Questions/page.addQuestions.html',
                controller: 'addquestionsController',
                size: size,
                keyboard: false,
                backdrop: 'static',
                resolve: {
                    addqstns: function () {
                        return vm.question;
                    }
                }
            });
            modalInstance.result.then(function () {
                $state.go($state.current, {}, { reload: true });
            }, function () {});
        };

        vm.submitAnswer = function (id, answer) {
            vm.event.id = id;
            vm.event.answer = answer;
            $http.post($rootScope.app.httpSource + 'api/Event/UpdateAnswer', vm.event)
                .then(function (response) {
                    vm.isBusy = false;
                    debugger;
                    vm.event = response.data;
                    SweetAlert.swal($filter('translate')('general.ok'), $filter('translate')('general.answerAddedSuccess'), "success");
                    $window.location.reload();
                    //$state.go("app.questionare");

                },
                    function (response) {
                        vm.isBusy = false;
                    });
        };

        $http.get($rootScope.app.httpSource + 'api/Event/GetQuestions')
            .then(function (response) {
                vm.questions = response.data;
            }, function (response) { });

        $http.get($rootScope.app.httpSource + 'api/Event/GetEventsLookUp')
            .then(function (response) {

                vm.events = response.data;
                console.log(vm.events);
            }, function (response) { });

        $http.get($rootScope.app.httpSource + 'api/Speaker/GetSpeakers')
            .then(function (response) {
                vm.eventSpeaker = response.data;
            }, function (response) { });

        $scope.getQuestions = function () {
            var eventId = vm.filterParams.eventId == undefined ? 0 : vm.filterParams.eventId;
            var speakerId = vm.filterParams.speakerId == undefined ? 0 : vm.filterParams.speakerId;

            $http.get($rootScope.app.httpSource + 'api/Event/GetQuestions?eventid=' + eventId + '&speakerid=' + speakerId)
                .then(function (response) {
                    vm.questions = response.data;
                });
        };
    }

    QuestionareController.$inject = ['$rootScope', 'UserProfile', '$scope', '$http', '$filter', '$uibModal', '$state', 'SweetAlert'];

})();