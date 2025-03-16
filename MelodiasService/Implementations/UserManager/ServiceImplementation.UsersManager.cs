using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.DAO;
using DataAccess.Models;

namespace MelodiasService.Implementations
{
    public partial class ServiceImplementation : IUsersManager
    {
        public bool AddEmployee(EmployeeDataContract employee)
        {
            try
            {
                Console.WriteLine("el emopleado name es " + employee.userName);
                Employee auxEmployee = new Employee();
                EmployeeDao employeeDao = new EmployeeDao();

                auxEmployee.userName = employee.userName;

                return employeeDao.AddEmployee(auxEmployee);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool LogIn(string username, string password)
        {
            Console.WriteLine("Client Working...");
            if (username == "1" && password == "123")
            {
                return true;
            }
            return false;
        }



    }
}
