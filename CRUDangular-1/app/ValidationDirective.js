formApp.directive('showErrors', ["$timeout", function ($timeout) {

    return {
        restrict: 'A',
        require: '^form',
        link: function (scope, el, attrs, formCtrl) {
            //find the text box element, which has the 'name' attribute
            var inputEl = el[0].querySelector("[name]");

            //convert the native text box element to an angular element 
            var inputNgEl = angular.element(inputEl);

            //get the name on the text box so we know the property to check 
            //on form controller
            var inputName = inputNgEl.attr('name');

            //var helpText = angular.element(el[0].querySelector(".help-block"));

            //only apply the has-error class after the user leaves the text box
            inputNgEl.bind('blur', function () {
                el.toggleClass('has-error', formCtrl[inputName].$invalid);
                // helpText.toggleClass('hide', formCtrl[inputName].$valid);

            });
            scope.$on('show-errors-event', function() {
                el.toggleClass('has-error',formCtrl[inputName].$invalid);
            })

            scope.$on('hide-errors-event', function () {
                $timeout(function () {
                    el.removeClass('has-error');
                    }, 0, false);

            });

        }
    }
}]);

formApp.directive('capitalize', function() {
    return {
        require: 'ngModel',
        link: function(scope, element, attrs, modelCtrl) {
            var capitalize = function(inputValue) {
                if (inputValue == undefined) inputValue = '';
                var capitalized = inputValue.toUpperCase();
                if (capitalized !== inputValue) {
                    modelCtrl.$setViewValue(capitalized);
                    modelCtrl.$render();
                }
                return capitalized;
            }
            modelCtrl.$parsers.push(capitalize);
            capitalize(scope[attrs.ngModel]); // capitalize initial value
        }
    };
});

formApp.directive('rotate', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            scope.$watch(attrs.degrees, function (rotateDegrees) {
                console.log(rotateDegrees);
                var r = 'rotate(' + rotateDegrees + 'deg)';
                element.css({
                    '-moz-transform': r,
                    '-webkit-transform': r,
                    '-o-transform': r,
                    '-ms-transform': r
                });
            });
        }
    }
});
formApp.directive('exportTable', [function () {
    //export html table to pdf, excel and doc format directive
        
        return {
            restrict: 'C',
            link: function ($scope, elm, attr) {
            $scope.$on('export-pdf', function (e, d) {
                elm.tableExport({
                    type: 'pdf', jspdf: {
                        orientation: 'l',
                        format: 'a3',
                        margins: { left: 10, right: 10, top: 20, bottom: 20 },
                        autotable: {
                            styles: {
                                fillColor: 'inherit',
                                textColor: 'inherit'
                            },
                            tableWidth: 'auto',
                             margin: { top: 80 },
                            beforePageContent: function (data) {
                                var doc = data.doc; // Internal jspdf instance
                                doc.setFontSize(1);
                                doc.setTextColor(40);
                                doc.setFontStyle('normal');
                                doc.text(" ", data.settings.margin.left, 60);
                            }
                        }
                    }
                });
            });
            $scope.$on('export-excel', function (e, d) {
                elm.tableExport({ type: 'excel', escape: false });
            });
            $scope.$on('export-doc', function (e, d) {
                elm.tableExport({ type: 'doc', escape: false });
            });
        }
    }

}]);