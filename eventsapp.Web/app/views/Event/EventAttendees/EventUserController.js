(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('eventUserController', eventUserController);

    eventUserController.$inject = ['$rootScope', '$http', 'UserProfile', '$scope', '$stateParams', '$state', 'WizardHandler', '$filter', 'DTOptionsBuilder',
        'DTColumnBuilder', '$compile', '$uibModal', 'SweetAlert', 'flotOptions'];
    function eventUserController($rootScope, $http, $scope, $stateParams, $state, WizardHandler, $filter, DTOptionsBuilder, DTColumnBuilder, $compile, $uibModal, SweetAlert, flotOptions) {
        var vm = this;
        vm.events = [];
        vm.eventId = 0;
        vm.downloadTemplateURL = "/webAPI/api/EventUser/GetAttendeeTemplate";
        vm.excelData = [];
        vm.translateFilter = $filter('translate');
        vm.dateFilter = $filter('date');
        vm.orderByFilter = $filter('orderBy');

        vm.attendance = [
            { id: 0, value: "Present" },
            { id: 1, value: "Absent" }
        ];

        $scope.controllerScope = $scope;
        vm.localizeString = $filter('localizeString');
        vm.localizeStringWithProperty = $filter('localizeStringWithProperty');
        vm.eventUsersDt = {};
        vm.eventUsersDt.dtInstance = {};
        vm.eventUsersDt.serverData = function (sSource, aoData, fnCallback, oSettings) {
            var aoDataLength = aoData.length;
            //All the parameters you need is in the aoData variable
            var draw = aoData[0].value;
            var order = aoData[2].value[0];
            var start = aoData[3].value;
            var length = aoData[4].value;
            var search = aoData[5].value;

            vm.params = {
                eventId: vm.params && vm.params.eventId ? vm.params.eventId : 0,
                searchtext: search.value ? search.value : null,
                page: (start / length) + 1,
                pageSize: length,
                sortBy: aoData[1].value[order.column].data,
                sortDirection: order.dir
            };

            //Then just call your service to get the records from server side           
            $http.post($rootScope.app.httpSource + 'api/EventUser/GetEventUsers', vm.params)
                .then(function (resp) {
                    vm.eventusers = resp.data.content;
                    for (var i = 0; i < vm.eventusers.length; i++) {
                        if (vm.eventusers[i].isAttended == true) {
                            vm.eventusers[i].attStatus = "Present";
                        }
                        else if (vm.eventusers[i].isAttended == false) {
                            vm.eventusers[i].attStatus = "Absent";
                        }
                    }
                    var records = {
                        'draw': draw,
                        'recordsTotal': resp.data.totalRecords,
                        'recordsFiltered': resp.data.totalRecords,
                        'data': vm.eventusers
                    };
                    fnCallback(records);
                });
        };

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

        vm.eventUsersDt.createdRow = function (row, data, dataIndex) {
            // Recompiling so we can bind Angular directive to the DT
            $compile(angular.element(row).contents())($scope);
        };

        vm.eventUsersDt.rowCallback = function (row, data, dataIndex) {
            $compile(angular.element(row).contents())($scope);
        };


        if ($rootScope.language.selected !== 'English') {
            vm.eventUsersDt.dtOptions = DTOptionsBuilder.newOptions()
                .withFnServerData(vm.eventUsersDt.serverData)
                .withOption('serverSide', true)
                .withDataProp('data')
                .withOption('processing', false)
                .withOption('aaSorting', [[0, 'asc']])
                .withOption('lengthMenu', [[10, 100, 1000, 5000], [10, 100, 1000, 5000]])
                .withPaginationType('full_numbers')
                .withDisplayLength(10)
                .withOption('responsive', {
                    details: {
                        renderer: renderer
                    }
                })
                .withLanguageSource('app/langs/ar.json')
                .withOption('bFilter', true)
                .withOption('paging', true)
                .withOption('info', true)
                .withOption('createdRow', vm.eventUsersDt.createdRow)
                .withOption('order', [[0, 'asc']])
                .withOption('rowCallback', vm.eventUsersDt.rowCallback).withBootstrap();
        }
        else {
            vm.eventUsersDt.dtOptions = DTOptionsBuilder.newOptions()
                .withFnServerData(vm.eventUsersDt.serverData)
                .withOption('serverSide', true)
                .withDataProp('data')
                .withOption('processing', false)
                .withOption('aaSorting', [[0, 'desc']])
                .withOption('lengthMenu', [[10, 100, 1000, 5000], [10, 100, 1000, 5000]])
                .withPaginationType('full_numbers')
                .withDisplayLength(10)
                .withOption('responsive', {
                    details: {
                        renderer: renderer
                    }
                })
                .withOption('bFilter', true)
                .withOption('paging', true)
                .withOption('info', true)
                .withOption('createdRow', vm.eventUsersDt.createdRow)
                .withOption('order', [[0, 'asc']])
                .withOption('rowCallback', vm.eventUsersDt.rowCallback).withBootstrap();
        }

        vm.eventUsersDt.actionsHtml = function (data, type, full, meta) {
            var htmlSection = '';
            //for (var i = 0; i < vm.eventusers.length; i++) {
            //    var index = 0;
            //    if (vm.eventusers[i].isAttended == true) {
            //        vm.eventusers[i].attStatus = "Present";
            //    }
            //    else if (vm.eventusers[i].isAttended == false) {
            //        vm.eventusers[i].attStatus = "Absent";
            //    }
            //    if (vm.eventusers[i].id == data.id) {
            //        index = i;
            //        break;
            //    }
            //}
            //htmlSection = `<ui-select ng-model="vm.eventusers[` + meta.row + `].attStatus" ng-select="vm.userAttended(vm.eventusers[` + meta.row + `])"><ui-select-match placeholder="Select"> <span ng-bind-html="$select.selected.value"></span></ui-select-match ><ui-select-choices repeat="att.value as att in vm.attendance"><span ng-bind-html="att.value"></span></ui-select-choices></ui-select >`;

            htmlSection = '<div class="checkbox checkbox-custom"><input id="userAttended' + meta.row + '"  ng-change="vm.userAttended(vm.eventusers[' + meta.row + '])" type="checkbox" name="userAttended' + meta.row + '" ng-model="vm.eventusers[' + meta.row + '].isAttended" ng-checked="vm.eventusers[' + meta.row + '].isAttended == true"><label for="HasSponsorsCkb"></label></div>';
            return htmlSection;
        };

        vm.eventUsersDt.printactionsHtml = function (data, type, full, meta) {
            var htmlSection = '';
            htmlSection = '<div class="list-icon"><div class="btn btn-icon waves-effect waves-light btn-info" ng-disabled="' + !data.badgePrintEnabled + '" ng-click="vm.printBatch(' + data.id + ', $event)" > <em class="fa fa-print" style="cursor:pointer" uib-tooltip="' + vm.translateFilter('general.print') + '"></em></div ></div > ';
            return htmlSection;
        };

        vm.eventUsersDt.dtColumns = [
            DTColumnBuilder.newColumn('user.firstName').withTitle(vm.translateFilter('users.firstName')),
            DTColumnBuilder.newColumn('user.lastName').withTitle(vm.translateFilter('users.lastName')),
            DTColumnBuilder.newColumn('user.email').withTitle(vm.translateFilter('users.email')),
            DTColumnBuilder.newColumn('user.phoneNumber').withTitle(vm.translateFilter('users.phoneNumber')),
            DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('users.type'))
                .renderWith(function (data) {
                    return vm.localizeString(data.registrationType, 'nameEn', 'nameAr');
                }),
            DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('users.event'))
                .renderWith(function (data) {
                    return vm.localizeString(data.event);
                }),
            DTColumnBuilder.newColumn('agendum').withTitle(vm.translateFilter('event.agenda'))
                .renderWith(function (data) {
                    if (data) {
                        return vm.localizeStringWithProperty(data, 'title') + ' ( ' + vm.dateFilter(data.startDate, 'shortDate') + ' ) ';
                    } else {
                        return '';
                    }
                }),
            DTColumnBuilder.newColumn('isAttended').withTitle(vm.translateFilter('users.attended'))
                .renderWith(vm.eventUsersDt.actionsHtml),
            DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('users.print')).notSortable()
                .renderWith(vm.eventUsersDt.printactionsHtml)
        ];

        vm.printBatch = function (attendeid, event) {
            debugger;

            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Event/EventAttendees/EventPrint.html',
                controller: 'eventPrintController',
                closable: true,
                animation: true,
                backdrop: 'static',
                keyboard: false,
                size: 'lg',
                resolve: {
                    attendeid: function () {
                        return attendeid;
                    }
                }
            });
            modalInstance.result.then(function (attendeid) {
                vm.setPage(1);
            }, function () {
            });

        };

        vm.userAttended = function (registeredEvent) {
            debugger;
            //registeredEvent.isAttended = !registeredEvent.isAttended;
            $http.post($rootScope.app.httpSource + 'api/EventUser/RegisteredUserAttended', registeredEvent)
                .then(function (response) {
                    //for (var i = 0; i < vm.eventusers.length; i++) {
                    //    if (vm.eventusers[i].id == registeredEvent.id) {
                    //        vm.eventusers[i] = registeredEvent;
                    //        break;
                    //    }
                    //}
                    vm.eventUsersDt.dtInstance.rerender();
                },
                    function (response) { });
        };
        vm.columnHeader = ["FirstName", "LastName", "Email", "Phone"];
        $scope.ExcelExport = function (event) {
            debugger;
            var translate = $filter('translate');
            if (vm.filterParams.EventId == 0) {
                SweetAlert.swal(translate('general.information'), translate('general.eventIdnotSelected'), "error");
                return;
            }

            var input = event.target;
            var reader = new FileReader();
            reader.onload = function () {
                var fileData = reader.result;
                if (fileData.size > 20971520) {
                    SweetAlert.swal(translate('general.limitExceed'), translate('general.fileSizeGreater'), "error");
                    return;
                }
                var wb = XLSX.read(fileData, { type: 'binary' });

                /* grab first sheet */
                const wsname = wb.SheetNames[0];
                const ws = wb.Sheets[wsname];
                var header = (XLSX.utils.sheet_to_json(ws, { header: 1 }));
                if (JSON.stringify(header[0]).toLowerCase() != JSON.stringify(vm.columnHeader).toLowerCase()) {
                    SweetAlert.swal(translate('general.information'), translate('general.headersMismatched'), "error");
                    return;
                }

                wb.SheetNames.forEach(function (sheetName) {
                    var rowObj = XLSX.utils.sheet_to_row_object_array(wb.Sheets[sheetName]);
                    $http.post($rootScope.app.httpSource + 'api/EventUser/AddEventUser?eventId=' + vm.filterParams.EventId, rowObj)
                        .then(function (response) {
                            debugger;
                            SweetAlert.swal(translate('general.userImport'), translate('general.successImportMsg'), "success");
                            $scope.onSelectedEvent(vm.filterParams.EventId);
                        }, function (error) {
                            SweetAlert.swal(translate('general.error'), translate('general.invalidData'), "error");
                        });
                });
            };
            reader.onsuccess = function () {
            }
            reader.onerror = function (ex) {
                console.log(ex);
            }
            reader.readAsBinaryString(input.files[0]);
        };
        $scope.reset = function () {
            $('#file').wrap('<form>').closest('form').get(0).reset();
            $('#file').unwrap();
        };

        $scope.getUsers = function () {
            var eventId = vm.filterParams.EventId == undefined ? 0 : vm.filterParams.EventId;
            vm.params.eventId = eventId;
            vm.eventUsersDt.Instance.DataTable.draw();
        };

        $http.get($rootScope.app.httpSource + 'api/Event/GetEventsLookUp')
            .then(function (response) {
                vm.events = response.data;
            }, function (response) { });

        vm.exportAttendeesList = function () {
            var attendeesData = [];
            /* this line is only needed if you are not adding a script tag reference */
            if (typeof XLSX == 'undefined') XLSX = require('xlsx');

            for (var i = 0; i < vm.eventusers.length; i++) {
                var obj = {
                    "Fisrt Name": vm.eventusers[i].user.firstName,
                    "Last Name": vm.eventusers[i].user.lastName,
                    "Email": vm.eventusers[i].user.email,
                    "Phone Number": vm.eventusers[i].user.phoneNumber,
                    "Is Attended": vm.eventusers[i].isAttended ? 'Present' : 'Absent',
                    "Event": vm.localizeString(vm.eventusers[i].event),
                    "Event Date": vm.dateFilter(vm.eventusers[i].event.startDate, 'shortDate'),
                    "Slot Date": vm.dateFilter(vm.eventusers[i].agendum.startDate, 'shortDate'),
                    "Slot": vm.localizeStringWithProperty(vm.eventusers[i].agendum, 'title'),
                    "Time": vm.eventusers[i].agendum.fromTime + " - " + vm.eventusers[i].agendum.toTime,
                    "Visitor Count": vm.eventusers[i].visitorCount
                };
                attendeesData.push(obj);
            }
            /* make the worksheet */
            var ws = XLSX.utils.json_to_sheet(attendeesData);

            /* add to workbook */
            var wb = XLSX.utils.book_new();
            XLSX.utils.book_append_sheet(wb, ws, "Attendees");

            /* write workbook (use type 'binary') */
            var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });

            /* generate a download */
            function s2ab(s) {
                var buf = new ArrayBuffer(s.length);
                var view = new Uint8Array(buf);
                for (var i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xFF;
                return buf;
            }
            saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), "Attendees List.xlsx");
        };

        //function removeDuplicateValue(myArray){
        //    var newArray = [];
        //    myArray.forEach(function (val) {
        //        var exists = false;
        //        newArray.forEach(function (val2) {
        //            if (val[0] == val2[0])
        //                exists = true;
        //        });
        //        if (exists == false)
        //            newArray.push(val);
        //    });
        //    return newArray;
        //}
    }
    eventUserController.$inject = ['$rootScope', '$http', '$scope', '$stateParams', '$state', 'WizardHandler', '$filter', 'DTOptionsBuilder',
        'DTColumnBuilder', '$compile', '$uibModal', 'SweetAlert', 'flotOptions'];

})();