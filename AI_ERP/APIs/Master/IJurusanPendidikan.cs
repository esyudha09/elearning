using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AI_ERP.APIs.Master
{
    [ServiceContract]
    public interface IJurusanPendidikan
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string[] ShowAutocomplete(string kata_kunci);
    }
}
