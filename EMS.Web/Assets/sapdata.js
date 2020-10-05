
app.controller("SAPData", ["$scope", "SAPDataFactory", "$timeout", "$location", "$window", function ($scope, SAPDataFactory, $timeout, $location, $window) {
    $scope.searchButtonText = "Search";
    $scope.allCheckBoxStatus = "Select All";
    $scope.BATCH_NUMBER = 1001;

    $scope.isAssignBtnHide = true;

    $scope.isAssignBatchStart = false;
    $scope.isExportbuttonHide = true;
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

    //    SAPDataFactory.getGridDetail("2020-04-15", claimtype).then(function (success) {
    //        debugger;
    //        $scope.sapgriddata = success.data;
    //    });

    //});

    $scope.GetGridRecord = function (asOnDate, claimType) {
        $scope.searchButtonText = "Searching";
        $scope.asOnDate = new Date(new Date().setHours(0, 0, 0, 0));

        SAPDataFactory.getGridDetail(asOnDate, claimType).then(function (success) {
            $scope.searchButtonText = "Search";
            $scope.sapgriddata = success.data;
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


    $scope.CheckboxActiity = function (index, ischecked, status) {
        debugger;
        if (status === 1) {
            for (var k = 0; k < $scope.sapgriddata.length; k++) {
                if (ischecked) {
                    $scope.sapgriddata[k].SELECT_STATUS = true;
                    $scope.isAssignBatchStart = true;
                }
                else {
                    $scope.sapgriddata[k].SELECT_STATUS = false;
                }
            }
        }
        else {
            if (ischecked) {
                $scope.sapgriddata[index].SELECT_STATUS = true;
                $scope.isAssignBatchStart = true;
            }
            else {
                $scope.sapgriddata[index].SELECT_STATUS = false;
            }

        }
    };


    $scope.ChangeSapFileStatus = function (RevisedExpenseGridData) {

        SAPDataFactory.ChangeSapFileStatus(RevisedExpenseGridData).then(function (success) {
            debugger;
            if (!$scope.isAssignBatchStart) {

                alert("Please select  at least one record!");
                return;
            }
            if (success.data.length < 1) {
                $scope.isExportbuttonHide = false;
                $scope.isAssignBatchStart = true;
                alert("Sap File Generated Successfully  !")


            }
            else {
                alert(success.data.ERR_MSG);
            }
            // $scope.sapgriddata = success.data;
        });
    }

    SAPDataFactory.init(
        function (success) {

            $scope.claimTypes = success[0].data;

            $scope.CLAIM_TYPE = $scope.claimTypes[0];
        });

}]);
app.factory("SAPDataFactory", ["$rootScope", "$http", "$q", "CommonFactory", function ($rootScope, $http, $q, CommonFactory) {
    this.init = function (success, failure) {
        var id = 0;
        $q.all([
            CommonFactory.getClaimTypes()
        ]).then(function (msg) {
            success(msg);
        }, failure);
    }
    this.getGridDetail = function (AsOnDate, ClaimType) {
        return $http.post(('/SAPData/GetGridDetail/'), { AsOnDate: AsOnDate.toJSON(), ClaimType: ClaimType });
    }
    this.ChangeSapFileStatus = function (RevisedExpenseGridData) {
        return $http.post(('/SAPData/ChangeSapFileStatus/'), { model: RevisedExpenseGridData });
    }
    this.getClaimType = function () {
        return $http.get('/SAPData/GetDetails/');
    }
    return this;
}]);