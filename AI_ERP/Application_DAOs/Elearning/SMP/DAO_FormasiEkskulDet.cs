using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities;
using AI_ERP.Application_Entities.Elearning.SMP;

namespace AI_ERP.Application_DAOs.Elearning.SMP
{
    public static class DAO_FormasiEkskulDet
    {
        public const string SP_SELECT_ALL = "SMP_FormasiEkskulDet_SELECT_ALL";
        public const string SP_SELECT_BY_ID = "SMP_FormasiEkskulDet_SELECT_BY_ID";
        public const string SP_SELECT_BY_HEADER = "SMP_FormasiEkskulDet_SELECT_BY_HEADER";
        public const string SP_SELECT_BY_GURU_BY_TAHUN_AJARAN_BY_SEMESTER = "SMP_FormasiEkskulDet_SELECT_BY_GURU_BY_TAHUN_AJARAN_BY_SEMESTER";
        public const string SP_SELECT_BY_MAPEL_BY_TAHUN_AJARAN_BY_SEMESTER = "SMP_FormasiEkskulDet_SELECT_BY_MAPEL_BY_TAHUN_AJARAN_BY_SEMESTER";
        
        public const string SP_INSERT = "SMP_FormasiEkskulDet_INSERT";

        public const string SP_UPDATE = "SMP_FormasiEkskulDet_UPDATE";

        public const string SP_DELETE = "SMP_FormasiEkskulDet_DELETE";
        public const string SP_DELETE_BY_HEADER = "SMP_FormasiEkskulDet_DELETE_BY_HEADER";

        public class AbsenEkskulLTS
        {
            public string TahunAjaran { get; set; }
            public string Semester { get; set; }
            public string Rel_Mapel { get; set; }
            public string Mapel { get; set; }
            public string Rel_Siswa { get; set; }
            public string LTS_HD { get; set; }
        }

        public class NilaiEkskul
        {
            public string TahunAjaran { get; set; }
            public string Semester { get; set; }
            public string Rel_Mapel { get; set; }
            public string Mapel { get; set; }
            public string Rel_Siswa { get; set; }
            public string Rel_Nilai { get; set; }
            public string Nilai { get; set; }
            public string Deskripsi { get; set; }
            public bool IsNilaiAkhir { get; set; }
            public int Urutan { get; set; }
        }

        public static class NamaFieldAbsenEkskulLTS
        {
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_Mapel = "Rel_Mapel";
            public const string Mapel = "Mapel";
            public const string Rel_Siswa = "Rel_Siswa";
            public const string LTS_HD = "LTS_HD";
        }

        public static class NamaFieldNilaiEkskul
        {
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_Mapel = "Rel_Mapel";
            public const string Mapel = "Mapel";
            public const string Rel_Siswa = "Rel_Siswa";
            public const string Rel_Nilai = "Rel_Nilai";
            public const string Nilai = "Nilai";
            public const string Deskripsi = "Deskripsi";
            public const string IsNilaiAkhir = "IsNilaiAkhir";
            public const string Urutan = "Urutan";
        }

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_FormasiEkskul = "Rel_FormasiEkskul";
            public const string Rel_Siswa = "Rel_Siswa";
            public const string Keterangan = "Keterangan";
            public const string Urutan = "Urutan";
        }

        public static List<Siswa> GetListSiswaEkskul(string tahun_ajaran, string semester, string rel_mapel)
        {
            List<Siswa> lst_hasil = new List<Siswa>();

            List<FormasiEkskulDet> lst_formasil_ekskul = DAO_FormasiEkskulDet.GetByGuruByMapelBySM_Entity(rel_mapel, tahun_ajaran, semester);
            foreach (FormasiEkskulDet item in lst_formasil_ekskul)
            {
                lst_hasil.Add(DAO_Siswa.GetByKode_Entity(tahun_ajaran, semester, item.Rel_Siswa));
            }

            return lst_hasil;
        }

