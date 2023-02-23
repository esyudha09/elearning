using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities;

namespace AI_ERP.Application_DAOs
{
    public static class _DAO_AbsenAjar
    {
        public const string SP_ELEARNING_SELECT_ABSEN_AJAR_BY_NISSEKOLAH = "ELEARNING_SELECT_ABSEN_AJAR_BY_NISSEKOLAH";

        public static class NamaField
        {
            public const string Tanggal = "Tanggal";
            public const string TahunAjaran = "TahunAjaran";
            public const string Kelas = "Kelas";
            public const string Semester = "Semester";
            public const string SubSemester = "SubSemester";
            public const string Guru = "Guru";
            public const string NamaGuru = "NamaGuru";
            public const string Mapel = "Mapel";
            public const string NamaMapel = "NamaMapel";
            public const string Ruang = "Ruang";
            public const string JamMasuk = "JamMasuk";
            public const string JamKeluar = "JamKeluar";
            public const string Keterangan = "Keterangan";
            public const string JamKe = "JamKe";
            public const string NISSekolah = "NISSekolah";
            public const string IsSakit = "IsSakit";
            public const string IsIzin = "IsIzin";
            public const string IsAlfa = "IsAlfa";
            public const string KeteranganAbsen = "KeteranganAbsen";
        }

        private static _AbsenAjar GetEntityFromDataRow(DataRow row)
        {
            return new _AbsenAjar
            {
                Tanggal = Convert.ToDateTime(row[NamaField.Tanggal]),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Kelas = row[NamaField.Kelas].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                SubSemester = row[NamaField.SubSemester].ToString(),
                Guru = row[NamaField.Guru].ToString(),
                NamaGuru = row[NamaField.NamaGuru].ToString(),
                Mapel = row[NamaField.Mapel].ToString(),
                NamaMapel = row[NamaField.NamaMapel].ToString(),
                Ruang = row[NamaField.Ruang].ToString(),
                JamMasuk = row[NamaField.JamMasuk].ToString(),
                JamKeluar = row[NamaField.JamKeluar].ToString(),
                Keterangan = row[NamaField.Keterangan].ToString(),
                JamKe = row[NamaField.JamKe].ToString(),
                NISSekolah = row[NamaField.NISSekolah].ToString(),
                IsSakit = Convert.ToBoolean(row[NamaField.IsSakit]),
                IsIzin = Convert.ToBoolean(row[NamaField.IsIzin]),
                IsAlfa = Convert.ToBoolean(row[NamaField.IsAlfa]),
                KeteranganAbsen = row[NamaField.KeteranganAbsen].ToString()
            };
        }

        public static List<_AbsenAjar> GetByNISSekolah_Entity(string nis_sekolah)
        {
            List<_AbsenAjar> hasil = new List<_AbsenAjar>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_ELEARNING_SELECT_ABSEN_AJAR_BY_NISSEKOLAH;
                comm.Parameters.AddWithValue("@NISSekolah", nis_sekolah);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRow(row));
                }
            }
            catch (Exception ec)
            {
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            return hasil;
        }
    }
}