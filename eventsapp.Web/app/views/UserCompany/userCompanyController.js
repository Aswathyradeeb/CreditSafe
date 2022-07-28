(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('userCompanyController', userCompanyController);
    /* @ngInject */
    function userCompanyController($rootScope, $http, $scope, UserProfile, $filter, SweetAlert, $state, $location) {
        $scope.PhotoPath = 'api/Upload/UploadFile?uploadFile=Attachment';

        var vm = this;
        vm.userToken = "";
        vm.userId = "";
        $scope.company = {
            address: {}
        };
        vm.user = UserProfile.getProfile();
        $scope.company.phone = vm.user.phoneNumber;
        $scope.company.email = vm.user.username;
        vm.showRequiredLanguageError = false;
        vm.setCompanyData = function () {
            $http.get($rootScope.app.httpSource + 'api/Event/GetEventById?id=' + vm.user.eventId)
                .then(function (resp) {
                    vm.event = resp.data;
                }, function (response) { });
            if (vm.user.userId == undefined)
                vm.userId = vm.user.id;
            else
                vm.userId = vm.user.userId;

            $http.get($rootScope.app.httpSource + 'api/Lookup/GetLanguages')
                .then(function (response) {
                    vm.languages = response.data;
                });

            $http.get($rootScope.app.httpSource + 'api/Company/GetCompanyByUser?uid=' + vm.userId)
                .then(function (response) {

                    if (response.data.result == null || response.data.result.id == 0) {
                        $scope.company = {
                            address: {}
                        };
                    }
                    else {
                        $scope.company = angular.copy(response.data.result);

                        $http.get($rootScope.app.httpSource + 'api/Company/GetCompanyPreferredLanguages?CompanyCode=' + $scope.company.companyCode)
                            .then(function (response) {
                                vm.preferredLanguagesList = [];
                                for (var i = 0; i < response.data.length; i++) {
                                    vm.preferredLanguagesList.push(response.data[i].language);
                                }
                            }, function (response) { });
                    }

                    $scope.company.phone = vm.user.phoneNumber;
                    $scope.company.email = vm.user.username;
                    //$scope.company = response.data.result;
                    console.log($scope.company);
                },
                    function (response) { });
        };

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
        }

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
                    vm.setCompanyData();
                }

            }, (function (err) {
                $scope.isView = false;
                console.log(err);
                SweetAlert.swal($filter('translate')('general.ok'), err.data.message, "Information");
            }));

        }
        else {
            $scope.isView = true;
            vm.setCompanyData();
        }

        $scope.saveCompany = function () {
            if (vm.preferredLanguagesList.length) {
                vm.showRequiredLanguageError = false;
                vm.preferredLanguages = [];

                for (var i = 0; i < vm.preferredLanguagesList.length; i++) {
                    var preferredLanguage = {
                        userId: parseInt(vm.user.userId),
                        languageId: vm.preferredLanguagesList[i].id
                    };
                    vm.preferredLanguages.push(preferredLanguage);
                }
            } else {
                vm.showRequiredLanguageError = true;
                return;
            }

            $http.post($rootScope.app.httpSource + 'api/Company/SavePreferredLanguages', vm.preferredLanguages)
                .then(function (resp) {
                    $state.go($state.current, {}, { reload: true }); 
                }, function (response) { });

            if ($scope.company.id == undefined) {
                $scope.company.createdBy = vm.user.userId;
                $http.post($rootScope.app.httpSource + 'api/Company/CreateCompany', $scope.company)
                    .then(function (resp) {
                        SweetAlert.swal($filter('translate')('general.ok'), $filter('translate')('company.companyAdded'), "success");
                        $state.reload();
                    }, function (response) { });
            }
            else {
                $http.post($rootScope.app.httpSource + 'api/Company/UpdateCompany', $scope.company)
                    .then(function (response) {
                        SweetAlert.swal($filter('translate')('general.ok'), $filter('translate')('company.companyEdited'), "success");
                        $state.reload();
                    }, function (response) { });
            }
        };
    }

    userCompanyController.$inject = ['$rootScope', '$http', '$scope', 'UserProfile', '$filter', 'SweetAlert', '$state', '$location'];
})();