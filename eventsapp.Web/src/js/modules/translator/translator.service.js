/**=========================================================
 * Module: TranslatorService
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('eventsapp')
        .service('translator', translator);
    /* @ngInject */
    function translator($rootScope, $translate, $http, $cookies, $localStorage, $window, $state, tmhDynamicLocale) {
        /*jshint validthis:true*/
        var self = this;

        self.init = init;
        self.set = set;
        self.data = {
            // Handles language dropdown
            listIsOpen: false,
            // list of available languages
            available: {
                'en': 'English',
                'ar': 'العربية'
            },
            selected: 'English'
        };

        /////////////////////

        // display always the current ui language
        function init() {


            var proposedLanguage = $translate.proposedLanguage() || $translate.use();
            var preferredLanguage = $translate.preferredLanguage(); // we know we have set a preferred one in App.config

            if ($cookies.get('Culture') == undefined) {
                $cookies.put('Culture', (preferredLanguage === 'ar' ? 'ar-AE' : 'en-US'));
            }
            else {
                set($cookies.get('Culture') === 'ar-AE' ? 'ar' : 'en', null, true);
                preferredLanguage = $cookies.get('Culture') === 'ar-AE' ? 'ar' : 'en';
            }

            self.data.selected = self.data.available[(preferredLanguage)];
            // Init internationalization service
            $rootScope.language = self.data;
            $rootScope.language.set = angular.bind(self, self.set);


            $rootScope.$locale = preferredLanguage;
            tmhDynamicLocale.set(preferredLanguage);
            $translate.use(preferredLanguage);
            if ($localStorage.settings != undefined) {
                if (preferredLanguage === 'ar') {
                    $localStorage.settings.layout.isRTL = true;
                    $localStorage.settings.sidebar.isRight = true;
                }
                else {
                    $localStorage.settings.layout.isRTL = false;
                    $localStorage.settings.sidebar.isRight = false;
                }
            }
            return self.data;
        }

        function set(localeId, ev, disableReload) {
            // Set the new idiom
            $cookies.put('Culture', (localeId === 'ar' ? 'ar-AE' : localeId));

            //$http.post($rootScope.app.httpSource + 'api/Culture/SetCulture', { value: (localeId === 'ar' ? 'ar-AE' : localeId) }, { withCredential: true })
            //    .then(function (response) {
            //    }, function (response) { });

            if (disableReload != true) {
                $window.location.reload();
            }
        }

    }
    translator.$inject = ['$rootScope', '$translate', '$http', '$cookies', '$localStorage', '$window', '$state', 'tmhDynamicLocale'];

})();
