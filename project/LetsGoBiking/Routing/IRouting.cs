using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Routing
{
    [ServiceContract]
    public interface IRouting
    {
        [OperationContract]
        WebResult GetDirectionSOAP(String depart, string arrive, string laVille);

        [OperationContract]
        Position GetPositionCitySOAP(string laVille);
    }

    
}
