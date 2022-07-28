/**=========================================================
 * Module: LoginController.js
 * Controller for the Chat APP 
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('RegisterController', RegisterController);
    /* @ngInject */
    function RegisterController($rootScope, UserProfile, $scope, $http, $filter, $uibModal, $state, $stateParams, RegisterService, SweetAlert, $location, vcRecaptchaService, $timeout) {
        debugger;
        var vm = this;
        vm.account = {
            isIndividual: false,
            agendaDate: new Date()
        };
        vm.todayDay = (new Date()).getDate();
        angular.element(document).ready(function () {
            $timeout(function () {
                if ($location.search().lang && $location.search().lang == 'ar' && sessionStorage.getItem('viewRendered') != 'true') {
                    sessionStorage.setItem('viewRendered', 'true');
                    var btn = document.getElementById('btn-ar');
                    btn.click();
                } else if ($location.search().lang && $location.search().lang == 'en' && sessionStorage.getItem('viewRendered') != 'true') {
                    sessionStorage.setItem('viewRendered', 'true');
                    var btn = document.getElementById('btn-en');
                    btn.click();
                }
            }, 10);
        });

        vm.account.selectedLang = $rootScope.language.selected == 'English' ? "en" : "ar";
        vm.checkData = 0;
        vm.EventId = "";

        if ($state.current.name == 'page.registerAdmin') {
            vm.account.UserRole = 'Admin';
        }

        //for displaying event name if registering from outside(url will have eventid and adminId)
        vm.checkEvent = function () {

            vm.eventid = $location.search().eid;
            vm.companyCode = $location.search().companyCode;
            if (vm.eventid) {
                $http.get($rootScope.app.httpSource + 'api/Event/GetEventRegitration?id=' + vm.eventid)
                    .then(function (resp) {
                        if (resp.data != null) {
                            vm.event = resp.data;
                            vm.event.startDate = new Date(vm.event.startDate);
                            vm.event.endDate = new Date(vm.event.endDate);
                            for (var i = 0; i < vm.event.agendaSessions.length; i++) {
                                vm.event.agendaSessions[i].date = new Date(vm.event.agendaSessions[i].agenda[0].startDate);

                                //for (var j = 0; j < vm.event.agendaSessions[i].agenda.length; j++) {
                                //    vm.event.agendaSessions[i].agenda[j].fromTime24Hr = moment(vm.event.agendaSessions[i].agenda[j].fromTime, "h:mm A").format("HH:mm");
                                //    //vm.event.agendaSessions[i].agenda[j].toTime24Hr = moment(vm.event.agendaSessions[i].agenda[j].toTime, "h:mm A").format("HH:mm");
                                //}
                            }

                            if (new Date() < new Date(vm.event.startDate)) {
                                vm.selectedSession = vm.event.agendaSessions[0];
                            }
                            else {
                                vm.selectedSession = vm.event.agendaSessions.filter(function (item) { return item.date.getDate() == new Date().getDate(); })[0];
                            }

                            if (!vm.selectedSession) {
                                vm.selectedSession = vm.event.agendaSessions[0];
                            }

                            vm.account.agendaDate = vm.selectedSession.date;

                            vm.checkData = 1;

                            if (vm.companyCode == vm.event.user.company.companyCode) {
                                vm.account.companyCode = vm.event.user.company.companyCode;
                            }
                        }

                        $http.get($rootScope.app.httpSource + 'api/RegistrationType/GetRegistrationTypes')
                            .then(function (response) {
                                vm.registrationTypes = response.data;
                                if (vm.event.exhibitorsOnlineRegister != true) {
                                    vm.registrationTypes = vm.registrationTypes.filter(function (item) { return item.id != 4 });
                                }
                                if (vm.event.partnersOnlineRegister != true) {
                                    vm.registrationTypes = vm.registrationTypes.filter(function (item) { return item.id != 6 });
                                }
                                if (vm.event.speakerOnlineRegister != true) {
                                    vm.registrationTypes = vm.registrationTypes.filter(function (item) { return item.id != 3 });
                                }
                                if (vm.event.sponsorsOnlineRegister != true) {
                                    vm.registrationTypes = vm.registrationTypes.filter(function (item) { return item.id != 5 });
                                }
                                if (vm.event.vipOnlineRegister != true) {
                                    vm.registrationTypes = vm.registrationTypes.filter(function (item) { return item.id != 2 });
                                }
                            }, function (response) { });
                    }
                        , function (response) { });
            }
        }

        vm.checkEvent();

        vm.account.isAgree = true;
        vm.validMobile = false;
        vm.isAgreed = true;
        vm.emailAlreadyTaken = false;
        vm.user = UserProfile.getProfile();
        //  vm.translateFilter = $filter('translate');
        vm.recaptchaLang = $rootScope.app.layout.isRTL ? "ar" : "en";

        //Date Popup Options
        vm.clearAgendaDate = function () {
            vm.account.agendaDate = null;
        };

        vm.today = new Date();

        var start = new Date();
        start.setFullYear(start.getFullYear() - 97);
        var end = new Date();
        end.setFullYear(end.getFullYear() - 16);

        vm.dateOptions = {
            minDate: start,
            maxDate: end,
            startingDay: 1,
            todayBtn: false
        };

        vm.openAgendaDatePopup = function () {
            vm.agendaDate.opened = true;
        };

        vm.setDate = function (year, month, day) {
            vm.account.agendaDate = new Date(year, month, day);
        };

        vm.format = 'dd-MMMM-yyyy';

        vm.agendaDate = {
            opened: false
        };

        vm.changeSesstionDate = function () {
            vm.selectedSession = vm.event.agendaSessions.filter(function (item) { return item.date.getDate() == vm.account.agendaDate.getDate() })[0];
        };

        if (vm.user.roleName == 1) {
            $http.get($rootScope.app.httpSource + 'api/Roles/GetAll')
                .then(function (response) {
                    vm.roles = response.data;
                });
        }

        if ($state.current.name == 'page.registerAdmin' || vm.account.userRole == 'Admin') {
            $http.get($rootScope.app.httpSource + 'api/Lookup/GetLanguages')
                .then(function (response) {
                    vm.languages = response.data;
                });

            $http.get($rootScope.app.httpSource + 'api/Lookup/GetEventPackages')
                .then(function (response) {
                    vm.eventPackages = response.data;
                });
        }

        vm.onSelectedEvent = function (selectedItem) {
            vm.account.UserRole = selectedItem.nameEn;
        };

        vm.openTerms = function (size) {

            var modalInstance = $uibModal.open({
                templateUrl: '/terms.html',
                controller: ModalInstanceCtrl,
                size: size
            });

            var state = $('#modal-state');
            modalInstance.result.then(function () {
                state.text('Modal dismissed with OK status');
            }, function () {
                state.text('Modal dismissed with Cancel status');
            });
        };

        vm.setWidgetId = function (widgetId) {
            vm.widgetId = widgetId;
        };

        vm.selectAgenda = function (agendaObj) {
            //if (vm.selectedSession.date.getDate() < vm.todayDay) {
            //    return;
            //}
            if (agendaObj.reservationCount >= agendaObj.participantsLimit) {
                return;
            }
            else if (!vm.account.isIndividual && ((agendaObj.reservationCount + vm.account.visitorCount) > agendaObj.participantsLimit)) {
                vm.selectedAgenda = agendaObj;
                vm.showRequiredError = true;
                return;
            } else {
                vm.account.agendaId = agendaObj.id;
                vm.selectedAgenda = agendaObj;
                vm.showRequiredError = false;
            }
        };

        vm.checkReservationAvaiablity = function () {
            if (vm.selectedAgenda.reservationCount >= vm.selectedAgenda.participantsLimit) {
                return;
            }
            else if (!vm.account.isIndividual && ((vm.selectedAgenda.reservationCount + vm.account.visitorCount) > vm.selectedAgenda.participantsLimit)) {
                vm.showRequiredError = true;
                return false;
            } else {
                vm.showRequiredError = false;
                return true;
            }
        }

        vm.submit = function () {
            if (vm.account.eventPackage) {
                vm.account.eventPackageId = vm.account.eventPackage.id;
                vm.account.eventPackage = null;
            }
            if (vm.event.participantsRegistrationTypeId == 2 && !vm.account.agendaId) {
                vm.showRequiredError = true;
                return;
            }
            if (!vm.checkReservationAvaiablity()) {
                vm.showRequiredError = true;
                return;
            };

            vm.showRequiredError = false;

            if (vm.account.preferredLanguagesList) {
                vm.account.preferredLanguages = [];

                for (var i = 0; i < vm.account.preferredLanguagesList.length; i++) {
                    var preferredLanguage = {
                        userId: 0,
                        languageId: vm.account.preferredLanguagesList[i].id
                    };
                    vm.account.preferredLanguages.push(preferredLanguage);
                }
            }

            debugger;
            if ($state.current.name == 'page.registerAdmin') {
                vm.account.UserRole = 'Admin';
            }

            if (!vm.account.isAgree) {
                vm.isAgreed = false;
                return false;
            }
            else {
                vm.isAgreed = true;
            }
            vm.isBusy = true;
            if (vm.checkData == 1) {
                vm.account.eventId = vm.event.id;
            }
            if (!vm.account.companyCode) {
                vm.account.companyCode = vm.companyCode;
            }
            vm.account.registrationTypeId = 1;
            vm.account.password = 'Attendee@123';
            vm.account.confirmPassword = 'Attendee@123';

            $http.post($rootScope.app.httpSource + 'api/Account/Register', vm.account)
                .then(function (response) {
                    RegisterService.setRegisteredUser(response.data.userId, response.data.phoneNumber);
                    SweetAlert.swal($filter('translate')('general.congrats'), $filter('translate')('register.successful'), "success");
                    vm.isBusy = false;
                    debugger;
                    if (vm.user == null) {
                        $state.go('page.login');
                    }
                    else {
                        $state.go('app.home');
                    }
                    setTimeout(function () {
                        window.location.reload();
                    }, 1000)

                },
                    function (response) {
                        vm.isBusy = false;
                        if (response.data.modelState.userAlreadyRegisteredInAgenda && response.data.modelState.userAlreadyRegisteredInAgenda[0] !== undefined) {
                            SweetAlert.swal($filter('translate')('general.error'), $filter('translate')('register.userAlreadyRegisteredInAgenda'), "error");
                        }
                        else if (response.data.modelState.userAlreadyRegisteredInEvent && response.data.modelState.userAlreadyRegisteredInEvent[0] !== undefined) {
                            SweetAlert.swal($filter('translate')('general.error'), $filter('translate')('register.userAlreadyRegisteredInEvent'), "error");
                        }
                        else if (response.data.modelState.eventParticipantLimitReached && response.data.modelState.eventParticipantLimitReached[0] !== undefined) {
                            SweetAlert.swal($filter('translate')('general.error'), $filter('translate')('register.eventParticipantLimitReached'), "error");
                        }
                        else if (response.data.modelState.agendaParticipantLimitReached && response.data.modelState.agendaParticipantLimitReached[0] !== undefined) {
                            SweetAlert.swal($filter('translate')('general.error'), $filter('translate')('register.agendaParticipantLimitReached'), "error");
                        }
                        else if (response.data.modelState.invalidDataExceptionBase && response.data.modelState.invalidDataExceptionBase[0] !== undefined) {
                            vm.emailAlreadyTaken = true;
                            SweetAlert.swal($filter('translate')('general.error'), $filter('translate')('register.failure'), "error");
                        }
                        else {
                            SweetAlert.swal($filter('translate')('general.error'), $filter('translate')('register.emailTaken'), "error");
                        }
                        vcRecaptchaService.reload(vm.widgetId);
                    });
        };

        vm.validMobileNumber = function () {
            vm.validMobile = true;
        };

        vm.invalidMobileNumber = function () {
            vm.validMobile = false;
        };

        // Please note that $uibModalInstance represents a modal window (instance) dependency.
        // It is not the same as the $uibModal service used above.

        var ModalInstanceCtrl = function ($scope, $uibModalInstance) {

            vm.ok = function () {
                $uibModalInstance.close('closed');
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss('cancel');
            };
        };
        ModalInstanceCtrl.$inject = ['$scope', '$uibModalInstance'];

    }

    RegisterController.$inject = ['$rootScope', 'UserProfile', '$scope', '$http', '$filter', '$uibModal', '$state', '$stateParams', 'RegisterService', 'SweetAlert', '$location', 'vcRecaptchaService','$timeout'];

})();