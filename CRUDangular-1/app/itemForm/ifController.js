formApp.controller('ifController', ["$scope", "DataService", "$routeParams", "$mdDialog", "$location", "itemId_service", "userCheck",
    function ifController($scope, DataService, $routeParams, $mdDialog, $location, itemId_service, userCheck) {
       // $scope.item = [];
        DataService.roleGet().then(
                //on success
            function (result) {
                $scope.username = result.data;
                if ($scope.username == 'director') {
                    userCheck.setUser('director', true);
                }
                else if ($scope.username == 'admin') {
                    userCheck.setUser('admin', true);
                }
            },
            //on error
            function (result) {

            }

            )

        $scope.singleRoom = [];
        $scope.item_ID = itemId_service.getId();
        $scope.keyword = [
                        "1","2","3","4","5","6","7","9","10","11","12","13","14","15","16"
        ];
        $scope.quantity = [
                        "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17"
                        , "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38",
                        "39","40","41","42","43","44","45","46","47","48","49","50"
        ];

        $scope.item_names = [
                        "Officer/Executive Table",
                        "Computer Table",
                        "Assistant Table",
                        "Centre Table",
                        "Officer Chair(Revolving)",
                        "Side Rack Wooden",
                        "File Cabinet(Steel)",
                        "File Cabinet(Wooden)",
                        "Curtain/Blind",
                        "Almareh(Steel)",
                        "Almareh(Wooden)",
                        "Computer System",
                        "Printer",
                        "Telephone Set",
                        "Scanner",
                        "Fax Machine",
                        "Heater Gas",
                        "Heater Electric",
                        "Photo Copy Machine",
                        "Air Room Cooler",
                        "Wall Clock",
                        "Air Conditioner",
                        "Chair with Arms",
                        "Wooden Stand",
                        "Chair without Arms",
                        "Dinning Table",
                        "Copy Printer RN2050",
                        "File Tray Wooden",
                        "Over Head Projector",
                        "Notice Board",
                        "Table Steel Green",
                        "Sofa Set 5 Seater",
                        "Stabilizer",
                        "Stool Wooden",
                        "DVD",
                        "Headphone",
                        "Carpet",
                        "Water Purification",
                        "Safe Cabinet",
                        "Stand for Room Air Cooler",
                        "Mouse box",
                        "TV + LED",
                        "White Board",
                        "TV Wall Bracket",
                        "Multimedia",
                        "Coat Hanger",
                        "Table/Photo Copy Machine",
                        "Steel Planter",
                        "Vehicle",
                        "Water Cooler Machine",
                        "Water Cooler Plastic",
                        "Foot Rest Wooden",
                        "Air Blower",
                        "Refrigerator",
                        "Microwave Oven",
                        "Bracket Fan",
                        "Franking Fan",
                        "Sofa Set 3 Seater",
                        "Sofa Set 1 Seater",
                        "Rostrum",
                        "Web Cam",
                        "Steel File Rack"

        ];
        if ($routeParams.id == "null" || $routeParams.id == undefined) {
            DataService.getItems().then(
                function (result) { // on success
                    $scope.items = result.data.item;
                    $scope.rooms = result.data.room;
                },
                function (result) // on error 
                {
                    $mdDialog.show(
                       $mdDialog.alert()
                         .parent(angular.element(document.querySelector('#popupContainer')))
                         .clickOutsideToClose(true)
                         .title('Error in fetching record!' + result.data)
                         .ariaLabel('Alert Dialog Demo')
                         .ok('Got it!')
                     );
                }

                )
        }
        else if ($routeParams.id != "null") {
            DataService.getItemsById($routeParams.id).then(

                function (result) {
                    $scope.singleRoom = result.data;

                },
                function (result) {
                    $mdDialog.show(
                           $mdDialog.alert()
                             .parent(angular.element(document.querySelector('#popupContainer')))
                             .clickOutsideToClose(true)
                             .title('Error in fetching record! Error:' + result.data)
                             .ariaLabel('Alert Dialog Demo')
                             .ok('Got it!')
                         );
            })
        }

        $scope.submitRoom = function () {
            DataService.putRoom($scope.singleRoom).then(
                function (result) {
                    $mdDialog.show(
                           $mdDialog.alert()
                             .parent(angular.element(document.querySelector('#popupContainer')))
                             .clickOutsideToClose(true)
                             .title('Record updated successfully')
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
                                 .title('Error in fetching record! Error:' + result.data)
                                 .ariaLabel('Alert Dialog Demo')
                                 .ok('Got it!')
                             );
                })
        }

        $scope.room_selected = 0;
        $scope.dropdownChange = function (value)
        {
            if (value == 1 || value > 1) {
                $scope.room_selected = value;
                
            }
            else
                $scope.room_selected = 0;

        }

        $scope.submitItem = function () {
            DataService.postItem($scope.item).then(
                function (result) {

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
                    $mdDialog.show(
                      $mdDialog.alert()
                      .parent(angular.element(document.querySelector('#popupContainer')))
                      .clickOutsideToClose(true)
                      .title('Error in Entering Record')
                      .ariaLabel('Alert Dialog Demo')
                      .ok('Got it!')
                  );
                }
                )

        }

        $scope.newItem = function (id)
        {
            $location.path("/newItem" + id);
        }
        $scope.isDisabled = false;

        $scope.disableButton = function () {
            $scope.isDisabled = true;
        }

        $scope.item_click = function (value) {
            itemId_service.setId(value);
        }
        $scope.detailItem = function () {


            if (userCheck.getUser("admin")) {
                $location.path("/detailItem");
            }
            else
                $location.path("/rdDetailItem");
        }
        $scope.edit_room = function (id) {
            $location.path('/EditRoomItem/' + id);
        }
        
    }])