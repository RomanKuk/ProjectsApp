import { HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ProjectTasks } from '../models/function-models/project-tasks.model';
import { ProjectCreate } from '../models/project/project-create.model';
import { ProjectEdit } from '../models/project/project-edit.model';
import { Project } from '../models/project/project.model';
import { HttpInternalService } from './http-internal.service';

@Injectable({ providedIn: 'root' })
export class ProjectService {
    public routePrefix = '/api/projects';

    constructor(private httpService: HttpInternalService) { }

    public getProjects(): Observable<HttpResponse<Project[]>> {
        return this.httpService.getFullRequest<Project[]>(`${this.routePrefix}`);
    }

    public getProjectById(projectId: number): Observable<HttpResponse<Project>> {
        return this.httpService.getFullRequest<Project>(`${this.routePrefix}/${projectId}`);
    }

    public createProject(project: ProjectCreate): Observable<HttpResponse<Project>> {
        return this.httpService.postFullRequest<Project>(`${this.routePrefix}`, project);
    }

    public updateProject(project: ProjectEdit): Observable<HttpResponse<Project>> {
        return this.httpService.putFullRequest<Project>(`${this.routePrefix}`, project);
    }

    public deleteProjectById(projectId: number): Observable<HttpResponse<void>> {
        return this.httpService.deleteFullRequest(`${this.routePrefix}/${projectId}`);
    }

    public getNewProjectStructure(): Observable<HttpResponse<ProjectTasks[]>>
    {
        return this.httpService.getFullRequest<ProjectTasks[]>(`${this.routePrefix}/new-project-structure`);
    }
}
