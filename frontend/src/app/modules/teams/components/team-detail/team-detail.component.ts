import { Component, OnInit, OnDestroy } from '@angular/core';
import { Team } from 'src/app/models/team/team.model';
import { Subject } from 'rxjs';
import { TeamService } from 'src/app/services/team.service';
import { TeamListComponent } from '../team-list/team-list.component';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { takeUntil } from 'rxjs/operators';
import { SnackBarService } from 'src/app/services/snack-bar.service';

@Component({
  selector: 'app-team-detail',
  templateUrl: './team-detail.component.html',
  styleUrls: ['./team-detail.component.css']
})
export class TeamDetailComponent implements OnInit, OnDestroy {
  team: Team;
  private unsubscribe$ = new Subject<void>();
  id: number;

  constructor(
    private teamService: TeamService,
    private teamListComponent: TeamListComponent,
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
        const index = this.teamListComponent.teams.indexOf(this.team);
        this.teamListComponent.teams.splice(index, 1);
        this.router.navigate(['../'], {relativeTo: this.route});
      },
      (error) => {this.snackbarService.showErrorMessage(error.message); }
  );
}
}
