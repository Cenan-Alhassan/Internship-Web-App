import { Component, inject, OnDestroy, OnInit, ViewChild, viewChild } from '@angular/core';
import { GenericService, IDepartments } from '../_services/generic.service';
import { FormsModule } from '@angular/forms';
import { NgbModule, NgbPaginationModule, NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { DepartmentsPopupComponent } from "../departments-popup/departments-popup.component";
import { Subscription } from 'rxjs';
import { ToastrService } from 'ngx-toastr';



@Component({
  selector: 'app-departments',
  standalone: true,
  imports: [FormsModule, NgbPaginationModule, NgbTypeaheadModule, NgbModule, DepartmentsPopupComponent],
  templateUrl: './departments.component.html',
  styleUrl: './departments.component.css'
})
export class DepartmentsComponent implements OnInit, OnDestroy {
  @ViewChild(DepartmentsPopupComponent) departmentsPopupComponent!: DepartmentsPopupComponent;  
  subscription!: Subscription;
  
  entityType: string = "Departments";
  service = inject(GenericService);
  toastr = inject(ToastrService)
  Departments: IDepartments[] = [];
  pagedDepartments: IDepartments[] = [];

  page = 1;
  pageSize = 6;
  collectionSize: number = 0;

  ngOnInit(): void {
    this.getDepartments();

    this.subscription = this.service.callFunction$.subscribe(() => {
      this.getDepartments();
    });

    this.getLength();
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  refreshDepartments() {
    this.pagedDepartments = this.Departments.slice(
      (this.page - 1) * this.pageSize,
      this.page * this.pageSize
    );
  }

  getDepartments() {
    this.service.getDepartments(this.entityType).subscribe({
      next: (response: IDepartments[]) => {
        this.Departments = response;
        this.getLength();
        this.refreshDepartments();
      },
      error: errorProperty => this.toastr.error(errorProperty.error),
      complete: () => console.log("Departments have been retrieved successfully.")
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

  deleteDepartment(id: any) {
    
    if (this.Departments.length <= 1)
      {
        this.toastr.error("List cannot be empty.")
        return
      }

    this.service.deleteDepartment(this.entityType, id).subscribe({
      next: (response) => {
        this.Departments = response;
        this.getLength();
        this.refreshDepartments();
      },
      error: errorProperty => this.toastr.error(errorProperty.error),
      complete: () => this.toastr.success(`Department (id=${id}) has been deleted successfully.`)
    });

  }

  openChildPopup(IsEditMode: boolean, IdIfEdit?: number, NameIfEdit?: string)
    {
      this.departmentsPopupComponent.open(IsEditMode, IdIfEdit, NameIfEdit)
    }
}
