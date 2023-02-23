using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities.Elearning;

namespace AI_ERP.Application_DAOs.Elearning
{
    public static class DAO_IsiNilai_Log
    {
        public const string SP_SELECT_ALL = "IsiNilai_Log_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "IsiNilai_Log_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "IsiNilai_Log_SELECT_BY_ID";
        public const string SP_SELECT_BY_GURU_BY_MAPEL_BY_KELASDET_BISA_EDIT = "IsiNilai_Log_SELECT_BY_GURU_BY_MAPEL_BY_KELASDET_BISA_EDIT";
        public const string SP_SELECT_BY_GURU_BY_MAPEL_BY_KELASDET_BY_TAHUN_AJARAN_BY_SEMESTER_BISA_EDIT = "IsiNilai_Log_SELECT_BY_GURU_BY_MAPEL_BY_KELASDET_BY_TA_BY_SM_BISA_EDIT";

        public const string SP_INSERT = "IsiNilai_Log_INSERT";

        public const string SP_UPDATE = "IsiNilai_Log_UPDATE";

        public const string SP_DELETE = "IsiNilai_Log_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Tanggal = "Tanggal";
            public const string Rel_ProsesRapor = "Rel_ProsesRapor";
            public const string Rel_Sekolah = "Rel_Sekolah";
            public const string Rel_Guru = "Rel_Guru";
            public const string Rel_Mapel = "Rel_Mapel";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string Alasan = "Alasan";
            public const string Keterangan = "Keterangan";
            public const string IsClosed = "IsClosed";
            public const string UserIDOpened = "UserIDOpened";
            public const string UserIDClosed = "UserIDClosed";
            public const string ClosedDate = "ClosedDate";
        }

        private static IsiNilai_Log GetEntityFromDataRow(DataRow row)
        {
            return new IsiNilai_Log
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Tanggal = Convert.ToDateTime(row[NamaField.Tanggal]),
                Rel_ProsesRapor = row[NamaField.Rel_ProsesRapor].ToString(),
                Rel_Sekolah = row[NamaField.Rel_Sekolah].ToString(),
                Rel_Guru = row[NamaField.Rel_Guru].ToString(),
                Rel_Mapel = row[NamaField.Rel_Mapel].ToString(),
                Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString(),
                Alasan = row[NamaField.Alasan].ToString(),
                Keterangan = row[NamaField.Keterangan].ToString(),
                IsClosed = (row[NamaField.IsClosed] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsClosed])),
                UserIDOpened = row[NamaField.UserIDOpened].ToString(),
                UserIDClosed = row[NamaField.UserIDClosed].ToString(),
                ClosedDate = Convert.ToDateTime(row[NamaField.ClosedDate] == DBNull.Value ? DateTime.MinValue : row[NamaField.ClosedDate])
            };
        }

        public static List<IsiNilai_Log> GetAll_Entity()
        {
            List<IsiNilai_Log> hasil = new List<IsiNilai_Log>();
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

        public static IsiNilai_Log GetByID_Entity(string kode)
        {
            IsiNilai_Log hasil = new IsiNilai_Log();
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

        public static List<IsiNilai_Log> GetBisaEdit_Entity(
                string rel_guru,
                string rel_mapel,
                string rel_kelasdet,
                string tahun_ajaran,
                string semester
            )
        {
            List<IsiNilai_Log> hasil = new List<IsiNilai_Log>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_GURU_BY_MAPEL_BY_KELASDET_BY_TAHUN_AJARAN_BY_SEMESTER_BISA_EDIT;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Guru, rel_guru);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Mapel, rel_mapel);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelasdet);
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

        public static void Delete(string Kode)
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

        public static void Insert(IsiNilai_Log m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Tanggal, m.Tanggal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_ProsesRapor, m.Rel_ProsesRapor));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, m.Rel_Sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Guru, m.Rel_Guru));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Alasan, m.Alasan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, m.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsClosed, m.IsClosed));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.UserIDOpened, m.UserIDOpened));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.UserIDClosed, m.UserIDClosed));
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

        public static void Update(IsiNilai_Log m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Guru, m.Rel_Guru));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Alasan, m.Alasan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, m.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsClosed, m.IsClosed));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.UserIDClosed, m.UserIDClosed));
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