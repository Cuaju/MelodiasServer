using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MelodiasContext : DbContext
    {
        public MelodiasContext() : base("name=MelodiasContext") { }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Supplier> SupplierCompanies { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().ToTable("Employees")
                .Property(e => e.UserName)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_EmployeeUserName") { IsUnique = true }));

            modelBuilder.Entity<Product>().ToTable("Products")
                .Property(p => p.ProductName)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_ProductName") { IsUnique = true }));

            modelBuilder.Entity<Product>()
                .Property(p => p.PurchasePrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.SalePrice)
                .HasPrecision(10, 2);


            modelBuilder.Entity<Supplier>().Property(n => n.Name)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_SupplierName") { IsUnique = true }));
            
            modelBuilder.Entity<Supplier>()
                        .Property(s => s.Email)
                        .HasColumnAnnotation(
                            IndexAnnotation.AnnotationName,
                            new IndexAnnotation(new IndexAttribute("IX_SupplierCompanyEmail") { IsUnique = true }));
            
            modelBuilder.Entity<Purchase>().ToTable("Purchases");
            modelBuilder.Entity<Purchase>()
                .Property(p => p.TotalCost)
                .HasPrecision(10, 2);
            base.OnModelCreating(modelBuilder);
        }
    }
}