import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProjectDetailComponent } from './components/project-detail/project-detail.component';
import { ProjectEditComponent } from './components/project-edit/project-edit.component';
import { ProjectItemComponent } from './components/project-list/project-item/project-item.component';
import { ProjectListComponent } from './components/project-list/project-list.component';
import { ProjectStartComponent } from './components/project-start/project-start.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AppRoutes } from '../../app.routes';
import { SharedModule } from '../shared/shared.module';
import { ProjectsComponent } from './components/projects.component';


@NgModule({
  declarations: [
    ProjectListComponent,
    ProjectDetailComponent,
    ProjectEditComponent,
    ProjectItemComponent,
    ProjectStartComponent,
    ProjectsComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(AppRoutes),
    FormsModule,
    ReactiveFormsModule,
    SharedModule
  ],
  exports: [
  ]
})
export class ProjectsModule { }
