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

        public bool EditEmployee(int idEmployee, EmployeeDataContract updatedEmployee)
        {
            try
            {
                EmployeeDao employeeDao = new EmployeeDao();

                Employee employeeToUpdate = new Employee
                {
                    Id = idEmployee, 
                    UserName = updatedEmployee.userName,
                    Name = updatedEmployee.name,
                    Surnames = updatedEmployee.surnames,
                    ZipCode = updatedEmployee.zipCode,
                    City = updatedEmployee.city,
                    Address = updatedEmployee.address,
                    Email = updatedEmployee.mail,
                    Phone = updatedEmployee.phone,
                    Password = updatedEmployee.password
                };

                return employeeDao.UpdateEmployee(employeeToUpdate);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al editar el empleado: {ex.Message}");
            }
        }


        public EmployeeDataContract  GetEmployeeDetailsWithoutPassword(int idEmployee)
        {
            try
            {
                EmployeeDao employeeDao = new EmployeeDao();
                Employee employee = employeeDao.GetEmployeeWithoutPasswordById(idEmployee);

                if (employee != null)
                {
                    EmployeeDataContract employeeDataContract = new EmployeeDataContract
                    {
                        userName = employee.UserName,
                        name = employee.Name,
                        surnames = employee.Surnames,
                        zipCode = employee.ZipCode,
                        city = employee.City,
                        address = employee.Address,
                        mail = employee.Email,
                        phone = employee.Phone
                        
                    };
                
                    return employeeDataContract;
                }
                else
                {
                    return null; 
                }
            }
            catch (Exception ex)
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

        public bool MailAlreadyExist(string mail)
        {
            EmployeeDao employeeDao = new EmployeeDao();
            try
            {
                return employeeDao.ExistMail(mail);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PhoneNumberExists(int number)
        {
            try
            {
                EmployeeDao employeeDao = new EmployeeDao();
                return employeeDao.ExistPhoneNumber(number);

            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool UserNameExist(string userName)
        {
            try
            {
                EmployeeDao employeeDao= new EmployeeDao();
                return employeeDao.ExistUserName(userName); 

            }catch (Exception)
            {
                return false;
            }
        }
    }
}
