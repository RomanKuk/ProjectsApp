import { Injectable } from '@angular/core';
import { CanActivate, CanActivateChild, Router, ActivatedRouteSnapshot, RouterStateSnapshot, CanDeactivate } from '@angular/router';
import { Observable } from 'rxjs';

export interface ComponentCanDeactivate {
    canDeactivate: () => boolean | Observable<boolean>;
  }

@Injectable()
export class UnsavedChangesGuard implements CanDeactivate<ComponentCanDeactivate> {
    canDeactivate(component: ComponentCanDeactivate): boolean | Observable<boolean> {
        return component.canDeactivate() ? true :
            confirm('You have unsaved changes.\nPress Cancel to go back and save these changes, or OK to lose these changes.');
    }
}
