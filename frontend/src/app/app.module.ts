import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

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
import { TaskDetailComponent } from './components/tasks/task-detail/task-detail.component';
import { TaskItemComponent } from './components/tasks/task-list/task-item/task-item.component';
import { TeamItemComponent } from './components/teams/team-list/team-item/team-item.component';
import { TeamDetailComponent } from './components/teams/team-detail/team-detail.component';
import { UserDetailComponent } from './components/users/user-detail/user-detail.component';
import { UserItemComponent } from './components/users/user-list/user-item/user-item.component';
import { DropdownDirective } from './shared/dropdown.directive';
import { LocalizedDatePipe } from './shared/localized-date.pipe';
import { ProjectStartComponent } from './components/projects/project-start/project-start.component';
import { AppRoutingModule } from './app.routes';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// export function createTranslateLoader(http: HttpClient) {
//   return new TranslateHttpLoader(http, './assets/i18n/', '.json');
// }

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
    ProjectItemComponent,
    TaskDetailComponent,
    TaskItemComponent,
    TeamItemComponent,
    TeamDetailComponent,
    UserDetailComponent,
    UserItemComponent,
    DropdownDirective,
    ProjectStartComponent,
    LocalizedDatePipe
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    // TranslateModule.forRoot({
    //   loader: {
    //     provide: TranslateLoader,
    //     useFactory: (createTranslateLoader),
    //     deps: [HttpClient]
    //   }
    // })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
