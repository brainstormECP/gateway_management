import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { DeviceService, Device } from '../services/device.service';
import { GatewayService } from '../services/gateway.service';

@Component({
  selector: 'app-device',
  templateUrl: './device.component.html',
  styleUrls: ['./device.component.css']
})
export class DeviceComponent implements OnInit {
  deviceForm = this.fb.group({
    company: null,
    uid: [null, Validators.required],
    vendor: [null, Validators.required],
    gatewayId: [null, Validators.required],
    status: [null, Validators.required],
  });

  statuses = [
    {name:"Offline", value:0},
    {name:"Online", value:1},
  ]

  gateways = []

  constructor(private fb: FormBuilder, private gatewayService: GatewayService,private deviceService: DeviceService, private router: Router) { }

  ngOnInit(): void {
    this.gatewayService.get_all().subscribe(g => {
      g.forEach(element => {
        this.gateways.push({name:element.name, value:element.id})
      });
    });
    if (this.deviceService.selectedDevice != null) {
      this.deviceForm.controls['uid'].setValue(this.deviceService.selectedDevice.uid);
      this.deviceForm.controls['vendor'].setValue(this.deviceService.selectedDevice.vendor);
      this.deviceForm.controls['gatewayId'].setValue(this.deviceService.selectedDevice.gatewayId);
      this.deviceForm.controls['status'].setValue(this.deviceService.selectedDevice.status);
    }
  }

  onCancel() {
    this.deviceService.selectedDevice = null;
    this.router.navigate(["/devices"]);
  }

  onSubmit() {
    if (!this.deviceForm.invalid) {
      let device = {
        id: 0,
        uid: this.deviceForm.controls['uid'].value,
        vendor: this.deviceForm.controls['vendor'].value,
        createdDate: new Date(),
        gateway: "",
        gatewayId: this.deviceForm.controls['gatewayId'].value,
        status: this.deviceForm.controls['status'].value,
      } as Device;
      if (this.deviceService.selectedDevice != null) {
        device.id = this.deviceService.selectedDevice.id;
        this.deviceService.update(device).subscribe(g => {
          this.deviceService.selectedDevice = null;
          this.router.navigate(["/devices"]);
        }, err => { alert('Error Updating gateway'); })
      } else {
        this.deviceService.create(device).subscribe(g => {
          this.router.navigate(["/devices"]);
        }, err => { alert('Error creating'); })
      }

    }
  }
}
