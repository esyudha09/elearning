using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AI_ERP.APIs.Elearning._GENERAL.AbsensiSiswa
{
    [ServiceContract]
    public interface IDoSave
    {
        [System.Web.Services.WebMethod(EnableSession = true)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string[] Do(
                string sw,
                string a,
                string k,
                string t,
                string s,
                string kd,
                string tgl,
                string lm,
                string m,
                string jk,
                string kj,
                string bs,
                string bsl,
                string skp,
                string tl,
                string act_ket,
                string ssid
            );

        [System.Web.Services.WebMethod(EnableSession = true)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string[] Do_V1(
                string sw,
                string a,
                string k,
                string t,
                string s,
                string kd,
                string tgl,
                string lm,
                string m,
                string jk,
                string kj,
                string bs,
                string bsl,
                string skp,
                string tl,
                string act_ket,
                string ssid
            );

        [System.Web.Services.WebMethod(EnableSession = true)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string GetValidateAbsen(string tgl, string m, string kd, string g);
    }
}
