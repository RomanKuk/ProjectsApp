import { Component, OnInit, OnDestroy } from '@angular/core';
import { TaskModel } from 'src/app/models/task/task.model';
import { Subject } from 'rxjs';
import { TaskService } from 'src/app/services/task.service';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { takeUntil } from 'rxjs/operators';
import { TaskListComponent } from '../task-list/task-list.component';
import { ProjectListComponent } from 'src/app/modules/projects/components/project-list/project-list.component';
import { SnackBarService } from 'src/app/services/snack-bar.service';
import { ProjectService } from 'src/app/services/project.service';
import { Project } from 'src/app/models/project/project.model';

@Component({
  selector: 'app-task-detail',
  templateUrl: './task-detail.component.html',
  styleUrls: ['./task-detail.component.css']
})
export class TaskDetailComponent implements OnInit, OnDestroy {

  task: TaskModel;
  project: Project;
  private unsubscribe$ = new Subject<void>();
  id: number;

  constructor(
    private taskService: TaskService,
    private projectService: ProjectService,
    private snackbarService: SnackBarService,
    private taskListComponent: TaskListComponent,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.params
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (params: Params) => {
            this.getTask(+params[`id`]);
        }
      );
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
                this.getProject();
            },
            (error) => {this.snackbarService.showErrorMessage(error.message); }
    );
  }

  onEditTask(): void {
    this.router.navigate(['edit'], {relativeTo: this.route});
  }

  onDeleteTask(): void {
    this.taskService
      .deleteTaskById(this.task.id)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
      (resp) => {
        const index = this.taskListComponent.tasks.indexOf(this.task);
        this.taskListComponent.tasks.splice(index, 1);
        this.router.navigate(['../'], {relativeTo: this.route});
      },
      (error) => {this.snackbarService.showErrorMessage(error.message); }
  );
}

  getProject(): void
  {
    const project = this.projectService
      .getProjectById(this.task.projectId)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
      (resp) => {
        this.project = resp.body;
      },
      (error) => {this.snackbarService.showErrorMessage(error.message); }
  );
  }
}
