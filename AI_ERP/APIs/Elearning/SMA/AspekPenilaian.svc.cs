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

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_Entities.Elearning.SMA;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_DAOs.Elearning.SMA;

namespace AI_ERP.APIs.Elearning.SMA
{
    public class AspekPenilaian : IAspekPenilaian
    {
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public string[] ShowAutocompleteAspekPenilaian(string kata_kunci)
        {
            List<string> hasil = new List<string>();
            if (kata_kunci == null) return hasil.ToArray();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = Libs.GetConnectionString_Rapor();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = DAO_Rapor_AspekPenilaian.SP_SELECT_ALL_FOR_SEARCH;
                    cmd.Parameters.AddWithValue("@nama", (kata_kunci == null ? "" : kata_kunci));
                    cmd.Connection = conn;
                    conn.Open();

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            hasil.Add(string.Format("{0}~{1}", Libs.GetNoHTMLFormat(sdr["Nama"].ToString()), sdr["Kode"].ToString().ToUpper()));
                        }
                    }
                    conn.Close();
                }
            }
            return hasil.ToArray();
        }
    }
}
