import { Component, ViewChild, OnInit } from '@angular/core';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';
import { FlatTreeControl } from '@angular/cdk/tree';
import { GatewayService, Gateway } from '../services/gateway.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-gateways',
  templateUrl: './gateways.component.html',
  styleUrls: ['./gateways.component.css']
})
export class GatewaysComponent implements OnInit {
  gateways: Gateway[]
  displayedColumns: string[] = ['uid', 'vendor', 'createdDate', 'status'];
  @ViewChild('myTable') table: any;
  constructor(public gatewayService: GatewayService, private router: Router) {
  }

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.gatewayService.get_all().subscribe(g => {
      this.gateways = g;
    }, err => { console.log("ERROR "); })
  }

  // ngAfterViewInit(){
  //   this.gatewayService.get_all().subscribe(g => {
  //     this.gateways = g;
  //   }, err => {console.log("ERROR ");})
  // }


  toggleExpandRow(row) {
    console.log('Toggled Expand Row!', row);
    this.table.rowDetail.toggleExpandRow(row);
  }

  onDetailToggle(event) {
    console.log('Detail Toggled', event);
  }

  edit(gateway: Gateway) {
    this.gatewayService.selectedGateway = gateway;
    this.router.navigate(["/gateway"]);
  }
  delete(gateway: Gateway) {
    this.gatewayService.delete(gateway).subscribe(g => { this.loadData() }, err => { alert("Can't delete this gateway") });
  }
}
