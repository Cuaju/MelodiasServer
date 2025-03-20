using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class EmployeeDao
    {

        public bool AddEmployee(Employee employee)
        {
            using (var context = new MelodiasContext())
            {
                try
                {
                    context.Employees.Add(employee);

                    int affectedRows= context.SaveChanges();
                    return affectedRows == 1;

                }catch (EntityException ex)
                {
                    Console.WriteLine(ex.Message);
                    throw new Exception(ex.Message);
                }catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);

                    throw new Exception(ex.Message);
                }

            }
        }

        public bool ExistPhoneNumber(int number)
        {
            try
            {
                using (var context = new MelodiasContext())
                {
                    return context.Employees.Any(x => x.Phone == number);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (EntityException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool ExistMail(string mail)
        {
            try
            {
                using (var context = new MelodiasContext())
                {
                    return context.Employees.Any(x => x.Email == mail);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (EntityException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool ExistUserName(string userName)
        {
            try
            {
                using (var context = new MelodiasContext())
                {
                    return context.Employees.Any(x => x.UserName == userName);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (EntityException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public bool UpdateEmployee(Employee employee)
        {
            using (var context = new MelodiasContext())
            {
                try
                {
                    var existingEmployee = context.Employees.Find(employee.Id);
                    if (existingEmployee == null)
                    {
                        return false; 
                    }
                    existingEmployee.UserName = employee.UserName;
                    existingEmployee.Name = employee.Name;
                    existingEmployee.Surnames = employee.Surnames;
                    existingEmployee.ZipCode = employee.ZipCode;
                    existingEmployee.City = employee.City;
                    existingEmployee.Address = employee.Address;
                    existingEmployee.Email = employee.Email;
                    existingEmployee.Phone = employee.Phone;
                    existingEmployee.Password = employee.Password;

                    int affectedRows = context.SaveChanges();
                    return affectedRows > 0;
                }
                catch (EntityException ex)
                {
                    Console.WriteLine(ex.Message);
                    throw new Exception(ex.Message);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    throw new Exception(ex.Message);
                }
            }
        }

        public Employee GetEmployeeByUserName(string userName)
        {
            try
            {
                using (var context = new MelodiasContext())
                {
                    return context.Employees.FirstOrDefault(e=>e.UserName ==userName);
                }
            }
            catch (EntityException ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public int GetIdEmployeeByUserName(string userName)
        {
            int idEmployee = 0;
            try
            {
                using (var context= new MelodiasContext())
                {
                    var employee = context.Employees
                        .FirstOrDefault(x => x.UserName == userName);

                    if(employee != null)
                    {
                        idEmployee = employee.Id;
                    }
                }

            }catch (EntityException ex)
            {
                throw new Exception(ex.Message);
            }
            return idEmployee;
        }


        public bool DeleteEmployee(int employeeId)
        {
            using (var context = new MelodiasContext())
            {
                try
                {
                    var employee = context.Employees.Find(employeeId);
                    if (employee == null)
                    {
                        return false;
                    }

                    context.Employees.Remove(employee);
                    int affectedRows = context.SaveChanges();
                    return affectedRows == 1;
                }
                catch (EntityException ex)
                {
                    Console.WriteLine(ex.Message);
                    throw new Exception(ex.Message);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    throw new Exception(ex.Message);
                }
            }
        }



    }
}
