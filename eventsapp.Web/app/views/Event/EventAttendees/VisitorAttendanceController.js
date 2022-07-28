(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('VisitorAttendanceController', VisitorAttendanceController);

    VisitorAttendanceController.$inject = ['$rootScope', '$http', 'UserProfile', '$scope', '$stateParams', '$state', '$filter', 'SweetAlert', 'moment'];
    function VisitorAttendanceController($rootScope, $http, UserProfile, $scope, $stateParams, $state, $filter, SweetAlert, moment) {
        var vm = this;
        vm.translateFilter = $filter('translate');
        var textAreaElement = document.getElementById("qrData");
        textAreaElement.focus();
        textAreaElement.addEventListener('blur', e => {
            e.target.focus();
        });
        textAreaElement.addEventListener("keypress keydown keyup", function (e) {
            e.stopPropagation();
        });
        vm.invalidQR = false;
        vm.vistorNotFound = false;
        vm.attendanceMarked = false;
        vm.userData = {};
        vm.selectedLang = $rootScope.language.selected == 'English' ? "en" : "ar";
        vm.currentTime = moment(new Date()).format("HH:mm");

        $http.get($rootScope.app.httpSource + 'api/Agenda/GetAgendaEvent?EventId=55')
            .then(function (response) {
                vm.agendaList = response.data;
                for (var i = 0; i < vm.agendaList.length; i++) {
                    vm.agendaList[i].startDate = new Date(vm.agendaList[i].startDate);
                }
                vm.agenda = vm.agendaList.filter(function (item) { return item.startDate.getDate() == new Date().getDate() && vm.currentTime >= item.fromTime24Hr && vm.currentTime <= item.toTime24Hr})[0];
            }, function (response) { });

        vm.validateVisitorData = function () {
            var QrDataArray = vm.qrData.split('|');
            if (QrDataArray.length == 6) {
                vm.invalidQR = false;
                vm.vistorNotFound = false;
                vm.userData = {
                    fName: QrDataArray[0],
                    phone: QrDataArray[1],
                    email: QrDataArray[2],
                    eventId: parseInt(QrDataArray[3]),
                    userId: parseInt(QrDataArray[4]),
                    agendaId: parseInt(QrDataArray[5]),
                };

                $http.get($rootScope.app.httpSource + 'api/EventUser/GetEventAttendee?UserId=' + vm.userData.userId + "&EventId=" + vm.userData.eventId + "&AgendaId=" + vm.userData.agendaId)
                    .then(function (response) {
                        vm.eventAttendee = response.data;
                    }, function (response) { });

                $http.get($rootScope.app.httpSource + 'api/EventUser/QRVisitorAttendance?UserId=' + vm.userData.userId + "&EventId=" + vm.userData.eventId + "&AgendaId=" + vm.userData.agendaId + "&lang=" + vm.selectedLang)
                    .then(function (response) {
                        if (response.data == "Visitor attendance is marked successfully") {
                            vm.attendanceMarked = true;
                            vm.qrData = '';
                            //vm.userData = {};
                            vm.vistorNotFound = false;
                        } else if (response.data == "QR Code is invalid. Vistor not found") {
                            vm.attendanceMarked = false;
                            vm.vistorNotFound = true;
                            vm.qrData = '';
                            vm.userData = {};
                        }
                        else if (response.data == "QR Code is not valid for current slot") {
                            vm.attendanceMarked = false;
                            vm.vistorNotFound = false;
                            vm.qrNotValidSlot = true;
                            vm.invalidQR = false;
                            vm.qrData = '';
                            vm.userData = {};
                        }
                    },
                        function (response) { });
            } else {
                vm.invalidQR = true;
                vm.showRequiredError = true;
                vm.vistorNotFound = false;
                vm.attendanceMarked = false;
                vm.userData = {};
            }

        };
    }

})();