app.controller("Menu", ["$scope", "MenuFactory", "$timeout", "$location", function ($scope, MenuFactory, $timeout, $location) {
    $scope.menutypes = ["System Settings", "Activity Process", "Reports"]

    //$scope.MenuType = "System Settings";

    //$scope.$watch('MenuType', function (newval, oldval) {

    //    debugger;
    //    if (typeof newval == "undefined" || newval == 0)
    //        return;
    //    //if (newval == oldval)
    //    //    return;

    //    var menus = "";
    //    if (newval === "System Settings") {
    //        menus = [{ "ID": 1, "Name": "User Master", "URL": "#" }, { "ID": 2, "Name": "Menu Master", "URL": "#" }, { "ID": 3, "Name": "Role Master", "URL": "#" }, { "ID": 3, "Name": "User Role Mapping", "URL": "#" }, { "ID": 3, "Name": "User Role Mapping", "URL": "#" }, { "ID": 3, "Name": "User Role Mapping", "URL": "#" }, { "ID": 3, "Name": "User Role Mapping", "URL": "#" }, { "ID": 3, "Name": "User Role Mapping", "URL": "#" }];
    //    }
    //    else if (newval === "Activity Process") {
    //        menus = [{ "ID": 1, "Name": "Process Of Record", "URL": "http://localhost:50454/ProcessOfRecord" }];
    //    }
    //    else if (newval === "Reports") {
    //        menus = [{ "ID": 1, "Name": "Report 1", "URL": "#" }, { "ID": 2, "Name": "Report 2", "URL": "#" }];
    //    }
    //    menus = [{ "ID": 1, "Name": "User Master", "URL": "#" }, { "ID": 2, "Name": "Menu Master", "URL": "#" }, { "ID": 3, "Name": "Role Master", "URL": "#" }, { "ID": 3, "Name": "User Role Mapping", "URL": "#" }, { "ID": 3, "Name": "User Role Mapping", "URL": "#" }, { "ID": 3, "Name": "User Role Mapping", "URL": "#" }, { "ID": 3, "Name": "User Role Mapping", "URL": "#" }, { "ID": 3, "Name": "User Role Mapping", "URL": "#" }];
    //    $scope.submenus = menus;

    //});

    MenuFactory.init(
        function (success) {
            debugger;
            $scope.submenus = success[0].data;
        });


}]);
app.factory("MenuFactory", ["$rootScope", "$http", "$q", "CommonFactory", function ($rootScope, $http, $q, CommonFactory) {
    this.init = function (success, failure) {
        var id = 0;
        $q.all([
           CommonFactory.getMenus()
        ]).then(function (msg) {
            success(msg);
        }, failure);
    }
    this.getGridDetail = function (AsOnDate, ClaimType) {
        return $http.get('/Menu/GetGridDetail/');
    }
    this.getClaimType = function () {
        return $http.get('/Menu/GetDetails/');
    }
    return this;
}]);