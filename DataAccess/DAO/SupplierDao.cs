using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class SupplierData
    { 
        public int supplierId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }

    public class SupplierDao
    {

        public bool IsSupplierNameTaken(string name)
        {
            using (var context = new MelodiasContext())
            {
                var isTaken = context.SupplierCompanies
                    .Any(s => s.Name.ToLower() == name.ToLower());

                return isTaken;
            }
        }
        public bool IsSupplierEmailTaken(string email)
        {
            using (var context = new MelodiasContext())
            {
                var isTaken = context.SupplierCompanies
                    .Any(s => s.Email == email);

                return isTaken;
            }
        }


        public bool AddSupplier(Supplier supplier)
        {
        
            using (var context = new MelodiasContext())
            {
                context.SupplierCompanies.Add(supplier);
                int alteredRows = context.SaveChanges();
                return alteredRows == 1;
            }
        }

        public bool UpdateSupplier(Supplier supplier)
        {

            using (var context = new MelodiasContext())
            {
                var existingSupplier = context.SupplierCompanies
                    .FirstOrDefault(s => s.supplierId == supplier.supplierId);

                if (existingSupplier != null)
                {
                        
                    existingSupplier.Name = supplier.Name;
                    existingSupplier.Address = supplier.Address;
                    existingSupplier.PostalCode = supplier.PostalCode;
                    existingSupplier.City = supplier.City;
                    existingSupplier.Country = supplier.Country;
                    existingSupplier.Phone = supplier.Phone;
                    existingSupplier.Email = supplier.Email;
                    
                    int alteredRows = context.SaveChanges();
                    return alteredRows == 1; 
                }
                return false; 
            }
            
        }

        public List<SupplierData> GetSuppliersList(string searchTerm)
        {

            using (var context = new MelodiasContext())
            {
                var normalizedSearchTerm = searchTerm.Trim().ToLower();

                var suppliers = context.SupplierCompanies
                    .Where(s => s.Name.ToLower().Contains(normalizedSearchTerm) ||
                                s.Email.ToLower().Contains(normalizedSearchTerm))
                    .Take(10)
                    .ToList();

                var supplierDataList = suppliers.Select(s => new SupplierData
                {
                    supplierId = s.supplierId,
                    Name = s.Name,
                    Address = s.Address,
                    PostalCode = s.PostalCode,
                    City = s.City,
                    Country = s.Country,
                    Phone = s.Phone,
                    Email = s.Email
                }).ToList();

                return supplierDataList;
            }

        }


        public bool DeleteSupplier(int supplierId)
        {
            using (var context = new MelodiasContext())
            {
                var supplierToDelete = context.SupplierCompanies
                    .FirstOrDefault(s => s.supplierId == supplierId);

                if (supplierToDelete != null)
                {
                    context.SupplierCompanies.Remove(supplierToDelete);
                    int alteredRows = context.SaveChanges();
                    return alteredRows == 1; 
                }
                return false; 
            }
        }

    }
}
