using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ServiceModel;
using Xunit;
using DataAccess;
using DataAccess.DAO;
using DataAccess.Models;
using System.Threading.Tasks;

namespace MelodiasTest.UserDAO
{
    public class TestEmployeeDao : IDisposable
    {
        private readonly MelodiasContext _context;
        private readonly DbContextTransaction _transaction;
        private readonly EmployeeDao _dao;
        private int _employeeId;

        public TestEmployeeDao()
        {
            _context = new MelodiasContext();
            _transaction = _context.Database.BeginTransaction();
            _dao = new EmployeeDao();        

            InicializarEmpleadoPrueba();
        }

        private void InicializarEmpleadoPrueba()
        {
            var empleado = new Employee
            {
                UserName = "UserTest",
                Name = "Prueba",
                Surnames = "Tester",
                Phone = 123456789,
                Email = "test@example.com",
                Address = "Calle Falsa 123",
                ZipCode = "12345",
                City = "TestCity",
                Password = "pwd123"
            };

            _context.Employees.Add(empleado);
            _context.SaveChanges();
            _employeeId = empleado.Id;
        }

        [Fact]
        public void AddEmployee_Success()
        {
            var nuevo = new Employee
            {
                UserName = "NewUser",
                Name = "Nuevo",
                Surnames = "Usuario",
                Phone = 987654321,
                Email = "new@example.com",
                Address = "Avenida Siempre Viva",
                ZipCode = "54321",
                City = "NewCity",
                Password = "secret"
            };

            bool result = _dao.AddEmployee(nuevo);
            Assert.True(result);
        }

        [Fact]
        public void UpdateEmployee_Success()
        {
            
            var actualizado = new Employee
            {
                Id = _employeeId,
                UserName = "UserTestUpdated",
                Name = "PruebaActualizada",
                Surnames = "Tester",
                Phone = 123456789,
                Email = "test@example.com",
                Address = "Nueva Dirección 456",
                ZipCode = "99999",
                City = "CiudadNueva",
                Password = "nuevaPwd"
            };

           
            bool result = _dao.UpdateEmployee(actualizado);

            Assert.True(result);
        }

        [Fact]
        public void GetEmployeeByUserName_Success()
        {
            var result = _dao.GetEmployeeByUserName("UserTest");
            Assert.NotNull(result);
            Assert.Equal("UserTest", result.UserName);
        }

        [Fact]
        public void GetEmployeeWithoutPasswordById_Success()
        {
            var result = _dao.GetEmployeeWithoutPasswordById(_employeeId);
            Assert.NotNull(result);
            Assert.Equal(_employeeId, result.Id);
            Assert.Null(result.Password); 
        }

        [Fact]
        public void ExistPhoneNumber_ReturnsTrue()
        {
            var exists = _dao.ExistPhoneNumber(123456789);
            Assert.True(exists);
        }

        [Fact]
        public void ExistMail_ReturnsTrue()
        {
            var exists = _dao.ExistMail("test@example.com");
            Assert.True(exists);
        }

        [Fact]
        public void ExistUserName_ReturnsTrue()
        {
            var exists = _dao.ExistUserName("UserTest");
            Assert.True(exists);
        }

        [Fact]
        public void GetIdEmployeeByUserName_ReturnsCorrectId()
        {
            var id = _dao.GetIdEmployeeByUserName("UserTest");
            Assert.Equal(_employeeId, id);
        }

        [Fact]
        public void DeleteEmployee_Success()
        {
            var result = _dao.DeleteEmployee(_employeeId);
            Assert.True(result);

            var deleted = _dao.GetEmployeeByUserName("UserTest");
            Assert.Null(deleted);
        }


        public void Dispose()
        {
            _transaction.Rollback();
            _transaction.Dispose();
            _context.Dispose();
        }
    }
}
