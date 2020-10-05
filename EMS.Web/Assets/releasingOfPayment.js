app.controller("ReleaseOfPayment", ["$scope", "ReleaseOfPaymentFactory", "$timeout", "$location", "$window", function ($scope, ReleaseOfPaymentFactory, $timeout, $location, $window) {
    $scope.searchButtonText = "Search";
    $scope.allCheckBoxStatus = "Select All";
    $scope.BATCH_NUMBER = 1001;
    $scope.isPaymentdone = true;
    $scope.isAssignBatchStart = false;
    $scope.AS_ON_DATE = new Date(new Date().setHours(0, 0, 0, 0));

    //$scope.$watchGroup(['AS_ON_DATE', 'CLAIM_TYPE'], function (newVal, oldVal) {

    //    debugger;

    //    if (typeof newVal == "undefined" || newVal == 0)
    //        return;
    //    if (newVal == oldVal)
    //        return;

    //    var asondate = newVal[0];
    //    var claimtype = newVal[1];

    //    $scope.allCheckBoxStatus = "Select All";
    //    $scope.IsCheckedStatus = "checked";

    //    ProcessOfRecordFactory.getGridDetail("2020-04-15", claimtype).then(function (success) {
    //        debugger;
    //        $scope.ReleaseOfPaymentGridData = success.data;
    //    });

    //});

    $scope.GetGridRecord = function (asOnDate, claimType) {
        debugger;
        $scope.searchButtonText = "Searching";
        $scope.asOnDate = new Date(new Date().setHours(0, 0, 0, 0));

        ReleaseOfPaymentFactory.getGridDetail(asOnDate, claimType).then(function (success) {
            debugger;
            $scope.searchButtonText = "Search";
            $scope.ReleaseOfPaymentGridData = success.data;
            if (success.data.length < 1)
                $scope.isPaymentdone = true;
            else
                $scope.isPaymentdone = false;
        });
    }
    $scope.CheckboxActiity = function (index, ischecked, status, asOnDate2) {
        debugger;
        if (status === 1) {
            for (var k = 0; k < $scope.ReleaseOfPaymentGridData.length; k++) {
                if (ischecked) {
                    $scope.ReleaseOfPaymentGridData[k].SELECT_STATUS = true;
                    $scope.isAssignBatchStart = true;
                }
                else {
                    $scope.ReleaseOfPaymentGridData[k].SELECT_STATUS = false;
                }
            }
        }
        else {
            if (ischecked) {
                $scope.ReleaseOfPaymentGridData[index].SELECT_STATUS = true;
                $scope.isAssignBatchStart = true;
            }
            else {
                $scope.ReleaseOfPaymentGridData[index].SELECT_STATUS = false;
            }

        }

        debugger;
    };
    

    $scope.paymentStatus = function (ReleaseOfPaymentGridData) {
        debugger;
        ReleaseOfPaymentFactory.paymentStatus(ReleaseOfPaymentGridData).then(function (success) {
            if (!$scope.isAssignBatchStart) {
                alert("Please select at least one record !");
                return;
            }
            if (success.data.length < 1) {
                alert("Record save Successfully !")
                $window.location.reload();
            }
            else {
                alert(success.data.ERR_MSG);
            }
            // $scope.ReleaseOfPaymentGridData = success.data;
        });
    }

    ReleaseOfPaymentFactory.init(
        function (success) {
            $scope.claimTypes = success[0].data;

            $scope.CLAIM_TYPE = $scope.claimTypes[0]; // Select as default value.
        });
}]);
app.factory("ReleaseOfPaymentFactory", ["$rootScope", "$http", "$q", "CommonFactory", function ($rootScope, $http, $q, CommonFactory) {
    this.init = function (success, failure) {
        var id = 0;
        $q.all([
            CommonFactory.getClaimTypes()
        ]).then(function (msg) {
            success(msg);
        }, failure);
    }
    this.getGridDetail = function (AsOnDate, ClaimType) {
        return $http.post(('/ReleasingOfPayment/GetGridDetail/'), { AsOnDate: AsOnDate.toJSON(), ClaimType: ClaimType });
    }
    this.paymentStatus = function (ReleaseOfPaymentGridData) {
        return $http.post(('/ReleasingOfPayment/PaymentStatus/'), { model: ReleaseOfPaymentGridData });
    }
    this.getClaimType = function () {
        return $http.get('/ReleasingOfPayment/GetDetails/');
    }
    return this;
}]);