import {Component, OnDestroy, OnInit} from '@angular/core';
import {NetworkService} from "../../services/network.service";
import {HubInterface} from "../../interfaces/hub.interface";
import {NgForm} from "@angular/forms";
import {ServiceCreateInterface} from "../../interfaces/service-create.interface";
import {Subscription} from "rxjs";

@Component({
    selector: 'app-create-service',
    templateUrl: './create-service.component.html',
    styleUrl: './create-service.component.css',
    standalone: false
})
export class CreateServiceComponent implements OnInit, OnDestroy{
  isLoading: boolean = false;
  existingHubs: {
    name: string;
    id: string;
    checked: boolean;
  }[] = [];
  selectedHubId: string = '';

  existingServicesSubscription: Subscription = new Subscription();

  constructor(
    private networkService: NetworkService
  ){}


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
    this.networkService.createService(service);
  }


}
