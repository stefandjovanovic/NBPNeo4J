import {Component, ElementRef, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {Map, MapStyle, config, Marker} from '@maptiler/sdk';
import {NetworkService} from "../../services/network.service";
import {HubInterface} from "../../interfaces/hub.interface";
import {Subscription} from "rxjs";


@Component({
    selector: 'app-network-map',
    templateUrl: './network-map.component.html',
    styleUrl: './network-map.component.css',
    standalone: false
})
export class NetworkMapComponent implements OnInit, OnDestroy{
  map: Map | undefined;
  hubs: HubInterface[] = [];
  hubsSubscription: Subscription = new Subscription();

  connectedHubs: {
    start: {longitude: number, latitude: number, id: string},
    end: {longitude: number, latitude: number, id: string}
  }[] = [];





  @ViewChild('map')
  private mapContainer!: ElementRef<HTMLElement>;

  constructor(private networkService: NetworkService) {
  }

  ngOnInit() {
    this.hubsSubscription = this.networkService.hubsChanged.subscribe((hubs: HubInterface[]) => {
      this.hubs = hubs;

      this.connectHubs(hubs);

      console.log(this.connectedHubs);

    });
    this.networkService.getHubs();


  }

  ngOnDestroy() {
    if (this.hubsSubscription) {
      this.hubsSubscription.unsubscribe();
    }
  }

  onHubClick(hub: HubInterface) {
    this.networkService.hubSelected.emit(hub);
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




}
