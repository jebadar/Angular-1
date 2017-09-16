formApp.controller('mfController', ["$scope", "DataService", "$routeParams", "$mdDialog", "$location", "itemId_service", "userCheck", "userInfo", "$http", "searchResultArray", "FileUploader", "$interval", "tutorID", "course_code",
    function mfController($scope, DataService, $routeParams, $mdDialog, $location, itemId_service, userCheck, userInfo, $http, searchResultArray, FileUploader, $interval, tutorID, course_code) {
        var totalItemGetId = "10101";
        var totalTutorGet = "Total";
        
        $scope.searchTutors = searchResultArray.getArray();
        $scope.user = userInfo.getInfo();
        $scope.semester = $scope.user.semester_current;
        
        $http.get("/api/itemWebApi/" + totalItemGetId).then(
                function (result) {
                    $scope.totalItems = result.data.item_amount;
                }, function (result) {
                    alert(result.alert);
                }
        )
        $http.get("/api/SectionTutorWebApi/" + totalTutorGet).then(
        function (result) { 
            $scope.totalTutors = result.data;
        }, function (result) {
            alert(result.alert); 
            }
        )
        DataService.sectionTutorGet().then(
            function (result) {
                $scope.secTutor = result.data;
            },
            function (result) {
                alert(result.alert);
            }
        )
        DataService.courseCode().then(
            function (result) {
                $scope.courseCodes = result.data;
            },
            function (result) {
                alert(result.alert);
            }
        )
        $scope.mainItem = function () {
            $location.path("/itemMain");
        }
        $scope.listView = function () {
            $location.path("/secTutor");
        }
        $scope.DetailTutor = function (id) {
            $location.path("/DetailTutor/" + id);
        }
        $scope.updateTutor = function (id) {
            $location.path("/EditTutor/" + id);
        }
        $scope.option = function (id) {
            $location.path("/option");
        }
        $scope.ListTutor = function (id) {
            $location.path("/tutorAdd");
        }
        $scope.searchView = function () {
            $location.path("/mfSearch");
        }
        $scope.toggleObjSelection = function ($event, description) {
            $event.stopPropagation();
        }

        $scope.search = "";
        $scope.searchKeyword = "";
        $scope.searchField = function (value) {
            var aChar;
            if ($scope.searchKeyword == "Registration No") {
                aChar = 'R';
            }
            else if ($scope.searchKeyword == "Name") {
                aChar = 'N';
            }
            else if ($scope.searchKeyword == "Address") {
                aChar = 'A';
            }
            else if ($scope.searchKeyword == "Verified") {
                aChar = 'V';
            }
            else if ($scope.searchKeyword == "Unverified") {
                aChar = 'U';
            }
            var query = value + "-" + aChar;
            $http.get('api/TotalRecordWebApi/' + query)
            .then(function (result) {
                searchResultArray.setArray(result.data);
                $location.path("/tutorAdd");
            },
            function (result) {
                alert(result.alert);
            });
        }

        $scope.keyword = [
        "Registration No",//for registration no
        "Name",//for name 
        "Address",//for Address
        "Verified",//for Verified
        "Unverified"//for Unverified
        ];

        $scope.fullTutorList = function () {
            $mdDialog.show(
                $mdDialog.alert()
                    .parent(angular.element(document.querySelector('#popupContainer')))
                    .clickOutsideToClose(true)
                    .title('It will take some time to load All record from Database. Be Patient!')
                    .ariaLabel('Alert Dialog Demo')
                    .ok('Got it!')
                );
         }
        
        $scope.selectedTutors = [];
        $scope.selectedCheckbox = function (value) {
            if ($scope.selectedTutors.indexOf(value) !== -1) {

                for (var i = $scope.selectedTutors.length - 1; i >= 0; i--) {
                    if ($scope.selectedTutors[i] == value) {
                        $scope.selectedTutors.splice(i, 1);
                    }
                }
            }
            else
            $scope.selectedTutors.push(value);
        }

        $scope.isDisabled = false;

        $scope.disableButton = function () {
            $scope.isDisabled = true;
        }

        $scope.importView = function () {
            $location.path("/mfImport");
        }
        $scope.studentView = function () {
            $location.path("/mfStudentList");
        }
        $scope.startBar = function (ev) {
            $mdDialog.show({
                controller:mfController,
                templateUrl: 'app/sectionForm/matricForm/loading.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: false,
                fullscreen: $scope.customFullscreen
                         
            });
        }
        var self = this;
        self.activated = true;
        self.determinateValue = 30;

        // Iterate every 100ms, non-stop and increment
        // the Determinate loader.
        $interval(function() {

            self.determinateValue += 1;
            if (self.determinateValue > 100) {
                self.determinateValue = 30;
            }

        }, 100);
        
        $scope.postTutorList = function () {
            DataService.sectionTutorPost($scope.selectedTutors).then(
                    function (result) { //on success
                        $mdDialog.show(
                        $mdDialog.alert()
                            .parent(angular.element(document.querySelector('#popupContainer')))
                            .clickOutsideToClose(true)
                            .title('Successfully Added!')
                            .ariaLabel('Alert Dialog Demo')
                            .ok('Got it!')
                        );
                        $location.path("/secTutor");
                    },
                    function (result) { //on error
                        $mdDialog.show(
                        $mdDialog.alert()
                            .parent(angular.element(document.querySelector('#popupContainer')))
                            .clickOutsideToClose(true)
                            .title(result.data)
                            .ariaLabel('Alert Dialog Demo')
                            .ok('Got it!')
                        );
                        $location.path("/secTutor");
                    }
                )
        }


        $scope.semester = [
        "Spring",
        "Autumn"
        ];

        $scope.year = [
        "2017",
        "2018",
        "2019",
        "2020",
        "2021",
        "2022",
        "2023",
        "2024",
        "2025",
        "2026",
        "2027",
        "2028","2027","2029","2030","2031","2032","2033","2034","2035"
        ];
        $scope.clickSemesterNew = function () {
            $location.path("/semesterNew");
        }
        $scope.startSemester = function (value) {
            DataService.newSemester(value).then(
                function (result) {
                    $mdDialog.show(
                        $mdDialog.alert()
                            .parent(angular.element(document.querySelector('#popupContainer')))
                            .clickOutsideToClose(true)
                            .title('Successfully Started!')
                            .ariaLabel('Alert Dialog Demo')
                            .ok('Got it!')
                        );
                    $location.path("/secTutor");
                }, function (result) {
                    $mdDialog.show(
                        $mdDialog.alert()
                            .parent(angular.element(document.querySelector('#popupContainer')))
                            .clickOutsideToClose(true)
                            .title('Error in Starting!')
                            .ariaLabel('Alert Dialog Demo')
                            .ok('Got it!')
                        );
                    $location.path("/secTutor");
                }
                )
        }
        
        $scope.student_sheet = 0;
        $scope.tutor_reg_sheet = 0;

        $scope.checkStudentSheet = function () {
            $scope.student_sheet = 1;
        }
        $scope.checkTutorSheet = function () {
            $scope.tutor_reg_sheet = 1;
        }
        //$scope.search = "";
        $scope.tutorImport = function () {
            $location.path("/secTutorImport");
        }

        $scope.courseCodeView = function (value) {
            tutorID.setId(value);
            $location.path("/courseCodeSelect");
        }

        $scope.studentList = function (value) {
            course_code.setCode(value.id);
            $location.path("/specStudentList");
        }
        
        $scope.stdAppointView = function () {

        }
        $scope.stdAppointList = function (id) {
            tutorID.setId(id);
            DataService.stdAppointGet(id).then(
                function (result) {
                    searchResultArray.setArray(result.data);
                    $location.path("/appStudentList");
                },
                function (result) {
                    $mdDialog.show(
                        $mdDialog.alert()
                            .parent(angular.element(document.querySelector('#popupContainer')))
                            .clickOutsideToClose(true)
                            .title('Error in Fetching Record of Students!')
                            .ariaLabel('Alert Dialog Demo')
                            .ok('Got it!')
                        );
                })
        }

        var uploader = $scope.uploader = new FileUploader({
            url: '/api/ImportWebApi/Post'
        });
        // FILTERS
        // a sync filter
        uploader.filters.push({
            name: 'syncFilter',
            fn: function (item /*{File|FileLikeObject}*/, options) {
                console.log('syncFilter');
                return this.queue.length < 10;
            }
        });
        // an async filter
        uploader.filters.push({
            name: 'asyncFilter',
            fn: function (item /*{File|FileLikeObject}*/, options, deferred) {
                console.log('asyncFilter');
                setTimeout(deferred.resolve, 1e3);
            }
        });
        // CALLBACKS
        uploader.onWhenAddingFileFailed = function (item /*{File|FileLikeObject}*/, filter, options) {
            console.info('onWhenAddingFileFailed', item, filter, options);
        };
        uploader.onAfterAddingFile = function (fileItem) {
            if ($scope.student_sheet == 1) {
                fileItem.file.name = '*studentList*' + fileItem.file.name;
                $scope.student_sheet = 0;
            }
            else if ($scope.tutor_reg_sheet == 1) {
                fileItem.file.name = '*sectionTutorList*' + fileItem.file.name;
                $scope.tutor_reg_sheet = 0;
            }
            console.info('onAfterAddingFile', fileItem);
        };
        uploader.onAfterAddingAll = function (addedFileItems) {
            console.info('onAfterAddingAll', addedFileItems);
        };
        uploader.onBeforeUploadItem = function (item) {
            
            console.info('onBeforeUploadItem', item);
        };
        uploader.onProgressItem = function (fileItem, progress) {
            console.info('onProgressItem', fileItem, progress);
        };
        uploader.onProgressAll = function (progress) {
            console.info('onProgressAll', progress);
        };
        uploader.onSuccessItem = function (data, fileItem, response, status, headers) {
            $mdDialog.show({
                controller:'DialogController',
                templateUrl: 'app/sectionForm/matricForm/response.html',
                parent: angular.element(document.body),
                clickOutsideToClose: true,
                fullscreen: $scope.customFullscreen,
                locals:{items: fileItem.data}
        })
            console.info('onSuccessItem', fileItem, response, status, headers);
        };
        uploader.onErrorItem = function (fileItem, response, status, headers) {
            console.info('onErrorItem', fileItem, response, status, headers);
        };
        uploader.onCancelItem = function (fileItem, response, status, headers) {
            console.info('onCancelItem', fileItem, response, status, headers);
        };
        uploader.onCompleteItem = function (fileItem, response, status, headers) {
            console.info('onCompleteItem', fileItem, response, status, headers);
        };
        uploader.onCompleteAll = function () {
            console.info('onCompleteAll');
        };
        console.info('uploader', uploader);

    }])
formApp.controller('DialogController', function ($scope, items) {
    // items is injected in the controller, not its scope!
    $scope.items = items;
});