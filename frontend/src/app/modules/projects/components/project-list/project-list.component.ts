import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { Project } from '../../../../models/project/project.model';
import { ProjectService } from '../../../../services/project.service';

@Component({
  selector: 'app-project-list',
  templateUrl: './project-list.component.html',
  styleUrls: ['./project-list.component.css']
})
export class ProjectListComponent implements OnInit {
  private unsubscribe$ = new Subject<void>();
  static projects: Project[] = [];

  constructor(
    private projectService: ProjectService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.getProjects();
  }

  public ngOnDestroy() {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
}

  public getProjects() {
    this.projectService
        .getProjects()
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe(
            (resp) => {
                ProjectListComponent.projects = resp.body;
            },
            () => {console.error()}
        );
}

onNewProject() {
  this.router.navigate(['new'], {relativeTo: this.route});
}

get getProjectsValue()
{
  return ProjectListComponent.projects;
}

}
