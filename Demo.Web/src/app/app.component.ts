import { Component } from '@angular/core'
import { Person } from '../entity/entities'
import { PersonService } from '../service/person.service'
import { PersonNamePipe } from './personName.pipe'

@Component({
  selector: 'my-app',
  templateUrl: './app.component.html'
})
export class AppComponent
{
    searchCriteria: string;
    persons: Person[] = [];
    selectedPerson: Person;
    updateMessage: string;;

    constructor(private personService: PersonService) {
        this.personService.getAllPersons()
            .subscribe(data => this.persons = data);
    }

    onSelect(person: Person): void {
        this.selectedPerson = person;
        this.updateMessage = "";
        console.log("person selected" + person.firstName);
    }

    search() {
        this.updateMessage = "";
        this.selectedPerson = null;
        console.log("search pressed");
    }

    update() {
        this.personService.getPerson(this.selectedPerson.id);
        this.personService.updatePerson(this.selectedPerson);
        setTimeout(() => this.updateMessage = "", 3000);
    }
}
