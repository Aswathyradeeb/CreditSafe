(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('SubsciptionsController', SubsciptionsController);

    SubsciptionsController.$inject = ['$rootScope', '$scope', 'UserProfile', '$filter', '$compile', '$http', '$uibModal', '$state', 'SweetAlert', '$window', 'DTOptionsBuilder', 'DTColumnBuilder'];
    function SubsciptionsController($rootScope, $scope, UserProfile, $filter, $compile, $http, $uibModal, $state, SweetAlert, $window, DTOptionsBuilder, DTColumnBuilder) {
        var vm = this;
        vm.user = UserProfile.getProfile();

        vm.userSubscriptions = [];
        vm.params = {
            searchtext: '',
            page: 1,
            pageSize: 9
        };


        vm.subscriptionDt = {};
        vm.subscriptionDt.dtInstance = {};
        vm.subscriptionDt.serverData = function (sSource, aoData, fnCallback, oSettings) {
            var aoDataLength = aoData.length;
            //All the parameters you need is in the aoData variable
            var draw = aoData[0].value;
            var order = aoData[2].value[0];
            var start = aoData[3].value;
            var length = aoData[4].value;
            var search = aoData[5].value;

            vm.params = {
                searchtext: search.value,
                page: (start / length) + 1,
                pageSize: length,
                sortBy: 'createdOn',
                sortDirection: order.dir,
                filterParams: vm.filterParams
            };

            //Then just call your service to get the records from server side           
            $http.post($rootScope.app.httpSource + 'api/User/GetUserSubscriptions', vm.params)
                .then(function (resp) {
                    debugger;
                    vm.userSubscriptions = resp.data.content;
                    console.log(vm.userSubscriptions);
                    var records = {
                        'draw': draw,
                        'recordsTotal': resp.data.totalRecords,
                        'recordsFiltered': resp.data.totalRecords,
                        'data': resp.data.content
                    };
                    fnCallback(records);
                });
        };

        vm.subscriptionDt.createdRow = function (row, data, dataIndex) { 
            // Recompiling so we can bind Angular directive to the DT
            $compile(angular.element(row).contents())($scope);
        };

        vm.subscriptionDt.rowCallback = function (row, data, dataIndex) {
            $compile(angular.element(row).contents())($scope);
        };

        vm.translateFilter = $filter('translate');
        vm.localizeStringFilter = $filter('localizeString');

        if ($rootScope.language.selected !== 'English') {
            vm.subscriptionDt.dtOptions = DTOptionsBuilder.newOptions()
                .withFnServerData(vm.subscriptionDt.serverData)
                .withOption('serverSide', true)
                .withDataProp('data')
                .withOption('processing', false)
                .withOption('responsive', {
                    details: {
                        renderer: renderer
                    }
                })
                .withLanguageSource('app/langs/ar.json')
                .withOption('bFilter', true)
                .withOption('paging', true)
                .withOption('info', true)
                .withOption('createdRow', vm.subscriptionDt.createdRow)
                .withOption('rowCallback', vm.subscriptionDt.rowCallback).withBootstrap();
        }
        else {
            vm.subscriptionDt.dtOptions = DTOptionsBuilder.newOptions()
                .withFnServerData(vm.subscriptionDt.serverData)
                .withOption('serverSide', true)
                .withDataProp('data')
                .withOption('processing', false)
                .withOption('responsive', {
                    details: {
                        renderer: renderer
                    }
                })
                .withOption('bFilter', true)
                .withOption('paging', true)
                .withOption('info', true)
                .withOption('createdRow', vm.subscriptionDt.createdRow)
                .withOption('rowCallback', vm.subscriptionDt.rowCallback).withBootstrap();
        }

        function renderer(api, rowIdx, columns) {
            var data = $.map(columns, function (col, i) {
                return col.hidden ?
                    '<li data-dtr-index="' + col.columnIndex + '" data-dt-row="' + col.rowIndex + '" data-dt-column="' + col.columnIndex + '">' +
                    '<span class="dtr-title">' +
                    col.title +
                    '</span> ' +
                    '<span class="dtr-data">' +
                    col.data +
                    '</span>' +
                    '</li>' :
                    '';
            }).join('');
            return data ?
                $compile(angular.element($('<ul data-dtr-index="' + rowIdx + '"/>').append(data)))($scope) :
                false;
        }

        vm.actionsHtml = function (data, type, full, meta) {
            var htmlSection = '<div   class="list-icon inline">';
            if (data.transactionId != null && data.paymentStatusId == 3) {
                htmlSection += '<a class="btn btn-icon waves-effect waves-light btn-info text-center"  ui-sref="app.viewPayment({transactionId: ' + data.transactionId + '})"><em class="fa fa-eye" style="cursor:pointer" uib-tooltip="Click To Pay"></em>' + vm.translateFilter('general.viewReceipt')+'</a>';

            }
            else if (vm.user.roleName == "3" || vm.user.roleName=="2") {
                htmlSection += '<span class="btn btn-icon waves-effect waves-light btn-info text-center" ng-click="vm.makePayment(' +
                    "'" + data.id + "'" + ')"><em class="fa fa-money" style="cursor:pointer" uib-tooltip="' + vm.translateFilter('general.pay') +'"></em> ' + vm.translateFilter('general.pay') +'</span>';
            }
            htmlSection += '</div>';
            return htmlSection;
        };

        vm.addSubscription = function () {
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Subsciptions/addSubscription/addSubscription.html',
                controller: 'addSubscriptionController',
                size: 'lg',
                keyboard: false,
                backdrop: 'static' 
            }); 
            modalInstance.result.then(function () {
                vm.subscriptionDt.dtInstance.rerender();
            });
        };


        vm.makePayment = function (subscriptionId) {
            $http.get($rootScope.app.httpSource + 'api/Payment/MakePayment?subscriptionId=' + subscriptionId)
                .then(function (resp) {
                    window.location = resp.data;
                });
        };
        if (vm.user.roleName == 2) {
            vm.subscriptionDt.dtColumns = [
                DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('general.name')).renderWith(
                    function (data) {
                        return data.user.firstName + ' ' + data.user.lastName;
                    }),
             
                DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('package.cost')).renderWith(
                    function (data) {
                        return data.fees + vm.translateFilter('package.aed');
                    }),
                DTColumnBuilder.newColumn('createdOn').withTitle(vm.translateFilter('general.createdOn'))
                    .renderWith(function (date) {
                        return $filter('date')(date, 'dd-MMM-yyyy');
                    }),
                DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('general.paymentStatus')).renderWith(
                    function (data) {
                        return $filter('localizeString')(data.paymentStatus);
                    }),
                DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('general.actions')).notSortable()
                    .renderWith(vm.actionsHtml).withOption('width', '20%')];
        }
        else {
            vm.subscriptionDt.dtColumns = [
                DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('general.name')).renderWith(
                    function (data) {
                        return data.user.firstName + ' ' + data.user.lastName;
                    }),
                DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('package.packageType')).renderWith(
                    function (data) {
                        if (data.eventPackage != null) {
                            return $filter('localizeString')(data.eventPackage);
                        }
                        return '';
                    }),
                DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('package.cost')).renderWith(
                    function (data) {
                        if (data.eventPackage != null) {
                            return data.eventPackage.price + vm.translateFilter('package.aed');
                        }
                        return '';
                    }),
                DTColumnBuilder.newColumn('createdOn').withTitle(vm.translateFilter('general.createdOn'))
                    .renderWith(function (date) {
                        return $filter('date')(date, 'dd-MMM-yyyy');
                    }),
                DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('general.paymentStatus')).renderWith(
                    function (data) {
                        return $filter('localizeString')(data.paymentStatus);
                    }),
                DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('general.actions')).notSortable()
                    .renderWith(vm.actionsHtml).withOption('width', '20%')];
        }

     

    }
})();
