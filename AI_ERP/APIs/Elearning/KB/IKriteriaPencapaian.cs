using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AI_ERP.APIs.Elearning.KB
{
    [ServiceContract]
    public interface IKriteriaPencapaian
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string[] ShowAutocompleteKriteriaPencapaian(string kata_kunci);
    }
}
