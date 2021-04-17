using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Routing
{
    [ServiceContract]
    public interface IRouting
    {
        [OperationContract]
        String GetDirection(String depart, string arrive, string laVille);
    }

    
}
