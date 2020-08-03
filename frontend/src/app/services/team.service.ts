import { Injectable, EventEmitter } from '@angular/core';
import { HttpInternalService } from './http-internal.service';
import { Team } from '../models/team/team.model';
import { TeamCreate } from '../models/team/team-create.model';
import { TeamEdit } from '../models/team/team-edit.model';
import { TeamUsers } from '../models/function-models/team-users.model';
import { Observable } from 'rxjs/internal/Observable';
import { HttpResponse } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class TeamService {
    public routePrefix = '/api/teams';
    teamSelected = new EventEmitter<Team>();

    constructor(private httpService: HttpInternalService) { }

    public getTeams(): Observable<HttpResponse<Team[]>> {
        return this.httpService.getFullRequest<Team[]>(`${this.routePrefix}`);
    }

    public getTeamById(teamId: number): Observable<HttpResponse<Team>> {
        return this.httpService.getFullRequest<Team>(`${this.routePrefix}/${teamId}`);
    }

    public createTeam(team: TeamCreate): Observable<HttpResponse<Team>> {
        return this.httpService.postFullRequest<Team>(`${this.routePrefix}`, team);
    }

    public updateTeam(team: TeamEdit): Observable<HttpResponse<Team>> {
        return this.httpService.putFullRequest<Team>(`${this.routePrefix}`, team);
    }

    public deleteTeamById(teamId: number): Observable<HttpResponse<void>> {
        return this.httpService.deleteFullRequest(`${this.routePrefix}/${teamId}`);
    }

    public getTeamsUsersSortedByRegisteredDate(): Observable<HttpResponse<TeamUsers[]>>
    {
        return this.httpService.getFullRequest<TeamUsers[]>(`${this.routePrefix}/team-users-sorted-by-registered-date`);
    }
}
