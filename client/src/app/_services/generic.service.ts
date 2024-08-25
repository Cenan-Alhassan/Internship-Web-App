import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { environment } from '../../environments/environment.development';

export interface IDepartments {
	id?: number;
	department?: string;
	numOfEmployee?: number;
	manager?: string;
}
export interface IEmployees {
	id?: number;
	name?: string;
	type?: number;
	department?: string;
}


@Injectable({
  providedIn: 'root'
})
export class GenericService {

  baseUrl: string = "api/"
  employeesDatabaseSignal = signal<any>(null)
  public employeeToBeEditedID = signal<number | null>(null)

  constructor(private http: HttpClient){}

  // Subject to handle the communication
  private callFunctionSource = new Subject<void>();
  callFunction$ = this.callFunctionSource.asObservable();

  // Method to trigger the function call
  callRefreshFunction() {
    this.callFunctionSource.next();
  }


  getDepartments(entityType:string): Observable<IDepartments[]>
  {
    return this.http.get<IDepartments[]>(this.baseUrl + entityType + "/get" )
  }

  getEmployees(entityType:string): Observable<IEmployees[]>
  {
    return this.http.get<IEmployees[]>(this.baseUrl + entityType + "/get" )
  }


  postEntity(entityType:string, model:any)
  {
    return this.http.post(this.baseUrl + entityType + "/post", model )
  }


  deleteDepartment(entityType:string, Id:number): Observable<IDepartments[]>
  {
    return this.http.delete<IDepartments[]>(this.baseUrl + entityType + `/delete/${Id}`)
  }

  deleteEmployee(entityType:string, Id:number): Observable<IEmployees[]>
  {
    return this.http.delete<IEmployees[]>(this.baseUrl + entityType + `/delete/${Id}`)
  }


  putEntity(entityType:string, Id:number, model:any)
  {
    return this.http.put(this.baseUrl + entityType + `/put/${Id}`, model)
  }

  getEntityNames(entityType:string): Observable<string[]>
  {
    return this.http.get<string[]>(this.baseUrl + entityType + "/get/names")
  }


  getLength(entityType:string): Observable<number>
  {
    return this.http.get<number>(this.baseUrl + entityType + "/length" )
  }


  convertEmployeeType(enumNumber:number){

    switch (enumNumber){
      case 0:
        return "Manager"
      case 1:
        return "Employee"
      case 2:
        return "HR"
      default:
        return `Invalid Type (${enumNumber})`
    }
  }




}
