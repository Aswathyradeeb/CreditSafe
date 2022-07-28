(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('eventController', eventController);

    function eventController($rootScope, $scope, $location, $window, $http, $stateParams, $state, UserProfile, WizardHandler, $filter, DTOptionsBuilder, DTColumnBuilder, $compile, $uibModal, SweetAlert, moment) {

        var vm = this;

        vm.isSpeaker = [];
        $scope.controllerScope = $scope;
        vm.photoPath = 'api/Upload/UploadFile?uploadFile=Attachment';
        vm.themes = [{ url: $rootScope.app.baseURL + 'viewEvent/Layout1/index.html?id=', nameEn: 'Theme 1', nameAr: 'Theme 1' },
        { url: $rootScope.app.baseURL + 'viewEvent/Layout2/index.html?id=', nameEn: 'Theme 2', nameAr: 'Theme 2' }];
        vm.enableUpload = true;
        vm.uploadedphotos = [];
        vm.uploadedPresentation = [];
        vm.presentation = {};
        vm.package = {};
        vm.translateFilter = $filter('translate');
        vm.numberFilter = $filter('number');
        vm.localizeString = $filter('localizeString');
        vm.isReadOnly = false;
        vm.showRequiredError = false;
        vm.activeTab = [false, false, false, false, false, false];
        vm.activeStep = 1;
        $scope.EventId = $location.search()['id'];
        vm.updateSurvey = 0;
        vm.showLimitMsg = false;
        vm.event = {
            eventTypeId: 14,
            nameEn: "",
            nameAr: "",
            descriptionEn: "",
            descriptionAr: "",
            startDate: null,
            endDate: null,
            agendaSessions: [],
            agenda: [],
            eventAddresses: [],
            packages: [],
            eventCompanies: [],
            eventPersons: [],
            photos: [],
            presentations: [],
            themeURL: vm.themes[0].url,
            participantsLimit: 50,
            participantsRegistrationTypeId: null
        };
        vm.currencyTypes = [
            {
                id: 1,
                nameEn: "Dollar",
                nameAr: "دولار",
                symbol: "$"
            },
            {
                id: 1,
                nameEn: "Dirham",
                nameAr: "درهم",
                symbol: "AED"
            }
        ];
        vm.selectTab = function (index) {
            vm.activeTab[index] = true;
            if ($scope.EventId > 0 && index == 1) {
                $scope.showLimitMsg = true;
            }
        };

        vm.selectTab(1);

        vm.checkFees = function () {
            if (vm.event.participantFees < 0) {
                delete vm.event.participantFees;
            }
        };

        //Date Popup Options
        vm.clearStartDate = function () {
            vm.event.startDate = null;
        };

        vm.clearEndDate = function () {
            vm.event.endDate = null;
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

        vm.disabled = function (date, mode) {
            if (vm.event.id == undefined) {
                var today = new Date();
                return date < today;
            }
            return true;
        };

        vm.enddisabled = function (date, mode) {
            return date < new Date(vm.event.startDate);
        };

        vm.openStartDatePopup = function () {
            vm.startDate.opened = true;
        };

        vm.openEndDatePopup = function () {
            vm.endDate.opened = true;
        };

        vm.setDate = function (year, month, day) {
            vm.event.startDate = new Date(year, month, day);
        };

        vm.format = 'dd-MMMM-yyyy';


        vm.toggleMin = function () {
            vm.minDate = start;
            //vm.minDate = vm.minDate ? null : new Date();
        };

        vm.startDate = {
            opened: false
        };

        vm.endDate = {
            opened: false
        };
        //END

        $http.get($rootScope.app.httpSource + 'api/EventTypes')
            .then(function (response) {
                vm.eventTypes = response.data;
            });

        $http.get($rootScope.app.httpSource + 'api/Lookup/GetBackgrounds')
            .then(function (response) {
                vm.backgroundThemes = response.data;
            });

        $http.get($rootScope.app.httpSource + 'api/SponserTypes/Get')
            .then(function (response) {
                vm.sponserTypes = response.data;
            }, function (response) { });

        $http.get($rootScope.app.httpSource + 'api/Lookup/GetParticipantsRegistrationType')
            .then(function (response) {
                vm.participantsRegistrationTypes = response.data;
            }, function (response) { });

        $http.get($rootScope.app.httpSource + 'api/User/Get')
            .then(function (response) {
                vm.user = response.data;
                vm.user.roleName = localStorage.getItem('roleName');
                if (vm.user.limitForEventsReached == true && ($stateParams.id == undefined || $stateParams.id == '')) {
                    var translate = $filter('translate');
                    SweetAlert.swal(translate('general.limitReached'), translate('general.limitReachedMsg'), "error");
                    $state.go("app.home");
                }
            });

        $http.get($rootScope.app.httpSource + 'api/Event/GetPaidPackages')
            .then(function (response) {
                vm.userSubscriptions = response.data;
            }, function (response) { });

        vm.checkEventDate = function () {
            if (new Date(vm.event.startDate) > new Date(vm.event.endDate)) {
                delete vm.event.endDate;
            }
        };

        vm.checkSubsription = function () {
            var eventId = vm.event.id != undefined ? vm.event.id : 0;
            $http.get($rootScope.app.httpSource + 'api/Event/checkSubsription?id=' + vm.event.userSubscriptionId + '&eventId=' + eventId)
                .then(function (response) {
                    var data = response.data;
                    if (data > 0) {
                        var translate = $filter('translate');
                        SweetAlert.swal(translate('general.existing'), translate('general.packageSelectedMsg'), "error");
                        delete vm.event.userSubscriptionId;
                    }
                    else {
                        vm.event.hasSponser = true;
                        vm.event.hasExhibitor = true;
                        vm.event.hasspeakerReg = true;
                        var userSubscription = vm.event.userSubscription != null ? vm.event.userSubscription : $filter('filter')(vm.userSubscriptions, { id: vm.event.userSubscriptionId }, true)[0];
                        if (userSubscription != null) {
                            if (userSubscription.eventPackageId != 2 && userSubscription.eventPackageId != 3 &&
                                userSubscription.eventPackageId != 4) {
                                vm.event.hasSponser = false;
                            }
                            if (userSubscription.eventPackageId != 3 && userSubscription.eventPackageId != 4) {
                                vm.event.hasExhibitor = false;
                                vm.event.hasspeakerReg = false;
                                vm.event.hasSponser = false;
                            }
                            if (userSubscription.eventPackageId == 5) {
                                vm.event.hasPayment = true;
                            }
                            vm.eventPackageId = userSubscription.eventPackageId;
                        }
                    }
                }, function (response) { });
        };

        //Agenda Session Datatable
        vm.agendaSessionDt = {};
        vm.agendaSessionDt.Instance = {};
        vm.agendaSessionDt.actionsHtml = function (data, type, full, meta) {
            var htmlSection = '';

            htmlSection = '<div class="list-icon"><div class="btn btn-icon waves-effect waves-light btn-info" ng-click="vm.agendaSessionDt.edit(\'lg\',' +
                data.id + ')"><em class="fa fa-pencil" style="cursor:pointer" uib-tooltip="' +
                vm.translateFilter('general.edit') + '"></em></div><div class="btn btn-icon waves-effect waves-light btn-info" ng-click="vm.agendaSessionDt.delete(' +
                data.id + ', $event)"><em class="fa fa-trash" style="cursor:pointer" uib-tooltip="' +
                vm.translateFilter('general.delete') + '"></em></div></div>';

            return htmlSection;
        };

        vm.agendaSessionDt.Columns = [
            DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('session.nameEn')).renderWith(function (data, type) {
                if (data.nameEn != null) {
                    return data.nameEn;
                }
                return '';
            }),
            DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('session.nameAr')).renderWith(function (data, type) {
                if (data.nameAr != null) {
                    return data.nameAr;
                }
                return '';
            }),
            DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('general.actions')).notSortable()
                .renderWith(vm.agendaSessionDt.actionsHtml).withOption('width', '15%')];

        vm.agendaSessionDt.open = function (size) {
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Event/Session/session.html',
                controller: 'sessionController',
                size: size,
                keyboard: false,
                backdrop: 'static',
                resolve: {
                    sessionObj: function () {
                        return null;
                    }
                }
            });

            modalInstance.result.then(function (session) {

                session.id = vm.event.agendaSessions.length + 1;
                vm.event.agendaSessions.push(session);
                vm.agendaSessionDt.Instance.rerender();
                vm.save();
            }, function () {
                //state.text('Modal dismissed with Cancel status');
            });
        };

        vm.agendaSessionDt.edit = function (size, sessionId) {
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Event/Session/session.html',
                controller: 'sessionController',
                size: size,
                keyboard: false,
                backdrop: 'static',
                resolve: {
                    sessionObj: function () {
                        return $filter('filter')(vm.event.agendaSessions, { id: sessionId }, true)[0];
                    }
                }
            });

            modalInstance.result.then(function (session) {

                for (var i = 0; i < vm.event.agendaSessions.length; i++) {
                    if (vm.event.agendaSessions[i].id == session.id) {
                        vm.event.agendaSessions[i] = session;
                        break;
                    }
                }
                vm.agendaSessionDt.Instance.rerender();
                vm.save();
            }, function () {
                //state.text('Modal dismissed with Cancel status');
            });
        };

        vm.agendaSessionDt.delete = function (sessionId, event) {
            var index;
            var tempStore;

            if (sessionId == 0 || sessionId == undefined) {
                index = vm.agendaSessionDt.Instance.DataTable.rows({ order: 'applied' }).nodes().indexOf(event.currentTarget.parentNode.parentNode.parentNode);
                tempStore = vm.event.agendaSessions[index];
                //vm.event.agendaSessions.splice(index, 1);
            }
            else {
                index = vm.event.agendaSessions.indexOf($filter('filter')(vm.event.agendaSessions, { id: sessionId }, true)[0]);
                tempStore = $filter('filter')(vm.event.agendaSessions, { id: sessionId }, true)[0];
                //vm.event.agendaSessions.splice(index, 1);
            }
            var translate = $filter('translate');
            vm.agendaSessionDt.Instance.rerender();

            SweetAlert.swal({
                title: translate('session.deleteSession'),
                text: translate('general.confirmDeleteInfo'),
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: translate('general.confirmDeleteBtn'),
                cancelButtonText: translate('general.restoreBtn'),
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        //delete

                        for (var i = 0; i < vm.event.agenda.length; i++) {
                            if (vm.event.agenda[i].sessionId === sessionId) {
                                vm.event.agenda.splice(i, 1);
                            }
                        }

                        vm.event.agendaSessions.splice(index, 1);
                        vm.save();
                        vm.agendaDt.Instance.rerender();
                        vm.agendaSessionDt.Instance.rerender();

                        SweetAlert.swal(translate('general.confirmDeleteBtn'), translate('general.deleteMessage'), "success");

                    } else {
                        //vm.event.agendaSessions.splice(index, 0, tempStore);
                        SweetAlert.swal(translate('general.restoreBtn'), translate('general.restoreMessage'), "success");
                        vm.agendaSessionDt.Instance.rerender();
                    }
                });
        };

        //Agenda Datatable
        vm.agendaDt = {};
        vm.agendaDt.Instance = {};
        vm.agendaDt.actionsHtml = function (data, type, full, meta) {
            var htmlSection = '';

            htmlSection = '<div class="list-icon"><div class="btn btn-icon waves-effect waves-light btn-info" ng-click="vm.agendaDt.edit(\'lg\',' +
                data.id + ')"><em class="fa fa-pencil" style="cursor:pointer" uib-tooltip="' +
                vm.translateFilter('general.edit') + '"></em></div><div class="btn btn-icon waves-effect waves-light btn-info" ng-click="vm.agendaDt.delete(' +
                data.id + ', $event)"><em class="fa fa-trash" style="cursor:pointer" uib-tooltip="' +
                vm.translateFilter('general.delete') + '"></em></div></div>';

            return htmlSection;
        };

        vm.agendaDt.Columns = [
            DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('agenda.title'))
                .renderWith(function (data, type) {
                    var localizeString = vm.localizeString(data, 'titleEn', 'titleAr');
                    return localizeString;
                }),
            DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('agenda.description'))
                .renderWith(function (data, type) {
                    var localizeString = vm.localizeString(data, 'descriptionEn', 'descriptionAr');
                    return localizeString;
                }),
            DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('agenda.session'))
                .renderWith(function (data, type) {
                    var session = vm.event.agendaSessions.filter(function (item) { return item.id == data.sessionId })[0];
                    if (session) {
                        return vm.localizeString(session, 'nameEn', 'nameAr');
                    } else {
                        return '';
                    }
                }),
            DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('event.participantsLimit'))
                .renderWith(function (data, type) {
                    if (vm.event.participantsRegistrationTypeId == 1) {
                        return vm.numberFilter(vm.event.participantsLimit);
                    } else {
                        return vm.numberFilter(data.participantsLimit);
                    }
                }),
            DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('agenda.date')).renderWith(function (data, type) {
                //return '<span>' + $filter('date')(data.startDate, 'EEEE') + " " + $filter('date')(data.startDate, 'longDate') + " - " + $filter('date')(data.endDate, 'EEEE') + " " + $filter('date')(data.endDate, 'longDate') + '</span>';
                return '<span>' + $filter('date')(data.startDate, 'EEEE') + " " + $filter('date')(data.startDate, 'longDate') + '</span>';
            }),
            DTColumnBuilder.newColumn('fromTime').withTitle(vm.translateFilter('agenda.fromTime')),
            DTColumnBuilder.newColumn('toTime').withTitle(vm.translateFilter('agenda.toTime')),
            DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('general.actions')).notSortable()
                .renderWith(vm.agendaDt.actionsHtml).withOption('width', '15%')];

        vm.agendaDt.open = function (size) {
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Event/Agenda/agenda.html',
                controller: 'agendaController',
                size: size,
                keyboard: false,
                backdrop: 'static',
                resolve: {
                    agendaObj: function () {
                        return null;
                    },
                    agendaStartDate: function () {
                        return vm.event.startDate;
                    },
                    agendaEndDate: function () {
                        return vm.event.endDate;
                    },
                    agendaSessions: function () {
                        return vm.event.agendaSessions;
                    },
                    speakers: function () {
                        return vm.event.eventPersons.filter(function (item) { return item.personTypeId == 1; });
                    },
                    participantsRegistrationTypeId: function () {
                        return vm.event.participantsRegistrationTypeId;
                    }
                }
            });

            modalInstance.result.then(function (agenda) {
                agenda.id = vm.event.agenda.length + 1;
                vm.event.agenda.push(agenda);
                vm.agendaDt.Instance.rerender();
                vm.save();
            }, function () {
                //state.text('Modal dismissed with Cancel status');
            });
        };

        vm.agendaDt.edit = function (size, agendaId) {
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Event/Agenda/agenda.html',
                controller: 'agendaController',
                size: size,
                keyboard: false,
                backdrop: 'static',
                resolve: {
                    agendaObj: function () {
                        return $filter('filter')(vm.event.agenda, { id: agendaId }, true)[0];
                    },
                    agendaStartDate: function () {
                        return vm.event.startDate;
                    },
                    agendaEndDate: function () {
                        return vm.event.endDate;
                    },
                    agendaSessions: function () {
                        return vm.event.agendaSessions;
                    },
                    speakers: function () {
                        return vm.event.eventPersons.filter(function (item) { return item.personTypeId == 1; });
                    },
                    participantsRegistrationTypeId: function () {
                        return vm.event.participantsRegistrationTypeId;
                    }
                }
            });

            modalInstance.result.then(function (agenda) {

                for (var i = 0; i < vm.event.agenda.length; i++) {
                    if (vm.event.agenda[i].id == agenda.id) {
                        vm.event.agenda[i] = agenda;
                        break;
                    }
                }
                vm.agendaDt.Instance.rerender();
                vm.save();
            }, function () {
                //state.text('Modal dismissed with Cancel status');
            });
        };

        vm.agendaDt.delete = function (agendaId, event) {

            var index;
            var tempStore;

            if (agendaId == 0 || agendaId == undefined) {
                index = vm.agendaDt.Instance.DataTable.rows({ order: 'applied' }).nodes().indexOf(event.currentTarget.parentNode.parentNode.parentNode);
                tempStore = vm.event.agenda[index];
                vm.event.agenda.splice(index, 1);
            }
            else {
                index = vm.event.agenda.indexOf($filter('filter')(vm.event.agenda, { id: agendaId }, true)[0]);
                tempStore = $filter('filter')(vm.event.agenda, { id: agendaId }, true)[0];
                vm.event.agenda.splice(index, 1);
            }
            var translate = $filter('translate');
            vm.agendaDt.Instance.rerender();

            SweetAlert.swal({
                title: translate('general.confirmDelete'),
                text: translate('general.confirmDeleteInfo'),
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: translate('general.confirmDeleteBtn'),
                cancelButtonText: translate('general.restoreBtn'),
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        //delete
                        SweetAlert.swal(translate('general.confirmDeleteBtn'), translate('general.deleteMessage'), "success");
                        vm.agendaDt.Instance.rerender();
                        vm.save();
                    } else {

                        vm.event.agenda.splice(index, 0, tempStore);
                        SweetAlert.swal(translate('general.restoreBtn'), translate('general.restoreMessage'), "success");
                        vm.agendaDt.Instance.rerender();
                    }
                });
        };

        //Locations Datatable
        vm.locationDt = {};
        vm.locationDt.dtInstance = {};
        vm.locationDt.actionsHtml = function (data, type, full, meta) {
            var htmlSection = '';

            htmlSection = '<div class="list-icon"><div class="btn btn-icon waves-effect waves-light btn-info" ng-click="vm.locationDt.edit(\'lg\',' +
                data.id + ')"><em class="fa fa-pencil" style="cursor:pointer" uib-tooltip="' +
                vm.translateFilter('general.edit') + '"></em></div><div class="btn btn-icon waves-effect waves-light btn-info" ng-click="vm.locationDt.delete(' +
                data.id + ', $event)"><em class="fa fa-trash" style="cursor:pointer" uib-tooltip="' +
                vm.translateFilter('general.delete') + '"></em></div></div>';

            return htmlSection;
        };

        vm.locationDt.dtColumns = [
            DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('address.country'))
                .renderWith(function (data, type) {
                    return vm.localizeString(data.address.country, 'nameEn', 'nameAr');
                }),
            DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('address.city'))
                .renderWith(function (data, type) {
                    return vm.localizeString(data.address.state, 'nameEn', 'nameAr');
                }),
            DTColumnBuilder.newColumn('address.street').withTitle(vm.translateFilter('address.Street')),
            DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('general.actions')).notSortable()
                .renderWith(vm.locationDt.actionsHtml).withOption('width', '15%')];

        vm.locationDt.open = function (size) {
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Event/eventVenueLocation/eventVenue.html',
                controller: 'eventVenueController',
                size: size,
                keyboard: false,
                backdrop: 'static',
                resolve: {
                    location: function () {
                        return null;
                    }
                }
            });

            modalInstance.result.then(function (location) {
                location.id = vm.event.eventAddresses.length + 1;
                vm.event.eventAddresses.push(location);
                vm.locationDt.dtInstance.rerender();
                vm.save();
            }, function () {
                //state.text('Modal dismissed with Cancel status');
            });
        };

        vm.locationDt.edit = function (size, addressId) {

            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Event/eventVenueLocation/eventVenue.html',
                controller: 'eventVenueController',
                size: size,
                keyboard: false,
                backdrop: 'static',
                resolve: {
                    location: function () {
                        return $filter('filter')(vm.event.eventAddresses, { id: addressId }, true)[0];
                    }
                }
            });

            modalInstance.result.then(function (location) {
                for (var i = 0; i < vm.event.eventAddresses.length; i++) {
                    if (vm.event.eventAddresses[i].id == location.id) {
                        vm.event.eventAddresses[i] = location;
                        break;
                    }
                }
                vm.locationDt.dtInstance.rerender();
                vm.save();
            }, function () {
                //state.text('Modal dismissed with Cancel status');
            });
        };

        vm.locationDt.delete = function (addressId, event) {


            var index;
            var tempStore;

            if (addressId == 0 || addressId == undefined) {
                index = vm.locationDt.dtInstance.DataTable.rows({ order: 'applied' }).nodes().indexOf(event.currentTarget.parentNode.parentNode.parentNode);
                tempStore = vm.event.eventAddresses[index];
                vm.event.eventAddresses.splice(index, 1);
            }
            else {
                index = vm.event.eventAddresses.indexOf($filter('filter')(vm.event.eventAddresses, { id: addressId }, true)[0]);
                tempStore = $filter('filter')(vm.event.eventAddresses, { id: addressId }, true)[0];
                vm.event.eventAddresses.splice(index, 1);
            }
            var translate = $filter('translate');
            vm.locationDt.dtInstance.rerender();

            SweetAlert.swal({
                title: translate('general.confirmDelete'),
                text: translate('general.confirmDeleteInfo'),
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: translate('general.confirmDeleteBtn'),
                cancelButtonText: translate('general.restoreBtn'),
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        //delete
                        SweetAlert.swal(translate('general.confirmDeleteBtn'), translate('general.deleteMessage'), "error");
                        vm.locationDt.dtInstance.rerender();
                        vm.save();
                    } else {

                        vm.event.eventAddresses.splice(index, 0, tempStore);
                        SweetAlert.swal(translate('general.restoreBtn'), translate('general.restoreMessage'), "success");
                        vm.locationDt.dtInstance.rerender();
                    }
                });
        };


        //Package Datatable
        vm.packageDt = {};
        vm.packageDt.dtInstance = {};
        vm.packageDt.actionsHtml = function (data, type, full, meta) {
            var htmlSection = '';

            htmlSection = '<div class="list-icon"><div class="btn btn-icon waves-effect waves-light btn-info" ng-click="vm.packageDt.edit(' +
                data.id + ')"><em class="fa fa-pencil" style="cursor:pointer" uib-tooltip="' +
                vm.translateFilter('general.edit') + '"></em></div><div class="btn btn-icon waves-effect waves-light btn-info" ng-click="vm.packageDt.delete(' +
                data.id + ', $event)"><em class="fa fa-trash" style="cursor:pointer" uib-tooltip="' +
                vm.translateFilter('general.delete') + '"></em></div></div>';

            return htmlSection;
        };

        vm.packageDt.dtColumns = [
            DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('event.sponserType')).notSortable()
                .renderWith(function (data, type) {
                    var localizeString = vm.localizeString(data.sponserType, 'nameEn', 'nameAr');
                    return localizeString;
                }),
            DTColumnBuilder.newColumn('cost').withTitle(vm.translateFilter("package.cost")),
            DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('general.actions')).notSortable()
                .renderWith(vm.packageDt.actionsHtml).withOption('width', '15%')];

        vm.addPackage = function (formName) {
            var data = "";
            vm.event.packages.forEach(function (value, key) {
                if ((value.sponserType.nameEn == vm.package.sponserType.nameEn || value.sponserType.nameAr == vm.package.sponserType.nameAr)) {
                    if (vm.package.id > 0)
                        data = "";
                    else
                        data = "existing";
                }
            });

            if (data) {
                vm.showRequiredError = true;
                SweetAlert.swal($filter('translate')('general.error'), $filter('translate')('general.packageMsg'), "error");
            }
            else {
                if (vm.package.cost && vm.package.sponserType && vm.package.benefits) {
                    if (!vm.package.id) {
                        //vm.package.id = 0;
                        var packageId = vm.event.packages.length > 0 ? vm.event.packages[0].id : 1;
                        vm.package.id = packageId + Math.floor(Math.random() * 2000);
                        vm.package.eventId = vm.event.id;
                        vm.event.packages.push(vm.package);
                    }
                    else {
                        for (var i = 0; i < vm.event.packages.length; i++) {
                            if (vm.package.id == vm.event.packages[i].id) {
                                vm.event.packages[i] = vm.package;
                                break;
                            }
                        }
                    }
                    vm.showRequiredError = false;
                    vm.packageDt.dtInstance.rerender();
                    vm.package = {};
                    formName.$setPristine();
                } else {
                    vm.showRequiredError = true;
                }
            }
        };

        vm.packageDt.edit = function (packageId) {
            vm.package = vm.event.packages.filter(function (item) { return item.id == packageId; })[0];
        };

        vm.clearPackage = function () {
            vm.package = {};
        };

        vm.packageDt.delete = function (packageId, event) {


            var index;
            var tempStore;

            if (packageId == 0 || packageId == undefined) {
                index = vm.packageDt.dtInstance.DataTable.rows({ order: 'applied' }).nodes().indexOf(event.currentTarget.parentNode.parentNode.parentNode);
                tempStore = vm.event.packages[index];
            }
            else {
                index = vm.event.packages.indexOf($filter('filter')(vm.event.packages, { id: packageId }, true)[0]);
                tempStore = $filter('filter')(vm.event.packages, { id: packageId }, true)[0];
            }
            var translate = $filter('translate');
            vm.packageDt.dtInstance.rerender();

            SweetAlert.swal({
                title: translate('general.confirmDelete'),
                text: translate('general.confirmDeleteInfo'),
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: translate('general.confirmDeleteBtn'),
                cancelButtonText: translate('general.restoreBtn'),
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        //delete
                        vm.event.packages.splice(index, 1);
                        SweetAlert.swal(translate('general.confirmDeleteBtn'), translate('general.deleteMessage'), "success");
                        vm.packageDt.dtInstance.rerender();
                    } else {
                        SweetAlert.swal(translate('general.restoreBtn'), translate('general.restoreMessage'), "success");
                        vm.packageDt.dtInstance.rerender();
                    }
                });
        };

        //Presentation Datatable
        vm.presentationDt = {};
        vm.presentationDt.dtInstance = {};
        vm.presentationDt.actionsHtml = function (data, type, full, meta) {
            var htmlSection = '';

            htmlSection = '<div class="list-icon"><div class="btn btn-icon waves-effect waves-light btn-info" ng-click="vm.presentationDt.edit(' +
                data.id + ')"><em class="fa fa-pencil" style="cursor:pointer" uib-tooltip="' +
                vm.translateFilter('general.edit') + '"></em></div><div class="btn btn-icon waves-effect waves-light btn-info" ng-click="vm.presentationDt.delete(' +
                data.id + ', $event)"><em class="fa fa-trash" style="cursor:pointer" uib-tooltip="' +
                vm.translateFilter('general.delete') + '"></em></div></div>';

            return htmlSection;
        };

        vm.presentationDt.dtColumns = [
            DTColumnBuilder.newColumn('nameEn').withTitle(vm.translateFilter('event.nameEn')),
            DTColumnBuilder.newColumn('nameAr').withTitle(vm.translateFilter('event.nameAr')),
            DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('event.presentationFiles')).renderWith(
                function (data) {
                    return '<a href="' + data.attachmentUrlFulPath + '"><i class="fa fa-download"></i></a>';
                }
            ),
            DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('general.actions')).notSortable()
                .renderWith(vm.presentationDt.actionsHtml).withOption('width', '15%')];

        vm.presentationDt.edit = function (presentationId) {
            vm.presentation = vm.event.presentations.filter(function (item) { return item.id == presentationId; })[0];
        };

        vm.presentationDt.delete = function (presentationId, event) {
            var index;
            var tempStore;

            if (presentationId == 0 || presentationId == undefined) {
                index = vm.presentationDt.dtInstance.DataTable.rows({ order: 'applied' }).nodes().indexOf(event.currentTarget.parentNode.parentNode.parentNode);
                tempStore = vm.event.presentations[index];
                vm.event.presentations.splice(index, 1);
            }
            else {
                index = vm.event.presentations.indexOf($filter('filter')(vm.event.presentations, { id: presentationId }, true)[0]);
                tempStore = $filter('filter')(vm.event.presentations, { id: presentationId }, true)[0];
                vm.event.presentations.splice(index, 1);
            }
            var translate = $filter('translate');
            vm.presentationDt.dtInstance.rerender();

            SweetAlert.swal({
                title: translate('general.confirmDelete'),
                text: translate('general.confirmDeleteInfo'),
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: translate('general.confirmDeleteBtn'),
                cancelButtonText: translate('general.restoreBtn'),
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        //delete
                        SweetAlert.swal(translate('general.confirmDeleteBtn'), translate('general.deleteMessage'), "success");
                        vm.presentationDt.dtInstance.rerender();
                        vm.save();
                    } else {

                        vm.event.agenda.splice(index, 0, tempStore);
                        SweetAlert.swal(translate('general.restoreBtn'), translate('general.restoreMessage'), "success");
                        vm.presentationDt.dtInstance.rerender();
                    }
                });
        };

        vm.addCompany = function (size, companyTypeId) {
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Event/Company/company.html',
                controller: 'companyController',
                size: size,
                keyboard: false,
                backdrop: 'static',
                resolve: {
                    eventId: function () {
                        return vm.event.id;
                    },
                    companyTypeId: function () {
                        return companyTypeId;
                    },
                    eventCompany: function () {
                        return null;
                    },
                    sponserPackages: function () {
                        return vm.event.packages;
                    }
                }
            });

            modalInstance.result.then(function (eventCompany) {
                eventCompany.editMode = true;
                vm.event.eventCompanies.push(eventCompany);
            }, function () {
                //state.text('Modal dismissed with Cancel status');
            });
        };

        vm.editCompany = function (size, eventCompany) {
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Event/Company/company.html',
                controller: 'companyController',
                size: size,
                keyboard: false,
                backdrop: 'static',
                resolve: {
                    eventId: function () {
                        return vm.event.id;
                    },
                    companyTypeId: function () {
                        return eventCompany.companyTypeId;
                    },
                    eventCompany: function () {
                        return eventCompany;
                    },
                    sponserPackages: function () {
                        return vm.event.packages;
                    }
                }
            });

            modalInstance.result.then(function (eventCompany) {
                eventCompany.editMode = true;
                for (var i = 0; i < vm.event.eventCompanies.length; i++) {
                    if (vm.event.eventCompanies[i].id == eventCompany.id) {
                        vm.event.eventCompanies[i] = eventCompany;
                        vm.save();
                        break;
                    }
                }
            }, function () {
                //state.text('Modal dismissed with Cancel status');
            });
        };

        vm.searchPackage = function (size) {
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/package/search/searchPackage.html',
                controller: 'searchPackageController',
                size: size,
                keyboard: false,
                backdrop: 'static'
            });

            modalInstance.result.then(function (eventCompanies) {
                for (var id in eventCompanies) {
                    var exists = $filter('filter')(vm.event.eventCompanies, { company: { id: eventCompanies[id].company.id } }, true);
                    if (exists.length == 0) {
                        eventCompanies[id].company.companyTypeId = eventCompanies[id].company.companyType.id;
                        delete eventCompanies[id].company.companyType;
                        vm.event.eventCompanies.push(eventCompanies[id]);
                        vm.save();
                    }
                }
            }, function () {
                //state.text('Modal dismissed with Cancel status');
            });
        };

        vm.searchCompany = function (size, companyTypeId) {
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Event/searchcompany/searchcompany.html',
                controller: 'searchCompanyController',
                size: size,
                keyboard: false,
                backdrop: 'static',
                resolve: {
                    eventId: function () {
                        return vm.event.id;
                    },
                    eventCompanies: function () {
                        return $filter('filter')(vm.event.eventCompanies, { companyTypeId: companyTypeId }, true);
                    },
                    companyTypeId: function () {
                        return companyTypeId;
                    },
                    eventPackages: function () {
                        return vm.event.packages;
                    }
                }
            });

            modalInstance.result.then(function (updatedCompanys) {
                // Remove
                vm.event.eventCompanies = vm.event.eventCompanies.filter(function (item) { return item.companyTypeId !== companyTypeId });

                // Add
                for (var i = 0; i < updatedCompanys.length; i++) {
                    vm.event.eventCompanies.push(updatedCompanys[i]);
                }
                SweetAlert.swal($filter('translate')('general.ok'), $filter('translate')('general.eventMsg'), "success");
                vm.save();
            }, function () {
                //state.text('Modal dismissed with Cancel status');
            });
        };

        vm.removeCompany = function (eventCompany) {
            var tempStore;
            var index = vm.event.eventCompanies.indexOf(eventCompany);
            tempStore = vm.event.eventCompanies[index];

            var translate = $filter('translate');

            SweetAlert.swal({
                title: translate('general.confirmDelete'),
                text: translate('general.confirmDeleteInfo'),
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: translate('general.confirmDeleteBtn'),
                cancelButtonText: translate('general.restoreBtn'),
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        vm.event.eventCompanies.splice(index, 1);
                        SweetAlert.swal(translate('general.confirmDeleteBtn'), translate('general.deleteMessage'), "success");
                    } else {
                        //vm.event.eventCompanies.splice(index, 0, tempStore);
                        SweetAlert.swal(translate('general.restoreBtn'), translate('general.restoreMessage'), "success");
                    }
                });
        };

        vm.approveCompany = function (eventCompany) {
            var tempStore;
            var index = vm.event.eventCompanies.indexOf(eventCompany);
            var translate = $filter('translate');
            SweetAlert.swal({
                title: translate('general.confirm'),
                text: translate('general.approveConfirmMSg'),
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: translate('general.confirm'),
                cancelButtonText: translate('general.cancel'),
                closeOnConfirm: true,
                closeOnCancel: true
            },
                function (isConfirm) {
                    if (isConfirm) {
                        vm.event.eventCompanies[index].isApproved = true;
                        vm.save();
                    }
                });
        };

        vm.rejectCompany = function (eventCompany) {
            var tempStore;
            var index = vm.event.eventCompanies.indexOf(eventCompany);
            var translate = $filter('translate');
            SweetAlert.swal({
                title: translate('general.confirm'),
                text: translate('general.rejectConfirmMSg'),
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: translate('general.confirm'),
                cancelButtonText: translate('general.cancel'),
                closeOnConfirm: true,
                closeOnCancel: true
            },
                function (isConfirm) {
                    if (isConfirm) {
                        vm.event.eventCompanies[index].isApproved = false;
                        vm.save();
                    }
                });
        };

        vm.approvePerson = function (eventPerson) {
            var tempStore;
            var index = vm.event.eventPersons.indexOf(eventPerson);
            var translate = $filter('translate');
            SweetAlert.swal({
                title: translate('general.confirm'),
                text: translate('general.approveConfirmMSg'),
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: translate('general.confirm'),
                cancelButtonText: translate('general.cancel'),
                closeOnConfirm: true,
                closeOnCancel: true
            },
                function (isConfirm) {
                    if (isConfirm) {
                        vm.event.eventPersons[index].isApproved = true;
                        vm.save();
                    }
                });
        };

        vm.rejectPerson = function (eventPerson) {
            var tempStore;
            var index = vm.event.eventPersons.indexOf(eventPerson);
            var translate = $filter('translate');
            SweetAlert.swal({
                title: translate('general.confirm'),
                text: translate('general.rejectConfirmMSg'),
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: translate('general.confirm'),
                cancelButtonText: translate('general.cancel'),
                closeOnConfirm: true,
                closeOnCancel: true
            },
                function (isConfirm) {
                    if (isConfirm) {
                        vm.event.eventPersons[index].isApproved = false;
                        vm.save();
                    }
                });
        };

        vm.searchSpeaker = function (size, personTypeId) {
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Event/searchSpeaker/searchSpeaker.html',
                controller: 'searchSpeakerController',
                size: size,
                keyboard: false,
                backdrop: 'static',
                resolve: {
                    eventId: function () {
                        return vm.event.id;
                    },
                    eventPersons: function () {
                        return $filter('filter')(vm.event.eventPersons, { personTypeId: personTypeId }, true);
                    },
                    personTypeId: function () {
                        return personTypeId;
                    }
                }
            });

            modalInstance.result.then(function (updatedSpeakers) {
                // Remove
                vm.event.eventPersons = vm.event.eventPersons.filter(function (item) { return item.personTypeId !== personTypeId; });

                // Add
                for (var i = 0; i < updatedSpeakers.length; i++) {
                    vm.event.eventPersons.push(updatedSpeakers[i]);
                }
                SweetAlert.swal($filter('translate')('general.ok'), $filter('translate')('general.eventMsg'), "success");
                vm.save();
            }, function () {
                //state.text('Modal dismissed with Cancel status');
            });
        };

        vm.removeSpeaker = function (eventPerson) {
            var tempStore;
            var index = vm.event.eventPersons.indexOf(eventPerson);
            tempStore = vm.event.eventPersons[index];

            var translate = $filter('translate');

            SweetAlert.swal({
                title: translate('general.confirmDelete'),
                text: translate('general.confirmDeleteInfo'),
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: translate('general.confirmDeleteBtn'),
                cancelButtonText: translate('general.restoreBtn'),
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        vm.event.eventPersons.splice(index, 1);
                        SweetAlert.swal(translate('general.confirmDeleteBtn'), translate('general.deleteMessage'), "success");
                    } else {

                        //vm.event.eventPersons.splice(index, 0, tempStore);
                        SweetAlert.swal(translate('general.restoreBtn'), translate('general.restoreMessage'), "success");
                    }
                });
        };

        vm.addSpeaker = function (size, personTypeId) {
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Event/Person/person.html',
                controller: 'personController',
                size: size,
                keyboard: false,
                backdrop: 'static',
                resolve: {
                    eventId: function () {
                        return vm.event.id;
                    },
                    eventSpeaker: function () {
                        return null;
                    },
                    personTypeId: function () {
                        return personTypeId;
                    }
                }
            });

            modalInstance.result.then(function (eventPerson) {

                eventPerson.editMode = true;
                vm.event.eventPersons.push(eventPerson);
                vm.save();
            }, function () {
                //state.text('Modal dismissed with Cancel status');
            });
        };

        vm.editSpeaker = function (size, eventPerson) {
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Event/Person/person.html',
                controller: 'personController',
                size: size,
                keyboard: false,
                backdrop: 'static',
                resolve: {
                    eventId: function () {
                        return vm.event.id;
                    },
                    eventSpeaker: function () {
                        return eventPerson;
                    },
                    personTypeId: function () {
                        return eventPerson.personTypeId;
                    }
                }
            });

            modalInstance.result.then(function (eventPerson) {
                eventPerson.editMode = true;
                for (var i = 0; i < vm.event.eventPersons.length; i++) {
                    if (vm.event.eventPersons[i].id == eventPerson.id) {
                        vm.event.eventPersons[i] = eventPerson;
                        break;
                    }
                }
                vm.save();
            }, function () {
                //state.text('Modal dismissed with Cancel status');
            });
        }

        vm.uploadAll = function (uploadAttachment) {
            var photo = {
                eventId: vm.event.id,
                documentPath: uploadAttachment.documentPath,
                documentFullPath: uploadAttachment.documentFullPath
            };
            vm.uploadedphotos.push(photo);
            //vm.event.photos.push(photo);
        };

        vm.deletePhoto = function (index) {
            vm.uploadedphotos.splice(index, 1);
            //vm.event.photos.splice(index, 1);
        };

        vm.addPresentation = function (formName) {

            if (vm.presentation.nameEn && vm.presentation.nameAr && vm.presentation.attachmentUrlFulPath) {
                if (!vm.presentation.id) {
                    vm.presentation.id = vm.event.presentations.length + 1;
                    vm.event.presentations.push(vm.presentation);
                }
                else {
                    for (var i = 0; i < vm.event.presentations.length; i++) {
                        if (vm.presentation.id == vm.event.presentations[i].id) {
                            vm.event.presentations[i] = vm.presentation;
                            break;
                        }
                    }
                }
                vm.showRequiredError = false;
                vm.presentationDt.dtInstance.rerender();
                formName.$setPristine();
                vm.presentation = {};
            } else {
                vm.showRequiredError = true;
            }
        };

        vm.hasSponsorChanged = function () {
            if (vm.event.sponsorsOnlineRegister == true && vm.event.hasSponsors == false) {
                vm.event.sponsorsOnlineRegister = false;
            }
        };

        vm.hasExhibitorsChanged = function () {
            if (vm.event.exhibitorsOnlineRegister == true && vm.event.hasExhibitors == false) {
                vm.event.exhibitorsOnlineRegister = false;
            }
        };

        vm.hasPartnersChanged = function () {
            if (vm.event.partnersOnlineRegister == true && vm.event.hasPartners == false) {
                vm.event.partnersOnlineRegister = false;
            }
        };

        vm.hasSpeakerChanged = function () {
            if (vm.event.speakerOnlineRegister == true && vm.event.hasSpeaker == false) {
                vm.event.speakerOnlineRegister = false;
            }
        };

        vm.hasVIPChanged = function () {
            if (vm.event.vipOnlineRegister == true && vm.event.hasVIP == false) {
                vm.event.vipOnlineRegister = false;
            }
        };

        vm.clearPresentation = function () {
            vm.presentation = {};
        };

        vm.loadEventData = function (EventId) {
            $http.get($rootScope.app.httpSource + 'api/Event/GetEventById?id=' + EventId)
                .then(function (resp) {

                    vm.event = resp.data;

                    if (vm.event.participantsLimit != null && vm.event.participantsLimit != 0 && vm.event.registeredUsersCount > vm.event.participantsLimit)
                        vm.showLimitMsg = true;
                    else
                        vm.showLimitMsg = false;
                    try {
                        debugger;
                        if (vm.event.eventPersons != undefined && vm.event.eventPersons != null)
                            vm.isSpeaker = vm.event.eventPersons.filter(x => x.personId == vm.user.personId);
                    }
                    catch (e) {
                        console.log(e);
                    }

                    vm.event.startDate = new Date(vm.event.startDate);
                    vm.event.endDate = new Date(vm.event.endDate);
                    for (var ii = 0; ii < vm.event.agenda.length; ii++) {
                        vm.event.agenda[ii].startDate = new Date(vm.event.agenda[ii].startDate);
                        //vm.event.agenda[ii].endDate = new Date(vm.event.agenda[ii].endDate);
                    }
                    vm.uploadedphotos = [];
                    vm.survey = vm.event.surveys.filter(x => x.fromSpeaker == null || !x.fromSpeaker);
                    vm.speakerSurvey = vm.event.surveys.filter(x => x.fromSpeaker);
                    for (var i = 0; i < vm.event.photos.length; i++) {
                        var photo = {
                            eventId: vm.event.id,
                            documentFullPath: vm.event.photos[i].photoFullPath,
                            documentPath: vm.event.photos[i].photoName
                        }
                        vm.uploadedphotos.push(photo);
                    }
                    if (vm.event.themeURL == null) {
                        vm.event.themeURL = vm.themes[0].url;
                    }
                    vm.checkSubsription();
                }, function (response) { });
        };

        vm.previous = function (stepNumber) {
            switch (stepNumber) {
                case 2:
                    vm.selectTab(stepNumber - 1);
                    vm.activeStep = 1;
                    break;
                case 3:
                    vm.selectTab(stepNumber - 1);
                    vm.activeStep = 2;
                    break;
                case 4:
                    vm.selectTab(stepNumber - 1);
                    vm.activeStep = 3;
                    break;
                case 5:
                    vm.selectTab(stepNumber - 1);
                    vm.activeStep = 4;
                    break;
                case 6:
                    vm.selectTab(stepNumber - 1);
                    vm.activeStep = 5;
                    break;
                case 7:
                    vm.selectTab(stepNumber - 1);
                    vm.activeStep = 6;
                    break;
                default:
                    break;
            }
        };

        if ($stateParams.id) {
            vm.loadEventData($stateParams.id);
        }

        vm.save = function (stepNumber) {
            debugger;
            vm.event.photos = [];
            vm.event.startDate = moment.utc(vm.event.startDate).format();
            vm.event.endDate = moment.utc(vm.event.endDate).format();
            for (var ii = 0; ii < vm.event.agenda.length; ii++) {
                vm.event.agenda[ii].startDate = moment.utc(vm.event.agenda[ii].startDate).format();
                //vm.event.agenda[ii].endDate = moment.utc(vm.event.agenda[ii].endDate).format();
            }

            for (var i = 0; i < vm.uploadedphotos.length; i++) {
                var photo = {
                    eventId: vm.event.id,
                    photoFullPath: vm.uploadedphotos[i].documentFullPath,
                    photoName: vm.uploadedphotos[i].documentPath
                }
                vm.event.photos.push(photo);
            }

            if (!vm.event.nameEn.length || !vm.event.descriptionEn.length  //|| !vm.event.descriptionAr.length || !vm.event.nameAr.length 
                || vm.event.startDate == null || vm.event.endDate == null) {
                vm.showRequiredError = true;
                return;
            }
            vm.showRequiredError = false;
            if (!vm.event.id && vm.event.id == undefined) {
                if (new Date() > new Date(vm.event.endDate)) {
                    SweetAlert.swal($filter('translate')('general.error'), "Selected event date is an old date", "error");
                    return;
                }

                $http.post($rootScope.app.httpSource + 'api/Event/CreateEvent', vm.event)
                    .then(function (response) {
                        $state.go("app.home");
                        //$state.go("app.event", { id: response.data.id });
                        SweetAlert.swal($filter('translate')('general.ok'), $filter('translate')('general.eventMsg'), "success");
                    }, function (response) { });
            }
            else {

                $http.post($rootScope.app.httpSource + 'api/Event/UpdateEvent', vm.event)
                    .then(function (response) {
                        $state.go("app.home");
                        //vm.loadEventData(vm.event.id);
                        SweetAlert.swal($filter('translate')('general.ok'), $filter('translate')('general.eventMsg'), "success");
                    }, function (response) { });
            }
        };

        vm.saveSurvey = function () {
            debugger;
            vm.eventSurvey.EventId = $scope.EventId;
            if (vm.updateSurvey == 1) {
                vm.updateSurvey = 0;
                $http.post($rootScope.app.httpSource + 'api/Survey/UpdateSurvey', vm.eventSurvey)
                    .then(function (response) {
                        vm.eventSurvey = {
                            AgendaId: 0,
                            nameEn: "",
                            nameAr: "",
                            optionEn: "",
                            optionAr: "",
                            fromSpeaker: false,
                            responseAlotTime: "",
                            surveyOptions: []
                        };
                        SweetAlert.swal($filter('translate')('general.ok'), $filter('translate')('general.questionMsg'), "success");
                        //$state.go("app.home");
                        vm.loadEventData(vm.event.id);
                    }, function (response) { });
            }
            else {
                vm.updateSurvey = 0;
                $http.post($rootScope.app.httpSource + 'api/Survey/CreateSurvey', vm.eventSurvey)
                    .then(function (response) {
                        vm.eventSurvey = {
                            AgendaId: 0,
                            nameEn: "",
                            nameAr: "",
                            optionEn: "",
                            optionAr: "",
                            fromSpeaker: false,
                            responseAlotTime: "",
                            surveyOptions: []
                        };
                        SweetAlert.swal($filter('translate')('general.ok'), $filter('translate')('general.questionMsg'), "success");
                        //$state.go("app.home");
                        vm.loadEventData(vm.event.id);

                    }, function (response) { });
            }

        };

        //______________Survey Questions Datatable_____________________________________

        vm.surveyQuestionsDt = {};
        vm.surveyQuestionsDt.dtInstance = {};
        vm.surveyQuestionsDt.actionsHtml = function (data, type, full, meta) {

            var htmlSection = '';

            htmlSection = '<div class="list-icon"><div class="btn btn-icon waves-effect waves-light btn-info" ng-click="vm.surveyQuestionsDt.edit(' +
                data.id + ')"><em class="fa fa-pencil" style="cursor:pointer" uib-tooltip="' +
                vm.translateFilter('general.edit') + '"></em></div><div class="btn btn-icon waves-effect waves-light btn-info" ng-click="vm.surveyQuestionsDt.delete(' +
                data.id + ', $event)"><em class="fa fa-trash" style="cursor:pointer" uib-tooltip="' +
                vm.translateFilter('general.delete') + '"></em></div></div>';

            return htmlSection;
        };

        vm.surveyQuestionsDt.actionsoptionsHtml = function (data, type, full, meta) {

            var htmlSection = '';
            var surveyItem = $filter('filter')(vm.event.surveys.filter(x => x.fromSpeaker == null || x.fromSpeaker == false), { id: data.id })[0];

            var index = vm.event.surveys.indexOf(surveyItem);

            htmlSection = '<ul><li ng-repeat="options in vm.event.surveys[' + index + '].surveyOptions | orderBy : \'orderNumber\'">{{options | localizeString}}</li></ul>';

            return htmlSection;
        };

        vm.surveyQuestionsDt.dtColumns = [
            DTColumnBuilder.newColumn('nameEn').withTitle(vm.translateFilter('event.questionEn')),
            DTColumnBuilder.newColumn('nameAr').withTitle(vm.translateFilter('event.questionAr')),
            DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('event.Surveyoptions')).renderWith(vm.surveyQuestionsDt.actionsoptionsHtml).withOption('width', '15%'),
            DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('general.actions')).notSortable()
                .renderWith(vm.surveyQuestionsDt.actionsHtml).withOption('width', '15%')];

        vm.surveyQuestionsDt.edit = function (surveyId) {

            vm.updateSurvey = 1;
            vm.eventSurvey = vm.event.surveys.filter(function (item) { return item.id == surveyId; })[0];
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Event/SurveyQuestion/surveyQuestion.html',
                controller: 'surveyQuestionController',
                size: 'lg',
                keyboard: false,
                backdrop: 'static',
                resolve: {
                    eventId: function () {
                        return vm.event.id;
                    },
                    eventSurvey: function () {
                        return vm.eventSurvey;
                    },
                    isSpeaker: function () {
                        return false;
                    }
                }
            });
            modalInstance.result.then(function (eventSurvey) {
                vm.loadEventData(vm.event.id);
            }, function () {
                //state.text('Modal dismissed with Cancel status');
            });
        };

        vm.surveyQuestionsDt.add = function (size) {
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Event/SurveyQuestion/surveyQuestion.html',
                controller: 'surveyQuestionController',
                size: size,
                keyboard: false,
                backdrop: 'static',
                resolve: {
                    eventId: function () {
                        return vm.event.id;
                    },
                    eventSurvey: function () {
                        return null;
                    },
                    isSpeaker: function () {
                        return false;
                    }
                }
            });
            modalInstance.result.then(function (eventSurvey) {
                vm.loadEventData(vm.event.id);
            }, function () {
                //state.text('Modal dismissed with Cancel status');
            });
        };

        vm.surveyQuestionsDt.delete = function (optionid, event) {
            var index;
            var tempStore;

            if (optionid == 0 || optionid == undefined) {
                index = vm.presentationDt.dtInstance.DataTable.rows({ order: 'applied' }).nodes().indexOf(event.currentTarget.parentNode.parentNode.parentNode);
                tempStore = vm.event.surveys[index];
                //vm.event.surveys.splice(index, 1);
            }
            else {
                index = vm.event.surveys.indexOf($filter('filter')(vm.event.surveys, { id: optionid }, true)[0]);
                tempStore = $filter('filter')(vm.event.surveys, { id: optionid }, true)[0];
                //vm.event.surveys.splice(index, 1);
            }
            var translate = $filter('translate');
            //vm.surveyQuestionsDt.dtInstance.rerender();

            SweetAlert.swal({
                title: translate('general.delete'),
                text: translate('general.confirmDeleteMsg'),
                type: "warning",
                showCancelButton: true,
                showLoaderOnConfirm: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: translate('general.delete'),
                cancelButtonText: translate('general.cancel'),
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        //delete

                        $http.get($rootScope.app.httpSource + 'api/Survey/DeleteSurvey?id=' + optionid)
                            .then(function (resp) {
                                SweetAlert.swal(translate('general.delete'), translate('general.deleteMessage'), "success");
                                vm.event.surveys.splice(index, 1);
                                vm.loadEventData(vm.event.id);
                                //window.location.reload();  
                            },
                                function (response) { });

                    } else {
                        SweetAlert.swal({ title: translate('general.restoreBtn'), text: translate('general.restoreMessage'), type: "success", confirmButtonText: translate('general.ok') });
                        vm.surveyQuestionsDt.dtInstance.rerender();
                    }
                });
        };

        //______________Survey Speakers Datatable_____________________________________

        vm.speakerQuestionsDt = {};
        vm.speakerQuestionsDt.dtInstance = {};
        vm.speakerQuestionsDt.actionsHtml = function (data, type, full, meta) {

            var htmlSection = '';

            htmlSection = '<div class="list-icon"><div class="btn btn-icon waves-effect waves-light btn-info" ng-click="vm.speakerQuestionsDt.edit(' +
                data.id + ')"><em class="fa fa-pencil" style="cursor:pointer" uib-tooltip="' +
                vm.translateFilter('general.edit') + '"></em></div><div class="btn btn-icon waves-effect waves-light btn-info" ng-click="vm.speakerQuestionsDt.delete(' +
                data.id + ', $event)"><em class="fa fa-trash" style="cursor:pointer" uib-tooltip="' +
                vm.translateFilter('general.delete') + '"></em></div></div>';

            return htmlSection;
        };

        vm.speakerQuestionsDt.actionsoptionsHtml = function (data, type, full, meta) {
            var htmlSection = '';
            var surveyItem = $filter('filter')(vm.event.surveys.filter(x => x.fromSpeaker == true), { id: data.id })[0];
            var index = vm.event.surveys.indexOf(surveyItem);

            htmlSection = '<ul><li ng-repeat="options in vm.event.surveys[' + index + '].surveyOptions | orderBy : \'orderNumber\'">{{options | localizeString}}</li></ul>';

            return htmlSection;
        };

        vm.speakerQuestionsDt.dtColumns = [
            DTColumnBuilder.newColumn('nameEn').withTitle(vm.translateFilter('event.questionEn')),
            DTColumnBuilder.newColumn('nameAr').withTitle(vm.translateFilter('event.questionAr')),
            DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('event.Surveyoptions'))
                .renderWith(vm.speakerQuestionsDt.actionsoptionsHtml).withOption('width', '15%'),
            DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('general.actions')).notSortable()
                .renderWith(vm.speakerQuestionsDt.actionsHtml).withOption('width', '15%')];

        vm.speakerQuestionsDt.edit = function (surveyId) {

            vm.updateSurvey = 1;
            vm.eventSurvey = vm.event.surveys.filter(function (item) { return item.id == surveyId; })[0];
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Event/SurveyQuestion/surveyQuestion.html',
                controller: 'surveyQuestionController',
                size: 'lg',
                keyboard: false,
                backdrop: 'static',
                resolve: {
                    eventId: function () {
                        return vm.event.id;
                    },
                    eventSurvey: function () {
                        return vm.eventSurvey;
                    },
                    isSpeaker: function () {
                        debugger;
                        if (vm.isSpeaker.length > 0)
                            return true;
                        else
                            return false;

                    }
                }
            });
            modalInstance.result.then(function (eventSurvey) {
                vm.loadEventData(vm.event.id);
            }, function () {
                //state.text('Modal dismissed with Cancel status');
            });
        };

        vm.speakerQuestionsDt.add = function (size) {
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Event/SurveyQuestion/surveyQuestion.html',
                controller: 'surveyQuestionController',
                size: size,
                keyboard: false,
                backdrop: 'static',
                resolve: {
                    eventId: function () {
                        return vm.event.id;
                    },
                    eventSurvey: function () {
                        return null;
                    },
                    isSpeaker: function () {
                        debugger;
                        if (vm.isSpeaker.length > 0)
                            return true;
                        else
                            return false;

                    }
                }
            });
            modalInstance.result.then(function (eventSurvey) {
                vm.loadEventData(vm.event.id);
            }, function () {
                //state.text('Modal dismissed with Cancel status');
            });
        };

        vm.speakerQuestionsDt.delete = function (optionid, event) {
            var index;
            var tempStore;

            if (optionid == 0 || optionid == undefined) {
                index = vm.presentationDt.dtInstance.DataTable.rows({ order: 'applied' }).nodes().indexOf(event.currentTarget.parentNode.parentNode.parentNode);
                tempStore = vm.event.surveys[index];
                //vm.event.surveys.splice(index, 1);
            }
            else {
                index = vm.event.surveys.indexOf($filter('filter')(vm.event.surveys, { id: optionid }, true)[0]);
                tempStore = $filter('filter')(vm.event.surveys, { id: optionid }, true)[0];
                //vm.event.surveys.splice(index, 1);
            }
            var translate = $filter('translate');
            //vm.speakerQuestionsDt.dtInstance.rerender();

            SweetAlert.swal({
                title: translate('general.delete'),
                text: translate('general.confirmDeleteMsg'),
                type: "warning",
                showCancelButton: true,
                showLoaderOnConfirm: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: translate('general.delete'),
                cancelButtonText: translate('general.cancel'),
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        //delete
                        $http.get($rootScope.app.httpSource + 'api/Survey/DeleteSurvey?id=' + optionid)
                            .then(function (resp) {
                                SweetAlert.swal(translate('general.delete'), translate('general.deleteMessage'), "success");
                                vm.loadEventData(vm.event.id);
                                //window.location.reload();  
                            }, function (response) { });
                    } else {
                        SweetAlert.swal(translate('general.restoreBtn'), translate('general.restoreMessage'), "success");
                        vm.speakerQuestionsDt.dtInstance.rerender();
                    }
                });
        };
    }

    eventController.$inject = ['$rootScope', '$scope', '$location', '$window', '$http', '$stateParams', '$state', 'UserProfile', 'WizardHandler', '$filter', 'DTOptionsBuilder',
        'DTColumnBuilder', '$compile', '$uibModal', 'SweetAlert', 'moment'];

})();