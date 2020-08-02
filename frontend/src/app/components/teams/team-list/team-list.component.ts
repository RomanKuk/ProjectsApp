import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { Team } from 'src/app/models/team/team.model';
import { TeamService } from 'src/app/services/team.service';

@Component({
  selector: 'app-team-list',
  templateUrl: './team-list.component.html',
  styleUrls: ['./team-list.component.css']
})
export class TeamListComponent implements OnInit {
  teams: Team[] = [];
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

  public getTeams() {
    this.teamService
        .getTeams()
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe(
            (resp) => {
                this.teams = resp.body;
                if(this.teams.length !== 0)
                {
                  this.teamService.teamSelected.emit(this.teams[0]);
                }
            },
            (error) => {console.error()}
        );
}
}
