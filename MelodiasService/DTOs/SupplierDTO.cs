using DataAccess.DAO;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MelodiasService.DTOs
{
    [DataContract]
    public class SupplierDTO
    {
        [DataMember]
        public int SupplierId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string PostalCode { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string Email { get; set; }

        public static explicit operator SupplierDTO(SupplierData supplierData)
        {
            return new SupplierDTO
            {
                SupplierId = supplierData.supplierId,
                Name = supplierData.Name,
                Address = supplierData.Address,
                PostalCode = supplierData.PostalCode,
                City = supplierData.City,
                Country = supplierData.Country,
                Phone = supplierData.Phone,
                Email = supplierData.Email
            };
        }
    }


    public class SupplierMapper
    {
        public static SupplierDTO ToDTO(Supplier supplier)
        {
            return new SupplierDTO
            {
                SupplierId = supplier.supplierId,
                Name = supplier.Name,
                Address = supplier.Address,
                PostalCode = supplier.PostalCode,
                City = supplier.City,
                Country = supplier.Country,
                Phone = supplier.Phone,
                Email = supplier.Email
            };
        }

        public static Supplier ToEntity(SupplierDTO supplierDTO)
        {
            return new Supplier
            {
                supplierId = supplierDTO.SupplierId,
                Name = supplierDTO.Name,
                Address = supplierDTO.Address,
                PostalCode = supplierDTO.PostalCode,
                City = supplierDTO.City,
                Country = supplierDTO.Country,
                Phone = supplierDTO.Phone,
                Email = supplierDTO.Email
            };
        }
    }
}
