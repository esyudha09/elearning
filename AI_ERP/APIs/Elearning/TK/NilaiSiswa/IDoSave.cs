using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AI_ERP.APIs.Elearning.TK.NilaiSiswa
{
    [ServiceContract]
    public interface IDoSave
    {
        [System.Web.Services.WebMethod(EnableSession = true)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string[] Do(string t, string sm, string s, string kd, string pp, string kr, string ds);

        [System.Web.Services.WebMethod(EnableSession = true)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string[] DoPF(string t, string sm, string s, string kd, string bb, string tb, string lk);

        [System.Web.Services.WebMethod(EnableSession = true)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string[] DoCK(string t, string sm, string s, string kd, string lst_ck_abs);
    }
}
