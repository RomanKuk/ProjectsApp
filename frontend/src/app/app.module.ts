import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppRoutes } from './app.routes';

import { AppComponent } from './app.component';
import { ProjectsComponent } from './components/projects/projects.component';
import { HeaderComponent } from './components/header/header.component';
import { ProjectListComponent } from './components/projects/project-list/project-list.component';
import { ProjectEditComponent } from './components/projects/project-edit/project-edit.component';
import { ProjectCreateComponent } from './components/projects/project-create/project-create.component';
import { TasksComponent } from './components/tasks/tasks.component';
import { TaskListComponent } from './components/tasks/task-list/task-list.component';
import { TaskEditComponent } from './components/tasks/task-edit/task-edit.component';
import { TaskCreateComponent } from './components/tasks/task-create/task-create.component';
import { TeamsComponent } from './components/teams/teams.component';
import { TeamListComponent } from './components/teams/team-list/team-list.component';
import { TeamEditComponent } from './components/teams/team-edit/team-edit.component';
import { TeamCreateComponent } from './components/teams/team-create/team-create.component';
import { UsersComponent } from './components/users/users.component';
import { UserListComponent } from './components/users/user-list/user-list.component';
import { UserEditComponent } from './components/users/user-edit/user-edit.component';
import { UserCreateComponent } from './components/users/user-create/user-create.component';
import { ProjectDetailComponent } from './components/projects/project-detail/project-detail.component';
import { ProjectItemComponent } from './components/projects/project-list/project-item/project-item.component';

@NgModule({
  declarations: [
    AppComponent,
    ProjectsComponent,
    HeaderComponent,
    ProjectListComponent,
    ProjectEditComponent,
    ProjectCreateComponent,
    TasksComponent,
    TaskListComponent,
    TaskEditComponent,
    TaskCreateComponent,
    TeamsComponent,
    TeamListComponent,
    TeamEditComponent,
    TeamCreateComponent,
    UsersComponent,
    UserListComponent,
    UserEditComponent,
    UserCreateComponent,
    ProjectDetailComponent,
    ProjectItemComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    RouterModule.forRoot(AppRoutes)
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
