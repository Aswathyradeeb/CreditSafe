/**=========================================================
 * Module: profileNationality
 * Reuse cases of nationality in user profile page
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('eventsapp')
        .directive('profileNationality', profileNationality)

    profileNationality.$inject = ['$rootScope', '$http', '$filter', '$window', 'browser'];

    function profileNationality( $rootScope, $http, $filter, $window, browser) {
        return {
            restrict: 'E',
            scope: {
                passModel: "=ngModel"
            },
            templateUrl: '/app/views/Controls/person/profileNationalityDirectiveTemplate.html',
            link: link
        };

        function link(scope, element, attrs) {
            // Static Values : Configuration
            var UAE_Code = "ARE";
            var ZFComponentName = "ZFComponent";
            scope.uploadPhotoUrl = 'api/Upload/UploadFile?uploadFile=Attachment';
            scope.uploadPassportUrl = 'api/Upload/UploadFile?uploadFile=Attachment';
            scope.uploadEmiratesIdUrl = 'api/Upload/UploadFile?uploadFile=Attachment';
            //-------------------------------------------------------------------------------
            debugger;
            scope.isChrome = browser.chrome;

            // TODO: Should make this code generic to be read accros the system
            //-------------------------------------
            // Detect IE
            //-------------------------------------
            var detectIE = function () {
                var ua = $window.navigator.userAgent;

                // Test values; Uncomment to check result …

                // IE 10
                // ua = 'Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0)';

                // IE 11
                // ua = 'Mozilla/5.0 (Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko';

                // Edge 12 (Spartan)
                // ua = 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36 Edge/12.0';

                // Edge 13
                // ua = 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2486.0 Safari/537.36 Edge/13.10586';

                var msie = ua.indexOf('MSIE ');
                if (msie > 0) {
                    // IE 10 or older => return version number
                    return parseInt(ua.substring(msie + 5, ua.indexOf('.', msie)), 10);
                }

                var trident = ua.indexOf('Trident/');
                if (trident > 0) {
                    // IE 11 => return version number
                    var rv = ua.indexOf('rv:');
                    return parseInt(ua.substring(rv + 3, ua.indexOf('.', rv)), 10);
                }

                var edge = ua.indexOf('Edge/');
                if (edge > 0) {
                    // Edge (IE 12+) => return version number
                    return parseInt(ua.substring(edge + 5, ua.indexOf('.', edge)), 10);
                }

                // other browser
                return false;
            }
            //-------------------------------------

            //Date Popup Options
            scope.clear = function () {
                scope.passModel.dateOfBirth = null;
            };

            var start = new Date();
            start.setFullYear(start.getFullYear() - 97);
            var end = new Date();
            end.setFullYear(end.getFullYear() - 16);

            scope.dateOptions = {
                minDate: start,
                maxDate: end,
                startingDay: 1,
                todayBtn: false
            };

            scope.opendateOfBirthDatePopup = function () {
                scope.dateOfBirthPopup.opened = true;
            };

            scope.setDate = function (year, month, day) {
                scope.passModel.dateOfBirth = new Date(year, month, day);
            };

            scope.format = 'dd-MMMM-yyyy';

          
            scope.toggleMin = function () {
                scope.minDate = start;
                //scope.minDate = scope.minDate ? null : new Date();
            };

            scope.dateOfBirthPopup = {
                opened: false
            };
            //END

            scope.isIE = detectIE();
            scope.isIE64 = ($window.navigator.userAgent.indexOf("x64") > 0);

            var unwatch = scope.$watch('passModel', function (newVal, oldVal) {
                if (newVal) {
                    init();
                    // remove the watcher
                    unwatch();
                }
                else {
                    // ------------------------------------
                    // Model 
                    // ------------------------------------
                    scope.passModel = {
                        //EmiratesId: 0,
                        //passportNumber: 0,
                        //dateOfBirth: "",
                        //genderId: "",
                       
                        photoUrl: "",
                        emiratesIdCopyUrl: "",
                        passportCopyUrl: ""
                    }

                    // ------------------------------------
                    // Lookups Services
                    // ------------------------------------
                    $http.get($rootScope.app.httpSource + 'api/Country')
                        .then(function (response) {
                            scope.countries = response.data;
                        },
                        function (response) { });

                    $http.get($rootScope.app.httpSource + 'api/Title')
                        .then(function (response) {
                            scope.titles = response.data;
                            if (scope.passModel.titleId) { // if edit mode set combo from data
                                scope.userTitle = $filter('filter')(scope.titles, { id: scope.passModel.titleId }, true)[0];
                            }
                        },
                        function (response) { });
                }
            });

            function init() {
                // ------------------------------------
                // Lookups Services
                // ------------------------------------
                $http.get($rootScope.app.httpSource + 'api/Country')
                    .then(function (response) {
                        scope.countries = response.data;
                    },
                    function (response) { });

                $http.get($rootScope.app.httpSource + 'api/Title')
                    .then(function (response) {
                        scope.titles = response.data;
                        if (scope.passModel.titleId) { // if edit mode set combo from data
                            scope.userTitle = $filter('filter')(scope.titles, { id: scope.passModel.titleId }, true)[0];
                        }
                    },
                    function (response) { });
            }

            //----------------------------------------------------
            //Emirates ID Kit Code
            //----------------------------------------------------
            scope.EIdReadError = $filter('translate')('profileNationalityDirective.ReadEmiratesIdError');
            scope.isReadingEId = false;

            scope.readEmiratesIdPublicData = function () {
                scope.isReadingEId = true;

                try {
                    var ZFComponent = document.getElementById(ZFComponentName);

                    var Ret = ZFComponent.Initialize();

                    if (Ret != "") {
                        scope.isReadingEId = false;
                        //TODO: change alert to proper validation mechanism
                        alert(scope.EIdReadError);

                        return false;
                    }

                    Ret = ZFComponent.ReadPublicData(true, true,
                    true, true, true, true);

                    if (Ret != "") {
                        scope.isReadingEId = false;
                        //TODO: change alert to proper validation mechanism
                        alert(scope.EIdReadError);
                        return false;
                    }


                    var EmiratesIdRequestDTO = {
                        ef_idn_cn: ZFComponent.GetEF_IDN_CN(),
                        ef_non_mod_data: ZFComponent.GetEF_NonModifiableData(),
                        ef_mod_data: ZFComponent.GetEF_ModifiableData(),
                        ef_sign_image: ZFComponent.GetEF_HolderSignatureImage(),
                        ef_photo: ZFComponent.GetEF_Photography(),
                        ef_root_cert: ZFComponent.GetEF_RootCertificate(),
                        ef_home_address: ZFComponent.GetEF_HomeAddressData(),
                        ef_work_address: ZFComponent.GetEF_WorkAddressData()
                    }

                    var sendurl = $rootScope.app.httpSource + 'EmiratesIDGetPublicData';
                    $http.post(sendurl, EmiratesIdRequestDTO)
                    .then(function (response) {
                        applyServerEIdPublicData(response.data);
                        scope.isReadingEId = false;
                        console.log(response);
                    },
                    function (response) { // optional
                        scope.isReadingEId = false;
                        //TODO: change alert to proper validation mechanism
                        alert(scope.EIdReadError);

                    });

                }
                catch (e) {
                    //TODO: translate error message
                    //TODO: show something help user to know how to activate applet and activex from browser
                    //TODO: change alert to proper validation mechanism
                    alert(scope.EIdReadError);
                    return false;
                }
            }

            var applyServerEIdPublicData = function (retData) {
                // for UI changes
                scope.isValidResidence = true;
                scope.userCountry = $filter('filter')(scope.countries, { isoCode3: retData.nationality }, true)[0];
                scope.photoUrlToShow = retData.photoHttpPath;

                // Model Changes
                scope.passModel.nationalityId = scope.userCountry.id;
                scope.passModel.emiratesId = retData.idn;
                scope.passModel.genderId = retData.sex;
                scope.passModel.dateOfBirth = moment(retData.doB, "dd-MMMM-YYYY").toDate();
                scope.passModel.photoUrl = retData.photoFileName

                // show photo on UI


            }
            //----------------------------------------------------
        }


    }


})();
