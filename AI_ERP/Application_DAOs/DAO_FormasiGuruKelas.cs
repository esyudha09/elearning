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
    public static class DAO_FormasiGuruKelas
    {
        public const string SP_SELECT_ALL = "FormasiGuruKelas_SELECT_ALL";
        public const string SP_SELECT_ALL_BY_UNIT = "FormasiGuruKelas_SELECT_ALL_BY_UNIT";
        public const string SP_SELECT_ALL_FOR_SEARCH = "FormasiGuruKelas_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_ALL_BY_UNIT_FOR_SEARCH = "FormasiGuruKelas_SELECT_ALL_BY_UNIT_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "FormasiGuruKelas_SELECT_BY_ID";        
        public const string SP_SELECT_BY_GURU = "FormasiGuruKelas_SELECT_BY_GURU";
        public const string SP_SELECT_BY_GURU_BY_TAHUN_AJARAN = "FormasiGuruKelas_SELECT_BY_GURU_BY_TA";
        public const string SP_SELECT_BY_GURU_BY_TAHUN_AJARAN_BY_SEMESTER = "FormasiGuruKelas_SELECT_BY_GURU_BY_TAHUN_AJARAN_BY_SEMESTER";
        public const string SP_SELECT_BY_UNIT_BY_TAHUN_AJARAN_BY_SEMESTER = "FormasiGuruKelas_SELECT_BY_UNIT_BY_TA_BY_SM";

        public const string SP_INSERT = "FormasiGuruKelas_INSERT";

        public const string SP_UPDATE = "FormasiGuruKelas_UPDATE";

        public const string SP_DELETE = "FormasiGuruKelas_DELETE";
        

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Sekolah = "Rel_Sekolah";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_GuruKelas = "Rel_GuruKelas";
            public const string Rel_GuruPendamping = "Rel_GuruPendamping";
            public const string Rel_KelasDet = "Rel_KelasDet";
        }

        public static class NamaField_ByGuru
        {
            public const string Rel_Sekolah = "Rel_Sekolah";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string KelasDet = "KelasDet";
            public const string Mapel = "Mapel";
            public const string KodeMapel = "KodeMapel";
            public const string Urutan = "Urutan";
        }

        public static FormasiGuruKelas GetEntityFromDataRow(DataRow row)
        {
            return new FormasiGuruKelas
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Sekolah = new Guid(row[NamaField.Rel_Sekolah].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Rel_GuruKelas = row[NamaField.Rel_GuruKelas].ToString(),
                Rel_GuruPendamping = row[NamaField.Rel_GuruPendamping].ToString(),
                Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString()
            };
        }

        public static FormasiGuruKelas_ByGuru GetEntityFromDataRow_ByGuru(DataRow row)
        {
            return new FormasiGuruKelas_ByGuru
            {
                Rel_Sekolah = new Guid(row[NamaField_ByGuru.Rel_Sekolah].ToString()),
                TahunAjaran = row[NamaField_ByGuru.TahunAjaran].ToString(),
                Semester = row[NamaField_ByGuru.Semester].ToString(),
                Rel_KelasDet = row[NamaField_ByGuru.Rel_KelasDet].ToString(),
                KelasDet = row[NamaField_ByGuru.KelasDet].ToString(),
                Mapel = row[NamaField_ByGuru.Mapel].ToString(),
                KodeMapel = row[NamaField_ByGuru.KodeMapel].ToString(),
                Urutan = Convert.ToDouble(row[NamaField_ByGuru.Urutan])
            };
        }

        public static List<FormasiGuruKelas> GetAll_Entity()
        {
            List<FormasiGuruKelas> hasil = new List<FormasiGuruKelas>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_ALL;

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

        public static FormasiGuruKelas GetByID_Entity(string kode)
        {
            FormasiGuruKelas hasil = new FormasiGuruKelas();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (kode == null) return hasil;
            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_ID;
                comm.Parameters.AddWithValue("@" + NamaField.Kode, kode);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil = GetEntityFromDataRow(row);
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

        

        public static void Delete(string Kode, string user_id)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
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
        public static void Insert(FormasiGuruKelas formasi_guru, string user_id)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_INSERT;

                if (formasi_guru.Kode.ToString() == Application_Libs.Constantas.GUID_NOL) formasi_guru.Kode = Guid.NewGuid();

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, formasi_guru.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, formasi_guru.Rel_Sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, formasi_guru.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, formasi_guru.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_GuruKelas, formasi_guru.Rel_GuruKelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_GuruPendamping, formasi_guru.Rel_GuruPendamping));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, formasi_guru.Rel_KelasDet));
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

        public static void Update(FormasiGuruKelas formasi_guru, string user_id)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_UPDATE;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, formasi_guru.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, formasi_guru.Rel_Sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, formasi_guru.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, formasi_guru.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_GuruKelas, formasi_guru.Rel_GuruKelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_GuruPendamping, formasi_guru.Rel_GuruPendamping));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, formasi_guru.Rel_KelasDet));
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

        public static List<FormasiGuruKelas> GetByGuruByTABySM_Entity(string rel_guru, string tahun_ajaran, string semester)
        {
            List<FormasiGuruKelas> hasil = new List<FormasiGuruKelas>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_GURU_BY_TAHUN_AJARAN_BY_SEMESTER;
                comm.Parameters.AddWithValue("@Rel_Guru", rel_guru);
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);

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

        public static List<FormasiGuruKelas_ByGuru> GetByGuruByTA_Entity(string rel_guru, string tahun_ajaran)
        {
            List<FormasiGuruKelas_ByGuru> hasil = new List<FormasiGuruKelas_ByGuru>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (rel_guru == null) return hasil;
            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_GURU_BY_TAHUN_AJARAN;
                comm.Parameters.AddWithValue("@Rel_Guru", rel_guru);
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRow_ByGuru(row));
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

        public static List<FormasiGuruKelas> GetByUnitByTABySM_Entity(string rel_sekolah, string tahun_ajaran, string semester)
        {
            List<FormasiGuruKelas> hasil = new List<FormasiGuruKelas>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_UNIT_BY_TAHUN_AJARAN_BY_SEMESTER;
                comm.Parameters.AddWithValue("@Rel_Sekolah", rel_sekolah);
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);

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

        public static List<FormasiGuruKelas_ByGuru> GetByGuru_Entity(string rel_guru)
        {
            List<FormasiGuruKelas_ByGuru> hasil = new List<FormasiGuruKelas_ByGuru>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (rel_guru == null) return hasil;
            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_GURU;
                comm.Parameters.AddWithValue("@Rel_Guru", rel_guru);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRow_ByGuru(row));
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