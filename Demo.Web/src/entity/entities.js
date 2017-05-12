//import { Component } from "@angular/core";
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
//@Component({
//    selector: "interfaces"
//})
//export class Interfaces {
//}
var Address = (function () {
    function Address(id, personId, street, zipCode, city) {
        this.id = id;
        this.personId = personId;
        this.street = street;
        this.zipCode = zipCode;
        this.city = city;
    }
    return Address;
}());
exports.Address = Address;
var Email = (function () {
    function Email(id, personId, emailAddress) {
        this.id = id;
        this.personId = personId;
        this.emailAddress = emailAddress;
    }
    return Email;
}());
exports.Email = Email;
var Person = (function () {
    function Person(id, firstName, middleName, lastName) {
        this.id = id;
        this.firstName = firstName;
        this.middleName = middleName;
        this.lastName = lastName;
    }
    return Person;
}());
exports.Person = Person;
//# sourceMappingURL=entities.js.map