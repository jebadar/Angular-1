formApp.controller('tfController', ["$scope", "DataService", "$routeParams", "$mdDialog", "$location", "imageAdd", "regNo", "tutorID", "imageCheck", "FileUploader", "Lightbox",
    function tfController($scope, DataService, $routeParams, $mdDialog, $location, imageAdd, regNo, tutorID, imageCheck, FileUploader, Lightbox) {
        $scope.qualification = [];
        $scope.experience = [];
        $scope.file = [];
        $scope.images = [];
        $scope.imagesTutor = [];
        $scope.Id = tutorID.getId();
        $scope.profile_image_add = 0;//check for profile image
        $scope.updation_image_add = 0;// check for Updation form image
        $scope.tutorRegNo = regNo.getNo();
        $scope.gender = [
        "MALE",
        "FEMALE"
            ];
        if ($routeParams.id != "null") {
            DataService.getTutor($routeParams.id).then(
                     function (result) {
                         //on success 
                         $scope.tutors = result.data.i_tutor;
                         $scope.profile_image_add = '10' + $scope.tutors.tutor_id;
                         $scope.updation_image_add = '11' + $scope.tutors.tutor_id;
                         $scope.qualification = result.data.i_tutor_qualification;
                         $scope.experience = result.data.i_tutor_experience;
                         $scope.tutorId = result.data.i_tutor.tutor_id;
                         if ($scope.tutors.image_profile != '../../')
                         {
                             $scope.imagesTutor.push($scope.tutors.image_profile);
                         }
                         if ($scope.tutors.image_updation_form != '../../') {
                             $scope.imagesTutor.push($scope.tutors.image_updation_form);
                         }
                         for (var counter = 0; counter < $scope.qualification.length; counter++) {

                             if ($scope.qualification[counter].image_degree != null || $scope.qualification[counter].image_degree != undefined)
                             {
                                 $scope.images.push($scope.qualification[counter].image_degree)
                             }
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
                    imageAdd.setAdd('r',-1);
                    $scope.tutors.t_status = 'unverified';
                    $scope.tutors.image_profile = '../../';
                    $scope.tutors.image_updation_form = '../../';
            }
                insertTutor($scope.tutors);
            }
            else if ($scope.tutors.tutor_id != 0 && $scope.tutors.tutor_id > 0) {

                if (imageAdd.getAdd(0) != null || imageAdd.getAdd(0) != '') {
                    var address = imageAdd.getAdd(100);
                    var counter = address.length;
                    for(var i = 0;i < counter;i++)
                    {
                        if (address[i][i].id == 'p')
                        {
                            $scope.tutors.image_profile = address[i][i].value;
                        }
                        else if (address[i][i].id == 'u')
                        {
                            $scope.tutors.image_updation_form = address[i][i].value;
                        }
                    }
                    imageAdd.setAdd('r',-1);
                }
                else if (imageAdd.getAdd(0) == null || imageAdd.getAdd(0) == '') {
                    $scope.tutors.image_profile = '../../';
                    $scope.tutors.image_updation_form = '../../';
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
            var address = imageAdd.getAdd(100);
            var length = address.length;
            if (length != 0)
            { 
                for (var counter = 0; counter < $scope.qualification.length; counter++) {
                    if (address[counter][0].id != 'd' && $scope.qualification[counter] != null)
                    {
                        $scope.qualification[counter].image_degree = '../../';
                    }
                    else if ($scope.qualification[counter] != null) {
                            if (address[counter][0].id == 'd') {
                            $scope.qualification[counter].image_degree = address[counter][0].value;
                        }
                    }
                }
                insertQualification($scope.qualification);
            }
            imageAdd.setAdd('r', -1);
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

        $scope.choices = [{ id: 0 }];
        var check = 0;
        $scope.addNewChoice = function () {
            check++;
            $scope.choices.push({ 'id': check });
        }
        $scope.removeChoice = function () {
            
            var lastItem = $scope.choices.length;
            $scope.qualification.splice(lastItem);
            $scope.experience.splice(lastItem);
            lastItem = $scope.choices.length - 1;
            $scope.choices.splice(lastItem);
           
        };
        $scope.addNewQual = function (id) {
            imageAdd.setAdd('r',-1);
            tutorID.setId(id);
            $location.path("/newQualForm/null");
        };
        $scope.editExpe = function (id) {
            $location.path("/expeForm/" + id);
        };

        $scope.editQual = function (id) {
            imageAdd.setAdd('r',-1);
            $location.path("/qualForm/" + id);
        };
        $scope.addNewExper = function (id) {
            tutorID.setId(id);
            $location.path("/newExperForm/null");
        };



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
            if (imageCheck.getType('p')) {
                fileItem.file.name = '*profile*' + fileItem.file.name;
            }
           else if (imageCheck.getType('u')) {
               fileItem.file.name = '*updation*' + fileItem.file.name;
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

            if (imageCheck.getType('p')) {
                imageAdd.setAdd('p',fileItem);
            }
            else if (imageCheck.getType('u')) {
                imageAdd.setAdd('u', fileItem);
            }
            else if(imageCheck.getType('d')){
                imageAdd.setAdd('d', fileItem);
            }
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
        $scope.openLightboxModalTutor = function (index) {

            Lightbox.openModal($scope.imagesTutor, index);
        };
        $scope.profile_image = function () {
            imageCheck.setType('p',true);
        }

        $scope.updation_image = function () {
            imageCheck.setType('u', true);
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
        $scope.addNewTutor = function (id) {
            $location.path("/newTutorForm" + id);
        };
        $scope.checkStart = function ()
        {
            var aChar = 'R';
            var query = $scope.tutorRegNo + "-" + aChar;
            DataService.checkTutor(query)
            .then(function (result) {
                if (!angular.isDefined(result.data.length) || result.data.length === 0)
                {

                   regNo.setNo($scope.tutorRegNo);
                    $scope.addNewTutor(null);
                }
                else if (angular.isDefined(result.data.length) || result.data.length > 0)
                {
                    $mdDialog.show(
                    $mdDialog.alert()
                    .parent(angular.element(document.querySelector('#popupContainer')))
                    .clickOutsideToClose(true)
                    .title('Tutor Already Exist')
                    .ariaLabel('Alert Dialog Demo')
                    .ok('Got it!')
                    )
                }
                
            },
            function (result) {
                alert(result.alert);
            });
        }
        $scope.isDisabled = false;

        $scope.disableButton = function () {
            $scope.isDisabled = true;
        }

    }])