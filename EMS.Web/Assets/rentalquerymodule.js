app.controller("RentalQueryModule", ["$scope", "RentalQueryModuleFactory", "$timeout", "$location", "$window", "CommonFactory", function ($scope, RentalQueryModuleFactory, $timeout, $location, $window, CommonFactory) {
    $scope.searchButtonText = "Search";
    $scope.queryTypes = [{ "Name": "Owner Name" }, { "Name": "Renter Name" }];
    $scope.isowner = true
    $scope.isrenter = true;
    //$scope.$watch("VAL_RI_NO", function (newVal, oldVal) {
    //    debugger;
    //    if (typeof newVal == "undefined" || newVal == 0)
    //        return;
    //    if (newVal == oldVal)
    //        return;
    //    CommonFactory.getCaseNobyRiNo(newVal).then(function (success) {
    //        $scope.caseNos = success.data;
    //        $scope.VAL_CASE_NO = $scope.caseNos[0];
    //    });

    //});


    //$scope.complete = function (string) {

    //    var output = [];
    //    RentalQueryModuleFactory.getEmployees(string).then(function (success) {
    //        debugger;
    //        $scope.countryList = success.data;
    //    });
    //    angular.forEach($scope.countryList, function (country) {
    //        if (country.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
    //            output.push(country);
    //        }
    //    });
    //    $scope.filterCountry = output;
    //}

    function search(nameKey, myArray) {
        for (var i = 0; i < myArray.length; i++) {
            if (myArray[i].EmpName === nameKey) {
                return myArray[i];
            }
        }
    }

    $scope.complete = function (string) {
        var output = [];
        $scope.filterCustomer = null;
        $scope.val_owner_code = 0;
        if (string === "")
            return;
        CommonFactory.getOwners(string).then(function (success) {
            debugger;
            $scope.customers = success.data;
            angular.forEach($scope.customers, function (Customer) {
                debugger;
                if (Customer.EmpName.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                    output.push(Customer.EmpName);
                }
            });
            $scope.filterCustomer = output;
        });

    }


    $scope.fillTextbox = function (string) {
        debugger;
        $scope.customer = string;
        var resultObject = search(string, $scope.customers);
        $scope.val_owner_code = resultObject.emp_code;
        $scope.filterCustomer = null;
    }



    $scope.complete2 = function (string) {
        var output = [];
        $scope.filterCustomer2 = null;
        $scope.val_renter_code = 0;
        if (string === "")
            return;
        CommonFactory.getRenters(string).then(function (success) {
            debugger;
            $scope.renters = success.data;
            angular.forEach($scope.renters, function (Renter) {
                debugger;
                if (Renter.EmpName.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                    output.push(Renter.EmpName);
                }
            });
            $scope.filterCustomer2 = output;
        });

    }


    $scope.fillTextbox2 = function (string) {
        debugger;
        $scope.Renter = string;
        var resultObject = search(string, $scope.renters);
        $scope.val_renter_code = resultObject.emp_code;
        $scope.filterCustomer2 = null;
    }
    $scope.$watchGroup(['QueryType'], function (newVal, oldVal) {

        debugger;

        $scope.isowner = true
        $scope.isrenter = true;

        if (typeof newVal == "undefined" || newVal == 0)
            return;
        if (newVal == oldVal)
            return;

        if (newVal[0].Name === "Owner Name")
            $scope.isowner = false
        else if (newVal[0].Name === "Renter Name")
            $scope.isrenter = false;
    });

    $scope.AddComment = function (Enquiry_No, Action_Type_ID, Comment) {

        RentalQueryModuleFactory.AddComment(Enquiry_No, Action_Type_ID, Comment).then(function (success) {
            alert(success.data);
        });
    }
    $scope.GetGridRecord = function (owner_id, renter_id, location_id, building_id) {
        debugger;
        if (owner_id === undefined)
            owner_id = 0;
        if (renter_id === undefined)
            renter_id = 0;
        if (location_id === undefined)
            location_id = 0;
        if (building_id === undefined)
            building_id = 0;

        $scope.searchButtonText = "Searching";
        $scope.asOnDate = new Date(new Date().setHours(0, 0, 0, 0));
        //$scope.queryParam.enquiry_date = new Date(new Date("1900-01-01").setHours(0, 0, 0, 0));
        RentalQueryModuleFactory.getGridDetail(owner_id, renter_id, location_id, building_id).then(function (success) {
            $scope.searchButtonText = "Search";
            $scope.query_data = success.data;
            if (success.data.length > 0) {
                if (success.data.length < 1)
                    $scope.isAssignBtnHide = true;
                else
                    $scope.isAssignBtnHide = false;
            }
            else {
                alert("No Record Found !");
            }
        });
    }

    //$scope.claimTypes = ["Conveyance"]

    RentalQueryModuleFactory.init(
        function (success) {
            // $scope.model = success[0].data;
            debugger;
            $scope.locations = success[0].data;
            $scope.buildings = success[1].data;
            $scope.queryParam = [{ "quuery_type_id": 0, "location_id": 0, "building_id": 0 }];
            $scope.queryTypes = [{ "Name": "Select" }, { "Name": "Owner Name" }, { "Name": "Renter Name" }];
            $scope.QueryType = $scope.queryTypes[0];
        });

}]);
app.factory("RentalQueryModuleFactory", ["$rootScope", "CommonFactory", "$http", "$q", function ($rootScope, CommonFactory, $http, $q) {
    this.init = function (success, failure) {
        var id = 0;
        $q.all([
           CommonFactory.getlocations(),
           CommonFactory.getBuildings()
        ]).then(function (msg) {
            success(msg);
        }, failure);
    }
    this.getGridDetail = function (owner_id, renter_id, location_id, building_id) {
        return $http.post(('/RentalQueryModule/GetGridDetail/'), { owner_id: owner_id, renter_id: renter_id, location_id: location_id, building_id: building_id });
    }
    this.AddComment = function (Enquiry_No, Action_Type_ID, Comment) {
        return $http.post(('/RentalQueryModule/AddComment/'), { Enquiry_No: Enquiry_No, Action_Type_ID: Action_Type_ID, Comment: Comment });
    }
    this.getCustomers = function (q) {
        return $http.get(('/Common/GetCustomers/'), { params: { q: q } });
    }
    return this;
}]);