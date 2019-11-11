import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/_models/User';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {
  registerForm: FormGroup;
  user: User;

  constructor(
    public fb: FormBuilder,
    public router: Router,
    private authService: AuthService,
    private toastr: ToastrService
  ) {}

  ngOnInit() {
    this.validation();
  }

  validation() {
    this.registerForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      userName: ['', Validators.required],
      passwords: this.fb.group(
        {
          password: ['', [Validators.required, Validators.minLength(4)]],
          confirmPassword: ['', Validators.required]
        },
        { validator: this.compararSenhas }
      )
    });
  }

  compararSenhas(fb: FormGroup) {
    const confirmPwd = fb.get('confirmPassword');
    if (confirmPwd.errors == null || 'mismatch' in confirmPwd.errors) {
      if (fb.get('password').value !== confirmPwd.value) {
        confirmPwd.setErrors({ mismatch: true });
      } else {
        confirmPwd.setErrors(null);
      }
    }
  }

  cadastrarUsuario() {
    if (!this.registerForm.valid) return;

    this.user = Object.assign(
      { password: this.registerForm.get('passwords.password').value },
      this.registerForm.value
    );

    this.authService.register(this.user).subscribe(
      () => {
        this.toastr.success('Cadastro Realizado.');
        this.router.navigate(['/user/login']);
      },
      (error: any) => {
        if (error.status == 500) {
          this.toastr.error(
            'Erro ao cadastrar. Tente novamente em alguns instantes.'
          );
        } else {
          const erro = error.error;
          erro.forEach(e => {
            switch (e.code) {
              case 'DuplicateUserName':
                this.toastr.error('Usuário já Existe!');
                break;
              case 'DuplicateEmail':
                this.toastr.error('Email já Existe!');
                break;

              default:
                this.toastr.error(`Erro no cadastro! CODE: ${e.code}`);
                break;
            }
          });
        }
      }
    );
  }
}
