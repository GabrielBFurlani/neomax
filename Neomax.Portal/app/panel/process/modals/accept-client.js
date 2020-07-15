(function () {
    "use strict";

    angular.module('app').controller('defineProcessController', ['$scope', '$http', 'CONFIG', '$uibModalInstance', function ($scope, $http, CONFIG, $uibModalInstance) {
        var _apiUrl = CONFIG.apiRootUrl;

        $scope.signUpUser = {
            username: '',
            password: '',
            name: '',
            nickname: '',
            cpf: ''
        };

        $scope.openSuggestion = false;

        $scope.reEnterPassword = '';

        //Load Page
        function loadPage() {

        }
        
        //Button: Sign-Up
        $scope.signUp = function () {
            /*
            $http.post(_apiUrl + '/user/signUp/', $scope.signUpUser)
                .then(function successCallback(response) {

                    //Closes the modal and return the login's credentials to auto log-in
                    $uibModalInstance.close($scope.signUpUser);
                })*/
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