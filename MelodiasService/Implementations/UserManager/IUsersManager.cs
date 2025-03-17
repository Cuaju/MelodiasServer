using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Policy;
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

        [OperationContract]
        bool DeleteEmployee(int idEmployee);

        [OperationContract]
        int GetIdEmployeeByUserName(string userName);
    }
    [DataContract]
    public class EmployeeDataContract
    {
        [DataMember]
        public string userName { get; set; }

        [DataMember]
        public string name {  get; set; }

        [DataMember]
        public string surnames { get; set; }

        [DataMember]
        public int phone { get; set; }

        [DataMember]
        public string mail  { get; set; }

        [DataMember]
        public string address { get; set; }

        [DataMember]
        public string zipCode { get; set; }

        [DataMember]
        public string city { get; set; }

        [DataMember]
        public string password { get; set; }

    }
}
