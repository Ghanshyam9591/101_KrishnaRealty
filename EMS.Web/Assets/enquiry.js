app.controller("Enquiry", ["$scope", "EnquiryFactory", "$timeout", "$location", "$window", function ($scope, EnquiryFactory, $timeout, $location, $window) {
    alert("okk");
    $scope.allCheckBoxStatus = "Select All";
    $scope.BATCH_NUMBER = 1001;

    $scope.isAssignBtnHide = true;

    $scope.isAssignBatchStart = false;
    $scope.searchButtonText = "Search";
    $scope.AS_ON_DATE = new Date(new Date().setHours(0, 0, 0, 0));

    $scope.agents = [{ "ID": 1, "Name": "Deepak" }, { "ID": 2, "Name": "Deepak2" }];

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

    //    EnquiryFactory.getGridDetail("2020-04-15", claimtype).then(function (success) {
    //        debugger;
    //        $scope.RevisedExpenseGridData = success.data;
    //    });

    //});


    $scope.exportToExcel = function (asOnDate, claimType) {

        $scope.asOnDate = new Date(new Date().setHours(0, 0, 0, 0));

        //$.ajax(
        //    {
        //        url: '@Url.Action("GetExportToExcel", "Enquiry")',
        //        contentType: 'application/json; charset=utf-8',
        //        datatype: 'json',
        //        data: {
        //            studentId: 123
        //        },
        //        type: "POST",
        //        success: function () {
        //            window.location = '@Url.Action("GetExportToExcel", "Enquiry", "++")';
        //        }
        //    });

        EnquiryFactory.getExportToExcel(asOnDate, claimType).then(function (success) {
            alert("success");

            // window.location = '@Url.Action("DownloadAttachment", "PostDetail")';
        });
    }

    $scope.GetGridRecord = function (asOnDate, claimType) {
        $scope.searchButtonText = "Searching";
        $scope.asOnDate = new Date(new Date().setHours(0, 0, 0, 0));

        EnquiryFactory.getGridDetail(asOnDate, claimType).then(function (success) {
            $scope.searchButtonText = "Search";
            if (success.data.length > 0) {
                $scope.RevisedExpenseGridData = success.data;
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
            for (var k = 0; k < $scope.RevisedExpenseGridData.length; k++) {
                if (ischecked) {
                    $scope.RevisedExpenseGridData[k].SELECT_STATUS = true;
                    $scope.isAssignBatchStart = true;
                }
                else {
                    $scope.RevisedExpenseGridData[k].SELECT_STATUS = false;
                }
            }
        }
        else {
            if (ischecked) {
                $scope.RevisedExpenseGridData[index].SELECT_STATUS = true;
                $scope.isAssignBatchStart = true;
            }
            else {
                $scope.RevisedExpenseGridData[index].SELECT_STATUS = false;
            }

        }

        debugger;
    };
    //$scope.claimTypes = ["Conveyance"]

    $scope.AssignBatchNumber = function (RevisedExpenseGridData) {

        EnquiryFactory.assingBatchNumber(RevisedExpenseGridData).then(function (success) {
            debugger;
            if (!$scope.isAssignBatchStart) {

                alert("Please select  at least one !");
                return;
            }
            if (success.data.length < 1) {
                alert("Batch Number Assigned Successfully !")
                $window.location.reload();
            }
            else {
                alert(success.data.ERR_MSG);
            }
            // $scope.RevisedExpenseGridData = success.data;
        });
    }

    EnquiryFactory.init(
        function (success) {
            // $scope.model = success[0].data;
            debugger;
            $scope.claimTypes = success[0].data;

            $scope.CLAIM_TYPE = $scope.claimTypes[0];
        });



}]);
app.factory("EnquiryFactory", ["$rootScope", "$http", "$q", "CommonFactory", function ($rootScope, $http, $q, CommonFactory) {
    this.init = function (success, failure) {
        var id = 0;
        $q.all([
            CommonFactory.getClaimTypes()
        ]).then(function (msg) {
            success(msg);
        }, failure);
    }
    this.getGridDetail = function (AsOnDate, ClaimType) {
        return $http.post(('/Enquiry/GetGridDetail/'), { AsOnDate: AsOnDate.toJSON(), ClaimType: ClaimType });
    }
    this.getExportToExcel = function (AsOnDate, ClaimType) {
        return $http.post(('/Enquiry/GetExportToExcel/'), { AsOnDate: AsOnDate.toJSON(), ClaimType: ClaimType });
    }
    this.assingBatchNumber = function (RevisedExpenseGridData) {
        return $http.post(('/Enquiry/AssignBatchNumber/'), { model: RevisedExpenseGridData });
    }
    this.getClaimType = function () {
        return $http.get('/Enquiry/GetDetails/');
    }
    return this;
}]);