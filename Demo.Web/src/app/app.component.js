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
var person_service_1 = require("../service/person.service");
var AppComponent = (function () {
    function AppComponent(personService) {
        var _this = this;
        this.personService = personService;
        this.persons = [];
        this.personService.getAllPersons()
            .subscribe(function (data) { return _this.persons = data; });
    }
    ;
    AppComponent.prototype.onSelect = function (person) {
        this.selectedPerson = person;
        this.updateMessage = "";
        console.log("person selected" + person.firstName);
    };
    AppComponent.prototype.search = function () {
        this.updateMessage = "";
        this.selectedPerson = null;
        console.log("search pressed");
    };
    AppComponent.prototype.update = function () {
        var _this = this;
        this.personService.getPerson(this.selectedPerson.id);
        this.personService.updatePerson(this.selectedPerson);
        setTimeout(function () { return _this.updateMessage = ""; }, 3000);
    };
    return AppComponent;
}());
AppComponent = __decorate([
    core_1.Component({
        selector: 'my-app',
        templateUrl: './app.component.html'
    }),
    __metadata("design:paramtypes", [person_service_1.PersonService])
], AppComponent);
exports.AppComponent = AppComponent;
//# sourceMappingURL=app.component.js.map