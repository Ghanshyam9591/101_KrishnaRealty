app.controller("assignEnquiryList", ["$scope", "AssignEnquiryListFactory", "$timeout", "$location", "$window", function ($scope, AssignEnquiryListFactory, $timeout, $location, $window) {
    debugger;
    AssignEnquiryListFactory.init(
        function (success) {
            debugger;
            $scope.enquiry_data = success[0].data;
            $scope.users = success[1].data;
        });

    $scope.save = function (model) {
        debugger;
        AssignEnquiryListFactory.save(model).then(function (success) {
            alert(success.data);
        });
    }
}]);

app.factory("AssignEnquiryListFactory", ["$rootScope", "$http", "$q", "CommonFactory", function ($rootScope, $http, $q, CommonFactory) {
    this.init = function (success, failure) {
        var id = 0;
        $q.all([
           this.ListData(),
           CommonFactory.getUsers()
        ]).then(function (msg) {
            success(msg);
        }, failure);
    }
    this.getItem = function (id) {
        return $http.post(("/AssignEnquiry/GetItem"), { id: id })
    },
    this.ListData = function () {
        return $http.post("/AssignEnquiry/ListData");
    },
    this.save = function (model) {
        return $http.post(('/AssignEnquiry/Save/'), { model: model });
    }
    return this;
}]);
