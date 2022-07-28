/**=========================================================
 * Module: VectorMapController.js
 * jVector Maps support
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('VectorMapController', VectorMapController);

    /* @ngInject */
    function VectorMapController($rootScope, $scope, $timeout, $http, colors) {
        var vm = this;

        // SERIES & MARKERS FOR WORLD MAP
        // ----------------------------------- 

        vm.seriesData = {
            'AU': 15710,    // Australia
            'RU': 17312,    // Russia
            'CN': 123370,    // China
            'US': 12337,     // USA
            'AR': 18613,    // Argentina
            'CO': 12170,   // Colombia
            'DE': 1358,    // Germany
            'FR': 1479,    // France
            'GB': 16311,    // Great Britain
            'IN': 19814,    // India
            'SA': 12137      // Saudi Arabia
        };

        vm.cities = [];
        vm.markers = [];
        if ($scope.$parent.$parent.eventDetails.event) {
            for (var i = 0; i < $scope.$parent.$parent.eventDetails.event.eventAddresses.length; i++) {
                vm.cities.push($scope.$parent.$parent.eventDetails.event.eventAddresses[i].address.city);

                $http.get('https://maps.googleapis.com/maps/api/geocode/json?key=AIzaSyCkvXG2TDIKHJTAjnPC66SGS6P6-ItDc0k&address=' + vm.cities[i])
                .then(function (resp) {
    
                    console.log(resp.data);
                    var eventLocation = {
                        latLng: [parseFloat(resp.data.results[0].geometry.location.lat.toFixed(2)), parseFloat(resp.data.results[0].geometry.location.lng.toFixed(2))],
                        name: resp.data.results[0].formatted_address
                    };
                    vm.markers.push(eventLocation);
                },
                function (response) { });
            }
        }

        vm.markersData = [
          { latLng: [25.20, 55.27], name: 'Dubai - United Arab Emirates' },
          { latLng: [23.46, 53.73], name: 'Abu Dhabi - United Arab Emirates' },
          //{ latLng: [41.90, 12.45], name: 'Vatican City' },
          //{ latLng: [43.73, 7.41], name: 'Monaco' },
          //{ latLng: [-0.52, 166.93], name: 'Nauru' },
          //{ latLng: [-8.51, 179.21], name: 'Tuvalu' },
          //{ latLng: [7.11, 171.06], name: 'Marshall Islands' },
          //{ latLng: [17.3, -62.73], name: 'Saint Kitts and Nevis' },
          //{ latLng: [3.2, 73.22], name: 'Maldives' },
          //{ latLng: [35.88, 14.5], name: 'Malta' },
          //{ latLng: [41.0, -71.06], name: 'New England' },
          //{ latLng: [12.05, -61.75], name: 'Grenada' },
          //{ latLng: [13.16, -59.55], name: 'Barbados' },
          //{ latLng: [17.11, -61.85], name: 'Antigua and Barbuda' },
          //{ latLng: [-4.61, 55.45], name: 'Seychelles' },
          //{ latLng: [7.35, 134.46], name: 'Palau' },
          //{ latLng: [42.5, 1.51], name: 'Andorra' }
        ];

        vm.mapOptions = {
            height: 500,
            map: 'world_mill_en',
            backgroundColor: 'transparent',
            zoomMin: 0,
            zoomMax: 8,
            zoomOnScroll: false,
            regionStyle: {
                initial: {
                    'fill': colors.byName('gray-dark'),
                    'fill-opacity': 1,
                    'stroke': 'none',
                    'stroke-width': 1.5,
                    'stroke-opacity': 1
                },
                hover: {
                    'fill-opacity': 0.8
                },
                selected: {
                    fill: 'blue'
                },
                selectedHover: {
                }
            },
            focusOn: { x: 0.4, y: 0.6, scale: 1 },
            markerStyle: {
                initial: {
                    fill: colors.byName('warning'),
                    stroke: colors.byName('warning')
                }
            },
            onRegionLabelShow: function (e, el, code) {
                if (vm.seriesData && vm.seriesData[code])
                    el.html(el.html() + ': ' + vm.seriesData[code] + ' visitors');
            },
            markers: vm.markersData,
            series: {
                regions: [{
                    values: vm.seriesData,
                    scale: [colors.byName('gray-darker')],
                    normalizeFunction: 'polynomial'
                }]
            },
        };

    }

    VectorMapController.$inject = ['$rootScope', '$scope', '$timeout', '$http', 'colors'];
})();
