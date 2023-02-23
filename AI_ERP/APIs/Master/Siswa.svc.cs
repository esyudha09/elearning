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
using System.Web;
using System.Web.Script.Serialization;

using AI_ERP.Application_Libs;
using AI_ERP.Application_DAOs;

namespace AI_ERP.APIs.Master
{
    public class Siswa : ISiswa
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
                    cmd.CommandText = DAO_Siswa.SP_SELECT_ALL_BY_NAMA_SIMPLE;
                    cmd.Parameters.AddWithValue("@nama", (kata_kunci == null ? "" : kata_kunci));
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            hasil.Add(string.Format("{0}~{1}", Libs.GetPerbaikiEjaanNama(sdr["Nama"].ToString()), sdr["Kode"]));
                        }
                    }
                    conn.Close();
                }
            }
            return hasil.ToArray();
        }

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public string[] ShowAutocompleteByUnitByTahunAjaranBySemester(string kata_kunci, string rel_unit, string tahun_ajaran, string semester)
        {
            tahun_ajaran = tahun_ajaran.Replace("-", "/");
            List<string> hasil = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = Libs.GetConnectionString_ERP();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if ((rel_unit == null ? "" : rel_unit).Trim() != "")
                    {
                        cmd.CommandText = DAO_Siswa.SP_SELECT_ALL_BY_UNIT_BY_TA_FOR_SEARCH_SIMPLE;
                        cmd.Parameters.AddWithValue("@Rel_Sekolah", (rel_unit == null ? "" : rel_unit));
                        cmd.Parameters.AddWithValue("@TahunAjaran", (tahun_ajaran == null ? "" : tahun_ajaran));
                        cmd.Parameters.AddWithValue("@Semester", (semester == null ? "" : semester));
                        cmd.Parameters.AddWithValue("@nama", (kata_kunci == null ? "" : kata_kunci));
                    }
                    else
                    {
                        cmd.CommandText = DAO_Siswa.SP_SELECT_ALL_BY_NAMA_SIMPLE;
                        cmd.Parameters.AddWithValue("@nama", (kata_kunci == null ? "" : kata_kunci));
                    }
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            hasil.Add(string.Format("{0}~{1}",
                                    Libs.GetPerbaikiEjaanNama(sdr["Nama"].ToString()) +
                                    (
                                        sdr["Kelas"].ToString().Trim() != ""
                                        ? HttpUtility.HtmlDecode("&nbsp;&nbsp;&rarr;&nbsp;&nbsp;") +
                                          sdr["Kelas"].ToString().Trim()
                                        : ""
                                    ),  
                                    sdr["Kode"])
                                );
                        }
                    }
                    conn.Close();
                }
            }
            return hasil.ToArray();
        }

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public string[] ShowAutocompleteNISSekolahByUnitByTahunAjaran(string kata_kunci, string rel_unit, string tahun_ajaran, string semester)
        {
            tahun_ajaran = tahun_ajaran.Replace("-", "/");
            List<string> hasil = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = Libs.GetConnectionString_ERP();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if ((rel_unit == null ? "" : rel_unit).Trim() != "")
                    {
                        cmd.CommandText = DAO_Siswa.SP_SELECT_ALL_BY_NISSEKOLAH_BY_UNIT_BY_TA_FOR_SEARCH_SIMPLE;
                        cmd.Parameters.AddWithValue("@Rel_Sekolah", (rel_unit == null ? "" : rel_unit));
                        cmd.Parameters.AddWithValue("@TahunAjaran", (tahun_ajaran == null ? "" : tahun_ajaran));
                        cmd.Parameters.AddWithValue("@Semester", (semester == null ? "" : semester));
                        cmd.Parameters.AddWithValue("@nama", (kata_kunci == null ? "" : kata_kunci));
                    }
                    else
                    {
                        cmd.CommandText = DAO_Siswa.SP_SELECT_ALL_BY_NISSEKOLAH_SIMPLE;
                        cmd.Parameters.AddWithValue("@nama", (kata_kunci == null ? "" : kata_kunci));
                    }
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            hasil.Add(string.Format("{0}~{1}",
                                    sdr["NISSekolah"].ToString() +
                                    (
                                        Libs.GetPerbaikiEjaanNama(sdr["Nama"].ToString()) != ""
                                        ? HttpUtility.HtmlDecode("&nbsp;&nbsp;&rarr;&nbsp;&nbsp;") +
                                          Libs.GetPerbaikiEjaanNama(sdr["Nama"].ToString())
                                        : ""
                                    ),
                                    sdr["Kode"])
                                );
                        }
                    }
                    conn.Close();
                }
            }
            return hasil.ToArray();
        }

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public string[] ShowAutocompleteNISSekolahByLevelByUnitByTahunAjaran(string kata_kunci, string rel_kelas, string rel_unit, string tahun_ajaran)
        {
            tahun_ajaran = tahun_ajaran.Replace("-", "/");
            List<string> hasil = new List<string>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = Libs.GetConnectionString_ERP();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if ((rel_unit == null ? "" : rel_unit).Trim() != "")
                    {
                        cmd.CommandText = DAO_Siswa.SP_SELECT_ALL_BY_NISSEKOLAH_BY_LEVEL_BY_UNIT_BY_TA_FOR_SEARCH_SIMPLE;
                        cmd.Parameters.AddWithValue("@Rel_Kelas", (rel_kelas == null ? "" : rel_kelas));
                        cmd.Parameters.AddWithValue("@Rel_Sekolah", (rel_unit == null ? "" : rel_unit));
                        cmd.Parameters.AddWithValue("@TahunAjaran", (tahun_ajaran == null ? "" : tahun_ajaran));
                        cmd.Parameters.AddWithValue("@nama", (kata_kunci == null ? "" : kata_kunci));
                    }
                    else
                    {
                        cmd.CommandText = DAO_Siswa.SP_SELECT_ALL_BY_NISSEKOLAH_SIMPLE;
                        cmd.Parameters.AddWithValue("@nama", (kata_kunci == null ? "" : kata_kunci));
                    }
                    cmd.Connection = conn;
                    conn.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            hasil.Add(string.Format("{0}~{1}",
                                    sdr["NISSekolah"].ToString() +
                                    (
                                        Libs.GetPerbaikiEjaanNama(sdr["Nama"].ToString()) != ""
                                        ? HttpUtility.HtmlDecode("&nbsp;&nbsp;&rarr;&nbsp;&nbsp;") +
                                          Libs.GetPerbaikiEjaanNama(sdr["Nama"].ToString())
                                        : ""
                                    ),
                                    sdr["Kode"])
                                );
                        }
                    }
                    conn.Close();
                }
            }
            return hasil.ToArray();
        }
    }
}