        public static FormasiEkskulDet GetEntityFromDataRow(DataRow row)
        {
            return new FormasiEkskulDet
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                Rel_FormasiEkskul = row[NamaField.Rel_FormasiEkskul].ToString(),
                Keterangan = row[NamaField.Keterangan].ToString(),
                Urutan = Application_Libs.Libs.GetStringToInteger(row[NamaField.Urutan].ToString())
            };
        }

        private static AbsenEkskulLTS GetEntityFromDataRowAbsenEkskulLTS(DataRow row)
        {
            return new AbsenEkskulLTS
            {
                TahunAjaran = row[NamaFieldAbsenEkskulLTS.TahunAjaran].ToString(),
                Semester = row[NamaFieldAbsenEkskulLTS.Semester].ToString(),
                Rel_Mapel = row[NamaFieldAbsenEkskulLTS.Rel_Mapel].ToString(),
                Mapel = row[NamaFieldAbsenEkskulLTS.Mapel].ToString(),
                Rel_Siswa = row[NamaFieldAbsenEkskulLTS.Rel_Siswa].ToString(),
                LTS_HD = row[NamaFieldAbsenEkskulLTS.LTS_HD].ToString()
            };
        }

        private static NilaiEkskul GetEntityFromDataRowNilaiEkskul(DataRow row)
        {
            return new NilaiEkskul
            {
                TahunAjaran = row[NamaFieldNilaiEkskul.TahunAjaran].ToString(),
                Semester = row[NamaFieldNilaiEkskul.Semester].ToString(),
                Rel_Mapel = row[NamaFieldNilaiEkskul.Rel_Mapel].ToString(),
                Mapel = row[NamaFieldNilaiEkskul.Mapel].ToString(),
                Rel_Siswa = row[NamaFieldNilaiEkskul.Rel_Siswa].ToString(),
                Rel_Nilai = row[NamaFieldNilaiEkskul.Rel_Nilai].ToString(),
                Nilai = row[NamaFieldNilaiEkskul.Nilai].ToString(),
                Deskripsi = row[NamaFieldNilaiEkskul.Deskripsi].ToString(),
                IsNilaiAkhir = (
                        row[NamaFieldNilaiEkskul.IsNilaiAkhir] == DBNull.Value
                        ? false
                        : Convert.ToBoolean(row[NamaFieldNilaiEkskul.IsNilaiAkhir])
                    ),
                Urutan = Application_Libs.Libs.GetStringToInteger(row[NamaFieldNilaiEkskul.Urutan].ToString())
            };
        }
        
        public static List<FormasiEkskulDet> GetAll_Entity()
        {
            List<FormasiEkskulDet> hasil = new List<FormasiEkskulDet>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
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

        public static FormasiEkskulDet GetByID_Entity(string kode)
        {
            FormasiEkskulDet hasil = new FormasiEkskulDet();
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

        public static List<FormasiEkskulDet> GetByGuruByTABySM_Entity(string rel_guru, string tahun_ajaran, string semester)
        {
            List<FormasiEkskulDet> hasil = new List<FormasiEkskulDet>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
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

        public static List<FormasiEkskulDet> GetByGuruByMapelBySM_Entity(string rel_mapel, string tahun_ajaran, string semester)
        {
            List<FormasiEkskulDet> hasil = new List<FormasiEkskulDet>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_MAPEL_BY_TAHUN_AJARAN_BY_SEMESTER;
                comm.Parameters.AddWithValue("@Rel_Mapel", rel_mapel);
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

        public static void DeleteByHeader(string Kode_header, string user_id)
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
                comm.CommandText = SP_DELETE_BY_HEADER;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_FormasiEkskul, Kode_header));
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

        public static void Insert(FormasiEkskulDet m)
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
                comm.CommandText = SP_INSERT;

                if (m.Kode.ToString() == Application_Libs.Constantas.GUID_NOL) m.Kode = Guid.NewGuid();

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_FormasiEkskul, m.Rel_FormasiEkskul));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, m.Keterangan));

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

        public static void Update(FormasiEkskulDet m)
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
                comm.CommandText = SP_UPDATE;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, m.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_FormasiEkskul, m.Rel_FormasiEkskul));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, m.Keterangan));

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

        public static List<FormasiEkskulDet> GetByHeader_Entity(string kode)
        {
            List<FormasiEkskulDet> hasil = new List<FormasiEkskulDet>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER;
                comm.Parameters.AddWithValue("@Rel_FormasiEkskul", kode);

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