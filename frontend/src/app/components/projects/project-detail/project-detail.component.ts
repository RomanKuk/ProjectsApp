import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Subject } from 'rxjs';
import { Project } from 'src/app/models/project/project.model';
import { ProjectService } from 'src/app/services/project.service';

@Component({
  selector: 'app-project-detail',
  templateUrl: './project-detail.component.html',
  styleUrls: ['./project-detail.component.css']
})
export class ProjectDetailComponent implements OnInit {
  project: Project;
  private unsubscribe$ = new Subject<void>();
  id: number;
  
  constructor(
    private projectService: ProjectService,
    // private translateService: TranslateService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    // this.translateService.use('en');
   }

  ngOnInit(): void {
    this.route.params
      .subscribe(
        (params: Params) => {
            this.getProject(+params['id']);
        }
      );
  }

  public ngOnDestroy() {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public getProject(id: number) {
    this.projectService
        .getProjectById(id)
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
