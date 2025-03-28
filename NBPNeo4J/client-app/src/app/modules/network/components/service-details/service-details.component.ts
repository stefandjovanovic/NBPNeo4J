import {Component, OnDestroy, OnInit} from '@angular/core';
import {ServiceInterface} from "../../interfaces/service.interface";
import {Subscription} from "rxjs";
import {NetworkService} from "../../services/network.service";
import {MatDialog} from "@angular/material/dialog";
import {DeleteDialogComponent} from "../delete-dialog/delete-dialog.component";

@Component({
  selector: 'app-service-details',
  standalone: false,
  templateUrl: './service-details.component.html',
  styleUrl: './service-details.component.css'
})
export class ServiceDetailsComponent implements OnInit, OnDestroy{
  selectedService: ServiceInterface | null = null;
  selectedServiceSubscription: Subscription = new Subscription();
  hubName: string = '';

  constructor(
    private networkService: NetworkService,
    private matDialog: MatDialog
  ) {
  }

  ngOnInit() {
    this.selectedServiceSubscription = this.networkService.serviceSelected.subscribe((service: ServiceInterface) => {
      this.selectedService = service;
      this.hubName = this.networkService.getHubName(service.connectedToHubId);
    });
  }

  ngOnDestroy() {
    if(this.selectedServiceSubscription) {
      this.selectedServiceSubscription.unsubscribe();
    }
  }

  deleteService(serviceId: string) {
    const dialogRef = this.matDialog.open(DeleteDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.networkService.deleteService(serviceId);
      }
    });
  }

}
