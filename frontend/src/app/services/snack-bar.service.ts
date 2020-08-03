import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({ providedIn: 'root' })
export class SnackBarService {
    public constructor(private snackBar: MatSnackBar) {}

    public showErrorMessage(error: any): void {
        this.snackBar.open(error, '', { duration: 3000, panelClass: 'error-snack-bar' });
    }

    public showUsualMessage(message: any): void {
        this.snackBar.open(message, '', { duration: 3000, panelClass: 'usual-snack-bar' });
    }
}
