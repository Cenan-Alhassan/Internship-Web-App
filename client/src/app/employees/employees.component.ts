import { Component, inject, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { GenericService, IEmployees,} from '../_services/generic.service';
import { NgStyle } from '@angular/common';
import { EmployeePopupComponent } from "../employee-popup/employee-popup.component";
import { Subscription } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { NgbModule, NgbPaginationModule, NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { DepartmentsPopupComponent } from '../departments-popup/departments-popup.component';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-employees',
  standalone: true,
  imports: [FormsModule, NgbPaginationModule, NgbTypeaheadModule, NgbModule, DepartmentsPopupComponent, EmployeePopupComponent],
  templateUrl: './employees.component.html',
  styleUrl: './employees.component.css',
})
export class EmployeesComponent implements OnInit {
  @ViewChild(EmployeePopupComponent) employeesPopupComponent!: EmployeePopupComponent;  
  subscription!: Subscription;
  
  private toastr = inject(ToastrService)
  entityType: string = "Employees";
  service = inject(GenericService);
  Employees: IEmployees[] = [];
  pagedEmployees: IEmployees[] = [];

  page = 1;
  pageSize = 6;
  collectionSize: number = 0;
  
  refreshEmployees() {
    this.pagedEmployees = this.Employees.slice(
      (this.page - 1) * this.pageSize,
      this.page * this.pageSize
    );
  }


  ngOnInit(): void {
    this.getEmployees();
    this.subscription = this.service.callFunction$.subscribe(() => {
      this.getEmployees();
    });

    this.getLength();
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  getEmployees() {
    this.service.getEmployees(this.entityType).subscribe({
      next: (response: IEmployees[]) => {
        this.Employees = response;
        this.getLength();
        this.refreshEmployees();
      },
      error: errorProperty => this.toastr.error(errorProperty.error),
      complete: () => console.log("Employees have been retrieved successfully.")
    });
  }

  getLength() {
    this.service.getLength(this.entityType).subscribe({
      next: (response: number) => {
        this.collectionSize = response;
      },
      error: errorProperty => this.toastr.error(errorProperty.error),
    });
  }

  deleteEmployee(id: any) {
    if (this.Employees.length <= 1)
    {
      this.toastr.error("List cannot be empty.")
      return
    }
    this.service.deleteEmployee(this.entityType, id).subscribe({
      next: (response) => {
        this.Employees = response;
        this.getLength();
        this.refreshEmployees();
      },
      error: errorProperty => this.toastr.error(errorProperty.error),
      complete: () => this.toastr.success(`Employee (id=${id}) has been deleted successfully.`)
    });

  }

  openChildPopup(IsEditMode: boolean, IdIfEdit?: number, NameIfEdit?: string)
    {
      this.employeesPopupComponent.open(IsEditMode, IdIfEdit, NameIfEdit)
    }

  setEmployeeToBeEditedId(Id: number) {
    this.service.employeeToBeEditedID.set(Id)
    console.log(this.service.employeeToBeEditedID())
  }

  

}
