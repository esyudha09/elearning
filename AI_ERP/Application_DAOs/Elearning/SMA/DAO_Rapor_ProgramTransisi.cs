using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities;
using AI_ERP.Application_Entities.Elearning.SMA;

namespace AI_ERP.Application_DAOs.Elearning.SMA
{
    public static class DAO_Rapor_ProgramTransisi
    {
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELASDET = "SMA_Rapor_ProgramTransisi_SELECT_BY_TA_BY_SM_BY_KELASDET";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELASDET_BY_SISWA = "SMA_Rapor_ProgramTransisi_SELECT_BY_TA_BY_SM_BY_KELASDET_BY_SISWA";

        public const string SP_INSERT = "SMA_Rapor_ProgramTransisi_INSERT";

        public const string SP_DELETE = "SMA_Rapor_ProgramTransisi_DELETE_BY_TA_BY_SM_BY_KELASDET_BY_SISWA";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string Rel_Siswa = "Rel_Siswa";
            public const string LayananSosial = "LayananSosial";
            public const string LayananSosial_JumlahJam = "LayananSosial_JumlahJam";
            public const string LayananSosial_Keterangan = "LayananSosial_Keterangan";
            public const string Kewirausahaan = "Kewirausahaan";
            public const string Kewirausahaan_JumlahJam = "Kewirausahaan_JumlahJam";
            public const string Kewirausahaan_Keterangan = "Kewirausahaan_Keterangan";
            public const string Internship = "Internship";
            public const string Internship_JumlahJam = "Internship_JumlahJam";
            public const string Internship_Keterangan = "Internship_Keterangan";

        }

        private static Rapor_ProgramTransisi GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_ProgramTransisi
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString(),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                LayananSosial = row[NamaField.LayananSosial].ToString(),
                LayananSosial_JumlahJam = row[NamaField.LayananSosial_JumlahJam].ToString(),
                LayananSosial_Keterangan = row[NamaField.LayananSosial_Keterangan].ToString(),
                Kewirausahaan = row[NamaField.Kewirausahaan].ToString(),
                Kewirausahaan_JumlahJam = row[NamaField.Kewirausahaan_JumlahJam].ToString(),
                Kewirausahaan_Keterangan = row[NamaField.Kewirausahaan_Keterangan].ToString(),
                Internship = row[NamaField.Internship].ToString(),
                Internship_JumlahJam = row[NamaField.Internship_JumlahJam].ToString(),
                Internship_Keterangan = row[NamaField.Internship_Keterangan].ToString()
            };
        }

        public static void Insert(Rapor_ProgramTransisi m)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();

                comm.Parameters.Clear();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_INSERT;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LayananSosial, m.LayananSosial));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LayananSosial_JumlahJam, m.LayananSosial_JumlahJam));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.LayananSosial_Keterangan, m.LayananSosial_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kewirausahaan, m.Kewirausahaan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kewirausahaan_JumlahJam, m.Kewirausahaan_JumlahJam));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kewirausahaan_Keterangan, m.Kewirausahaan_Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Internship, m.Internship));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Internship_JumlahJam, m.Internship_JumlahJam));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Internship_Keterangan, m.Internship_Keterangan));
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

        public static void DeleteByTABySMByKelasDetBySiswa(
                string tahun_ajaran,
                string semester,
                string rel_kelasdet,
                string rel_siswa
            )
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();

                comm.Parameters.Clear();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_DELETE;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, tahun_ajaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, rel_kelasdet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, rel_siswa));
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

        public static List<Rapor_ProgramTransisi> GetByTABySMByKelasDetBySiswa(
                string tahun_ajaran,
                string semester,
                string rel_kelasdet,
                string rel_siswa
            )
        {
            List<Rapor_ProgramTransisi> hasil = new List<Rapor_ProgramTransisi>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELASDET_BY_SISWA;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, tahun_ajaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, rel_kelasdet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, rel_siswa));

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

        public static List<Rapor_ProgramTransisi> GetByTABySMByKelasDet(
                string tahun_ajaran,
                string semester,
                string rel_kelasdet
            )
        {
            List<Rapor_ProgramTransisi> hasil = new List<Rapor_ProgramTransisi>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELASDET;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, tahun_ajaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, rel_kelasdet));

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