(function () {
    'use strict';

    angular
        .module('eventsapp')
        .directive('ckeditorAr', Directive);

    function Directive($rootScope) {
        return {
            require: 'ngModel',
            link: function (scope, element, attr, ngModel) {
                var editorOptions;
                if (attr.ckeditorAr === 'minimum') {
                    // minimum editor
                    editorOptions = {
                        height: 40,
                        toolbar: [
                            { name: 'basic', items: ['Bold', 'Italic', 'Underline'] },
                            { name: 'links', items: ['Link', 'Unlink'] },
                            { name: 'tools', items: ['Maximize'] },
                            { name: 'document', items: ['Source'] },
                        ],
                        removePlugins: '',
                        resize_enabled: true,
                        toolbarCanCollapse: true,
                        toolbarStartupExpanded: false,
                        contentsCss: ['app/css/styles-ar.css', 'app/css/bootstrap-rtl.css'],
                        allowedContent: true,
                        extraAllowedContent: '*(*);*{*}',
                        extraPlugins: 'colorbutton,imagebrowser,font, easyimage,image2,imagebrowser'
                    }; 
                } else {
                    // regular editor

                    editorOptions = {
                        height: 40,
                        removeButtons: '',
                        removePlugins: '',
                        toolbarStartupExpanded: false,
                        toolbarCanCollapse: true,
                        toolbar: 'full',
                        resize_enabled: true,
                        extraPlugins: '',
                        contentsCss: ['app/css/styles-ar.css', 'app/css/bootstrap-rtl.css'],
                        allowedContent: true,
                        extraAllowedContent: '*(*);*{*}'
                    }; 
                }

                // enable ckeditorAr
                var ckeditorAr = element.ckeditor(editorOptions);
                // update ngModel on change
                ckeditorAr.editor.on('change', function () {
                    ngModel.$setViewValue(this.getData());
                });
            }
        };
    }
})();