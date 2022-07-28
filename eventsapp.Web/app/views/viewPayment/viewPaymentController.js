(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('viewPaymentController', viewPaymentController);

    function viewPaymentController($rootScope, $http, $scope, $stateParams, $state, WizardHandler, $filter, $compile, UserProfile, SweetAlert) {
        var vm = this;  

        $http.get($rootScope.app.httpSource + 'api/Payment/GetTransaction?transactionId=' + $stateParams.transactionId)
            .then(function (resp) { 
                vm.transaction = resp.data;
            });
    }

    viewPaymentController.$inject = ['$rootScope', '$http', '$scope', '$stateParams', '$state', 'WizardHandler', '$filter',
         '$compile', 'UserProfile', 'SweetAlert'];

})();