import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Subject, Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ProjectCreate } from '../../../../models/project/project-create.model';
import { ProjectEdit } from '../../../../models/project/project-edit.model';
import { Project } from '../../../../models/project/project.model';
import { Team } from '../../../../models/team/team.model';
import { User } from '../../../../models/user/user.model';
import { ProjectService } from '../../../../services/project.service';
import { TeamService } from '../../../../services/team.service';
import { UserService } from '../../../../services/user.service';
import { SnackBarService } from '../../../../services/snack-bar.service';
import { ProjectListComponent } from '../project-list/project-list.component';
import { ComponentCanDeactivate } from 'src/app/modules/shared/guards/unsaved-changes.guard';
import { ProjectsComponent } from '../../projects.component';

@Component({
  selector: 'app-project-edit',
  templateUrl: './project-edit.component.html',
  styleUrls: ['./project-edit.component.css']
})

export class ProjectEditComponent implements OnInit, OnDestroy, ComponentCanDeactivate  {
  project: Project;
  users: User[];
  teams: Team[];
  projectForm: FormGroup;
  editMode = false;
  isChangesSaved = false;
  today: Date;
  private unsubscribe$ = new Subject<void>();

  constructor(
    private projectService: ProjectService,
    private teamService: TeamService,
    private userService: UserService,
    private snackbarService: SnackBarService,
    private projectListComponent: ProjectListComponent,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  @HostListener('window:beforeunload')
  canDeactivate(): Observable<boolean> | boolean {
    return !this.projectForm.dirty || this.isChangesSaved;
  }

  ngOnInit(): void {
    this.route.params
    .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (params: Params) => {
            this.editMode = params[`id`] != null;
            this.initFormCreate();
            this.getUsers();
            this.getTeams();
            this.today = new Date();
            if (this.editMode)
            {
              this.getProject(+params[`id`]);
            }
        }
      );
  }

  onSubmit(): void {
    if (this.editMode) {
      this.updateProject(this.project.id, this.projectForm);
    } else {
      this.createProject(this.projectForm);
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

  public getProject(id: number): void {
    this.projectService
        .getProjectById(id)
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe(
            (resp) => {
                this.project = resp.body;
                this.initFormEdit(this.project);
            },
            (error) => {this.snackbarService.showErrorMessage(error.message); }
    );
  }

  private initFormCreate(): void {
    this.projectForm = new FormGroup({
      name: new FormControl('', Validators.required),
      description : new FormControl('', Validators.required),
      deadline : new FormControl('', Validators.required),
      authorId : new FormControl('', Validators.required),
      teamId : new FormControl('', Validators.required)
    });
  }

  private initFormEdit(project: Project): void {
    this.projectForm = new FormGroup({
      name: new FormControl(project.name, Validators.required),
      description : new FormControl(project.description, Validators.required),
      deadline : new FormControl(new Date(project.deadline).toLocaleDateString('en-CA'),
        Validators.required),
      authorId : new FormControl(project.author.id, Validators.required),
      teamId : new FormControl(project.team.id, Validators.required)
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

 private updateProject(id: number, formGroup: FormGroup): void
 {
    const updatedProject: ProjectEdit = formGroup.value;
    updatedProject.id = id;
    updatedProject.authorId = Number(formGroup.value[`authorId`]),
    updatedProject.teamId = Number(formGroup.value[`teamId`]);

    this.projectService.updateProject(updatedProject)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((resp) => {
        const index = ProjectListComponent.projects.findIndex(p => p.id === resp.body.id);
        ProjectListComponent.projects[index] = resp.body;
        this.isChangesSaved = true;
        this.onCancel();
      },
      (error) => this.snackbarService.showErrorMessage(error.message));
 }

 private createProject(formGroup: FormGroup): void
 {
    const createdProject: ProjectCreate = formGroup.value;
    createdProject.authorId = Number(formGroup.value[`authorId`]),
    createdProject.teamId = Number(formGroup.value[`teamId`]);

    this.projectService.createProject(createdProject)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((resp) => {
        ProjectListComponent.projects.push(resp.body);
        this.isChangesSaved = true;
        this.onCancel(resp.body.id);
      },
      (error) => {this.snackbarService.showErrorMessage(error.message); } );
 }

}
