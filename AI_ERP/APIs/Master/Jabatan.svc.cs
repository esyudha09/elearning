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
using AI_ERP.Application_DAOs;

namespace AI_ERP.APIs.Master
{
    public class Jabatan : IJabatan
    {
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public string[] ShowAutocomplete(string kata_kunci)
        {
            List<string> hasil = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = Libs.GetConnectionString_ERP();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = DAO_Jabatan.SP_SELECT_BY_NAMA;
                    cmd.Parameters.AddWithValue("@nama", (kata_kunci == null ? "" : kata_kunci));
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            hasil.Add(string.Format("{0}~{1}", sdr["Nama"], sdr["Kode"]));
                        }
                    }
                    conn.Close();
                }
            }
            return hasil.ToArray();
        }
    }
}
