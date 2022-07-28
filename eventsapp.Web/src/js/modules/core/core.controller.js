/**=========================================================
 * Module: CoreController.js
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('CoreController', CoreController);

    /* @ngInject */
    function CoreController($rootScope, $filter, $http, UserProfile) {
        var vm = this;
        $(document).ready(function () {
            setTimeout(function () {
                $("#wrapper").toggleClass("enlarged");
            }, 2000);
        });


        vm.user = UserProfile.getProfile();
        $rootScope.userPreferredLanguages = [];
        $rootScope.englishPreferredLanguage = false;
        $rootScope.arabicPreferredLanguage = false;
        // Get title for each page
        $rootScope.pageTitle = function () {
            return $filter('localizeString')($rootScope.app, "nameEn", "nameAr") + ' - ' + $filter('localizeString')($rootScope.app.description, "nameEn", "nameAr");
        };
        $rootScope.isLoading = false;
        // Cancel events from templates

        if (vm.user.userId) {
            $http.get($rootScope.app.httpSource + 'api/User/UserPreferredLanguages?userId=' + parseInt(vm.user.userId))
                .then(function (response) {
                    for (var i = 0; i < response.data.length; i++) {
                        $rootScope.userPreferredLanguages.push(response.data[i].language);
                        if (response.data[i].language.nameEn === 'English') {
                            $rootScope.englishPreferredLanguage = true;
                        } else if (response.data[i].language.nameEn === 'Arabic') {
                            $rootScope.arabicPreferredLanguage = true;
                        }
                    }
                }, function (response) { });
        }

        $rootScope.cancel = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
        };
    }

    CoreController.$inject = ['$rootScope', '$filter', '$http', 'UserProfile'];
})();
