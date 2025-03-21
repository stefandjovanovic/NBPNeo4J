import {Component, OnDestroy, OnInit} from '@angular/core';
import {NetworkService} from "../../services/network.service";
import {Router} from "@angular/router";
import {NgForm} from "@angular/forms";
import {HubInterface} from "../../interfaces/hub.interface";
import {HubCreateInterface} from "../../interfaces/hub-create.interface";

@Component({
    selector: 'app-create-hub',
    templateUrl: './create-hub.component.html',
    styleUrl: './create-hub.component.css',
    standalone: false
})
export class CreateHubComponent implements OnInit, OnDestroy{
  isLoading: boolean = false;
  existingHubs: {
    name: string;
    id: string;
    checked: boolean;
  }[] = [];

  constructor(
    private networkService: NetworkService
  ){}


  ngOnInit() {
    this.networkService.hubsChanged.subscribe((hubs) => {
      this.existingHubs = hubs.map((hub: HubInterface) => {
        return {
          name: hub.name,
          id: hub.id,
          checked: false
        }
      })
    })
    this.networkService.getHubs();
  }

  ngOnDestroy() {
    this.networkService.hubsChanged.unsubscribe();
  }


  onSubmit(form: NgForm) {
    this.isLoading = true;
    const hub: HubCreateInterface = {
      name: form.value.name,
      city: form.value.city,
      address: form.value.address,
      longitude: form.value.longitude,
      latitude: form.value.latitude,
      connectedHubsIds: this.existingHubs.filter((hub) => hub.checked).map((hub) => hub.id)
    }
    this.networkService.createHub(hub);
  }



}
