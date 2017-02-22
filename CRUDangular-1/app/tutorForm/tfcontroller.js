formApp.controller('tfController', ["$scope", "DataService", "$routeParams", "$mdDialog", "$location", "imageAdd", "tutorID", "FileUploader", "Lightbox",
    function tfController($scope, DataService, $routeParams, $mdDialog, $location, imageAdd, tutorID, FileUploader, Lightbox) {
        $scope.qualification = [];
        $scope.experience = [];
        $scope.file = [];
        $scope.images = [];
        $scope.profile_image_check = 0;
        $scope.Id = tutorID.getId();
        $scope.profile_image_add = 0;
        if ($routeParams.id != "null") {
            DataService.getTutor($routeParams.id).then(
                     function (result) {
                         //on success 
                         $scope.tutors = result.data.i_tutor;
                         $scope.profile_image_add = '101010' + $scope.tutors.tutor_id;
                         $scope.qualification = result.data.i_tutor_qualification;
                         $scope.experience = result.data.i_tutor_experience;
                         $scope.tutorId = result.data.i_tutor.tutor_id;
                         for (var counter = 0; counter <= $scope.qualification.length; counter++) {
                             $scope.images.push($scope.qualification[counter].image_degree)

                         }
                     },
                         (function (result) {
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
            $scope.tutors = { tutor_id: 0 };
        }

        insertTutor = function (tutor) {
            DataService.insertTutor(tutor).then(
                    function (result) {
                        //on success
                        $scope.editableTutor = angular.copy($scope.tutors);
                        $mdDialog.show(
                        $mdDialog.alert()
                        .parent(angular.element(document.querySelector('#popupContainer')))
                        .clickOutsideToClose(true)
                        .title('Record Entered Successfully!')
                        .ariaLabel('Alert Dialog Demo')
                        .ok('Got it!')
                    );
                        $location.path("/newQualForm" + tutor.tutor_id);
                       // history.go(-1);
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
        insertQualification = function (qualification) {
            DataService.insertQualification(qualification).then(
                    function (result) {
                        //on success
                        $scope.editableQualification = angular.copy($scope.qualification);
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

        insertExperience = function (experience) {
            DataService.insertExperience(experience).then(
                    function (result) {
                        //on success
                        $scope.editableExperience = angular.copy($scope.experience);
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
        $scope.submitForm = function () {
            $scope.$broadcast('has-error');
            if ($scope.tutors.tutor_id == "null" || $scope.tutors.tutor_id == 0) {
                if (imageAdd.getAdd(0) != null || imageAdd.getAdd(0) != '') {
                    $scope.tutors.image_profile = imageAdd.getAdd(0);
                    imageAdd.setAdd(-1);
            }
                insertTutor($scope.tutors);
            }
            else if ($scope.tutors.tutor_id != 0 && $scope.tutors.tutor_id > 0) {

                if (imageAdd.getAdd(0) != null || imageAdd.getAdd(0) != '') {
                    $scope.tutors.image_profile = imageAdd.getAdd(0);
                    imageAdd.setAdd(-1);
                }
                DataService.putTutor($scope.tutors).then(
                    function (result) {
                        $scope.editableTutor = angular.copy($scope.tutors);
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
        $scope.submitQual = function () {
            $scope.$broadcast('has-error');
            for (var counter = 1; counter < $scope.qualification.length; counter++) {
                if (imageAdd.getAdd(counter - 1) == null && $scope.qualification[counter] != null)
                {
                    $scope.qualification[counter].image_degree = '../../';
                }
                else
                $scope.qualification[counter].image_degree = imageAdd.getAdd(counter - 1);
            }
            insertQualification($scope.qualification);
        };

        $scope.submitExper = function () {
            insertExperience($scope.experience);
        }

        $scope.cancelForm = function () {
            //$uibModalInstance.dismiss('cancel');
            history.go(-1);
        };

        $scope.resetForm = function () {
            $scope.$broadcast('hide-errors-event');
        };

        $scope.choices = [{ id: 1 }];
        $scope.addNewChoice = function () {
            if ($scope.choices.length == 0) {
                $scope.choices.push({ 'id': $scope.choices.length });
            }
            
            $scope.choices.push({ 'id':  $scope.choices.length + 1});
        };
        $scope.removeChoice = function () {
            
            var lastItem = $scope.choices.length;
            $scope.qualification.splice(lastItem);
            $scope.experience.splice(lastItem);
            lastItem = $scope.choices.length - 1;
            $scope.choices.splice(lastItem);
           
        };
        $scope.addNewQual = function (id) {
            imageAdd.setAdd(-1);
            tutorID.setId(id);
            $location.path("/newQualForm/null");
        };
        $scope.editExpe = function (id) {
            $location.path("/expeForm/" + id);
        };

        $scope.editQual = function (id) {
            imageAdd.setAdd(-1);
            $location.path("/qualForm/" + id);
        };
        $scope.addNewExper = function (id) {
            tutorID.setId(id);
            $location.path("/newExperForm/null");
        };

        $scope.gender = [
            "MALE",
            "FEMALE"
        ];

        var uploader = $scope.uploader = new FileUploader({
            url: '/api/Upload/PostUserImage'
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
            console.info('onAfterAddingFile', fileItem);
        };
        uploader.onAfterAddingAll = function (addedFileItems) {
            console.info('onAfterAddingAll', addedFileItems);
        };
        uploader.onBeforeUploadItem = function (item) {
            if ($scope.profile_image_check == 1) {
                item.file.name = '*profile*' + item.file.name;
            }
            console.info('onBeforeUploadItem', item);
        };
        uploader.onProgressItem = function (fileItem, progress) {
            console.info('onProgressItem', fileItem, progress);
        };
        uploader.onProgressAll = function (progress) {
            console.info('onProgressAll', progress);
        };
        uploader.onSuccessItem = function (data, fileItem, response, status, headers) {
            imageAdd.setAdd(fileItem);
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
        $scope.openLightboxModal = function (index) {
            Lightbox.openModal($scope.images, index);
        };
        $scope.profile_image = function () {
            $scope.profile_image_check = 1;
        }
        $scope.showConfirm = function (ev, add) {
            var confirm = $mdDialog.confirm()
                 .title('Would you like to Remove Image')
                 //.content('All of the banks have agreed to forgive you your debts.')
                 .ariaLabel('Lucky day')
                 .targetEvent(ev)
                 .ok('Please do it!')
                 .cancel('Cancel');
            tutor_img_add = add;
            $mdDialog.show(confirm).then(function () {
                DataService.removeImage(tutor_img_add).then(function (result) {
                    //on success
                    $mdDialog.show(
                      $mdDialog.alert()
                        .parent(angular.element(document.querySelector('#popupContainer')))
                        .clickOutsideToClose(true)
                        .title('Image Deleted Successfully!')
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
                    .title('Image Not Deleted Successfully!')
                    .ariaLabel('Alert Dialog Demo')
                    .ok('Got it!')
                );
           })

            },
            function () {
                $mdDialog.hide(confirm);
            });
        };

    }])