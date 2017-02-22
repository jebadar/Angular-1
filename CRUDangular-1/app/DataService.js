formApp.factory('DataService',["$http",
    function ($http) {

        var getEmployee = function (id) {
            return $http.get('/api/EmployeeWebApi/' + id);
        };
        
        var insertEmployee = function(newEmployee) {
            return $http.post("/api/EmployeeWebApi/Post",newEmployee);
        };

        var updateEmployee = function (employee) {
            return true;
        }

        var fetchEmployee = function (id) {
           return ;
        };

        var putEmployee = function (employee) {
            return $http.put("/api/EmployeeWebApi/Put", employee);
        }
        var deleteEmployee = function (id) {
            return $http.delete("/api/EmployeeWebApi/Delete/"+ id);
        }

        var getTutor = function (id) {
            return $http.get("/api/TutorWebApi/" + id);
        }

        var getQual = function (id) {
            return $http.get("/api/QualificationWebApi/" + id);
        }
        var getExpe = function (id) {
            return $http.get("/api/ExperienceWebApi/" + id);
        }
        var putTutor = function (tutor) {
            return $http.put("/api/TutorWebApi/Put", tutor);
        }

        var putQual = function (qual) {
            return $http.put("/api/QualificationWebApi/Put", qual);
        }
        var putExpe = function (expe) {
            return $http.put("/api/ExperienceWebApi/Put", expe);
        }

        var insertTutor = function (newTutor) {
            return $http.post("/api/TutorWebApi/Post", newTutor);
        };
        var insertQualification = function (newQual) {
            return $http.post("/api/QualificationWebApi/Post", newQual);
        };

        var insertExperience = function (newExper) {
            return $http.post("/api/ExperienceWebApi/Post", newExper);
        };
        var removeImage = function (img_add) {
            return $http.delete("/api/Upload/Delete/" + img_add);
        };
        return {
            getEmployee: getEmployee,
            updateEmployee: updateEmployee,
            insertEmployee: insertEmployee,
            putEmployee: putEmployee,
            deleteEmployee: deleteEmployee,
            getTutor: getTutor,
            insertTutor: insertTutor,
            putTutor: putTutor,
            insertQualification: insertQualification,
            insertExperience: insertExperience,
            putQual: putQual,
            getQual: getQual,
            getExpe: getExpe,
            putExpe: putExpe,
            removeImage: removeImage
            
            
        };
    }]);
 