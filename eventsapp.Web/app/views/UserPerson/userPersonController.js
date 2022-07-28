(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('userPersonController', userPersonController);
    /* @ngInject */
    function userPersonController($rootScope, $http, $scope, UserProfile, $filter, SweetAlert, $location) {
        $scope.PhotoPath = 'api/Upload/UploadFile?uploadFile=Attachment';
        debugger;
        var vm = this;
        vm.userToken = "";
        vm.userId = "";
        $scope.person = {
            address: {}
        };

        vm.setSpeaker = function () {

            $http.get($rootScope.app.httpSource + 'api/Event/GetEventById?id=' + vm.user.eventId)
                .then(function (resp) {
                    vm.event = resp.data;
                }, function (response) { });

            if (vm.user.userId == undefined)
                vm.userId = vm.user.id;
            else
                vm.userId = vm.user.userId;
            $http.get($rootScope.app.httpSource + 'api/Speaker/GetSpeakerByUser?uid=' + vm.userId)
                .then(function (response) {
                    if (response.data.result == null) {
                        $scope.person = {
                            address: {}
                        };
                    }
                    else {
                        $scope.person = angular.copy(response.data.result);
                    }
                    $scope.person.phone = vm.user.phoneNumber;
                    $scope.person.email = vm.user.username;
                    //$scope.person = response.data.result;
                    console.log($scope.person);
                },
                    function (response) { });


        }
        vm.user = UserProfile.getProfile();
        $scope.person.phone = vm.user.phoneNumber;
        $scope.person.email = vm.user.username;


        $scope.hideSidebar = function () {
            $("#wrapper").toggleClass("enlarged");
            $("#wrapper").addClass("forced");

            if ($("#wrapper").hasClass("enlarged") && $("body").hasClass("fixed-left")) {
                $("body").removeClass("fixed-left").addClass("fixed-left-void");
            } else if (!$("#wrapper").hasClass("enlarged") && $("body").hasClass("fixed-left-void")) {
                $("body").removeClass("fixed-left-void").addClass("fixed-left");
            }

            if ($("#wrapper").hasClass("enlarged")) {
                $(".left ul").removeAttr("style");
            } else {
                $(".subdrop").siblings("ul:first").show();
            }
            $("body").trigger("resize");
        };

        if (vm.user.userId == null) {
            $scope.isView = false;
            vm.userToken = $location.search().uid;
            $http.defaults.withCredentials = true;
            $http.defaults.headers.common['Authorization'] = 'Bearer ' + vm.userToken;
            $scope.hideSidebar();
            $http.get($rootScope.app.httpSource + 'api/User/GetUserbyToken', {
                headers: { 'Authorization': 'Bearer ' + vm.userToken }
            }).then(function (res) {
                if (res.message != undefined) {
                    SweetAlert.swal($filter('translate')('general.ok'), res.message, "Information");
                    $scope.isView = false;
                }
                else {
                    $scope.isView = true;
                    vm.user = res.data;
                    vm.setSpeaker();
                }

            }, (function (err) {
                $scope.isView = false;
                console.log(err);
                SweetAlert.swal($filter('translate')('general.ok'), err.data.message, "Information");
            }));

        }
        else {
            $scope.isView = true;
            vm.setSpeaker();
        }

        $scope.savePerson = function () {
            if ($scope.person.id == undefined) {
                $scope.person.createdBy = vm.user.userId;
                $http.post($rootScope.app.httpSource + 'api/Speaker/CreateSpeaker', $scope.person)
                    .then(function (resp) {
                        SweetAlert.swal($filter('translate')('general.ok'), $filter('translate')('person.personAdded'), "success");
                    }, function (response) { });
            }
            else {
                $http.post($rootScope.app.httpSource + 'api/Speaker/UpdateSpeaker', $scope.person)
                    .then(function (response) {
                        SweetAlert.swal($filter('translate')('general.ok'), $filter('translate')('person.personEdited'), "success");
                    }, function (response) { });
            }
        };



    }

    userPersonController.$inject = ['$rootScope', '$http', '$scope', 'UserProfile', '$filter', 'SweetAlert', '$location'];
})();