(function () {
    'use strict';

    angular
        .module('eventsapp')
        .controller('eventPrintController', eventPrintController);
    /* @ngInject */
    function eventPrintController($rootScope, $scope, $http, $uibModalInstance, $filter, attendeid) {
       
        var vm = this;
        $scope.bgImage = "/app/img/bg-badge.jpg";
       

        if (attendeid !== null) {
            $http.get($rootScope.app.httpSource + 'api/EventUser/GetEventUsersbyAttendeeID?attId=' + attendeid)
                .then(function (response) {
                    debugger;
                    $scope.eventUser = response.data;
                    $scope.personObj = response.data.user;
                    $scope.eventObj = response.data.event;
                    console.log($scope.personObj);
                    $http.get($rootScope.app.httpSource + 'api/Lookup/GetBackgroundById/' + $scope.eventObj.bgImageId)
                        .then(function (resp) {
                            $scope.bgImage = (resp != null) ? resp.data : "/app/img/bg-badge.jpg";
                        });
                    JsBarcode("#barcode", $scope.personObj.firstName, {                
                        displayValue: false
                    });
                   // JsBarcode("#barcode", $scope.personObj.firstName);
                }, function (response) { });        
        }
        else {
            $scope.personObj = {};
        }
       
        $scope.printDiv = function (divName) {
            var printContents = document.getElementById(divName).innerHTML;
            var popupWin = window.open('', '_blank', 'width=100%,height=100%‬');
            popupWin.document.open();
            popupWin.document.write('<html><head><style> @page { size: auto;  margin: 0mm; } * {-webkit-print-color-adjust: exact !important;color-adjust: exact !important;}@media print{.col-md-12 {width:100%;}  .text-center { text-align: center; } .noprint  { display: none; }   #printdiv .member-card {position: relative;top: 20%;left: 30%;} #printdiv { height:600px; background-repeat: no-repeat; background-size:cover; background-image: url(' + $scope.bgImage +');background-size: 100% 100%;} } @media screen { .noscreen { visibility: hidden; position: absolute; } }</style></head><body onload="window.print()"><div  id="printdiv" style="margin:0px; padding:20px 0 0 0;" >' + printContents + '</div></body></html>');
            popupWin.document.close();
        } 
       
        $scope.ok = function () {
            if ($scope.personObj.id == undefined) {
                $http.post($rootScope.app.httpSource + 'api/Speaker/CreateSpeaker', $scope.personObj)
                    .then(function (response) {
                        $uibModalInstance.close($scope.personObj);
                    }, function (response) { });
            }
            else {
                $http.post($rootScope.app.httpSource + 'api/Speaker/UpdatePerson', $scope.personObj)
                    .then(function (response) {
                        $uibModalInstance.close($scope.personObj);
                    }, function (response) { });
            }
        };

        $scope.closeModal = function () {
            $uibModalInstance.dismiss('cancel');
        };

    }

    eventPrintController.$inject = ['$rootScope', '$scope', '$http', '$uibModalInstance', '$filter', 'attendeid'];
})();