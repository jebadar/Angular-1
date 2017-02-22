var formApp = angular.module('formApp', ["ngRoute", "ngDialog", "ngMaterial", "ui.bootstrap", "ui.select", "angularUtils.directives.dirPagination", "ngTable", "angularFileUpload", "bootstrapLightbox"]);
formApp.config(["$routeProvider", "$locationProvider", "LightboxProvider", function ($routeProvider, $locationProvider, LightboxProvider) {
    $routeProvider
    .when("/home", {
        templateUrl: "app/TutorList.html",
        controller:"HomeController"
    })
        .when("/adminMain", {
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
            return imageAdd[num];
        },
        setAdd: function (value) {
            if (value == -1) {
                imageAdd = [];
                counter = 0;
            }
            else {
                imageAdd[counter] = value;
                counter++;
            }
        }
    };
});

formApp.controller("HomeController", ["$scope", "$location", "DataService", "$http", "ngDialog", "$mdDialog", "NgTableParams", function ($scope, $location, DataService, $http, ngDialog, $mdDialog, NgTableParams) {

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

    $http.get("api/TutorWebApi").then(
        //on success
        function (result) {
            $scope.tutors = result.data;

        },
        function (result) {
            alert(result.alert);
        });


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

    $scope.addNewTutor = function (id) {
        $location.path("/newTutorForm" + id);
    };

    $scope.rdListView = function () {
        $location.path("/RdTList");
    }
    var deleteEmployeeId;
    $scope.showConfirm = function (ev,id) {
        var confirm = $mdDialog.confirm()
             .title('Would you like to delete your debt?')
             //.content('All of the banks have agreed to forgive you your debts.')
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
                $location.path("/adminMain");
            }
        },
        //on error
        function (result) { }

        )


}]);