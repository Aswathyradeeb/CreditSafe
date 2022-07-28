(function () {
    'use strict';

    angular
        .module('eventsapp')
        .directive('datatableDirective', datatableDirective);

    datatableDirective.$inject = ['$rootScope', '$http', '$filter', 'DTOptionsBuilder', 'DTColumnBuilder', '$compile', '$state', '$timeout', '$q']

    function datatableDirective($rootScope, $http, $filter, DTOptionsBuilder, DTColumnBuilder, $compile, $state, $timeout, $q) {
        return {
            restrict: 'E',
            scope: {
                controlModel: "=ngModel",
                datatableId: "=?",
                dtColumns: "=?",
                dtInstance: "=?",
                controllerScope: "=controllerScope",
                rowCallback: "=?",
                orderBy: "=?"
            },
            templateUrl: '/app/views/Controls/dataTableDirective/dataTableDirectiveTemplate.html',
            link: link
        };

        function link(scope, element, attrs) {
            var unwatch = scope.$watch('controlModel', function (newVal, oldVal) {
                if (newVal) {
                    init();
                    unwatch();
                }
            });

            function init() {

                scope.loadTable = false;
                scope.translateFilter = $filter('translate');
                scope.localizeFilter = $filter('localizeString');
                scope.total = 0;
                scope.toggleValue = [];

                scope.dtIntanceCallback = function (instance) {
                    scope.dtInstance = instance;
                };
                scope.reloadTable = function () {
                    if (scope.dtInstance.rerender) {
                        scope.dtInstance.rerender();
                        scope.dtInstance.DataTable.draw();
                    }
                };

                scope.$watch('controlModel', function (newVal, oldVal) {
                    if (newVal) {
                        if (scope.controlModel != null) {
                            scope.reloadTable();
                        }
                    }
                });

                scope.createdRow = function (row, data, dataIndex) {
                    // Recompiling so we can bind Angular directive to the DT
                    $compile(angular.element(row).contents())(scope.controllerScope);
                }
                if (scope.rowCallback == undefined) {
                    scope.rowCallback = function (row, data, dataIndex) {
                        // Recompiling so we can bind Angular directive to the DT
                        //$compile(angular.element(row).contents())(scope.controllerScope);
                    };
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
                        $compile(angular.element($('<ul data-dtr-index="' + rowIdx + '"/>').append(data)))(scope.controllerScope) :
                        false;
                }
                if ($rootScope.language.selected !== 'English') {
                    scope.dtOptions = DTOptionsBuilder.fromFnPromise(
                        function () {
                            var deferred = $q.defer();
                            deferred.resolve(scope.controlModel);
                            return deferred.promise;
                        })
                        //$timeout(function () {
                        //return scope.controlModel;
                        //})

                        //.newOptions() 
                        //.withOption('serverSide', true)
                        //.withDataProp('data')
                        .withOption('processing', false)
                        .withOption('responsive', {
                            details: {
                                renderer: renderer
                            }
                        })
                        .withLanguageSource('app/langs/ar.json')
                        .withOption('bFilter', true)
                        .withOption('paging', true)
                        .withPaginationType('full_numbers')
                        .withDisplayLength(10)
                        .withOption('info', true)
                        .withOption('createdRow', scope.createdRow)
                        .withOption('order', [[scope.orderBy != undefined ? scope.orderBy : 1, 'asc']])
                        .withOption('rowCallback', scope.rowCallback).withBootstrap().withBootstrapOptions({
                            TableTools: {
                                classes: {
                                    container: 'btn-group',
                                    buttons: {
                                        normal: 'btn btn-danger'
                                    }
                                }
                            },
                            pagination: {
                                classes: {
                                    ul: 'pagination pagination-sm'
                                }
                            }
                        });
                }
                else {
                    scope.dtOptions = DTOptionsBuilder.fromFnPromise(
                        function () {
                            var deferred = $q.defer();
                            deferred.resolve(scope.controlModel);
                            return deferred.promise;
                        })
                        //fromFnPromise($timeout(function () {
                        //    return scope.controlModel;
                        //}))
                        //.withOption('serverSide', true)
                        //.withDataProp('data')
                        .withOption('processing', false)
                        .withOption('responsive', {
                            details: {
                                renderer: renderer
                            }
                        })
                        .withLanguageSource('app/langs/en.json')
                        .withOption('bFilter', true)
                        .withOption('paging', true)
                        .withPaginationType('full_numbers')
                        .withDisplayLength(10)
                        .withOption('info', true)
                        .withOption('createdRow', scope.createdRow)
                        .withOption('order', [[scope.orderBy != undefined ? scope.orderBy : 1, 'asc']])
                        .withOption('rowCallback', scope.rowCallback).withBootstrap().withBootstrapOptions({
                            TableTools: {
                                classes: {
                                    container: 'btn-group',
                                    buttons: {
                                        normal: 'btn btn-danger'
                                    }
                                }
                            },
                            pagination: {
                                classes: {
                                    ul: 'pagination pagination-sm'
                                }
                            }
                        });
                }

                scope.returnValue = function () {
                    return scope.controlModel;
                }
                scope.loadTable = true;
            }
        }
    }
})();