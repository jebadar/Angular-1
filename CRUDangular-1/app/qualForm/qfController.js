formApp.controller('qfController', ["$scope", "DataService", "$routeParams", "$mdDialog", "imageAdd", "$location", "FileUploader", "Lightbox",
    function qfController($scope, DataService, $routeParams, $mdDialog, imageAdd, $location, FileUploader, Lightbox) {
        $scope.qualification;
        $scope.tutorId;
        $scope.images = [];
        if ($routeParams.id != "null") {
            DataService.getQual($routeParams.id).then(
                     function (result) {
                         //on success 
                         $scope.qualification = result.data;
                         $scope.images = [
                             {
                                 'url': $scope.qualification.image_degree
                             }
                         ]
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

        $scope.submitQual = function () {
            $scope.$broadcast('has-error');
            if ($scope.qualification.q_id != 0 && $scope.qualification.q_id > 0) {
                $scope.qualification.image_degree = imageAdd.getAdd(0);
                DataService.putQual($scope.qualification).then(
                    function (result) {
                        $scope.editableQual = angular.copy($scope.qualification);
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

        $scope.choices = [{ id: 0 }];
        $scope.addNewChoice = function () {
            if ($scope.choices.length == 0) {
                $scope.choices.push({ 'id': $scope.choices.length });
            }

            $scope.choices.push({ 'id': $scope.choices.length + 1 });
        };
        $scope.removeChoice = function () {

            var lastItem = $scope.choices.length - 1;
            $scope.choices.splice(lastItem);
        };
        $scope.addNewQual = function (id) {
            $location.path("/newQualForm/" + id);
        };
        $scope.showConfirm = function (ev, id) {
            var confirm = $mdDialog.confirm()
                 .title('Would you like to Remove Image')
                 //.content('All of the banks have agreed to forgive you your debts.')
                 .ariaLabel('Lucky day')
                 .targetEvent(ev)
                 .ok('Please do it!')
                 .cancel('Cancel');
            qual_id = id;
            $mdDialog.show(confirm).then(function () {
                DataService.removeImage(qual_id).then(function (result) {
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

        $scope.file = new Image();

        var uploader = $scope.uploader = new FileUploader({

            url: '/api/Upload/PostUserImage',
            success: function (result) {
                $scope.$scope.qualification.image_degree = result.data;
            }
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
            console.info('onBeforeUploadItem', item);
            };
        uploader.onProgressItem = function (fileItem, progress) {
            console.info('onProgressItem', fileItem, progress);
            };
        uploader.onProgressAll = function (progress) {
            console.info('onProgressAll', progress);
            };
        uploader.onSuccessItem = function (imgData,fileItem, response, status, headers) {
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
            Lightbox.openModal($scope.images,index);
        };

    }]);