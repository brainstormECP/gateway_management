import { Component, ViewChild, OnInit } from '@angular/core';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';
import { FlatTreeControl } from '@angular/cdk/tree';
import { DeviceService, Device } from '../services/device.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-devices',
  templateUrl: './devices.component.html',
  styleUrls: ['./devices.component.css']
})
export class DevicesComponent implements OnInit {
  devices: Device[]
  @ViewChild('myTable') table: any;
  constructor(public deviceService: DeviceService, private router: Router) {
  }

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.deviceService.get_all().subscribe(g => {
      this.devices = g;
    }, err => { console.log("ERROR "); })
  }

  edit(device: Device) {
    this.deviceService.selectedDevice = device;
    this.router.navigate(["/device"]);
  }
  delete(device: Device) {
    this.deviceService.delete(device).subscribe(g => { this.loadData() }, err => { alert("Can't delete this device") });
  }
}
