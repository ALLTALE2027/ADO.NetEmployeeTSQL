using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADOEmployeePayRoll
{
    public class Employee
    {
        public int CompID { get; set; }
        public string CompanyName { get; set; }
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string Gender { get; set; }
        public string EmpAddress { get; set; }
        public long PhoneNumber { get; set; }
        public string Department { get; set; }
        public DateTime StartDate { get; set; }
        public double BasicPay { get; set; }
        public double Deductions { get; set; }
        public double TaxablePay { get; set; }
        public double IncomeTax { get; set; }
        public double NetPay { get; set; }
    }
}
