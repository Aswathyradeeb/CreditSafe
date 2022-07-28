/**=========================================================
 * Module: uploadingFiles
 * Reuse cases of uploading files
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('eventsapp')
        .directive('uploadingFiles', uploadingFiles)

    uploadingFiles.$inject = ['FileUploader', '$rootScope', '$http', '$filter', '$window', 'browser'];

    function uploadingFiles(FileUploader, $rootScope, $http, $filter, $window, browser) {
        return {
            replace: false,
            scope: {
                'copyurl': "=",
                'copyurlfullpath': '=',
                'url': '=',
                'maxsize': '=?',
                'isreadonly': '=?',
                'allowfiletype': '=?'
            },
            templateUrl: '/app/views/Controls/UploadingFilesDirective/UploadingFilesDirectiveTemplate.html',
            link: link
        };

        function link(scope, element, attrs) {
            var unwatch = scope.$watch('url', function (newVal, oldVal) {
                if (newVal) {

                    init();
                    // remove the watcher
                    unwatch();
                }
                else { }
            });

            scope.GetMb = function (kb) {
                return (kb / 1024) / 1024;
            };

            function init() {
                if (scope.maxsize == undefined) {
                    scope.maxsize = 20971520;
                }

                scope.maxsizeMB = scope.GetMb(scope.maxsize);

                scope.fileType = "image/*, .docx, .pdf , .xlsx";

                if (scope.allowfiletype != null || scope.allowfiletype != undefined) {
                    scope.fileType = scope.allowfiletype.join(",");
                }

                if (scope.copyurlfullpath) {
                    switch (scope.copyurlfullpath.split('.')[1]) {
                        case "jpg":
                        case "png":
                        case "jpeg":
                            scope.fileExtension = "fa fa-2x fa-file-image-o";
                            break;
                        case "docx":
                            scope.fileExtension = "fa fa-2x fa-file-word-o";
                            break;
                        case "pdf":
                            scope.fileExtension = "fa fa-2x fa-file-pdf-o";
                            break;
                    }
                }

                var uploadUrl = $rootScope.app.httpSource + scope.url;

                scope.uploader = new FileUploader({
                    autoUpload: true,
                    url: uploadUrl
                });

                scope.uploader.onSuccessItem = function (fileItem, response, status, headers) {

                    scope.copyurl = response.fileName;
                    scope.copyurlfullpath = response.httpPath;
                    scope.$apply();
                };
                scope.uploader.onErrorItem = function (fileItem, response, status, headers) {

                    scope.copyurl = "";
                    scope.copyurlfullpath = "";
                    scope.$apply();
                };

                scope.uploader.onAfterAddingFile = function (fileItem, response, status, headers) {
                    //if (fileItem.file.type !== "image/jpeg" && fileItem.file.type !== "image/png" && fileItem.file.type !== "image/jpg") {
                    if (scope.allowfiletype.includes(fileItem.file.name.substr(fileItem.file.name.lastIndexOf('.'), fileItem.file.name.length).toLowerCase()) == false) {
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
                    //switch (fileItem.file.name.split('.')[1]) {
                    //    case "jpg":
                    //    case "png":
                    //    case "jpeg":
                    //        scope.fileExtension = "fa fa-2x fa-file-image-o";
                    //        break;
                    //    case "docx":
                    //        scope.fileExtension = "fa fa-2x fa-file-word-o";
                    //        break;
                    //    case "pdf":
                    //        scope.fileExtension = "fa fa-2x fa-file-pdf-o";
                    //        break;
                    //}
                };

                scope.uploadAgain = function () {
                    $http.get($rootScope.app.httpSource + 'api/Upload/DeleteFile?fileTodelete=' + scope.copyurl)
                        .then(function (resp) {
                            delete scope.copyurlfullpath;
                            delete scope.copyurl;
                            scope.uploader.queue.pop();
                        }, function (response) { });
                };
            }
        }
    }


})();
