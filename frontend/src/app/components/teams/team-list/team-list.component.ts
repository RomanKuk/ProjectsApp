import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Team } from 'src/app/models/team/team.model';
import { TeamService } from 'src/app/services/team.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-team-list',
  templateUrl: './team-list.component.html',
  styleUrls: ['./team-list.component.css']
})
export class TeamListComponent implements OnInit {
  @Output() teamWasSelected = new EventEmitter<Team>();
  teams: Team[] = [];
  firstTeam: Team;
  private unsubscribe$ = new Subject<void>();

  constructor(
    private teamService: TeamService
  ) { }

  ngOnInit() {
    this.getTeams();
  }

  public ngOnDestroy() {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
}

  onTeamSelected(team: Team) {
   this.teamWasSelected.emit(team);
  }

  public getTeams() {
    this.teamService
        .getTeams()
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe(
            (resp) => {
                this.teams = resp.body;
                if(this.teams.length !== 0)
                {
                  this.firstTeam = this.teams[0];
                  this.onTeamSelected(this.firstTeam);
                }
            },
            (error) => {console.error()}
        );
}
}
