export class RegisterUser{

  public firstName: string;
  public lastName: string;
  public phoneNumber: string;
  public userName: string;
  public email: string;
  public password: string;
  public gender: number;
  public userRoles: string[];

  public constructor() {
    this.firstName='';
    this.lastName='';
    this.phoneNumber='';
    this.userName='';
    this.email='';
    this.password='';
    this.gender=1;
    this.userRoles=['022ff16d-b979-49f3-849f-08dae9da1c4e'];
  }

}
