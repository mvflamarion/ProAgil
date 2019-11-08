import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { HttpClientModule } from "@angular/common/http";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import {
  BsDropdownModule,
  TooltipModule,
  ModalModule,
  BsDatepickerModule
} from "ngx-bootstrap";
import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { EventosComponent } from "./eventos/eventos.component";
import { NavComponent } from "./nav/nav.component";
import { DateTimeFormatPipePipe } from "./_helps/DateTimeFormatPipe.pipe";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { ToastrModule } from "ngx-toastr";
import { PalestrantesComponent } from "./palestrantes/palestrantes.component";
import { DashboardComponent } from "./dashboard/dashboard.component";
import { ContatosComponent } from "./contatos/contatos.component";
import { TituloComponent } from "./_shared/titulo/titulo.component";

@NgModule({
  declarations: [
    AppComponent,
    EventosComponent,
    NavComponent,
    DateTimeFormatPipePipe,
    PalestrantesComponent,
    DashboardComponent,
    ContatosComponent,
    TituloComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    BsDropdownModule.forRoot(),
    TooltipModule.forRoot(),
    ModalModule.forRoot(),
    BsDatepickerModule.forRoot(),
    ReactiveFormsModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      timeOut: 3000,
      positionClass: "toast-top-right",
      preventDuplicates: true
    })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {}
