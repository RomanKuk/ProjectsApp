import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LocalizedDatePipe } from './pipes/localized-date.pipe';
import { DropdownDirective } from './directives/dropdown.directive';
import { UnsavedChangesGuard } from './guards/unsaved-changes.guard';
import { HeaderComponent } from './components/header/header.component';
import { RouterModule } from '@angular/router';
import { AppRoutes } from 'src/app/app.routes';

@NgModule({
  declarations: [
    LocalizedDatePipe,
    DropdownDirective,
    HeaderComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(AppRoutes)
  ],
  exports: [
    LocalizedDatePipe,
    DropdownDirective,
    HeaderComponent
  ],
  providers: [ UnsavedChangesGuard ]
})
export class SharedModule { }