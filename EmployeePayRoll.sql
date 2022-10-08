
create database EmployeePayRollServices;
use EmployeePayRollServices;

Create Table Company
(CompanyID int identity(1,1) primary key,
CompanyName varchar(50));

--Insert Values in Company
Insert into Company values ('Tesla'),('SpaceX')
Select * from Company

--Create Employee Table
create table Employee
(EmployeeID int identity(1000,1) primary key,
CompanyIdentity int,
EmployeeName varchar(50),
EmployeePhoneNumber bigInt,
EmployeeAddress varchar(100),
StartDate date,
Gender char,
Foreign key (CompanyIdentity) references Company(CompanyID)
)

--Insert Values in Employee
insert into Employee values
(1,'Darshan Deshmukh',9995905050,'street 123','2012-03-28','M'),
(2,'Light Yagami',9842905550,'street 789 ,near abc','2017-04-22','M'),
(1,'Angelina Jolie',7612905050,'Hollywood road','2015-08-22','F'),
(2,'Gol D Roger',88129050000,'one piece,tokyo','2012-08-29','M')
Select * from Employee


--Create Payroll Table
create table PayrollCalculate
(BasicPay float,
Deductions float,
TaxablePay float,
IncomeTax float,
NetPay float,
EmployeeIdentity int,
Foreign key (EmployeeIdentity) references Employee(EmployeeID)
)

--Insert Values in Payroll Table
insert into PayrollCalculate(BasicPay,Deductions,IncomeTax,EmployeeIdentity) values 
(5000000,10000,20000,1000),
(4500000,8000,4000,1001),
(6000000,10000,5000,1002),
(9000000,399994,6784,1003)

--Updating Derived attribute values of Payroll Table,TaxablePay and NetPay
update PayrollCalculate set TaxablePay=BasicPay-Deductions
update PayrollCalculate set NetPay=TaxablePay-IncomeTax
select * from PayrollCalculate

--Create Department Table
create table Department
(
DepartmentId int identity(1,1) primary key,
DepartName varchar(20)
)

--Insert Values in Department Table
insert into Department values
('Marketing'),
('Sales'),
('Publishing')
select * from Department


--Create table EmployeeDepartment table
create table EmployeeDepartment
(
DepartmentIdentity int ,
EmployeeIdentity int,
Foreign key (EmployeeIdentity) references Employee(EmployeeID),
Foreign key (DepartmentIdentity) references Department(DepartmentID)
)

--Insert Values in EmployeeDepartment
insert into EmployeeDepartment values
(3,1000),
(2,1001),
(1,1002),
(3,1003)

select * from EmployeeDepartment;

--------------------------UC12---------------------------------------
--UC 4: Retrieve all Data
SELECT CompanyID,CompanyName,EmployeeID,EmployeeName,EmployeeAddress,EmployeePhoneNumber,StartDate,Gender,BasicPay,Deductions,TaxablePay,IncomeTax,NetPay,DepartName from Company
INNER JOIN Employee ON Company.CompanyID = Employee.CompanyIdentity
INNER JOIN PayrollCalculate on PayrollCalculate.EmployeeIdentity=Employee.EmployeeID
INNER JOIN EmployeeDepartment on Employee.EmployeeID=EmployeeDepartment.EmployeeIdentity
INNER JOIN Department on Department.DepartmentId=EmployeeDepartment.DepartmentIdentity

--UC 5: Select Query using Cast() and GetDate()
SELECT CompanyID,CompanyName,EmployeeID,EmployeeName,BasicPay,Deductions,TaxablePay,IncomeTax,NetPay from Company
INNER JOIN Employee ON Company.CompanyID = Employee.CompanyIdentity and StartDate BETWEEN Cast('2012-11-12' as Date) and GetDate()
INNER JOIN PayrollCalculate on PayrollCalculate.EmployeeIdentity=Employee.EmployeeID

--Retrieve query based on employee name
SELECT CompanyID,CompanyName,EmployeeID,EmployeeName,BasicPay,Deductions,TaxablePay,IncomeTax,NetPay from Company
INNER JOIN Employee ON Company.CompanyID = Employee.CompanyIdentity and Employee.EmployeeName='Darshan Deshmukh'
INNER JOIN PayrollCalculate on PayrollCalculate.EmployeeIdentity=Employee.EmployeeID

--UC 7: Use Aggregate Functions and Group by Gender
select Sum(BasicPay) as "TotalSalary",Gender from Employee
INNER JOIN PayrollCalculate on PayrollCalculate.EmployeeIdentity=Employee.EmployeeID group by Gender;

select Avg(BasicPay) as "AverageSalary",Gender from Employee
INNER JOIN PayrollCalculate on PayrollCalculate.EmployeeIdentity=Employee.EmployeeID group by Gender;

select Min(BasicPay) as "MinimumSalary",Gender from Employee
INNER JOIN PayrollCalculate on PayrollCalculate.EmployeeIdentity=Employee.EmployeeID group by Gender;

select Max(BasicPay)  as "MaximumSalary",Gender from Employee
INNER JOIN PayrollCalculate on PayrollCalculate.EmployeeIdentity=Employee.EmployeeID group by Gender;

