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
    public static class DAO_FormasiGuruMapelDet
    {
        public const string SP_SELECT_ALL = "FormasiGuruMapelDet_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "FormasiGuruMapelDet_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "FormasiGuruMapelDet_SELECT_BY_ID";
        public const string SP_SELECT_BY_HEADER = "FormasiGuruMapelDet_SELECT_BY_HEADER";
        public const string SP_SELECT_BY_UNIT = "FormasiGuruMapelDet_SELECT_BY_UNIT";
        public const string SP_SELECT_BY_GURU_BY_TAHUN_AJARAN_BY_SEMESTER = "FormasiGuruMapelDet_SELECT_BY_GURU_BY_TAHUN_AJARAN_BY_SEMESTER";
        public const string SP_SELECT_BY_GURU_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELASDET_BY_MAPEL = "FormasiGuruMapelDet_SELECT_BY_GURU_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELASDET_BY_MAPEL";
        public const string SP_SELECT_BY_GURU_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELASDET_BY_MAPEL_AS_SELF = "FormasiGuruMapelDet_SELECT_BY_GURU_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELASDET_BY_MAPEL_AS_SELF";
        public const string SP_SELECT_BY_GURU_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELASDET_BY_MAPEL_AS_OTHER = "FormasiGuruMapelDet_SELECT_BY_GURU_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELASDET_BY_MAPEL_AS_OTHER";
        public const string SP_SELECT_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELASDET_BY_MAPEL = "FormasiGuruMapelDet_SELECT_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELASDET_BY_MAPEL";
        public const string SP_SELECT_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELAS_BY_MAPEL = "FormasiGuruMapelDet_SELECT_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELAS_BY_MAPEL";

        public const string SP_INSERT = "FormasiGuruMapelDet_INSERT";

        public const string SP_UPDATE = "FormasiGuruMapelDet_UPDATE";
        public const string SP_UPDATE_IS_SISWA_PILIHAN = "FormasiGuruMapelDet_UPDATE_IS_SISWA_PILIHAN";        

        public const string SP_DELETE = "FormasiGuruMapelDet_DELETE";
        public const string SP_DELETE_BY_HEADER = "FormasiGuruMapelDet_DELETE_BY_HEADER";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_FormasiGuruMapel = "Rel_FormasiGuruMapel";
            public const string Rel_Guru = "Rel_Guru";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string Keterangan = "Keterangan";
            public const string Urutan = "Urutan";
            public const string IsSiswaPilihan = "IsSiswaPilihan";

            public const string KelasDet = "KelasDet";
            public const string JenisKelas = "JenisKelas";
            public const string Guru = "Guru";
        }

        public class FormasiGuruMapelDet_Lengkap : FormasiGuruMapelDet
        {
            public string KelasDet { get; set; }
            public string JenisKelas { get; set; }
            public string Guru { get; set; }
        }

        public static FormasiGuruMapelDet GetEntityFromDataRow(DataRow row)
        {
            return new FormasiGuruMapelDet
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Guru = row[NamaField.Rel_Guru].ToString(),
                Rel_KelasDet = new Guid(row[NamaField.Rel_KelasDet].ToString()),
                Rel_FormasiGuruMapel = new Guid(row[NamaField.Rel_FormasiGuruMapel].ToString()),
                Keterangan = row[NamaField.Keterangan].ToString(),
                IsSiswaPilihan = (row[NamaField.IsSiswaPilihan] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsSiswaPilihan])),
                Urutan = Application_Libs.Libs.GetStringToInteger(row[NamaField.Urutan].ToString())
            };
        }

        public static FormasiGuruMapelDet_Lengkap GetEntityFromDataRow_Lengkap(DataRow row)
        {
            return new FormasiGuruMapelDet_Lengkap
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Guru = row[NamaField.Rel_Guru].ToString(),
                Rel_KelasDet = new Guid(row[NamaField.Rel_KelasDet].ToString()),
                Rel_FormasiGuruMapel = new Guid(
                        row[NamaField.Rel_FormasiGuruMapel].ToString().Trim() == ""
                        ? Application_Libs.Constantas.GUID_NOL
                        : row[NamaField.Rel_FormasiGuruMapel].ToString()
                    ),
                Keterangan = row[NamaField.Keterangan].ToString(),
                IsSiswaPilihan = (row[NamaField.IsSiswaPilihan] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsSiswaPilihan])),
                Urutan = Application_Libs.Libs.GetStringToInteger(row[NamaField.Urutan].ToString()),

                KelasDet = row[NamaField.KelasDet].ToString(),
                JenisKelas = row[NamaField.JenisKelas].ToString(),
                Guru = row[NamaField.Guru].ToString()
            };
        }

        public static List<FormasiGuruMapelDet> GetAll_Entity()
        {
            List<FormasiGuruMapelDet> hasil = new List<FormasiGuruMapelDet>();
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

        public static FormasiGuruMapelDet GetByID_Entity(string kode)
        {
            FormasiGuruMapelDet hasil = new FormasiGuruMapelDet();
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

        public static List<FormasiGuruMapelDet> GetByUnit_Entity(string rel_sekolah)
        {
            List<FormasiGuruMapelDet> hasil = new List<FormasiGuruMapelDet>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

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

        public static List<FormasiGuruMapelDet_Lengkap> GetByTABySMByKelasByMapel_Entity(string tahun_ajaran, string semester, string rel_kelas, string rel_mapel)
        {
            List<FormasiGuruMapelDet_Lengkap> hasil = new List<FormasiGuruMapelDet_Lengkap>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELAS_BY_MAPEL;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_Kelas", rel_kelas);
                comm.Parameters.AddWithValue("@Rel_Mapel", rel_mapel);

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

        public static List<FormasiGuruMapelDet> GetByGuruByTABySM_Entity(string rel_guru, string tahun_ajaran, string semester)
        {
            List<FormasiGuruMapelDet> hasil = new List<FormasiGuruMapelDet>();
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

        public static List<FormasiGuruMapelDet> GetByGuruByTABySMByKelasDetByMapel_Entity(string rel_guru, string tahun_ajaran, string semester, string rel_kelasdet, string rel_mapel)
        {
            List<FormasiGuruMapelDet> hasil = new List<FormasiGuruMapelDet>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_GURU_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELASDET_BY_MAPEL;
                comm.Parameters.AddWithValue("@Rel_Guru", rel_guru);
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_Mapel", rel_mapel);
                comm.Parameters.AddWithValue("@Rel_KelasDet", rel_kelasdet);

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

        public static List<FormasiGuruMapelDet> GetByGuruByTABySMByKelasDetByMapelAsSelf_Entity(string rel_guru, string tahun_ajaran, string semester, string rel_kelasdet, string rel_mapel)
        {
            List<FormasiGuruMapelDet> hasil = new List<FormasiGuruMapelDet>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_GURU_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELASDET_BY_MAPEL_AS_SELF;
                comm.Parameters.AddWithValue("@Rel_Guru", rel_guru);
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_Mapel", rel_mapel);
                comm.Parameters.AddWithValue("@Rel_KelasDet", rel_kelasdet);

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

        public static List<FormasiGuruMapelDet> GetByGuruByTABySMByKelasDetByMapelAsOther_Entity(string rel_guru, string tahun_ajaran, string semester, string rel_kelasdet, string rel_mapel)
        {
            List<FormasiGuruMapelDet> hasil = new List<FormasiGuruMapelDet>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_GURU_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELASDET_BY_MAPEL_AS_OTHER;
                comm.Parameters.AddWithValue("@Rel_Guru", rel_guru);
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_Mapel", rel_mapel);
                comm.Parameters.AddWithValue("@Rel_KelasDet", rel_kelasdet);

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

        public static List<FormasiGuruMapelDet> GetByTABySMByKelasDetByMapel_Entity(string rel_guru, string tahun_ajaran, string semester, string rel_kelasdet, string rel_mapel)
        {
            List<FormasiGuruMapelDet> hasil = new List<FormasiGuruMapelDet>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TAHUN_AJARAN_BY_SEMESTER_BY_KELASDET_BY_MAPEL;
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_Mapel", rel_mapel);
                comm.Parameters.AddWithValue("@Rel_KelasDet", rel_kelasdet);

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

        public static void DeleteByHeader(string Kode_header, string user_id)
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
                comm.CommandText = SP_DELETE_BY_HEADER;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_FormasiGuruMapel, Kode_header));
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

        public static void Insert(FormasiGuruMapelDet formasi_guru, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_FormasiGuruMapel, formasi_guru.Rel_FormasiGuruMapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Guru, formasi_guru.Rel_Guru));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, formasi_guru.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, formasi_guru.Keterangan));
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

        public static void Update(FormasiGuruMapelDet formasi_guru, string user_id)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_FormasiGuruMapel, formasi_guru.Rel_FormasiGuruMapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Guru, formasi_guru.Rel_Guru));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, formasi_guru.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Keterangan, formasi_guru.Keterangan));
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

        public static bool IsSiswaPilihanByGuru(string rel_guru, string tahun_ajaran, string semester, string rel_kelas_det, string rel_mapel)
        {
            List<FormasiGuruMapelDet> lst_formasi_ = DAO_FormasiGuruMapelDet.GetByTABySMByKelasDetByMapel_Entity(
                                                                rel_guru,
                                                                tahun_ajaran,
                                                                semester,
                                                                rel_kelas_det,
                                                                rel_mapel
                                                            );
            if (lst_formasi_.Count > 0)
            {
                var m_formasi = lst_formasi_.FirstOrDefault();
                if (m_formasi != null)
                {
                    if (m_formasi.Rel_Guru != null)
                    {
                        if (m_formasi.IsSiswaPilihan)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static void UpdateIsSiswaPilihan(string kode, bool is_siswa_pilihan)
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
                comm.CommandText = SP_UPDATE_IS_SISWA_PILIHAN;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsSiswaPilihan, is_siswa_pilihan));

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

        public static List<FormasiGuruMapelDet> GetByHeader_Entity(string kode)
        {
            List<FormasiGuruMapelDet> hasil = new List<FormasiGuruMapelDet>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_HEADER;
                comm.Parameters.AddWithValue("@Rel_FormasiGuruMapel", kode);

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