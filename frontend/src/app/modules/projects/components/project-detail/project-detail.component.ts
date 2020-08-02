import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { Project } from '../../../../models/project/project.model';
import { ProjectService } from '../../../../services/project.service';
import { ProjectListComponent } from '../project-list/project-list.component';

@Component({
  selector: 'app-project-detail',
  templateUrl: './project-detail.component.html',
  styleUrls: ['./project-detail.component.css']
})
export class ProjectDetailComponent implements OnInit, OnDestroy  {
  project: Project;
  private unsubscribe$ = new Subject<void>();
  id: number;
  dateOptions = {
    weekday: 'short',
    year: 'numeric',
    month: 'long',
    day: 'numeric'
};

  constructor(
    private projectService: ProjectService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.params
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (params: Params) => {
            this.getProject(+params[`id`]);
        }
      );
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
            },
            () => {console.error(); }
    );
  }

  onEditProject(): void {
    this.router.navigate(['edit'], {relativeTo: this.route});
  }

  onDeleteProject(): void {
    this.projectService
      .deleteProjectById(this.project.id)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
      (resp) => {
        const index = ProjectListComponent.projects.indexOf(this.project);
        ProjectListComponent.projects.splice(index, 1);
        this.router.navigate(['../'], {relativeTo: this.route});
      },
      () => {console.error(); }
  );
}

}
