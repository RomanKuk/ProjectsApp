import { Component, OnInit, Input } from '@angular/core';
import { Project } from 'src/app/models/project/project.model';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ProjectService } from 'src/app/services/project.service';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-project-detail',
  templateUrl: './project-detail.component.html',
  styleUrls: ['./project-detail.component.css']
})
export class ProjectDetailComponent implements OnInit {
  project: Project;
  private unsubscribe$ = new Subject<void>();
  
  constructor(
    private projectService: ProjectService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.route.params
      .subscribe(
        (params: Params) => {
            this.getProject(+params['id']);
        }
      )
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
            },
            () => {console.error()}
    );
  }

  onEditProject() {
    this.router.navigate(['edit'], {relativeTo: this.route});
  }

}
