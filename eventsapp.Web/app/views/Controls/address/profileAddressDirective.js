(function () {
    'use strict';

    angular
        .module('eventsapp')
        .directive('profileAddress', profileAddress);


    profileAddress.$inject = ['$rootScope', '$http', '$filter'];
    function profileAddress($rootScope, $http, $filter) {
        return {
            restrict: 'E',
            scope: {
                address: "=ngModel",
                viewMode : "="
            },
            templateUrl: '/app/views/Controls/address/profileAddressDirectiveTemplate.html',
            link: link
        };

        function link(scope, element, attrs) {
            scope.fn = {
                onChangeCountry: {}
            };
            var unwatch = scope.$watch('address', function (newVal, oldVal) {
                if (newVal) {
                    init();
                    // remove the watcher
                    unwatch();
                }
            });

            function init() {
                if (scope.address == undefined || scope.address == null) {
                    scope.address = {};
                }
                scope.fn = {
                    onChangeCountry: {}
                };
                scope.locationPhotoPath = 'api/Upload/UploadFile?uploadFile=Attachment';

                $http.get($rootScope.app.httpSource + 'api/Country/GetCountries')
                    .then(function (response) {
                        scope.countries = response.data;

                        if (!scope.address.country) {
                            scope.address.country = scope.countries.filter(function (item) {
                                if (item.nameEn == "United Arab Emirates") {
                                    return item;
                                }
                            })[0];
                        }

                    }, function (response) { });

                scope.fn.onChangeCountry = function () {
                    scope.address.state = {};
                    scope.showMap();
                    scope.address.mobileNumber = '+' + scope.address.country.phoneCode;
                    scope.address.landlineNumber = '+' + scope.address.country.phoneCode;
                };

                scope.showMap = function (lat, lng) {
                    var center = new google.maps.LatLng(lat, lng);
                    scope.isShowMap = true;
                    var mapOptions = {
                        center: center,
                        zoom: 14,
                        mapTypeId: google.maps.MapTypeId.ROADMAP
                    };
                    var infoWindow = new google.maps.InfoWindow();
                    var latlngbounds = new google.maps.LatLngBounds();
                    var mapDiv = document.getElementById("dvMap");
                    var map = new google.maps.Map(mapDiv, mapOptions);

                    google.maps.event.trigger(map, 'resize');
                    var latLng = new google.maps.LatLng(lat, lng);
                    var gmarkers = [];
                    var marker = new google.maps.Marker({
                        position: latLng,
                        map: map
                    });
                    gmarkers.push(marker);

                    google.maps.event.addListenerOnce(map, 'idle', function () {
                        google.maps.event.trigger(map, 'resize');
                    });
                    //map.show();
                    google.maps.event.addListener(map, 'click', function (e) {
                        scope.$apply(function () {

                            var latLng = e.latLng;
                            var marker = new google.maps.Marker({
                                position: latLng,
                                map: map
                            });
                            // Push your newly created marker into the array:
                            removeMarkers();
                            gmarkers.push(marker);
                            scope.address.lat = latLng.lat();
                            scope.address.lng = latLng.lng();
                            google.maps.event.trigger(map, 'resize');
                            map.setZoom(17);
                            map.panTo(marker.position);
                        });
                    });

                    if (scope.address.country != undefined && scope.address.state != undefined) {
                        var geocoder = new google.maps.Geocoder();
                        geocoder.geocode({ 'address': scope.address.country.nameEn + ',' + scope.address.state.nameEn + ',' + scope.address.street }, function (results, status) {
                            if (status == google.maps.GeocoderStatus.OK) {
                                map.setCenter(results[0].geometry.location);
                                map.fitBounds(results[0].geometry.viewport);
                            }
                        });
                    }
                    function removeMarkers() {
                        for (var i = 0; i < gmarkers.length; i++) {
                            gmarkers[i].setMap(null);
                        }
                    }
                };

                if (scope.address.lng != undefined && scope.address.lng != '') {
                    scope.showMap(scope.address.lat, scope.address.lng);
                }
                else {
                    scope.showMap(25.208242, 55.266152);
                }
            }
        }
    }
})();
