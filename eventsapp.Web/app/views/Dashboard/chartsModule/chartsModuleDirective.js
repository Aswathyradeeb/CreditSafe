/**=========================================================
 * Module: chartsModule
 * Reuse cases of uploading files
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('eventsapp')
        .directive('chartsModule', chartsModule);

    chartsModule.$inject = ['$rootScope', '$http', '$filter', 'DTOptionsBuilder', 'DTColumnBuilder', '$compile', '$uibModal', '$timeout', 'moment'];

    function chartsModule($rootScope, $http, $filter, DTOptionsBuilder, DTColumnBuilder, $compile, $uibModal, $timeout, moment) {
        return {
            restrict: 'E',
            scope: {
                ChartData: "=ngModel"
            },
            templateUrl: '/app/views/Dashboard/chartsModule/chartsModuleDirectiveTemplate.html',
            link: link
        };

        function link(scope, element, attrs) {

            var unwatch = scope.$watch('ChartData', function (newVal, oldVal) {
                if (newVal != undefined) {
                    debugger;
                    init();
                    //unwatch();
                }
            });


            function init() {
                Highcharts.setOptions({
                    colors: ['#8d4654', '#fe9700', '#caae5f', '#DDDF00', '#64E572', '#FF9655', '#FFF263', '#6AF9C4'],
                    lang: {
                        decimalPoint: '.',
                        thousandsSep: ','
                    }
                });
                scope.TestChart = {
                    chart: {
                        renderTo: 'chart539',
                        type: 'column'
                    },
                    title: {
                        text: 'Basic drilldown'
                    },
                    xAxis: {
                        type: 'category',
                        //categories:["Animals"]
                    },

                    plotOptions: {
                        series: {
                            borderWidth: 0,
                            dataLabels: {
                                enabled: true,
                            }
                        }
                    },

                    series: [{
                        name: 'Things',
                        colorByPoint: true,
                        data: [{
                            name: 'Animals',
                            y: 5,
                            drilldown: 'animals'
                        }]
                    }],
                    drilldown: {
                        activeDataLabelStyle: { color: 'red' },
                        series: [{
                            id: 'animals',
                            name: 'Animals',
                            data: [{
                                name: 'Cats',
                                y: 4,
                                drilldown: 'cats'
                            }, ['Dogs', 2],
                            ['Cows', 1],
                            ['Sheep', 2],
                            ['Pigs', 1]
                            ]
                        }, {

                            id: 'cats',
                            data: [1, 2, 3]
                        }]
                    }
                };
                scope.filterParams = {};
                scope.exportExcel = function () {
                    //var params = {
                    //    graph: chartConfigItem.series,
                    //    categories : chartConfigItem.xAxis.categories
                    //}
                    //scope.ChartData.graph[0].columnClustered = chartConfigItem.chart.type;
                    $http.post($rootScope.app.httpSource + 'api/KPI/ExportExcel', scope.ChartData.graph, { responseType: 'arraybuffer' })
                        .then(function (resp) {
                            debugger;
                            var data = new Blob([resp.data], { type: 'application/vnd.ms-excel' });
                            saveAs(data, "KPIGraph.xlsx");
                        },
                            function (response) {
                            });
                };
                scope.localizeString = $filter('localizeString');
                scope.isReadonly = false;
                scope.selectAll = true;
                scope.chart_Type = 'chart-bar';
                if (!scope.ChartData.enableStacked) {
                    scope.chartTypes = [
                        {
                            "id": 1,
                            "nameEn": "column",
                            "nameAr": "أعمدة",
                            "type": "column"
                        },
                        {
                            "id": 2,
                            "nameEn": "column - 3d",
                            "nameAr": "3d -أعمدة",
                            "type": "column"
                        },
                        {
                            "id": 3,
                            "nameEn": "line",
                            "nameAr": "خطوط",
                            "type": "line"
                        },
                        {
                            "id": 5,
                            "nameEn": "pie",
                            "nameAr": "دائري",
                            "type": "pie"
                        },
                        {
                            "id": 6,
                            "nameEn": "pie - 3d",
                            "nameAr": "دائري",
                            "type": "pie"
                        },
                        {
                            "id": 7,
                            "nameEn": "heatmap",
                            "nameAr": "heatmap",
                            "type": "heatmap"
                        },
                        {
                            "id": 9,
                            "nameEn": "bar",
                            "nameAr": "bar",
                            "type": "bar"
                        }
                    ];
                }
                else {
                    scope.chartTypes = [
                        {
                            "id": 10,
                            "nameEn": "column",
                            "nameAr": "أعمدة",
                            "type": "column"
                        },
                        {
                            "id": 11,
                            "nameEn": "Line",
                            "nameAr": "خطوط",
                            "type": "line"
                        }
                    ];
                }
                scope.activeStep == 1;
                scope.controllerScope = scope;
                scope.translateFilter = $filter('translate');


                //scope.viewApplications = function (item1, item2) {
                //    debugger;
                //    if (item1 != undefined && item1 != "" && item2 == "Applications By Statuses KPI") {
                //        scope.filterParams = { searchtext: item1, services: [], productCategories: [], NotifiedBody: [] }
                //        if (scope.ChartData.selectedService != null) {
                //            scope.filterParams.services.push(scope.ChartData.selectedService);
                //        }
                //        if (scope.ChartData.selectedProductCategory != null) {
                //            scope.filterParams.productCategories.push(scope.ChartData.selectedProductCategory);
                //        }
                //        if (scope.ChartData.selectedNotifiedBody != null) {
                //            scope.filterParams.NotifiedBody.push(scope.ChartData.selectedNotifiedBody);
                //        }
                //        scope.params = {
                //            searchtext: null,
                //            page: 1,
                //            pageSize: 1000000,
                //            sortBy: ('id'),
                //            sortDirection: 'desc',
                //            filterParams: ((Object.keys(scope.filterParams).length == 0) ? null : scope.filterParams)
                //        };
                //        $http.post($rootScope.app.httpSource + 'api/Application/GetApplications', scope.params)
                //            .then(function (resp) {
                //                var modalInstance = $uibModal.open({
                //                    templateUrl: '/applications.html',
                //                    size: 'xl',
                //                    controller: applicationsModalCtrl,
                //                    resolve: {
                //                        applications: function () {
                //                            return resp.data.content;
                //                        }
                //                    }
                //                });

                //            });
                //    }
                //};
                //var applicationsModalCtrl = function ($scope, $uibModalInstance, applications) {
                //    $scope.applications = angular.copy(applications);
                //    $scope.dtApplciationsInstance = {};
                //    $scope.controllerScope = $scope;
                //    $scope.translateFilter = $filter('translate');
                //    $scope.dtApplicationsColumns = [DTColumnBuilder.newColumn(null).withTitle($scope.translateFilter('dashboard.applicationNumber')).withOption('width', '100px').renderWith(function (data, type, full, meta) {
                //        if (data.applicationNumber != null) {
                //            return '<div uibt-tooltip="' + $filter('localizeStringWithProperty')(data, 'service') + '">' + data.applicationNumber + '</div>';
                //        }
                //        else if (data.requestNumber != null) {
                //            return '<div uibt-tooltip="' + $filter('localizeStringWithProperty')(data, 'service') + '">' + data.requestNumber + '</div>';
                //        }
                //    }).notSortable(),
                //    DTColumnBuilder.newColumn(null).withTitle($scope.translateFilter('eqm.productCategory')).renderWith(function (data, type, full, meta) {
                //        debugger;
                //        if (data.productCategoryEn != null) {
                //            return '<div>' + $filter('localizeStringWithProperty')(data, 'productCategory') + '</div>';
                //        }
                //        else {
                //            return '';
                //        }
                //    }).notSortable(),
                //    //DTColumnBuilder.newColumn(null).withTitle($scope.translateFilter('application.applicationStatus')).renderWith(function (data, type, full, meta) {
                //    //    debugger;
                //    //    if (data.applicationStatusEn != null) {
                //    //        return '<div>' + $filter('localizeStringWithProperty')(data, 'applicationStatus') + '</div>';
                //    //    }
                //    //    else {
                //    //        return '';
                //    //    }
                //    //}).notSortable(),
                //    DTColumnBuilder.newColumn(null).withTitle($scope.translateFilter('general.handledBy')).renderWith(function (data, type, full, meta) {
                //        if (data.rfidVendorEn != null) {
                //            return '<div>' + $filter('localizeStringWithProperty')(data, 'rfidVendor') + '</div>';
                //        }
                //        else if (data.notifiedBodyEn != null) {
                //            return '<div>' + $filter('localizeStringWithProperty')(data, 'notifiedBody') + '</div>';
                //        }
                //        else if (data.thirdPartyEn != null) {
                //            return '<div>' + $filter('localizeStringWithProperty')(data, 'thirdParty') + '</div>';
                //        }
                //        else {
                //            return $scope.translateFilter('title');
                //        }
                //    }).notSortable(),
                //    DTColumnBuilder.newColumn(null).withTitle($scope.translateFilter('workflow.Organization')).renderWith(function (data, type, full, meta) {
                //        if (data.orgNameEn != null) {
                //            return '<div>' + $filter('localizeStringWithProperty')(data, 'orgName') + '</div>';
                //        }
                //    }).notSortable(),
                //    DTColumnBuilder.newColumn(null).withTitle($scope.translateFilter('dashboard.consumedTimeLabel')).renderWith(
                //        function (data, type) {
                //            if (data != undefined) {
                //                if ((data.consumedTime % 1440) == 0) {
                //                    return moment.duration(data.consumedTime, "minutes").format("d [" + $scope.translateFilter('dashboard.days') + "]");
                //                }
                //                else {
                //                    return moment.duration(data.consumedTime, "minutes").format("d [" + $scope.translateFilter('dashboard.days') + "], h [" + $scope.translateFilter('dashboard.hours') +
                //                        "], m [" + $scope.translateFilter('dashboard.minutes') + "]");
                //                }
                //            }
                //            else {
                //                return '';
                //            }
                //        }).notSortable(),
                //    DTColumnBuilder.newColumn(null).withTitle($scope.translateFilter('dashboard.createdOn')).withOption('width', '100px').renderWith(
                //        function (data, type) {
                //            if (data != undefined) {
                //                return moment(data.createdOn).format('DD-MMM-YYYY');
                //            }
                //            else {
                //                return '';
                //            };
                //        }),
                //    //DTColumnBuilder.newColumn(null).withTitle($scope.translateFilter('dashboard.applicationStatus')).renderWith(
                //    //    function (data, type) {

                //    //        if (data != undefined) {

                //    //            return $filter('localizeStringWithProperty')(data, 'applicationStatus');
                //    //        }
                //    //        else {
                //    //            return '';
                //    //        }
                //    //    }).notSortable(),
                //    DTColumnBuilder.newColumn(null).withTitle($scope.translateFilter('general.actions')).notSortable()
                //        .renderWith(actionsHtml).withOption('width', '200px')];

                //    function actionsHtml(data, type, full, meta) {
                //        var htmlSection = '';

                //        var index = $scope.applications.indexOf(data);

                //        htmlSection = $scope.bindButtons(data, index);

                //        return htmlSection;
                //    };

                //    $scope.bindButtons = function (data, index) {
                //        var htmlSection = '';
                //        htmlSection = '<div style="display:inline-block" class="list-icon"><workflow-action ng-model="applications[' + index +
                //            '] " reload-data="reloadData"></workflow-action></div>';

                //        //htmlSection = htmlSection + '<button>Submit</button>';


                //        return htmlSection;
                //    };
                //    $scope.reloadData = function () {
                //        if ($scope.dtApplciationsInstance.DataTable != undefined)
                //            $scope.dtApplciationsInstance.DataTable.draw();
                //    }
                //    $scope.cancel = function () {
                //        $uibModalInstance.dismiss('cancel');
                //    };
                //};


                scope.downloadResults = function () {
                    var model = { evaluationRoundId: scope.filterParams.evaluationRound.id, applicationNumber: scope.filterParams.applicationNumber }
                    $http.post($rootScope.app.httpSource + 'api/Statistics/DownloadResults', model, { responseType: 'arraybuffer' })
                        .then(function (resp) {
                            debugger;
                            var data = new Blob([resp.data], { type: 'application/vnd.ms-excel' });
                            saveAs(data, "EvaluationResults.xlsx");
                        },
                            function (response) {
                            });
                };

                scope.rerenderChart = function (kpiListArray, loadFirstTime) {
                    debugger;
                    if (kpiListArray.selectedChartType == undefined) {
                        kpiListArray.selectedChartType = scope.chartTypes[4];
                    }
                    var screenWidth = (window.innerWidth > 0) ? window.innerWidth : screen.width;
                    var maxCount = 9
                    var barWidth = kpiListArray.barWidth != null ? kpiListArray.barWidth : 40;
                    if (screenWidth < 700 || kpiListArray.selectedChartType.id == 9) {
                        barWidth = 20;
                    }
                    if (!kpiListArray && kpiListArray.graph != undefined) {
                        return;
                    }

                    //if (kpiListArray.selectedLabel == undefined && kpiListArray.labels.length == 1) {
                    //    kpiListArray.selectedLabel = kpiListArray.labels[0];
                    //}
                    if (kpiListArray.selectedSeries == undefined && kpiListArray.series.length == 1) {
                        kpiListArray.selectedSeries = kpiListArray.series[0];
                    }

                    if (loadFirstTime == true) {
                        if (kpiListArray.labels) {
                            var defaultLabel = kpiListArray.labels.filter(function (item) {
                                if (item.isDefault == true) {
                                    return item;
                                }
                            });

                            if (defaultLabel != undefined && defaultLabel.length > 0) {
                                //KPIListArray.selectedLabel = defaultLabel[0];
                            }
                        }
                    }

                    kpiListArray.chartConfig = {

                        chart: {
                            backgroundColor: kpiListArray.bgColor,
                            renderTo: 'chart' + kpiListArray.id,
                            type: kpiListArray.selectedChartType == undefined ? kpiListArray.chartType : kpiListArray.selectedChartType.type,

                        },
                        title: {
                            text: '',//$filter('localizeString')(kpiListArray),
                            style: {
                                color: kpiListArray.textColor
                            }
                        },
                        credits: {
                            text: $filter('localizeString')($rootScope.app),
                            href: 'javascript:window.open("https://www.inlogic.ae/", "_blank")',
                            style: {
                                color: kpiListArray.textColor,
                                fontSize: '10px',
                                fontWeight: 'bold'
                            }
                        },
                        series: [],
                        yAxis: {
                            labels: {
                                formatter: function () {
                                    return Highcharts.numberFormat(this.value, 0);
                                },
                                style: {
                                    color: kpiListArray.textColor
                                }
                            }
                        },
                        xAxis: {
                            labels: {
                                formatter: function () {
                                    return this.value;
                                },
                                style: {
                                    color: kpiListArray.textColor
                                }
                            }
                        },
                        colors: [kpiListArray.defaultColor, '#8d4654', '#fe9700', '#caae5f', '#DDDF00', '#64E572', '#FF9655', '#FFF263', '#6AF9C4'],
                        plotOptions: {
                            line: {
                                dataLabels: {
                                    crop: false,
                                    overflow: 'none',
                                    padding: 10.6,
                                    enabled: true,
                                    style: {
                                        color: kpiListArray.textColor
                                    }
                                },
                                cursor: 'pointer',
                                point: {
                                    events: {
                                        //click: function () {
                                        //    scope.viewApplications(this.category, $filter('localizeString')(kpiListArray))
                                        //}
                                    }
                                }
                            },
                            column: {
                                dataLabels: {
                                    crop: false,
                                    overflow: 'none',
                                    padding: 10.6,
                                    enabled: true,
                                    style: {
                                        color: kpiListArray.textColor
                                    }
                                },
                                cursor: 'pointer',
                                point: {
                                    events: {
                                        //click: function () {
                                        //    scope.viewApplications(this.category, $filter('localizeString')(kpiListArray))
                                        //}
                                    }
                                }
                            },
                            pie: {
                                allowPointSelect: true,
                                cursor: 'pointer',
                                dataLabels: {
                                    enabled: true,
                                    format: '<b>{point.name}</b>: {point.y}',
                                    style: {
                                        color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                                    }
                                },
                                //allowPointSelect: true,
                                //cursor: 'pointer',
                                //dataLabels: {
                                //    crop: false,
                                //    overflow: 'none',
                                //    padding: 10.6,
                                //    style: {
                                //        color: kpiListArray.textColor,
                                //        width: '100px'
                                //    },
                                //    enabled: true,
                                //    //format:  '{point.name}' + ' / ' + '{point.y}', // \u202B is RLE char for RTL support
                                //    useHTML: true,
                                //    formatter: function () {
                                //        return ("<span class='datalabel'>" + this.key + ' / ' + Highcharts.numberFormat(this.y) + "</span>");
                                //    }
                                //},
                                point: {
                                    events: {
                                        //click: function () {
                                        //    scope.viewApplications(this.name, $filter('localizeString')(kpiListArray))
                                        //}
                                    }
                                }
                            },
                            heatmap: {
                                dataLabels: {
                                    enabled: false,
                                    style: {
                                        color: kpiListArray.lightColor
                                    },
                                    states: {
                                        hover: {
                                            enabled: true,
                                        }
                                    }
                                },
                                //events: {
                                //    mouseOut: function () {
                                //        this.chart.hoverPoints.forEach(p => {
                                //            p.update({
                                //                dataLabels: {
                                //                    enabled: false
                                //                }
                                //            }, false, false);
                                //        });
                                //    }
                                //},
                                //point: {
                                //    events: {
                                //        mouseOver: function (e) {
                                //            this.series.data.forEach(p => {
                                //                p.update({
                                //                    dataLabels: {
                                //                        enabled: false
                                //                    }
                                //                },false,false)
                                //            });
                                //            this.update({
                                //                dataLabels: {
                                //                    enabled: true
                                //                }
                                //            })                                         
                                //        }
                                //    }
                                //},
                                point: {
                                    events: {
                                        //click: function () {
                                        //    scope.viewApplications(this.category, $filter('localizeString')(kpiListArray))
                                        //}
                                    }
                                }
                            },
                            map: {
                                dataLabels: {
                                    crop: false,
                                    overflow: 'none',
                                    padding: 10.6,
                                    enabled: true,
                                    useHTML: true,
                                    style: {
                                        color: kpiListArray.textColor
                                    },
                                    formatter: function () {
                                        return ("<span class='datalabel'>" + this.key + ' / ' + Highcharts.numberFormat(this.y) + "</span>");
                                    },
                                    cursor: 'pointer'
                                },
                                point: {
                                    events: {
                                        click: function () {
                                            scope.open(this.category);
                                        }
                                    }
                                }
                            },
                            series: {
                                pointWidth: barWidth,
                                dataLabels: {
                                    crop: false,
                                    overflow: 'none',
                                    padding: 10.6,
                                    style: {
                                        color: kpiListArray.textColor
                                    }
                                }
                            }
                        },
                        scrollbar: {
                            enabled: true,
                            barBackgroundColor: '#9E9E9E',
                            barBorderRadius: 7,
                            barBorderWidth: 1,
                            buttonBackgroundColor: '#9E9E9E',
                            buttonBorderWidth: 1,
                            buttonBorderRadius: 7,
                            trackBackgroundColor: 'none',
                            trackBorderWidth: 1,
                            trackBorderRadius: 8,
                            trackBorderColor: '#9E9E9E'
                        }
                    };

                    kpiListArray.chartConfig.yAxis = {
                        gridLineColor: 'transparent',
                        lineWidth: 0,
                        minorGridLineWidth: 0,
                        lineColor: 'transparent',
                        labels: {
                            formatter: function () {
                                if (kpiListArray.series[0].unit != null) {
                                    return Highcharts.numberFormat(this.value, 0) + ' ' + $filter('localizeStringWithProperty')(kpiListArray.series[0].unit, 'symbol');
                                }
                                else {
                                    return Highcharts.numberFormat(this.value, 0);
                                }
                            },
                            style: {
                                color: kpiListArray.textColor
                            }
                        },
                        title: {
                            enabled: true,
                            text: $filter('localizeString')(kpiListArray.series[0].unit),
                            style: {
                                color: kpiListArray.textColor
                            }
                        },
                        minorTickLength: 0,
                        tickLength: 0
                    };

                    if (kpiListArray.selectedChartType.id == 1 || kpiListArray.selectedChartType.id == 2 || kpiListArray.selectedChartType.id == 9) {
                        if (kpiListArray.selectedChartType.id == 1 || kpiListArray.selectedChartType.id == 2) {
                            if (kpiListArray.selectedChartType.id == 2) {
                                kpiListArray.chartConfig.chart.options3d = {
                                    enabled: true,
                                    alpha: 5,
                                    beta: 2,
                                    depth: 50,
                                    viewDistance: 25
                                }
                                kpiListArray.chartConfig.plotOptions = {
                                    column: {
                                        dataLabels: {
                                            crop: false,
                                            overflow: 'none',
                                            padding: 10.6,
                                            enabled: true,
                                            style: {
                                                color: kpiListArray.textColor
                                            }
                                        },
                                        cursor: 'pointer',
                                        depth: 35
                                    }
                                }
                            }

                            kpiListArray.chartConfig.xAxis = {
                                categories: [],
                                tickInterval: 1,
                                tickLength: 20,
                                title: {
                                    enabled: true,
                                    text: $filter('localizeString')(kpiListArray.labels[0].unit),
                                    style: {
                                        color: kpiListArray.textColor
                                    }
                                },
                                labels: {
                                    style: {
                                        width: barWidth + 'px',
                                        whiteSpace: 'normal',//set to normal   
                                        color: kpiListArray.textColor
                                    },
                                    enabled: true,
                                    useHTML: true,
                                    formatter: function () {
                                        return this.value;
                                    }
                                },
                                min: 0

                            };
                        }
                        else {
                            kpiListArray.chartConfig.yAxis = {
                                categories: [],
                                tickInterval: 1,
                                tickLength: 20,
                                title: {
                                    enabled: true,
                                    text: $filter('localizeString')(kpiListArray.labels[0].unit),
                                    style: {
                                        fontWeight: 'normal',
                                        color: kpiListArray.textColor
                                    }
                                },
                                labels: {
                                    style: {
                                        width: '20px',
                                        whiteSpace: 'normal',
                                        color: kpiListArray.textColor//set to normal
                                    },
                                    enabled: true,
                                    useHTML: true,
                                    formatter: function () {
                                        return this.value;
                                    }
                                },
                                min: 0

                            };
                        }
                        var targetGraphSeries = {
                            name: 'Target',
                            data: []
                        };
                        var categoriesLabels = [];

                        if (kpiListArray.graph.length > 0) {
                            var seriesLength = 0;
                            var graph = {
                                data: [],
                                showInLegend: false,
                                name: '.'
                            };
                            for (var y = 0; y < kpiListArray.graph.length; y++) {
                                var series = kpiListArray.series.filter(function (item) {
                                    if (item.nameEn == kpiListArray.graph[y].name) {
                                        return item;
                                    }
                                })[0];
                                if (kpiListArray.graph[y] != undefined && kpiListArray.graph[y] != null) {
                                    if ((kpiListArray.selectedSeries == undefined && kpiListArray.selectedSeries == null) ||
                                        (kpiListArray.selectedSeries != undefined && kpiListArray.selectedSeries.nameEn == kpiListArray.graph[y].name)) {
                                        var data = angular.copy(kpiListArray.graph[y].data);
                                        if (data.length > 0) {
                                            for (var z = 0; z < data.length; z++) {
                                                var label = kpiListArray.labels.filter(function (item) {
                                                    if (item.nameEn == data[z][0]) {
                                                        return item;
                                                    }
                                                })[0];
                                                if ((kpiListArray.selectedLabel == undefined &&
                                                    kpiListArray.selectedLabel == null) ||
                                                    (
                                                        $filter('localizeString')(kpiListArray.selectedLabel) ==
                                                        $filter('localizeString')(label))) {
                                                    data[z][1] = parseFloat(data[z][1]);
                                                    //var dataItem = { "name": data[z][0], "y": data[z][1], "drilldown": kpiListArray.drillDown != null ? data[z][0] : null };
                                                    graph.data.push(data[z][1]);
                                                    var category = '';
                                                    if (kpiListArray.selectedLabel == undefined) {
                                                        category = category + $filter('localizeString')(label);
                                                    }
                                                    if (kpiListArray.selectedSeries == undefined) {
                                                        category = category + $filter('localizeString')(series);
                                                    }
                                                    category = category.substring(0, category.length > 50 ? 50 : category.length);
                                                    //if (categoriesLabels.indexOf(category) == -1) {
                                                    categoriesLabels.push(category);
                                                    //}
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (graph.data.length > 0) {
                                seriesLength++;
                                kpiListArray.chartConfig.series.push(graph);
                                if (graph.data.length < 6) {
                                    delete kpiListArray.chartConfig.scrollbar;
                                }
                            }
                        }
                        if (kpiListArray.drillDown != null && kpiListArray.drillDown.graph.length > 0) {
                            kpiListArray.chartConfig.drilldown = {
                                activeDataLabelStyle: { color: 'red' }, series: []
                            };
                            kpiListArray.chartConfig.xAxis.type = 'category';
                            for (var y = 0; y < kpiListArray.drillDown.graph.length; y++) {
                                var drilldowngraph = {
                                    data: [],
                                    showInLegend: false,
                                    name: kpiListArray.drillDown.graph[y].name,
                                    id: kpiListArray.drillDown.graph[y].id,
                                };
                                var data = angular.copy(kpiListArray.drillDown.graph[y].data);
                                if (data.length > 0) {
                                    for (var z = 0; z < data.length; z++) {
                                        data[z][1] = parseFloat(data[z][1]);
                                        var dataItem = { name: data[z][0], "y": data[z][1], "drilldown": data[z][2] != null ? data[z][2] : null };
                                        drilldowngraph.data.push(dataItem);
                                    }
                                }
                                if (drilldowngraph.data.length > 0) {
                                    kpiListArray.chartConfig.drilldown.series.push(drilldowngraph);
                                }
                            }
                        }

                        if (categoriesLabels.length > 0) {
                            if (kpiListArray.selectedChartType.id == 9) {
                                maxCount = 5;
                            }
                            if (seriesLength > 1) {
                                switch (seriesLength) {
                                    case 2:
                                        maxCount = 2;
                                        if (kpiListArray.selectedChartType.id == 9) {
                                            maxCount = 1;
                                        }
                                        break;

                                    case 3:
                                        maxCount = 2;
                                        if (kpiListArray.selectedChartType.id == 9) {
                                            maxCount = 0;
                                        }
                                        break;

                                    case 4:
                                        maxCount = 1;
                                        if (kpiListArray.selectedChartType.id == 9) {
                                            maxCount = 0;
                                        }
                                        break;

                                    default:
                                        maxCount = 0;
                                        if (kpiListArray.selectedChartType.id == 9) {
                                            maxCount = 0;
                                        }
                                        break;
                                }
                            }
                            delete kpiListArray.chartConfig.xAxis.categories;
                            if (kpiListArray.chartConfig.drilldown == null) {
                                kpiListArray.chartConfig.xAxis.categories = categoriesLabels;
                                kpiListArray.chartConfig.xAxis.max = (categoriesLabels.length < maxCount) ? (categoriesLabels.length - 1) : maxCount;
                            }
                        }
                    }
                    if (kpiListArray.selectedChartType.id == 3 || kpiListArray.selectedChartType.id == 8) {
                        kpiListArray.chartConfig.xAxis = {
                            categories: [],
                            tickInterval: 1,
                            tickLength: 20,
                            title: {
                                useHTML: true,
                                enabled: true,
                                text: $filter('localizeString')(kpiListArray.labels[0].unit),
                                style: {
                                    fontWeight: 'normal',
                                    color: kpiListArray.textColor
                                }
                            },
                            style: {
                                width: barWidth + 'px',
                                whiteSpace: 'normal'//set to normal
                            },
                            labels: {
                                useHTML: true,
                                enabled: true
                            },

                        };
                        var targetGraphSeries = {
                            name: 'Target',
                            data: []
                        };
                        var categoriesLabels = [];
                        var graph = {
                            data: [],
                            name: '.', showInLegend: false
                        };
                        for (var y = 0; y < kpiListArray.graph.length; y++) {
                            var series = kpiListArray.series.filter(function (item) { if (item.nameEn == kpiListArray.graph[y].name) { return item; } })[0];

                            var rangeGraph = {
                                name: 'Min. & Max. Scores',
                                type: 'arearange',
                                data: [],
                                color: kpiListArray.lightColor,
                                fillOpacity: 0.3,
                                zIndex: 0,
                                marker: {
                                    enabled: false
                                }
                            }
                            if (kpiListArray.graph[y] != undefined && kpiListArray.graph[y] != null) {
                                if ((kpiListArray.selectedSeries == undefined && kpiListArray.selectedSeries == null) || (
                                    kpiListArray.selectedSeries != undefined && kpiListArray.selectedSeries.nameEn == kpiListArray.graph[y].name)) {
                                    var data = angular.copy(kpiListArray.graph[y].data);
                                    if (data.length > 0) {
                                        for (var z = 0; z < data.length; z++) {
                                            var label = kpiListArray.labels.filter(function (item) { if (item.nameEn == data[z][0]) { return item; } })[0];
                                            if ((kpiListArray.selectedLabel == undefined && kpiListArray.selectedLabel == null) || (
                                                $filter('localizeString')(kpiListArray.selectedLabel) == $filter('localizeString')(label))) {
                                                data[z][1] = parseFloat(data[z][1]);
                                                var dataItem = data[z][1];
                                                graph.data.push(dataItem);
                                                var category = '';
                                                if (kpiListArray.selectedLabel == undefined) {
                                                    category = category + $filter('localizeString')(label);
                                                }
                                                if (kpiListArray.selectedSeries == undefined) {
                                                    category = category + $filter('localizeString')(series);
                                                }
                                                category = category.substring(0, category.length > 50 ? 50 : category.length);
                                                //if (categoriesLabels.indexOf(category) == -1) {
                                                categoriesLabels.push(category);
                                                //}
                                            }
                                        }
                                    }

                                }
                            }
                        }
                        if (graph.data.length > 0) {
                            kpiListArray.chartConfig.series.push(graph);
                            if (graph.data.length < 6) {
                                delete kpiListArray.chartConfig.scrollbar;
                            }
                        }
                        if (categoriesLabels.length > 0) {
                            kpiListArray.chartConfig.xAxis.categories = categoriesLabels;
                            kpiListArray.chartConfig.xAxis.max = (categoriesLabels.length < maxCount) ? (categoriesLabels.length - 1) : maxCount;
                        }
                    }
                    if (kpiListArray.selectedChartType.id == 5 || kpiListArray.selectedChartType.id == 6) {
                        if (kpiListArray.selectedChartType.id == 6) {
                            kpiListArray.chartConfig.chart.options3d = {
                                enabled: true,
                                alpha: 45,
                                beta: 0
                            }
                            kpiListArray.chartConfig.plotOptions.pie.depth = 35;
                        }
                        var graph = {
                            name: '.',
                            data: []
                        };
                        graph.name = $filter('localizeString')(kpiListArray.selectedSeries);
                        kpiListArray.chartConfig.title.text = kpiListArray.chartConfig.title.text + ' - ' + $filter('localizeString')(kpiListArray.selectedSeries);
                        var categoriesLabels = [];
                        for (var y = 0; y < kpiListArray.graph.length; y++) {
                            var series = kpiListArray.series.filter(function (item) { if (item.nameEn == kpiListArray.graph[y].name) { return item; } })[0];

                            if (kpiListArray.graph[y] != undefined && kpiListArray.graph[y] != null) {
                                if ((kpiListArray.selectedSeries == undefined && kpiListArray.selectedSeries == null) || (
                                    kpiListArray.selectedSeries != undefined && kpiListArray.selectedSeries.nameEn == kpiListArray.graph[y].name)) {
                                    var data = kpiListArray.graph[y].data;
                                    if (data.length > 0) {
                                        for (var z = 0; z < data.length; z++) {
                                            var label = kpiListArray.labels.filter(function (item) { if (item.nameEn == data[z][0]) { return item; } })[0];

                                            if ((kpiListArray.selectedLabel == undefined && kpiListArray.selectedLabel == null) || (
                                                $filter('localizeString')(kpiListArray.selectedLabel) == $filter('localizeString')(label))) {
                                                var dataItem = {};
                                                dataItem.name = $filter('localizeString')(series);
                                                if (kpiListArray.selectedLabel == null) {
                                                    dataItem.name = ($filter('localizeString')(label));
                                                }
                                                dataItem.y = parseFloat(data[z][1]);

                                                graph.data.push(dataItem);
                                                categoriesLabels.push(data[z][0]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        kpiListArray.chartConfig.series.push(graph);
                    }
                    if (kpiListArray.selectedChartType.id == 7) {
                        //delete kpiListArray.selectedSeries;
                        //delete kpiListArray.selectedLabel;
                        kpiListArray.chartConfig.colorAxis = {
                            min: 0,
                            minColor: kpiListArray.lightColor,
                            maxColor: kpiListArray.defaultColor
                        };

                        kpiListArray.chartConfig.legend = {
                            align: 'right',
                            layout: 'vertical',
                            margin: 0,
                            verticalAlign: 'top',
                            y: 25,
                            style: {
                                color: kpiListArray.textColor
                            }
                        };
                        kpiListArray.chartConfig.xAxis.title = {
                            enabled: true,
                            text: $filter('localizeString')(kpiListArray.labels[0].unit),
                            style: {
                                fontWeight: 'normal',
                                color: kpiListArray.textColor
                            }
                        };

                        kpiListArray.chartConfig.tooltip = {
                            formatter: function () {
                                if (this.point.value != null)
                                    return '<b>' +
                                        this.point.value + '</b> applications in ' + '<b>' + this.series.xAxis.categories[this.point.x] + '</b> For <br>' +

                                        '  <b>' + this.series.yAxis.categories[this.point.y] + '</b>';
                                else
                                    return '';
                            }
                        };
                        var xAxisCategoriesLabels = [];
                        var yAxisCategoriesLabels = [];
                        var graph = {
                            data: [],
                            showInLegend: false,
                            name: $filter('localizeString')(series)
                        };
                        if (kpiListArray.graph) {
                            var seriesLength = 0;
                            for (var y = 0; y < kpiListArray.graph.length; y++) {
                                var series = kpiListArray.series.filter(function (item) {
                                    if (item.nameEn == kpiListArray.graph[y].name) {
                                        return item;
                                    }
                                })[0];

                                if (yAxisCategoriesLabels.indexOf(kpiListArray.graph[y].name) == -1) {
                                    yAxisCategoriesLabels.push(kpiListArray.graph[y].name);
                                }
                                if (kpiListArray.graph[y] != undefined && kpiListArray.graph[y] != null) {
                                    if ((kpiListArray.selectedSeries == undefined && kpiListArray.selectedSeries == null) ||
                                        (kpiListArray.selectedSeries != undefined && kpiListArray.selectedSeries.nameEn == kpiListArray.graph[y].name)) {
                                        var data = angular.copy(kpiListArray.graph[y].data);
                                        if (data.length > 0) {
                                            for (var z = 0; z < data.length; z++) {
                                                var label = kpiListArray.labels.filter(function (item) {
                                                    if (item.nameEn == data[z][0]) {
                                                        return item;
                                                    }
                                                })[0];
                                                if ((kpiListArray.selectedLabel == undefined && kpiListArray.selectedLabel == null) ||
                                                    ($filter('localizeString')(kpiListArray.selectedLabel) == $filter('localizeString')(label))) {
                                                    if (xAxisCategoriesLabels.indexOf(data[z][0]) == -1) {
                                                        xAxisCategoriesLabels.push(data[z][0]);
                                                    }
                                                    data[z][1] = parseFloat(data[z][1]);
                                                    // if (data[z][1] != 0) //Use This Condition To Hide zero Values
                                                    {
                                                        graph.data.push([xAxisCategoriesLabels.indexOf(data[z][0]), yAxisCategoriesLabels.indexOf(kpiListArray.graph[y].name), data[z][1]]);
                                                    }
                                                    //else
                                                    //    graph.data.push([xAxisCategoriesLabels.indexOf(data[z][0]), yAxisCategoriesLabels.indexOf(kpiListArray.graph[y].name), null]);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (xAxisCategoriesLabels.length > 0) {
                            kpiListArray.chartConfig.series.push(graph);
                            kpiListArray.chartConfig.xAxis.categories = xAxisCategoriesLabels;
                            delete kpiListArray.chartConfig.yAxis;
                            kpiListArray.chartConfig.yAxis = {
                                categories: yAxisCategoriesLabels, title: '',
                                style: {
                                    color: kpiListArray.textColor
                                }
                            };
                            //kpiListArray.chartConfig.xAxis.max = (xAxisCategoriesLabels.length < maxCount) ? (xAxisCategoriesLabels.length - 1) : maxCount;

                        }
                    }
                    if (kpiListArray.selectedChartType.id == 8) {
                        delete kpiListArray.chartConfig.plotOptions;
                        delete kpiListArray.chartConfig.scrollbar;
                        kpiListArray.chartConfig.chart = {
                            polar: true,
                            type: 'line'
                        };
                        kpiListArray.chartConfig.pane = {
                            startAngle: 0,
                            endAngle: 360
                        }
                        kpiListArray.chartConfig.xAxis = {
                            categories: [],
                            tickInterval: 45,
                            min: 0,
                            max: 360,
                            labels: {
                                formatter: function () {
                                    return this.value + '°';
                                },
                                style: {
                                    fontWeight: 'normal',
                                    color: kpiListArray.textColor
                                }
                            }
                        };

                        kpiListArray.chartConfig.yAxis = {
                            min: 0,
                            style: {
                                color: kpiListArray.textColor
                            }
                        };

                        kpiListArray.chartConfig.legend = {
                            align: 'right',
                            verticalAlign: 'top',
                            y: 70,
                            layout: 'vertical',
                            style: {
                                color: kpiListArray.textColor
                            }
                        };
                        var categoriesLabels = [];
                        for (var y = 0; y < kpiListArray.graph.length; y++) {
                            var series = kpiListArray.series.filter(function (item) { if (item.nameEn == kpiListArray.graph[y].name) { return item; } })[0];
                            var graph = {
                                data: [],
                                type: 'line',
                                name: 'Line',
                                showInLegend: false,
                                pointPlacement: 'on',
                                name: $filter('localizeString')(series)
                            };
                            if (kpiListArray.graph[y] != undefined && kpiListArray.graph[y] != null) {
                                if ((kpiListArray.selectedSeries == undefined && kpiListArray.selectedSeries == null) || (
                                    kpiListArray.selectedSeries != undefined && kpiListArray.selectedSeries.nameEn == kpiListArray.graph[y].name)) {
                                    var data = angular.copy(kpiListArray.graph[y].data);
                                    if (data.length > 0) {
                                        for (var z = 0; z < data.length; z++) {
                                            var label = kpiListArray.labels.filter(function (item) { if (item.nameEn == data[z][0]) { return item; } })[0];
                                            if ((kpiListArray.selectedLabel == undefined && kpiListArray.selectedLabel == null) || (
                                                $filter('localizeString')(kpiListArray.selectedLabel) == $filter('localizeString')(label))) {
                                                data[z][1] = parseFloat(data[z][1]);
                                                var dataItem = data[z][1];
                                                graph.data.push(dataItem);
                                                if (categoriesLabels.indexOf(data[z][0]) == -1) {
                                                    categoriesLabels.push(data[z][0]);
                                                }
                                            }
                                        }
                                    }

                                    if (graph.data.length > 0) {
                                        kpiListArray.chartConfig.series.push(graph);
                                        if (graph.data.length < 6) {
                                            delete kpiListArray.chartConfig.scrollbar;
                                        }
                                    }
                                }
                            }
                        }
                        if (categoriesLabels.length > 0) {
                            kpiListArray.chartConfig.xAxis.categories = categoriesLabels;
                        }
                    }

                    if (kpiListArray.selectedChartType.id == 4) {          //Maps    
                        delete kpiListArray.chartConfig.scrollbar;
                        if (screenWidth < 700) {
                            kpiListArray.chartConfig.mapNavigation = {
                                enabled: true,
                                buttonOptions: {
                                    verticalAlign: 'bottom'
                                },
                                //enableDoubleClickZoomTo: true
                            };
                        }
                        kpiListArray.chartConfig.colorAxis = {
                            min: 0,
                            maxColor: kpiListArray.defaultColor
                        };
                        kpiListArray.chartConfig.tooltip = {
                            formatter: function () {
                                if (this.point.value != null)
                                    return this.key + ' <b>' + this.point.value + '</b> <br> UAE : <b>67</b>';
                                else
                                    return '';
                            }
                        };
                        kpiListArray.chartConfig.animation = true;
                        kpiListArray.chartConfig.plotOptions = {
                            map: {
                                mapData: Highcharts.maps['countries/ae/ae-all'],
                                //joinBy: ['name']
                            }
                        };
                        delete kpiListArray.chartConfig.xAxis;
                        delete kpiListArray.chartConfig.yAxis;
                        kpiListArray.chartConfig.xAxis = {
                            lineWidth: 0,
                            minorGridLineWidth: 0,
                            lineColor: 'transparent',
                            labels: {
                                enabled: false
                            },
                            title: {
                                enabled: true,
                                //text: $filter('localizeString')(kpiListArray.criteria.filter(function (item) { if (item.axisType == 'Y') { return item; } })[0]),
                                style: {
                                    fontWeight: 'normal'
                                }
                            },
                            useHTML: true,
                        }

                        kpiListArray.chartConfig.yAxis = {
                            lineWidth: 0,
                            minorGridLineWidth: 0,
                            labels: {
                                enabled: false
                            },
                            minorTickLength: 0,
                            tickLength: 0
                        };
                        var categoriesLabels = [];
                        if (kpiListArray.selectedSeries == undefined || kpiListArray.selectedSeries == null) {
                            kpiListArray.selectedSeries = kpiListArray.series[0];
                        }
                        var graph = {
                            data: [],
                            showInLegend: false,
                            states: {
                                hover: {
                                    color: '#BADA55'
                                }
                            },
                            dataLabels: {
                                enabled: false,
                            },
                            name: 'Number Of Projects'
                        };
                        for (var y = 0; y < kpiListArray.graph.length; y++) {
                            var series = kpiListArray.series.filter(function (item) {
                                if (item.nameEn == kpiListArray.graph[y].name) {
                                    return item;
                                }
                            })[0];
                            if (kpiListArray.graph[y] != undefined && kpiListArray.graph[y] != null) {
                                if ((kpiListArray.selectedSeries == undefined && kpiListArray.selectedSeries == null) ||
                                    (kpiListArray.selectedSeries != undefined && kpiListArray.selectedSeries.nameEn == kpiListArray.graph[y].name)) {
                                    var data = angular.copy(kpiListArray.graph[y].data);
                                    if (data.length > 0) {
                                        for (var z = 0; z < data.length; z++) {
                                            var label = kpiListArray.labels.filter(function (item) {
                                                if (item.nameEn == data[z][0]) {
                                                    return item;
                                                }
                                            })[0];
                                            if ((kpiListArray.selectedLabel == undefined &&
                                                kpiListArray.selectedLabel == null) ||
                                                (
                                                    $filter('localizeString')(kpiListArray.selectedLabel) ==
                                                    $filter('localizeString')(label))) {
                                                data[z][1] = parseFloat(data[z][1]);
                                                //var dataItem = { "name": data[z][0], "y": data[z][1], "drilldown": kpiListArray.drillDown != null ? data[z][0] : null };
                                                graph.data.push([
                                                    data[z][0],
                                                    data[z][1]
                                                ]);
                                                var category = '';
                                                if (kpiListArray.selectedLabel == undefined) {
                                                    category = category + $filter('localizeString')(label);
                                                }
                                                if (kpiListArray.selectedSeries == undefined) {
                                                    category = category + $filter('localizeString')(series);
                                                }
                                                category = category.substring(0, category.length > 50 ? 50 : category.length);
                                                if (categoriesLabels.indexOf(category) == -1) {
                                                    categoriesLabels.push(category);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (graph.data.length > 0) {
                            seriesLength++;
                            kpiListArray.chartConfig.series.push(graph);

                        }
                    }


                    if (kpiListArray.selectedChartType.id == 10 || kpiListArray.selectedChartType.id == 11) {

                        kpiListArray.chartConfig.legend = {
                            layout: 'vertical',
                            align: 'right',
                            verticalAlign: 'middle'
                        };
                        kpiListArray.chartConfig.plotOptions.column = {
                            stacking: 'normal',
                            dataLabels: {
                                crop: false,
                                overflow: 'none',
                                padding: 10.6,
                                enabled: false,
                                style: {
                                    color: kpiListArray.textColor
                                }
                            },
                            cursor: 'pointer'
                        };
                        kpiListArray.chartConfig.yAxis = {
                            title: {
                                enabled: true,
                                text: $filter('localizeString')(kpiListArray.labels[0].unit),
                                style: {
                                    fontWeight: 'normal',
                                    color: kpiListArray.textColor
                                }
                            },
                            labels: {
                                style: {
                                    width: '20px',
                                    whiteSpace: 'normal',
                                    color: kpiListArray.textColor//set to normal
                                },
                                enabled: true,
                                useHTML: true,
                                formatter: function () {
                                    return this.value;
                                }
                            },
                            min: 0,
                            stackLabels: {
                                enabled: true,
                                style: {
                                    fontWeight: 'bold',
                                    color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                                }
                            }
                        }
                        var targetGraphSeries = {
                            name: 'Target',
                            data: []
                        };
                        var categoriesLabels = [];

                        if (kpiListArray.graph.length > 0) {
                            for (var y = 0; y < kpiListArray.graph.length; y++) {
                                var series = kpiListArray.series.filter(function (item) {
                                    if (item.nameEn == kpiListArray.graph[y].name) {
                                        return item;
                                    }
                                })[0];
                                var seriesLength = 0;
                                var graph = {
                                    data: [],
                                    showInLegend: true,
                                    name: $filter('localizeString')(series)
                                };
                                if ([2012, 2013, 2014, 2015, 2016, 2010, 2011].indexOf(series.nameEn) != -1) {
                                    graph.visible = false;
                                }
                                if (kpiListArray.graph[y] != undefined && kpiListArray.graph[y] != null) {
                                    if ((kpiListArray.selectedSeries == undefined && kpiListArray.selectedSeries == null) ||
                                        (kpiListArray.selectedSeries != undefined && kpiListArray.selectedSeries.nameEn == kpiListArray.graph[y].name)) {
                                        var data = angular.copy(kpiListArray.graph[y].data);
                                        if (data.length > 0) {
                                            for (var z = 0; z < data.length; z++) {
                                                var label = kpiListArray.labels.filter(function (item) {
                                                    if (item.nameEn == data[z][0]) {
                                                        return item;
                                                    }
                                                })[0];
                                                if ((kpiListArray.selectedLabel == undefined &&
                                                    kpiListArray.selectedLabel == null) ||
                                                    (
                                                        $filter('localizeString')(kpiListArray.selectedLabel) ==
                                                        $filter('localizeString')(label))) {
                                                    data[z][1] = parseFloat(data[z][1]);
                                                    //var dataItem = { "name": data[z][0], "y": data[z][1], "drilldown": kpiListArray.drillDown != null ? data[z][0] : null };
                                                    // if (data[z][1] != 0)//Use This Condition To Hide zero Values
                                                    {
                                                        graph.data.push(data[z][1]);
                                                    }
                                                    //else
                                                    //    graph.data.push(null);
                                                    var category = '';
                                                    if (kpiListArray.selectedLabel == undefined) {
                                                        category = category + $filter('localizeString')(label);
                                                    }

                                                    category = category.substring(0, category.length > 50 ? 50 : category.length);
                                                    if (categoriesLabels.indexOf(category) == -1) {
                                                        categoriesLabels.push(category);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (graph.data.length > 0) {
                                    seriesLength++;
                                    kpiListArray.chartConfig.series.push(graph);
                                    if (graph.data.length < 6) {
                                        delete kpiListArray.chartConfig.scrollbar;
                                    }
                                }
                            }

                        }
                        if (categoriesLabels.length > 0) {
                            delete kpiListArray.chartConfig.xAxis.categories;
                            if (kpiListArray.chartConfig.drilldown == null) {
                                kpiListArray.chartConfig.xAxis = {
                                    categories: categoriesLabels,
                                    title: {
                                        enabled: true,
                                        text: $filter('localizeString')(kpiListArray.labels[0].unit),
                                        style: {
                                            color: kpiListArray.textColor
                                        }
                                    },
                                    labels: {
                                        style: {
                                            width: barWidth + 'px',
                                            whiteSpace: 'normal',//set to normal   
                                            color: kpiListArray.textColor
                                        },
                                        enabled: true,
                                        useHTML: true,
                                        formatter: function () {
                                            return this.value;
                                        }
                                    },
                                    min: 0
                                };
                                if (categoriesLabels.length < 12) {
                                    delete kpiListArray.chartConfig.scrollbar;
                                }
                                kpiListArray.chartConfig.xAxis.max = (categoriesLabels.length < 12) ? (categoriesLabels.length - 1) : 12;
                            }
                        }
                    }
                }
                scope.rerenderChart(scope.ChartData, true);

                scope.open = function (emirateId) {
                    var modalInstance = $uibModal.open({
                        templateUrl: 'app/views/Admin/UAEProjectMatrix/ProjectsPop/ProjectsPop.html',
                        controller: 'ProjectsPopController',
                        size: 'lg',
                        resolve: {
                            wsisId: function () {
                                return null;
                            }, sdgId: function () {
                                return null;
                            }, emirateId: function () {
                                return emirateId;
                            },
                            emirateId: function () {
                                return null;
                            }
                        }
                    });
                    modalInstance.result.then(function (award) {
                    }, function () {
                        //state.text('Modal dismissed with Cancel status');
                    });
                };

            }
        }


    }


})();
