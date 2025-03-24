import {Component, OnChanges, OnDestroy, OnInit} from '@angular/core';
import {NetworkService} from "../../services/network.service";
import {ActivatedRoute} from "@angular/router";
import {Subscription} from "rxjs";
import {HubInterface} from "../../interfaces/hub.interface";
import {NgForm} from "@angular/forms";
import {HubCreateInterface} from "../../interfaces/hub-create.interface";

@Component({
  selector: 'app-update-hub',
  standalone: false,
  templateUrl: './update-hub.component.html',
  styleUrl: './update-hub.component.css'
})
export class UpdateHubComponent implements OnInit, OnDestroy{
  isLoading: boolean = false;
  existingHubs: {
    name: string;
    id: string;
    checked: boolean;
  }[] = [];

  existingHubsSubscription: Subscription = new Subscription();

  hubToUpdate: HubInterface | null = null;
  hubToUpdateId: string = '';

  nameInput: string = '';
  cityInput: string = '';
  addressInput: string = '';
  longitudeInput: number = 0;
  latitudeInput: number = 0;



  constructor(
    private networkService: NetworkService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.hubToUpdateId = this.route.snapshot.params['id'];

    this.existingHubsSubscription = this.networkService.hubsChanged.subscribe((hubs) => {
      this.existingHubs = hubs.filter(hub => {
        return hub.id !== this.hubToUpdateId;
      }).map((hub: HubInterface) => {
        return {
          name: hub.name,
          id: hub.id,
          checked: false
        }
      })
    })
    this.networkService.getHubs();


    this.route.params.subscribe((params) => {
      this.hubToUpdateId = params['id'];
      this.hubToUpdate = this.networkService.getHub(this.hubToUpdateId);
      if(this.hubToUpdate) {
        this.nameInput = this.hubToUpdate.name;
        this.cityInput = this.hubToUpdate.city;
        this.addressInput = this.hubToUpdate.address;
        this.longitudeInput = this.hubToUpdate.longitude;
        this.latitudeInput = this.hubToUpdate.latitude;
        this.existingHubs = this.existingHubs.map((hub) => {
          return {
            ...hub,
            checked: this.hubToUpdate.connectedHubs.some((connectedHub) => connectedHub.id === hub.id)
          }
        });
      }
    });
  }

  ngOnDestroy(): void {
    if (this.existingHubsSubscription) {
      this.existingHubsSubscription.unsubscribe();
    }
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
    this.networkService.updateHub(hub, this.hubToUpdateId);
  }


}