select Count(BasicPay) as "CountSalary",Gender from Employee
INNER JOIN PayrollCalculate on PayrollCalculate.EmployeeIdentity=Employee.EmployeeID group by Gender



---------------------------Stored Procedures---------------------------------------

---Get all details
create procedure GetAllDetails 
as
SELECT CompanyID,CompanyName,EmployeeID,EmployeeName,EmployeeAddress,EmployeePhoneNumber,StartDate,Gender,BasicPay,Deductions,TaxablePay,IncomeTax,NetPay,DepartName from Company
INNER JOIN Employee ON Company.CompanyID = Employee.CompanyIdentity
INNER JOIN PayrollCalculate on PayrollCalculate.EmployeeIdentity=Employee.EmployeeID
INNER JOIN EmployeeDepartment on Employee.EmployeeID=EmployeeDepartment.EmployeeIdentity
INNER JOIN Department on Department.DepartmentId=EmployeeDepartment.DepartmentIdentity;

exec GetAllDetails;

--Add Details of employee

create procedure AddEmployeeDetails
@company varchar(20),
@FullName varchar(60),
@PhoneNumber bigInt,
@Address varchar(100),
@Date date,
@gender char,
@basicPay float,
@deductions float,
@Taxablepay float,
@IncomeTax float,
@netpay float,
@Department varchar(20)
as
DECLARE @compId int
SELECT @compId = (SELECT CompanyID
FROM Company
WHERE CompanyName=@company);
insert into Employee values(@compId,@FullName,@PhoneNumber,@Address,@Date,@gender)
declare @id int
select @id = SCOPE_IDENTITY() from Employee
insert into PayrollCalculate(BasicPay,Deductions,TaxablePay,IncomeTax,NetPay,EmployeeIdentity) values (@basicPay,@deductions,@Taxablepay,@IncomeTax,@netpay,@id)
DECLARE @DeptID int
SELECT @DeptID = (SELECT DepartmentId
FROM Department
WHERE DepartName =@Department);
insert into EmployeeDepartment values (@DeptID,@id)


--exec AddEmployeeDetails 'Tesla','rwrjrhw fdf',998737131,'ddds','2020-02-13','M',122322,123,2123,3445,23244,'Sales';


---Update Employee basic pay

create procedure  updateEmployee 
@EmpName varchar(50),
@BasicPay float,
@deductions float,
@incometax float
as 
DECLARE @EmployeeID int
SELECT @EmployeeID = (SELECT EmployeeID
FROM Employee
WHERE EmployeeName=@EmpName)
update PayrollCalculate set BasicPay =@BasicPay,Deductions=@deductions,IncomeTax=@incometax, TaxablePay=BasicPay-Deductions,NetPay=TaxablePay-IncomeTax where  EmployeeIdentity=@EmployeeID


-----Delete employee details
create procedure DeleteEmployeeDetails
@FullName varchar(60)
as
DECLARE @empId int
SELECT @empId = (SELECT EmployeeID
FROM Employee
WHERE EmployeeName=@FullName)
DELETE FROM EmployeeDepartment WHERE EmployeeIdentity=@empId
DELETE FROM PayrollCalculate WHERE EmployeeIdentity=@empId
DELETE FROM Employee WHERE EmployeeName=@FullName;


----Get Employees in date range
create procedure GetEmployeeInDateRange
@fromDate date,
@toDate date
as
SELECT CompanyID,CompanyName,EmployeeID,EmployeeName,BasicPay,Deductions,TaxablePay,IncomeTax,NetPay from Company
INNER JOIN Employee ON Company.CompanyID = Employee.CompanyIdentity and StartDate BETWEEN @fromDate and @toDate
INNER JOIN PayrollCalculate on PayrollCalculate.EmployeeIdentity=Employee.EmployeeID


-------------TSQL stored procedures-----------------

-----Add Empoyee details 

create procedure AddToEmployeeTable
@company varchar(20),
@FullName varchar(60),
@PhoneNumber bigInt,
@Address varchar(100),
@Date date,
@gender char,
@id int output
as
DECLARE @compId int
SELECT @compId = (SELECT CompanyID
FROM Company
WHERE CompanyName=@company);
insert into Employee values(@compId,@FullName,@PhoneNumber,@Address,@Date,@gender)
set @id = SCOPE_IDENTITY()
return @id;


create procedure AddToPayrollTable
@basicPay float,
@deductions float,
@Taxablepay float,
@IncomeTax float,
@netpay float,
@Id int
as
insert into PayrollCalculate(BasicPay,Deductions,TaxablePay,IncomeTax,NetPay,EmployeeIdentity) values (@basicPay,@deductions,@Taxablepay,@IncomeTax,@netpay,@Id);


create procedure AddToDepartmentTable
@Department varchar(20),
@EmpId int
as
DECLARE @DeptID int
SELECT @DeptID = (SELECT DepartmentId
FROM Department
WHERE DepartName =@Department)
insert into EmployeeDepartment values (@DeptID,@EmpId);

alter procedure AddToDepartmentTable
@DeptName varchar(20),
@EmpId int
as
DECLARE @DeptID int
SELECT @DeptID = (SELECT DepartmentId
FROM Department
WHERE DepartName =@DeptName)
insert into EmployeeDepartment values (@DeptID,@EmpId);


exec AddToDepartmentTable 'Sales',1019;
