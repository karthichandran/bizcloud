import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { FuseConfigService } from '@fuse/services/config.service';
import { fuseAnimations } from '@fuse/animations';
import { LoginService } from '../login/login.service';

//import { AuthenticationService } from 'app/core/authentication/authentication.service';
//import { NotificationService } from 'app/core/notification/notification.service';

@Component({
    selector     : 'login',
    templateUrl  : './login.component.html',
    styleUrls    : ['./login.component.scss'],
    encapsulation: ViewEncapsulation.None,
    animations   : fuseAnimations
})
export class LoginComponent implements OnInit
{
  isLogin: boolean = true;
  loginForm: UntypedFormGroup;
  forgotPasswordForm: UntypedFormGroup;
    /**
     * Constructor
     *
     * @param {FuseConfigService} _fuseConfigService
     * @param {FormBuilder} _formBuilder
     */
    constructor(
      private _fuseConfigService: FuseConfigService,
      private _formBuilder: UntypedFormBuilder,
      private loginServ: LoginService,
      //private authenticationService: AuthenticationService,
      private toastr: ToastrService,
      private router: Router
    )
    {
        // Configure the layout
        this._fuseConfigService.config = {
            layout: {
                navbar   : {
                    hidden: true
                },
                toolbar  : {
                    hidden: true
                },
                footer   : {
                    hidden: true
                },
                sidepanel: {
                    hidden: true
                }
            }
        };
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void
    {
        this.loginForm = this._formBuilder.group({          
            user   : ['', Validators.required],
          password: ['', Validators.required],
          rememberMe:['']
        });
      this.forgotPasswordForm = this._formBuilder.group({
        user: ['', Validators.required],
        email: ['', Validators.email]
      });
      this.RememberMeEnabled();
    }

  RememberMeEnabled() {
    if (localStorage.getItem("Credentials") != "" && localStorage.getItem("Credentials") !=null) {
      this.loginServ.loginWithSavedCredentials().subscribe(res => {
        this.router.navigate(['/client']);
      });
    }
  }

  login(): void {
  //  this.loginServ.refreshToken();
      this.loginForm.disable();
      let value = this.loginForm.value;
      this.loginServ.login(value.user, value.password, (value.rememberMe == true) ? true : false )
            .pipe(finalize(() => {
                this.loginForm.reset();
                this.loginForm.markAsPristine();
                this.loginForm.enable();
            })).subscribe((data: any) => {
            this.router.navigate(['/client']);
            });
  }

  forgotPassword(): void {
    if (this.forgotPasswordForm.valid) {
      let value = this.forgotPasswordForm.value;
      let model = { "userName": value.user, 'email': value.email };
      this.loginServ.forgotPassword(model).subscribe(res => {
        this.toastr.success("Login Details are sent to your email address.");
        this.isLogin = true;
        this.forgotPasswordForm.reset();
      });

    }
    else {
      this.forgotPasswordForm.markAsDirty();
    }    
  }

  cancel(): void {
    this.isLogin = true;
    this.forgotPasswordForm.reset();
  }
}
