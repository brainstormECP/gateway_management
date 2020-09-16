import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { GatewayService, Gateway } from '../services/gateway.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-gateway',
  templateUrl: './gateway.component.html',
  styleUrls: ['./gateway.component.css']
})
export class GatewayComponent implements OnInit {
  ipPattern =
    "(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)";
  gatewayForm = this.fb.group({
    company: null,
    name: [null, Validators.required],
    serialNumber: [null, Validators.required],
    ipv4: [null, [Validators.required, Validators.pattern(this.ipPattern)]],
  });

  constructor(private fb: FormBuilder, private gatewayService: GatewayService, private router: Router) { }

  ngOnInit(): void {
    if (this.gatewayService.selectedGateway != null) {
      this.gatewayForm.controls['serialNumber'].setValue(this.gatewayService.selectedGateway.serialNumber);
      this.gatewayForm.controls['name'].setValue(this.gatewayService.selectedGateway.name);
      this.gatewayForm.controls['ipv4'].setValue(this.gatewayService.selectedGateway.iPv4);
    }
  }

  onCancel() {
    this.gatewayService.selectedGateway = null;
    this.router.navigate(["/gateways"]);
  }

  onSubmit() {
    if (!this.gatewayForm.invalid) {
      let gateway = {
        id: 0,
        serialNumber: this.gatewayForm.controls['serialNumber'].value,
        name: this.gatewayForm.controls['name'].value,
        iPv4: this.gatewayForm.controls['ipv4'].value,
        devices: [],
      } as Gateway;
      if (this.gatewayService.selectedGateway != null) {
        gateway.id = this.gatewayService.selectedGateway.id;
        this.gatewayService.update(gateway).subscribe(g => {
          this.gatewayService.selectedGateway = null;
          this.router.navigate(["/gateways"]);
        }, err => { alert('Error Updating gateway'); })
      } else {
        this.gatewayService.create(gateway).subscribe(g => {
          this.router.navigate(["/gateways"]);
        }, err => { alert('Error creating'); })
      }

    }
  }
}
