import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NetworkDisplayComponent } from './components/network-display/network-display.component';
import { NetworkMapComponent } from './components/network-map/network-map.component';
import { CreateHubComponent } from './components/create-hub/create-hub.component';
import { CreateServiceComponent } from './components/create-service/create-service.component';
import { HubDetailsComponent } from './components/hub-details/hub-details.component';
import {NetworkRoutingModule} from "./network-routing.module";
import {MatButtonModule} from "@angular/material/button";
import {MatCardModule} from "@angular/material/card";
import {MatCheckboxModule} from "@angular/material/checkbox";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatGridListModule} from "@angular/material/grid-list";
import {MatInputModule} from "@angular/material/input";
import {MatProgressBarModule} from "@angular/material/progress-bar";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {HttpClientModule} from "@angular/common/http";
import {MatRadioModule} from "@angular/material/radio";



@NgModule({
  declarations: [
    NetworkDisplayComponent,
    NetworkMapComponent,
    CreateHubComponent,
    CreateServiceComponent,
    HubDetailsComponent
  ],
  imports: [
    CommonModule,
    NetworkRoutingModule,
    MatButtonModule,
    MatCardModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatGridListModule,
    MatInputModule,
    MatProgressBarModule,
    FormsModule,
    ReactiveFormsModule,
    MatRadioModule
  ]
})
export class NetworkModule { }
