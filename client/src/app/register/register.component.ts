import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model:any={};
  registerForm : FormGroup;
  maxDate : Date;
  validationErrors :string[]=[];
  constructor(private accountService: AccountService, private toastr : ToastrService,
    private fb:FormBuilder,private router : Router) { }
  //@Input() usersFromHomeComponent;
  @Output() cancelRegister = new EventEmitter();

  ngOnInit(): void {
    this.initializeFBForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear()-18);
  }

  initializeForm(){
    this.registerForm = new FormGroup({
      usernName : new FormControl('',Validators.required),
      password : new FormControl('',[Validators.required,Validators.minLength(5),Validators.maxLength(8)]),
      confirmPassword : new FormControl('',[Validators.required,this.matchValues('password')])
        })
    this.registerForm.controls.password.valueChanges.subscribe(()=>{
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    })
  }
  initializeFBForm(){
    this.registerForm = this.fb.group({
      userName :['',Validators.required],
      gender :['male'],
      knownAs :['',Validators.required],
      dateOfBirth :['',Validators.required],
      city :['',Validators.required],
      country :['',Validators.required],
      password : ['',[Validators.required,
        Validators.minLength(5),Validators.maxLength(8)]],
      confirmPassword : ['',[Validators.required,
        this.matchValues('password')]]
        })
    this.registerForm.controls.password.valueChanges.subscribe(()=>{
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    })
  }

  matchValues(matchTo : string): ValidatorFn{

    return (control : AbstractControl) => {
      return control?.value=== control?.parent?.controls[matchTo].value ? null:{isMatching : true};
    }
  }

  register(){
   // console.log(this.registerForm.value);
    this.accountService.register(this.registerForm.value).subscribe(response=>
      {
      this.router.navigateByUrl('/members');
      //this.cancel();
      },
      error=>{console.log(error);
       // this.toastr.error(error.error);
       this.validationErrors = error;
      }
      )
  }

  cancel(){
    this.cancelRegister.emit(false);
  }



}
