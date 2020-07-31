import { Injectable } from '@angular/core';
import { HttpInternalService } from './http-internal.service';
import { TaskModel } from '../models/task/task.model';
import { TaskModelCreate } from '../models/task/task-create.model';
import { TaskModelEdit } from '../models/task/task-edit.model';
import { TaskFinishedThisYear } from '../models/function-models/task-finished-this-year.model';
import { UserLastProjectTasks } from '../models/function-models/user-last-project-tasks.model';
import { UserTasks } from '../models/function-models/user-tasks.model';


@Injectable({ providedIn: 'root' })
export class TaskService {
    public routePrefix = '/api/tasks';

    constructor(private httpService: HttpInternalService) { }

    public getTasks() {
        return this.httpService.getFullRequest<TaskModel[]>(`${this.routePrefix}`);
    }

    public getTaskById(taskId: number) {
        return this.httpService.getFullRequest<TaskModel>(`${this.routePrefix}/${taskId}`);
    }

    public createTask(task: TaskModelCreate) {
        return this.httpService.postFullRequest<TaskModel>(`${this.routePrefix}`, task);
    }

    public updateTask(task: TaskModelEdit) {
        return this.httpService.putFullRequest<TaskModel>(`${this.routePrefix}`, task);
    }

    public deleteTaskById(userId: number) {
        return this.httpService.deleteFullRequest(`${this.routePrefix}/${userId}`);
    }

    public getTasksByUserIdWithNameCondition(userId: number)
    {
        return this.httpService.getFullRequest<TaskModel[]>(`${this.routePrefix}/by-user-with-name-condition/${userId}`);
    }

    public getTasksByUserFinishedThisYear(userId: number)
    {
        return this.httpService.getFullRequest<TaskFinishedThisYear[]>(`${this.routePrefix}/by-user-finished-this-year/${userId}`);
    }

    public getUsersSortedByFirstNameAndSortedTasks()
    {
        return this.httpService.getFullRequest<UserTasks[]>(`${this.routePrefix}/users-sorted-by-first-name-sorted-tasks`);
    }

    public getNewUserStructure(userId: number)
    {
        return this.httpService.getFullRequest<UserLastProjectTasks[]>(`${this.routePrefix}/structure-with-tasks-by-user/${userId}`);
    }

    public getUnfinishedTasksByUser(userId: number)
    {
        return this.httpService.getFullRequest<TaskModel[]>(`${this.routePrefix}/unfinished-tasks-by-user/${userId}`);
    }

    public getUnfinishedTasks()
    {
        return this.httpService.getFullRequest<TaskModel[]>(`${this.routePrefix}/unfinished-tasks`);
    }
}
