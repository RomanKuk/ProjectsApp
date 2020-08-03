import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsersComponent } from './users.component';
import { UserListComponent } from './user-list/user-list.component';
import { UserItemComponent } from './user-list/user-item/user-item.component';
import { UserDetailComponent } from './user-detail/user-detail.component';
import { UserEditComponent } from './user-edit/user-edit.component';
import { UserStartComponent } from './user-start/user-start.component';
import { RouterModule } from '@angular/router';
import { AppRoutes } from 'src/app/app.routes';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';



@NgModule({
  declarations: [
    UsersComponent,
    UserListComponent,
    UserItemComponent,
    UserDetailComponent,
    UserEditComponent,
    UserStartComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(AppRoutes),
    FormsModule,
    ReactiveFormsModule,
    SharedModule
  ],
  providers: [
    UserListComponent
  ]
})
export class UsersModule { }
