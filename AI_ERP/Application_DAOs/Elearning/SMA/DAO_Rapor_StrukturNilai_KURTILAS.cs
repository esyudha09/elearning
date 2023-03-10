using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities.Elearning.SMA;

namespace AI_ERP.Application_DAOs.Elearning.SMA
{
    public static class DAO_Rapor_StrukturNilai_KURTILAS
    {
        public const string SP_SELECT_ALL = "SMA_Rapor_StrukturNilai_KURTILAS_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "SMA_Rapor_StrukturNilai_KURTILAS_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "SMA_Rapor_StrukturNilai_KURTILAS_SELECT_BY_ID";
        public const string SP_SELECT_BY_TA_BY_SM = "SMA_Rapor_StrukturNilai_KURTILAS_SELECT_BY_TA_BY_SM";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELAS_BY_MAPEL = "SMA_Rapor_StrukturNilai_KURTILAS_SELECT_BY_TA_BY_SM_BY_KELAS_BY_MAPEL";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELAS = "SMA_Rapor_StrukturNilai_KURTILAS_SELECT_BY_TA_BY_SM_BY_KELAS";

        public const string SP_INSERT = "SMA_Rapor_StrukturNilai_KURTILAS_INSERT";

        public const string SP_UPDATE = "SMA_Rapor_StrukturNilai_KURTILAS_UPDATE";

        public const string SP_DELETE = "SMA_Rapor_StrukturNilai_KURTILAS_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_Kelas = "Rel_Kelas";
            public const string Rel_Mapel = "Rel_Mapel";
            public const string Kurikulum = "Kurikulum";
            public const string KKM = "KKM";
            public const string JenisPerhitungan = "JenisPerhitungan";
            public const string BobotRaporPengetahuan = "BobotRaporPengetahuan";
            public const string BobotRaporUAS = "BobotRaporUAS";
            public const string Deskripsi = "Deskripsi";
            public const string IsNilaiAkhir = "IsNilaiAkhir";
            public const string DeskripsiSikapSosial = "DeskripsiSikapSosial";
            public const string DeskripsiSikapSpiritual = "DeskripsiSikapSpiritual";
        }

        private static Rapor_StrukturNilai_KURTILAS GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_StrukturNilai_KURTILAS
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Rel_Kelas = row[NamaField.Rel_Kelas].ToString(),
                Rel_Mapel = new Guid(row[NamaField.Rel_Mapel].ToString()),
                KKM = Convert.ToDecimal(row[NamaField.KKM]),
                JenisPerhitungan = row[NamaField.JenisPerhitungan].ToString(),
                BobotRaporPengetahuan = Convert.ToDecimal(
                        (
                            row[NamaField.BobotRaporPengetahuan] == DBNull.Value
                            ? 0
                            : row[NamaField.BobotRaporPengetahuan]
                        )
                    ),
                BobotRaporUAS = Convert.ToDecimal(
                        (
                            row[NamaField.BobotRaporUAS] == DBNull.Value
                            ? 0 
                            : row[NamaField.BobotRaporUAS]
                        )
                    ),
                Deskripsi = row[NamaField.Deskripsi].ToString(),
                IsNilaiAkhir = (row[NamaField.IsNilaiAkhir] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsNilaiAkhir])),
                DeskripsiSikapSosial = row[NamaField.DeskripsiSikapSosial].ToString(),
                DeskripsiSikapSpiritual = row[NamaField.DeskripsiSikapSpiritual].ToString()
            };
        }

        public static List<Rapor_StrukturNilai_KURTILAS> GetAllByTABySMByKelasByMapel_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelas,
                string rel_mapel
            )
        {
            List<Rapor_StrukturNilai_KURTILAS> hasil = new List<Rapor_StrukturNilai_KURTILAS>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELAS_BY_MAPEL;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas, rel_kelas);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Mapel, rel_mapel);

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
        
        public static List<Rapor_StrukturNilai_KURTILAS> GetAllByTABySMByKelas_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelas
            )
        {
            List<Rapor_StrukturNilai_KURTILAS> hasil = new List<Rapor_StrukturNilai_KURTILAS>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELAS;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas, rel_kelas);

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

        public static Rapor_StrukturNilai_KURTILAS GetByID_Entity(string kode)
        {
            Rapor_StrukturNilai_KURTILAS hasil = new Rapor_StrukturNilai_KURTILAS();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
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

        public static void Insert(Rapor_StrukturNilai_KURTILAS m, string user_id)
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

                Guid kode = Guid.NewGuid();

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas, m.Rel_Kelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KKM, m.KKM));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisPerhitungan, m.JenisPerhitungan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotRaporPengetahuan, m.BobotRaporPengetahuan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotRaporUAS, m.BobotRaporUAS));                
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Deskripsi, m.Deskripsi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsNilaiAkhir, m.IsNilaiAkhir));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DeskripsiSikapSosial, m.DeskripsiSikapSosial));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DeskripsiSikapSpiritual, m.DeskripsiSikapSpiritual));
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

        public static void InsertLengkap(Rapor_StrukturNilai_KURTILAS m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas, m.Rel_Kelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KKM, m.KKM));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisPerhitungan, m.JenisPerhitungan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotRaporPengetahuan, m.BobotRaporPengetahuan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotRaporUAS, m.BobotRaporUAS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Deskripsi, m.Deskripsi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsNilaiAkhir, m.IsNilaiAkhir));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DeskripsiSikapSosial, m.DeskripsiSikapSosial));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DeskripsiSikapSpiritual, m.DeskripsiSikapSpiritual));
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

        public static List<Rapor_StrukturNilai_KURTILAS> GetAll_Entity()
        {
            List<Rapor_StrukturNilai_KURTILAS> hasil = new List<Rapor_StrukturNilai_KURTILAS>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
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

        public static List<Rapor_StrukturNilai_KURTILAS> GetAllByTABySM_Entity(
                string tahun_ajaran,
                string semester
            )
        {
            List<Rapor_StrukturNilai_KURTILAS> hasil = new List<Rapor_StrukturNilai_KURTILAS>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);

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

        public static void Update(Rapor_StrukturNilai_KURTILAS m, string user_id)
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
                comm.CommandText = SP_UPDATE;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas, m.Rel_Kelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.KKM, m.KKM));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JenisPerhitungan, m.JenisPerhitungan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotRaporPengetahuan, m.BobotRaporPengetahuan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.BobotRaporUAS, m.BobotRaporUAS));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Deskripsi, m.Deskripsi));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsNilaiAkhir, m.IsNilaiAkhir));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DeskripsiSikapSosial, m.DeskripsiSikapSosial));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DeskripsiSikapSpiritual, m.DeskripsiSikapSpiritual));
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