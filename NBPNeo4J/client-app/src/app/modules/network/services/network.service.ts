import {Injectable, OnDestroy, OnInit} from '@angular/core';
import {HubInterface} from "../interfaces/hub.interface";
import { HttpClient } from "@angular/common/http";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Subject} from "rxjs";
import {HubCreateInterface} from "../interfaces/hub-create.interface";
import {Router} from "@angular/router";
import {ServiceInterface} from "../interfaces/service.interface";
import {ServiceCreateInterface} from "../interfaces/service-create.interface";

@Injectable({
  providedIn: 'root'
})
export class NetworkService{
  hubs: HubInterface[] = [];
  hubsChanged: Subject<HubInterface[]> = new Subject<HubInterface[]>();

  services: ServiceInterface[] = [];
  servicesChanged: Subject<ServiceInterface[]> = new Subject<ServiceInterface[]>();


  private hubUrl = 'https://localhost:7255/api/hub';
  private serviceUrl = 'https://localhost:7255/api/service';

  constructor(private http: HttpClient, private snackBar: MatSnackBar, private router: Router) { }

  fetchHubs() {
    this.http.get<HubInterface[]>(this.hubUrl+'/all').subscribe((hubs) => {
      this.hubs = hubs;
      this.hubsChanged.next(this.hubs.slice());
    });
  }

  getHubs(){
    if (this.hubs.length === 0){
      this.fetchHubs();
    }else{
      this.hubsChanged.next(this.hubs.slice());
    }
  }

  createHub(hub: HubCreateInterface){
    this.http.post<HubInterface>(this.hubUrl+'/create', hub).subscribe((createdHub) => {
      this.hubs.push(createdHub);
      this.hubsChanged.next(this.hubs.slice());
      this.snackBar.open('Hub created', 'Close', {
        duration: 2000,
      });
      this.router.navigate(['/network']);
    });
  }

  updateHub(hub: HubCreateInterface, hubId: string){
    this.http.put<HubInterface>(this.hubUrl+'/update/'+hubId, hub).subscribe((updatedHub) => {
      const index = this.hubs.findIndex((h) => h.id === updatedHub.id);
      this.hubs[index] = updatedHub;
      this.hubsChanged.next(this.hubs.slice());
      this.snackBar.open('Hub updated', 'Close', {
        duration: 2000,
      });
      this.router.navigate(['/network']);
    });
  }

  deleteHub(hubId: string){
    this.http.delete(this.hubUrl+'/delete/'+hubId).subscribe(() => {
      const index = this.hubs.findIndex((h) => h.id === hubId);
      this.hubs.splice(index, 1);
      this.hubsChanged.next(this.hubs.slice());
      this.snackBar.open('Hub deleted', 'Close', {
        duration: 2000,
      });
    });
  }

  fetchServices() {
    this.http.get<ServiceInterface[]>(this.serviceUrl+'/all').subscribe((services) => {
      this.services = services;
      this.servicesChanged.next(this.services.slice());
    });
  }

  getServices(){
    if (this.services.length === 0){
      this.fetchServices();
    }else{
      this.servicesChanged.next(this.services.slice());
    }
  }

  createService(service: ServiceCreateInterface, connectedHubId: string){
    this.http.post<ServiceInterface>(this.serviceUrl+'/create', service).subscribe((createdService) => {
      this.http.post(`${this.serviceUrl}/${createdService.id}/connect-to-hub/${connectedHubId}`, {}).subscribe(() => {
        createdService.connectedToHubId = connectedHubId;
        this.services.push(createdService);
        this.servicesChanged.next(this.services.slice());
        this.snackBar.open('Service created', 'Close', {
          duration: 2000,
        });
        this.router.navigate(['/network']);
      });

    });
  }

  updateService(service: ServiceCreateInterface, serviceId: string, connectedHubId: string){
    this.http.put<ServiceInterface>(this.serviceUrl+'/update/'+serviceId, service).subscribe((updatedService) => {

      const index = this.services.findIndex((s) => s.id === updatedService.id);
      this.services[index] = updatedService;
      this.servicesChanged.next(this.services.slice());
      this.snackBar.open('Service updated', 'Close', {
        duration: 2000,
      });
      this.router.navigate(['/network']);
    });
  }

  deleteService(serviceId: string){
    this.http.delete(this.serviceUrl+'/delete/'+serviceId).subscribe(() => {
      const index = this.services.findIndex((s) => s.id === serviceId);
      this.services.splice(index, 1);
      this.servicesChanged.next(this.services.slice());
      this.snackBar.open('Service deleted', 'Close', {
        duration: 2000,
      });
    });
  }



}
