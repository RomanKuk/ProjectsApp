import { Component, OnInit, OnDestroy } from '@angular/core';
import { Team } from 'src/app/models/team/team.model';
import { Subject } from 'rxjs';
import { TeamService } from 'src/app/services/team.service';
import { SnackBarService } from 'src/app/services/snack-bar.service';
import { Router, ActivatedRoute } from '@angular/router';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-team-list',
  templateUrl: './team-list.component.html',
  styleUrls: ['./team-list.component.css']
})
export class TeamListComponent implements OnInit, OnDestroy {
  static teams: Team[] = [];
  private unsubscribe$ = new Subject<void>();

  constructor(
    private teamService: TeamService,
    private snackbarService: SnackBarService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.getTeams();
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
}

  public getTeams(): void {
    this.teamService
        .getTeams()
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe(
            (resp) => {
                TeamListComponent.teams = resp.body;
            },
            (error) => {this.snackbarService.showErrorMessage(error.message); }
        );
}

onNewTeam(): void {
  this.router.navigate(['new'], {relativeTo: this.route});
}

get getTeamsValue(): Team[]
{
  return TeamListComponent.teams;
}
}
