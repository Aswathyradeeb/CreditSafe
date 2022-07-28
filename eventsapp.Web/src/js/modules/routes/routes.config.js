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
                templateUrl: Route.base('Home/home.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'easypiechart', 'blueimp-gallery')
                },
                requireAuth: true
            })
            .state('app.MyEvents', {
                url: '/MyEvents',
                templateUrl: Route.base('MyEvents/MyEvents.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'layerMorph', 'moment', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'easypiechart')
                },
                requireAuth: true
            })
            .state('app.event', {
                url: '/event/:id',
                templateUrl: Route.base('Event/page.event.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'angularWizard', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'fuelux', 'blueimp-gallery', 'vectormap', 'clockpicker')
                },
                requireAuth: true
            })

            .state('app.eventattendees', {
                url: '/eventUser/',
                templateUrl: Route.base('Event/EventAttendees/EventUser.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'angularWizard', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'fuelux', 'blueimp-gallery', 'vectormap', 'clockpicker', 'loadGoogleMapsJS', function () {
                        return loadGoogleMaps();
                    }, 'ui.map')
                },
                requireAuth: true
            })
            .state('app.visitorAttendance', {
                url: '/visitorAttendance/',
                templateUrl: Route.base('Event/EventAttendees/visitorAttendance.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload')
                },
                requireAuth: false
            })
            .state('app.userEventregister', {
                url: '/userEventregister/:id',
                templateUrl: Route.base('Event/userevent/page.userEventregister.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'angularWizard', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'fuelux', 'blueimp-gallery', 'vectormap', 'clockpicker', 'loadGoogleMapsJS', function () {
                        return loadGoogleMaps();
                    }, 'ui.map')
                },
                requireAuth: true
            })
            .state('app.dashboard', {
                url: '/dashboard',
                templateUrl: Route.base('Dashboard/page.dashboard.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'angularWizard', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'vectormap', 'vectormap-maps', 'flot-chart', 'flot-chart-plugins', 'highCharts')
                },
                requireAuth: false
            })

            .state('app.questionare', {
                url: '/questionare',
                templateUrl: Route.base('Questionare/page.questionare.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'angularWizard', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'vectormap', 'vectormap-maps', 'flot-chart', 'flot-chart-plugins')
                },
                requireAuth: false
            })



            .state('app.PassCode', {
                url: '/PassCode',
                templateUrl: Route.base('PassCode/page.PassCode.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'angularWizard', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'vectormap', 'vectormap-maps')
                },
                requireAuth: true
            })
            .state('app.Certificate', {
                url: '/Certificate',
                templateUrl: Route.base('Certificate/Certificate.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'angularWizard', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'vectormap', 'vectormap-maps')
                },
                requireAuth: true
            })
            .state('app.survey', {
                url: '/Survey',
                templateUrl: Route.base('Survey/page.surveyregister.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'angularWizard', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'vectormap', 'vectormap-maps')
                },
                requireAuth: true
            })
            .state('app.Questions', {
                url: '/Questions',
                templateUrl: Route.base('Questions/questions.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'angularWizard', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'vectormap', 'vectormap-maps')
                },
                requireAuth: true
            })
            .state('app.speakers', {
                url: '/speakers',
                templateUrl: Route.base('Speakers/speakers.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'angularWizard', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'fuelux')
                },
                requireAuth: true
            })
            .state('app.sponsors', {
                url: '/sponsors',
                templateUrl: Route.base('Sponsors/sponsor.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'angularWizard', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'fuelux')
                },
                requireAuth: true
            })
            .state('app.companies', {
                url: '/companies',
                templateUrl: Route.base('Companies/companies.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'angularWizard', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'fuelux', 'loadGoogleMapsJS', function () {
                        return loadGoogleMaps();
                    }, 'ui.map')
                },
                requireAuth: true
            })
            .state('app.athletes', {
                url: '/athletes',
                templateUrl: Route.base('Athletes/athletes.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'angularWizard', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'fuelux')
                },
                requireAuth: true
            })
            .state('app.allguests', {
                url: '/allguests',
                templateUrl: Route.base('Guests/allGuests.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'angularWizard', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'fuelux')
                },
                requireAuth: true
            })
            .state('app.restaurants', {
                url: '/restaurants',
                templateUrl: Route.base('Restaurants/restaurants.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'angularWizard', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'fuelux')
                },
                requireAuth: true
            })
            .state('app.guests', {
                url: '/guests/:id?',
                templateUrl: Route.base('Athletes/Guests/guests.html'),
                resolve: {
                    assets: Route.require('blueimp-gallery', 'ui.select', 'datatables', 'filestyle', 'angularFileUpload', 'moment', 'ngMask', 'draganddrop', 'xeditable', 'oitozero.ngSweetAlert', 'ngImgCrop')
                },
                requireAuth: true
            })
            .state('app.vouchers', {
                url: '/vouchers',
                templateUrl: Route.base('Vouchers/vouchers.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'angularWizard', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'fuelux')
                },
                requireAuth: true
            })
            .state('app.verifyVoucher', {
                url: '/verifyVoucher/:id/:eventId',
                templateUrl: Route.base('Vouchers/verifyVoucher/verifyVoucher.html'),
                resolve: {
                    assets: Route.require('blueimp-gallery', 'ui.select', 'filestyle', 'ngMask', 'oitozero.ngSweetAlert', 'ngImgCrop', 'moment')
                },
                requireAuth: true
            })
            .state('app.userCompany', {
                url: '/userCompany',
                templateUrl: Route.base('UserCompany/userCompany.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'angularWizard', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'fuelux', 'loadGoogleMapsJS', function () {
                        return loadGoogleMaps();
                    }, 'ui.map')
                },
                requireAuth: true
            })
            .state('app.userProfile', {
                url: '/userProfile',
                templateUrl: Route.base('UserPerson/userPerson.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'angularWizard', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'fuelux')
                },
                requireAuth: true
            })
            .state('app.profileSetup', {
                url: '/profileSetup',
                templateUrl: Route.base('UserPerson/userPerson.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'angularWizard', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'fuelux', 'loadGoogleMapsJS', function () {
                        return loadGoogleMaps();
                    }, 'ui.map')
                },
                requireAuth: false
            })
            .state('app.companyProfileSetup', {
                url: '/companyProfileSetup',
                templateUrl: Route.base('UserCompany/userCompany.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'angularWizard', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'fuelux', 'loadGoogleMapsJS', function () {
                        return loadGoogleMaps();
                    }, 'ui.map')
                },
                requireAuth: false
            })
            .state('app.users', {
                url: '/users',
                templateUrl: Route.base('Account/Users/users.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'angularWizard', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'fuelux')
                },
                requireAuth: true
            })
            .state('app.reports', {
                url: '/reports',
                templateUrl: Route.base('Reports/Reports.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'angularWizard', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'fuelux')
                },
                requireAuth: true
            })
            .state('app.profile', {
                url: '/profile/:id?',
                templateUrl: Route.base('Account/profile/app.profile.html'),
                resolve: {
                    assets: Route.require('blueimp-gallery', 'ui.select', 'datatables', 'filestyle', 'angularFileUpload', 'moment', 'ngMask', 'draganddrop', 'xeditable', 'oitozero.ngSweetAlert', 'ngImgCrop')
                },
                requireAuth: true
            })
            .state('page', {
                url: '/page',
                templateUrl: Route.base('page.html'),
                resolve: {
                    assets: Route.require('icons', 'animate')
                }
            })
            .state('page.login', {
                url: '/login',
                templateUrl: Route.base('Account/login/page.login.html'),
                requireAuth: false,
                resolve: {
                    assets: Route.require('oitozero.ngSweetAlert')
                }
            })
            .state('page.register', {
                url: '/register',
                templateUrl: Route.base('Account/register/page.register.html'),
                resolve: {
                    assets: Route.require('ui.select', 'oitozero.ngSweetAlert', 'ngMask', 'moment')
                },
                requireAuth: false
            })
            .state('page.registerAdmin', {
                url: '/registerAdmin',
                templateUrl: Route.base('Account/register/page.register.html'),
                resolve: {
                    assets: Route.require('ui.select', 'oitozero.ngSweetAlert', 'ngMask')
                },
                requireAuth: false
            }).state('page.confirmEmail', {
                url: '/confirmEmail?userId&code',
                templateUrl: Route.base('Account/confirmEmail/page.confirmEmail.html'),
                resolve: {
                    assets: Route.require('oitozero.ngSweetAlert')
                },
                requireAuth: false
            }).state('page.confirmPhone', {
                url: '/confirmPhone',
                templateUrl: Route.base('Account/confirmPhone/page.confirmPhone.html'),
                requireAuth: false
            }).state('page.recover', {
                url: '/recover',
                templateUrl: Route.base('Account/recover/page.recover.html'),
                resolve: {
                    assets: Route.require('oitozero.ngSweetAlert')
                },
                requireAuth: false
            }).state('page.reset', {
                url: '/resetPassword?userId&code',
                templateUrl: Route.base('Account/recover/page.reset.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'angularWizard', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'fuelux', 'blueimp-gallery', 'vectormap', 'clockpicker')
                },
                requireAuth: false
            }).state('page.setUserPassword', {
                url: '/setUserPassword?userId',
                templateUrl: Route.base('Account/setPassword/page.setPassword.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'angularWizard', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'fuelux', 'blueimp-gallery', 'vectormap', 'clockpicker')
                },
                requireAuth: false
            })
            .state('page.lock', {
                url: '/lock',
                templateUrl: Route.base('page.lock.html')
            })
            .state('app.subsciptions', {
                url: '/subsciptions',
                templateUrl: Route.base('Subsciptions/Subsciptions.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'angularWizard', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'fuelux', 'blueimp-gallery', 'vectormap', 'clockpicker')
                },
                requireAuth: true
            })
            .state('app.viewPayment', {
                url: '/viewPayment/:transactionId',
                templateUrl: Route.base('viewPayment/page.viewPayment.html'),
                resolve: {
                    assets: Route.require('datatables', 'oitozero.ngSweetAlert', 'angularWizard', 'layerMorph', 'icons', 'ui.select', 'moment', 'filestyle', 'angularFileUpload', 'fuelux', 'blueimp-gallery', 'vectormap', 'clockpicker')
                },
                requireAuth: true
            });
    }

})();

