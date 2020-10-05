//var app = angular.module('myApp', [])
app.controller("roleMenuMapping", ["$scope", "roleMenuMappingFactory", "$timeout", "$location", "$window", function ($scope, roleMenuMappingFactory, $timeout, $location, $window) {
    $scope.searchButtonText = "Search";
    $scope.allCheckBoxStatus = "Select All";

    $scope.AS_ON_DATE = new Date(new Date().setHours(0, 0, 0, 0));

    $scope.$watch("RoleID", function (newVal, oldVal) {
        debugger;
        if (typeof newVal == "undefined" || newVal == 0)
            return;
        if (newVal == oldVal)
            return;

        roleMenuMappingFactory.getAssignedMenuToRole(newVal).then(function (success) {
            debugger;
            $scope.searchButtonText = "Search";
            $scope.assignedmenus = success.data;

            for (var k3 = 0; k3 < $scope.menus_list.length; k3++) {
                $scope.menus_list[k3].select_status = false;
            }

            for (var k2 = 0; k2 < $scope.menus_list.length; k2++) {

                for (var i = 0; i < $scope.assignedmenus.length; i++) {
                    if ($scope.assignedmenus[i].menu_id === $scope.menus_list[k2].seqid) {
                        $scope.menus_list[k2].select_status = true;
                    }
                }
            }
        });
    });


    $scope.GetGridRecord = function (asOnDate, claimType) {
        debugger;
        $scope.searchButtonText = "Searching";
        $scope.asOnDate = new Date(new Date().setHours(0, 0, 0, 0));

        roleMenuMappingFactory.getGridDetail(asOnDate, claimType).then(function (success) {
            debugger;
            $scope.searchButtonText = "Search";
            $scope.roleMenuMappingGridData = success.data;

            for (var k = 0; k < $scope.roleMenuMappingGridData.length; k++) {
                $scope.roleMenuMappingGridData[k].SELECT_STATUS = true;
                $scope.isAssignBatchStart = true;
            }

            if (success.data.length > 0) {
                $scope.isPaymentdone = false;
            }
            else {
                $scope.isPaymentdone = true;
                alert("No Record Found !")
            }
        });
    }
    $scope.CheckboxActivity = function (index, ischecked) {
        debugger;

        if (ischecked) {
            $scope.menus_list[index].select_status = true;
            $scope.isselectedStart = true;
        }
        else {
            $scope.menus_list[index].select_status = false;
        }
    };


    $scope.save = function (menu_list, role_id) {

        roleMenuMappingFactory.save(menu_list, role_id).then(function (success) {
            debugger;
            alert("Record save Successfully !")
            $window.location.reload();
        });
    }

    roleMenuMappingFactory.init(
        function (success) {
            debugger;
            $scope.rolelist = success[0].data;
            $scope.menus_list = success[1].data;

        });
}]);
app.factory("roleMenuMappingFactory", ["$rootScope", "$http", "$q", "CommonFactory", function ($rootScope, $http, $q, CommonFactory) {
    this.init = function (success, failure) {
        var id = 0;
        $q.all([
            CommonFactory.getRoles(),
           // CommonFactory.getMenus()
            this.getGridDetail()
        ]).then(function (msg) {
            success(msg);
        }, failure);
    }
    this.getGridDetail = function () {
        return $http.post('/MenuRoleMap/GetGridDetail/');
    }
    this.save = function (menus_list, role_id) {
        return $http.post(('/MenuRoleMap/Save/'), { model: menus_list, role_id: role_id });
    }
    this.getAssignedMenuToRole = function (RoleID) {
        return $http.post(('/MenuRoleMap/GetAssignedMenuToRole/'), { RoleID: RoleID });
    }
    return this;
}]);