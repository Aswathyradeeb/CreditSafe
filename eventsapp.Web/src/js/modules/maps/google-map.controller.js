/**=========================================================
 * Module: GoogleMapController.js
 * Google Map plugin controller
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('GoogleMapController', GoogleMapController);
    /* @ngInject */
    function GoogleMapController($rootScope, $scope, $http, $timeout) {
        var vm = this;
        debugger;

        vm.myMarkers = [];


        vm.addMarker = function ($event, $params) {
            vm.myMarkers.push({
                marker: new google.maps.Marker({
                    map: vm.myMap,
                    position: $params[0].latLng
                }),
                address: {}
            });
        };

        vm.setZoomMessage = function (zoom) {
            vm.zoomMessage = 'You just zoomed to ' + zoom + '!';
            console.log(zoom, 'zoomed');
        };

        vm.openMarkerInfo = function (marker) {
            vm.currentMarker = marker;
            vm.currentMarkerLat = marker.getPosition().lat();
            vm.currentMarkerLng = marker.getPosition().lng();
            vm.myInfoWindow.open(vm.myMap, marker);
        };

        vm.setMarkerPosition = function (marker, lat, lng) {
            marker.setPosition(new google.maps.LatLng(lat, lng));
        };

        vm.refreshMap = function () {

            $timeout(function () {
                google.maps.event.trigger(vm.myMap, 'resize');
            }, 100);

        };



        vm.cities = [];
        var event = $scope.$parent.$parent.eventDetails.event;
        for (var i = 0; i < event.eventAddresses.length; i++) {
            vm.cities.push(event.eventAddresses[i].address.street + " " + event.eventAddresses[i].address.city);

            $http.get('https://maps.googleapis.com/maps/api/geocode/json?key=AIzaSyCkoQ8zJQ2tLmPOE9ASqXQstoqhrJ562Nw&address=' + vm.cities[i])
            .then(function (resp) {

                console.log(resp.data);
                var params = [];
                if (vm.mapOptions == undefined) {
                    vm.mapOptions = {
                        center: new google.maps.LatLng(resp.data.results[0].geometry.location.lat, resp.data.results[0].geometry.location.lng),
                        zoom: 15,
                        mapTypeId: google.maps.MapTypeId.ROADMAP
                    };
                }
                params.push({
                    latLng: resp.data.results[0].geometry.location.lat + ',' + resp.data.results[0].geometry.location.lng
                });
                vm.mapOptions = {
                    center: new google.maps.LatLng(resp.data.results[0].geometry.location.lat, resp.data.results[0].geometry.location.lng),
                    zoom: 15,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                };
                vm.myMarkers.push({
                    marker: new google.maps.Marker({
                        map: vm.myMap,
                        position: resp.data.results[0].geometry.location
                    }),
                    address: resp.data.results[0].formatted_address
                });
                vm.refreshMap();
                vm.setMarkerPosition(vm.myMarkers[0].marker, resp.data.results[0].geometry.location.lat, resp.data.results[0].geometry.location.lng);
                //vm.addMarker('self', params);
            },
            function (response) { });
        }

    }
    GoogleMapController.$inject = ['$rootScope', '$scope', '$http', '$timeout'];
})();
