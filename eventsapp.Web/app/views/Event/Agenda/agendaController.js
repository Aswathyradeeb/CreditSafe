(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('agendaController', agendaController);
    /* @ngInject */
    function agendaController($rootScope, $scope, $uibModalInstance, $filter, $location, agendaObj, agendaStartDate, agendaEndDate, agendaSessions, $http, moment, speakers, participantsRegistrationTypeId) {
        debugger;
        $scope.persons = speakers;
        $scope.participantsRegistrationTypeId = participantsRegistrationTypeId;
        $scope.EventId = $location.search()['id'];
        if (agendaObj != undefined) {
            $scope.agenda = angular.copy(agendaObj);
            $scope.agenda.startDate = new Date($scope.agenda.startDate);
            //$scope.agenda.endDate = new Date($scope.agenda.endDate);
        } else {
            $scope.agenda = {};
        }
        $scope.agendaStartDate = angular.copy(agendaStartDate);
        //$scope.agendaEndDate = angular.copy(agendaEndDate);
        $scope.agendaSessions = angular.copy(agendaSessions);

        $scope.ok = function () {
            debugger;
            if ($scope.participantsRegistrationTypeId == 2 && !$scope.agenda.participantsLimit) {
                $scope.showLimitMsg = true;
                return;
            }
            else {
                $scope.showLimitMsg = false;
            }
            $scope.agenda.startDate = moment.utc($scope.agenda.startDate).format();
            //$scope.agenda.endDate = moment.utc($scope.agenda.endDate).format();
            $uibModalInstance.close($scope.agenda);
        };

        $scope.closeModal = function () {
            $uibModalInstance.dismiss('cancel');
        };

        setTimeout(function () {
            $('#timeFrom').clockpicker({
                autoclose: true,
                placement: 'top',
                twelvehour: true,
                align: 'left',
                'default': 'now',
                donetext: 'Select Time: HH:MM',
                afterDone: function () {
                    //console.log("after done");
                    $('#timeFrom').val($('#timeFrom').val().slice(0, -2) + ' ' + $('#timeFrom').val().slice(-2));
                    $scope.agenda.fromTime = $('#timeFrom').val();
                }
            });

            $('#timeTo').clockpicker({
                //minTime: '11:45:00', // 11:45:00 AM,
                //startTime: new Date(0, 0, 10, 15, 0, 0), // 3:00:00 PM - noon
                autoclose: true,
                placement: 'top',
                twelvehour: true,
                align: 'left',
                'default': 'now',
                donetext: 'Select Time: HH:MM',
                afterDone: function () {
                    //console.log("after done");
                    $('#timeTo').val($('#timeTo').val().slice(0, -2) + ' ' + $('#timeTo').val().slice(-2));
                    $scope.agenda.toTime = $('#timeTo').val();
                }
            });
        }, 2000);

        //if ($scope.EventId || $scope.agenda.eventId) {
        //    var eventID = $scope.EventId ? $scope.EventId : $scope.agenda.eventId;
        //    $http.get($rootScope.app.httpSource + 'api/Speaker/GetSpeakersByEventId?EventId=' + eventID)
        //        .then(function (response) {
        //            $scope.persons = response.data;
        //        });
        //}

        $scope.checkTime = function () {
            debugger;
            var fromTime = moment($scope.agenda.fromTime, "h:mm A").format("HH:mm");
            var toTime = moment($scope.agenda.toTime, "h:mm A").format("HH:mm");
            if (toTime < fromTime) {
                delete $scope.agenda.toTime;
            }
        };

        //Date Popup Options
        $scope.clearStartDate = function () {
            $scope.agenda.startDate = null;
        };

        $scope.clearEndDate = function () {
            $scope.agenda.endDate = null;
        };
        $scope.today = new Date();

        var start = new Date();
        start.setFullYear(start.getFullYear() - 97);
        var end = new Date();
        end.setFullYear(end.getFullYear() - 16);

        $scope.dateOptions = {
            minDate: start,
            maxDate: end,
            startingDay: 1,
            todayBtn: false
        };

        $scope.openStartDatePopup = function () {
            $scope.startDate.opened = true;
        };

        $scope.openEndDatePopup = function () {
            $scope.endDate.opened = true;
        };

        $scope.setDate = function (year, month, day) {
            $scope.agenda.startDate = new Date(year, month, day);
        };

        $scope.format = 'dd-MMMM-yyyy';


        $scope.toggleMin = function () {
            $scope.minDate = start;
            //$scope.minDate = $scope.minDate ? null : new Date();
        };

        $scope.startDate = {
            opened: false
        };

        $scope.endDate = {
            opened: false
        };

        //Date Popup Options
        $scope.clearStartDate = function () {
            $scope.agenda.startDate = null;
        };

        $scope.clearEndDate = function () {
            $scope.agenda.endDate = null;
        };
        $scope.today = new Date();

        $scope.disabled = function (date, mode) {
            if ($scope.agenda.id == undefined) {
                var today = new Date();
                return date < today;
            }
            return true;
        };

        $scope.enddisabled = function (date, mode) {
            return date < new Date($scope.agenda.startDate);
        };

        $scope.openStartDatePopup = function () {
            $scope.startDate.opened = true;
        };

        $scope.openEndDatePopup = function () {
            $scope.endDate.opened = true;
        };

        $scope.setDate = function (year, month, day) {
            $scope.agenda.startDate = new Date(year, month, day);
        };

        //END

        //Time Picker

        $scope.mytime = new Date();

        $scope.hstep = 1;
        $scope.mstep = 15;

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        $scope.ismeridian = true;

        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        $scope.update = function () {
            var d = new Date();
            d.setHours(14);
            d.setMinutes(0);
            $scope.mytime = d;
        };

        $scope.changed = function () {
            //console.log('Time changed to: ' + $scope.mytime);
        };

        $scope.clear = function () {
            $scope.mytime = null;
        };

        //End

    }

    agendaController.$inject = ['$rootScope', '$scope', '$uibModalInstance', '$filter', '$location', 'agendaObj', 'agendaStartDate', 'agendaEndDate', 'agendaSessions', '$http', 'moment', 'speakers', 'participantsRegistrationTypeId'];
})();