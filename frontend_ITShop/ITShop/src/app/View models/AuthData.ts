export class AuthData{
  public userId: string;
  public firstName: string;
  public lastName: string;
  public token: string;
  public phoneNumber: string;
  public email: string;
  public tokenExpireDate : any;
  public gender: number;
  public roles: string[];

  public constructor() {
    this.userId = '';
    this.firstName = '';
    this.lastName = '';
    this.token = '';
    this.phoneNumber = '';
    this.email = '';
    this.tokenExpireDate = '';
    this.gender = 0;
    this.roles = [];
  }
}
