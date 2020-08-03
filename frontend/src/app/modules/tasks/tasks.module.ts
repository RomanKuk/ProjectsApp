import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TasksComponent } from './tasks.component';
import { TaskDetailComponent } from './components/task-detail/task-detail.component';
import { TaskItemComponent } from './components/task-list/task-item/task-item.component';
import { TaskEditComponent } from './components/task-edit/task-edit.component';
import { TaskListComponent } from './components/task-list/task-list.component';
import { TaskStartComponent } from './components/task-start/task-start.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AppRoutes } from '../../app.routes';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    TasksComponent,
    TaskItemComponent,
    TaskEditComponent,
    TaskDetailComponent,
    TaskListComponent,
    TaskStartComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(AppRoutes),
    FormsModule,
    ReactiveFormsModule,
    SharedModule
  ],
  providers: [
    TaskListComponent
  ]
})
export class TasksModule { }
