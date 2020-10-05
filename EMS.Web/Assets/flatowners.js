app.controller("FlatOwnersList", ["$scope", "FlatOwnersListFactory", "$timeout", "$location", "$window", function ($scope, FlatOwnersListFactory, $timeout, $location, $window) {
    FlatOwnersListFactory.init(
        function (success) {
            debugger;
            $scope.enquiry_data = success[0].data;
        });
}]);

app.factory("FlatOwnersListFactory", ["$rootScope", "$http", "$q", "CommonFactory", function ($rootScope, $http, $q, CommonFactory) {
    this.init = function (success, failure) {
        var id = 0;
        $q.all([
           this.ListData()
        ]).then(function (msg) {
            success(msg);
        }, failure);
    }
    this.getItem = function (id) {
        return $http.post(("/FlatOwners/GetItem"), { id: id })
    },
    this.ListData = function () {
        return $http.post("/FlatOwners/ListData");
    }
    return this;
}]);


app.controller("FlatOwners", ["$scope", "FlatOwnersFactory", "$timeout", "$location", "$window", function ($scope, FlatOwnersFactory, $timeout, $location, $window) {
    $scope.save = function (model) {
        debugger;
        FlatOwnersFactory.save(model).then(function (success) {
            alert(success.data);
            $window.location.reload();
        });
    };


    FlatOwnersFactory.init(
        function (success) {
            debugger;
            $scope.model = success[0].data;
            if ($scope.model.ID > 0)
                $scope.model.enquiry_date = new Date($scope.model.enquiry_date);
            else
                $scope.model.enquiry_date = new Date(new Date().setHours(0, 0, 0, 0));
            $scope.property_types = success[1].data;
            $scope.locations = success[2].data;
        });
}]);
app.factory("FlatOwnersFactory", ["$rootScope", "$http", "$q", "CommonFactory", function ($rootScope, $http, $q, CommonFactory) {
    this.init = function (success, failure) {
        var id = 0;
        var urls = window.location.href.split('?')[0].split('/');
        if (!isNaN(urls[urls.length - 1]))
            id = eval(urls[urls.length - 1]);

        $q.all([
           this.getItem(id),
           CommonFactory.getPropertyTypes(),
           CommonFactory.getlocations()
        ]).then(function (msg) {
            success(msg);
        }, failure);
    }
    this.getItem = function (id) {
        return $http.post(("/FlatOwners/GetItem"), { id: id })
    },
    this.save = function (model) {
        return $http.post(("/FlatOwners/save"), { model: model });
    }
    this.getGridDetail = function (AsOnDate, ClaimType) {
        return $http.get('/FlatOwners/GetGridDetail/');
    }
    this.getClaimType = function () {
        return $http.get('/FlatOwners/GetDetails/');
    }
    return this;
}]);