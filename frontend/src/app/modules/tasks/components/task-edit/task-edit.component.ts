import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
import { ComponentCanDeactivate } from 'src/app/modules/shared/guards/unsaved-changes.guard';
import { TaskModel } from 'src/app/models/task/task.model';
import { User } from 'src/app/models/user/user.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Subject, Observable } from 'rxjs';
import { TaskService } from 'src/app/services/task.service';
import { UserService } from 'src/app/services/user.service';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { takeUntil } from 'rxjs/operators';
import { TaskModelEdit } from 'src/app/models/task/task-edit.model';
import { TaskListComponent } from '../task-list/task-list.component';
import { TaskModelCreate } from 'src/app/models/task/task-create.model';
import { SnackBarService } from 'src/app/services/snack-bar.service';
import { ProjectService } from 'src/app/services/project.service';
import { Project } from 'src/app/models/project/project.model';
import { TaskState } from 'src/app/models/task-state/task-state.model';

@Component({
  selector: 'app-task-edit',
  templateUrl: './task-edit.component.html',
  styleUrls: ['./task-edit.component.css']
})
export class TaskEditComponent implements OnInit, OnDestroy, ComponentCanDeactivate  {
  task: TaskModel;
  users: User[];
  projects: Project[];
  taskForm: FormGroup;
  editMode = false;
  isChangesSaved = false;
  today: Date;
  keys = Object.keys;
  states = TaskState;
  private unsubscribe$ = new Subject<void>();

  constructor(
    private taskService: TaskService,
    private userService: UserService,
    private projectService: ProjectService,
    private snackbarService: SnackBarService,
    private taskListComponent: TaskListComponent,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  @HostListener('window:beforeunload')
  canDeactivate(): Observable<boolean> | boolean {
    return !this.taskForm.dirty || this.isChangesSaved;
  }

  ngOnInit(): void {
    this.route.params
    .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (params: Params) => {
            this.editMode = params[`id`] != null;
            this.initFormCreate();
            this.today = new Date();
            this.getUsers();
            this.getProjects();
            if (this.editMode)
            {
              this.getTask(+params[`id`]);
            }
        }
      );
  }

  onSubmit(): void {
    if (this.editMode) {
      this.updateTask(this.task.id, this.taskForm);
    } else {
      this.createTask(this.taskForm);
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

  public getTask(id: number): void {
    this.taskService
        .getTaskById(id)
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe(
            (resp) => {
                this.task = resp.body;
                this.initFormEdit(this.task);
            },
            (error) => {this.snackbarService.showErrorMessage(error.message); }
    );
  }

  private initFormCreate(): void {
    this.taskForm = new FormGroup({
      name: new FormControl('', Validators.required),
      description : new FormControl('', Validators.required),
      finishedAt : new FormControl('', Validators.required),
      state : new FormControl('', Validators.required),
      performerId : new FormControl('', Validators.required),
      projectId : new FormControl('', Validators.required)
    });
  }

  private initFormEdit(task: TaskModel): void {

    this.taskForm = new FormGroup({
      name: new FormControl(task.name, Validators.required),
      description : new FormControl(task.description, Validators.required),
      finishedAt : new FormControl(new Date(task.finishedAt).toISOString().slice(0, 10),
        Validators.required),
      state : new FormControl(task.state, Validators.required),
      performerId : new FormControl(task.performer.id, Validators.required),
      projectId : new FormControl('', Validators.required)
    });
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

public getProjects(): void {
  this.projectService
      .getProjects()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
          (resp) => {
              this.projects = resp.body;
          },
          (error) => {this.snackbarService.showErrorMessage(error.message); }
      );
}

 private updateTask(id: number, formGroup: FormGroup): void
 {
    const updatedTask: TaskModelEdit = formGroup.value;
    updatedTask.id = id;
    updatedTask.performerId = Number(formGroup.value[`performerId`]),
    updatedTask.projectId = this.task.projectId;

    console.log(updatedTask);
    this.taskService.updateTask(updatedTask)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((resp) => {
        const index = this.taskListComponent.tasks.findIndex(p => p.id === resp.body.id);
        this.taskListComponent.tasks[index] = resp.body;
        this.isChangesSaved = true;
        this.onCancel();
      },
      (error) => console.log(error));
 }

 private createTask(formGroup: FormGroup): void
 {
    const createdTask: TaskModelCreate = formGroup.value;
    createdTask.performerId = Number(formGroup.value[`authorId`]),
    createdTask.projectId = Number(formGroup.value[`projectId`]);

    this.taskService.createTask(createdTask)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((resp) => {
        this.taskListComponent.tasks.push(resp.body);
        this.isChangesSaved = true;
        this.onCancel(resp.body.id);
      },
      (error) => console.log(error));
 }

}
