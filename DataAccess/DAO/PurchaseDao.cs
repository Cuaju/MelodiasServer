using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class PurchaseData
    {
        public int PurchaseId { get; set; }
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal TotalCost { get; set; }
    }

    public class PurchaseDao
    {
        public bool RegisterPurchase(Purchase purchase)
        {
            if (purchase == null)
            {
                return false;
            }

            try
            {
                using (var context = new MelodiasContext())
                {
                    context.Purchases.Add(purchase);
                    int alteredRows = context.SaveChanges();
                    return alteredRows ==1;
                }
            }
            catch (SqlException)
            {
                throw new FaultException("No fue posible conectarse a la base de datos. Por favor intente más tarde.");
            }
            catch (InvalidOperationException)
            {
                throw new FaultException("Error interno al consultar la base de datos. Intente más tarde.");
            }
            catch (Exception ex)
            {
                throw new FaultException("Error inesperado al guardar la compra: " + ex.Message);
            }
        }
    }
}
