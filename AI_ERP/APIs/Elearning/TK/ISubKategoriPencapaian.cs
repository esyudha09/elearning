using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AI_ERP.APIs.Elearning.TK
{
    [ServiceContract]
    public interface ISubKategoriPencapaian
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string[] ShowAutocompleteSubKategoriPencapaian(string kata_kunci);
    }
}
