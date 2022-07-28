(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('surveyQuestionController', surveyQuestionController);
    /* @ngInject */
    function surveyQuestionController($rootScope, $scope, $http, $uibModalInstance, $filter, eventId, eventSurvey, isSpeaker, DTColumnBuilder, SweetAlert) {

        $scope.controllerScope = $scope;
        if (eventSurvey !== null) {
            $scope.eventSurvey = angular.copy(eventSurvey);
            $scope.eventId = angular.copy(eventId);
            $scope.isSpeaker = angular.copy(isSpeaker);
        }
        else {
            $scope.eventSurvey = {
                AgendaId: 0,
                nameEn: "",
                nameAr: "",
                optionEn: "",
                optionAr: "",
                fromSpeaker: null,
                responseAlotTime: "",
                surveyOptions: []
            };
            $scope.eventId = angular.copy(eventId);
            $scope.isSpeaker = angular.copy(isSpeaker);
        }
        $scope.translateFilter = $filter('translate');
        $scope.localizeString = $filter('localizeString');
        $scope.ok = function () {
            $scope.eventSurvey.eventId = $scope.eventId;
            if ($scope.eventSurvey.surveyOptions.length == 0) {
                SweetAlert.swal($filter('translate')('general.error'), "Please add options for survey", "error");
                return;
            }

            if ($scope.eventSurvey.id) {
                $http.post($rootScope.app.httpSource + 'api/Survey/UpdateSurvey', $scope.eventSurvey)
                    .then(function (response) {
                        SweetAlert.swal($filter('translate')('general.ok'), $filter('translate')('general.questionMsg'), "success");
                        //$state.go("app.home");
                        $uibModalInstance.close($scope.eventSpeaker);
                    }, function (response) { });
            }
            else {
                $http.post($rootScope.app.httpSource + 'api/Survey/CreateSurvey', $scope.eventSurvey)
                    .then(function (response) {
                        SweetAlert.swal($filter('translate')('general.ok'), $filter('translate')('general.questionMsg'), "success");
                        //$state.go("app.home");
                        $uibModalInstance.close($scope.eventSpeaker);

                    }, function (response) { });
            }
        };

        $scope.closeModal = function () {
            $uibModalInstance.dismiss('cancel');
        };

        $http.get($rootScope.app.httpSource + 'api/Agenda/GetAgendaEvent?eventId=' + $scope.eventId)
            .then(function (response) {
                $scope.agendas = response.data;
            }, function (response) { });


        $scope.onSelectedAgenda = function (selectedItem) {
            $scope.eventSurvey.agendaId = selectedItem.id;
        };

        //______________Survey Options Datatable_____________________________________
        $scope.surveyOptionsDt = {};
        $scope.surveyOptionsDt.dtInstance = {};
        $scope.surveyOptionsDt.actionsHtml = function (data, type, full, meta) {
            var htmlSection = '';
            htmlSection = '<div class="list-icon"><div class="btn btn-icon waves-effect waves-light btn-info" ng-click="surveyOptionsDt.edit(' +
                data.id + ')"><em class="fa fa-pencil" style="cursor:pointer" uib-tooltip="' +
                $scope.translateFilter('general.edit') + '"></em></div><div class="btn btn-icon waves-effect waves-light btn-info" ng-click="surveyOptionsDt.delete(' +
                data.id + ', $event)"><em class="fa fa-trash" style="cursor:pointer" uib-tooltip="' +
                $scope.translateFilter('general.delete') + '"></em></div></div>';
            return htmlSection;
        };

        $scope.surveyOptionsDt.dtColumns = [
            DTColumnBuilder.newColumn(null).withTitle($scope.translateFilter('event.optionAr'))
                .renderWith(function (data, type) {
                    if (data.nameAr) {
                        return data.nameAr;
                    }
                    return '';
                }),
            DTColumnBuilder.newColumn(null).withTitle($scope.translateFilter('event.optionEn'))
                .renderWith(function (data, type) {
                    if (data.nameEn) {
                        return data.nameEn;
                    }
                    return '';
                }),
            DTColumnBuilder.newColumn('orderNumber').withTitle($scope.translateFilter('session.orderNumber')),
            DTColumnBuilder.newColumn(null).withTitle($scope.translateFilter('general.actions')).notSortable()
                .renderWith($scope.surveyOptionsDt.actionsHtml).withOption('width', '15%')];


        $scope.surveyOption = {};
        $scope.addSurveyoptions = function (formName) {
            if ($rootScope.englishPreferredLanguage && !$scope.surveyOption.nameEn) {
                $scope.showRequiredError = true;
                return;
            }
            if ($rootScope.arabicPreferredLanguage && !$scope.surveyOption.nameAr) {
                $scope.showRequiredError = true;
                return;
            }
            if ($scope.surveyOption.orderNumber) {
                if (!$scope.surveyOption.id) {
                    $scope.surveyOption.id = $scope.eventSurvey.surveyOptions.length + 1;
                    $scope.eventSurvey.surveyOptions.push($scope.surveyOption);
                }
                else {
                    for (var i = 0; i < $scope.eventSurvey.surveyOptions.length; i++) {
                        if ($scope.surveyOption.sid == $scope.eventSurvey.surveyOptions[i].id) {
                            $scope.eventSurvey.surveyOptions[i] = $scope.surveyOption;
                            break;
                        }
                    }
                }
                $scope.showRequiredError = false;
                $scope.surveyOptionsDt.dtInstance.rerender();
                formName.$setPristine();
                $scope.surveyOption = {};
            }
            else {
                $scope.showRequiredError = true;
                return;
            }
        };


        $scope.surveyOptionsDt.edit = function (optionid) {
            $scope.surveyOption = $scope.eventSurvey.surveyOptions.filter(function (item) { return item.id == optionid; })[0];
        };

        $scope.surveyOptionsDt.delete = function (optionid, event) {
            var index;
            var tempStore;
            if (optionid == 0 || optionid == undefined) {
                index = $scope.surveyOptionsDt.dtInstance.DataTable.rows({ order: 'applied' }).nodes().indexOf(event.currentTarget.parentNode.parentNode.parentNode);
                tempStore = $scope.eventSurvey.surveyOptions[index];
                $scope.eventSurvey.surveyOptions.splice(index, 1);
            }
            else {
                index = $scope.eventSurvey.surveyOptions.indexOf($filter('filter')($scope.eventSurvey.surveyOptions, { id: optionid }, true)[0]);
                tempStore = $filter('filter')($scope.eventSurvey.surveyOptions, { id: optionid }, true)[0];
                $scope.eventSurvey.surveyOptions.splice(index, 1);
            }
            var translate = $filter('translate');
            $scope.surveyOptionsDt.dtInstance.rerender();

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
                    debugger;
                    if (isConfirm) {
                        //delete
                        SweetAlert.swal(translate('general.confirmDeleteBtn'), translate('general.deleteMessage'), "success");
                        $scope.surveyOptionsDt.dtInstance.rerender();
                    } else {
                        debugger;
                        $scope.eventSurvey.surveyOptions.splice(index, 0, tempStore);
                        SweetAlert.swal(translate('general.restoreBtn'), translate('general.restoreMessage'), "success");
                        $scope.surveyOptionsDt.dtInstance.rerender();
                    }
                });
        };
    }

    surveyQuestionController.$inject = ['$rootScope', '$scope', '$http', '$uibModalInstance', '$filter', 'eventId', 'eventSurvey', 'isSpeaker', 'DTColumnBuilder', 'SweetAlert'];
})();