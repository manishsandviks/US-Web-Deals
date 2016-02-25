'use strict';

var app = angular.module('AngularStore', []);
 app.config(['$routeProvider', function ($routeProvider) {
     $routeProvider.
         when('/home', {
             templateUrl: 'partials/home.html',
             controller: homeController
         }).
         when('/shipping', {
             templateUrl: 'partials/shipping.html',
             controller: shippingController
         }).
          when('/upsell-offer1', {
              templateUrl: 'partials/upsell-offer1.html',
              controller: upsellOffer1Controller
          }).
           when('/upsell-offer2', {
               templateUrl: 'partials/upsell-offer2.html',
               controller: upsellOffer2Controller
           }).
           when('/thankyou', {
               templateUrl: 'partials/thankyou.html',
               controller: thankyouController
           }).
          otherwise({
              redirectTo: '/home'
          });
  }]);

app.directive('matchValidator', [function () {
    return {
        require: 'ngModel',
        link: function (scope, elm, attr, ctrl) {
            var pwdWidget = elm.inheritedData('$formController')[attr.matchValidator];

            ctrl.$parsers.push(function (value) {
                if (value === pwdWidget.$viewValue) {
                    ctrl.$setValidity('match', true);
                    return value;
                }

                if (value && pwdWidget.$viewValue) {
                    ctrl.$setValidity('match', false);
                }

            });

            pwdWidget.$parsers.push(function (value) {
                if (value && ctrl.$viewValue) {
                    ctrl.$setValidity('match', value === ctrl.$viewValue);
                }
                return value;
            });
        }
    };
}]);

app.directive('toNumber', function () {
    return {
        require: 'ngModel',
        link: function (scope, elem, attrs, ctrl) {
            ctrl.$parsers.push(function (value) {
                return parseFloat(value || '');
            });
        }
    };
});

app.directive
  ('creditCardType'
  , function () {
      var directive =
        {
            require: 'ngModel'
        , link: function (scope, elm, attrs, ctrl) {
            ctrl.$parsers.unshift(function (value) {
                scope.type =
                  (/^5[1-5]/.test(value)) ? "mastercard"
                  : (/^4/.test(value)) ? "visa"
                  : (/^3[47]/.test(value)) ? 'amex'
                  : (/^6011|65|64[4-9]|622(1(2[6-9]|[3-9]\d)|[2-8]\d{2}|9([01]\d|2[0-5]))/.test(value)) ? 'discover'
                  : undefined
                ctrl.$setValidity('invalid')
                return value
            })
        }
        }
      return directive
  }
 );
