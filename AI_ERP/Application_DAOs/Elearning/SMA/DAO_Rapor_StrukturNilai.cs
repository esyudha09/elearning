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
    public static class DAO_Rapor_StrukturNilai
    {
        public const string SP_SELECT_ALL = "SMA_Rapor_StrukturNilai_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "SMA_Rapor_StrukturNilai_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_ALL_BY_TA_BY_SM = "SMA_Rapor_StrukturNilai_SELECT_ALL_BY_TA_BY_SM";
        public const string SP_SELECT_ALL_BY_TA_BY_SM_FOR_SEARCH = "SMA_Rapor_StrukturNilai_SELECT_ALL_BY_TA_BY_SM_FOR_SEARCH";

        public const string SP_SELECT_DISTINCT_TAHUN_AJARAN = "SMA_Rapor_StrukturNilai_SELECT_DISTINCT_TAHUN_AJARAN";
        public const string SP_SELECT_DISTINCT_TAHUN_AJARAN_PERIODE = "SMA_Rapor_StrukturNilai_SELECT_PERIODE";
        public const string SP_SELECT_DISTINCT_TAHUN_AJARAN_SEMESTER_BY_TA_BY_KELAS = "SMA_Rapor_StrukturNilai_SELECT_DISTINCT_TAHUN_AJARAN_SEMESTER_BY_TA_BY_KELAS";

        public const string SP_SELECT_BY_ID = "SMA_Rapor_StrukturNilai_SELECT_BY_ID";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELAS_BY_MAPEL = "SMA_Rapor_StrukturNilai_SELECT_BY_TA_BY_SM_BY_KELAS_BY_MAPEL";
        public const string SP_SELECT_BY_TA_BY_KELAS_BY_GURU_KELAS = "SMA_Rapor_StrukturNilai_SELECT_BY_TA_BY_KELAS_BY_GURU_KELAS";

        public const string SP_SELECT_SIKAP_BY_TA_BY_SM_BY_KELAS = "SMA_Rapor_StrukturNilai_SIKAP_BY_TA_BY_SM_BY_KELAS";

        public const string SP_SELECT_PREDIKAT_BY_HEADER = "SMA_Rapor_StrukturNilai_SELECT_PREDIKAT_BY_HEADER";

        public const string SP_SELECT_MAX_PERIODE = "SMA_Rapor_StrukturNilai_SELECT_MAX_PERIODE";

        public class StrukturNilai
        {
            public Guid Kode { get; set; }
            public string TahunAjaran { get; set; }
            public string Semester { get; set; }
            public string Rel_Kelas { get; set; }
            public Guid Rel_Mapel { get; set; }
            public string Kurikulum { get; set; }
            public decimal KKM { get; set; }
            public bool IsNilaiAkhir { get; set; }
            public string DeskripsiSikapSpiritual { get; set; }
            public string DeskripsiSikapSosial { get; set; }
        }

        public class TahunAjaranSemester
        {
            public string TahunAjaran { get; set; }
            public string Semester { get; set; }
        }
        public class StrukturNilaiPredikat
        {
            public Guid Kode { get; set; }
            public Guid Rel_Rapor_StrukturNilai { get; set; }
            public decimal Minimal { get; set; }
            public decimal Maksimal { get; set; }
            public string Predikat { get; set; }
            public int Urutan { get; set; }
            public string Deskripsi { get; set; }
        }
        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_Kelas = "Rel_Kelas";
            public const string Rel_Mapel = "Rel_Mapel";
            public const string Kurikulum = "Kurikulum";
            public const string KKM = "KKM";
            public const string IsNilaiAkhir = "IsNilaiAkhir";
            public const string DeskripsiSikapSpiritual = "DeskripsiSikapSpiritual";
            public const string DeskripsiSikapSosial = "DeskripsiSikapSosial";
        }

        public static class NamaFieldPredikat
        {
            public const string Kode = "Kode";
            public const string Rel_Rapor_StrukturNilai = "Rel_Rapor_StrukturNilai";
            public const string Minimal = "Minimal";
            public const string Maksimal = "Maksimal";
            public const string Predikat = "Predikat";
            public const string Urutan = "Urutan";
            public const string Deskripsi = "Deskripsi";
        }

        private static StrukturNilai GetEntityFromDataRow(DataRow row)
        {
            return new StrukturNilai
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                Rel_Kelas = row[NamaField.Rel_Kelas].ToString(),
                Rel_Mapel = new Guid(row[NamaField.Rel_Mapel].ToString()),
                Kurikulum = row[NamaField.Kurikulum].ToString(),
                KKM = Convert.ToDecimal(row[NamaField.KKM]),
                IsNilaiAkhir = (row[NamaField.IsNilaiAkhir] == DBNull.Value ? false : Convert.ToBoolean(row[NamaField.IsNilaiAkhir])),
                DeskripsiSikapSpiritual = row[NamaField.DeskripsiSikapSpiritual].ToString(),
                DeskripsiSikapSosial = row[NamaField.DeskripsiSikapSosial].ToString()
            };
        }

        private static StrukturNilaiPredikat GetEntityFromDataRowPredikat(DataRow row)
        {
            return new StrukturNilaiPredikat
            {
                Kode = new Guid(row[NamaFieldPredikat.Kode].ToString()),
                Rel_Rapor_StrukturNilai = new Guid(row[NamaFieldPredikat.Rel_Rapor_StrukturNilai].ToString()),
                Minimal = Convert.ToInt16(row[NamaFieldPredikat.Minimal]),
                Maksimal = Convert.ToInt16(row[NamaFieldPredikat.Maksimal]),
                Predikat = row[NamaFieldPredikat.Predikat].ToString(),
                Urutan = Convert.ToInt16(row[NamaFieldPredikat.Urutan]),
                Deskripsi = row[NamaFieldPredikat.Deskripsi].ToString()
            };
        }

        public static StrukturNilai GetByID_Entity(
                string kode
            )
        {
            StrukturNilai hasil = new StrukturNilai();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
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
                    hasil = (GetEntityFromDataRow(row));
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

        public static List<StrukturNilai> GetAllByTAByKelasByGuru_Entity(
                string tahun_ajaran,
                string rel_kelas,
                string rel_guru
            )
        {
            List<StrukturNilai> hasil = new List<StrukturNilai>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_KELAS_BY_GURU_KELAS;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas, rel_kelas);
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

        public static List<StrukturNilai> GetAllMapelSikapByTAByKelas_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelas
            )
        {
            List<StrukturNilai> hasil = new List<StrukturNilai>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_SIKAP_BY_TA_BY_SM_BY_KELAS;
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

        public static List<StrukturNilai> GetAllByTABySMByKelasByMapel_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelas,
                string rel_mapel
            )
        {
            List<StrukturNilai> hasil = new List<StrukturNilai>();
            if (rel_mapel == "") return hasil;

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

        public static List<StrukturNilaiPredikat> GetPredikatByHeader_Entity(
                string rel_rapor_strukturnilai
            )
        {
            List<StrukturNilaiPredikat> hasil = new List<StrukturNilaiPredikat>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_PREDIKAT_BY_HEADER;
                comm.Parameters.AddWithValue("@" + NamaFieldPredikat.Rel_Rapor_StrukturNilai, rel_rapor_strukturnilai);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(GetEntityFromDataRowPredikat(row));
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

        public static TahunAjaranSemester GetMaxPeriode_Entity()
        {
            TahunAjaranSemester hasil = new TahunAjaranSemester();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_MAX_PERIODE;

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil = (
                            new TahunAjaranSemester
                            {
                                TahunAjaran = row["TahunAjaran"].ToString(),
                                Semester = row["Semester"].ToString()
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

        public static List<TahunAjaranSemester> GetDistinctTahunAjaranPeriode_Entity()
        {
            List<TahunAjaranSemester> hasil = new List<TahunAjaranSemester>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_DISTINCT_TAHUN_AJARAN_PERIODE;

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(
                            new TahunAjaranSemester
                            {
                                TahunAjaran = row[0].ToString(),
                                Semester = row[1].ToString()
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

        public static List<TahunAjaranSemester> GetDistinctTahunAjaranSemester_Entity(
                string tahun_ajaran,
                string rel_kelas
            )
        {
            List<TahunAjaranSemester> hasil = new List<TahunAjaranSemester>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_DISTINCT_TAHUN_AJARAN_SEMESTER_BY_TA_BY_KELAS;
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_Kelas, rel_kelas);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(
                            new TahunAjaranSemester
                            {
                                TahunAjaran = row["TahunAjaran"].ToString(),
                                Semester = row["Semester"].ToString()
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

        public static List<string> GetDistinctTahunAjaran_Entity()
        {
            List<string> hasil = new List<string>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_DISTINCT_TAHUN_AJARAN;

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil.Add(
                            row[0].ToString()
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

        public static TahunAjaranSemester GetNextPeriode_Entity()
        {
            TahunAjaranSemester hasil = new TahunAjaranSemester();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_MAX_PERIODE;

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil = (
                            new TahunAjaranSemester
                            {
                                TahunAjaran = row["TahunAjaran"].ToString(),
                                Semester = row["Semester"].ToString()
                            }
                        );
                }

                if (hasil != null)
                {
                    if (hasil.TahunAjaran != null)
                    {
                        if (hasil.Semester.ToString() == "2")
                        {
                            hasil.Semester = "1";
                            hasil.TahunAjaran = Application_Libs.Libs.GetTahunAjaranPlus(hasil.TahunAjaran, 1);
                        }
                        else if (hasil.Semester.ToString() == "1")
                        {
                            hasil.Semester = "2";
                        }
                    }
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

        public static bool IsNilaiCanEdit(string kode, string rel_guru, string rel_kelas_det)
        {
            bool hasil = false;
            
            Rapor_StrukturNilai_KTSP m = DAO_Rapor_StrukturNilai_KTSP.GetByID_Entity(kode);
            if (m != null)
            {
                if (m.TahunAjaran != null)
                {
                    //by formasi guru mata pelajaran
                    var lst_guru_mapel = DAO_FormasiGuruMapelDet.GetByGuruByTABySMByKelasDetByMapelAsSelf_Entity(
                            rel_guru, m.TahunAjaran, m.Semester, rel_kelas_det, m.Rel_Mapel.ToString()
                        );

                    if (
                        (
                            lst_guru_mapel.Count > 0
                        )
                    ) return true;
                }
            }

            Rapor_StrukturNilai_KURTILAS m0 = DAO_Rapor_StrukturNilai_KURTILAS.GetByID_Entity(kode);
            if (m0 != null)
            {
                if (m0.TahunAjaran != null)
                {
                    //by formasi guru mata pelajaran
                    var lst_guru_mapel = DAO_FormasiGuruMapelDet.GetByGuruByTABySMByKelasDetByMapelAsSelf_Entity(
                            rel_guru, m0.TahunAjaran, m0.Semester, rel_kelas_det, m0.Rel_Mapel.ToString()
                        );

                    if (
                        (
                            lst_guru_mapel.Count > 0
                        )
                    ) return true;
                }
            }

            return hasil;
        }
    }
}