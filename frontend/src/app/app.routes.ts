import { Routes } from '@angular/router';
import { ProjectsComponent } from './modules/projects/components/projects.component';
import { TasksComponent } from './components/tasks/tasks.component';
import { TeamsComponent } from './components/teams/teams.component';
import { UsersComponent } from './components/users/users.component';
import { ProjectStartComponent } from './modules/projects/components/project-start/project-start.component';
import { ProjectEditComponent } from './modules/projects/components/project-edit/project-edit.component';
import { ProjectDetailComponent } from './modules/projects/components/project-detail/project-detail.component';
import { UnsavedChangesGuard } from './modules/shared/guards/unsaved-changes.guard';

export const AppRoutes: Routes = [
    { path: '', redirectTo: '/projects', pathMatch: 'full' },
    { path: 'projects', component: ProjectsComponent, children: [
        { path: '', component: ProjectStartComponent },
        { path: 'new', component: ProjectEditComponent, canDeactivate: [UnsavedChangesGuard] },
        { path: ':id', component: ProjectDetailComponent },
        { path: ':id/edit', component: ProjectEditComponent, canDeactivate: [UnsavedChangesGuard]},
    ] },
    { path: 'tasks', component: TasksComponent, pathMatch: 'full'},
    { path: 'teams', component: TeamsComponent, pathMatch: 'full' },
    { path: 'users', component: UsersComponent, pathMatch: 'full' },
    { path: '**', redirectTo: '' }
];
