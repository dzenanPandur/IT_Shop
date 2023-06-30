import { Component, OnInit } from '@angular/core';
import { UserGetVM } from "../_models/UserGetVM";
import { HttpClient } from "@angular/common/http";
import { Globals } from "../globals";
import { UserRoleGetVM } from "../_models/UserRoleGetVM";

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  tableData?: UserGetVM[];
  selectData?: UserRoleGetVM[];
  currentPage = 1;
  itemsPerPage = 10;
  totalPages = 0;
  visibleTableData: UserGetVM[] = [];
  allPages: number[] = [];
  role: string ="All";
  isDownloadChecked: boolean = false;
  currentDate = new Date();
  report_name: string="";
  fromDate: Date;
  toDate: Date;

  constructor(public http: HttpClient, public globals: Globals) {
    this.fromDate = new Date("2020-06-30T17:06:50.930Z");
    this.toDate = new Date();
  }

  ngOnInit(): void {
    this.loadUserData();
    this.loadRoles();
  }

  loadUserData() {
    this.http.get(this.globals.serverAddress + "/User").subscribe({
      next: (value: any) => {
        this.tableData = value.data;
        if (this.tableData) {
          this.totalPages = Math.ceil(this.tableData.length / this.itemsPerPage);
          this.updateTableData();
          this.updateAllPages();
        }
        console.log(this.tableData);
      },
      error: (err: any) => {
        alert("error");
        console.log(err);
      }
    });
  }

  loadRoles() {
    this.http.get(this.globals.serverAddress + '/Role/get-roles').subscribe({
      next: (value: any) => {
        this.selectData = value.data;
        console.log(this.selectData);
      },
      error: (err: any) => {
        alert("error");
        console.log(err);
      }
    });
  }

  changeRole(roleId: string, userId: string) {
    let request = {
      roleId: roleId,
      userId: userId
    };
    this.http.put(this.globals.serverAddress + '/Role/update-role', request).subscribe(data => {
      this.loadUserData();
    });
  }

  updateTableData() {
    if (this.tableData) {
      const startIndex = (this.currentPage - 1) * this.itemsPerPage;
      const endIndex = startIndex + this.itemsPerPage;
      this.visibleTableData = this.tableData.slice(startIndex, endIndex);
    }
  }

  goToPreviousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updateTableData();
    }
  }

  goToNextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.updateTableData();
    }
  }

  goToPage(page: number) {
    this.currentPage = page;
    this.updateTableData();
  }

  updateAllPages() {
    this.allPages = Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }

  generateReport() {
    // Define the report parameters
    const parameters = {
      role: this.role,
      fromDate: this.fromDate,
      toDate: this.toDate
    };
    // Make an HTTP POST request to the server to generate the report
    this.http.post(this.globals.serverAddress + '/Report/user-report', parameters, {responseType: 'blob'})
      .subscribe(response => {
        // Create a Blob URL from the response
        const blob = new Blob([response], {type: 'application/pdf'});
        const url = window.URL.createObjectURL(blob);

        // Create a download link for the report
        const link = document.createElement('a');
        link.href = url;
        link.download = this.report_name + "_" + this.currentDate.getDate() + '.' + (this.currentDate.getMonth()+1) + '.' + this.currentDate.getFullYear() + '_report.pdf';
        if(this.isDownloadChecked)
          link.click();
        else
          window.open(url, '_blank');

        // Clean up the Blob URL after the download
        window.URL.revokeObjectURL(url);
      }, error => {
        console.log('An error occurred while generating the report:', error);
        // Handle the error as needed
      });
  }

  setValue() {
    this.fromDate = new Date("2020-06-30T17:06:50.930Z");
    this.toDate = new Date();
    this.report_name = "";
    this.role="All";
  }
}
