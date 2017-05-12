//import { Component } from "@angular/core";

//@Component({
//    selector: "interfaces"
//})

//export class Interfaces {

//}

export class Address {
    id: number;
    personId: number;
    street: string;
    zipCode: number;
    city: string;

    constructor(id: number, personId: number, street: string, zipCode: number, city: string) {
        this.id = id;
        this.personId = personId;
        this.street = street;
        this.zipCode = zipCode;
        this.city = city;
    }
}

export class Email {
    id: number;
    personId: number;
    emailAddress: string;

    constructor(id: number, personId: number, emailAddress: string) {
        this.id = id;
        this.personId = personId;
        this.emailAddress = emailAddress;
    }

}

export class Person {
    id: number;
    firstName: string;
    middleName: string;
    lastName: string;

    constructor(id: number, firstName: string, middleName: string, lastName: string) {
        this.id = id;
        this.firstName = firstName;
        this.middleName = middleName;
        this.lastName = lastName;
    }
}