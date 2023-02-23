using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AI_ERP.APIs.Elearning.SD.NilaiSiswa
{
    [ServiceContract]
    public interface IDoSave
    {
        [System.Web.Services.WebMethod(EnableSession = true)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string[] Do(
                string j,
                string t,
                string sm,
                string k,
                string kdt,
                string s,
                string n,
                string ap,
                string kd,
                string kp,
                string mp,
                string ms,
                string nr,
                string lts_ck_hd,
                string lts_ck_kw,
                string lts_ck_ps,
                string lts_ck_pk,
                string ssid
            );

        [System.Web.Services.WebMethod(EnableSession = true)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string[] DoSaveEkskul(
                string j,
                string t,
                string sm,
                string k,
                string s,
                string n,
                string ap,
                string kd,
                string kp,
                string mp,
                string nr,
                string sk,
                string iz,
                string al,
                string lts_ck_hd,
                string lts_ck_kw,
                string lts_ck_ps,
                string lts_ck_pk,
                string ssid
            );

        [System.Web.Services.WebMethod(EnableSession = true)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string[] DoSaveVolunteer(
                string j,
                string t,
                string sm,
                string k,
                string s,
                string n,
                string vset,
                string ssid
            );

        [System.Web.Services.WebMethod(EnableSession = true)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string[] DoSaveSikapSemester(
                string j,
                string t,
                string sm,
                string k,
                string kdt,
                string s,
                string n,
                string ap,
                string kd,
                string kp,
                string mp,
                string ms,
                string nr,
                string ssid
            );
    }
}
