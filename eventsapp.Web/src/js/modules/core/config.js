/**=========================================================
 * Module: CoreConfig
 =========================================================*/
(function () {
    'use strict';

    angular
      .module('eventsapp')
      .config(commonConfig)
      .config(lazyLoadConfig);

    // Common object accessibility
    commonConfig.$inject = ['$controllerProvider', '$compileProvider', '$filterProvider', '$provide', '$httpProvider'];
    function commonConfig($controllerProvider, $compileProvider, $filterProvider, $provide, $httpProvider) {

        var app = angular.module('eventsapp');
        app.controller = $controllerProvider.register;
        app.directive = $compileProvider.directive;
        app.filter = $filterProvider.register;
        app.factory = $provide.factory;
        app.service = $provide.service;
        app.constant = $provide.constant;
        app.value = $provide.value;
        $httpProvider.defaults.withCredentials = true;
        $httpProvider.defaults.headers.common['Authorization'] = 'Bearer ' + localStorage.getItem('accessToken');
        $httpProvider.interceptors.push('httpInterceptor');
    }

    // Lazy load configuration
    lazyLoadConfig.$inject = ['$ocLazyLoadProvider', 'VENDOR_ASSETS'];
    function lazyLoadConfig($ocLazyLoadProvider, VENDOR_ASSETS) {

        $ocLazyLoadProvider.config({
            debug: false,
            events: true,
            modules: VENDOR_ASSETS.modules,
            modulesAr: VENDOR_ASSETS.modulesAr
        });

    }

})();


