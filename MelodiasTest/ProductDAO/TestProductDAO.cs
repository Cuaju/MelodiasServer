using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel;
using Xunit;
using DataAccess;
using DataAccess.DAO;
using DataAccess.Models;

namespace MelodiasTests.ProductDAO
{
    public class TestProductDao : IDisposable
    {
        private readonly MelodiasContext _context;
        private readonly DbContextTransaction _transaction;
        private readonly ProductDao _dao;
        private int _productId;

        public TestProductDao()
        {
            _context = new MelodiasContext();
            _transaction = _context.Database.BeginTransaction();
            _dao = new ProductDao();
            InicializarProductoPrueba();
        }

        private void InicializarProductoPrueba()
        {
            var producto = new Product
            {
                ProductName = "ProductoTest",
                ProductCode = "PTEST",
                Description = "Producto de prueba",
                PurchasePrice = 10,
                SalePrice = 15,
                Category = "Accesorios",
                Brand = "MarcaTest",
                Model = "ModeloX",
                Stock = 10,
                Photo = "img.jpg",
                Status = true,
                HasSales = false
            };

            _context.Products.Add(producto);
            _context.SaveChanges();
            _productId = producto.ProductId;
        }

        [Fact]
        public void RegisterProduct_Success()
        {
            var nuevo = new Product
            {
                ProductName = "NuevoProducto",
                Description = "Desc",
                PurchasePrice = 5,
                SalePrice = 8,
                Category = "Otros",
                Brand = "MarcaNueva",
                Model = "M1",
                Stock = 3,
                Photo = "img.png",
                Status = true,
                HasSales = false
            };

            var result = _dao.RegisterProduct(nuevo);
            Assert.True(result);
        }

        [Fact]
        public void RegisterProduct_Fail_DuplicatedName()
        {
            var duplicado = new Product
            {
                ProductName = "ProductoTest",
                ProductCode = "P12345"
            };

            var ex = Assert.Throws<FaultException>(() => _dao.RegisterProduct(duplicado));
            Assert.Contains("A product with this name already exists.", ex.Message);
        }

        [Fact]
        public void GetProducts_ReturnsActiveProducts()
        {
            var productos = _dao.GetProducts();
            Assert.NotEmpty(productos);
            Assert.All(productos, p => Assert.True(p.Status));
        }

        [Fact]
        public void UpdateProduct_Success()
        {
            var modificado = new Product
            {
                ProductId = _productId,
                ProductName = "ProductoTestModificado",
                Description = "Modificado",
                PurchasePrice = 12,
                SalePrice = 18,
                Category = "Nueva",
                Brand = "Otra",
                Model = "Nuevo",
                Stock = 20,
                Photo = "mod.jpg",
                Status = true
            };

            var result = _dao.UpdateProduct(modificado);
            Assert.True(result);
        }

        [Fact]
        public void UpdateProduct_Fail_DuplicatedName()
        {
            var otro = new Product
            {
                ProductName = "OtroNombre",
                Description = "Desc",
                PurchasePrice = 1,
                SalePrice = 2,
                Category = "Cat",
                Brand = "Brand",
                Model = "Model",
                Stock = 5,
                Photo = "",
                Status = true,
                HasSales = false
            };

            _context.Products.Add(otro);
            _context.SaveChanges();

            var duplicado = new Product
            {
                ProductId = _productId,
                ProductName = "OtroNombre"
            };

            var ex = Assert.Throws<FaultException>(() => _dao.UpdateProduct(duplicado));
            Assert.Contains("already exists", ex.Message);
        }

        [Fact]
        public void DeleteProduct_Success()
        {
            var resultado = _dao.DeleteProduct(_productId);
            Assert.True(resultado);
        }

        [Fact]
        public void DeleteProduct_Fail_HasSales()
        {
            var vendido = new Product
            {
                ProductName = "Vendido",
                Description = "Ya fue vendido",
                PurchasePrice = 20,
                SalePrice = 25,
                Category = "Eliminación",
                Brand = "TestBrand",
                Model = "X1",
                Stock = 10,
                Photo = "vendido.jpg",
                Status = true,
                HasSales = true
            };

            _context.Products.Add(vendido);
            _context.SaveChanges();

            var ex = Assert.Throws<FaultException>(() => _dao.DeleteProduct(vendido.ProductId));
            Assert.Contains("no puede ser eliminado", ex.Message);
        }


        [Fact]
        public void ExistsProductByName_ReturnsTrue()
        {
            var existe = _dao.ExistsProductByName("ProductoTest", _productId + 1000);
            Assert.True(existe);
        }

        [Fact]
        public void ExistsProductByName_ReturnsFalse()
        {
            var existe = _dao.ExistsProductByName("Inexistente", 999);
            Assert.False(existe);
        }

        [Fact]
        public void GetProductsList_ByName()
        {
            var lista = _dao.GetProductsList("ProductoTest");
            Assert.NotEmpty(lista);
        }

        [Fact]
        public void SearchProducts_WithFilters()
        {
            var resultados = _dao.SearchProducts("ProductoTest", "PTEST", "Accesorios", "MarcaTest");
            Assert.NotEmpty(resultados);
        }

        public void Dispose()
        {
            _transaction.Rollback();
            _transaction.Dispose();
            _context.Dispose();
        }
    }
}
