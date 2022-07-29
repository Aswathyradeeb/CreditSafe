/**=========================================================
 * Module: CoreController.js
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('CoreController', CoreController);

    /* @ngInject */
    function CoreController($rootScope, $filter, $http) {
        var vm = this;
        $(document).ready(function () {
            setTimeout(function () {
                $("#wrapper").toggleClass("enlarged");
            }, 2000);
        });


        $rootScope.userPreferredLanguages = [];
        $rootScope.englishPreferredLanguage = false;
        $rootScope.arabicPreferredLanguage = false;
        // Get title for each page
        $rootScope.pageTitle = function () {
            return $filter('localizeString')($rootScope.app, "nameEn", "nameAr") + ' - ' + $filter('localizeString')($rootScope.app.description, "nameEn", "nameAr");
        };
        $rootScope.isLoading = false;
        // Cancel events from templates

      
        $rootScope.cancel = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
        };
    }

    CoreController.$inject = ['$rootScope', '$filter', '$http'];
})();
