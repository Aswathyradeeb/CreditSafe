/**=========================================================
 * Module: ColorsService.js
 * Services to retrieve global colors
 =========================================================*/
 
(function() {
    'use strict';

    angular
        .module('eventsapp')
        .factory('colors', colors);
    
    colors.$inject = ['COLORS'];
    function colors(COLORS) {

      return {
        byName: function(name) {
          return (COLORS[name] || '#fff');
        }
      };
    }

})();