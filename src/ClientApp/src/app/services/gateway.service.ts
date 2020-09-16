import { Injectable } from '@angular/core';
import { Device } from './device.service';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';

export interface Gateway{
  id: number;
  serialNumber: string;
  name:string;
  iPv4:string;
  devices: Device[]
}
@Injectable({
  providedIn: 'root'
})
export class GatewayService {
  base_url='https://localhost:5001/'
  url='gateway/'

  selectedGateway: Gateway;

  constructor(private http: HttpClient) { }

  get_all(): Observable<Gateway[]> {
    try {
      console.log('Getting Gateways');
      const send_url = this.base_url + this.url;
      return this.http.get<Gateway[]>(send_url);
    } catch (error) {
      console.log('Error getting gateways: ' + error);
      return of([]);
    }
  }

  get(id: string): Observable<any> {
    try {
      console.log('Saving Gateway');
      const send_url = this.base_url + this.url + id;
      return this.http.get<Gateway>(send_url).pipe(map((result: any) => {
        if (!result) {
          console.log('Error getting gateway by ID.');
          return false;
        } else {
          console.log('Gateway returned');
          return true;
        }
      }, error => {
        console.log(error);
        return false;
      }));
    } catch (error) {
      console.log('Error getting gateway by ID: ' + error);
      return of(false);
    }
  }


  create(gateway: Gateway): Observable<boolean> {
    try {
      console.log('Saving Gateway');
      const send_url = this.base_url + this.url;
      return this.http.post<any>(send_url, gateway).pipe(map((result: any) => {
        if (!result) {
          console.log('Error creating gateway.');
          return false;
        } else {
          console.log('Gateway created');
          return true;
        }
      }, error => {
        console.log(error);
        return false;
      }));
    } catch (error) {
      console.log('error enviando a correo: ' + error);
      return of(false);
    }
  }

  update(gateway: Gateway): Observable<boolean> {
    try {
      console.log('Saving Gateway');
      const send_url = this.base_url + this.url + gateway.id;
      return this.http.put<boolean>(send_url, gateway).pipe(map((result: any) => {
        if (!result) {
          console.log('Error updating gateway.');
          return false;
        } else {
          console.log('Gateway updated');
          return true;
        }
      }, error => {
        console.log(error);
        return false;
      }));
    } catch (error) {
      console.log('Error updating gateway: ' + error);
      return of(false);
    }
  }

  delete(gateway: Gateway): Observable<boolean> {
    try {
      console.log('Deleting Gateway');
      const send_url = this.base_url + this.url + gateway.id;
      return this.http.delete<any>(send_url).pipe(map((result: any) => {
        if (!result) {
          console.log('Gateway deleted.');
          return false;
        } else {
          console.log('Can\'t delete the gateway');
          return true;
        }
      }, error => {
        console.log(error);
        return false;
      }));
    } catch (error) {
      console.log('Error deleting gateway: ' + error);
      return of(false);
    }
  }
}
