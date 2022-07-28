(function () {
    'use strict';

    angular.module('eventsapp')
        .controller('usersController', usersController);

    usersController.$inject = ['$rootScope', '$scope', 'UserProfile', '$filter', '$compile', '$http', '$uibModal', '$state', '$stateParams', 'SweetAlert', '$window', 'DTOptionsBuilder', 'DTColumnBuilder', 'moment'];

    function usersController($rootScope, $scope, UserProfile, $filter, $compile, $http, $uibModal, $state, $stateParams, SweetAlert, $window, DTOptionsBuilder, DTColumnBuilder, moment) {
        debugger;
        var vm = this;
        vm.productCategories = [];
        vm.openUserModelPopup = false;
        vm.openPasswordModelPopup = false;
        vm.users = [];
        vm.params = {
            searchtext: '',
            page: 1,
            pageSize: 9
        };

        $http.get($rootScope.app.httpSource + 'api/Roles/GetAll')
            .then(function (resp) {
                vm.roles = resp.data;
            },
            function (response) { });
        vm.filterParams = {};
        vm.usersDt = {};
        vm.usersDt.dtInstance = {};
        vm.usersDt.serverData = function (sSource, aoData, fnCallback, oSettings) {
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
                filterParams : vm.filterParams
            };

            //Then just call your service to get the records from server side           
            $http.post($rootScope.app.httpSource + 'api/User/GetUsers', vm.params)
                .then(function (resp) {
                    vm.users = resp.data.content;
                  
                    var records = {
                        'draw': draw,
                        'recordsTotal': resp.data.totalRecords,
                        'recordsFiltered': resp.data.totalRecords,
                        'data': resp.data.content
                    };
                    fnCallback(records);
                });
        };

        vm.usersDt.createdRow = function (row, data, dataIndex) {
            if (!data.isActive) {
                $(row).addClass('lockedUser');
            }
            //if (!data.isApproved && data.roles[0].name == 'Admin') {
            //    $(row).addClass('pendingApprovalUser');
            //}
            // Recompiling so we can bind Angular directive to the DT
            $compile(angular.element(row).contents())($scope);
        };

        vm.usersDt.rowCallback = function (row, data, dataIndex) {
            $compile(angular.element(row).contents())($scope);
        };

        vm.translateFilter = $filter('translate');
        vm.localizeStringFilter = $filter('localizeString');

        if ($rootScope.language.selected !== 'English') {
            vm.usersDt.dtOptions = DTOptionsBuilder.newOptions()
                .withFnServerData(vm.usersDt.serverData)
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
                .withOption('createdRow', vm.usersDt.createdRow)
                .withOption('rowCallback', vm.usersDt.rowCallback).withBootstrap();
        }
        else {
            vm.usersDt.dtOptions = DTOptionsBuilder.newOptions()
                .withFnServerData(vm.usersDt.serverData)
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
                .withOption('createdRow', vm.usersDt.createdRow)
                .withOption('rowCallback', vm.usersDt.rowCallback).withBootstrap();
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

        vm.usersDt.dtColumns = [
            DTColumnBuilder.newColumn(null).withTitle('Name').renderWith(
                function (data) {
                    return data.firstName + ' '+  data.lastName;
                }),
            DTColumnBuilder.newColumn('roles').withTitle('Roles').renderWith(
                function (data) {
                    if (data.length) {
                        return data[0].name;
                    } else {
                        return '';
                    }
                }),
            DTColumnBuilder.newColumn('createdOn').withTitle("Created On")
                .renderWith(function (date) {
                    return $filter('date')(date, 'dd-MMM-yyyy');
                }),
            DTColumnBuilder.newColumn('email').withTitle("Email"),  
            DTColumnBuilder.newColumn(null).withTitle(vm.translateFilter('general.actions')).notSortable()
                .renderWith(vm.actionsHtml).withOption('width', '20%')];

        vm.addUser = function () {
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Admin/Users/AddUser/addUser.html',
                controller: 'addUserController',
                size: 'lg',
                backdrop: 'static',
                keyboard: false,
                resolve: {
                    user: function () {
                        return null;
                    },
                    productCategories: function () {
                        return vm.productCategories;
                    },
                    roles: function () {
                        return vm.roles;
                    }
                }
            });

            modalInstance.result.then(function (user) {
                vm.users.push(user);
                vm.usersDt.dtInstance.rerender();
            }, function () {
                //state.text('Modal dismissed with Cancel status');
            });
        };

        vm.editUser = function (id) {
            if (vm.openUserModelPopup) { return; }

            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Admin/Users/AddUser/addUser.html',
                controller: 'addUserController',
                size: 'lg',
                backdrop: 'static',
                keyboard: false,
                resolve: {
                    user: function () {
                        return $filter('filter')(vm.users, { id: id }, true)[0];
                    },
                    productCategories: function () {
                        return vm.productCategories;
                    },
                    roles: function () {
                        return vm.roles;
                    }
                }
            });

            vm.openUserModelPopup = true;

            modalInstance.result.then(function (user) {
                for (var i = 0; i < vm.users.length; i++) {
                    if (vm.users[i].id === user.id) {
                        vm.users[i] = user;
                        break;
                    }
                }
                vm.openUserModelPopup = false;
                vm.usersDt.dtInstance.rerender();
            }, function () {
                //state.text('Modal dismissed with Cancel status');
                vm.openUserModelPopup = false;
            });
        };

        vm.lockUser = function (userId) {
            $http.get($rootScope.app.httpSource + 'api/User/UserLock?userId=' + userId + '&isLock=false')
                .then(function (resp) {
                    SweetAlert.swal("Succesful", "User Locked Succesfully", "success");
                    vm.usersDt.dtInstance.rerender();
                });
        }

        vm.unlockUser = function (userId) {
            $http.get($rootScope.app.httpSource + 'api/User/UserLock?userId=' + userId + '&isLock=true')
                .then(function (resp) {
                    SweetAlert.swal("Succesful", "User UnLocked Succesfully", "success");
                    vm.usersDt.dtInstance.rerender();
                });
        }
        vm.changePass = function (id) {
            if (vm.openPasswordModelPopup) { return; }
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Account/changePassword/changePassword.html',
                controller: 'changePassController',
                size: 'md',
                backdrop: 'static',
                keyboard: false,
                resolve: {
                    user: function () {
                        return vm.users.filter(function (item) { return item.id == id;})[0];
                    }
                }
            });

            vm.openPasswordModelPopup = true;

            modalInstance.result.then(function (user) {
                vm.openPasswordModelPopup = false;
            }, function () {
                //state.text('Modal dismissed with Cancel status');
                vm.openPasswordModelPopup = false;
            });
        };

        vm.configureAdmin = function (id) {
            if (vm.openPasswordModelPopup) { return; }
            var modalInstance = $uibModal.open({
                templateUrl: 'app/views/Account/configureAdmin/configureAdmin.html',
                controller: 'configureAdminController',
                size: 'md',
                backdrop: 'static',
                keyboard: false,
                resolve: {
                    user: function () {
                        return vm.users.filter(function (item) { return item.id == id; })[0];
                    }
                }
            });

            vm.openPasswordModelPopup = true;

            modalInstance.result.then(function (user) {
                vm.openPasswordModelPopup = false;
                vm.usersDt.dtInstance.rerender();
            }, function () {
                //state.text('Modal dismissed with Cancel status');
                    vm.openPasswordModelPopup = false;
                    vm.usersDt.dtInstance.rerender();
            });
        };
    }
})();