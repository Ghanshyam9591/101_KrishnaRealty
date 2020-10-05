app.controller("CallHistory", ["$scope", "CallHistoryFactory", "$timeout", "$location", "$window", function ($scope, CallHistoryFactory, $timeout, $location, $window) {

    $scope.allCheckBoxStatus = "Select All";
    $scope.searchButtonText = "Search";

    $scope.BATCH_NUMBER = 1001;
    $scope.isPaymentdone = true;
    $scope.isAssignBatchStart = false;
    $scope.AS_ON_DATE = new Date(new Date().setHours(0, 0, 0, 0));


    $scope.GetGridRecord = function (enquiry_no) {
        $scope.searchButtonText = "Searching";
        $scope.asOnDate = new Date(new Date().setHours(0, 0, 0, 0));

        CallHistoryFactory.getGridDetail(enquiry_no).then(function (success) {
            $scope.searchButtonText = "Search";
            if (success.data.length >0)
                $scope.CallHistoryGridData = success.data;
            else
               alert("No Record Found !")
        });
    }
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
        CallHistoryFactory.getCustomers(string).then(function (success) {
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

}]);
app.factory("CallHistoryFactory", ["$rootScope", "$http", "$q", "CommonFactory", function ($rootScope, $http, $q, CommonFactory) {
    this.init = function (success, failure) {
        var id = 0;
        $q.all([
            CommonFactory.getClaimTypes()
        ]).then(function (msg) {
            success(msg);
        }, failure);
    }
    this.getGridDetail = function (enquiry_no) {
        return $http.post(('/CallHistory/GetGridDetail/'), { enquiry_no: enquiry_no });
    }
    this.getCustomers = function (q) {
        return $http.get(('/Common/GetCustomers/'), { params: { q: q } });
    }
    return this;
}]);