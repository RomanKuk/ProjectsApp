import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatSnackBar } from '@angular/material/snack-bar';

import { LocalizedDatePipe } from './pipes/localized-date.pipe';
import { DropdownDirective } from './directives/dropdown.directive';
import { UnsavedChangesGuard } from './guards/unsaved-changes.guard';
import { HeaderComponent } from './components/header/header.component';
import { RouterModule } from '@angular/router';
import { AppRoutes } from 'src/app/app.routes';
import { TaskStatusColorDirective } from './directives/task-status-color.directive';
import { TaskStatusStringPipe } from './pipes/task-status-string.pipe';

@NgModule({
  declarations: [
    LocalizedDatePipe,
    DropdownDirective,
    HeaderComponent,
    TaskStatusColorDirective,
    TaskStatusStringPipe
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(AppRoutes)
  ],
  exports: [
    LocalizedDatePipe,
    DropdownDirective,
    HeaderComponent,
    TaskStatusColorDirective,
    TaskStatusStringPipe
  ],
  providers: [ UnsavedChangesGuard, MatSnackBar ]
})
export class SharedModule { }
