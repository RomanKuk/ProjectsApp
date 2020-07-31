import { Routes } from '@angular/router';
//import { AuthGuard } from './guards/auth.guard';
import { ProjectsComponent } from './components/projects/projects.component';
import { TasksComponent } from './components/tasks/tasks.component';
import { TeamsComponent } from './components/teams/teams.component';
import { UsersComponent } from './components/users/users.component';

export const AppRoutes: Routes = [
    { path: '', component: ProjectsComponent, pathMatch: 'full' },
    { path: 'projects', component: ProjectsComponent, pathMatch: 'full' },
    { path: 'tasks', component: TasksComponent, pathMatch: 'full'/*, canActivate: [AuthGuard] */},
    { path: 'teams', component: TeamsComponent, pathMatch: 'full' },
    { path: 'users', component: UsersComponent, pathMatch: 'full' },
    { path: '**', redirectTo: '' }
];
