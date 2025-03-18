using DataAccess.DAO;
using MelodiasService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MelodiasService.Implementations
{
    public partial class ServiceImplementation : ISuppliersManager
    {
        public bool EditSupplier(SupplierDTO supplier)
        {

            // we still need this shi to like verify email or username be4 edtiting
            // that shii will be in other shitty method like yeah that 
            // 
            // still needs to implement Logger and DBException so yeah still needs iterations
            var supplierEdited = SupplierMapper.ToEntity(supplier);

            try
            {
                return new SupplierDao().UpdateSupplier(supplierEdited);
            }
            catch (CommunicationException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            catch (TimeoutException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            return false;

        }

        public List<SupplierDTO> GetSuppliers(string searchCiteria)
        {
            try
            {
                List<SupplierData> supplierDataList = new SupplierDao().GetSuppliersList(searchCiteria);
                var suppliersList = new List<SupplierDTO>();
                foreach (SupplierData supplier in supplierDataList)
                {
                    suppliersList.Add((SupplierDTO)supplier);
                }
                return suppliersList;
            }
            catch (CommunicationException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            catch (TimeoutException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            return null;
        }

        public bool IsSupplierEmailTaken(string email)
        {
            try
            {
                return new SupplierDao().IsSupplierEmailTaken(email);
            }
            catch (CommunicationException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            catch (TimeoutException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            return false;
        }

        public bool IsSupplierNameTaken(string name)
        {
            try
            {
                return new SupplierDao().IsSupplierNameTaken(name);
            }
            catch (CommunicationException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            catch (TimeoutException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            return false;
        }

        public bool RegisterSupplier(SupplierDTO supplier)
        {

            // we still need this shi to like verify email or username be4 creating a new one
            // that shii will be in other shitty method like yeah that 
            // 
            // still needs to implement Logger and DBException so yeah still needs iterations

            var supplierData = SupplierMapper.ToEntity(supplier);

            try
            {
                return new SupplierDao().AddSupplier(supplierData);
            }
            catch (CommunicationException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            catch (TimeoutException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            return false;
        }
    }
}
