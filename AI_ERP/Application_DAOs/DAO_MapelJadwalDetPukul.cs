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
    public static class DAO_MapelJadwalDetPukul
    {
        public const string SP_SELECT_BY_ID = "MapelJadwalDetPukul_SELECT_BY_ID";
        public const string SP_SELECT_BY_HEADER = "MapelJadwalDetPukul_SELECT_BY_HEADER";

        public const string SP_INSERT = "MapelJadwalDetPukul_INSERT";

        public const string SP_UPDATE = "MapelJadwalDetPukul_UPDATE";

        public const string SP_DELETE = "MapelJadwalDetPukul_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_MapelJadwalDet = "Rel_MapelJadwalDet";
            public const string DariJam = "DariJam";
            public const string SampaiJam = "SampaiJam";
        }

        private static MapelJadwalDetPukul GetEntityFromDataRow(DataRow row)
        {
            return new MapelJadwalDetPukul
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_MapelJadwalDet = row[NamaField.Rel_MapelJadwalDet].ToString(),
                DariJam = Convert.ToDateTime(row[NamaField.DariJam]),
                SampaiJam = Convert.ToDateTime(row[NamaField.SampaiJam])
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
        
        public static void Insert(MapelJadwalDetPukul m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_MapelJadwalDet, m.Rel_MapelJadwalDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DariJam, m.DariJam));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SampaiJam, m.SampaiJam));
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

        public static void Update(MapelJadwalDetPukul m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_MapelJadwalDet, m.Rel_MapelJadwalDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.DariJam, m.DariJam));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.SampaiJam, m.SampaiJam));
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

        public static List<MapelJadwalDetPukul> GetAllByHeader_Entity(
                string rel_mapeljadwaldet
            )
        {
            List<MapelJadwalDetPukul> hasil = new List<MapelJadwalDetPukul>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_MapelJadwalDet, rel_mapeljadwaldet);

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

        public static MapelJadwalDetPukul GetByID_Entity(
                string kode
            )
        {
            MapelJadwalDetPukul hasil = new MapelJadwalDetPukul();
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