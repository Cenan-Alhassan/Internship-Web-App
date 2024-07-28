import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavComponent } from "./nav/nav.component";
import { EmployeesComponent } from "./employees/employees.component";
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavComponent, EmployeesComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit{
  
  toastr = inject(ToastrService)
  title = 'client';

  ngOnInit(): void {
    this.toastr.info("This is an app where you can perform HTTP requests on two tables to see how they affect eachother. (Click to close)",
       "Welcome!", {timeOut: 0, extendedTimeOut: 0, closeButton: true});
       this.toastr.info("Cenan Alhassan. You can find my links printed on the console.",
       "Made by:", {timeOut: 0, extendedTimeOut: 0, closeButton: true});
  }
}
