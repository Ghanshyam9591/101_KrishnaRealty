app.controller("QueryModule", ["$scope", "QueryModuleFactory", "$timeout", "$location", "$window","CommonFactory", function ($scope, QueryModuleFactory, $timeout, $location, $window,CommonFactory) {
    $scope.isAssignBatchStart = false;
    $scope.isAssignBtnHide = true;
    $scope.searchButtonText = "Search";
    $scope.countryList = ["Afghanistan", "Albania"];
    $scope.queryTypes = [{ "Name": "Select" }, { "Name": "Process" }, { "Name": "Payment" }, { "Name": "SAP" }];


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
    //    QueryModuleFactory.getEmployees(string).then(function (success) {
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
        $scope.val_customer_code = 0;
        if (string === "")
            return;
        CommonFactory.getCustomers(string).then(function (success) {
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
        $scope.val_customer_code = resultObject.emp_code;
        $scope.filterCustomer = null;
    }

    //$scope.$watchGroup(['queryParam.PAYMENT_DATE'], function (newVal, oldVal) {

    //    debugger;

    //    if (typeof newVal == "undefined" || newVal == 0)
    //        return;
    //    if (newVal == oldVal)
    //        return;

    //    alert("OK");
    //    var asondate = newVal[0];
    //    var claimtype = newVal[1];

    //    //$scope.allCheckBoxStatus = "Select All";
    //    //$scope.IsCheckedStatus = "checked";

    //    //QueryModuleFactory.getGridDetail("2020-04-15", claimtype).then(function (success) {
    //    //    debugger;
    //    //    $scope.RevisedExpenseGridData = success.data;
    //    //});

    //});

    $scope.AddComment = function (Enquiry_No, Action_Type_ID, Comment) {

        QueryModuleFactory.AddComment(Enquiry_No, Action_Type_ID, Comment).then(function (success) {
            alert(success.data);
        });
    }
    $scope.GetGridRecord = function (cust_id, location_id, enquiry_source_id, enquiry_from, enquiry_to, action_type_id) {
        debugger;
        if (cust_id === undefined)
            cust_id = 0;
        if (location_id === undefined)
            location_id = 0;
        if (enquiry_source_id === undefined)
            enquiry_source_id = 0;
        if (action_type_id === undefined)
            action_type_id = 0;

        $scope.searchButtonText = "Searching";
        $scope.asOnDate = new Date(new Date().setHours(0, 0, 0, 0));
        //$scope.queryParam.enquiry_date = new Date(new Date("1900-01-01").setHours(0, 0, 0, 0));
        QueryModuleFactory.getGridDetail(cust_id, location_id, enquiry_source_id, enquiry_from, enquiry_to, action_type_id).then(function (success) {
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

    QueryModuleFactory.init(
        function (success) {
            // $scope.model = success[0].data;
            debugger;
            $scope.enquiry_sources = success[0].data;
            $scope.locations = success[1].data;
            $scope.action_types = success[2].data;
            $scope.queryParam = [{ "cust_name": "", "cust_id": 0, "location_id": 0, "enquiry_source_id": 0, "enquiry_from": "", "enquiry_to": "", "action_type_id": 0, "query_type": "" }];

            //$scope.queryParam.CLAIM_TYPE = $scope.claimTypes[0]; // Select as default value.
            //$scope.queryParam.CLAIM_STATUS = $scope.recordStatus[0]; // Select as default value.
            //$scope.queryParam.QUERY_TYPE = $scope.queryTypes[0];

        });

}]);
app.factory("QueryModuleFactory", ["$rootScope", "CommonFactory", "$http", "$q", function ($rootScope, CommonFactory, $http, $q) {
    this.init = function (success, failure) {
        var id = 0;
        $q.all([
             CommonFactory.getEnquirySources(),
           CommonFactory.getlocations(),
           CommonFactory.getActionTypes()
        ]).then(function (msg) {
            success(msg);
        }, failure);
    }
    this.getGridDetail = function (cust_id, location_id, enquiry_source_id, enquiry_from, enquiry_to, action_type_id) {
        return $http.post(('/QueryModule/GetGridDetail/'), { cust_id: cust_id, location_id: location_id, enquiry_source_id: enquiry_source_id, enquiry_from: enquiry_from, enquiry_to: enquiry_to, action_type_id: action_type_id });
    }
    this.getClaimType = function () {
        return $http.get('/QueryModule/GetDetails/');
    }
    this.AddComment = function (Enquiry_No, Action_Type_ID, Comment) {
        return $http.post(('/QueryModule/AddComment/'), { Enquiry_No: Enquiry_No, Action_Type_ID: Action_Type_ID, Comment: Comment });
    }
    this.getCustomers = function (q) {
        return $http.get(('/Common/GetCustomers/'), { params: { q: q } });
    }
    return this;
}]);