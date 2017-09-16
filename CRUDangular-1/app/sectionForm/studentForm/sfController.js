formApp.controller('sfController', ["$scope", "DataService", "$routeParams", "$mdDialog", "$location", "$http", "FileUploader", "$interval","course_code","tutorID","searchResultArray",
    function sfController($scope, DataService, $routeParams, $mdDialog, $location, $http, FileUploader, $interval, course_code, tutorID, searchResultArray) {
         DataService.getTutor(tutorID.getId()).then(
            function (result) {
                $scope.tutor = result.data.i_tutor;
            },
            function (result) {
                $mdDialog.show(
                        $mdDialog.alert()
                            .parent(angular.element(document.querySelector('#popupContainer')))
                            .clickOutsideToClose(true)
                            .title("Unable to Load Tutor Record Try Again!")
                            .ariaLabel('Alert Dialog Demo')
                            .ok('Got it!')
                        );
                $location.path("/load");
            }
            )
         $scope.print = function () {
             $location.path("/printStudents");
         }
        $scope.appStudents = searchResultArray.getArray();
        $scope.export_action = 'pdf';
        //In controller
        $scope.exportAction = function () {
            switch ($scope.export_action) {
                case 'pdf': $scope.$broadcast('export-pdf', {});
                    break;
                case 'excel': $scope.$broadcast('export-excel', {});
                    break;
                case 'doc': $scope.$broadcast('export-doc', {});
                    break;
                default: console.log('no event caught');
            }
        }
    }])