import { Component, OnInit, OnDestroy } from '@angular/core';
import { User } from 'src/app/models/user/user.model';
import { Subject } from 'rxjs';
import { UserService } from 'src/app/services/user.service';
import { UserListComponent } from '../user-list/user-list.component';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { takeUntil } from 'rxjs/operators';
import { Team } from 'src/app/models/team/team.model';
import { TeamService } from 'src/app/services/team.service';
import { SnackBarService } from 'src/app/services/snack-bar.service';
import { Project } from 'src/app/models/project/project.model';
import { TaskModel } from 'src/app/models/task/task.model';
import { TaskService } from 'src/app/services/task.service';
import { ProjectService } from 'src/app/services/project.service';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent implements OnInit, OnDestroy {
  user: User;
  team: Team;
  projects: Project[];
  tasks: TaskModel[];
  private unsubscribe$ = new Subject<void>();
  id: number;

  constructor(
    private userService: UserService,
    private teamService: TeamService,
    private taskService: TaskService,
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
            this.getUser(+params[`id`]);
        }
      );
  }

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  public getUser(id: number): void {
    this.userService
        .getUserById(id)
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe(
            (resp) => {
                this.user = resp.body;
                this.getProjects();
                this.getTasks();
                if (this.user.teamId != null)
                {
                    this.getTeam();
                }
            },
            (error) => {this.snackbarService.showErrorMessage(error.message); }
    );
  }

  onEditUser(): void {
    this.router.navigate(['edit'], {relativeTo: this.route});
  }

  onDeleteUser(): void {
    this.userService
      .deleteUserById(this.user.id)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
      (resp) => {
        const index = UserListComponent.users.indexOf(this.user);
        UserListComponent.users.splice(index, 1);
        this.router.navigate(['../'], {relativeTo: this.route});
      },
      (error) => {this.snackbarService.showErrorMessage(error.message); }
  );
}

public getTeam(): void {
  this.teamService
      .getTeamById(this.user.teamId)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
          (resp) => {
              this.team = resp.body;
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
              this.projects = resp.body.filter(item => item.author.id === this.user.id);
          },
          (error) => {this.snackbarService.showErrorMessage(error.message); }
      );
}

public getTasks(): void {
  this.taskService
      .getTasks()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(
          (resp) => {
              this.tasks = resp.body.filter(item => item.performer.id === this.user.id);
          },
          (error) => {this.snackbarService.showErrorMessage(error.message); }
      );
}
}
