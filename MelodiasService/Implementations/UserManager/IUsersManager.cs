using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MelodiasService
{
    [ServiceContract]
    public interface IUsersManager
    {
        [OperationContract]
        bool LogIn(string username, string password);

        [OperationContract]
        bool AddEmployee(EmployeeDataContract employee);
    }
    [DataContract]
    public class EmployeeDataContract
    {
        [DataMember]
        public string userName { get; set; }

    }
}
