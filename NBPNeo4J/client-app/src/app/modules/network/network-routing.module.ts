import {NgModule} from "@angular/core";
import {RouterModule, Routes} from "@angular/router";
import {NetworkDisplayComponent} from "./components/network-display/network-display.component";
import {CreateHubComponent} from "./components/create-hub/create-hub.component";
import {CreateServiceComponent} from "./components/create-service/create-service.component";

const routes: Routes = [
  {path: '', component: NetworkDisplayComponent, children: []},
  {path: 'hub/create', component: CreateHubComponent},
  {path: 'service/create', component: CreateServiceComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class NetworkRoutingModule {}
