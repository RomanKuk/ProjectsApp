import { Component, OnInit, OnDestroy } from '@angular/core';
import { User } from 'src/app/models/user/user.model';
import { Subject } from 'rxjs';
import { UserService } from 'src/app/services/user.service';
import { SnackBarService } from 'src/app/services/snack-bar.service';
import { Router, ActivatedRoute } from '@angular/router';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit, OnDestroy {
  users: User[] = [];
  private unsubscribe$ = new Subject<void>();

  constructor(
    private userService: UserService,
    private snackbarService: SnackBarService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.getUsers();
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
}

  public getUsers(): void {
    this.userService
        .getUsers()
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe(
            (resp) => {
                this.users = resp.body;
            },
            (error) => {this.snackbarService.showErrorMessage(error.message); }
        );
}

onNewUser(): void {
  this.router.navigate(['new'], {relativeTo: this.route});
}
}
