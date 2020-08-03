import { Component, OnInit, OnDestroy } from '@angular/core';
import { Team } from 'src/app/models/team/team.model';
import { Subject } from 'rxjs';
import { TeamService } from 'src/app/services/team.service';
import { TeamListComponent } from '../team-list/team-list.component';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { takeUntil } from 'rxjs/operators';
import { SnackBarService } from 'src/app/services/snack-bar.service';
import { Project } from 'src/app/models/project/project.model';
import { ProjectService } from 'src/app/services/project.service';

@Component({
  selector: 'app-team-detail',
  templateUrl: './team-detail.component.html',
  styleUrls: ['./team-detail.component.css']
})
export class TeamDetailComponent implements OnInit, OnDestroy {
  team: Team;
  projects: Project[];
  private unsubscribe$ = new Subject<void>();
  id: number;

  constructor(
    private teamService: TeamService,
    private projectService: ProjectService,
    private snackbarService: SnackBarService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.params
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
        (params: Params) => {
            this.getTeam(+params[`id`]);
        }
      );
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public getTeam(id: number): void {
    this.teamService
        .getTeamById(id)
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe(
            (resp) => {
                this.team = resp.body;
                this.getProjects();
            },
            (error) => {this.snackbarService.showErrorMessage(error.message); }
    );
  }

  onEditTeam(): void {
    this.router.navigate(['edit'], {relativeTo: this.route});
  }

  onDeleteTeam(): void {
    this.teamService
      .deleteTeamById(this.team.id)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
      (resp) => {
        const index = TeamListComponent.teams.indexOf(this.team);
        TeamListComponent.teams.splice(index, 1);
        this.router.navigate(['../'], {relativeTo: this.route});
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
              this.projects = resp.body.filter(item => item.team.id === this.team.id);
          },
          (error) => {this.snackbarService.showErrorMessage(error.message); }
      );
}
}
