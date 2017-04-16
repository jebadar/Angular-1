formApp.controller('efController', ["$scope", "DataService", "$routeParams", "$mdDialog", "$location",
    function efController($scope, DataService, $routeParams, $mdDialog, $location) {
        $scope.experience;
        $scope.tutorId;
        if ($routeParams.id != "null") {
            DataService.getExpe($routeParams.id).then(
                     function (result) {
                         //on success 
                        $scope.experience = result.data;
                    }, (function (result) {
                         //on error
                         $mdDialog.show(
                         $mdDialog.alert()
                         .parent(angular.element(document.querySelector('#popupContainer')))
                         .clickOutsideToClose(true)
                         .title('Data Get Session not completed')
                         .ariaLabel('Alert Dialog Demo')
                         .ok('Got it!')
                     );
                     })
                     );
        }
        else {

            $scope.tutors = { tutorId: 0 };
        }

        $scope.submitExpe = function () {
            $scope.$broadcast('has-error');
            if ($scope.experience.e_id != 0 && $scope.experience.e_id > 0) {
                DataService.putExpe($scope.experience).then(
                    function (result) {
                        $scope.editableExpe = angular.copy($scope.experience);
                        $mdDialog.show(
                        $mdDialog.alert()
                        .parent(angular.element(document.querySelector('#popupContainer')))
                        .clickOutsideToClose(true)
                        .title('Data Updated Successfully')
                        .ariaLabel('Alert Dialog Demo')
                        .ok('Got it!')
                    );
                        history.go(-1);
                    },
                    function (result) {
                        $mdDialog.show(
                      $mdDialog.alert()
                        .parent(angular.element(document.querySelector('#popupContainer')))
                        .clickOutsideToClose(true)
                        .title('Error in updating record')
                        .ariaLabel('Alert Dialog Demo')
                        .ok('Got it!')
                    );
                    });
            }
        };
        $scope.cancelForm = function () {
            //$uibModalInstance.dismiss('cancel');
            history.go(-1);
        };

        $scope.resetForm = function () {
            $scope.$broadcast('hide-errors-event');
        };
        
    }]);