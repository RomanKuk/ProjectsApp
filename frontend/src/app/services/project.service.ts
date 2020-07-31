import { Injectable } from '@angular/core';
import { HttpInternalService } from './http-internal.service';
import { Project } from '../models/project/project.model';
import { ProjectCreate } from '../models/project/project-create.model';
import { ProjectEdit } from '../models/project/project-edit.model';
import { ProjectTasks } from '../models/function-models/project-tasks.model';

@Injectable({ providedIn: 'root' })
export class ProjectService {
    public routePrefix = '/api/projects';

    constructor(private httpService: HttpInternalService) { }

    public getProjects() {
        return this.httpService.getFullRequest<Project[]>(`${this.routePrefix}`);
    }

    public getProjectById(projectId: number) {
        return this.httpService.getFullRequest<Project>(`${this.routePrefix}/${projectId}`);
    }

    public createProject(project: ProjectCreate) {
        return this.httpService.postFullRequest<Project>(`${this.routePrefix}`, project);
    }

    public updateProject(project: ProjectEdit) {
        return this.httpService.putFullRequest<Project>(`${this.routePrefix}`, project);
    }

    public deleteProjectById(projectId: number) {
        return this.httpService.deleteFullRequest(`${this.routePrefix}/${projectId}`);
    }

    public getNewProjectStructure()
    {
        return this.httpService.getFullRequest<ProjectTasks[]>(`${this.routePrefix}/new-project-structure`);
    }

    public getUserProjectTasksCountByUserId(userId: number)
    {
        //return this.httpService.getFullRequest<ProjectTasks[]>(`${this.routePrefix}/project-tasks-count-by-user/${userId}`);
    }
}
