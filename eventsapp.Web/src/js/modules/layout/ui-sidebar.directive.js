/**=========================================================
 * Module: SidebarDirective
 * Wraps the sidebar. Handles collapsed state and slide
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('eventsapp')
        .directive('uiSidebar', uiSidebar);

    uiSidebar.$inject = ['$rootScope', '$window', '$timeout', 'MEDIA_QUERY', '$http', '$uibModal', '$state', '$filter', 'SweetAlert'];
    function uiSidebar($rootScope, $window, $timeout, MEDIA_QUERY, $http, $uibModal, $state, $filter, SweetAlert) {

        return {
            restrict: 'A',
            link: link
        };

        function link(scope, element) {

            $rootScope.IsLoadingMenu = true;
            $http.defaults.withCredentials = true;
            $http.defaults.headers.common['Authorization'] = 'Bearer ' + localStorage.getItem('accessToken');
            $http.get($rootScope.app.httpSource + 'api/Menu/GetMenu')
               .then(function (response) {
                   $rootScope.IsLoadingMenu = false;
                   $rootScope.userMenu = response.data;

                   $timeout(function () {
                       element.find('a').on('click', function (event) {

                           var ele = angular.element(this),
                               par = ele.parent()[0];

                           // remove active class (ul > li > a)
                           var lis = ele.parent().parent().children();
                           angular.forEach(lis, function (li) {
                               if (li !== par)

                                   angular.element(li).removeClass('active');
                           });

                           var next = ele.next();
                           if (next.length && next[0].tagName === 'UL') {

                               ele.parent().toggleClass('active');
                               event.preventDefault();
                           }
                       });
                   });
               },
               function (response) { // optional
                   $rootScope.IsLoadingMenu = false;
               });

            $http.get($rootScope.app.httpSource + 'api/UserProfile')
                .then(function (resp) {
                    scope.userProfile = resp.data;
                },
                function (response) { });

            // on mobiles, sidebar starts off-screen
            if (onMobile()) $timeout(function () {

                $rootScope.app.sidebar.isOffscreen = true;
            });
            // hide sidebar on route change
            $rootScope.$on('$stateChangeStart', function () {

                if (onMobile())
                    $rootScope.app.sidebar.isOffscreen = true;
            });

            $window.addEventListener('resize', function () {
                $timeout(function () {

                    $rootScope.app.sidebar.isOffscreen = onMobile();
                });
            });

            function onMobile() {

                return $window.innerWidth < MEDIA_QUERY.tablet;
            }

        }
    }
})();
