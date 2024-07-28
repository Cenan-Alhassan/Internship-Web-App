The following app is used to manipulate two tables of a database: a table for employees, and a table for departments. 

The aim of the application is to use a frontend framework (Angular 17) to send commands to an API (ASP.NET 8) that checks for conditions and saves only safe data to an SQL server database. 
Each employee has a name, type and department from the existing selection. Departments can be added with a manager from the existing employees, with the number of employees included. A department can only ever have one manager.

The API handles checking conditions and updating entries for each change. The frontend then dynamically updates without need to refresh.
