import { Injectable, EventEmitter } from '@angular/core';
import { HttpInternalService } from './http-internal.service';
import { User } from '../models/user/user.model';
import { UserCreate } from '../models/user/user-create.model';
import { UserEdit } from '../models/user/user-edit.model';
import { Observable } from 'rxjs';
import { HttpResponse } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class UserService {
    public routePrefix = '/api/users';
    userSelected = new EventEmitter<User>();

    constructor(private httpService: HttpInternalService) { }

    public getUsers(): Observable<HttpResponse<User[]>> {
        return this.httpService.getFullRequest<User[]>(`${this.routePrefix}`);
    }

    public getUserById(userId: number): Observable<HttpResponse<User>> {
        return this.httpService.getFullRequest<User>(`${this.routePrefix}/${userId}`);
    }

    public createUser(user: UserCreate): Observable<HttpResponse<User>> {
        return this.httpService.postFullRequest<User>(`${this.routePrefix}`, user);
    }

    public updateUser(user: UserEdit): Observable<HttpResponse<User>> {
        return this.httpService.putFullRequest<User>(`${this.routePrefix}`, user);
    }

    public deleteUserById(userId: number): Observable<HttpResponse<void>> {
        return this.httpService.deleteFullRequest(`${this.routePrefix}/${userId}`);
    }
}
