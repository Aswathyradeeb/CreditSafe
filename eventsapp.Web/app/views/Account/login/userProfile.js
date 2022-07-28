(function () {
    'use strict';

    angular
        .module('eventsapp')
        .factory('UserProfile', UserProfile);
    /* @ngInject */
    function UserProfile($http) {
        var setProfile = function (userName, token, refreshToken, firstName, lastName, emailConfirmed, phoneNumber, phoneNumberConfirmed, roleName, userId, companyId, eventId, registrationTypeId) {
            debugger;
            localStorage.setItem('userName', userName);
            localStorage.setItem('accessToken', token);
            localStorage.setItem('refreshToken', refreshToken);
            localStorage.setItem('firstName', firstName);
            localStorage.setItem('lastName', lastName);
            localStorage.setItem('emailConfirmed', emailConfirmed);
            localStorage.setItem('phoneNumber', phoneNumber);
            localStorage.setItem('phoneNumberConfirmed', phoneNumberConfirmed);
            localStorage.setItem('roleName', roleName);  
            localStorage.setItem('companyId', companyId);
            localStorage.setItem('eventId', eventId);
            localStorage.setItem('userId', userId);
            localStorage.setItem('registrationTypeId', registrationTypeId);
            $http.defaults.withCredentials = true;
            $http.defaults.headers.common['Authorization'] = 'Bearer ' + token;
        };

        var setUserTypeCode = function (userTypeCode) {
            localStorage.setItem('userTypeCode', userTypeCode);
        };
        // sharpoint - angular study (build new for profile) - task -  
        var getProfile = function () {
            var profile = {
                isLoggedIn: localStorage.getItem('accessToken') != null,
                username: localStorage.getItem('userName'),
                token: localStorage.getItem('accessToken'),
                refreshToken: localStorage.getItem('refreshToken'),
                firstName: localStorage.getItem('firstName'),
                lastName: localStorage.getItem('lastName'),
                emailConfirmed: localStorage.getItem('emailConfirmed'),
                phoneNumber: localStorage.getItem('phoneNumber'),
                phoneNumberConfirmed: localStorage.getItem('phoneNumberConfirmed'), 
                companyId: localStorage.getItem('companyId'), 
                eventId: localStorage.getItem('eventId'),
                roleName: localStorage.getItem('roleName'),
                userId: localStorage.getItem('userId'),
                registrationTypeId: localStorage.getItem('registrationTypeId')

            }; 
            return profile;
        }

        return {
            setProfile: setProfile,
            getProfile: getProfile,
            setUserTypeCode: setUserTypeCode
        }
    }
    UserProfile.$inject = ['$http'];

})();