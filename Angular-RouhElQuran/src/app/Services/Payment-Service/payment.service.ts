import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  baseUrl = environment.apiBaseUrl
  constructor(private http: HttpClient) { }

  createPayment(planId: number): Observable<any> {
    const url = `${this.baseUrl}/api/Payments/CreatePayment?PlanId=${planId}`;
    return this.http.post(url,{});
  }
}
