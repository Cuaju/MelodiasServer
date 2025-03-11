using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MelodiasService
{
    [ServiceContract]
    internal interface IUsersManager
    {
        [OperationContract]
        bool LogIn(string username, string password);
    }
}
