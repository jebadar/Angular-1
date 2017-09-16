formApp.controller('afController', ["$scope", "DataService", "$routeParams", "$mdDialog", "$location", "$http", "FileUploader", "$interval","course_code","tutorID","searchResultArray",
    function afController($scope, DataService, $routeParams, $mdDialog, $location, $http, FileUploader, $interval, course_code, tutorID, searchResultArray) {
        $scope.ID = tutorID.getId();
        $scope.code = course_code.getCode();
        if ($scope.code != null) {
            DataService.specStudents($scope.code).then(
                function (result) {
                    $scope.students = result.data;
                },
                function (result) {
                    $mdDialog.show(
                        $mdDialog.alert()
                            .parent(angular.element(document.querySelector('#popupContainer')))
                            .clickOutsideToClose(true)
                            .title("Unable to Load Students Try Again!")
                            .ariaLabel('Alert Dialog Demo')
                            .ok('Got it!')
                        );
                    $location.path("/matricMain");
                }
            )
        }
        else {
            $mdDialog.show(
                          $mdDialog.alert()
                              .parent(angular.element(document.querySelector('#popupContainer')))
                              .clickOutsideToClose(true)
                              .title("Please Select Course Code! Try Again!")
                              .ariaLabel('Alert Dialog Demo')
                              .ok('Got it!')
                          );
            $location.path("/matricMain");
        }
        $scope.selectedStudents = [];
    
        $scope.selectedCheckbox = function (value) {
            if ($scope.selectedStudents.indexOf(value) !== -1) {

                for (var i = $scope.selectedStudents.length - 1; i >= 0; i--) {
                    if ($scope.selectedStudents[i] == value) {
                        $scope.selectedStudents.splice(i, 1);
                    }
                }
            }
            else
                $scope.selectedStudents.push(value);
        }

        $scope.pushStudents = function () {
            var length = $scope.selectedStudents.length;
            $scope.appoint = [{
                tutor_id: "",
                course_code_id: "",
                student_appoint_id: ""
            }];
            
            for (var counter = 0; counter < $scope.selectedStudents.length; counter++) {
                
                $scope.appoint.push({
                    tutor_id: tutorID.getId(),
                    course_code_id: course_code.getCode(),
                    student_appoint_id: $scope.selectedStudents[counter]
                })
                
            }
            
            DataService.stdAppoint($scope.appoint).then(
                function (result) {
                    $mdDialog.show(
                        $mdDialog.alert()
                            .parent(angular.element(document.querySelector('#popupContainer')))
                            .clickOutsideToClose(true)
                            .title('Students Appointed Successfully!' + result.data +'!')
                            .ariaLabel('Alert Dialog Demo')
                            .ok('Got it!')
                        );
                    $location.path("/secTutor");
                },
                function (result) {
                    $mdDialog.show(
                        $mdDialog.alert()
                            .parent(angular.element(document.querySelector('#popupContainer')))
                            .clickOutsideToClose(true)
                            .title('Error in Appointment of Students Retry!')
                            .ariaLabel('Alert Dialog Demo')
                            .ok('Got it!')
                        );
                    $location.path("/load");
                }
                )

            //In controller
            $scope.exportAction = function(){ 
                switch($scope.export_action){ 
                    case 'pdf': $scope.$broadcast('export-pdf', {}); 
                        break; 
                    case 'excel': $scope.$broadcast('export-excel', {}); 
                        break; 
                    case 'doc': $scope.$broadcast('export-doc', {});
                        break; 
                    default: console.log('no event caught'); 
                }
            }
        }

    }])