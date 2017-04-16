var formApp = angular.module('formApp', ["ngRoute", "ngDialog", "ngMaterial", "ui.bootstrap", "ui.select", "angularUtils.directives.dirPagination", "ngTable", "angularFileUpload", "bootstrapLightbox"]);
formApp.config(["$routeProvider", "$locationProvider", "LightboxProvider", function ($routeProvider, $locationProvider, LightboxProvider) {
    $routeProvider
    .when("/home", {
        templateUrl: "app/TutorList.html",
        controller:"HomeController"
    })
        .when("/#adminMain", {
            templateUrl: "app/adminView.html",
            controller: "HomeController"
        })
        .when("/load", {
            templateUrl: "/app/loadPage.html",
            controller: "MainController"
        })
        .when("/RdMain", {
            templateUrl: "/app/DirectorView.html",
            controller: "HomeController"
        })
        .when("/RdTList", {
            templateUrl: "/app/DirectorListView.html",
            controller: "HomeController"
        })
        .when("/EditTutor/:id", {
            templateUrl: "/app/tutorForm/tfTemplate.html",
            controller: "tfController"
        })
        .when("/DetailTutor/:id", {
            templateUrl: "/app/tutorForm/tfDetail.html",
            controller: "tfController"
        })
        .when("/tutorcheck/:id", {
            templateUrl: "/app/tutorForm/tutorCheck.html",
            controller: "tfController"
        })
        .when("/newTutorForm:id", {
            templateUrl: "/app/tutorForm/tfNew.html",
            controller: "tfController"
        })
        
        .when("/newQualForm/:id", {
            templateUrl: "/app/qualForm/tfNewQual.html",
            controller: "tfController"
        })
        .when("/expeForm/:id", {
            templateUrl: "/app/expeForm/efExpe.html",
            controller: "efController"
        })
        .when("/qualForm/:id", {
            templateUrl: "/app/qualForm/tfQual.html",
            controller: "qfController"
        })
        .when("/newExperForm/:id", {
            templateUrl: "/app/tutorForm/tfExper.html",
            controller: "tfController"
        })
        .when("/importView", {
            templateUrl: "app/importSheetView.html",
            controller: "HomeController"
        })
        .when("/searchResult", {
            templateUrl: "app/searchList.html",
            controller: "HomeController"
        })
        .when("/searchResultAdmin", {
            templateUrl: "app/searchListAdmin.html",
            controller: "HomeController"
        })
    .otherwise({
        redirectTo:"/load"
    });

    $locationProvider.html5Mode({
        enabled: true,
        requireBase: false
    });

    LightboxProvider.fullScreenMode = true;

}]);

formApp.service("tutorID", function TutorID() {
    var tutorId = this;

    return {
        getId: function () {
            return tutorId;
        },
        setId: function (value) {
            tutorId = value;
        }
    };
});

formApp.service("imageAdd", function imageAdd() {
    var imageAdd = [];
    var counter = 0;
    return {
        getAdd: function (num) {
            if (num == 100)
            { return imageAdd; }
            else
            return imageAdd[num];
        },
        setAdd: function (type,value) {
            if (value == -1) {
                imageAdd = [];
                counter = 0;
            }
            else {
                imageAdd[counter] = [{
                    id: type,
                    value: value
                }];
                
                counter++;
            }
        }
    };
});
formApp.service("regNo", function regNo() {
    var regNo;
    return {
        getNo: function () {
            return regNo;
        },
        setNo: function (value) {
            regNo = value;
        }
    }
})
formApp.service("searchResultArray", function () {
    var arrayResult = [];
    return {
        getArray: function () {
            return arrayResult;
        },
        setArray: function (value) {
            arrayResult = value;
        }
    }
})

