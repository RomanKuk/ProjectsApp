import { Routes, RouterModule } from '@angular/router';
//import { AuthGuard } from './guards/auth.guard';
import { ProjectsComponent } from './components/projects/projects.component';
import { TasksComponent } from './components/tasks/tasks.component';
import { TeamsComponent } from './components/teams/teams.component';
import { UsersComponent } from './components/users/users.component';
import { ProjectDetailComponent } from './components/projects/project-detail/project-detail.component';
import { ProjectItemComponent } from './components/projects/project-list/project-item/project-item.component';
import { ProjectStartComponent } from './components/projects/project-start/project-start.component';
import { ProjectEditComponent } from './components/projects/project-edit/project-edit.component';
import { NgModule } from '@angular/core';

const appRoutes: Routes = [
    { path: '', redirectTo: '/projects', pathMatch: 'full' },
    { path: 'projects', component: ProjectsComponent, children: [
        { path: '', component: ProjectStartComponent },
        { path: 'new', component: ProjectEditComponent },
        { path: ':id', component: ProjectDetailComponent },
        { path: ':id/edit', component: ProjectEditComponent },
    ] },
    { path: 'tasks', component: TasksComponent, pathMatch: 'full'/*, canActivate: [AuthGuard] */},
    { path: 'teams', component: TeamsComponent, pathMatch: 'full' },
    { path: 'users', component: UsersComponent, pathMatch: 'full' },
    { path: '**', redirectTo: '' }
];

@NgModule({
    imports: [RouterModule.forRoot(appRoutes)],
    exports: [RouterModule]
  })
  export class AppRoutingModule {
  
  }
