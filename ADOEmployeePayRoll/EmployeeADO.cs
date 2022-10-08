using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADOEmployeePayRoll
{
   
        public class EmployeeADO
        {
            public static string connectionString = "Data Source=(localDB)\\MSSQLLocalDB;Initial Catalog=EmployeePayRollServices";
            Employee emp = new Employee();

            public void GetEmployeeDetails()
            {
                SqlConnection sqlConnect = new SqlConnection(connectionString);
                try
                {
                    using (sqlConnect)
                    {
                        sqlConnect.Open();
                        //string query = "select * from employee_payroll";
                        SqlCommand cmd = new SqlCommand("GetAllDetails", sqlConnect);
                        cmd.CommandType = CommandType.StoredProcedure;


                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr.HasRows)
                        {
                            Console.WriteLine("EmployeePayroll Table Contents : \n");
                            Console.WriteLine();
                            while (dr.Read())
                            {
                                emp.CompID = dr.GetInt32(0);
                                emp.CompanyName = dr.GetString(1);
                                emp.EmpId = dr.GetInt32(2);
                                emp.EmpName = dr.GetString(3);
                                emp.EmpAddress = dr.GetString(4);
                                emp.PhoneNumber = dr.GetInt64(5);
                                emp.StartDate = dr.GetDateTime(6);
                                emp.Gender = dr.GetString(7);
                                emp.BasicPay = dr.GetDouble(8);
                                emp.Deductions = dr.GetDouble(9);
                                emp.TaxablePay = dr.GetDouble(10);
                                emp.IncomeTax = dr.GetDouble(11);
                                emp.NetPay = dr.GetDouble(12);
                                emp.Department = dr.GetString(13);

                                Console.WriteLine("{0}  {1}   {2}   {3}   {4}    {5}   {6}   {7}   {8}   {9}   {10}   {11}   {12}   {13}", emp.CompID, emp.CompanyName, emp.EmpId, emp.EmpName, emp.EmpAddress, emp.PhoneNumber, emp.StartDate, emp.Gender, emp.BasicPay, emp.Deductions, emp.TaxablePay, emp.IncomeTax, emp.NetPay, emp.Department);
                            }
                        }
                        dr.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    sqlConnect.Close();
                }         
            }

        public void AddEmpDetails()
        {
            SqlConnection sqlConnect = new SqlConnection(connectionString);
            try
            {
                using (sqlConnect)
                {
                    sqlConnect.Open();
                    SqlCommand cmd = new SqlCommand("AddEmployeeDetails", sqlConnect);
                    cmd.CommandType = CommandType.StoredProcedure;

                    Console.WriteLine("Enter Company Name"); emp.CompanyName = Console.ReadLine();
                    Console.Write("Enter EmpName : "); emp.EmpName = Console.ReadLine();
                    Console.Write("Enter Gender : "); emp.Gender = Console.ReadLine();
                    Console.Write("Enter Phone : "); emp.PhoneNumber = Int64.Parse(Console.ReadLine());
                    Console.Write("Enter EmpAddress : "); emp.EmpAddress = Console.ReadLine();
                    Console.Write("Enter Department : "); emp.Department = Console.ReadLine();
                    Console.Write("Enter StartDate yyyy-mm-dd : "); emp.StartDate = DateTime.Parse(Console.ReadLine());
                    Console.Write("Enter BasicPay : "); emp.BasicPay = int.Parse(Console.ReadLine());
                    Console.Write("Enter Deductions : "); emp.Deductions = int.Parse(Console.ReadLine());
                    Console.Write("Enter IncomeTax : "); emp.IncomeTax = int.Parse(Console.ReadLine());
                    emp.TaxablePay = emp.BasicPay - emp.Deductions;
                    emp.NetPay = emp.TaxablePay - emp.IncomeTax;

                    cmd.Parameters.AddWithValue("@company", emp.CompanyName);
                    cmd.Parameters.AddWithValue("@FullName", emp.EmpName);
                    cmd.Parameters.AddWithValue("@gender", emp.Gender);
                    cmd.Parameters.AddWithValue("@PhoneNumber", emp.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Address", emp.EmpAddress);
                    cmd.Parameters.AddWithValue("@Date", emp.StartDate);
                    cmd.Parameters.AddWithValue("@Department", emp.Department);
                    cmd.Parameters.AddWithValue("@basicPay", emp.BasicPay);
                    cmd.Parameters.AddWithValue("@Taxablepay", emp.TaxablePay);
                    cmd.Parameters.AddWithValue("@deductions", emp.Deductions);
                    cmd.Parameters.AddWithValue("@IncomeTax", emp.IncomeTax);
                    cmd.Parameters.AddWithValue("@netPay", emp.NetPay);

                    int affRows = cmd.ExecuteNonQuery();
                    if (affRows >= 1)
                    {
                        Console.WriteLine("Employee added successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Employee not added..");
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlConnect.Close();
            }
        }
        public void UpdateBasicPay()
        {
            Console.Write("Enter Employee Name to update: ");
            string empName = Console.ReadLine();
            Console.Write("Enter Employee basic salary to update: ");
            float basic = float.Parse(Console.ReadLine());
            Console.Write("Enter Employee deductions to update: ");
            float deductions = float.Parse(Console.ReadLine());
            Console.Write("Enter Employee incometax  to update: ");
            float incometax = float.Parse(Console.ReadLine());

            SqlConnection sqlConnect = new SqlConnection(connectionString);
            try
            {
                using (sqlConnect)
                {
                    sqlConnect.Open();
                    SqlCommand cmd = new SqlCommand("updateEmployee", sqlConnect);

                    cmd.CommandType = CommandType.StoredProcedure;
                    emp.EmpName = empName;
                    emp.BasicPay = basic;
                    emp.Deductions = deductions;
                    emp.IncomeTax = incometax;
                    cmd.Parameters.AddWithValue("@EmpName", emp.EmpName);
                    cmd.Parameters.AddWithValue("@BasicPay", emp.BasicPay);
                    cmd.Parameters.AddWithValue("@deductions", emp.Deductions);
                    cmd.Parameters.AddWithValue("@incometax", emp.IncomeTax);


                    int affRows = cmd.ExecuteNonQuery();

                    if (affRows >= 1)
                    { Console.WriteLine(" Employee pay  details Updated.."); }
                    else
                    { Console.WriteLine(" Employee pay not Updated..."); }
                }
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }

            finally
            {
                sqlConnect.Close();
            }
        }

        public void GetRowsByDateRange()
        {
            SqlConnection sqlConnect = new SqlConnection(connectionString);
            try
            {
                using (sqlConnect)
                {
                    sqlConnect.Open();
                    SqlCommand cmd = new SqlCommand("GetEmployeeInDateRange", sqlConnect);

                    cmd.CommandType = CommandType.StoredProcedure;

                    Console.WriteLine("\nEnter date range yyyy-mm-dd:");
                    Console.Write("Minimum date: "); DateTime date1 = DateTime.Parse(Console.ReadLine());
                    Console.Write("Maximum date: "); DateTime date2 = DateTime.Parse(Console.ReadLine());

                    cmd.Parameters.AddWithValue("@fromDate", date1);
                    cmd.Parameters.AddWithValue("@toDate", date2);

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            emp.CompID = dr.GetInt32(0);
                            emp.CompanyName = dr.GetString(1);
                            emp.EmpId = dr.GetInt32(2);
                            emp.EmpName = dr.GetString(3);
                            emp.BasicPay = dr.GetDouble(4);
                            emp.Deductions = dr.GetDouble(5);
                            emp.TaxablePay = dr.GetDouble(6);
                            emp.IncomeTax = dr.GetDouble(7);
                            emp.NetPay = dr.GetDouble(8);

                            Console.Write(" {0}  {1}   {2}   {3}   {4}    {5}   {6}   {7}   {8}\n", emp.CompID, emp.CompanyName, emp.EmpId, emp.EmpName, emp.BasicPay, emp.Deductions, emp.TaxablePay, emp.IncomeTax, emp.NetPay);
                        }
                    }
                    dr.Close();

                    int affRows = cmd.ExecuteNonQuery();

                    if (affRows >= 1)
                    { Console.WriteLine(" Query Executed successfully."); }

                }
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }
            finally
            {
                sqlConnect.Close();
            }
        }

        public void DeleteEmployeeRecord()
        {
            SqlConnection sqlConnect = new SqlConnection(connectionString);
            try
            {
                using (sqlConnect)
                {
                    sqlConnect.Open();
                    SqlCommand cmd = new SqlCommand("DeleteEmployeeDetails", sqlConnect);
                    cmd.CommandType = CommandType.StoredProcedure;

                    Console.Write("Enter employee full name to delete record: ");
                    emp.EmpName = Console.ReadLine();
                    cmd.Parameters.AddWithValue("@FullName", emp.EmpName);
                    int affRows = cmd.ExecuteNonQuery();

                    if (affRows >= 1)
                    { Console.WriteLine("Employee details Removed successfully."); }
                    else
                    { Console.WriteLine("Employee not Removed.."); }
                }
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }
            finally
            {
                sqlConnect.Close();
            }
        }
        public void AddMultipleEmployees(List<Employee> employees)
        {
            DateTime start = DateTime.Now;

            foreach (Employee employee in employees)
                MultipleEmployees(employee);

            DateTime end = DateTime.Now;
            Console.WriteLine("With single main thread: " + (end-start).TotalMilliseconds);
        }


        public  void AddMultipleEmployeesUsingThreads(List<Employee> employees)
        {
            DateTime start = DateTime.Now;

            foreach (Employee employee in employees)
            {
                Task thread = new Task(() => MultipleEmployees(employee));
                thread.Start();
            }
            DateTime end = DateTime.Now;
            Console.WriteLine("With multi threading: "+ (end - start).TotalMilliseconds);
        }

        public  void  MultipleEmployees(Employee emp)
        {
            SqlConnection sqlConnect = new SqlConnection(connectionString);
            try
            {
                using (sqlConnect)
                {
                    sqlConnect.Open();
                    SqlCommand cmd = new SqlCommand("AddEmployeeDetails", sqlConnect);
                    cmd.CommandType = CommandType.StoredProcedure;

                    emp.TaxablePay = emp.BasicPay - emp.Deductions;
                    emp.NetPay = emp.TaxablePay - emp.IncomeTax;

                    cmd.Parameters.AddWithValue("@company", emp.CompanyName);
                    cmd.Parameters.AddWithValue("@FullName", emp.EmpName);
                    cmd.Parameters.AddWithValue("@gender", emp.Gender);
                    cmd.Parameters.AddWithValue("@PhoneNumber", emp.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Address", emp.EmpAddress);
                    cmd.Parameters.AddWithValue("@Date", emp.StartDate);
                    cmd.Parameters.AddWithValue("@Department", emp.Department);
                    cmd.Parameters.AddWithValue("@basicPay", emp.BasicPay);
                    cmd.Parameters.AddWithValue("@Taxablepay", emp.TaxablePay);
                    cmd.Parameters.AddWithValue("@deductions", emp.Deductions);
                    cmd.Parameters.AddWithValue("@IncomeTax", emp.IncomeTax);
                    cmd.Parameters.AddWithValue("@netPay", emp.NetPay);

                    int affRows = cmd.ExecuteNonQuery();
                    if (affRows >= 1)
                    {
                        Console.WriteLine("Employee added successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Employee not added..");
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlConnect.Close();
            }
        }


        // TSQL

        public void TSQLAddEmpDetails()
        {
            SqlTransaction transaction = null;
            SqlConnection sqlConnect = new SqlConnection(connectionString);
           
            try
            {
                using (sqlConnect) 
                { 
                    sqlConnect.Open();
                    transaction = sqlConnect.BeginTransaction();

                    SqlCommand cmd = new SqlCommand("dbo.AddToEmployeeTable", sqlConnect, transaction);
                    cmd.CommandType = CommandType.StoredProcedure;
              
                    Console.WriteLine("Enter Company Name"); emp.CompanyName = Console.ReadLine();
                    Console.Write("Enter EmpName : "); emp.EmpName = Console.ReadLine();
                    Console.Write("Enter Gender : "); emp.Gender = Console.ReadLine();
                    Console.Write("Enter Phone : "); emp.PhoneNumber = Int64.Parse(Console.ReadLine());
                    Console.Write("Enter EmpAddress : "); emp.EmpAddress = Console.ReadLine();
                    Console.Write("Enter StartDate yyyy-mm-dd : "); emp.StartDate = DateTime.Parse(Console.ReadLine());

                    cmd.Parameters.AddWithValue("@company", emp.CompanyName);
                    cmd.Parameters.AddWithValue("@FullName", emp.EmpName);
                    cmd.Parameters.AddWithValue("@gender", emp.Gender);
                    cmd.Parameters.AddWithValue("@PhoneNumber", emp.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Address", emp.EmpAddress);
                    cmd.Parameters.AddWithValue("@Date", emp.StartDate);

                    cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();

                    int EmployeeId = Convert.ToInt32(cmd.Parameters["@id"].Value);
                    Console.WriteLine(EmployeeId);

                    SqlCommand cmd2 = new SqlCommand("dbo.AddToPayrollTable", sqlConnect, transaction);
                    Console.Write("Enter Department : "); emp.Department = Console.ReadLine();
                    Console.Write("Enter BasicPay : "); emp.BasicPay = int.Parse(Console.ReadLine());
                    Console.Write("Enter Deductions : "); emp.Deductions = int.Parse(Console.ReadLine());
                    Console.Write("Enter IncomeTax : "); emp.IncomeTax = int.Parse(Console.ReadLine());
                    emp.TaxablePay = emp.BasicPay - emp.Deductions;
                    emp.NetPay = emp.TaxablePay - emp.IncomeTax;

                    
                    cmd2.Parameters.AddWithValue("@basicPay", emp.BasicPay);
                    cmd2.Parameters.AddWithValue("@Taxablepay", emp.TaxablePay);
                    cmd2.Parameters.AddWithValue("@deductions", emp.Deductions);
                    cmd2.Parameters.AddWithValue("@IncomeTax", emp.IncomeTax);
                    cmd2.Parameters.AddWithValue("@netPay", emp.NetPay);
                    cmd2.Parameters.AddWithValue("@Id", EmployeeId);
                    cmd2.ExecuteNonQuery();


                    SqlCommand cmd3 = new SqlCommand("AddToDepartmentTable", sqlConnect, transaction);
                    cmd3.Parameters.AddWithValue("@DeptName", emp.Department);
                    cmd3.Parameters.AddWithValue("@EmpId",EmployeeId);
                    
                    int affRows = cmd3.ExecuteNonQuery();

                    transaction.Commit();
                    if (affRows >= 1)
                    {
                        Console.WriteLine("Employee added successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Employee not added..");
                    }     
                }

                
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine(ex.Message);
                
            }
            finally
            {
                sqlConnect.Close();
            }
        }
    }

}      

