import { Component, OnInit, OnDestroy } from '@angular/core';
import { User } from 'src/app/models/user/user.model';
import { Subject } from 'rxjs';
import { UserService } from 'src/app/services/user.service';
import { UserListComponent } from '../user-list/user-list.component';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { takeUntil } from 'rxjs/operators';
import { Team } from 'src/app/models/team/team.model';
import { TeamService } from 'src/app/services/team.service';
import { SnackBarService } from 'src/app/services/snack-bar.service';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent implements OnInit, OnDestroy {
  user: User;
  team: Team;
  private unsubscribe$ = new Subject<void>();
  id: number;

  constructor(
    private userService: UserService,
    private teamService: TeamService,
    private userListComponent: UserListComponent,
    private snackbarService: SnackBarService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.params
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (params: Params) => {
            this.getUser(+params[`id`]);
        }
      );
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public getUser(id: number): void {
    this.userService
        .getUserById(id)
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe(
            (resp) => {
                this.user = resp.body;
                this.getTeam();
            },
            (error) => {this.snackbarService.showErrorMessage(error.message); }
    );
  }

  onEditUser(): void {
    this.router.navigate(['edit'], {relativeTo: this.route});
  }

  onDeleteUser(): void {
    this.userService
      .deleteUserById(this.user.id)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
      (resp) => {
        const index = this.userListComponent.users.indexOf(this.user);
        this.userListComponent.users.splice(index, 1);
        this.router.navigate(['../'], {relativeTo: this.route});
      },
      (error) => {this.snackbarService.showErrorMessage(error.message); }
  );
}

public getTeam(): void {
  this.teamService
      .getTeamById(this.user.teamId)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
          (resp) => {
              this.team = resp.body;
          },
          (error) => {this.snackbarService.showErrorMessage(error.message); }
      );
}
}
