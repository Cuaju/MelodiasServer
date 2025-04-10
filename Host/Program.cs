using System;
using System.ServiceModel;
using MelodiasService.Implementations;

namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(ServiceImplementation)))
            {
                try
                {
                    host.Open();

                    Console.WriteLine("🟢 Servicio ejecutándose en los siguientes endpoints:\n");

                    foreach (var endpoint in host.Description.Endpoints)
                    {
                        Console.WriteLine($"➡️  Contrato: {endpoint.Contract.Name}");
                        Console.WriteLine($"    Dirección: {endpoint.Address.Uri}");
                        Console.WriteLine($"    Binding: {endpoint.Binding.Name}");
                        Console.WriteLine();
                    }

                    Console.WriteLine("Presiona ENTER para cerrar el servicio...");
                    Console.ReadLine();

                    host.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error al iniciar el servicio: {ex.Message}");
                }
            }
        }
    }
}
