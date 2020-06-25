(function () {
    "use strict";

    angular.module("app").directive('tileButton', function () {
        return {
            restrict: 'E',
            scope: {
                icon: '@icon',
                description: '@description'
            },
            replace: true,
            template:
                '<div style="height: 150px; width: 150px; text-align: center; color: #ffffff; background-color: #106b28; cursor: pointer; border-radius: 20px;">' +
                '   <div style="font-size: 76px; max-height: 100px;">' +
                '       <i class="{{icon}}"></i>' +
                '   </div>' +
                '   <div style="height: 50px; background-color: rgba(0, 0, 0, 0.60); border-bottom-left-radius: 20px; border-bottom-right-radius: 20px;">' +
                '       <div style="padding-top: 5px;">{{description}}</div>' +
                '   </div>' +
                '</div>'
        }
    });
}());