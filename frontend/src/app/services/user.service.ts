import { Injectable, EventEmitter } from '@angular/core';
import { HttpInternalService } from './http-internal.service';
import { User } from '../models/user/user.model';
import { UserCreate } from '../models/user/user-create.model';
import { UserEdit } from '../models/user/user-edit.model';

@Injectable({ providedIn: 'root' })
export class UserService {
    public routePrefix = '/api/users';
    userSelected = new EventEmitter<User>();

    constructor(private httpService: HttpInternalService) { }

    public getUsers() {
        return this.httpService.getFullRequest<User[]>(`${this.routePrefix}`);
    }

    public getUserById(userId: number) {
        return this.httpService.getFullRequest<User>(`${this.routePrefix}/${userId}`);
    }

    public createUser(user: UserCreate) {
        return this.httpService.postFullRequest<User>(`${this.routePrefix}`, user);
    }

    public updateUser(user: UserEdit) {
        return this.httpService.putFullRequest<User>(`${this.routePrefix}`, user);
    }

    public deleteUserById(userId: number) {
        return this.httpService.deleteFullRequest(`${this.routePrefix}/${userId}`);
    }
}
