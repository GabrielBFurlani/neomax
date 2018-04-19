(function () {
    "use strict";

    angular.module('app').controller('signUpController', ['$scope', '$http', 'CONFIG', '$uibModalInstance', function ($scope, $http, CONFIG, $uibModalInstance) {
        var _apiUrl = CONFIG.apiRootUrl;

        $scope.signUpUser = {
            username: '',
            password: '',
            name: '',
            nickname: '',
            cpf: ''
        };

        $scope.reEnterPassword = '';

        //Load Page
        function loadPage() {

        }
        
        //Button: Sign-Up
        $scope.signUp = function () {

            $http.post(_apiUrl + '/user/signUp/', $scope.signUpUser)
                .then(function successCallback(response) {
                    var httpResultModel = response.data;

                    if (httpResultModel.operationSuccess) {

                        //Closes the modal and return the login's credentials to auto log-in
                        $uibModalInstance.close($scope.signUpUser);
                    }
                })
        }

        //Button: Cancel
        $scope.cancel = function () {
            $uibModalInstance.dismiss();
        }

        loadPage();

    }]);
}());