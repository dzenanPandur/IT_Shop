export class Auth{
  public username: string;
  public password: string;
  public verificationCode: string;

  public constructor() {
    this.username = '';
    this.password = '';
    this.verificationCode = ''
  }
}
