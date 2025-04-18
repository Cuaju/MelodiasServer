﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using MelodiasService.Implementations;

namespace Host
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(ServiceImplementation)))
            {
                host.Open();
                Console.WriteLine("Service is running...");
                Console.ReadLine();
            }
        }
    }
}
