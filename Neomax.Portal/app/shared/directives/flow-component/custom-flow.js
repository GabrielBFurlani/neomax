(function () {
    "use strict";

    angular.module('app').directive('customFlow', function () {
        return {
            restrict: 'E',
            scope: {
                listItems: '='
            },
            templateUrl: '/app/shared/directives/flow-component/custom-flow.html',
            link: function (scope, element, attrs) {
                var className = '';
                var previousHasDate = false;
                var previousIsActive = false;

                for (var i = 0; i < scope.listItems.length; i++) {
                    if (scope.listItems[i].modifiedDate) {
                        className = 'visited';
                        previousHasDate = true;
                        previousIsActive = false;
                    }
                    else {
                        if (previousHasDate) {
                            className = 'active';
                            previousHasDate = false;
                            previousIsActive = true;
                        }
                        else {
                            if (previousIsActive) {
                                className = 'next';
                                previousHasDate = false;
                                previousIsActive = false;
                            }
                        }
                    }

                    scope.listItems[i].class = className;
                }
            }
        };
    });
}());