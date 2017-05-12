import { Injectable } from "@angular/core";
import { Http, Response, Headers, RequestOptions } from "@angular/http";
import { Observable } from "rxjs/Observable";
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';

import { Person, Email, Address } from "../entity/entities";

@Injectable()
export class PersonService {
    private apiUrl = "http://localhost:58139/api/persons/";

    constructor(private http: Http) { }

    getAllPersons(): Observable<Person[]> {

        //return this.http.get('http://localhost:58139/api/persons/')
        //    .map(res => res.json());

        return this.http.get(this.apiUrl)
            .map((response: Response) => {
                return <Person[]>response.json()
            });
    }

    getPerson(id: number): Observable<Person> {
        //let persons;
        return this.http.get(this.apiUrl + id)
            .map((response: Response) => {
                console.log(response.json());
                return <Person>response.json();
            });
    }

    addPerson(person: Person) : Observable<Person> {
        //var headers = new Headers({ 'Content-Type': 'application/json' });
        //var options = new RequestOptions({ headers: headers });
        //var body = JSON.stringify(person);
        //return this.http.post(this.heroesUrl, body, options);


        return this.http.post(this.apiUrl, JSON.stringify(person), { headers: this.getHeader() })
            .map((response: Response) => {
                return <Person>response.json();
            });
    }

    updatePerson(person: Person) {
        this.http.post(this.apiUrl, person, { headers: this.getHeader() })
            .map((response: Response) => {
                return <Person>response.json();
            });
        console.log("url"+ this.apiUrl + person.id);
        console.log(JSON.stringify(person));
        //return this._http.post(this._reportUrl + 'Inspect', body).map((response: Response) => <boolean>response.json());
        return this.http.put(this.apiUrl + person.id, person)
            .map(res => res.json());


        
    }

    //toPerson(data: string): Person {
    //    let jsonData = JSON.parse(data);

    //    let personData = new Person(jsonData.id, jsonData.firstName, jsonData.middleName, jsonData.lastName);
    //    return personData;
    //}

    getHeader(): Headers {
        let headers = new Headers();
        headers.append('Content-Type', 'application/json');
        return headers;
    }
    //toPerson(r: any): Person {
    //    let person: ({
    //        id: r.id,
    //        firstName: r.firstName,
    //        middleName: r.middleName,
    //        lastName: r.lastName
    //    });

    //    return person;
    //}

    //getArchiveFileEntries(pk: number): Observable<IArchiveFileEntry[]> {
    //    return this._http.get(this._reportUrl + "Documents/" + pk).map(mapArchiveFileEntries);
    //    // return this._http.get(this._reportUrl + "Documents/" + pk).map((response: Response) => <IArchiveFileEntry[]>response.json());

    // getArchiveFilterContainer(): Observable<IArchiveFilterContainer>
    //{ return this._http.get(this._reportUrl + "Filter").map((response: Response) => <IArchiveFilterContainer>response.json());
    //getHeroesSlowly(): Promise<Hero[]> {
    //    return new Promise(resolve => {
    //        // Simulate server latency with 2 second delay
    //        setTimeout(() => resolve(this.getHeroes()), 2000);
    //    });
    //}
}