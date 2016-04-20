app.service('myService', ['$http', function ($http) {


    this.defaultAPI = function () {
        var urlBase = 'https://iservices.earlymoments.com/visitorRegistration?format=json&callback=JSON_CALLBACK';
        return $http.jsonp(urlBase);
    };
    this.shippingAPI = function (data) {
        var urlBase = 'https://iservices.earlymoments.com/createOrder?format=json&callback=JSON_CALLBACK';
        var c = this.getPreferences(data[0].shippingdata.checkbox1, data[0].shippingdata.checkbox2);

        console.log("Shipping service call:");
        console.log(data);

        var inputdata = {
            'Token': '',
            'Order': {
                'Customer': {
                    'Billing': {
                        'FirstName': data[0].shippingdata.firstname,
                        'Lastname': data[0].shippingdata.lastname,
                        'Address1': data[0].shippingdata.streetaddress,
                        'Address2': data[0].shippingdata.address2,
                        'City': data[0].shippingdata.city,
                        'State': data[0].shippingdata.state,
                        'Zipcode': data[0].shippingdata.zipcode,
                        'Country': ''
                    },
                    'Shipping': {
                        'FirstName': data[0].shippingdata.firstname,
                        'Lastname': data[0].shippingdata.lastname,
                        'Address1': data[0].shippingdata.streetaddress,
                        'Address2': data[0].shippingdata.address2,
                        'City': data[0].shippingdata.city,
                        'State': data[0].shippingdata.state,
                        'Zipcode': data[0].shippingdata.zipcode,
                        'Country': ''
                    },
                    'EmailAddress': data[0].shippingdata.email,
                    'Preference': c,
                    'Phone': data[0].shippingdata.phone
                },
                'ChildInfo': [{
                    'Name': data[0].shippingdata.childname,
                    'Gender': data[0].shippingdata.gender,
                    'DateOfBirth': data[0].shippingdata.childdob,
                    'NickName': ''
                }],
                'ItemsToAdd': [{
                    'OfferCode': '19632366',
                    'Quantity': 1
                },
                {
                    'OfferCode': '19632367',
                    'Quantity': 1
                }],
                //'ItemsToRemove': [{
                //    'OfferCode': '',
                //    'Quantity': 0
                //}],
                //'Payment': {
                //    'PaymentType': '',
                //    'CreditCard': [{
                //        'CardNumber': '',
                //        'ExpirationDate': '',
                //        'SecurityCode': ''
                //    }]
                //},
                'IsBonusSelected': false,
                'Voucher': '',
                'OrderId': 0
            }
        };
        //var str = JSON.stringify(inputdata);
        //console.log(str);
        //var result = $.param(inputdata);
        //console.log(result);
        //console.log("inputdata");
        //console.log(inputdata);

        var outputdata = $http.jsonp(encodeURI(urlBase), { params: inputdata });
        return outputdata;
    };

    this.billingAPI = function (data) {
        var urlBase = 'https://iservices.earlymoments.com/updateOrder?format=json&callback=JSON_CALLBACK';
        var c = this.getPreferences(data[0].shippingdata.checkbox1, data[0].shippingdata.checkbox2);

        var cardtype = this.getCreditCardType(data[0].billingdata.cardnumber);
        console.log(cardtype);
        
        console.log("Billing service call:");
        console.log(data);
        

        var inputdata = {
            "Token": "",
            "Order": {
                "Customer": {
                    "Billing": {
                        "FirstName": data[0].billingdata.firstname,
                        "Lastname": data[0].billingdata.lastname,
                        "Address1": data[0].billingdata.streetaddress,
                        "Address2": data[0].billingdata.address2,
                        "City": data[0].billingdata.city,
                        "State": data[0].billingdata.state,
                        "Zipcode": data[0].billingdata.zipcode,
                        "Country": ''
                    },
                    "Shipping": {
                        "FirstName": data[0].shippingdata.firstname,
                        "Lastname": data[0].shippingdata.lastname,
                        "Address1": data[0].shippingdata.streetaddress,
                        "Address2": data[0].shippingdata.address2,
                        "City": data[0].shippingdata.city,
                        "State": data[0].shippingdata.state,
                        "Zipcode": data[0].shippingdata.zipcode,
                        "Country": ''
                    },
                    "EmailAddress": data[0].shippingdata.email,
                    "Preference": c,
                    "Phone": data[0].shippingdata.phone
                },
                "ChildInfo": [{
                    "Name": data[0].shippingdata.childname,
                    "Gender": data[0].shippingdata.gender,
                    "DateOfBirth": data[0].shippingdata.childdob,
                    "NickName": ""
                }],
                "ItemsToAdd": [{
                    "OfferCode": "",
                    "Quantity": 0
                }],
                "ItemsToRemove": [{
                    "OfferCode": "",
                    "Quantity": 0
                }],
                "Payment": {
                    "PaymentType": 'CC',
                    "CreditCard": [{
                        "CardNumber": data[0].billingdata.cardnumber,
                        "ExpirationDate": data[0].billingdata.expmonth + "/" + data[0].billingdata.expyear,
                        "SecurityCode": data[0].billingdata.cvv
                    }]
                },
                "IsBonusSelected": false,
                "Voucher": "",
                "OrderId": 0
            }
        };
        console.log(inputdata);
        var outputdata = $http.jsonp(encodeURI(urlBase), { params: inputdata });
        return outputdata;
    };

    this.getPreferences = function (c1, c2) {
        var c = "";
        if (c1 == 'Y' && c2 == 'Y')
            c = "";
        else if (c1 == 'Y' && c2 == 'N')
            c = "5";
        else if (c1 == 'N' && c2 == 'Y')
            c = "7";
        else if (c1 == 'N' && c2 == 'N')
            c = "6";
        return c;
    };

    this.getCreditCardType = function (accountNumber) {
        //start without knowing the credit card type
        var result = "unknown";
        //first check for MasterCard
        if (/^5[1-5]/.test(accountNumber)) {
            result = "mastercard";
        }
        //then check for Visa
        else if (/^4/.test(accountNumber)) {
            result = "visa";
        }
        //then check for AmEx
        else if (/^3[47]/.test(accountNumber)) {
            result = "amex";
        }
            //then check for discover
        else if ((/^6011|65|64[4-9]|622(1(2[6-9]|[3-9]\d)|[2-8]\d{2}|9([01]\d|2[0-5]))/.test(accountNumber))) {
            result = "discover";
        }
        return result;
    };


}]);