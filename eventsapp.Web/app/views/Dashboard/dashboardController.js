(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('dashboardController', dashboardController);

    function dashboardController($rootScope, $http, $scope, $stateParams, $state, WizardHandler, $filter, DTOptionsBuilder, DTColumnBuilder, $compile, $uibModal, SweetAlert, flotOptions) {
        var vm = this;
        vm.events = [];
        vm.translateFilter = $filter('translate');

        //vm.chartPieFlotChart = flotOptions['pie'];
        //vm.chartDonutFlotChart = flotOptions['donut'];
        //vm.userAttendanceSourceEn = [
        //    {
        //        id: 1,
        //        "color": "#00ff00",
        //        "data": 0,
        //        "label": "Present"
        //    },
        //    {
        //        id: 2,
        //        "color": "#ff0000",
        //        "data": 0,
        //        "label": "Absent"
        //    }
        //];
        //vm.userAttendanceSourceAr = [
        //    {
        //        id: 1,
        //        "color": "#00ff00",
        //        "data": 0,
        //        "label": "حضور"
        //    },
        //    {
        //        id: 2,
        //        "color": "#ff0000",
        //        "data": 0,
        //        "label": "غياب"
        //    }
        //];

        $scope.controllerScope = $scope;

        $http.get($rootScope.app.httpSource + 'api/Event/GetEvents')
            .then(function (resp) {
                vm.events = resp.data;
                if (vm.events.length > 0) {
                    vm.eventId = vm.events[0].id;
                    vm.reloadKPI();
                }
            },
                function (response) { });

        //$http.get($rootScope.app.httpSource + 'api/EventUser/GetEventUsers?eventId=' + 0)
        //    .then(function (response) {
        //        vm.eventusers = response.data;

        //        for (var i = 0; i < vm.eventusers.length; i++) {
        //            if (vm.eventusers[i].isAttended) {
        //                vm.userAttendanceSourceEn[0].data = vm.userAttendanceSourceEn[0].data + 1;
        //                vm.userAttendanceSourceAr[0].data = vm.userAttendanceSourceAr[0].data + 1;
        //            } else {
        //                vm.userAttendanceSourceEn[1].data = vm.userAttendanceSourceEn[1].data + 1;
        //                vm.userAttendanceSourceAr[1].data = vm.userAttendanceSourceAr[1].data + 1;
        //            }
        //        }
        //    }, function (response) { });

        vm.rerenderKPIByRegisteredUsers = function () {
            $http.post($rootScope.app.httpSource + 'api/KPI/GetRegisteredUsers?eventId=' + vm.eventId)
                .then(function (response) {
                    var KPIByRegisteredUsers = vm.KPIByRegisteredUsers != null ? vm.KPIByRegisteredUsers : { defaultColor: '#ff9999' };
                    delete vm.KPIByRegisteredUsers;
                    vm.KPIByRegisteredUsers = response.data;
                    vm.KPIByRegisteredUsers.selectedChartType = KPIByRegisteredUsers.selectedChartType;
                    vm.KPIByRegisteredUsers.selectedSeries = vm.KPIByRegisteredUsers.series[0];
                    vm.KPIByRegisteredUsers.chartConfig = {};
                    vm.KPIByRegisteredUsers.defaultColor = KPIByRegisteredUsers.defaultColor;
                    vm.KPIByRegisteredUsers.lightColor = '#ecd192';
                    vm.KPIByRegisteredUsers.bgColor = 'white';
                    vm.KPIByRegisteredUsers.textColor = 'black';
                    vm.KPIByRegisteredUsers.barWidth = '40';
                    vm.KPIByRegisteredUsers.height = '450px';
                    vm.KPIByRegisteredUsers.width = '300px';
                });
        };

        vm.rerenderKPIBySurveyResult = function () {
            $http.post($rootScope.app.httpSource + 'api/KPI/GetSurveyResult?eventId=' + vm.eventId)
                .then(function (response) {
                    var KPIBySurveyResult = vm.KPIBySurveyResult != null ? vm.KPIBySurveyResult : { defaultColor: '#ff9999' };
                    delete vm.KPIBySurveyResult;
                    vm.KPIBySurveyResult = response.data;
                    vm.KPIBySurveyResult.selectedChartType = KPIBySurveyResult.selectedChartType;
                    vm.KPIBySurveyResult.selectedSeries = vm.KPIBySurveyResult.series[0];
                    vm.KPIBySurveyResult.chartConfig = {};
                    vm.KPIBySurveyResult.defaultColor = KPIBySurveyResult.defaultColor;
                    vm.KPIBySurveyResult.lightColor = '#ecd192';
                    vm.KPIBySurveyResult.bgColor = 'white';
                    vm.KPIBySurveyResult.textColor = 'black';
                    vm.KPIBySurveyResult.barWidth = '40';
                    vm.KPIBySurveyResult.height = '450px';
                    vm.KPIBySurveyResult.width = '300px';
                });
        };

        vm.rerenderKPIByUsersAttendance = function () {
            $http.post($rootScope.app.httpSource + 'api/KPI/GetUsersAttendance?eventId=' + vm.eventId)
                .then(function (response) {
                    var KPIByUsersAttendance = vm.KPIByUsersAttendance != null ? vm.KPIByUsersAttendance : { defaultColor: '#ff9999' };
                    delete vm.KPIByUsersAttendance;
                    vm.KPIByUsersAttendance = response.data;
                    vm.KPIByUsersAttendance.selectedChartType = KPIByUsersAttendance.selectedChartType;
                    vm.KPIByUsersAttendance.selectedSeries = vm.KPIByUsersAttendance.series[0];
                    vm.KPIByUsersAttendance.chartConfig = {};
                    vm.KPIByUsersAttendance.defaultColor = KPIByUsersAttendance.defaultColor;
                    vm.KPIByUsersAttendance.lightColor = '#ecd192';
                    vm.KPIByUsersAttendance.bgColor = 'white';
                    vm.KPIByUsersAttendance.textColor = 'black';
                    vm.KPIByUsersAttendance.barWidth = '40';
                    vm.KPIByUsersAttendance.height = '450px';
                    vm.KPIByUsersAttendance.width = '300px';
                });
        };

        vm.reloadKPI = function () {
            vm.rerenderKPIByRegisteredUsers();
            vm.rerenderKPIBySurveyResult();
            vm.rerenderKPIByUsersAttendance();
        };

        //$http.get($rootScope.app.httpSource + 'api/Event/GetEventsLookUp')
        //    .then(function (response) {
        //        //vm.events = response.data;
        //        var event = response.data[response.data.length - 1]
        //        event.nameEn = 'Cutting-Edge Research on Talent Development';
        //        event.nameAr = 'احدث الابحاث في تطوير المواهب';
        //        vm.events.push(event);
        //    },
        //    function (response) { });

        $scope.onSelectedEvent = function (selectedItem) {
            $http.get($rootScope.app.httpSource + 'api/Survey/GetSurvey?eventId=' + selectedItem.id)
                .then(function (response) {
                    vm.survey = response.data.result;
                }, function (response) { });
        };
    }

    dashboardController.$inject = ['$rootScope', '$http', '$scope', '$stateParams', '$state', 'WizardHandler', '$filter', 'DTOptionsBuilder',
        'DTColumnBuilder', '$compile', '$uibModal', 'SweetAlert', 'flotOptions'];

})();