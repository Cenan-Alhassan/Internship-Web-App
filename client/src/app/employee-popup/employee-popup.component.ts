import { Component, inject, Input, input, OnInit, ViewChild } from '@angular/core';
import { GenericService } from '../_services/generic.service';
import { Subscription } from 'rxjs';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DepartmentConfig } from '../_Interfaces/department-config';
import { FormsModule } from '@angular/forms';
import { NgFor } from '@angular/common';
import { EmployeeConfig } from '../_Interfaces/employee-config';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'ngbd-modal-content',
  standalone: true,
  imports: [FormsModule, NgFor],
  template: `

		<div class="modal-header">

    @if (IsEditMode)
      {
			<h5 class="modal-title">Edit Department</h5>
      }
    @else 
    {
      <h5 class="modal-title">Add Department</h5>
    }

			<button type="button" class="btn-close" 
              aria-label="Close" 
              (click)="activeModal.dismiss('Cross click')">
      </button>
		</div>
		<div class="modal-body">

         
        <div class="container mt-3">
          
            <form #selectForm="ngForm" (ngSubmit)="submitForm(IsEditMode)" autocomplete="off">

                <div class="form-group mb-3">
                    <label>Employee Name</label>
                    <input name="username" [(ngModel)]="model.name" class="form-control" placeholder="Employee Name" />
                </div>
  
                <div class="form-group mb-3">
                  <label for="manager">Employee Type</label>
                  <select id="manager" name="manager" [(ngModel)]="model.type" class="form-select">
                    <option value="" disabled>Select Type</option>
                    <option [value]=0>Manager</option>
                    <option [value]=1>Employee</option>
                    <option [value]=2>HR</option>
                  </select>
                </div>

                <div class="form-group mb-5" style="margin-bottom: 100px;">
                  <label for="manager">Department</label>
                  <select id="manager" name="manager" [(ngModel)]="model.department" class="form-select">
                    <option value="" disabled>Select Department</option>
                    <option *ngFor="let department of departmentNames" [value]="department">
                      {{ department }}
                    </option>
                  </select>
                </div>

              
                <div class="modal-footer ">
                  <button type="submit" class="btn btn-primary">Submit</button>
			            <button type="button" class="btn btn-outline-secondary" (click)="activeModal.close('Close click')">Close</button>
		            </div>

          </form>
      </div>


		</div>
		
	`,
})

export class NgbdModalContent2 implements OnInit{
  activeModal = inject(NgbActiveModal);
  genericService = inject(GenericService)
  model: EmployeeConfig = {name: "", type: 0, department: ""}
  private toastr = inject(ToastrService)
  
  departmentNames: string[] = [];

  @Input() IsEditMode!: boolean;
  @Input() IdIfEdit: number | undefined;
  @Input() NameIfEdit: string | undefined;

  ngOnInit(): void {
    this.model.name = this.NameIfEdit 
    this.getEmployees()
  }


  submitForm(IsEditMode:boolean|undefined) {
    
    if (IsEditMode)
    {
      this.model.type = Number(this.model.type)
      console.log("Edit", this.model)
      this.genericService.putEntity("Employees", this.IdIfEdit!, this.model).subscribe({
        next: response => {
          console.log(response)},
        error: errorProperty => 
          {
            if (typeof(errorProperty.error) != "string")
            {
              this.toastr.error("API does not accept the data being sent.");
            }
            else
            {
              this.toastr.error(errorProperty.error);
            }
          },
        complete: () => {
          this.toastr.success("Employee has been successfully Edited.");
          this.activeModal.close('Close click');
          this.genericService.callRefreshFunction();}
      })

      
    }
    else
    {
      this.model.type = Number(this.model.type)
      console.log("Add", this.model)

      this.genericService.postEntity("Employees", this.model).subscribe({
      next: response => {
        console.log(response)},
      error: errorProperty => 
        {
          if (typeof(errorProperty.error) != "string")
          {
            this.toastr.error("API does not accept the data being sent.");
          }
          else
          {
            this.toastr.error(errorProperty.error);
          }
        },
        
      complete: () => {
        this.toastr.success("Employee has been successfully added.");
        this.activeModal.close('Close click');
        this.genericService.callRefreshFunction();}
      })

    }

  }

  getEmployees()
  {
    this.genericService.getEntityNames("Departments").subscribe({
      next: response => this.departmentNames = response,
      error: errorProperty => this.toastr.error(errorProperty.error),
      complete: () => console.log("Departments popup has successfully retrieved the employees")
    })
  }


}
@Component({
  selector: 'app-employee-popup',
  standalone: true,
  imports: [],
  templateUrl: './employee-popup.component.html',
  styleUrl: './employee-popup.component.css'
})
export class EmployeePopupComponent {
  
  private modalService = inject(NgbModal);

  open(IsEditMode: boolean, IdIfEdit?: number, NameIfEdit?: string) {
    const modalRef = this.modalService.open(NgbdModalContent2);
    modalRef.componentInstance.IsEditMode = IsEditMode;
    modalRef.componentInstance.IdIfEdit = IdIfEdit;
    modalRef.componentInstance.NameIfEdit = NameIfEdit;

  }
  }
  



