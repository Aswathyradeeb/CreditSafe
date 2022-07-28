(function () {
    'use strict';

    angular
        .module('eventsapp')
        .directive('ckeditor', Directive);

    function Directive($rootScope) {
        return {
            require: 'ngModel',
            link: function (scope, element, attr, ngModel) {
                var editorOptions;
                if (attr.ckeditor === 'minimum') {
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
                        contentsCss: ['app/css/styles.css', 'app/css/bootstrap.css'],
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
                        contentsCss: ['app/css/styles.css', 'app/css/bootstrap.css'],
                        allowedContent: true,
                        extraAllowedContent: '*(*);*{*}'
                    };
                }

                // enable ckeditor
                var ckeditor = element.ckeditor(editorOptions);
                // update ngModel on change
                ckeditor.editor.on('change', function () {
                    ngModel.$setViewValue(this.getData());
                });
            }
        };
    }
})();