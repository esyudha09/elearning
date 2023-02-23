using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities.Elearning.TK;

namespace AI_ERP.Application_DAOs.Elearning.TK
{
    public static class DAO_Rapor_UploadHist
    {
        public static class Kelas
        {
            public const string SP_SELECT_BY_TA_BY_SM_BY_KELASDET = "TK_Rapor_UploadHistKelas_SELECT_BY_TA_BY_SM_BY_KELASDET";
            public const string SP_SELECT_BY_ID = "TK_Rapor_UploadHistKelas_SELECT_BY_ID";

            public const string SP_INSERT = "TK_Rapor_UploadHistKelas_INSERT";

            public const string SP_DELETE = "TK_Rapor_UploadHistKelas_DELETE";

            public static class NamaField
            {
                public const string Kode = "Kode";
                public const string Tanggal = "Tanggal";
                public const string PosisiUpload = "PosisiUpload";
                public const string TahunAjaran = "TahunAjaran";
                public const string Semester = "Semester";
                public const string Rel_KelasDet = "Rel_KelasDet";
                public const string TipeRapor = "TipeRapor";
            }

            private static Rapor_UploadHistKelas GetEntityFromDataRow(DataRow row)
            {
                return new Rapor_UploadHistKelas
                {
                    Kode = new Guid(row[NamaField.Kode].ToString()),
                    Tanggal = Convert.ToDateTime(row[NamaField.Tanggal]),
                    PosisiUpload = row[NamaField.PosisiUpload].ToString(),
                    TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                    Semester = row[NamaField.Semester].ToString(),
                    Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString()
                };
            }

