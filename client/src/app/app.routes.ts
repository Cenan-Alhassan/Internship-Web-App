import { Routes } from '@angular/router';
import { EmployeesComponent } from './employees/employees.component';
import { DepartmentsComponent } from './departments/departments.component';

export const routes: Routes = 
[
    {path: "employees", component: EmployeesComponent},
    {path: "departments", component: DepartmentsComponent}
];
