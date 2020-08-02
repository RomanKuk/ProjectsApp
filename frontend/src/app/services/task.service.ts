import { Injectable, EventEmitter } from '@angular/core';
import { HttpInternalService } from './http-internal.service';
import { TaskModel } from '../models/task/task.model';
import { TaskModelCreate } from '../models/task/task-create.model';
import { TaskModelEdit } from '../models/task/task-edit.model';
import { TaskFinishedThisYear } from '../models/function-models/task-finished-this-year.model';
import { UserLastProjectTasks } from '../models/function-models/user-last-project-tasks.model';
import { UserTasks } from '../models/function-models/user-tasks.model';
import { HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';


@Injectable({ providedIn: 'root' })
export class TaskService {
    public routePrefix = '/api/tasks';
    taskSelected = new EventEmitter<TaskModel>();

    constructor(private httpService: HttpInternalService) { }

    public getTasks(): Observable<HttpResponse<TaskModel[]>> {
        return this.httpService.getFullRequest<TaskModel[]>(`${this.routePrefix}`);
    }

    public getTaskById(taskId: number): Observable<HttpResponse<TaskModel>> {
        return this.httpService.getFullRequest<TaskModel>(`${this.routePrefix}/${taskId}`);
    }

    public createTask(task: TaskModelCreate): Observable<HttpResponse<TaskModel>> {
        return this.httpService.postFullRequest<TaskModel>(`${this.routePrefix}`, task);
    }

    public updateTask(task: TaskModelEdit): Observable<HttpResponse<TaskModel>> {
        return this.httpService.putFullRequest<TaskModel>(`${this.routePrefix}`, task);
    }

    public deleteTaskById(userId: number): Observable<HttpResponse<void>> {
        return this.httpService.deleteFullRequest(`${this.routePrefix}/${userId}`);
    }

    public getTasksByUserIdWithNameCondition(userId: number): Observable<HttpResponse<TaskModel[]>>
    {
        return this.httpService.getFullRequest<TaskModel[]>(`${this.routePrefix}/by-user-with-name-condition/${userId}`);
    }

    public getTasksByUserFinishedThisYear(userId: number): Observable<HttpResponse<TaskFinishedThisYear[]>>
    {
        return this.httpService.getFullRequest<TaskFinishedThisYear[]>(`${this.routePrefix}/by-user-finished-this-year/${userId}`);
    }

    public getUsersSortedByFirstNameAndSortedTasks(): Observable<HttpResponse<UserTasks[]>>
    {
        return this.httpService.getFullRequest<UserTasks[]>(`${this.routePrefix}/users-sorted-by-first-name-sorted-tasks`);
    }

    public getNewUserStructure(userId: number): Observable<HttpResponse<UserLastProjectTasks[]>>
    {
        return this.httpService.getFullRequest<UserLastProjectTasks[]>(`${this.routePrefix}/structure-with-tasks-by-user/${userId}`);
    }

    public getUnfinishedTasksByUser(userId: number): Observable<HttpResponse<TaskModel[]>>
    {
        return this.httpService.getFullRequest<TaskModel[]>(`${this.routePrefix}/unfinished-tasks-by-user/${userId}`);
    }

    public getUnfinishedTasks(): Observable<HttpResponse<TaskModel[]>>
    {
        return this.httpService.getFullRequest<TaskModel[]>(`${this.routePrefix}/unfinished-tasks`);
    }
}
