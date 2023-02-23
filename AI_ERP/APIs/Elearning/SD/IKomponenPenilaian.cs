using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AI_ERP.APIs.Elearning.SD
{
    [ServiceContract]
    public interface IKomponenPenilaian
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string[] ShowAutocompleteKomponenPenilaian(string kata_kunci);
    }
}
