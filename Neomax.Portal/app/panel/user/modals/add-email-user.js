(function () {
    "use strict";

    angular.module('app').controller('addEmailUserController', ['$scope', '$http', 'CONFIG', '$uibModalInstance', function ($scope, $http, CONFIG, $uibModalInstance) {
        var _apiUrl = CONFIG.apiRootUrl;

        $scope.newEmail = null;

        //Load Page
        function loadPage() {

        }
        
        //Button: Sign-Up
        $scope.send = function () {
            
            $http.post(_apiUrl + '/user/releaseEmail/'+ $scope.newEmail + '/')
                .then(function successCallback(response) {

                    //Closes the modal and return the login's credentials to auto log-in
                    $uibModalInstance.close($scope.signUpUser);
                })
        }

        //Button: Cancel
        $scope.cancel = function (close) {
            $uibModalInstance.dismiss();
        }

        loadPage();

    }]);
}());