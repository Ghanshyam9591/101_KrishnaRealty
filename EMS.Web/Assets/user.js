app.controller("UserList", ["$scope", "UserListFactory", "$timeout", "$location", "$window", function ($scope, UserListFactory, $timeout, $location, $window) {


    //$scope.save = function () {
    //    debugger;
    //    RoleFactory.save($scope.model).then(function (success) {
    //        alert(success.data);
    //        $window.location.reload();
    //    });
    //};


    UserListFactory.init(
        function (success) {
            debugger;
            $scope.usergridData = success[0].data;
        });
}]);

app.factory("UserListFactory", ["$rootScope", "$http", "$q", "CommonFactory", function ($rootScope, $http, $q, CommonFactory) {
    this.init = function (success, failure) {
        var id = 0;
        $q.all([
           this.getRoleGrid()
        ]).then(function (msg) {
            success(msg);
        }, failure);
    }
    this.getRoleGrid = function () {
        return $http.post("/User/GetRoleGrid");
    }
    return this;
}]);


app.controller("User", ["$scope", "UserFactory", "$timeout", "$location", "$window", function ($scope, UserFactory, $timeout, $location, $window) {


    $scope.save = function (model) {
        UserFactory.save($scope.model).then(function (success) {
            alert(success.data);
            window.location.href = window.location.origin + '/User';
            //$window.location.reload();
        });
    };


    UserFactory.init(
        function (success) {
            debugger;
            $scope.model = success[0].data;
            $scope.getroles = success[1].data;
        });
}]);
app.factory("UserFactory", ["$rootScope", "$http", "$q", "CommonFactory", function ($rootScope, $http, $q, CommonFactory) {
    this.init = function (success, failure) {
        var id = 0;
        var urls = window.location.href.split('?')[0].split('/');
        if (!isNaN(urls[urls.length - 1]))
            id = eval(urls[urls.length - 1]);

        $q.all([
           this.getItem(id),
           CommonFactory.getRoles()
        ]).then(function (msg) {
            success(msg);
        }, failure);
    }
    this.getItem = function (id) {
        return $http.post(("/User/GetItem"), { id: id })
    },
    this.save = function (model) {
        return $http.post(("/User/save"), { model: model });
    }
    this.getGridDetail = function (AsOnDate, ClaimType) {
        return $http.get('/User/GetGridDetail/');
    }
    this.getClaimType = function () {
        return $http.get('/User/GetDetails/');
    }
    return this;
}]);