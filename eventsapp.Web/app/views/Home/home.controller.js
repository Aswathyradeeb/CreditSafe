(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('homeController', homeController);

    homeController.$inject = ['$rootScope', '$scope', 'UserProfile', '$filter', '$compile', '$http', '$uibModal', '$state', 'SweetAlert', '$window', 'PagerService', 'moment'];
    function homeController($rootScope, $scope, UserProfile, $filter, $compile, $http, $uibModal, $state, SweetAlert, $window, PagerService, moment) {

        var vm = this;
        vm.user = UserProfile.getProfile();
        vm.events = {};
        vm.compName = "";
        vm.registerEvent = function (eventid) {
            var event = $filter('filter')(vm.events, { id: eventid })[0];
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Home/RegisterEvent/registerEvent.html',
                controller: 'registerEventController',
                closable: true,
                animation: true,
                backdrop: 'static',
                keyboard: false,
                size: 'lg',
                resolve: {
                    event: function () {
                        return event;
                    }
                }
            });
            modalInstance.result.then(function () {
                vm.setPage(1);
            }, function () {
            });
        }

        vm.printCertificateOfParticipation = function (event) {
            debugger;
            var CertificateOfAppreciation = {
                studentName: vm.user.firstName + " " + vm.user.lastName,
                subjectName: event.nameAr,
                participationDate: event.startDate
            };
            $http.post(window.location.protocol + '//reporting.inlogic.ae/WebApi/' + 'api/Certificate/GenerateCertificateOfParticipation', CertificateOfAppreciation)
                .then(function (resp) {
                    window.open(resp.data, '_blank');
                }, function (response) { });
        };

        vm.userEventRegister = function (eventid) {
            // vm.events.registrationTypeId = 1;
            $http.post($rootScope.app.httpSource + 'api/EventUser/EventUserAdd?userid=' + vm.user.userId + '&eventid=' + eventid)
                .then(function (resp) {
                    if (resp.data == 'Existing') {
                        SweetAlert.swal($filter('translate')('general.notApplicable'), "Already Registered in this event", "error");
                    }
                    else {
                        SweetAlert.swal($filter('translate')('general.ok'), "Registered Successfully", "success");
                    }

                },
                    function (response) { });

        };

        if (vm.user.roleName == 1) {
            $rootScope.AdminMode = true;
            $rootScope.app.sidebar.isOffscreen = false;
            vm.show = false;
        }

        if (vm.user.roleName == 2) {
            $rootScope.AdminMode = true;
            vm.show = true;
            $rootScope.app.sidebar.isOffscreen = false;
        }

        vm.todayDate = new Date();
        vm.getDifference = function (todate) {
            var now = moment(new Date()); //todays date
            var end = moment(todate); // another date
            var duration = moment.duration(end.diff(now));
            vm.days = Math.round(duration.asDays());
            return vm.days;
        };

        $http.get($rootScope.app.httpSource + 'api/Company/GetAllCompanies')
            .then(function (resp) {
                vm.companies = resp.data;
            });

        vm.userRoles = [
            { name: "Speaker", displayName: $filter('translate')('event.speaker') },
            { name: "Participant", displayName: $filter('translate')('event.participant') },
            { name: "VIP", displayName: $filter('translate')('event.vip') },
            { name: "Sponsor", displayName: $filter('translate')('event.sponser') },
            { name: "Exhibitor", displayName: $filter('translate')('event.exhibitor') },
            { name: "Creator", displayName: $filter('translate')('event.creator') },
            { name: "Partner", displayName: $filter('translate')('event.partner') }];

        vm.filterParams = { events: [] };

        vm.params = {
            filterParams: (vm.filterParams === undefined ? null : vm.filterParams),
            page: 1,
            pageSize: 9
        };

        $http.post($rootScope.app.httpSource + 'api/Event/GetAllEvents', vm.params)
            .then(function (resp) {
                vm.events = resp.data.content;
                setTimeout(function () { if (typeof addthis !== 'undefined') { addthis.layers.refresh(); } }, 500);
            }, function (response) { });

        vm.pager = {};

        vm.setPage = function (page) {
            vm.params = {
                filterParams: (vm.filterParams === undefined ? null : vm.filterParams),
                page: page,
                pageSize: 9
            };

            $http.post($rootScope.app.httpSource + 'api/Event/GetAllEvents', vm.params)
                .then(function (resp) {
                    vm.events = resp.data.content;
                    setTimeout(function () { if (typeof addthis !== 'undefined') { addthis.layers.refresh(); } }, 500);
                }, function (response) { });
        };

        vm.delete = function (eventId) {
            var translate = $filter('translate');
            SweetAlert.swal({
                title: translate('general.delete'),
                text: translate('general.confirmDeleteMsg'),
                type: "warning",
                showCancelButton: true,
                showLoaderOnConfirm: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: translate('general.delete'),
                closeOnConfirm: true,
                closeOnCancel: true
            },
                function (isConfirm) {
                    if (isConfirm) {
                        //delete
                        debugger;
                        $http.post($rootScope.app.httpSource + 'api/Event/DeleteEvent?eventId=' + eventId)
                            .then(function (resp) {
                                SweetAlert.swal(translate('general.delete'), translate('general.deleteMessage'), "success");
                                vm.setPage(1);
                            },
                                function (response) { });

                    } else {
                        SweetAlert.swal(translate('general.restoreBtn'), translate('general.restoreMessage'), "success");
                    }
                });
        };
    }

})();
