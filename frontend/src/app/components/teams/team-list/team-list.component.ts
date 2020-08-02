import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { Team } from 'src/app/models/team/team.model';
import { TeamService } from 'src/app/services/team.service';

@Component({
  selector: 'app-team-list',
  templateUrl: './team-list.component.html',
  styleUrls: ['./team-list.component.css']
})
export class TeamListComponent implements OnInit, OnDestroy {
  teams: Team[] = [];
  private unsubscribe$ = new Subject<void>();

  constructor(
    private teamService: TeamService
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
                this.teams = resp.body;
                if (this.teams.length !== 0)
                {
                  this.teamService.teamSelected.emit(this.teams[0]);
                }
            },
            (error) => {console.error(); }
        );
}
}
