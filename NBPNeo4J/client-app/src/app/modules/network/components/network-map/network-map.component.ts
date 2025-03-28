import {Component, ElementRef, EventEmitter, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {Map, MapStyle, config, Marker} from '@maptiler/sdk';
import {NetworkService} from "../../services/network.service";
import {HubInterface} from "../../interfaces/hub.interface";
import {Subscription} from "rxjs";
import {ServiceInterface} from "../../interfaces/service.interface";


@Component({
    selector: 'app-network-map',
    templateUrl: './network-map.component.html',
    styleUrl: './network-map.component.css',
    standalone: false
})
export class NetworkMapComponent implements OnInit, OnDestroy{
  hubs: HubInterface[] = [];
  hubsSubscription: Subscription = new Subscription();

  services: ServiceInterface[] = [];
  servicesSubscription: Subscription = new Subscription();

  connectedHubs: {
    start: {longitude: number, latitude: number, id: string},
    end: {longitude: number, latitude: number, id: string}
  }[] = [];

  connectedServices: {
    start: {longitude: number, latitude: number, id: string},
    end: {longitude: number, latitude: number, id: string}
  }[] = [];

  hubsLoaded: boolean = false;



  constructor(private networkService: NetworkService) {
  }

  ngOnInit() {
    this.hubsSubscription = this.networkService.hubsChanged.subscribe((hubs: HubInterface[]) => {
      this.hubs = hubs;
      this.connectHubs(hubs);
      this.hubsLoaded = true;
      this.connectServices();
    });

    this.servicesSubscription = this.networkService.servicesChanged.subscribe(services => {
      this.services = services;
      this.connectServices();
    })

    this.networkService.getHubs();
    this.networkService.getServices();
  }

  ngOnDestroy() {
    if (this.hubsSubscription) {
      this.hubsSubscription.unsubscribe();
    }
    if (this.servicesSubscription) {
      this.servicesSubscription.unsubscribe();
    }
  }

  onHubClick(hub: HubInterface) {
    this.networkService.hubSelected.next(hub);
  }

  onServiceClick(service: ServiceInterface) {
    this.networkService.serviceSelected.next(service);
  }

  private connectHubs(hubs: HubInterface[]) {
    hubs.forEach((hub: HubInterface) => {
      hub.connectedHubs.forEach((connectedHub) => {

        const connectedHubInformation = hubs.find((hub) => hub.id === connectedHub.id);

        if(connectedHubInformation &&
        !this.connectedHubs.find((connectedHub) => connectedHub.start.id === connectedHubInformation.id && connectedHub.end.id === hub.id)) {
          this.connectedHubs.push({
            start: {
              longitude: hub.longitude,
              latitude: hub.latitude,
              id: hub.id
            },
            end: {
              longitude: connectedHubInformation.longitude,
              latitude: connectedHubInformation.latitude,
              id: connectedHub.id
            }
          });
        }

      });
    });
  }

  private connectServices() {
    if(this.hubsLoaded) {
      this.services.forEach(service => {
        const connectedHubInformation = this.hubs.find((hub) => hub.id === service.connectedToHubId);
        if (connectedHubInformation) {
          this.connectedServices.push({
            start: {
              longitude: connectedHubInformation.longitude,
              latitude: connectedHubInformation.latitude,
              id: connectedHubInformation.id
            },
            end: {
              longitude: service.longitude,
              latitude: service.latitude,
              id: service.id
            }
          });
        }
      })
    }
  }




}
