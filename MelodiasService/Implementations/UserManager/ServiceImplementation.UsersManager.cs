﻿using System;
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
                    Console.WriteLine("el nombre del empleado es "+employeeDataContract.name);
                    Console.WriteLine("el userName del empleado es " + employeeDataContract.userName);
                    Console.WriteLine("el surnames del empleado es " + employeeDataContract.surnames);
                    Console.WriteLine("el zipCode del empleado es " + employeeDataContract.zipCode);
                    Console.WriteLine("el city del empleado es " + employeeDataContract.city);
                    Console.WriteLine("el address del empleado es " + employeeDataContract.address);
                    Console.WriteLine("el mail del empleado es " + employeeDataContract.mail);
                    Console.WriteLine("el phone del empleado es " + employeeDataContract.phone);


                    Console.WriteLine("el nombre del empleado es " + employeeDataContract.name);
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
