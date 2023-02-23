using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities;
using AI_ERP.Application_Entities.Elearning;

namespace AI_ERP.Application_DAOs.Elearning
{
    public static class DAO_CatatanSiswa
    {
        public const string SP_SELECT_BY_SISWA = "CatatanSiswa_SELECT_BY_SISWA";
        public const string SP_SELECT_KELAS_BY_GURU = "CatatanSiswa_SELECT_KELAS_BY_GURU";
        public const string SP_SELECT_KELAS_TAHUNAJARAN_SEMESTER_BY_GURU = "CatatanSiswa_SELECT_KELAS_TAHUNAJARAN_SEMESTER_BY_GURU";
        public const string SP_SELECT_BY_ID = "CatatanSiswa_SELECT_BY_ID";
        public const string SP_SELECT_BY_GURU_BY_KELAS = "CatatanSiswa_SELECT_BY_GURU_BY_KELAS";
        public const string SP_SELECT_BY_GURU = "CatatanSiswa_SELECT_BY_GURU";
        public const string SP_SELECT_BY_GURU_FOR_SEARCH = "CatatanSiswa_SELECT_KELAS_BY_GURU_FOR_SEARCH";
        public const string SP_SELECT_BY_GURU_BY_KELAS_FOR_SEARCH = "CatatanSiswa_SELECT_BY_GURU_BY_KELAS_FOR_SEARCH";

        public const string SP_DELETE = "CatatanSiswa_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Siswa = "Rel_Siswa";
            public const string Rel_Kategori = "Rel_Kategori";
            public const string Tanggal = "Tanggal";
            public const string Catatan = "Catatan";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string Rel_Mapel = "Rel_Mapel";
            public const string Rel_Guru = "Rel_Guru";

            public const string Siswa = "Siswa";
            public const string Kelas = "Kelas";

            public static class KelasPeriode
            {
                public const string Kode = "Kode";
                public const string Kelas = "Kelas";
                public const string TahunAjaran = "TahunAjaran";
                public const string Semester = "Semester";
            }
        }
        
        public static CatatanSiswa GetEntityFromDataRow(DataRow row)
        {
            return new CatatanSiswa
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                Rel_Kategori = row[NamaField.Rel_Kategori].ToString(),
                Tanggal = Convert.ToDateTime(row[NamaField.Tanggal]),
                Catatan = row[NamaField.Catatan].ToString(),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString(),
                Rel_Mapel = row[NamaField.Rel_Mapel].ToString(),
                Rel_Guru = row[NamaField.Rel_Guru].ToString()
            };
        }

        public static CatatanSiswaByGuru GetEntityFromDataRow_ByGuru(DataRow row)
        {
            return new CatatanSiswaByGuru
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                Rel_Kategori = row[NamaField.Rel_Kategori].ToString(),
                Tanggal = Convert.ToDateTime(row[NamaField.Tanggal]),
                Catatan = row[NamaField.Catatan].ToString(),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString(),
                Rel_Mapel = row[NamaField.Rel_Mapel].ToString(),
                Rel_Guru = row[NamaField.Rel_Guru].ToString(),
                Siswa = row[NamaField.Siswa].ToString(),
                Kelas = row[NamaField.Kelas].ToString()
            };
        }

        public static CatatanSiswaKelasPeriode GetEntityFromDataRow_KelasPeriode(DataRow row)
        {
            return new CatatanSiswaKelasPeriode
            {
                Kode = new Guid(row[NamaField.KelasPeriode.Kode].ToString()),
                Kelas = row[NamaField.KelasPeriode.Kelas].ToString(),
                TahunAjaran = row[NamaField.KelasPeriode.TahunAjaran].ToString(),
                Semester = row[NamaField.KelasPeriode.Semester].ToString()
            };
        }

        public static List<CatatanSiswa> GetBySiswa_Entity(string rel_siswa)
        {
            List<CatatanSiswa> hasil = new List<CatatanSiswa>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_SISWA;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Siswa, rel_siswa);

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

        public static List<KelasDet> GetKelasByGuru_Entity(string rel_guru)
        {
            List<KelasDet> hasil = new List<KelasDet>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_KELAS_BY_GURU;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Guru, rel_guru);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(DAO_KelasDet.GetEntityFromDataRow(row));
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

        public static CatatanSiswa GetByID_Entity(string kode)
        {
            CatatanSiswa hasil = new CatatanSiswa();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_ID;
                comm.Parameters.AddWithValue("@" + NamaField.Kode, kode);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil = DAO_CatatanSiswa.GetEntityFromDataRow(row);
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

        public static List<CatatanSiswaKelasPeriode> GetKelasPeriode_Entity(string rel_guru)
        {
            List<CatatanSiswaKelasPeriode> hasil = new List<CatatanSiswaKelasPeriode>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_KELAS_TAHUNAJARAN_SEMESTER_BY_GURU;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Guru, rel_guru);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRow_KelasPeriode(row));
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

        public static void Delete(string Kode, string rel_guru, string user_id)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_DELETE;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Guru, rel_guru));
                comm.Parameters.Add(new SqlParameter("@user_id", user_id));

                comm.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ec)
            {
                transaction.Rollback();
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
    }
}