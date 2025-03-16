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



    }
}
