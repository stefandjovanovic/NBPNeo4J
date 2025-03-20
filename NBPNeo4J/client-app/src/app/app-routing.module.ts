import { NgModule } from '@angular/core';
import {PreloadAllModules, RouterModule, Routes} from '@angular/router';

const routes: Routes = [
  {path: '', redirectTo: 'network', pathMatch: 'full'},
  {path: 'network', loadChildren: () => import('./modules/network/network.module').then(m => m.NetworkModule)},
  {path: 'parts', loadChildren: () => import('./modules/parts/parts.module').then(m => m.PartsModule)},
  {path: 'services', loadChildren: () => import('./modules/services/services.module').then(m => m.ServicesModule)},
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {preloadingStrategy: PreloadAllModules})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
