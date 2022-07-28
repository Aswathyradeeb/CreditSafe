(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('SurveyController', SurveyController);

    function SurveyController($rootScope, $http, $scope, $stateParams, $state, WizardHandler, $filter, $compile, UserProfile, SweetAlert) {
        var vm = this;


        vm.Init = function () {

            vm.event = {
                id: 0,
                eventSpeakers: []
            };
            vm.eventSpeaker = {
                eventId: 0, 
                person: {}
            }; 
            debugger; 
            $http.get($rootScope.app.httpSource + 'api/Survey/GetSurveys')
              .then(function (resp) { 
                  vm.survey = resp.data;
                  console.log(vm.survey);
              },
              function (response) {
              });

        };

        vm.Init();

        vm.submitOption = function (data) {
            debugger;
            $http.post($rootScope.app.httpSource + 'api/Survey/CreateSurveyResult  ', data)
               .then(function (response) {

               },
               function (response) {
               });
        }
 
    }

    SurveyController.$inject = ['$rootScope', '$http', '$scope', '$stateParams', '$state', 'WizardHandler', '$filter',
         '$compile', 'UserProfile', 'SweetAlert'];

})();