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

  @Output() userWasSelected = new EventEmitter<User>();
  users: User[] = [];
  firstUser: User;
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

  onUserSelected(user: User) {
   this.userWasSelected.emit(user);
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
                  this.firstUser = this.users[0];
                  this.onUserSelected(this.firstUser);
                }
            },
            (error) => {console.error()}
        );
}
}
