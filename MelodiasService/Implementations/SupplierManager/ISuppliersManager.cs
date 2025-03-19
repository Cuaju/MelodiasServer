using MelodiasService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MelodiasService
{
    [ServiceContract]
    public interface ISuppliersManager
    {
        [OperationContract]
        bool RegisterSupplier(SupplierDTO supplier);

        [OperationContract]
        bool EditSupplier(SupplierDTO supplier);

        [OperationContract]
        List<SupplierDTO> GetSuppliers(string searchCiteria);

        [OperationContract]
        bool IsSupplierNameTaken(string name);

        [OperationContract]
        bool IsSupplierEmailTaken(string email);

        [OperationContract]
        bool DeleteSupplier(int supplierId);
    }
}
