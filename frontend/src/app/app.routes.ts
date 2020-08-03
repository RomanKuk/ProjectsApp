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
import { TeamsComponent } from './modules/teams/teams.component';
import { UsersComponent } from './modules/users/users.component';
import { TeamStartComponent } from './modules/teams/components/team-start/team-start.component';
import { TeamEditComponent } from './modules/teams/components/team-edit/team-edit.component';
import { TeamDetailComponent } from './modules/teams/components/team-detail/team-detail.component';
import { UserStartComponent } from './modules/users/user-start/user-start.component';
import { UserEditComponent } from './modules/users/user-edit/user-edit.component';
import { UserDetailComponent } from './modules/users/user-detail/user-detail.component';


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
     { path: 'teams', component: TeamsComponent, children: [
        { path: '', component: TeamStartComponent },
        { path: 'new', component: TeamEditComponent, canDeactivate: [UnsavedChangesGuard] },
        { path: ':id', component: TeamDetailComponent },
        { path: ':id/edit', component: TeamEditComponent, canDeactivate: [UnsavedChangesGuard]},
     ]},
     { path: 'users', component: UsersComponent, children: [
        { path: '', component: UserStartComponent },
        { path: 'new', component: UserEditComponent, canDeactivate: [UnsavedChangesGuard] },
        { path: ':id', component: UserDetailComponent },
        { path: ':id/edit', component: UserEditComponent, canDeactivate: [UnsavedChangesGuard]},
     ]},
    { path: '**', redirectTo: '' }
];
