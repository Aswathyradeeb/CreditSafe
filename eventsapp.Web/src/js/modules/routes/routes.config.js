/**=========================================================
 * Module: RoutesConfig.js
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('eventsapp')
        .config(routesConfig);

    routesConfig.$inject = ['$locationProvider', '$stateProvider', '$urlRouterProvider', 'RouteProvider'];
    function routesConfig($locationProvider, $stateProvider, $urlRouterProvider, Route) {

        // use the HTML5 History API
        $locationProvider.html5Mode(false);

        // Default route
        //$urlRouterProvider.otherwise('/app/dashboard');
        $urlRouterProvider.otherwise(function ($injector, $location) {
            debugger
            var $state = $injector.get("$state");
            $state.go("app.home");
        });

        // Application Routes States
        $stateProvider
            .state('app', {
                url: '/app',
                abstract: true,
                templateUrl: Route.base('app.html'),
                resolve: {
                    _assets: Route.require('icons', 'screenfull', 'sparklines', 'slimscroll', 'toaster', 'animate', 'blueimp-gallery')
                }
            })
            .state('app1', {
                url: '/app1',
                abstract: true,
                templateUrl: Route.base('app1.html'),
                resolve: {
                    _assets: Route.require('icons', 'screenfull', 'sparklines', 'slimscroll', 'toaster', 'animate', 'blueimp-gallery')
                }
            })
            .state('app.home', {
                url: '/home',
                templateUrl: Route.base('Cities/cities.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'easypiechart', 'blueimp-gallery')
                },
                requireAuth: false
            })
            .state('app.city', {
                url: '/city',
                templateUrl: Route.base('Cities/cities.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'layerMorph', 'moment', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'easypiechart')
                },
                requireAuth: false
            })
            .state('app.cityweather', {
                url: '/cityweather',
                templateUrl: Route.base('CityWeather/citiesWeather.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'angularWizard', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'fuelux', 'blueimp-gallery', 'vectormap', 'clockpicker')
                },
                requireAuth: false
            })

    
    }

})();

