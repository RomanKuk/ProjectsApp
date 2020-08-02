import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { Project } from 'src/app/models/project/project.model';
import { ProjectService } from 'src/app/services/project.service';

@Component({
  selector: 'app-project-list',
  templateUrl: './project-list.component.html',
  styleUrls: ['./project-list.component.css']
})
export class ProjectListComponent implements OnInit {
  //projects: Project[] = [];
  @Input() newProject: Project;
  //projectsChanged = new Subject<Project[]>();
  private unsubscribe$ = new Subject<void>();
  static projects: Project[] = [];
  //static projectsChanged: any;

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
// public static addProject(project: Project) {
//   this.projects.push(project);
//   this.projectsChanged.next(this.projects.slice());
// }

}
