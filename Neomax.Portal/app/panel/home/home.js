(function () {

    angular.module("app").controller('homeController', ['$scope', '$state', '$http', function ($scope, $state, $http) {

        console.log($scope.session);
        $scope.isAdmin = $scope.session.isAdmin;

        $scope.sendNextPage = function (pageNumber) {
            if (pageNumber == 1) {
                if ($scope.isAdmin)
                    $state.go('panel.user.userArea.edit');
                else
                    $state.go('panel.myAccount');
            }
            else {
                if (pageNumber == 2) {
                    $state.go('panel.process.client.list');
                }
                else
                    if (pageNumber == 3) {
                        $state.go('panel.client.list');
                    }
                    else
                        if (pageNumber == 4) {
                            $state.go('panel.user.management.list');
                        }
                        else
                            if (pageNumber == 5) {
                                $state.go('panel.process.admin.list');
                            }
            }
        }

    }]);
}());