import { Injectable } from '@angular/core';
import { HttpInternalService } from './http-internal.service';
import { Team } from '../models/team/team.model';
import { TeamCreate } from '../models/team/team-create.model';
import { TeamEdit } from '../models/team/team-edit.model';
import { TeamUsers } from '../models/function-models/team-users.model';

@Injectable({ providedIn: 'root' })
export class TeamService {
    public routePrefix = '/api/teams';

    constructor(private httpService: HttpInternalService) { }

    public getTeams() {
        return this.httpService.getFullRequest<Team[]>(`${this.routePrefix}`);
    }

    public getTeamById(teamId: number) {
        return this.httpService.getFullRequest<Team>(`${this.routePrefix}/${teamId}`);
    }

    public createTeam(team: TeamCreate) {
        return this.httpService.postFullRequest<Team>(`${this.routePrefix}`, team);
    }

    public updateTeam(team: TeamEdit) {
        return this.httpService.putFullRequest<Team>(`${this.routePrefix}`, team);
    }

    public deleteTeamById(teamId: number) {
        return this.httpService.deleteFullRequest(`${this.routePrefix}/${teamId}`);
    }

    public getTeamsUsersSortedByRegisteredDate()
    {
        return this.httpService.getFullRequest<TeamUsers[]>(`${this.routePrefix}/team-users-sorted-by-registered-date`);
    }
}
