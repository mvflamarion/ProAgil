import { Component, OnInit, TemplateRef } from "@angular/core";
import { EventoService } from "../_services/evento.service";
import { Evento } from "../_models/evento";
import { BsModalRef, BsModalService } from "ngx-bootstrap";
import { FormGroup, Validators, FormBuilder } from "@angular/forms";
import { defineLocale, BsLocaleService, ptBrLocale } from "ngx-bootstrap";
defineLocale("pt-br", ptBrLocale);

@Component({
  selector: "app-eventos",
  templateUrl: "./eventos.component.html",
  styleUrls: ["./eventos.component.css"]
})
export class EventosComponent implements OnInit {
  eventosFiltrados: Evento[];
  eventos: Evento[];
  evento: Evento;
  imagemLargura = 50;
  imagemMargem = 2;
  mostrarImagem = false;
  registerForm: FormGroup;
  novoRegistro = false;
  bodyDeletarEvento = "";

  _filtroLista = "";

  get filtroLista(): string {
    return this._filtroLista;
  }
  set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista
      ? this.filtrarEventos(this.filtroLista)
      : this.eventos;
  }

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private fb: FormBuilder,
    private localeService: BsLocaleService
  ) {
    this.localeService.use("pt-br");
  }

  ngOnInit() {
    this.validation();
    this.getEventos();
  }

  excluirEvento(evento: Evento, template: any) {
    this.openModal(template);
    this.evento = evento;
    this.bodyDeletarEvento = `Tem certeza que deseja excluir o Evento: ${
      evento.tema
    }, Código: ${evento.id}`;
  }

  confirmeDelete(template: any) {
    this.eventoService.deleteEvento(this.evento.id).subscribe(
      () => {
        template.hide();
        this.getEventos();
      },
      error => {
        console.log(error);
      }
    );
  }

  editarEvento(evento: Evento, template: any) {
    this.novoRegistro = false;
    this.openModal(template);
    this.evento = evento;
    this.registerForm.patchValue(evento);
  }

  novoEvento(template: any) {
    this.novoRegistro = true;
    this.openModal(template);
  }

  validation() {
    this.registerForm = this.fb.group({
      tema: [
        "",
        [Validators.required, Validators.minLength(4), Validators.maxLength(50)]
      ],
      local: ["", Validators.required],
      dataEvento: ["", Validators.required],
      qtdPessoas: [
        "",
        [Validators.required, Validators.max(100000), Validators.min(1)]
      ],
      imagemURL: ["", Validators.required],
      telefone: ["", Validators.required],
      email: ["", [Validators.required, Validators.email]]
    });
  }

  salvarAlteracao(template: any) {
    if (this.registerForm.valid) {
      if (this.novoRegistro) {
        this.evento = Object.assign({}, this.registerForm.value);
        this.eventoService.postEvento(this.evento).subscribe(
          (novoEvento: Evento) => {
            console.log(novoEvento);
            template.hide();
            this.getEventos();
          },
          error => console.log(error)
        );
      }

      this.evento = Object.assign(
        { id: this.evento.id },
        this.registerForm.value
      );
      this.eventoService.putEvento(this.evento).subscribe(
        () => {
          template.hide();
          this.getEventos();
        },
        error => console.log(error)
      );
    }
  }

  openModal(template: any) {
    this.registerForm.reset();
    template.show(template);
  }

  filtrarEventos(filtrarPor: string): Evento[] {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      evento => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );
  }

  alternarImagem() {
    this.mostrarImagem = !this.mostrarImagem;
  }

  getEventos() {
    this.eventoService.getAllEventos().subscribe(
      (_eventos: Evento[]) => {
        this.eventos = _eventos;
        this.eventosFiltrados = this.eventos;
      },
      error => {
        console.log(error);
      }
    );
  }
}
