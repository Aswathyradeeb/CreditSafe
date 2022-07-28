/**=========================================================
 * Module: TooltipConfig.js
 =========================================================*/

(function() {
    'use strict';

    angular
        .module('eventsapp')
        .config(tooltipConfig);
    /* @ngInject */
    function tooltipConfig($tooltipProvider) {

      $tooltipProvider.options({
        appendToBody: true
      });

    }
    tooltipConfig.$inject = ['$tooltipProvider'];

})();
