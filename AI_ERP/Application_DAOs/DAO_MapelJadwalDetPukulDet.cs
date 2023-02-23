using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities;
using AI_ERP.Application_Entities.Elearning;

namespace AI_ERP.Application_DAOs
{
    public static class DAO_MapelJadwalDetPukulDet
    {
        public const string SP_SELECT_BY_ID = "MapelJadwalDetPukulDet_SELECT_BY_ID";
        public const string SP_SELECT_BY_HEADER = "MapelJadwalDetPukulDet_SELECT_BY_HEADER";
        public const string SP_SELECT_BY_JADWAL = "MapelJadwalDetPukulDet_SELECT_BY_JADWAL";

        public const string SP_INSERT = "MapelJadwalDetPukulDet_INSERT";

        public const string SP_UPDATE = "MapelJadwalDetPukulDet_UPDATE";

        public const string SP_DELETE = "MapelJadwalDetPukulDet_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_MapelJadwal = "Rel_MapelJadwal";
            public const string Rel_MapelJadwalDetPukul = "Rel_MapelJadwalDetPukul";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string Rel_Mapel = "Rel_Mapel";
            public const string Mapel = "Mapel";
            public const string AliasMapel = "AliasMapel";
        }

        public class MapelJadwalDetPukulDet_Lengkap : MapelJadwalDetPukulDet
        {
            public string Mapel { get; set; }
            public string AliasMapel { get; set; }
        }

        private static MapelJadwalDetPukulDet GetEntityFromDataRow(DataRow row)
        {
            return new MapelJadwalDetPukulDet
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_MapelJadwalDetPukul = row[NamaField.Rel_MapelJadwalDetPukul].ToString(),
                Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString(),
                Rel_Mapel = row[NamaField.Rel_Mapel].ToString()
            };
        }

        private static MapelJadwalDetPukulDet_Lengkap GetEntityFromDataRow_Lengkap(DataRow row)
        {
            return new MapelJadwalDetPukulDet_Lengkap
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_MapelJadwalDetPukul = row[NamaField.Rel_MapelJadwalDetPukul].ToString(),
                Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString(),
                Rel_Mapel = row[NamaField.Rel_Mapel].ToString(),
                Mapel = row[NamaField.Mapel].ToString(),
                AliasMapel = row[NamaField.AliasMapel].ToString()
            };
        }

        public static void Delete(string Kode)
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

        public static void Insert(MapelJadwalDetPukulDet m)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
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

                Guid kode = m.Kode;
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_MapelJadwalDetPukul, m.Rel_MapelJadwalDetPukul));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
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

        public static void Update(MapelJadwalDetPukulDet m)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_MapelJadwalDetPukul, m.Rel_MapelJadwalDetPukul));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, m.Rel_Mapel));
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

        public static List<MapelJadwalDetPukulDet> GetAllByHeader_Entity(
                string rel_mapeljadwaldetpukul
            )
        {
            List<MapelJadwalDetPukulDet> hasil = new List<MapelJadwalDetPukulDet>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_MapelJadwalDetPukul, rel_mapeljadwaldetpukul);

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

        public static List<MapelJadwalDetPukulDet_Lengkap> GetAllByJadwal_Entity(
                string rel_mapeljadwal
            )
        {
            List<MapelJadwalDetPukulDet_Lengkap> hasil = new List<MapelJadwalDetPukulDet_Lengkap>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_JADWAL;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_MapelJadwal, rel_mapeljadwal);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRow_Lengkap(row));
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

        public static MapelJadwalDetPukulDet GetByID_Entity(
                string kode
            )
        {
            MapelJadwalDetPukulDet hasil = new MapelJadwalDetPukulDet();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
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
    }
}