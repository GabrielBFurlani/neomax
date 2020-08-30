(function () {
    "use strict";

    angular.module('app').controller('defineProcessController', ['$scope', '$http', 'CONFIG', '$uibModalInstance', 'title', 'id', function ($scope, $http, CONFIG, $uibModalInstance, title, id) {
        var _apiUrl = CONFIG.apiRootUrl;

        $scope.title = title;

        $scope.id = id;

        $scope.openSuggestion = false;

        $scope.suggestion = '';

        //Load Page
        function loadPage() {

        }

        //Button: Sign-Up
        $scope.updateStatus = function (status) {

            $scope.statusUpdateObject = {
                status: status,
                suggestion: $scope.suggestion
            }

            $http.put(_apiUrl + '/solicitations/' + id + '/updateProductStatus', $scope.statusUpdateObject)
                .then(function successCallback(response) {
                    $uibModalInstance.close();
                })
        }

        //Button: Cancel
        $scope.cancel = function (close) {

            if ($scope.openSuggestion && !close)
                $scope.openSuggestion = false;
            else
                $uibModalInstance.dismiss();
        }

        loadPage();

    }]);
}());