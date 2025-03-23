import {Component, OnDestroy, OnInit} from '@angular/core';
import {HubInterface} from "../../interfaces/hub.interface";
import {NetworkService} from "../../services/network.service";
import {Subscription} from "rxjs";
import {MatDialog} from "@angular/material/dialog";
import {DeleteDialogComponent} from "../delete-dialog/delete-dialog.component";

@Component({
    selector: 'app-hub-details',
    templateUrl: './hub-details.component.html',
    styleUrl: './hub-details.component.css',
    standalone: false
})
export class HubDetailsComponent implements OnInit, OnDestroy{
  selectedHub: HubInterface | null = null;

  selectedHubSubscription: Subscription = new Subscription();


  constructor(private networkService: NetworkService, private dialog: MatDialog) {
  }

  ngOnInit() {
    this.selectedHubSubscription = this.networkService.hubSelected.subscribe((hub: HubInterface) => {
      this.selectedHub = hub;
    });
  }

  ngOnDestroy() {
    if(this.selectedHubSubscription) {
      this.selectedHubSubscription.unsubscribe();
    }
  }

  deleteHub(hubId: string) {
    const dialogRef = this.dialog.open(DeleteDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.networkService.deleteHub(hubId);
      }
    })



  }

}
