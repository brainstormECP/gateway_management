import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { MaterialModule } from "./material.module";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { GatewaysComponent } from './gateways/gateways.component';
import { GatewayComponent } from './gateway/gateway.component';
import { DevicesComponent } from './devices/devices.component';
import { DeviceComponent } from './device/device.component';
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';

const maskConfig: Partial<IConfig> = {
  validation: true,
};
@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    GatewaysComponent,
    GatewayComponent,
    DevicesComponent,
    DeviceComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    MaterialModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    NgxDatatableModule,
    NgxMaskModule.forRoot(maskConfig),
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'gateways', component: GatewaysComponent },
      { path: 'gateway', component: GatewayComponent },
      { path: 'devices', component: DevicesComponent },
      { path: 'device', component: DeviceComponent },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
