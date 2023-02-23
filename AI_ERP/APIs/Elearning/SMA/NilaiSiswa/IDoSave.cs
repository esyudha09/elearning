using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AI_ERP.APIs.Elearning.SMA.NilaiSiswa
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
                string nr_p,
                string nr_ppk,
                string nr_prd,
                string nr_k,
                string pnr_p,
                string pnr_k,
                string lts_hd,
                string lts_maxhd,
                string lts_lk,
                string lts_rj,
                string lts_rpkb,
                string lts_ck_hd,
                string lts_ck_kw,
                string lts_ck_ps,
                string lts_ck_pk,
                string ssid
            );

        [System.Web.Services.WebMethod(EnableSession = true)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string[] DoUpdateEkskul(
                string t,
                string sm,
                string s,
                string mp,
                string n,
                string lts_hd,
                string sakit,
                string izin,
                string alpa
            );

        [System.Web.Services.WebMethod(EnableSession = true)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string[] DoUpdateLTSHDEkskul(
                string t,
                string sm,
                string s,
                string mp,
                string n
            );

        [System.Web.Services.WebMethod(EnableSession = true)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string[] DoUpdateSakitEkskul(
                string t,
                string sm,
                string s,
                string mp,
                string n
            );

        [System.Web.Services.WebMethod(EnableSession = true)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string[] DoUpdateIzinEkskul(
                string t,
                string sm,
                string s,
                string mp,
                string n
            );

        [System.Web.Services.WebMethod(EnableSession = true)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string[] DoUpdateAlpaEkskul(
                string t,
                string sm,
                string s,
                string mp,
                string n
            );

        [System.Web.Services.WebMethod(EnableSession = true)]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string[] DoUpdateSikap(
                string t,
                string sm,
                string s,
                string mp,
                string kd,
                string ssp,
                string sss
            );
    }
}
