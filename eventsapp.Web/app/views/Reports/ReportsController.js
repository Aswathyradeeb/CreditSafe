(function () {
    'use strict';

    angular.module('eventsapp')
        .controller('ReportsController', ReportsController);

    ReportsController.$inject = ['$rootScope', '$scope', 'UserProfile', '$filter', '$compile', '$http', '$uibModal', '$state', '$stateParams', 'SweetAlert', '$window', 'DTOptionsBuilder', 'DTColumnBuilder', 'moment'];

    function ReportsController($rootScope, $scope, UserProfile, $filter, $compile, $http, $uibModal, $state, $stateParams, SweetAlert, $window, DTOptionsBuilder, DTColumnBuilder, moment) {
        debugger;
        var vm = this;
        vm.user = UserProfile.getProfile();
        vm.productCategories = [];
        vm.openUserModelPopup = false;
        vm.openPasswordModelPopup = false;
        vm.claimedVouchers = [];
        vm.events = [];
        vm.registrationTypes = [];
        vm.restaurants = [];
        vm.dateFilter = $filter('date');
        vm.orderByFilter = $filter('orderBy');
        vm.filterParams = {};
        vm.params = {
            filterParams: (vm.filterParams === undefined ? null : vm.filterParams),
            searchtext: '',
            page: 1,
            pageSize: 9
        };

        $http.get($rootScope.app.httpSource + 'api/Roles/GetAll')
            .then(function (resp) {
                vm.roles = resp.data;
            }, function (response) { });

        $http.get($rootScope.app.httpSource + 'api/Event/GetEvents')
            .then(function (resp) {
                vm.events = resp.data;
                if (!vm.params.filterParams.eventId) {
                    vm.params.filterParams.eventId = null;
                }
                vm.params.filterParams.eventId = vm.events[0].id;
                //vm.claimedVouchersDt.dtInstance.rerender();
            }, function (response) { });

        $http.get($rootScope.app.httpSource + 'api/RegistrationType/GetRegistrationTypes')
            .then(function (response) {
                vm.registrationTypes = response.data;
                vm.registrationTypes = vm.registrationTypes.filter(function (item) { return item.id != 2 });
                vm.registrationTypes = vm.registrationTypes.filter(function (item) { return item.id != 4 });
                vm.registrationTypes = vm.registrationTypes.filter(function (item) { return item.id != 6 });

                if (!vm.params.filterParams.registrationTypeId) {
                    vm.params.filterParams.registrationTypeId = null;
                }
                vm.params.filterParams.registrationTypeId = vm.registrationTypes[0].id;
            }, function (response) { });

        if (vm.user.roleName == 1) {
            $http.get($rootScope.app.httpSource + 'api/Restaurant/GetAllRestaurants')
                .then(function (response) {
                    vm.restaurants = response.data;
                    if (!vm.params.filterParams.restaurantUserId) {
                        vm.params.filterParams.restaurantUserId = null;
                    }
                    vm.params.filterParams.restaurantUserId = vm.restaurants[0].users[0].id;
                    //vm.claimedVouchersDt.dtInstance.rerender();
                }, function (response) { });
        }
        else if (vm.user.roleName == 2){
            vm.params.filterParams.restaurantUserId = vm.user.userId;
        }

        vm.claimedVouchersDt = {};
        vm.claimedVouchersDt.dtInstance = {};
        vm.claimedVouchersDt.serverData = function (sSource, aoData, fnCallback, oSettings) {
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
            $http.post($rootScope.app.httpSource + 'api/ClaimedVoucher/GetClaimedVouchers', vm.params)
                .then(function (resp) {
                    vm.claimedVouchers = resp.data.content;
                    vm.totalCount = resp.data.totalCount;
                    var records = {
                        'draw': draw,
                        'recordsTotal': resp.data.totalRecords,
                        'recordsFiltered': resp.data.totalRecords,
                        'data': resp.data.content
                    };
                    fnCallback(records);
                });
        };

        vm.claimedVouchersDt.createdRow = function (row, data, dataIndex) {
            if (!data.isActive) {
                $(row).addClass('lockedUser');
            }
            //if (!data.isApproved && data.roles[0].name == 'Admin') {
            //    $(row).addClass('pendingApprovalUser');
            //}
            // Recompiling so we can bind Angular directive to the DT
            $compile(angular.element(row).contents())($scope);
        };

        vm.claimedVouchersDt.rowCallback = function (row, data, dataIndex) {
            $compile(angular.element(row).contents())($scope);
        };

        vm.translateFilter = $filter('translate');
        vm.localizeStringFilter = $filter('localizeString');

        if ($rootScope.language.selected !== 'English') {
            vm.claimedVouchersDt.dtOptions = DTOptionsBuilder.newOptions()
                .withFnServerData(vm.claimedVouchersDt.serverData)
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
                .withOption('order', [[3, 'desc']])
                .withOption('createdRow', vm.claimedVouchersDt.createdRow)
                .withOption('rowCallback', vm.claimedVouchersDt.rowCallback).withBootstrap();
        }
        else {
            vm.claimedVouchersDt.dtOptions = DTOptionsBuilder.newOptions()
                .withFnServerData(vm.claimedVouchersDt.serverData)
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
                .withOption('order', [[3, 'desc']])
                .withOption('createdRow', vm.claimedVouchersDt.createdRow)
                .withOption('rowCallback', vm.claimedVouchersDt.rowCallback).withBootstrap();
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
            //if (data.roles[0].name == 'Admin') {
            //    htmlSection += '<span class="btn btn-icon waves-effect waves-light btn-info text-center" ng-click="vm.changePass(' +
            //        "'" + data.id + "'" + ')"><em class="fa fa-key" style="cursor:pointer" uib-tooltip="' +
            //        vm.translateFilter('Change Password') + '"></em></span><span class="btn btn-icon waves-effect waves-light btn-info text-center" ng-click="vm.configureAdmin(' +
            //        "'" + data.id + "'" + ')"><em class="mdi mdi-account-settings-variant" style="cursor:pointer" uib-tooltip="Approved/Modify No Of Events"></em></span>';
            //}
            //else
            {
                htmlSection += '<span class="btn btn-icon waves-effect waves-light btn-info text-center" ng-click="vm.changePass(' +
                    "'" + data.id + "'" + ')"><em class="fa fa-key" style="cursor:pointer" uib-tooltip="' +
                    vm.translateFilter('Change Password') + '"></em></span>';
            }
            if (data.isActive == true) {
                htmlSection += '<span class="btn btn-icon waves-effect waves-light btn-info text-center" ng-click="vm.lockUser(' +
                    "'" + data.id + "'" + ')"><em class="mdi mdi-lock-outline" style="cursor:pointer" uib-tooltip="Lock User"></em></span>';

            }
            else {
                htmlSection += '<span class="btn btn-icon waves-effect waves-light btn-info text-center" ng-click="vm.unlockUser(' +
                    "'" + data.id + "'" + ')"><em class="fa fa-unlock" style="cursor:pointer" uib-tooltip="UnLock User"></em></span>';


            }
            htmlSection += '</div>';
            return htmlSection;
        };

        vm.claimedVouchersDt.dtColumns = [
            DTColumnBuilder.newColumn(null).withTitle('Name').renderWith(
                function (data) {
                    if (data.user) {
                        return data.user.firstName + ' ' + data.user.lastName;
                    }
                }),
            DTColumnBuilder.newColumn(null).withTitle('Email').renderWith(
                function (data) {
                    if (data.user) {
                        return data.user.email;
                    }
                }),
            DTColumnBuilder.newColumn(null).withTitle('Type').renderWith(
                function (data) {
                    if (data.user) {
                        return data.user.registrationType.nameEn;
                    }
                }),
            //DTColumnBuilder.newColumn(null).withTitle('Restaurant').renderWith(
            //    function (data) {
            //        if (data.user1) {
            //            return data.user1.firstName + " " + data.user1.lastName;
            //        }
            //    }),
            DTColumnBuilder.newColumn('createdOn').withTitle("Time")
                .renderWith(function (date) {
                    return $filter('date')(date, 'medium');
                }),
            DTColumnBuilder.newColumn(null).withTitle('Claimed Food Voucher').renderWith(
                function (data) {
                    if (data) {
                        return data.claimedFoodVoucher;
                    }
                }),
            DTColumnBuilder.newColumn(null).withTitle('Claimed Drink Voucher').renderWith(
                function (data) {
                    if (data) {
                        return data.claimedDrinkVoucher;
                    }
                }),
            DTColumnBuilder.newColumn(null).withTitle('Assigned Food Voucher').renderWith(
                function (data) {
                    if (data.user) {
                        return data.user.assignedFoodVouchers;
                    }
                }),
            DTColumnBuilder.newColumn(null).withTitle('Assigned Drink Voucher').renderWith(
                function (data) {
                    if (data.user) {
                        return data.user.assignedDrinkVouchers;
                    }
                }),
            DTColumnBuilder.newColumn(null).withTitle('Used Food Voucher').renderWith(
                function (data) {
                    if (data.user) {
                        return data.user.usedFoodVouchers;
                    }
                }),
            DTColumnBuilder.newColumn(null).withTitle('Used Drink Voucher').renderWith(
                function (data) {
                    if (data.user) {
                        return data.user.usedDrinkVouchers;
                    }
                })
        ];
        //DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('general.actions')).notSortable()
        //    .renderWith(vm.actionsHtml).withOption('width', '20%')];

        vm.exportReportList = function () {
            if (vm.user.roleName == 1) {
                $http.get($rootScope.app.httpSource + 'api/ClaimedVoucher/GetClaimedVouchers?IsASuperAdmin=true')
                    .then(function (response) {
                        vm.reportDataList = response.data;
                        var reportData = [];
                        /* this line is only needed if you are not adding a script tag reference */
                        if (typeof XLSX == 'undefined') XLSX = require('xlsx');

                        for (var i = 0; i < vm.reportDataList.length; i++) {
                            var obj = {
                                "Name": vm.reportDataList[i].user.firstName + ' ' + vm.reportDataList[i].user.lastName,
                                "Email": vm.reportDataList[i].user.email,
                                "Phone Number": vm.reportDataList[i].user.phoneNumber,
                                "user Type": vm.reportDataList[i].user.registrationType.nameEn,
                                "Restaurant": vm.reportDataList[i].user1.firstName + " " + vm.reportDataList[i].user1.lastName,
                                "Event": vm.reportDataList[i].event.nameEn,
                                "Time": vm.dateFilter(vm.reportDataList[i].createdOn, 'medium'),
                                "Claimed Food Voucher": vm.reportDataList[i].claimedFoodVoucher,
                                "Claimed Drink Voucher": vm.reportDataList[i].claimedDrinkVoucher,
                                "Assigned Food Voucher": vm.reportDataList[i].user.assignedFoodVouchers,
                                "Assigned Drink Voucher": vm.reportDataList[i].user.assignedDrinkVouchers,
                                "Used Food Voucher": vm.reportDataList[i].user.usedFoodVouchers,
                                "Used Drink Voucher": vm.reportDataList[i].user.usedDrinkVouchers
                            };
                            reportData.push(obj);
                        }
                        /* make the worksheet */
                        var ws = XLSX.utils.json_to_sheet(reportData);

                        /* add to workbook */
                        var wb = XLSX.utils.book_new();
                        XLSX.utils.book_append_sheet(wb, ws, "Report");

                        /* write workbook (use type 'binary') */
                        var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });

                        /* generate a download */
                        function s2ab(s) {
                            var buf = new ArrayBuffer(s.length);
                            var view = new Uint8Array(buf);
                            for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
                            return buf;
                        }
                        saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), "Report.xlsx");
                    }, function (response) { });
            }
            else if (vm.user.roleName == 2) {
                $http.get($rootScope.app.httpSource + 'api/ClaimedVoucher/GetClaimedVouchers?IsASuperAdmin=false')
                    .then(function (response) {
                        vm.reportDataList = response.data;
                        var reportData = [];
                        /* this line is only needed if you are not adding a script tag reference */
                        if (typeof XLSX == 'undefined') XLSX = require('xlsx');

                        for (var i = 0; i < vm.reportDataList.length; i++) {
                            var obj = {
                                "Name": vm.reportDataList[i].user.firstName + ' ' + vm.reportDataList[i].user.lastName,
                                "Email": vm.reportDataList[i].user.email,
                                "Phone Number": vm.reportDataList[i].user.phoneNumber,
                                "user Type": vm.reportDataList[i].user.registrationType.nameEn,
                                "Restaurant": vm.reportDataList[i].user1.firstName + " " + vm.reportDataList[i].user1.lastName,
                                "Event": vm.reportDataList[i].event.nameEn,
                                "Time": vm.dateFilter(vm.reportDataList[i].createdOn, 'medium'),
                                "Claimed Food Voucher": vm.reportDataList[i].claimedFoodVoucher,
                                "Claimed Drink Voucher": vm.reportDataList[i].claimedDrinkVoucher,
                                "Assigned Food Voucher": vm.reportDataList[i].user.assignedFoodVouchers,
                                "Assigned Drink Voucher": vm.reportDataList[i].user.assignedDrinkVouchers,
                                "Used Food Voucher": vm.reportDataList[i].user.usedFoodVouchers,
                                "Used Drink Voucher": vm.reportDataList[i].user.usedDrinkVouchers
                            };
                            reportData.push(obj);
                        }
                        /* make the worksheet */
                        var ws = XLSX.utils.json_to_sheet(reportData);

                        /* add to workbook */
                        var wb = XLSX.utils.book_new();
                        XLSX.utils.book_append_sheet(wb, ws, "Report");

                        /* write workbook (use type 'binary') */
                        var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });

                        /* generate a download */
                        function s2ab(s) {
                            var buf = new ArrayBuffer(s.length);
                            var view = new Uint8Array(buf);
                            for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
                            return buf;
                        }
                        saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), "Report.xlsx");
                    }, function (response) { });
            }
            
        };
    }
})();