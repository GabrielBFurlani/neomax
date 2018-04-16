(function () {
    "use strict";

    angular.module('app').controller('userDelete', ['$scope', '$http', '$uibModalInstance', function ($scope, $http, $uibModalInstance) {

        //Button: OK
        $scope.ok = function () {
            $uibModalInstance.close()
        }

        //Button: Cancel
        $scope.cancel = function () {
            $uibModalInstance.dismiss();
        }
        
    }]);
}());