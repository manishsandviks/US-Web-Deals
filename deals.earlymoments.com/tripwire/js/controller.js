'use strict';

function homeController($scope) {

    $scope.title = 'Home';

};

function shippingController($scope, $window) {

    $scope.alldata = [];
    $scope.shippingdata = [];
    $scope.billingdata = [];
    $scope.showBillAdd = false;
    $scope.isSameAddress = 'Yes';
    
    angular.element('#tab2').addClass('disabled');
    angular.element('#tab2').find('a').removeAttr("data-toggle");

   
    $scope.saveShippingInfo = function () {
        if ($scope.shipform1.$valid) {
            $scope.alldata = [{
                "shippingdata": {
                    'firstname': $scope.shipform.firstname,
                    'lastname': $scope.shipform.lastname,
                    'streetaddress': $scope.shipform.streetaddress,
                    'address2': this.isUndefinedOrNull($scope.shipform.address2) == true ? '' : $scope.shipform.address2,
                    'city': $scope.shipform.city,
                    'state': $scope.shipform.state,
                    'zipcode': $scope.shipform.zipcode,
                    'phone': this.isUndefinedOrNull($scope.shipform.phone) == true ? '' : $scope.shipform.phone,
                    'email': $scope.shipform.email,
                    'checkbox1': $scope.shipform.checkbox1 == true ? 'Y' : 'N',
                    'checkbox2': $scope.shipform.checkbox2 == true ? 'Y' : 'N',
                    'childname': this.isUndefinedOrNull($scope.shipform.childname) == true ? '' : $scope.shipform.childname,
                    'childdob': this.isUndefinedOrNull($scope.shipform.childdob) == true ? '' : $scope.shipform.childdob,
                    'gender': this.isUndefinedOrNull($scope.shipform.gender) == true ? '' : $scope.shipform.gender
                },
                "billingdata": {
                    'firstname': '',
                    'lastname': '',
                    'streetaddress': '',
                    'address2': '',
                    'city': '',
                    'state': '',
                    'zipcode': '',
                    'cardnumber': '',
                    'expmonth': '',
                    'expyear': '',
                    'cvv': ''
                }
            }];
            console.log("shipping form");
            console.log($scope.alldata);
            /*enable next tab*/
            angular.element('#tab1').removeClass('active');
            angular.element('#tab1').addClass('disabled');
            angular.element('#tab1').removeAttr("data-toggle");
            angular.element('#tab1').find('a').attr("aria-expanded", "false");

            angular.element('#tab2').removeClass('disabled');
            angular.element('#tab2').addClass('active');
            angular.element('#tab2').attr("data-toggle", "tab");
            angular.element('#tab2').find('a').attr("aria-expanded", "true");

            angular.element("#shipping").removeClass("active");
            angular.element("#billing").addClass("active in");
            this.getValue();
        } else {
            angular.element(".errMsg").show();
        }
    };

    $scope.getValue = function () {       
               
        if ($scope.isSameAddress == 'Yes') {

            $scope.firstname = $scope.alldata[0].shippingdata.firstname;
            $scope.lastname = $scope.alldata[0].shippingdata.lastname;
            $scope.streetaddress = $scope.alldata[0].shippingdata.streetaddress;
            $scope.address2 = $scope.alldata[0].shippingdata.address2;
            $scope.city = $scope.alldata[0].shippingdata.city;
            $scope.state = $scope.alldata[0].shippingdata.state;
            $scope.zipcode = $scope.alldata[0].shippingdata.zipcode;
            
            $scope.showBillAdd = false;
        } else {
            
            $scope.firstname = "";
            $scope.lastname = '';
            $scope.streetaddress = '';
            $scope.address2 = '';
            $scope.city = '';
            $scope.state = '';
            $scope.zipcode = '';       
            
            $scope.showBillAdd = true;
        }
        console.log("call getvalue function");
    };

    $scope.saveBillingInfo = function () {
        if ($scope.billform1.$valid) {
           
            $scope.alldata[0].billingdata.firstname = $scope.firstname;
            $scope.alldata[0].billingdata.lastname = $scope.lastname;
            $scope.alldata[0].billingdata.streetaddress = $scope.streetaddress;
            $scope.alldata[0].billingdata.address2 = $scope.address2;
            $scope.alldata[0].billingdata.city = $scope.city;
            $scope.alldata[0].billingdata.state = $scope.state;
            $scope.alldata[0].billingdata.zipcode = $scope.zipcode;

            $scope.alldata[0].billingdata.cardnumber = $scope.billform.cardnumber;
            $scope.alldata[0].billingdata.expmonth = $scope.billform.expmonth;
            $scope.alldata[0].billingdata.expyear = $scope.billform.expyear;
            $scope.alldata[0].billingdata.cvv = $scope.billform.cvv;

            console.log("billing data");
            console.log($scope.alldata);

            $window.location.href = '#/upsell-offer1';
        } else {
            angular.element(".errMsgBill").show();
        }
    };

    $scope.isUndefinedOrNull = function (val) {
        return angular.isUndefined(val) || val === null
    }

    $scope.range = function (min, max, step) {
        step = step || 1;
        var input = [];
        for (var i = min; i <= max; i += step) input.push(i);
        return input;
    };
};

function upsellOffer1Controller($scope, $window) {

    $scope.addFirstOffer = function () {
        $window.location.href = '#/upsell-offer2';
    };

};
function upsellOffer2Controller($scope, $window) {

    $scope.addSecondOffer = function () {
        $window.location.href = '#/thank-you';
    };

};
function thankyouController($scope) {

    console.log("thank you page");

};
