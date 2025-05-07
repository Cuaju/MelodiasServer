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
using System.Transactions;

namespace MelodiasTest.UserDAO
{
    public class TestEmployeeDao : IDisposable
    {
        private readonly MelodiasContext _context;
        private readonly TransactionScope _transactionScope;
        private readonly EmployeeDao _dao;
        private int _employeeId;

        public TestEmployeeDao()
        {
            _transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            _context = new MelodiasContext();
            _dao = new EmployeeDao();

            InicializarEmpleadoPrueba();
        }

        private void InicializarEmpleadoPrueba()
        {
            var empleado = new Employee
            {
                UserName = "UserTesttt",
                Name = "Pruebaaa",
                Surnames = "Tesster",
                Phone = 12349,
                Email = "test@exssample.com",
                Address = "Calle Fsssalsa 123",
                ZipCode = "1234a5",
                City = "TestCity",
                Password = "pwd1aaa23"
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
                UserName = "NewUssaer",
                Name = "Nuevod",
                Surnames = "Ussuario",
                Phone = 98761,
                Email = "new@exaample.com",
                Address = "Aveniada Siempre Viva",
                ZipCode = "5a4321",
                City = "NewCisty",
                Password = "sescret"
            };

            bool result = _dao.AddEmployee(nuevo);
            Assert.True(result);
        }

        [Fact]
        public void UpdateEmployee_EmployeeNotFound()
        {
            var inexistente = new Employee
            {
                Id = -1,
                UserName = "Ghost",
                Name = "NoExiste",
                Surnames = "Tampoco",
                Phone = 0,
                Email = "nobody@example.com",
                Address = "Nowhere",
                ZipCode = "00000",
                City = "Nada",
                Password = "null"
            };

            bool result = _dao.UpdateEmployee(inexistente);
            Assert.False(result);
        }

        [Fact]
        public void GetEmployeeByUserName_Exists_ReturnsEmployee()
        {
            var result = _dao.GetEmployeeByUserName("UserTesttt");

            Assert.NotNull(result);
            Assert.Equal("UserTesttt", result.UserName);
            Assert.Equal("Pruebaaa", result.Name);
            Assert.Equal("Tesster", result.Surnames);
        }

        [Fact]
        public void GetEmployeeByUserName_NotExists_ReturnsNull()
        {
            var result = _dao.GetEmployeeByUserName("UsuarioInexistenteXYZ");

            Assert.Null(result);
        }

        [Fact]
        public void GetEmployeeWithoutPasswordById_Exists_ReturnsEmployeeWithoutPassword()
        {
            var result = _dao.GetEmployeeWithoutPasswordById(_employeeId);

            Assert.NotNull(result);
            Assert.Equal(_employeeId, result.Id);
            Assert.Equal("UserTesttt", result.UserName);
            Assert.Equal("Pruebaaa", result.Name);
            Assert.Equal("Tesster", result.Surnames);
            Assert.Null(result.Password);
        }

        [Fact]
        public void GetEmployeeWithoutPasswordById_NotExists_ReturnsNull()
        {
            var result = _dao.GetEmployeeWithoutPasswordById(-999);

            Assert.Null(result);
        }

        [Fact]
        public void ExistPhoneNumber_Exists_ReturnsTrue()
        {
            var result = _dao.ExistPhoneNumber(12349);

            Assert.True(result);
        }

        [Fact]
        public void ExistPhoneNumber_NotExists_ReturnsFalse()
        {
            var result = _dao.ExistPhoneNumber(99999999);

            Assert.False(result);
        }

        [Fact]
        public void ExistMail_Exists_ReturnsTrue()
        {
            var result = _dao.ExistMail("test@exssample.com");

            Assert.True(result);
        }

        [Fact]
        public void ExistMail_NotExists_ReturnsFalse()
        {
            var result = _dao.ExistMail("noexiste@correo.com");

            Assert.False(result);
        }

        [Fact]
        public void ExistUserName_Exists_ReturnsTrue()
        {
            var result = _dao.ExistUserName("UserTesttt");

            Assert.True(result);
        }

        [Fact]
        public void ExistUserName_NotExists_ReturnsFalse()
        {
            var result = _dao.ExistUserName("UsuarioFalso123");

            Assert.False(result);
        }
        [Fact]
        public void GetIdEmployeeByUserName_Exists_ReturnsId()
        {
            int result = _dao.GetIdEmployeeByUserName("UserTesttt");

            Assert.Equal(_employeeId, result);
        }

        [Fact]
        public void GetIdEmployeeByUserName_NotExists_ReturnsZero()
        {
            int result = _dao.GetIdEmployeeByUserName("NoExisteUser123");

            Assert.Equal(0, result);
        }
        [Fact]
        public void DeleteEmployee_Exists_ReturnsTrue()
        {
            var empleado = new Employee
            {
                UserName = "DeleteTestUser",
                Name = "Eliminar",
                Surnames = "Prueba",
                Phone = 44444,
                Email = "delete@test.com",
                Address = "Dirección eliminar",
                ZipCode = "55555",
                City = "DeleteCity",
                Password = "deletepass"
            };

            _context.Employees.Add(empleado);
            _context.SaveChanges();

            int nuevoEmpleadoId = empleado.Id;

            bool result = _dao.DeleteEmployee(nuevoEmpleadoId);


            Assert.True(result);
        }

        [Fact]
        public void DeleteEmployee_NotExists_ReturnsFalse()
        {
            bool result = _dao.DeleteEmployee(-999);

            Assert.False(result);
        }

        public void Dispose()
        {
            _transactionScope.Dispose();
            _context.Dispose();
        }
    }
}
