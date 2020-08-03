import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
import { Team } from 'src/app/models/team/team.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Subject, Observable } from 'rxjs';
import { TeamService } from 'src/app/services/team.service';
import { SnackBarService } from 'src/app/services/snack-bar.service';
import { TeamListComponent } from '../team-list/team-list.component';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { takeUntil } from 'rxjs/operators';
import { TeamEdit } from 'src/app/models/team/team-edit.model';
import { TeamCreate } from 'src/app/models/team/team-create.model';

@Component({
  selector: 'app-team-edit',
  templateUrl: './team-edit.component.html',
  styleUrls: ['./team-edit.component.css']
})
export class TeamEditComponent implements OnInit, OnDestroy {
  team: Team;
  teamForm: FormGroup;
  editMode = false;
  isChangesSaved = false;
  private unsubscribe$ = new Subject<void>();

  constructor(
    private teamService: TeamService,
    private snackbarService: SnackBarService,
    private teamListComponent: TeamListComponent,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  @HostListener('window:beforeunload')
  canDeactivate(): Observable<boolean> | boolean {
    return !this.teamForm.dirty || this.isChangesSaved;
  }

  ngOnInit(): void {
    this.route.params
    .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (params: Params) => {
            this.editMode = params[`id`] != null;
            this.initFormCreate();
            if (this.editMode)
            {
              this.getTeam(+params[`id`]);
            }
        }
      );
  }

  onSubmit(): void {
    if (this.editMode) {
      this.updateTeam(this.team.id, this.teamForm);
    } else {
      this.createTeam(this.teamForm);
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

  public getTeam(id: number): void {
    this.teamService
        .getTeamById(id)
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe(
            (resp) => {
                this.team = resp.body;
                this.initFormEdit(this.team);
            },
            (error) => {this.snackbarService.showErrorMessage(error.message); }
    );
  }

  private initFormCreate(): void {
    this.teamForm = new FormGroup({
      name: new FormControl('', Validators.required)
    });
  }

  private initFormEdit(team: Team): void {
    this.teamForm = new FormGroup({
      name: new FormControl(team.name, Validators.required)
    });
 }

 private updateTeam(id: number, formGroup: FormGroup): void
 {
    const updatedTeam: TeamEdit = formGroup.value;
    updatedTeam.id = id;

    this.teamService.updateTeam(updatedTeam)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((resp) => {
        const index = TeamListComponent.teams.findIndex(p => p.id === resp.body.id);
        TeamListComponent.teams[index] = resp.body;
        this.isChangesSaved = true;
        this.onCancel();
      },
      (error) => this.snackbarService.showErrorMessage(error.message));
 }

 private createTeam(formGroup: FormGroup): void
 {
    const createdTeam: TeamCreate = formGroup.value;

    this.teamService.createTeam(createdTeam)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((resp) => {
        TeamListComponent.teams.push(resp.body);
        this.isChangesSaved = true;
        this.onCancel(resp.body.id);
      },
      (error) => {this.snackbarService.showErrorMessage(error.message); } );
 }

}
