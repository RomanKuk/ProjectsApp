import { OverlayModule } from '@angular/cdk/overlay';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { AppRoutes } from './app.routes';
import { ProjectsModule } from './modules/projects/projects.module';
import { SharedModule } from './modules/shared/shared.module';
import { TasksModule } from './modules/tasks/tasks.module';
import { TeamsModule } from './modules/teams/teams.module';
import { UsersModule } from './modules/users/users.module';

@NgModule({
  declarations: [
    AppComponent,

  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    SharedModule,
    ProjectsModule,
    TasksModule,
    TeamsModule,
    UsersModule,
    RouterModule.forRoot(AppRoutes),
    BrowserAnimationsModule,
    OverlayModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
