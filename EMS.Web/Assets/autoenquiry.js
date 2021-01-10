app.controller("AutoEnquiry", ["$scope", "AutoEnquiryFactory", "$timeout", "$location", "$window", function ($scope, AutoEnquiryFactory, $timeout, $location, $window) {

    $scope.view_email = function () {
        $window.open('https://www.google.com', '_blank');
    }

    setInterval(function () {
        AutoEnquiryFactory.init(
        function (success) {
            $scope.auto_enquiries = success[0].data;
        });
    }, 120000)

    AutoEnquiryFactory.init(
        function (success) {
            $scope.auto_enquiries = success[0].data;
        });
}]);

app.factory("AutoEnquiryFactory", ["$rootScope", "$http", "$q", "CommonFactory", function ($rootScope, $http, $q, CommonFactory) {
    this.init = function (success, failure) {
        var id = 0;
        $q.all([
           this.getAutoEnquiryGrid()
        ]).then(function (msg) {
            success(msg);
        }, failure);
    }
    this.getAutoEnquiryGrid = function () {
        return $http.post("/AutoEnquiry/GetAutoEnquiryGrid");
    }
    return this;
}]);
