import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { User } from 'src/app/models/user/user.model';
import { UserService } from 'src/app/services/user.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  users: User[] = [];
  private unsubscribe$ = new Subject<void>();

  constructor(
    private userService: UserService
  ) { }

  ngOnInit() {
    this.getUsers();
  }

  public ngOnDestroy() {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
}

  public getUsers() {
    this.userService
        .getUsers()
        .pipe(takeUntil(this.unsubscribe$))
        .subscribe(
            (resp) => {
                this.users = resp.body;
                if(this.users.length !== 0)
                {
                  this.userService.userSelected.emit(this.users[0]);
                }
            },
            (error) => {console.error()}
        );
}
}
