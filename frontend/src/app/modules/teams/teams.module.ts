import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TeamsComponent } from './teams.component';
import { TeamListComponent } from './components/team-list/team-list.component';
import { TeamItemComponent } from './components/team-list/team-item/team-item.component';
import { TeamDetailComponent } from './components/team-detail/team-detail.component';
import { TeamStartComponent } from './components/team-start/team-start.component';
import { TeamEditComponent } from './components/team-edit/team-edit.component';
import { RouterModule } from '@angular/router';
import { AppRoutes } from 'src/app/app.routes';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';



@NgModule({
  declarations: [
    TeamsComponent,
    TeamListComponent,
    TeamItemComponent,
    TeamDetailComponent,
    TeamStartComponent,
    TeamEditComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(AppRoutes),
    FormsModule,
    ReactiveFormsModule,
    SharedModule
  ],
  providers: [
    TeamListComponent
  ]
})
export class TeamsModule { }
