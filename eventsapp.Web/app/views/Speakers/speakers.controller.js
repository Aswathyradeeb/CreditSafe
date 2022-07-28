(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('speakersController', speakersController);

    speakersController.$inject = ['$rootScope', '$scope', 'UserProfile', '$filter', '$compile', '$http', '$uibModal', '$state', 'SweetAlert', '$window'];
    function speakersController($rootScope, $scope, UserProfile, $filter, $compile, $http, $uibModal, $state, SweetAlert, $window) {
        debugger;
        var vm = this;

        vm.filterParams = { events: [] };
        vm.params = {
            filterParams: (vm.filterParams === undefined ? null : vm.filterParams),
            page: 1,
            pageSize: 12
        };

        $http.post($rootScope.app.httpSource + 'api/Speaker/GetAllSpeakers', vm.params)
                .then(function (resp) {
    
                    vm.speakers = resp.data.content;
                    vm.pager = resp.data;
                },
                function (response) { });

        vm.addSpeaker = function (size) {
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Person/person.html',
                controller: 'editPersonController',
                size: size,
                closable: true,
                animation: true,
                backdrop: 'static',
                keyboard: false,
                resolve: {
                    person: function () {
                        return null;
                    },
                    viewMode: false
                }
            });

            modalInstance.result.then(function (eventSpeaker) {
                vm.setPage(1);  
            }, function () {
                //state.text('Modal dismissed with Cancel status');
            });
        };
        vm.view = function (person) {
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Person/person.html',
                controller: 'editPersonController',
                closable: true,
                animation: true,
                backdrop: 'static',
                keyboard: false,
                size: 'lg',
                resolve: {
                    person: function () {
                        return person;
                    },
                    viewMode: true
                }
            });
            modalInstance.result.then(function (person) {
                vm.setPage(1);
            }, function () {
            });
        }
        vm.edit = function (person) {
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Person/person.html',
                controller: 'editPersonController',
                closable: true,
                animation: true,
                backdrop: 'static',
                keyboard: false,
                size: 'lg',
                resolve: {
                    person: function () {
                        return person;
                    },
                    viewMode : false
                }
            });
            modalInstance.result.then(function (person) {
                vm.setPage(1);
            }, function () {
            });
        }

        vm.delete = function (speakerId) { 
            var translate = $filter('translate'); 
            SweetAlert.swal({
                title: translate('general.delete'),
                text: translate('general.confirmDeleteMsg'),
                type: "warning",
                showCancelButton: true,
                showLoaderOnConfirm: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: translate('general.delete'),
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        //delete
                        $http.get($rootScope.app.httpSource + 'api/Speaker/DeleteSpeaker?id=' + speakerId)
                            .then(function (resp) { 
                                SweetAlert.swal(translate('general.delete'), translate('general.deleteMessage'), "success");
                                window.location.reload();
                            },
                                function (response) { });

                    } else {

                        SweetAlert.swal(translate('general.restoreBtn'), translate('general.restoreMessage'), "success");
                        vm.testReportDt.dtInstance.rerender();
                    }
                });
        };

        vm.pager = {};
        vm.setPage = function (page) {
            debugger;
            vm.params = {
                filterParams: (vm.filterParams === undefined ? null : vm.filterParams),
                page: page,
                pageSize: vm.pager.pageSize
            };
            $http.post($rootScope.app.httpSource + 'api/Speaker/GetAllSpeakers', vm.params)
                 .then(function (resp) {
     
                     vm.speakers = resp.data.content;
                     vm.pager = resp.data;
                 },
                 function (response) { });
        };
    }
})();

function rotateCard(btn) {
    var $card = $(btn).closest('.card-container');
    console.log($card);
    if ($card.hasClass('hover')) {
        $card.removeClass('hover');
    } else {
        $card.addClass('hover');
    }
}