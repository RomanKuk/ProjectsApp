import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { Project } from '../../../../models/project/project.model';
import { ProjectService } from '../../../../services/project.service';
import { SnackBarService } from 'src/app/services/snack-bar.service';

@Component({
  selector: 'app-project-list',
  templateUrl: './project-list.component.html',
  styleUrls: ['./project-list.component.css']
})
export class ProjectListComponent implements OnInit, OnDestroy {
  static projects: Project[] = [];
  private unsubscribe$ = new Subject<void>();

  constructor(
    private projectService: ProjectService,
    private snackbarService: SnackBarService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.getProjects();
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
}

  public getProjects(): void {
    this.projectService
        .getProjects()
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe(
            (resp) => {
                ProjectListComponent.projects = resp.body;
            },
            (error) => {this.snackbarService.showErrorMessage(error.message); }
        );
}

onNewProject(): void {
  this.router.navigate(['new'], {relativeTo: this.route});
}

get getProjectsValue(): Project[]
{
  return ProjectListComponent.projects;
}

}
