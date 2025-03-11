using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MelodiasService.Implementations
{
    public partial class ServiceImplementation : IUsersManager
    {
        public bool LogIn(string username, string password)
        {
            Console.WriteLine("Client Working...");
            if (username == "1" && password == "123")
            {
                return true;
            }
            return false;
        }
    }
}
