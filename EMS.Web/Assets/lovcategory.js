app.controller("LovCategoryList", ["$scope", "LovCategoryListFactory", "$timeout", "$location", "$window", function ($scope, LovCategoryListFactory, $timeout, $location, $window) {


    //$scope.save = function () {
    //    debugger;
    //    LovCategoryFactory.save($scope.model).then(function (success) {
    //        alert(success.data);
    //        $window.location.reload();
    //    });
    //};


    LovCategoryListFactory.init(
        function (success) {
            $scope.lov_category = success[0].data;
        });
}]);

app.factory("LovCategoryListFactory", ["$rootScope", "$http", "$q", "CommonFactory", function ($rootScope, $http, $q, CommonFactory) {
    this.init = function (success, failure) {
        var id = 0;
        $q.all([
           this.getLovCategoryGrid()
        ]).then(function (msg) {
            success(msg);
        }, failure);
    }
    this.getLovCategoryGrid = function () {
        return $http.post("/LovCategory/GetLoveCategoryGrid");
    }
    return this;
}]);


app.controller("LovCategory", ["$scope", "LovCategoryFactory", "$timeout", "$location", "$window", function ($scope, LovCategoryFactory, $timeout, $location, $window) {


    $scope.save = function () {
        debugger;
        LovCategoryFactory.save($scope.model).then(function (success) {
            alert(success.data);
            window.location.href = window.location.origin + '/LovCategory';
        });
    };


    LovCategoryFactory.init(
        function (success) {
            $scope.model = success[0].data;
        });
}]);


app.factory("LovCategoryFactory", ["$rootScope", "$http", "$q", "CommonFactory", function ($rootScope, $http, $q, CommonFactory) {
    this.init = function (success, failure) {
        var id = 0;
        var urls = window.location.href.split('?')[0].split('/');
        if (!isNaN(urls[urls.length - 1]))
            id = eval(urls[urls.length - 1]);
        $q.all([
           this.getItem(id)
        ]).then(function (msg) {
            success(msg);
        }, failure);
    }
    this.getItem = function (id) {
        return $http.post(("/LovCategory/GetItem"), { id: id })
    },
    this.save = function (model) {
        return $http.post(("/LovCategory/save"), { model: model });
    }
    return this;
}]);