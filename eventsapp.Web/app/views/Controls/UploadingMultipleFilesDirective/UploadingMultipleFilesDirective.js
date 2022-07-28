/**=========================================================
 * Module: uploadingFiles
 * Reuse cases of uploading files
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('eventsapp')
        .directive('uploadingMultipleFiles', uploadingMultipleFiles)

    uploadingMultipleFiles.$inject = ['FileUploader', '$rootScope', '$http', '$filter', '$window', 'browser', '$timeout'];

    function uploadingMultipleFiles(FileUploader, $rootScope, $http, $filter, $window, browser, $timeout) {
        return {
            replace: false,
            scope: {
                'id': '=?',
                'controlmodel': '=',
                'url': '=',
                'maxsize': '=?',
                'uploadAllAction': '=?',
                'deleteAction': '=?',
                "counter": '=?',
                "enableUpload": '=?',
                "isRequired": '=?',
                'isreadonly': '=?',
                'iscopylink': '=?'
            },
            templateUrl: '/app/views/Controls/UploadingMultipleFilesDirective/UploadingMultipleFilesDirectiveTemplate.html',
            link: link
        };

        function link(scope, element, attrs) {

            var unwatch = scope.$watch('url', function (newVal, oldVal) {
                if (newVal) {

                    init();
                    // remove the watcher
                    unwatch();
                }
                else {

                }
            });

            var unwatchControlmodel = scope.$watch('controlmodel', function (newVal, oldVal) {
                if (newVal) {
                    init();
                    // remove the watcher
                    unwatchControlmodel();
                }
                else {

                }
            });

            function init() {
                var uploadUrl = $rootScope.app.httpSource + scope.url;
                scope.counter = 0;
                scope.fileUploader = new FileUploader({
                    url: uploadUrl
                });

                // FILTERS

                scope.fileUploader.filters.push({
                    name: 'customFilter',
                    fn: function (item /*{File|FileLikeObject}*/, options) {
                        return this.queue.length < 10;
                    }
                });

                scope.controlmodelFiltered = scope.controlmodel;

                // CALLBACKS

                scope.fileUploader.onWhenAddingFileFailed = function (item /*{File|FileLikeObject}*/, filter, options) {
                    console.info('onWhenAddingFileFailed', item, filter, options);
                };
                scope.fileUploader.onAfterAddingFile = function (fileItem) {
                    console.info('onAfterAddingFile', fileItem);
                };
                scope.fileUploader.onAfterAddingAll = function (addedFileItems) {
                    console.info('onAfterAddingAll', addedFileItems);
                };
                scope.fileUploader.onBeforeUploadItem = function (item) {
                    console.info('onBeforeUploadItem', item);
                };
                scope.fileUploader.onProgressItem = function (fileItem, progress) {
                    console.info('onProgressItem', fileItem, progress);
                };
                scope.fileUploader.onProgressAll = function (progress) {
                    console.info('onProgressAll', progress);
                };
                scope.fileUploader.onSuccessItem = function (fileItem, response, status, headers) {
                    console.info('onSuccessItem', fileItem, response, status, headers);
                };
                scope.fileUploader.onErrorItem = function (fileItem, response, status, headers) {
                    console.info('onErrorItem', fileItem, response, status, headers);
                };
                scope.fileUploader.onCancelItem = function (fileItem, response, status, headers) {
                    console.info('onCancelItem', fileItem, response, status, headers);
                };
                scope.fileUploader.onCompleteItem = function (fileItem, response, status, headers) {
                    console.info('onCompleteItem', fileItem, response, status, headers);
                };
                scope.fileUploader.onCompleteAll = function () {
                    console.info('onCompleteAll');
                };

                console.info('fileUploader', scope.fileUploader);

                scope.fileUploader.uploadAll = function () {
                    angular.forEach(scope.fileUploader.queue, function (item, key) {
                        item.upload();
                        //item.remove();
                    });
                };


                if (scope.maxsize == undefined) {
                    scope.maxsize = 20971520;
                }


                if (scope.copyurlfullpath) {
                    switch (scope.copyurlfullpath.split('.')[1]) {
                        case "jpg":
                        case "png":
                        case "jpeg":
                            scope.fileExtension = "fa fa-2x fa-file-image-o";
                            break;
                    }
                }

                var uploadUrl = $rootScope.app.httpSource + scope.url;

                scope.fileUploader.onSuccessItem = function (fileItem, response, status, headers) {

                    var document = {
                        id: scope.id,
                        name: fileItem.file.name,
                        documentPath: response.fileName,
                        documentFullPath: response.httpPath
                    };
                    scope.fileUploader.queue[scope.counter].documentFullPath = document.documentFullPath;
                    scope.counter++;
                    if (scope.controlmodel == undefined) {
                        scope.controlmodel = [];
                    }
                    //scope.controlmodel.push(document);
                    scope.uploadAllAction(document);
                    scope.$apply();
                };
                scope.fileUploader.onErrorItem = function (fileItem, response, status, headers) {

                    scope.copyurl = "";
                    scope.copyurlfullpath = "";
                    scope.$apply();
                };

                scope.fileUploader.onAfterAddingFile = function (fileItem, response, status, headers) {


                    if (fileItem.file.type !== "image/jpeg" && fileItem.file.type !== "image/png" && fileItem.file.type !== "image/jpg" && fileItem.file.type !== "application/pdf" && fileItem.file.type !== "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && fileItem.file.type !== "application/msword" && fileItem.file.type !== "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && fileItem.file.type !== "application/vnd.ms-excel") {
                        fileItem.remove();
                        scope.incorrectType = true;
                    }
                    else {
                        scope.incorrectType = false;
                    }
                    if (fileItem.file.size > scope.maxsize) {
                        fileItem.remove();
                        scope.incorrectSize = true;
                    }
                    else {
                        scope.incorrectSize = false;
                    }
                    switch (fileItem.file.name.split('.').pop()) {
                        case "jpg":
                        case "png":
                        case "jpeg":
                            fileItem.file.fileExtension = "fa fa-2x fa-file-image-o";
                            break;
                        case "docx":
                            fileItem.file.fileExtension = "fa fa-2x fa-file-word-o";
                            break;
                        default:
                            fileItem.file.fileExtension = "fa fa-2x fa-file-pdf-o";
                            break;
                    }
                    fileItem.upload();
                    //scope.fileUploader.uploadAll();
                };

                scope.uploadAgain = function (index) {
                    $timeout(function () {
                        scope.$apply(function () {
                            scope.controlmodel.splice(index, 1);
                            scope.deleteAction(index);
                        });
                    }, 1000);
                };
            }
        }
    }
})();
