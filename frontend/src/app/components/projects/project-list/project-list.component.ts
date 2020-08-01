import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Project } from 'src/app/models/project/project.model';
import { ProjectService } from 'src/app/services/project.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-project-list',
  templateUrl: './project-list.component.html',
  styleUrls: ['./project-list.component.css']
})
export class ProjectListComponent implements OnInit {
  projects: Project[] = [];
  private unsubscribe$ = new Subject<void>();

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
                this.projects = resp.body;
            },
            () => {console.error()}
        );
}

onNewProject() {
  this.router.navigate(['new'], {relativeTo: this.route});
}

}
