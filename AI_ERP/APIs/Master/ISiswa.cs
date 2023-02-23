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
    public interface ISiswa
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string[] ShowAutocomplete(string kata_kunci);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string[] ShowAutocompleteByUnitByTahunAjaranBySemester(string kata_kunci, string rel_unit, string tahun_ajaran, string semester);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string[] ShowAutocompleteNISSekolahByUnitByTahunAjaran(string kata_kunci, string rel_unit, string tahun_ajaran, string semester);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string[] ShowAutocompleteNISSekolahByLevelByUnitByTahunAjaran(string kata_kunci, string rel_kelas, string rel_unit, string tahun_ajaran);
    }
}
