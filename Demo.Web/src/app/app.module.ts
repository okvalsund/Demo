import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule, JsonpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { PersonService } from '../service/person.service'
//import { AddressService } from '../service/address.service'
//import { EmailService } from '../service/email.service'

@NgModule({
    imports: [BrowserModule, FormsModule, HttpModule, JsonpModule],
    declarations: [AppComponent],
    providers: [PersonService],
    bootstrap: [AppComponent]
})
export class AppModule { }