formApp.controller("HomeController", ["$scope", "$location", "DataService", "$http", "ngDialog", "$mdDialog", "NgTableParams", "FileUploader", "searchResultArray", function ($scope, $location, DataService, $http, ngDialog, $mdDialog, NgTableParams, FileUploader, searchResultArray) {

    $scope.verify = [{
        active: true
    }];
    $scope.tutors = [];
    $scope.qualification = [];

    $http.get("api/TotalRecordWebApi").then(
        //on success
        function (result) {
            $scope.totalTutors = result.data;

        },
        function (result) {
            alert(result.alert);
        });

    /*$http.get("api/TutorWebApi").then(
        //on success
        function (result) {
            $scope.tutors = result.data;

        },
        function (result) {
            alert(result.alert);
        });*/
    $scope.$watch('currentPage + numPerPage', function () {
        var begin = (($scope.currentPage - 1) * $scope.numPerPage)
        , end = begin + $scope.numPerPage;
        $scope.filteredTutors = $scope.tutors.slice(begin, end);
    });
    $scope.updateTutor = function (id) {
        $location.path("/EditTutor/" + id);
    }
    $scope.adminTutor = function () {
        $location.path("/home");
    }
    $scope.DetailTutor = function (id) {
        $location.path("/DetailTutor/" + id);
    }
    $scope.tutorCheck = function (id)
    {
        $location.path("/tutorcheck/" + id);
    }
    $scope.importView = function () {
        $location.path("/importView");
    }
    $scope.downloadSheet = function () {
        $http.get('/api/ImportWebApi', { responseType: 'arraybuffer' })
          .success(function (data) {
              var file = new Blob([data], { type: 'application/pdf' });
              saveAs(file, 'Tutor Sheet Sample.xlsx');
          });
    }

    $scope.rdListView = function () {
        $location.path("/RdTList");
    }
    var deleteEmployeeId;
    $scope.showConfirm = function (ev,id) {
        var confirm = $mdDialog.confirm()
             .title('Would you like to delete your debt?')
             .ariaLabel('Lucky day')
             .targetEvent(ev)
             .ok('Please do it!')
             .cancel('Cancel');
        deleteEmployeeId = id;
        $mdDialog.show(confirm).then(function () {
             DataService.deleteEmployee(deleteEmployeeId).then(function (result) {
                //on success
                    $mdDialog.show(
                      $mdDialog.alert()
                        .parent(angular.element(document.querySelector('#popupContainer')))
                        .clickOutsideToClose(true)
                        .title('Record Deleted Successfully!')
                        .ariaLabel('Alert Dialog Demo')
                        .ok('Got it!')
                    );
                    window.location.reload();
            },
        function (result) {
            //on error
            $mdDialog.show(
                       $mdDialog.alert()
                         .parent(angular.element(document.querySelector('#popupContainer')))
                         .clickOutsideToClose(true)
                         .title('Record Not Deleted Successfully!')
                         .ariaLabel('Alert Dialog Demo')
                         .ok('Got it!')
                     );
            history.go(-1);
        })
    },
        function () {
            $mdDialog.hide(confirm);
        });
    };
    $scope.sort = function (keyname) {
        $scope.sortKey = keyname;   //set the sortKey to the param passed
        $scope.reverse = !$scope.reverse; //if true make it false and vice versa
    }
     $scope.cancelForm = function () {
        history.go(-1);
    };
    var self = this;
    var data = $scope.tutors;
    self.tableParams = new NgTableParams({}, { dataset: data });
    $scope.totalTutors = 0;
    $scope.tutorsPerPage = 10; // this should match however many results your API puts on one page
    getResultsPage(1);
    $scope.pagination = {
        current: 1
    };
    $scope.pageChanged = function (newPage) {
        getResultsPage(newPage);
    };
    function getResultsPage(pageNumber) {
        // this is just an example, in reality this stuff should be in a service
        $http.get('api/TutorWebApi?page=' + pageNumber)
            .then(function (result) {
                $scope.tutors = result.data;
                $scope.totTutors = result.data.Count
            });
    }
    $scope.search = "";
    $scope.searchKeyword = "";
    var isAdmin = 0;
    $scope.searchTutors = searchResultArray.getArray();
    $scope.admin = function () {
         isAdmin = 1;
    }
    $scope.searchField = function (value) {
        var aChar;
        if ($scope.searchKeyword == "Registration No")
        {
            aChar = 'R';
        }
        else if ($scope.searchKeyword == "Name")
        {
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
        var query = value +"-"+ aChar;
        $http.get('api/TotalRecordWebApi/' + query )
        .then(function (result) {
            searchResultArray.setArray(result.data);
            if (isAdmin == 1) {
                $location.path("/searchResultAdmin");
            }
            else 
                $location.path("/searchResult");
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
        console.info('onAfterAddingFile', fileItem);
    };
    uploader.onAfterAddingAll = function (addedFileItems) {
        console.info('onAfterAddingAll', addedFileItems);
    };
    uploader.onBeforeUploadItem = function (item) {
        if ($scope.profile_image_check == 1) {
            item.file.name = '*profile*' + item.file.name;
        }
        else if ($scope.updation_image_check == 1) {
            item.file.name = '*updation*' + item.file.name;
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

}]);

formApp.controller("MainController", ["$scope", "$location", "DataService", "$http", "ngDialog", "$mdDialog", "NgTableParams", function ($scope, $location, DataService, $http, ngDialog, $mdDialog, NgTableParams) {
    $http.get("api/RoleWebApi").then(
        //on success
        function (result) {
            $scope.username = result.data;
            if ($scope.username == 'director') {
                $location.path("/RdMain");
            }
            else if ($scope.username == 'admin')
            {
                $location.path("/#adminMain");
            }
        },
        //on error
        function (result) { }
            
        )
}]);