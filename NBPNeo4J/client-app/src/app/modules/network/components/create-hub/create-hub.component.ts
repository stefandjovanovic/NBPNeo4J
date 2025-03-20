import { Component } from '@angular/core';
import {NetworkService} from "../../services/network.service";
import {Router} from "@angular/router";
import {NgForm} from "@angular/forms";

@Component({
  selector: 'app-create-hub',
  templateUrl: './create-hub.component.html',
  styleUrl: './create-hub.component.css'
})
export class CreateHubComponent {
  constructor(
    private networkService: NetworkService,
    private router: Router
  ){}

  isLoading: boolean = false;

  onSubmit(form: NgForm) {


  }



}
