formApp.controller('efController', ["$scope", "DataService", "$routeParams", "$mdDialog",
    function efController($scope, DataService,$routeParams,$mdDialog) {

        if ($routeParams.id != "null") {
        //$scope.employee = DataService.employee;
           DataService.getEmployee($routeParams.id).then(
                    function (result) {
                        //on success 
                        $scope.employee = result.data;
                       
                    },(function (result) {
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
        else
        {
          
           $scope.employee = { id: 0 };
        }

       // if ($routeParams.id)
       // {
        //    var promiseVariable = DataService.fetchEmployee($routeParams.id)
       // }
        //$scope.lastName = "Ahmed";
        $scope.editableEmployee = angular.copy($scope.employee);

        $scope.department = [
            "Engineering",
            "Marketing",
            "Administration",
            "Finance"

        ];

        $scope.designation = [
            "Director",
            "Board Member",
            "Account officer",
            "Progress manager",
            "Administrative Manager",
            "Project Coordinator",
            "Field Worker"
        ];

        $scope.submitForm = function () {
            $scope.$broadcast('has-error');

            if ($scope.employee.id == "null" || $scope.employee.id == 0 ) {
                DataService.insertEmployee($scope.employee).then(
                    function (result) {
                        //on success
                        $scope.editableEmployee = angular.copy($scope.employee);
                        $mdDialog.show(
                      $mdDialog.alert()
                        .parent(angular.element(document.querySelector('#popupContainer')))
                        .clickOutsideToClose(true)
                        .title('Record Entered Successfully!')
                        .ariaLabel('Alert Dialog Demo')
                        .ok('Got it!')
                    );
                        history.go(-1);
                    },
                    function (result) {
                        //on error
                        $mdDialog.show(
                      $mdDialog.alert()
                        .parent(angular.element(document.querySelector('#popupContainer')))
                        .clickOutsideToClose(true)
                        .title('Error in Entering new record!')
                        .ariaLabel('Alert Dialog Demo')
                        .ok('Got it!')
                    );
                        
                    });
            }

            else if ($scope.employee.employee_id != 0 && $scope.employee.employee_id > 0)
            {
                DataService.putEmployee($scope.employee).then(
                    function (result) {
                        $scope.editableEmployee = angular.copy($scope.employee);
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

      

        $scope.isAge = function(n) {
            if (n > 0 && n <= 100 && n % 1 === 0) {
                return true;
            }
            else
                return false;
        }
    }]);