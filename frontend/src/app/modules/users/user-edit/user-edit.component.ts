import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
import { User } from 'src/app/models/user/user.model';
import { Team } from 'src/app/models/team/team.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Subject, Observable } from 'rxjs';
import { UserService } from 'src/app/services/user.service';
import { TeamService } from 'src/app/services/team.service';
import { SnackBarService } from 'src/app/services/snack-bar.service';
import { UserListComponent } from '../user-list/user-list.component';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { takeUntil } from 'rxjs/operators';
import { UserEdit } from 'src/app/models/user/user-edit.model';
import { UserCreate } from 'src/app/models/user/user-create.model';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit, OnDestroy {
  user: User;

  teams: Team[];
  userForm: FormGroup;
  editMode = false;
  isChangesSaved = false;
  today: Date;
  private unsubscribe$ = new Subject<void>();

  constructor(
    private userService: UserService,
    private teamService: TeamService,
    private snackbarService: SnackBarService,
    private userListComponent: UserListComponent,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  @HostListener('window:beforeunload')
  canDeactivate(): Observable<boolean> | boolean {
    return !this.userForm.dirty || this.isChangesSaved;
  }

  ngOnInit(): void {
    this.route.params
    .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (params: Params) => {
            this.editMode = params[`id`] != null;
            this.initFormCreate();
            this.today = new Date();
            this.getTeams();
            if (this.editMode)
            {
              this.getUser(+params[`id`]);
            }
        }
      );
  }

  onSubmit(): void {
    if (this.editMode) {
      this.updateUser(this.user.id, this.userForm);
    } else {
      this.createUser(this.userForm);
    }
  }

  onCancel(id: number = null): void {
    if (this.editMode)
    {
      this.onCancelEdit();
    }
    else {
      this.onCancelCreate(id);
    }
  }

  onCancelEdit(): void {
    this.router.navigate(['../'], {relativeTo: this.route});
  }
  onCancelCreate(id: number): void {
    if (id !== null)
    {
      this.router.navigate([`../${id}`], {relativeTo: this.route});
    } else {
      this.router.navigate(['../'], {relativeTo: this.route});
    }
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
                this.initFormEdit(this.user);
            },
            (error) => {this.snackbarService.showErrorMessage(error.message); }
    );
  }

  private initFormCreate(): void {
    this.userForm = new FormGroup({
      firstName: new FormControl('', Validators.required),
      lastName : new FormControl('', Validators.required),
      email : new FormControl('', Validators.required),
      birthday : new FormControl('', Validators.required),
      teamId : new FormControl('', Validators.required),
    });
  }

  private initFormEdit(user: User): void {

    this.userForm = new FormGroup({
      firstName: new FormControl(user.firstName, Validators.required),
      lastName : new FormControl(user.lastName, Validators.required),
      email : new FormControl(user.email, Validators.required),
      birthday : new FormControl(new Date(user.birthday).toLocaleDateString('en-CA'),
       Validators.required),
      teamId : new FormControl(user.teamId, Validators.required),
    });
 }

public getTeams(): void {
  this.teamService
      .getTeams()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
          (resp) => {
              this.teams = resp.body;
          },
          (error) => {this.snackbarService.showErrorMessage(error.message); }
      );
}

 private updateUser(id: number, formGroup: FormGroup): void
 {
    const updatedUser: UserEdit = formGroup.value;
    updatedUser.id = id;
    updatedUser.teamId = Number(formGroup.value[`teamId`]),

    this.userService.updateUser(updatedUser)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((resp) => {
        const index = UserListComponent.users.findIndex(p => p.id === resp.body.id);
        UserListComponent.users[index] = resp.body;
        this.isChangesSaved = true;
        this.onCancel();
      },
      (error) => {this.snackbarService.showErrorMessage(error.message); });
 }

 private createUser(formGroup: FormGroup): void
 {
    const createdUser: UserCreate = formGroup.value;
    createdUser.teamId = Number(formGroup.value[`teamId`]),

    this.userService.createUser(createdUser)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((resp) => {
        UserListComponent.users.push(resp.body);
        this.isChangesSaved = true;
        this.onCancel(resp.body.id);
      },
      (error) => {this.snackbarService.showErrorMessage(error.message); });
 }

}
