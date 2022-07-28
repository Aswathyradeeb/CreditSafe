(function () {
    'use strict';

    angular
        .module('eventsapp')
        .factory('LoginService', LoginService);
    /* @ngInject */
    function LoginService($http, $rootScope) {
        this.login = function (userLogin) {
            var resp = $http({
                url: $rootScope.app.httpSource + 'TOKEN',
                method: 'POST',
                data: $.param({
                    grant_type: 'password',
                    username: userLogin.userName,
                    password: userLogin.password
                }),
                headers: { 'Content-Type': 'application/x-www-form-urlencoded', 'Authorization': 'Bearer null' }
            });
            return resp;
        };

        this.logout = function () {
            debugger;
            $http.post($rootScope.app.httpSource + 'api/Account/Logout', null);
            localStorage.removeItem('accessToken');
            localStorage.removeItem('userName');
            localStorage.removeItem('refreshToken');
            localStorage.removeItem('firstName');
            localStorage.removeItem('lastName');
            localStorage.removeItem('emailConfirmed');
            localStorage.removeItem('phoneNumber');
            localStorage.removeItem('phoneNumberConfirmed');
            localStorage.removeItem('userProfileCompleted');
            localStorage.removeItem('companyId');
            localStorage.removeItem('lastLoginDate');
            localStorage.removeItem('eventId');
            localStorage.removeItem('roleName');
            localStorage.removeItem('userId'); 
        };

        return {
            login: this.login,
            logout: this.logout
        }
    }
    LoginService.$inject = ['$http', '$rootScope'];

})();