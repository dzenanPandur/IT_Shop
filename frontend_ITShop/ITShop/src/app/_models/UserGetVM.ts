import {UserRoleGetVM} from "./UserRoleGetVM";

export class UserGetVM {
  public id: string;
  public firstName: string;
  public lastName: string;
  public phoneNumber: string;
  public userName: string;
  public email: string;
  public gender: any;
  public role: UserRoleGetVM[];
  public constructor() {
    this.id  = '';
    this.firstName='';
    this.lastName='';
    this.phoneNumber = '';
    this.userName='';
    this.email='';
    this.role = [];
  }
}

