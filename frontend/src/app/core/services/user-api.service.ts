import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserApiService {

  constructor(private http: HttpClient) { }

  getApiKey(userId: string) {
    return this.http.get(`${environment.apiUrl}/users/getApiKey/${userId}`);
  }

  generateApiKey(userId: string) {
    return this.http.get(`${environment.apiUrl}/users/generateApiKey/${userId}`);
  }
  revokeApiKey(userId: string) {
    return this.http.delete(`${environment.apiUrl}/users/revokeApiKey/${userId}`);
  }
}
