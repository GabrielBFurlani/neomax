(function () {
    "use strict";

    angular.module('app').controller('userDelete', ['$scope', '$http', '$uibModalInstance', 'idUser', function ($scope, $http, $uibModalInstance, idUser) {

        $scope.cancel = function () {
            $uibModalInstance.dismiss();
        }

        $scope.ok = function () {
            $uibModalInstance.close(idUser)
        }

    }]);
}());