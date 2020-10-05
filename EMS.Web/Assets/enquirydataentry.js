app.controller("EnquiryDataEntryList", ["$scope", "EnquiryDataEntryListFactory", "$timeout", "$location", "$window", function ($scope, EnquiryDataEntryListFactory, $timeout, $location, $window) {

    EnquiryDataEntryListFactory.init(
        function (success) {
            debugger;
            $scope.enquiry_data = success[0].data;


        });
}]);

app.factory("EnquiryDataEntryListFactory", ["$rootScope", "$http", "$q", "CommonFactory", function ($rootScope, $http, $q, CommonFactory) {
    this.init = function (success, failure) {
        var id = 0;
        $q.all([
           this.ListData()
        ]).then(function (msg) {
            success(msg);
        }, failure);
    }
    this.getItem = function (id) {
        return $http.post(("/EnquiryDataEntry/GetItem"), { id: id })
    },
    this.ListData = function () {
        return $http.post("/EnquiryDataEntry/ListData");
    }
    return this;
}]);


app.controller("EnquiryDataEntry", ["$scope", "EnquiryDataEntryFactory", "$timeout", "$location", "$window", function ($scope, EnquiryDataEntryFactory, $timeout, $location, $window) {
    $scope.save = function (model) {
        debugger;
        EnquiryDataEntryFactory.save(model).then(function (success) {
            alert(success.data);
            $window.location.reload();
        });
    };


    EnquiryDataEntryFactory.init(
        function (success) {
            debugger;
            $scope.model = success[0].data;
            if ($scope.model.ID > 0)
                $scope.model.enquiry_date = new Date($scope.model.enquiry_date);
            else
                $scope.model.enquiry_date = new Date(new Date().setHours(0, 0, 0, 0));
            $scope.property_types = success[1].data;
            $scope.enquiry_sources = success[2].data;
            $scope.locations = success[3].data;
        });
}]);
app.factory("EnquiryDataEntryFactory", ["$rootScope", "$http", "$q", "CommonFactory", function ($rootScope, $http, $q, CommonFactory) {
    this.init = function (success, failure) {
        var id = 0;
        var urls = window.location.href.split('?')[0].split('/');
        if (!isNaN(urls[urls.length - 1]))
            id = eval(urls[urls.length - 1]);

        $q.all([
           this.getItem(id),
           CommonFactory.getPropertyTypes(),
           CommonFactory.getEnquirySources(),
           CommonFactory.getlocations()
        ]).then(function (msg) {
            success(msg);
        }, failure);
    }
    this.getItem = function (id) {
        return $http.post(("/EnquiryDataEntry/GetItem"), { id: id })
    },
    this.save = function (model) {
        return $http.post(("/EnquiryDataEntry/save"), { model: model });
    }
    this.getGridDetail = function (AsOnDate, ClaimType) {
        return $http.get('/EnquiryDataEntry/GetGridDetail/');
    }
    this.getClaimType = function () {
        return $http.get('/EnquiryDataEntry/GetDetails/');
    }
    return this;
}]);