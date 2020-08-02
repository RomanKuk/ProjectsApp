import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ProjectCreate } from 'src/app/models/project/project-create.model';
import { ProjectEdit } from 'src/app/models/project/project-edit.model';
import { Project } from 'src/app/models/project/project.model';
import { Team } from 'src/app/models/team/team.model';
import { User } from 'src/app/models/user/user.model';
import { ProjectService } from 'src/app/services/project.service';
import { TeamService } from 'src/app/services/team.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-project-edit',
  templateUrl: './project-edit.component.html',
  styleUrls: ['./project-edit.component.css']
})
export class ProjectEditComponent implements OnInit {
  project: Project;
  users: User[];
  teams: Team[];
  projectForm: FormGroup;
  editMode = false;
  private unsubscribe$ = new Subject<void>();
  id: number;
  
  constructor(
    private projectService: ProjectService,
    private teamService: TeamService,
    private userService: UserService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.route.params
    .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (params: Params) => {
            this.editMode = params['id'] != null;
            this.init();
            this.getUsers();
            this.getTeams();
            if(this.editMode)
              this.getProject(+params['id']);
        }
      )
  }

  onSubmit() {
    if (this.editMode) {
      this.updateProject(this.project.id, this.projectForm);
    } else {
      this.createProject(this.projectForm);
    }
  }

  onCancel() {
    this.router.navigate(['../'], {relativeTo: this.route});
  }

  public ngOnDestroy() {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public getProject(id: number) {
    this.projectService
        .getProjectById(id)
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe(
            (resp) => {
                this.project = resp.body;
                this.initForm(this.project);
            },
            () => {console.error()}
    );
  }

  private init() {
    this.projectForm = new FormGroup({
      'name': new FormControl('', Validators.required),
      'description' : new FormControl('', Validators.required),
      'deadline' : new FormControl('', Validators.required),
      'authorId' : new FormControl('', Validators.required),
      'teamId' : new FormControl('', Validators.required)
    });
  }

  private initForm(project: Project) {

    this.projectForm = new FormGroup({
      'name': new FormControl(project.name, Validators.required),
      'description' : new FormControl(project.description, Validators.required),
      'deadline' : new FormControl(new Date(project.deadline).toISOString().slice(0, 10), 
        Validators.required),
      'authorId' : new FormControl(project.author.id, Validators.required),
      'teamId' : new FormControl(project.team.id, Validators.required)
    });
 }

 public getTeams() {
  this.teamService
      .getTeams()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
          (resp) => {
              this.teams = resp.body;
          },
          () => {console.error()}
      );
}

public getUsers() {
  this.userService
      .getUsers()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
          (resp) => {
              this.users = resp.body;
          },
          () => {console.error()}
      );
}

 private updateProject(id: number, formGroup: FormGroup)
 {
    const updatedProject: ProjectEdit = formGroup.value;
    updatedProject.id = id;
    updatedProject.authorId = Number(formGroup.value['authorId']),
    updatedProject.teamId = Number(formGroup.value['teamId'])

    console.log(updatedProject);
    this.projectService.updateProject(updatedProject)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((resp) => {
        this.onCancel();
      },
      (error) =>console.log(error));
 }
 
 private createProject(formGroup: FormGroup)
 {
    const createdProject: ProjectCreate = formGroup.value;
    createdProject.authorId = Number(formGroup.value['authorId']),
    createdProject.teamId = Number(formGroup.value['teamId']);

    this.projectService.createProject(createdProject)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((resp) => {
        this.onCancel();
      },
          (error) =>console.log(error));
 }

}
