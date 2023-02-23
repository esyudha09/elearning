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
    public static class DAO_Kelas
    {
        public const string SP_SELECT_ALL = "Kelas_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "Kelas_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "Kelas_SELECT_BY_ID";
        public const string SP_SELECT_BY_UNIT = "Kelas_SELECT_BY_SEKOLAH";

        public const string SP_INSERT = "Kelas_INSERT";

        public const string SP_UPDATE = "Kelas_UPDATE";

        public const string SP_DELETE = "Kelas_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Sekolah = "Rel_Sekolah";
            public const string Nama = "Nama";
            public const string UrutanLevel = "UrutanLevel";
            public const string Keterangan = "Keterangan";
            public const string IsAktif = "IsAktif";
        }

        public static Kelas GetEntityFromDataRow(DataRow row)
        {
            return new Kelas
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Sekolah = new Guid(row[NamaField.Rel_Sekolah].ToString()),
                Nama = row[NamaField.Nama].ToString(),
                UrutanLevel = Convert.ToInt16(row[NamaField.UrutanLevel]),
                Keterangan = row[NamaField.Keterangan].ToString(),
                IsAktif = (row[NamaField.IsAktif] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsAktif]))
            };
        }

        public static List<Kelas> GetAll_Entity()
        {
            List<Kelas> hasil = new List<Kelas>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
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

        public static List<Kelas> GetAllByUnit_Entity(string rel_sekolah)
        {
            List<Kelas> hasil = new List<Kelas>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (rel_sekolah == null || rel_sekolah.Trim() == "") return hasil;
            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_UNIT;
                comm.Parameters.AddWithValue("@Rel_Sekolah", rel_sekolah);

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

        public static Kelas GetKelasNext(string rel_sekolah, string kode)
        {
            Kelas kelas = new Kelas();
            List<Kelas> lst_kelas = DAO_Kelas.GetAll_Entity().FindAll(m => m.Rel_Sekolah == new Guid(rel_sekolah)).OrderBy(m => m.UrutanLevel).ToList();
            bool ada_kelas = false;
            foreach (Kelas item in lst_kelas)
            {
                if (ada_kelas && !Application_Libs.Libs.IsAngka(item.Nama)) return item;
                if (item.Kode.ToString().Trim().ToLower() == kode.Trim().ToLower())
                {
                    ada_kelas = true;
                }
            }

            return kelas;
        }

        public static Kelas GetByID_Entity(string kode)
        {
            Kelas hasil = new Kelas();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (kode == null) return hasil;
            if (kode.Trim() == "") return hasil;
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

        public static void Insert(Kelas kelas, List<KelasDet> lst_kelas_det)
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

                Guid kode = Guid.NewGuid();

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, kelas.Rel_Sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, kelas.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.UrutanLevel, kelas.UrutanLevel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, kelas.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsAktif, kelas.IsAktif));
                comm.ExecuteNonQuery();

                comm.CommandText = DAO_KelasDet.SP_DELETE_BY_KELAS;
                comm.Parameters.Clear();
                comm.Parameters.Add(new SqlParameter("@" + DAO_KelasDet.NamaField.Rel_Kelas, kode));
                comm.ExecuteNonQuery();

                int id_urut = 1;
                foreach (KelasDet item in lst_kelas_det)
                {
                    comm.Transaction = transaction;
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.CommandText = DAO_KelasDet.SP_INSERT;
                    comm.Parameters.Clear();

                    comm.Parameters.Add(new SqlParameter("@" + DAO_KelasDet.NamaField.Kode, item.Kode));
                    comm.Parameters.Add(new SqlParameter("@" + DAO_KelasDet.NamaField.Rel_Kelas, kode));
                    comm.Parameters.Add(new SqlParameter("@" + DAO_KelasDet.NamaField.Nama, item.Nama));
                    comm.Parameters.Add(new SqlParameter("@" + DAO_KelasDet.NamaField.UrutanKelas, item.UrutanKelas));
                    comm.Parameters.Add(new SqlParameter("@" + DAO_KelasDet.NamaField.Keterangan, item.Keterangan));
                    comm.Parameters.Add(new SqlParameter("@" + DAO_KelasDet.NamaField.IsKelasJurusan, item.IsKelasJurusan));
                    comm.Parameters.Add(new SqlParameter("@" + DAO_KelasDet.NamaField.IsKelasSosialisasi, item.IsKelasSosialisasi));
                    comm.Parameters.Add(new SqlParameter("@" + DAO_KelasDet.NamaField.IsAktif, item.IsAktif));
                    comm.ExecuteNonQuery();

                    id_urut++;
                }

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

        public static void Update(Kelas kelas, List<KelasDet> lst_kelas_det)
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

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, kelas.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Sekolah, kelas.Rel_Sekolah));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, kelas.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.UrutanLevel, kelas.UrutanLevel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, kelas.Keterangan));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsAktif, kelas.IsAktif));
                comm.ExecuteNonQuery();

                comm.CommandText = DAO_KelasDet.SP_DELETE_BY_KELAS;
                comm.Parameters.Clear();
                comm.Parameters.Add(new SqlParameter("@" + DAO_KelasDet.NamaField.Rel_Kelas, kelas.Kode));
                comm.ExecuteNonQuery();

                int id_urut = 1;
                foreach (KelasDet item in lst_kelas_det)
                {
                    comm.Transaction = transaction;
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.CommandText = DAO_KelasDet.SP_INSERT;
                    comm.Parameters.Clear();

                    comm.Parameters.Add(new SqlParameter("@" + DAO_KelasDet.NamaField.Kode, item.Kode));
                    comm.Parameters.Add(new SqlParameter("@" + DAO_KelasDet.NamaField.Rel_Kelas, kelas.Kode));
                    comm.Parameters.Add(new SqlParameter("@" + DAO_KelasDet.NamaField.Nama, item.Nama));
                    comm.Parameters.Add(new SqlParameter("@" + DAO_KelasDet.NamaField.UrutanKelas, item.UrutanKelas));
                    comm.Parameters.Add(new SqlParameter("@" + DAO_KelasDet.NamaField.Keterangan, item.Keterangan));
                    comm.Parameters.Add(new SqlParameter("@" + DAO_KelasDet.NamaField.IsKelasJurusan, item.IsKelasJurusan));
                    comm.Parameters.Add(new SqlParameter("@" + DAO_KelasDet.NamaField.IsKelasSosialisasi, item.IsKelasSosialisasi));
                    comm.Parameters.Add(new SqlParameter("@" + DAO_KelasDet.NamaField.IsAktif, item.IsAktif));
                    comm.ExecuteNonQuery();

                    id_urut++;
                }

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