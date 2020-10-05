app.controller("LovList", ["$scope", "LovListFactory", "$timeout", "$location", "$window", function ($scope, LovListFactory, $timeout, $location, $window) {


    //$scope.save = function () {
    //    debugger;
    //    LovFactory.save($scope.model).then(function (success) {
    //        alert(success.data);
    //        $window.location.reload();
    //    });
    //};


    LovListFactory.init(
        function (success) {
            $scope.lov_category = success[0].data;
        });
}]);

app.factory("LovListFactory", ["$rootScope", "$http", "$q", "CommonFactory", function ($rootScope, $http, $q, CommonFactory) {
    this.init = function (success, failure) {
        var id = 0;
        $q.all([
           this.getLovGrid()
        ]).then(function (msg) {
            success(msg);
        }, failure);
    }
    this.getLovGrid = function () {
        return $http.post("/Lov/GetLovGrid");
    }
    return this;
}]);


app.controller("Lov", ["$scope", "LovFactory", "$timeout", "$location", "$window", function ($scope, LovFactory, $timeout, $location, $window) {


    $scope.save = function () {
        debugger;
        LovFactory.save($scope.model).then(function (success) {
            alert(success.data);
            window.location.href = window.location.origin + '/Lov';
        });
    };


    LovFactory.init(
        function (success) {
            debugger;
            $scope.model = success[0].data;
            $scope.lov_categories=success[1].data;
        });
}]);


app.factory("LovFactory", ["$rootScope", "$http", "$q", "CommonFactory", function ($rootScope, $http, $q, CommonFactory) {
    this.init = function (success, failure) {
        var id = 0;
        var urls = window.location.href.split('?')[0].split('/');
        if (!isNaN(urls[urls.length - 1]))
            id = eval(urls[urls.length - 1]);
        $q.all([
           this.getItem(id),
           CommonFactory.getLovCategories()
        ]).then(function (msg) {
            success(msg);
        }, failure);
    }
    this.getItem = function (id) {
        return $http.post(("/Lov/GetItem"), { id: id })
    },
    this.save = function (model) {
        return $http.post(("/Lov/save"), { model: model });
    }
    return this;
}]);