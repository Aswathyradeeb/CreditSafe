/**=========================================================
 * Module: SettingsService
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('eventsapp')
        .service('settings', settings);
    /* @ngInject */
    function settings($rootScope, $localStorage, $translate) {
        /*jshint validthis:true*/
        var self = this;
        self.init = init;
        self.loadAndWatch = loadAndWatch;
        self.availableThemes = availableThemes;
        self.setTheme = setTheme;

        /////////////////

        self.themes = [
          { name: 'primary', sidebar: 'bg-white', sidebarHeader: 'bg-primary bg-light', brand: 'bg-primary', topbar: 'bg-primary' },
          { name: 'purple', sidebar: 'bg-white', sidebarHeader: 'bg-purple bg-light', brand: 'bg-purple', topbar: 'bg-purple' },
          { name: 'success', sidebar: 'bg-white', sidebarHeader: 'bg-success bg-light', brand: 'bg-success', topbar: 'bg-success' },
          { name: 'warning', sidebar: 'bg-white', sidebarHeader: 'bg-warning bg-light', brand: 'bg-warning', topbar: 'bg-warning' },
          { name: 'info', sidebar: 'bg-white', sidebarHeader: 'bg-info bg-light', brand: 'bg-info', topbar: 'bg-info' },
          { name: 'danger', sidebar: 'bg-white', sidebarHeader: 'bg-danger bg-light', brand: 'bg-danger', topbar: 'bg-danger' },
          { name: 'pink', sidebar: 'bg-white', sidebarHeader: 'bg-pink bg-light', brand: 'bg-pink', topbar: 'bg-pink' },
          { name: 'amber', sidebar: 'bg-white', sidebarHeader: 'bg-amber bg-light', brand: 'bg-amber', topbar: 'bg-amber' },
        ];

        function init() {
            // Global settings
            $rootScope.app = {
                nameEn: 'E-Voucher System',
                nameAr: "مرحبًا خمسة أحداث",
                description: {
                    nameEn: 'Create, Organize and Manage your Events',
                    nameAr: "إنشاء وتنظيم وإدارة الأحداث الخاصة بك"
                },
                year: new Date().getFullYear(),
                views: {
                    animation: 'ng-fadeInLeft2'
                },
                layout: {
                    isFixed: false,
                    isBoxed: false,
                    isDocked: false,
                    isRTL: false
                },
                sidebar: {
                    isOffscreen: false,
                    isMini: false,
                    isRight: false
                },
                footer: {
                    hidden: false
                },
                themeId: 0,
                // default theme
                theme: {
                    name: 'primary',
                    sidebar: 'bg-white',
                    sidebarHeader: 'bg-primary bg-light',
                    brand: 'bg-primary',
                    topbar: 'bg-primary'
                }, 
                httpSource: 'http://localhost:4445/WebApi/',
                baseURL: 'http://localhost:4445/' 
            };
            $rootScope.lookup = {};
            $rootScope.lookup.registrationTypes = {
                Participant :1,
                VIP: 2,
                Speaker: 3,
                Exhibitor: 4,
                Sponsor: 5,
                Partner:6
            };

            $rootScope.setArabicInit = function () {
                $("[id$='Ar']").arabisk();
            };
            this.loadAndWatch();
        }

        function loadAndWatch() { 
             //Load current settings from local storage
            if (angular.isDefined($localStorage.settings))

               // $localStorage.settings = $rootScope.app;

                $rootScope.app = $localStorage.settings; 
            else
                $localStorage.settings = $rootScope.app;


            $rootScope.$watch('app.layout', function () {
                $localStorage.settings = $rootScope.app;
            }, true);
        }

        function availableThemes() {
            return self.themes;
        }

        function setTheme(idx) {
            $rootScope.app.theme = this.themes[idx];
        }

    }
    settings.$inject = ['$rootScope', '$localStorage', '$translate'];

})();
