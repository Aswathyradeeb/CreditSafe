/**=========================================================
 * Module: RoutesRun
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('eventsapp')
        .run(appRun);
    /* @ngInject */
    function appRun($rootScope, $window, UserProfile, $state, $http) {

        // Hook not found
        $rootScope.$on('$stateNotFound',
            function (event, unfoundState, fromState, fromParams) {
                console.log(unfoundState.to); // "lazy.state"
                console.log(unfoundState.toParams); // {a:1, b:2}
                console.log(unfoundState.options); // {inherit:false} + default options
            });

        // Hook success
        $rootScope.$on('$stateChangeSuccess',
            function (event, toState, toParams, fromState, fromParams) {
                // success here
                // display new view from top
                $window.scrollTo(0, 0);
            });

        $rootScope.$on('$stateChangeStart', function (event, newUrl, toParams, fromState, fromParams) {
            var userProfile = UserProfile.getProfile();
            if (newUrl.requireAuth) {
                if (!userProfile.isLoggedIn) {
                    event.preventDefault();
                    console.log('DENY');
                    //$location.path('/login');
                    $state.go('page.login');
                    return;
                }
            }
            if (newUrl.name != 'app.userCompany' && userProfile.roleName == 3 && (userProfile.companyId == 'null' || userProfile.companyId == 'undefined')) {
                //Redirecting to company if company not registered for admin
                $http.get($rootScope.app.httpSource + 'api/Company/GetCompanyByUser?uid=' + userProfile.userId)
                    .then(function (response) {
                        console.log(response.data.result);
                        if (response.data.result.id == 0) {
                            $state.go("app.userCompany");
                        }

                    });
            }
            if (newUrl.name != 'app.userCompany' && userProfile.roleName == 2 &&
                (userProfile.registrationTypeId == $rootScope.lookup.registrationTypes.Exhibitor ||
                    userProfile.registrationTypeId == $rootScope.lookup.registrationTypes.Sponsor ||
                    userProfile.registrationTypeId == $rootScope.lookup.registrationTypes.Partner)) {
                //Redirecting to company if company not registered for admin
                $http.get($rootScope.app.httpSource + 'api/Company/GetCompanyByUser?uid=' + userProfile.userId)
                    .then(function (response) {
                        console.log(response.data.result);
                        if (response.data.result.id == 0) {
                            $state.go("app.userCompany");
                        }

                    });
            }
            if (newUrl.name != 'app.userProfile' && userProfile.roleName == 2 &&
                (userProfile.registrationTypeId == $rootScope.lookup.registrationTypes.Speaker ||
                    userProfile.registrationTypeId == $rootScope.lookup.registrationTypes.VIP)) {
                $http.get($rootScope.app.httpSource + 'api/User/Get')
                    .then(function (response) {
                        if (response.data.personId == null || response.data.personId == 0) {
                            $state.go("app.userProfile");
                        }
                    });
            }
        });

        $rootScope.$on('unauthorized', function () {
            LoginService.logout();
        });

    }
    appRun.$inject = ['$rootScope', '$window', 'UserProfile', '$state', '$http'];

})();

