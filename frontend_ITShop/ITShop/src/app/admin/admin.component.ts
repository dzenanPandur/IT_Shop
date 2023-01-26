import { Component, OnInit } from '@angular/core';
import {UserGetVM} from "../_models/UserGetVM";
import {HttpClient} from "@angular/common/http";
import {Globals} from "../globals";
import {UserRoleGetVM} from "../_models/UserRoleGetVM";

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  tableData?: UserGetVM[];
  selectData?: UserRoleGetVM[];

  constructor( public http: HttpClient,
               public globals: Globals) {
 }

  ngOnInit(): void {
    this.loadUserData();
    this.loadRoles();
  }
  loadUserData() {
    this.http.get(this.globals.serverAddress + "/User")
      .subscribe({
        next: (value: any) => {
          this.tableData = value.data;

          console.log(this.tableData);
        },
        error: (err: any) => {
          alert("error");
          console.log(err);
        }});
  }
  loadRoles() {
    this.http.get(this.globals.serverAddress + '/Role/get-roles').subscribe({
      next: (value: any)=> {
        this.selectData = value.data;
        console.log(this.selectData);
      },
      error: (err: any) => {
        alert("error");
        console.log(err);}
    })
  }
  changeRole(roleId: string, userId: string){
    let request = {
      roleId: roleId  , //
      userId: userId
    };
    this.http.put(this.globals.serverAddress + '/Role/update-role', request).subscribe(data => {
      this.loadUserData();

    });
  }

}
