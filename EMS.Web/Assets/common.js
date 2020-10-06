app.factory("CommonFactory", ["$rootScope", "$http", "$q", function ($rootScope, $http, $q) {
    this.getPropertyTypes = function () {
        return $http.get('/Common/GetPropertytypes/');
    },
    this.getEnquiryTypes = function () {
        return $http.get('/Common/GetEnquiryTypes/');
    },
     this.getLovCategories = function () {
         return $http.get('/Common/GetLovCategories/');
     },
    this.getActionTypes = function () {
        return $http.get('/Common/GetActionTypes/');
    },
    this.getEnquirySources = function () {
        return $http.get('/Common/GetEnquirySources/');
    },
    this.getlocations = function () {
        return $http.get('/Common/GetLocations/');
    },
    this.getMenus = function () {
        return $http.get('/Common/GetMenus/');
    },
    this.getRoles = function () {
        return $http.get('/Common/GetRoles/');
    },
    this.getUsers = function () {
        return $http.get('/Common/GetUsers/');
    },
    this.getCustomers = function (q) {
        return $http.get(('/Common/GetCustomers/'), { params: { q: q } });
    }
    return this;
}]);