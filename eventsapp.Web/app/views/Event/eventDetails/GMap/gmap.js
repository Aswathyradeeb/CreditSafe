/**=========================================================
 * Module: GMapController.js
 * Google Map plugin controller
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('GMapController', GMapController);
    /* @ngInject */
    function GMapController($timeout, $rootScope) {
        var vm = this;
        vm.myMarkers = [];
        $rootScope.lat = "";
        $rootScope.lng = "";
        console.log($rootScope);

        vm.mapOptions = {
            center: new google.maps.LatLng(25.276987, 55.296249),
            zoom: 14,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        vm.addMarker = function ($event, $params) {
            if (vm.myMarkers.length == 0) {

            vm.myMarkers.push(new google.maps.Marker({
                map: vm.myMap,
                position: $params[0].latLng
            }));
            $rootScope.lat = vm.myMarkers[0].getPosition().lat();
            $rootScope.lng = vm.myMarkers[0].getPosition().lng();

            }
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

    }
    GMapController.$inject = ['$timeout', '$rootScope'];
})();
