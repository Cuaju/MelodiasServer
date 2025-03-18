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

                auxEmployee.UserName = employee.userName;
                auxEmployee.Name = employee.name;
                auxEmployee.Surnames = employee.surnames;
                auxEmployee.ZipCode = employee.zipCode;
                auxEmployee.City = employee.city;
                auxEmployee.Address = employee.address;
                auxEmployee.Email = employee.mail;
                auxEmployee.Phone = employee.phone;
                auxEmployee.Password = employee.password;

                return employeeDao.AddEmployee(auxEmployee);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool DeleteEmployee(int idEmployee)
        {
            EmployeeDao employeeDao = new EmployeeDao();
            return employeeDao.DeleteEmployee(idEmployee);
        }

        public int GetIdEmployeeByUserName(string userName)
        {
           EmployeeDao employeeDao = new EmployeeDao();
            try
            {
                return employeeDao.GetIdEmployeeByUserName(userName);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
        public bool LogIn(string username, string password)
        {
          EmployeeDao employeeDao=new EmployeeDao();
          Employee employee = employeeDao.GetEmployeeByUserName(username);

            if (employee != null && employee.Password == password)
            {
                return true;
            }
                return false;
        }



    }
}
