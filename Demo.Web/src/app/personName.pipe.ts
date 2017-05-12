import { Pipe, PipeTransform } from '@angular/core';
import { Person } from '../entity/entities'

@Pipe({
    name: 'personNamePipe'
})

export class PersonNamePipe implements PipeTransform {
    transform(person: Person): string {
        let fullName = person.firstName + " " + person.middleName + " " + person.lastName;
        fullName = fullName.replace("  ", " ");
        return name;
    }
}

