import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTreeModule } from '@angular/material/tree';

import { GatewaysComponent } from './gateways.component';

describe('GatewaysComponent', () => {
  let component: GatewaysComponent;
  let fixture: ComponentFixture<GatewaysComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GatewaysComponent ],
      imports: [
        MatButtonModule,
        MatIconModule,
        MatTreeModule,
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GatewaysComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should compile', () => {
    expect(component).toBeTruthy();
  });
});
