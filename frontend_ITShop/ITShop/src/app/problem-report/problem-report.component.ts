import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Globals} from "../globals";

@Component({
  selector: 'app-problem-report',
  templateUrl: './problem-report.component.html',
  styleUrls: ['./problem-report.component.css']
})
export class ProblemReportComponent implements OnInit {

  constructor(
    public http: HttpClient,
    public globals: Globals
  ) { }

  title:any;
  message:any;
  description:any;

  ngOnInit(): void {
  }


  submit() {

    this.http.post<any>(this.globals.serverAddress + '/SendGrid/send-email-troubleshoot?title='+this.title +'&message='+this.message+'&description='+this.description, null ).subscribe(data=>{

    })
    window.location.reload();

  }
}
