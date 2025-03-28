import {Component, OnDestroy, OnInit} from '@angular/core';
import {NetworkService} from "../../services/network.service";
import {Subscription} from "rxjs";
import {ServiceInterface} from "../../interfaces/service.interface";
import {HubInterface} from "../../interfaces/hub.interface";
import {ActivatedRoute} from "@angular/router";
import {NgForm} from "@angular/forms";
import {ServiceCreateInterface} from "../../interfaces/service-create.interface";

@Component({
  selector: 'app-update-service',
  standalone: false,
  templateUrl: './update-service.component.html',
  styleUrl: './update-service.component.css'
})
export class UpdateServiceComponent implements OnInit, OnDestroy{
  isLoading: boolean = false;
  existingHubs: {
    name: string;
    id: string;
    checked: boolean;
  }[] = [];
  selectedHubId: string = '';

  existingServicesSubscription: Subscription = new Subscription();

  serviceToUpdateId: string = '';
  serviceToUpdate: ServiceInterface | null = null;

  nameInput: string = '';
  cityInput: string = '';
  addressInput: string = '';
  longitudeInput: number = 0;
  latitudeInput: number = 0;


  constructor(private networkService: NetworkService, private route: ActivatedRoute) {
  }

  ngOnInit() {
    this.existingServicesSubscription = this.networkService.hubsChanged.subscribe((hubs) => {
      this.existingHubs = hubs.map((hub: HubInterface) => {
        return {
          name: hub.name,
          id: hub.id,
          checked: false
        }
      })
    })
    this.networkService.getHubs();

    this.serviceToUpdateId = this.route.snapshot.params['id'];

    this.route.params.subscribe((params) => {
      this.serviceToUpdateId = params['id'];
      this.serviceToUpdate = this.networkService.getService(this.serviceToUpdateId);
      if(this.serviceToUpdate) {
        this.nameInput = this.serviceToUpdate.name;
        this.cityInput = this.serviceToUpdate.city;
        this.addressInput = this.serviceToUpdate.address;
        this.longitudeInput = this.serviceToUpdate.longitude;
        this.latitudeInput = this.serviceToUpdate.latitude;
        this.selectedHubId = this.serviceToUpdate.connectedToHubId;
      }
    });
  }

  ngOnDestroy() {
    if(this.existingServicesSubscription) {
      this.existingServicesSubscription.unsubscribe();
    }
  }

  onSubmit(form: NgForm) {
    this.isLoading = true;
    const service: ServiceCreateInterface = {
      name: form.value.name,
      city: form.value.city,
      address: form.value.address,
      longitude: form.value.longitude,
      latitude: form.value.latitude,
      hubId: this.selectedHubId
    }
    this.networkService.updateService(service, this.serviceToUpdateId);
  }


}
