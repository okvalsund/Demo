"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
require("rxjs/add/operator/catch");
require("rxjs/add/operator/map");
var PersonService = (function () {
    function PersonService(http) {
        this.http = http;
        this.apiUrl = "http://localhost:58139/api/persons/";
    }
    PersonService.prototype.getAllPersons = function () {
        //return this.http.get('http://localhost:58139/api/persons/')
        //    .map(res => res.json());
        return this.http.get(this.apiUrl)
            .map(function (response) {
            return response.json();
        });
    };
    PersonService.prototype.getPerson = function (id) {
        //let persons;
        return this.http.get(this.apiUrl + id)
            .map(function (response) {
            console.log(response.json());
            return response.json();
        });
    };
    PersonService.prototype.addPerson = function (person) {
        //var headers = new Headers({ 'Content-Type': 'application/json' });
        //var options = new RequestOptions({ headers: headers });
        //var body = JSON.stringify(person);
        //return this.http.post(this.heroesUrl, body, options);
        return this.http.post(this.apiUrl, JSON.stringify(person), { headers: this.getHeader() })
            .map(function (response) {
            return response.json();
        });
    };
    PersonService.prototype.updatePerson = function (person) {
        this.http.post(this.apiUrl, person, { headers: this.getHeader() })
            .map(function (response) {
            return response.json();
        });
        console.log("url" + this.apiUrl + person.id);
        console.log(JSON.stringify(person));
        //return this._http.post(this._reportUrl + 'Inspect', body).map((response: Response) => <boolean>response.json());
        return this.http.put(this.apiUrl + person.id, person)
            .map(function (res) { return res.json(); });
    };
    //toPerson(data: string): Person {
    //    let jsonData = JSON.parse(data);
    //    let personData = new Person(jsonData.id, jsonData.firstName, jsonData.middleName, jsonData.lastName);
    //    return personData;
    //}
    PersonService.prototype.getHeader = function () {
        var headers = new http_1.Headers();
        headers.append('Content-Type', 'application/json');
        return headers;
    };
    return PersonService;
}());
PersonService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [http_1.Http])
], PersonService);
exports.PersonService = PersonService;
//# sourceMappingURL=person.service.js.map