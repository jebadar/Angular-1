formApp.factory('DataService',["$http",
    function ($http) {
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
        var checkTutor = function (query) {
            return $http.get('/api/TotalRecordWebApi/' + query);
        };
        var getItems = function () {
            return $http.get('/api/ItemWebApi');

        };
        var getItemsById = function (id) {
            return $http.get('/api/ItemWebApi/' + id);
        };
        var postItem = function (value) {
            return $http.post("/api/ItemWebApi/Post", value);
        };
        var putRoom = function (value) {
            return $http.put("/api/ItemWebApi/Put", value);
        };
        var roleGet = function(){
            return $http.get("api/RoleWebApi");
        }
        var sectionTutorGet = function () {
            return $http.get("api/SectionTutorWebApi");
        }
        var sectionTutorPost = function (value) {
            return $http.post("/api/SectionTutorWebApi/Post", value);
        }
        var newSemester = function (value)
        {
            return $http.put("/api/NewSemesterWebApi/Put", value);
        }
        var courseCode = function () {
            return $http.get("api/CourseCodeWebApi");
        }
        var roleGet = function () {
          return  $http.get("api/RoleWebApi");
        }
        var specStudents = function (id) {
            return $http.get("/api/StudentWebApi/" + id);
        }
        var stdAppoint = function (value)
        {
            return $http.post("/api/StudentWebApi/Post", value);
        }
        var stdAppointGet = function (id) {
            return $http.get("/api/StudentAppointWebApi/" + id);
        }
        return {
            getTutor: getTutor,
            insertTutor: insertTutor,
            putTutor: putTutor,
            insertQualification: insertQualification,
            insertExperience: insertExperience,
            putQual: putQual,
            getQual: getQual,
            getExpe: getExpe,
            putExpe: putExpe,
            removeImage: removeImage,
            checkTutor: checkTutor,
            getItems: getItems,
            getItemsById: getItemsById,
            postItem:postItem,
            putRoom: putRoom,
            roleGet: roleGet,
            sectionTutorGet: sectionTutorGet,
            sectionTutorPost: sectionTutorPost,
            newSemester:newSemester,
            courseCode: courseCode,
            roleGet: roleGet,
            specStudents: specStudents,
            stdAppoint: stdAppoint,
            stdAppointGet: stdAppointGet

        };
    }]);
 