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
            public virtual DbSet<Employee> Employees { get; set; }

        public MelodiasContext() : base("name=MelodiasContext") { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Employee>().ToTable("Employees").Property(e => e.UserName)
         .HasColumnAnnotation(IndexAnnotation.AnnotationName,
             new IndexAnnotation(new IndexAttribute("IX_EmployeeUserName") { IsUnique = true }));

            base.OnModelCreating(modelBuilder);


        }
    }
    }
