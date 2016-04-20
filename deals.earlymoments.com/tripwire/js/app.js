'use strict';

var app = angular.module('AngularStore', ['ngRoute','ngCookies']);
app.config(['$routeProvider', function ($routeProvider, $locationProvider) {
     $routeProvider.
         when('/', {
             templateUrl: 'partials/home.html',
             controller: 'homeController'
         }).
         when('/home', {
             templateUrl: 'partials/home.html',
             controller: 'homeController'
         }).
         when('/shipping', {
             templateUrl: 'partials/shipping.html',
             controller: 'shippingController'
         }).
          when('/upsell-offer1', {
              templateUrl: 'partials/upsell-offer1.html',
              controller: 'upsellOffer1Controller'
          }).
           when('/upsell-offer2', {
               templateUrl: 'partials/upsell-offer2.html',
               controller: 'upsellOffer2Controller'
           }).
           when('/thankyou', {
               templateUrl: 'partials/thankyou.html',
               controller: 'thankyouController'
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

app.directive('modal', function () {
    return {
        template: '<div class="modal fade errorModal">' +
            '<div class="modal-dialog">' +
              '<div class="modal-content">' +
                '<div class="modal-header">' +
                  '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>' +
                  '<h4 class="modal-title">{{ title }}</h4>' +
                '</div>' +
                '<div class="modal-body" ng-transclude></div>' +
              '</div>' +
            '</div>' +
          '</div>',
        restrict: 'E',
        transclude: true,
        replace: true,
        scope: true,
        link: function postLink(scope, element, attrs) {
            scope.title = attrs.title;

            scope.$watch(attrs.visible, function (value) {
                if (value == true)
                    $(element).modal('show');
                else
                    $(element).modal('hide');
            });

            $(element).on('shown.bs.modal', function () {
                scope.$apply(function () {
                    scope.$parent[attrs.visible] = true;
                });
            });

            $(element).on('hidden.bs.modal', function () {
                scope.$apply(function () {
                    scope.$parent[attrs.visible] = false;
                });
            });
        }
    };
});

app.directive('creditCardType', function () {
      var directive =
        {
            require: 'ngModel'
        , link: function (scope, elm, attrs, ctrl) {
            ctrl.$parsers.unshift(function (value) {
                scope.billform.type =
                  (/^5[1-5]/.test(value)) ? "mastercard"
                  : (/^4/.test(value)) ? "visa"
                  : (/^3[47]/.test(value)) ? 'amex'
                  : (/^6011|65|64[4-9]|622(1(2[6-9]|[3-9]\d)|[2-8]\d{2}|9([01]\d|2[0-5]))/.test(value)) ? 'discover'
                  : undefined
                ctrl.$setValidity('invalid', !!scope.billform.type)
                return value;
            })
        }
        }
      return directive;
});

app.directive
  ('cardExpiration'
  , function () {
      var directive =
        {
            require: 'ngModel'
        , link: function (scope, elm, attrs, ctrl) {
            scope.$watch('[billform.expmonth,billform.expyear]', function (value) {
                ctrl.$setValidity('invalid', true)
                if (scope.billform.expyear == scope.currentYear
                     && scope.billform.expmonth <= scope.currentMonth
                   ) {
                    ctrl.$setValidity('invalid', false)
                }
                return value
            }, true)
        }
        }
      return directive;
  });

//Factory
app.factory('MyFactory', function () {
    var factory = {};
    factory.getGUID = function() {
        var d = new Date().getTime();
        var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = (d + Math.random() * 16) % 16 | 0;
            d = Math.floor(d / 16);
            return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
        return uuid;
    };
    factory.getCookies = function () {
        var pairs = document.cookie.split(';');
        var cookies = {}, name, value;
        for (var i = 0; i < pairs.length; i++) {
            //var pair = (pairs[i].split("="));
            //cookies[pair[0].trim()] = unescape(pair[1]);
            name = pairs[i].split('=')[0].trim();
            value = (pairs[i].split('=')[1]);
            cookies['"'+name+'"'] = value;
        }
        return cookies;
    };
    factory.checkGUIDinCookies = function () {
        var myObj = this.getCookies();
        if ('"guid"' in myObj)
            return 1;
        else
            return 0;
    };
    

    return factory;
});