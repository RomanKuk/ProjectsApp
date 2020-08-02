import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { AppRoutes } from './app.routes';

import { ProjectsComponent } from './modules/projects/components/projects.component';
import { TaskDetailComponent } from './components/tasks/task-detail/task-detail.component';
import { TaskEditComponent } from './components/tasks/task-edit/task-edit.component';
import { TaskItemComponent } from './components/tasks/task-list/task-item/task-item.component';
import { TaskListComponent } from './components/tasks/task-list/task-list.component';
import { TasksComponent } from './components/tasks/tasks.component';
import { TeamDetailComponent } from './components/teams/team-detail/team-detail.component';
import { TeamEditComponent } from './components/teams/team-edit/team-edit.component';
import { TeamItemComponent } from './components/teams/team-list/team-item/team-item.component';
import { TeamListComponent } from './components/teams/team-list/team-list.component';
import { TeamsComponent } from './components/teams/teams.component';
import { UserDetailComponent } from './components/users/user-detail/user-detail.component';
import { UserEditComponent } from './components/users/user-edit/user-edit.component';
import { UserItemComponent } from './components/users/user-list/user-item/user-item.component';
import { UserListComponent } from './components/users/user-list/user-list.component';
import { UsersComponent } from './components/users/users.component';

import { ProjectsModule } from './modules/projects/projects.module';
import { SharedModule } from './modules/shared/shared.module';
import { RouterModule } from '@angular/router';
import { DropdownDirective } from './modules/shared/directives/dropdown.directive';
import { LocalizedDatePipe } from './modules/shared/pipes/localized-date.pipe';



@NgModule({
  declarations: [
    AppComponent,
    TasksComponent,
    TaskListComponent,
    TaskEditComponent,
    TeamsComponent,
    TeamListComponent,
    TeamEditComponent,
    UsersComponent,
    UserListComponent,
    UserEditComponent,
    TaskDetailComponent,
    TaskItemComponent,
    TeamItemComponent,
    TeamDetailComponent,
    UserDetailComponent,
    UserItemComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    SharedModule,
    ProjectsModule,
    RouterModule.forRoot(AppRoutes)
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
