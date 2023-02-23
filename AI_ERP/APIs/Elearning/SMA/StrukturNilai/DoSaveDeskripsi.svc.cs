using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities.Elearning;
using AI_ERP.Application_DAOs.Elearning;
using AI_ERP.Application_Entities.Elearning.SMA;
using AI_ERP.Application_DAOs.Elearning.SMA;

namespace AI_ERP.APIs.Elearning.SMA.StrukturNilai
{
    public class DoSaveDeskripsi : IDoSaveDeskripsi
    {
        public string[] Do(
                string k,
                string j,
                string d,                
                string ssid
            )
        {
            List<string> hasil = new List<string>();
            string s_ssid = Libs.Dekrip(ssid);

            switch (j.Trim().ToUpper())
            {
                case "KD":
                    DAO_Rapor_StrukturNilai_KURTILAS_KD.UpdateDeskripsiRapor(
                            new Guid(k), d, s_ssid
                        );
                    break;
                case "KP":
                    DAO_Rapor_StrukturNilai_KURTILAS_KP.UpdateDeskripsiRapor(
                            new Guid(k), d, s_ssid
                        );
                    break;
                case "KP_ITEM":
                    DAO_Rapor_StrukturNilai_KURTILAS_KP.UpdateDeskripsi(
                            new Guid(k), d, s_ssid
                        );
                    break;
            }

            return hasil.ToArray();
        }
    }
}
