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
    public static class DAO_LinimasaKelas
    {
        public const string SP_SELECT_BY_ID = "LinimasaKelas_SELECT_BY_ID";
        public const string SP_SELECT_BY_TAHUNAJARAN_BY_KELASDET = "LinimasaKelas_SELECT_BY_TAHUNAJARAN_BY_KELASDET";
        public const string SP_SELECT_BY_TAHUNAJARAN_BY_KELASDET_BY_PERIODE = "LinimasaKelas_SELECT_BY_TAHUNAJARAN_BY_KELASDET_BY_PERIODE";       
        public const string SP_SELECT_BY_TAHUNAJARAN_BY_KELASDET_BY_PERIODE_BY_MAPEL = "LinimasaKelas_SELECT_BY_TAHUNAJARAN_BY_KELASDET_BY_PERIODE_BY_MAPEL";
        public const string SP_SELECT_BY_TANGGAL_BY_JENIS_BY_TAHUNAJARAN_BY_KELASDET = "LinimasaKelas_SELECT_BY_TANGGAL_BY_JENIS_BY_TAHUNAJARAN_BY_KELASDET";
        public const string SP_SELECT_BY_TANGGAL_BY_JENIS_BY_TAHUNAJARAN_BY_KELASDET_BY_KETERANGAN = "LinimasaKelas_SELECT_BY_TANGGAL_BY_JENIS_BY_TAHUNAJARAN_BY_KELASDET_BY_KETERANGAN";

        public const string SP_INSERT = "LinimasaKelas_INSERT";

        public const string SP_UPDATE = "LinimasaKelas_UPDATE";

        public const string SP_DELETE = "LinimasaKelas_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Tanggal = "Tanggal";
            public const string Jenis = "Jenis";
            public const string TahunAjaran = "TahunAjaran";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string Keterangan = "Keterangan";
            public const string TanggalUpdate = "TanggalUpdate";
            public const string RTG_UNIT = "RTG_UNIT";
            public const string RTG_LEVEL = "RTG_LEVEL";
            public const string RTG_SEMESTER = "RTG_SEMESTER";
            public const string RTG_KELAS = "RTG_KELAS";
            public const string RTG_SUBKELAS = "RTG_SUBKELAS";
            public const string ATTRIB_01 = "ATTRIB_01";
            public const string ATTRIB_02 = "ATTRIB_02";
            public const string ATTRIB_03 = "ATTRIB_03";
            public const string ATTRIB_04 = "ATTRIB_04";
            public const string ATTRIB_05 = "ATTRIB_05";
            public const string ATTRIB_06 = "ATTRIB_06";
            public const string ATTRIB_07 = "ATTRIB_07";
            public const string ATTRIB_08 = "ATTRIB_08";
            public const string ATTRIB_09 = "ATTRIB_09";
            public const string ATTRIB_10 = "ATTRIB_10";
            public const string ACT = "ACT";
            public const string ACT_KETERANGAN = "ACT_KETERANGAN";
        }

        private static LinimasaKelas GetEntityFromDataRow(DataRow row)
        {
            return new LinimasaKelas
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Tanggal = Convert.ToDateTime(row[NamaField.Tanggal]),
                Jenis = row[NamaField.Jenis].ToString(),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString(),
                Keterangan = row[NamaField.Keterangan].ToString(),
                TanggalUpdate = Convert.ToDateTime(row[NamaField.TanggalUpdate]),
                RTG_UNIT = row[NamaField.RTG_UNIT].ToString(),
                RTG_LEVEL = row[NamaField.RTG_LEVEL].ToString(),
                RTG_SEMESTER = row[NamaField.RTG_SEMESTER].ToString(),
                RTG_KELAS = row[NamaField.RTG_KELAS].ToString(),
                RTG_SUBKELAS = row[NamaField.RTG_SUBKELAS].ToString(),
                ATTRIB_01 = row[NamaField.ATTRIB_01].ToString(),
                ATTRIB_02 = row[NamaField.ATTRIB_02].ToString(),
                ATTRIB_03 = row[NamaField.ATTRIB_03].ToString(),
                ATTRIB_04 = row[NamaField.ATTRIB_04].ToString(),
                ATTRIB_05 = row[NamaField.ATTRIB_05].ToString(),
                ATTRIB_06 = row[NamaField.ATTRIB_06].ToString(),
                ATTRIB_07 = row[NamaField.ATTRIB_07].ToString(),
                ATTRIB_08 = row[NamaField.ATTRIB_08].ToString(),
                ATTRIB_09 = row[NamaField.ATTRIB_09].ToString(),
                ATTRIB_10 = row[NamaField.ATTRIB_10].ToString(),
                ACT = row[NamaField.ACT].ToString(),
                ACT_KETERANGAN = row[NamaField.ACT_KETERANGAN].ToString()
            };
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

        public static void Insert(LinimasaKelas m, string user_id)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                LinimasaKelas m_linimasa = DAO_LinimasaKelas.GetByID_Entity(m.Kode.ToString());
                bool ada_linimasa = false;
                //get lini masa && insert jika belum ada
                if (
                    m_linimasa != null
                )
                {
                    if (m_linimasa.Jenis != null)
                    {
                        ada_linimasa = true;
                    }
                }

                if (!ada_linimasa)
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();

                    comm.Parameters.Clear();
                    comm.Transaction = transaction;
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.CommandText = SP_INSERT;

                    Guid kode = m.Kode;
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, kode));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Tanggal, m.Tanggal));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Jenis, m.Jenis));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, m.Keterangan));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.ACT, m.ACT));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.ACT_KETERANGAN, m.ACT_KETERANGAN));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.RTG_UNIT, m.RTG_UNIT));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.RTG_LEVEL, m.RTG_LEVEL));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.RTG_KELAS, m.RTG_KELAS));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.RTG_SEMESTER, m.RTG_SEMESTER));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.RTG_SUBKELAS, m.RTG_SUBKELAS));
                    comm.Parameters.Add(new SqlParameter("@user_id", user_id));
                    comm.ExecuteNonQuery();

                    transaction.Commit();
                }
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

        public static void Update(LinimasaKelas m, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Jenis, m.Jenis));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, m.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.ACT_KETERANGAN, m.ACT_KETERANGAN));
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

        public static List<LinimasaKelas> GetAllByTanggalByJenisByTahunAjaranByKelasDet_Entity(
                DateTime tanggal,
                string jenis,
                string tahun_ajaran,
                string rel_kelasdet
            )
        {
            List<LinimasaKelas> hasil = new List<LinimasaKelas>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TANGGAL_BY_JENIS_BY_TAHUNAJARAN_BY_KELASDET;
                comm.Parameters.AddWithValue("@" + NamaField.Tanggal, tanggal);
                comm.Parameters.AddWithValue("@" + NamaField.Jenis, jenis);
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelasdet);

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

        public static List<LinimasaKelas> GetAllByTahunAjaranByKelasDet_Entity(
                string tahun_ajaran,
                string rel_kelasdet
            )
        {
            List<LinimasaKelas> hasil = new List<LinimasaKelas>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TAHUNAJARAN_BY_KELASDET;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelasdet);

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
        
        public static List<LinimasaKelas> GetAllByTahunAjaranByKelasDetByPeriodeByMapel_Entity(
                string tahun_ajaran,
                string rel_kelasdet,
                int bulan,
                int tahun,
                string rel_mapel
            )
        {
            List<LinimasaKelas> hasil = new List<LinimasaKelas>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TAHUNAJARAN_BY_KELASDET_BY_PERIODE_BY_MAPEL;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelasdet);
                comm.Parameters.AddWithValue("@Bulan", bulan);
                comm.Parameters.AddWithValue("@Tahun", tahun);
                comm.Parameters.AddWithValue("@Rel_Mapel", rel_mapel);

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

        public static List<LinimasaKelas> GetAllByTahunAjaranByKelasDetByPeriode_Entity(
                string tahun_ajaran,
                string rel_kelasdet,
                int bulan,
                int tahun
            )
        {
            List<LinimasaKelas> hasil = new List<LinimasaKelas>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TAHUNAJARAN_BY_KELASDET_BY_PERIODE;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelasdet);
                comm.Parameters.AddWithValue("@Bulan", bulan);
                comm.Parameters.AddWithValue("@Tahun", tahun);

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

        public static LinimasaKelas GetByID_Entity(
                string kode
            )
        {
            LinimasaKelas hasil = new LinimasaKelas();
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

        public static List<LinimasaKelas> GetAllByTanggalByJenisByTahunAjaranByKelasDetByKeterangan_Entity(
                DateTime tanggal,
                string jenis,
                string tahun_ajaran,
                string rel_kelasdet,
                string keterangan
            )
        {
            List<LinimasaKelas> hasil = new List<LinimasaKelas>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TANGGAL_BY_JENIS_BY_TAHUNAJARAN_BY_KELASDET_BY_KETERANGAN;
                comm.Parameters.AddWithValue("@" + NamaField.Tanggal, tanggal);
                comm.Parameters.AddWithValue("@" + NamaField.Jenis, jenis);
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelasdet);
                comm.Parameters.AddWithValue("@" + NamaField.Keterangan, keterangan);

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