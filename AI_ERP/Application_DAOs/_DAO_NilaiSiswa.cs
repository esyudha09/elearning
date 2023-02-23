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
    public class _DAO_NilaiSiswa
    {
        public const string SP_ELEARNING_SELECT_NILAI_BY_SISWA = "ELEARNING_SELECT_NILAI_BY_SISWA";

        public static class NamaField
        {
            public const string TahunAjaran = "TahunAjaran";
            public const string Kelas = "Kelas";
            public const string Semester = "Semester";
            public const string Mapel = "Mapel";
            public const string NamaMapel = "NamaMapel";
            public const string Guru = "Guru";
            public const string NamaGuru = "NamaGuru";
            public const string NISSekolah = "NISSekolah";
            public const string AspekNilai = "AspekNilai";
            public const string NamaAspek = "NamaAspek";
            public const string NamaSubAspek = "NamaSubAspek";
            public const string UrutNilai = "UrutNilai";
            public const string Nilai = "Nilai";
        }

        private static _NilaiSiswa GetEntityFromDataRow(DataRow row)
        {
            return new _NilaiSiswa
            {
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Kelas = row[NamaField.Kelas].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Mapel = row[NamaField.Mapel].ToString(),
                NamaMapel = row[NamaField.NamaMapel].ToString(),
                Guru = row[NamaField.Guru].ToString(),
                NamaGuru = row[NamaField.NamaGuru].ToString(),                
                NISSekolah = row[NamaField.NISSekolah].ToString(),
                AspekNilai = row[NamaField.AspekNilai].ToString(),
                NamaAspek = row[NamaField.NamaAspek].ToString(),
                NamaSubAspek = row[NamaField.NamaSubAspek].ToString(),
                UrutNilai = row[NamaField.UrutNilai].ToString(),
                Nilai = (row[NamaField.Nilai]==DBNull.Value ? Application_Libs.Constantas.NilaiDesimalNULL : Convert.ToDecimal(row[NamaField.Nilai]))
            };
        }

        public static List<_NilaiSiswa> GetByNISSekolah_Entity(string nis_sekolah)
        {
            List<_NilaiSiswa> hasil = new List<_NilaiSiswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_RaporOld();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_ELEARNING_SELECT_NILAI_BY_SISWA;
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