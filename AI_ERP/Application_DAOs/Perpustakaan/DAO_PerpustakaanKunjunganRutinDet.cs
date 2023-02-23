using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities.Perpustakaan;

namespace AI_ERP.Application_DAOs.Perpustakaan
{
    public static class DAO_PerpustakaanKunjunganRutinDet
    {
        public const string SP_SELECT_ALL = "PerpustakaanKunjunganRutinDet_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "PerpustakaanKunjunganRutinDet_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_BY_HEADER = "PerpustakaanKunjunganRutinDet_SELECT_BY_HEADER";
        public const string SP_SELECT_BY_ID = "PerpustakaanKunjunganRutinDet_SELECT_BY_ID";
        public const string SP_SELECT_BY_GURU = "PerpustakaanKunjunganRutinDet_SELECT_BY_GURU";
        public const string SP_SELECT_BY_TAHUNAJARAN_BY_KELASDET = "PerpustakaanKunjunganRutinDet_SELECT_BY_TAHUNAJARAN_BY_KELASDET";
        public const string SP_SELECT_BY_TAHUNAJARAN_BY_KELASDET_FOR_SEARCH = "PerpustakaanKunjunganRutinDet_SELECT_BY_TAHUNAJARAN_BY_KELASDET_FOR_SEARCH";
        public const string SP_SELECT_GURUKELAS_BY_ID = "PerpustakaanKunjunganRutinDet_SELECT_GURUKELAS_BY_ID";

        public const string SP_INSERT = "PerpustakaanKunjunganRutinDet_INSERT";

        public const string SP_UPDATE = "PerpustakaanKunjunganRutinDet_UPDATE";

        public const string SP_DELETE = "PerpustakaanKunjunganRutinDet_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_PerpustakaanKunjunganRutin = "Rel_PerpustakaanKunjunganRutin";
            public const string Hari = "Hari";
            public const string Waktu = "Waktu";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string Keterangan = "Keterangan";
        }

        public class GuruKelas
        {
            public string Rel_GuruKelas { get; set; }
            public string Rel_GuruPendamping { get; set; }
            public string NamaGuruKelas { get; set; }
            public string NamaGuruPendamping { get; set; }
        }

        public static PerpustakaanKunjunganRutinDet GetEntityFromDataRow(DataRow row)
        {
            return new PerpustakaanKunjunganRutinDet
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_PerpustakaanKunjunganRutin = new Guid(row[NamaField.Rel_PerpustakaanKunjunganRutin].ToString()),
                Hari = Convert.ToInt16(row[NamaField.Hari]),
                Waktu = row[NamaField.Waktu].ToString(),
                Rel_KelasDet = new Guid(row[NamaField.Rel_KelasDet].ToString()),
                Keterangan = row[NamaField.Keterangan].ToString()
            };
        }

        public static List<PerpustakaanKunjunganRutinDet> GetAll_Entity()
        {
            List<PerpustakaanKunjunganRutinDet> hasil = new List<PerpustakaanKunjunganRutinDet>();
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

        public static PerpustakaanKunjunganRutinDet GetByID_Entity(string kode)
        {
            PerpustakaanKunjunganRutinDet hasil = new PerpustakaanKunjunganRutinDet();
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

        public static List<PerpustakaanKunjunganRutinDet> GetByGuru_Entity(string rel_guru)
        {
            List<PerpustakaanKunjunganRutinDet> hasil = new List<PerpustakaanKunjunganRutinDet>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

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

        public static List<GuruKelas> GetGuruKelasByID_Entity(string kode)
        {
            List<GuruKelas> hasil = new List<GuruKelas>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (kode == null) return hasil;
            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_GURUKELAS_BY_ID;
                comm.Parameters.AddWithValue("@" + NamaField.Kode, kode);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(
                            new GuruKelas {
                                Rel_GuruKelas = row["Rel_GuruKelas"].ToString(),
                                Rel_GuruPendamping = row["Rel_GuruPendamping"].ToString(),
                                NamaGuruKelas = row["GuruKelas"].ToString(),
                                NamaGuruPendamping = row["GuruPendamping"].ToString()
                            }
                        );
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
        public static void Insert(PerpustakaanKunjunganRutinDet m, string user_id)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, Guid.NewGuid()));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_PerpustakaanKunjunganRutin, m.Rel_PerpustakaanKunjunganRutin));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Hari, m.Hari));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Waktu, m.Waktu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, m.Keterangan));
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

        public static void Update(PerpustakaanKunjunganRutinDet m, string user_id)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_PerpustakaanKunjunganRutin, m.Rel_PerpustakaanKunjunganRutin));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Hari, m.Hari));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Waktu, m.Waktu));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, m.Keterangan));
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