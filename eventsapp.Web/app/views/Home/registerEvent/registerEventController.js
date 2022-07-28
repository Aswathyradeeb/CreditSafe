/**=========================================================
 * Module: DashboardController.js
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('registerEventController', registerEventController);

    registerEventController.$inject = ['$rootScope', '$scope', 'SweetAlert', '$filter', '$http', 'event','$uibModalInstance'];
    function registerEventController($rootScope, $scope, SweetAlert, $filter, $http, event, $uibModalInstance) {
        debugger;
        var vm = this; 
        $scope.model = {
            registrationTypeId: 1
        };
        $http.get($rootScope.app.httpSource + 'api/User/Get')
            .then(function (response) { 
                $scope.user = response.data; 
                $scope.model.userId = response.data.id;
            });

        $http.get($rootScope.app.httpSource + 'api/RegistrationType')
        .then(function (response) {
            $scope.registrationTypes = response.data; 
            $http.get($rootScope.app.httpSource + 'api/Event/GetEventById?id=' + event.id)
                .then(function (resp) {
                    $scope.event = resp.data;
                    $scope.model.eventId = resp.data.id;
                    if ($scope.event.hasSponsors != true || $scope.event.sponsorsOnlineRegister != true) {
                        $scope.registrationTypes = $scope.registrationTypes.filter(function (item) {
                            return item.id != $rootScope.lookup.registrationTypes.Sponsor;
                        });
                    }
                    if ($scope.event.hasExhibitors != true || $scope.event.exhibitorsOnlineRegister != true) {
                        $scope.registrationTypes = $scope.registrationTypes.filter(function (item) {
                            return item.id != $rootScope.lookup.registrationTypes.Exhibitor;
                        });
                    }
                    if ($scope.event.hasPartners != true || $scope.event.partnersOnlineRegister != true) {
                        $scope.registrationTypes = $scope.registrationTypes.filter(function (item) {
                            return item.id != $rootScope.lookup.registrationTypes.Partner;
                        });
                    }
                    if ($scope.event.hasSpeaker != true || $scope.event.speakerOnlineRegister != true) {
                        $scope.registrationTypes = $scope.registrationTypes.filter(function (item) {
                            return item.id != $rootScope.lookup.registrationTypes.Speaker;
                        });
                    } if ($scope.event.hasVIP != true || $scope.event.vipOnlineRegister != true) {
                        $scope.registrationTypes = $scope.registrationTypes.filter(function (item) {
                            return  item.id != $rootScope.lookup.registrationTypes.VIP;
                        });
                    }
                });
        });
         
        $scope.ok = function () {
            if ($scope.model.registrationTypeId == $rootScope.lookup.registrationTypes.Speaker
                || $scope.model.registrationTypeId == $rootScope.lookup.registrationTypes.VIP) {
                if ($scope.user.personId == null || $scope.user.personId == 0) {
                    $scope.showUserLink = true;
                    SweetAlert.swal($filter('translate')('general.notApplicable'), $filter('translate')('event.plsCompleteProfile'), "error");
                    $state.go("app.userProfile");
                    return;
                }
            }
            if ($scope.model.registrationTypeId == $rootScope.lookup.registrationTypes.Exhibitor ||
                $scope.model.registrationTypeId == $rootScope.lookup.registrationTypes.Partner ||
                $scope.model.registrationTypeId == $rootScope.lookup.registrationTypes.Sponsor) {
                if ($scope.user.companyId == null || $scope.user.companyId == 0) {
                    $scope.showCompanyLink = true; 
                    SweetAlert.swal($filter('translate')('general.notApplicable'), $filter('translate')('event.plsCompleteCompanyProfile'), "error");
                    $state.go("app.userCompany");
                    return;
                }
            }
            $http.post($rootScope.app.httpSource + 'api/EventUser/EventUserAdd',$scope.model)
                .then(function (resp) {
                    if (resp.data.message == 'Existing') {
                        SweetAlert.swal($filter('translate')('general.notApplicable'), $filter('translate')('event.alreadyRegisteredEvent'), "error");
                    }
                    else if ($scope.model.registrationTypeId == 1){
                        SweetAlert.swal($filter('translate')('general.ok'), $filter('translate')('event.registerSuccess'), "success");
                    } else  {
                        SweetAlert.swal($filter('translate')('general.ok'), $filter('translate')('event.registerSuccessAdminApproval'), "success");
                    }
                    $uibModalInstance.close($scope.model);
                },
                    function (response) { }); 
        };

        $scope.closeModal = function () {
            $uibModalInstance.dismiss('cancel');
        };
    }
})();
