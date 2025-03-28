import {Component, OnDestroy, OnInit} from '@angular/core';
import {NetworkService} from "../../services/network.service";
import {Subscription} from "rxjs";

@Component({
    selector: 'app-network-display',
    templateUrl: './network-display.component.html',
    styleUrl: './network-display.component.css',
    standalone: false
})
export class NetworkDisplayComponent implements OnInit, OnDestroy {
    displayService: boolean = false;
    displayHub: boolean = false;

    hubSubscription: Subscription = new Subscription();
    serviceSubscription: Subscription = new Subscription();

    constructor(private networkService: NetworkService) {
    }

    ngOnInit() {
      this.hubSubscription = this.networkService.hubSelected.subscribe((hub) => {
        if(hub) {
          this.displayHub = true;
          this.displayService = false;
        } else {
          this.displayHub = false;
          this.displayService = false;
        }
      })
      this.serviceSubscription = this.networkService.serviceSelected.subscribe((service) => {
        if(service) {
          this.displayHub = false;
          this.displayService = true;
        } else {
          this.displayHub = false;
          this.displayService = false;
        }
      })
    }

    ngOnDestroy() {
        this.hubSubscription.unsubscribe();
        this.serviceSubscription.unsubscribe();
    }

}
