import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';

export enum Status {
  Offline = 0,
  Online = 1
}
export interface Device {
  id: number;
  uid: number;
  vendor: string;
  createdDate: Date;
  status: Status;
  gatewayId: number;
  gateway: string;
}

@Injectable({
  providedIn: 'root'
})
export class DeviceService {
  base_url = 'https://localhost:5001/'
  url = 'devices/'

  selectedDevice: Device;

  constructor(private http: HttpClient) { }

  get_all(): Observable<Device[]> {
    try {
      console.log('Getting Devices');
      const send_url = this.base_url + this.url;
      return this.http.get<Device[]>(send_url);
    } catch (error) {
      console.log('Error getting devices: ' + error);
      return of([]);
    }
  }

  get(id: string): Observable<any> {
    try {
      console.log('Getting Device');
      const send_url = this.base_url + this.url + id;
      return this.http.get<Device>(send_url).pipe(map((result: any) => {
        if (!result) {
          console.log('Error getting device by id.');
          return false;
        } else {
          console.log('Device returned');
          return true;
        }
      }, error => {
        console.log(error);
        return false;
      }));
    } catch (error) {
      console.log('Error getting device by ID: ' + error);
      return of(false);
    }
  }


  create(device: Device): Observable<boolean> {
    try {
      console.log('Saving Device');
      const send_url = this.base_url + this.url;
      return this.http.post<any>(send_url, device).pipe(map((result: any) => {
        if (!result) {
          console.log('Error creating device.');
          return false;
        } else {
          console.log('Device created.');
          return true;
        }
      }, error => {
        console.log(error);
        return false;
      }));
    } catch (error) {
      console.log('Error creating device: ' + error);
      return of(false);
    }
  }

  update(device: Device): Observable<boolean> {
    try {
      console.log('Updating Device');
      const send_url = this.base_url + this.url + device.id;
      return this.http.put<boolean>(send_url, device).pipe(map((result: any) => {
        if (!result) {
          console.log('Error updating device.');
          return false;
        } else {
          console.log('Device updated.');
          return true;
        }
      }, error => {
        console.log(error);
        return false;
      }));
    } catch (error) {
      console.log('Error updating device: ' + error);
      return of(false);
    }
  }

  delete(device: Device): Observable<boolean> {
    try {
      console.log('Deleting Device');
      const send_url = this.base_url + this.url + device.id;
      return this.http.delete<any>(send_url).pipe(map((result: any) => {
        if (!result) {
          console.log('Device deleted.');
          return false;
        } else {
          console.log('Can\'t delete the device');
          return true;
        }
      }, error => {
        console.log(error);
        return false;
      }));
    } catch (error) {
      console.log('Error deleting device: ' + error);
      return of(false);
    }
  }
}
