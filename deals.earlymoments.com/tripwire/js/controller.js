'use strict';

app.controller('homeController', function ($scope, $http, myService) {
    var results;
    $scope.title = 'Home';
   //call the API
    myService.defaultAPI()
        .success(function (data, status, headers, config) {
            console.log("Default API called result =");
            console.log(data);
        });      
});


app.controller('shippingController', function ($scope, $window, $http, myService) {
    $scope.alldata = [];
    $scope.shippingdata = [];
    $scope.billingdata = [];
    $scope.showBillAdd = false;
    $scope.isSameAddress = 'Yes';
    $scope.showModal = false;
    $scope.showBillingModal = false;
    $scope.showWaitingModal = false;
    $scope.billform = { type: undefined };
    $scope.currentYear = new Date().getFullYear();
    $scope.currentMonth = new Date().getMonth() + 1;
   
    angular.element('#tab2').addClass('disabled');
    angular.element('#tab2').find('a').removeAttr("data-toggle");

    $scope.saveShippingInfo = function () {
        if ($scope.shipform1.$valid) {
            $scope.showModal = false;
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
            //$scope.showWaitingModal = true;

            /*Call the shipping API*/     
            //myService.shippingAPI($scope.alldata)
            //        .then(function (response) {
            //            results = response;
            //            console.log("After API called result =");
            //            console.log(results);
            //        }).finally(function () {
            //            //$scope.showWaitingModal = false;
            //        });
           myService.shippingAPI($scope.alldata)
                    .success(function (data, status, headers, config) {
                        console.log("After API called result =");
                        console.log(data);
                    }).finally(function () {
                        //$scope.showWaitingModal = false;
                    });
            /*End the shipping API*/

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
            //angular.element(".errMsg").show();
            $scope.showModal = true;
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
            $scope.showBillingModal = false;
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

            /*Call the shipping API*/
            myService.billingAPI($scope.alldata)
                .success(function (data, status, headers, config) {
                    console.log("After API called result =");
                    console.log(data);
                }).finally(function () {
                        $window.location.href = '#/upsell-offer1';
                    });
            /*End the shipping API*/
            
        } else {
            $scope.showBillingModal = true;
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
});

app.controller('upsellOffer1Controller', function ($scope, $window) {
    $scope.showUpsellOffer1Modal = false;
    //for selecting yes option
    $scope.addFirstOffer = function () {
        if ($scope.upsellform1.$valid) {
            $scope.showUpsellOffer1Modal = false;
            /*Call the upsell first offer API*/
            //myService.upsellFirstOfferAPI($scope.alldata)
            //        .then(function (response) {
            //            results = response;
            //            console.log("After API called result =");
            //            console.log(results);
            //        }).finally(function () {
                        
            //        });
            /*End the API*/
            $window.location.href = '#/upsell-offer2';
        } else {
            $scope.showUpsellOffer1Modal = true;
        }
    };

});

app.controller('upsellOffer2Controller', function ($scope, $window) {
    $scope.addSecondOffer = function () {
        /*Call the upsell second offer API*/
        //myService.upsellSecondOfferAPI($scope.alldata)
        //        .then(function (response) {
        //            results = response;
        //            console.log("After API called result =");
        //            console.log(results);
        //        }).finally(function () {

        //        });
        /*End the API*/
        $window.location.href = '#/thank-you';
    };

});

app.controller('thankyouController', function ($scope, $window) {
    console.log("thank you page");
});