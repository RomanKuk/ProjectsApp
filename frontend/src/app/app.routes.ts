import { Routes } from '@angular/router';
import { ProjectsComponent } from './modules/projects/projects.component';
import { ProjectStartComponent } from './modules/projects/components/project-start/project-start.component';
import { ProjectEditComponent } from './modules/projects/components/project-edit/project-edit.component';
import { ProjectDetailComponent } from './modules/projects/components/project-detail/project-detail.component';
import { UnsavedChangesGuard } from './modules/shared/guards/unsaved-changes.guard';
import { TasksComponent } from './modules/tasks/tasks.component';
import { TaskStartComponent } from './modules/tasks/components/task-start/task-start.component';
import { TaskDetailComponent } from './modules/tasks/components/task-detail/task-detail.component';
import { TaskEditComponent } from './modules/tasks/components/task-edit/task-edit.component';


export const AppRoutes: Routes = [
    { path: '', redirectTo: '/projects', pathMatch: 'full' },
    { path: 'projects', component: ProjectsComponent, children: [
        { path: '', component: ProjectStartComponent },
        { path: 'new', component: ProjectEditComponent, canDeactivate: [UnsavedChangesGuard] },
        { path: ':id', component: ProjectDetailComponent },
        { path: ':id/edit', component: ProjectEditComponent, canDeactivate: [UnsavedChangesGuard]},
    ] },
     { path: 'tasks', component: TasksComponent, children: [
        { path: '', component: TaskStartComponent },
        { path: 'new', component: TaskEditComponent, canDeactivate: [UnsavedChangesGuard] },
        { path: ':id', component: TaskDetailComponent },
        { path: ':id/edit', component: TaskEditComponent, canDeactivate: [UnsavedChangesGuard]},
     ]},
    // { path: 'teams', component: TeamsComponent, pathMatch: 'full' },
    // { path: 'users', component: UsersComponent, pathMatch: 'full' },
    { path: '**', redirectTo: '' }
];