            public static Rapor_UploadHistKelas GetByID_Entity(string kode)
            {
                Rapor_UploadHistKelas hasil = new Rapor_UploadHistKelas();
                SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
                SqlCommand comm = conn.CreateCommand();
                SqlDataAdapter sqlDA;

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

            public static List<Rapor_UploadHistKelas> GetByTABySMByKelasDet_Entity(string tahun_ajaran, string semester, string rel_kelasdet)
            {
                List<Rapor_UploadHistKelas> hasil = new List<Rapor_UploadHistKelas>();
                SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
                SqlCommand comm = conn.CreateCommand();
                SqlDataAdapter sqlDA;

                try
                {
                    conn.Open();
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELASDET;
                    comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                    comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
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

            public static void Insert(Rapor_UploadHistKelas m)
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
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Tanggal, m.Tanggal));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.PosisiUpload, m.PosisiUpload));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TipeRapor, m.TipeRapor));
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

        public static class Siswa
        {
            public const string SP_SELECT_BY_TA_BY_SM_BY_KELASDET_BY_SISWA = "TK_Rapor_UploadHistSiswa_SELECT_BY_TA_BY_SM_BY_KELASDET_BY_SISWA";
            public const string SP_SELECT_BY_ID = "TK_Rapor_UploadHistSiswa_SELECT_BY_ID";
            public const string SP_SELECT_UPLOAD_LOG_BY_TA_BY_SM_BY_KELASDET = "TK_Rapor_UploadHistKelas_SELECT_UPLOAD_LOG_BY_TA_BY_SM_BY_KELASDET";

            public const string SP_INSERT = "TK_Rapor_UploadHistSiswa_INSERT";

            public const string SP_DELETE = "TK_Rapor_UploadHistSiswa_DELETE";

            public static class NamaField
            {
                public const string Kode = "Kode";
                public const string Tanggal = "Tanggal";
                public const string PosisiUpload = "PosisiUpload";
                public const string TahunAjaran = "TahunAjaran";
                public const string Semester = "Semester";
                public const string Rel_KelasDet = "Rel_KelasDet";
                public const string TipeRapor = "TipeRapor";
                public const string Rel_Siswa = "Rel_Siswa";
            }

            private static Rapor_UploadHistSiswa GetEntityFromDataRow(DataRow row)
            {
                return new Rapor_UploadHistSiswa
                {
                    Kode = new Guid(row[NamaField.Kode].ToString()),
                    Tanggal = Convert.ToDateTime(row[NamaField.Tanggal]),
                    PosisiUpload = row[NamaField.PosisiUpload].ToString(),
                    TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                    Semester = row[NamaField.Semester].ToString(),
                    Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString(),
                    Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                    TipeRapor = row[NamaField.TipeRapor].ToString()
                };
            }

            public static Rapor_UploadHistSiswa GetByID_Entity(string kode)
            {
                Rapor_UploadHistSiswa hasil = new Rapor_UploadHistSiswa();
                SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
                SqlCommand comm = conn.CreateCommand();
                SqlDataAdapter sqlDA;

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

            public static List<Rapor_UploadHistSiswa> GetByTABySMByKelasDetBySiswa_Entity(string tahun_ajaran, string semester, string rel_kelasdet, string rel_siswa)
            {
                List<Rapor_UploadHistSiswa> hasil = new List<Rapor_UploadHistSiswa>();
                SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
                SqlCommand comm = conn.CreateCommand();
                SqlDataAdapter sqlDA;

                try
                {
                    conn.Open();
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELASDET_BY_SISWA;
                    comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                    comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelasdet);
                    comm.Parameters.AddWithValue("@" + NamaField.Rel_Siswa, rel_siswa);

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

            public static string GetStatusUpload(string tahun_ajaran, string semester, string rel_kelasdet, string rel_siswa, string tipe_rapor)
            {
                string hasil = "";

                var lst_log_kelas = GetLogByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelasdet).FindAll(m0 => m0.Rel_Siswa.Trim() == "" && m0.TipeRapor.ToUpper().Trim() == tipe_rapor.ToUpper().Trim());
                var lst_log_siswa = GetLogByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelasdet).FindAll(m0 => m0.Rel_Siswa.Trim().ToUpper() == rel_siswa.Trim().ToUpper() && m0.TipeRapor.ToUpper().Trim() == tipe_rapor.ToUpper().Trim());

                var m_log_kelas = lst_log_kelas.FirstOrDefault();
                var m_log_siswa = lst_log_siswa.FirstOrDefault();

                Rapor_UploadHistSiswa m_log = null;

                if (m_log_kelas != null && m_log_siswa != null)
                {
                    if (m_log_kelas.Rel_KelasDet != null && m_log_siswa.Rel_Siswa != null)
                    {
                        if (m_log_kelas.Tanggal > m_log_siswa.Tanggal)
                        {
                            m_log = m_log_kelas;
                        }
                        else if (m_log_kelas.Tanggal < m_log_siswa.Tanggal)
                        {
                            m_log = m_log_siswa;
                        }
                    }
                }
                else if (m_log_kelas != null && m_log_siswa == null)
                {
                    if (m_log_kelas.Rel_KelasDet != null)
                    {
                        m_log = m_log_kelas;
                    }
                }
                else if (m_log_siswa != null && m_log_kelas == null)
                {
                    if (m_log_siswa.Rel_KelasDet != null)
                    {
                        m_log = m_log_siswa;
                    }
                }

                if (m_log != null)
                {
                    if (m_log.TahunAjaran != null)
                    {
                        return m_log.PosisiUpload;
                    }
                }

                return hasil;

                //string hasil = "";

                //var lst_log = GetLogByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelasdet)
                //            .FindAll(
                //                m0 => m0.TipeRapor.Trim().ToUpper() == tipe_rapor.Trim().ToUpper()
                //            );

                //if (lst_log.Count > 0)
                //{
                //    var m_log = lst_log.FirstOrDefault();
                //    if (m_log != null)
                //    {
                //        if (m_log.TahunAjaran != null)
                //        {
                //            if (m_log.Rel_Siswa == "")
                //            {
                //                return m_log.PosisiUpload;
                //            }
                //            else
                //            {
                //                lst_log = lst_log.FindAll(m => m.Rel_Siswa.ToUpper().Trim() == rel_siswa.ToUpper().Trim()).ToList();
                //                if (lst_log.Count > 0)
                //                {
                //                    m_log = lst_log.FirstOrDefault();
                //                    if (m_log != null)
                //                    {
                //                        if (m_log.TahunAjaran != null)
                //                        {
                //                            return m_log.PosisiUpload;
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}

                //return hasil;
            }

            public static List<Rapor_UploadHistSiswa> GetLogByTABySMByKelasDet_Entity(string tahun_ajaran, string semester, string rel_kelasdet)
            {
                List<Rapor_UploadHistSiswa> hasil = new List<Rapor_UploadHistSiswa>();
                SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
                SqlCommand comm = conn.CreateCommand();
                SqlDataAdapter sqlDA;

                try
                {
                    conn.Open();
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.CommandText = SP_SELECT_UPLOAD_LOG_BY_TA_BY_SM_BY_KELASDET;
                    comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                    comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
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

            public static void Insert(Rapor_UploadHistSiswa m)
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
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Tanggal, m.Tanggal));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.PosisiUpload, m.PosisiUpload));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Siswa, m.Rel_Siswa));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.TipeRapor, m.TipeRapor));
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
}