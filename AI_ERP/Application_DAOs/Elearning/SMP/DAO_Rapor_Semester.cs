using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_Entities.Elearning;
using AI_ERP.Application_Entities.Elearning.SMP.Reports;
using AI_ERP.Application_Entities.Elearning.SMP;

namespace AI_ERP.Application_DAOs.Elearning.SMP
{
    public static class DAO_Rapor_Semester
    {
        public const decimal KKM_GLOBAL = 70;

        public const string SP_SELECT_ALL = "SD_Rapor_Semester_SELECT_ALL";
        public const string SP_SELECT_ALL_FOR_SEARCH = "SD_Rapor_Semester_SELECT_ALL_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "SD_Rapor_Semester_SELECT_BY_ID";
        public const string SP_SELECT_BY_TA_BY_SM_BY_KELAS_DET = "SD_Rapor_Semester_SELECT_BY_TA_BY_SM_BY_KELAS_DET";

        public const string SP_INSERT = "SD_Rapor_Semester_INSERT";

        public const string SP_UPDATE = "SD_Rapor_Semester_UPDATE";

        public const string SP_DELETE = "SD_Rapor_Semester_DELETE";

        public const string FONT_SIZE = "@fontsize";

        //public static List<RaporLTSCapaianKedisiplinan> ListRaporLTSCapaianKedisiplinan = new List<RaporLTSCapaianKedisiplinan>();

        public static class Constantas_Kode_Mapel
        {
            public const string KODE_BIOLOGI = "637B6FA0-26C4-494B-A1C4-00F579047006";
            public const string KODE_FISIKA = "63A76A21-1FC5-428E-9927-2E696A72ABB2";
        }

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string TahunAjaran = "TahunAjaran";
            public const string Semester = "Semester";
            public const string Rel_KelasDet = "Rel_KelasDet";
            public const string IsLocked = "IsLocked";
            public const string GuruKelas = "GuruKelas";
            public const string Kurikulum = "Kurikulum";
            public const string Tanggal = "Tanggal";
        }

        public class FormatNilai
        {
            public string Rel_StrukturNilai_AP { get; set; }
            public string Rel_KP { get; set; }
        }

        public class FormatNilaiWithNilai : FormatNilai
        {
            public string Nama_KP { get; set; }
            public string Nilai { get; set; }
        }

        public class NilaiWithKey
        {
            public string KodeMapel { get; set; }
            public string Key { get; set; }
            public decimal Nilai { get; set; }
            public string JenisMapel { get; set; }
        }

        private static Rapor_Semester GetEntityFromDataRow(DataRow row)
        {
            return new Rapor_Semester
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                Semester = row[NamaField.Semester].ToString(),
                IsLocked = Convert.ToBoolean((row[NamaField.IsLocked] == DBNull.Value ? false : row[NamaField.IsLocked])),
                GuruKelas = row[NamaField.GuruKelas].ToString(),
                Kurikulum = row[NamaField.Kurikulum].ToString(),
                Tanggal = (row[NamaField.Tanggal] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row[NamaField.Tanggal]))
            };
        }

        public static Sekolah GetUnitSekolah()
        {
            Sekolah m_unit = DAO_Sekolah.GetAll_Entity().FindAll(m => m.UrutanJenjang == (int)Libs.UnitSekolah.SMP).FirstOrDefault();
            return m_unit;
        }

        public static string GetPredikatDeskripsi(string s_nilai)
        {
            return "";

            if (s_nilai.Trim() == "") return ", (-)";

            decimal nilai = Libs.GetStringToDecimal(s_nilai);

            if (nilai < 70)
            {
                return ", kurang (" + s_nilai + ")";
            }
            else if (nilai >= 70 && nilai < 80)
            {
                return ", cukup (" + s_nilai + ")";
            }
            else if (nilai >= 80 && nilai < 90)
            {
                return ", baik (" + s_nilai + ")";
            }
            else if (nilai >= 90 && nilai <= 100)
            {
                return ", sangat baik (" + s_nilai + ")";
            }

            return "";
        }

        public static string GetPredikatRapor_OLD(decimal nilai)
        {
            if (nilai >= 86) return "A";
            if (nilai >= 70 && nilai <= 85) return "B";
            if (nilai < 70) return "C";
            return "";
        }

        public static string GetPredikatRapor(decimal nilai, string tahun_ajaran)
        {
            if (Libs.GetStringToDecimal(tahun_ajaran.Replace("/", "")) < 20202021)
            {
                return GetPredikatRapor_OLD(nilai);
            }

            if (nilai >= 90 && nilai <= 100) return "A";
            if (nilai >= 80 && nilai < 90) return "B";
            if (nilai >= 70 && nilai < 80) return "C";
            if (nilai < 70) return "D";
            return "";
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

        public static void Insert(Rapor_Semester m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsLocked, m.IsLocked));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.GuruKelas, m.GuruKelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kurikulum, m.Kurikulum));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Tanggal, m.Tanggal));
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

        public static void Update(Rapor_Semester m)
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
                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, m.TahunAjaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, m.Semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, m.Rel_KelasDet));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.IsLocked, m.IsLocked));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.GuruKelas, m.GuruKelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kurikulum, m.Kurikulum));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Tanggal, m.Tanggal));
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

        public static decimal GetSikapSosial(
                string tahun_ajaran,
                string semester,
                string rel_kelasdet,
                string rel_siswa
            )
        {
            decimal hasil = 0;

            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();

                comm.Parameters.Clear();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SMP_Rapor_NilaiSiswa_GET_SIKAP_SOSIAL";

                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, tahun_ajaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, rel_kelasdet));
                comm.Parameters.Add(new SqlParameter("@Rel_Siswa", rel_siswa));

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil = Convert.ToDecimal((row[0] == DBNull.Value ? 0 : row[0]));
                }

                return hasil;
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

        public static decimal GetSikapSpiritual(
                string tahun_ajaran,
                string semester,
                string rel_kelasdet,
                string rel_siswa
            )
        {
            decimal hasil = 0;

            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();

                comm.Parameters.Clear();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SMP_Rapor_NilaiSiswa_GET_SIKAP_SPIRITUAL";

                comm.Parameters.Add(new SqlParameter("@" + NamaField.TahunAjaran, tahun_ajaran));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Semester, semester));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_KelasDet, rel_kelasdet));
                comm.Parameters.Add(new SqlParameter("@Rel_Siswa", rel_siswa));

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil = Convert.ToDecimal(row[0]);
                }

                return hasil;
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
        
        public static List<Rapor_Semester> GetAllByTABySMByKelasDet_Entity(
                string tahun_ajaran,
                string semester,
                string rel_kelasdet
            )
        {
            List<Rapor_Semester> hasil = new List<Rapor_Semester>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_TA_BY_SM_BY_KELAS_DET;
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

        public static List<KTSP_RaporNilai> GetNilaiRapor_KTSP(
            string tahun_ajaran,
            string semester,
            string rel_kelas_det,
            string halaman,
            string rel_siswa,
            string s_lokasi_ttd
            )
        {
            System.Drawing.Image img = null;
            string s_loc = s_lokasi_ttd;
            if (File.Exists(s_loc) && s_loc.Trim() != "")
            {
                img = System.Drawing.Image.FromFile(s_loc);
            }
            byte[] img_ttd_guru = (byte[])(new ImageConverter()).ConvertTo(img, typeof(byte[]));

            List<KTSP_RaporNilai> hasil = new List<KTSP_RaporNilai>();
            string s_walikelas = "";
            List<FormasiGuruKelas> lst_formasi_guru_kelas = DAO_FormasiGuruKelas.GetByUnitByTABySM_Entity(
                        GetUnitSekolah().Kode.ToString(), tahun_ajaran, semester
                    ).FindAll(m => m.Rel_KelasDet.ToString().ToUpper() == rel_kelas_det.Trim().ToUpper());
            if (lst_formasi_guru_kelas != null)
            {
                if (lst_formasi_guru_kelas.Count > 0)
                {
                    FormasiGuruKelas m_guru_kelas = lst_formasi_guru_kelas.FirstOrDefault();
                    if (m_guru_kelas != null)
                    {
                        if (m_guru_kelas.TahunAjaran != null)
                        {
                            Pegawai m_pegawai = DAO_Pegawai.GetByID_Entity(m_guru_kelas.Rel_GuruKelas);
                            if (m_pegawai != null)
                            {
                                if (m_pegawai.Nama != null)
                                {
                                    s_walikelas = m_pegawai.Nama;
                                }
                            }
                        }
                    }
                }
            }

            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {

                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());
                    if (m_kelas != null)
                    {
                        if (m_kelas.Nama != null)
                        {

                            Rapor_Desain rapor_desain = DAO_Rapor_Desain.GetByTABySMByKelas_Entity(
                                    tahun_ajaran, semester, rel_kelas_det, DAO_Rapor_Desain.JenisRapor.Semester
                                ).FirstOrDefault();
                            List<Rapor_Desain_Det> lst_rapor_desain_det = DAO_Rapor_Desain_Det.GetAllByHeader_Entity(rapor_desain.Kode.ToString());

                            if (rapor_desain != null)
                            {

                                List<DAO_Siswa.SiswaDataSimple> lst_siswa = DAO_Siswa.GetAllSiswaDataSimpleByTahunAjaranUnitKelas_Entity(
                                        m_kelas.Rel_Sekolah.ToString(),
                                        rel_kelas_det,
                                        tahun_ajaran,
                                        semester
                                    );
                                List<NilaiWithKey> lst_nilai_ap = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_nilai_kd = new List<NilaiWithKey>();
                                List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT> lst_nilai_siswa_det = new List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT>();
                                List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT> lst_nilai_siswa_det_by_periode = new List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT>();

                                lst_nilai_siswa_det_by_periode = DAO_Rapor_NilaiSiswa_Det.GetAllByTABySMByKelasDet_Entity(
                                        tahun_ajaran, semester, rel_kelas_det
                                    );

                                int nomor = 1;
                                if (rel_siswa.Trim() != "") lst_siswa = lst_siswa.FindAll(m => rel_siswa.Trim().ToUpper().IndexOf(m.Kode.ToString().ToUpper() + ";") >= 0);
                                List<NilaiWithKey> lst_all_nilai_rapor = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_kkm_nilai_rapor = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_jumlah_nilai_keseluruhan = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_nilai_rapor = new List<NilaiWithKey>();
                                decimal jumlah_keseluruhan = 0;
                                decimal jumlah_mapel_rapor = 0;

                                Rapor_Arsip m_rapor_arsip = DAO_Rapor_Arsip.GetAll_Entity().FindAll(
                                            m0 => m0.TahunAjaran == tahun_ajaran &&
                                                  m0.Semester == semester &&
                                                  m0.JenisRapor == "Semester"

                                        ).FirstOrDefault();

                                foreach (DAO_Siswa.SiswaDataSimple m_siswa in lst_siswa.OrderBy(m => m.Nama))
                                {
                                    bool is_naik_kelas = true;
                                    string naik_ke_kelas = "";
                                    KenaikanKelas kenaikan_kelas = DAO_KenaikanKelas.GetByTAByKelasBySiswa_Entity(tahun_ajaran, rel_kelas_det, m_siswa.Kode.ToString());
                                    if (kenaikan_kelas != null)
                                    {
                                        if (kenaikan_kelas.TahunAjaran != null)
                                        {
                                            is_naik_kelas = kenaikan_kelas.IsNaik;
                                        }
                                    }
                                    Kelas kelas_naik = DAO_Kelas.GetKelasNext(m_kelas.Rel_Sekolah.ToString(), m_kelas.Kode.ToString());
                                    if (is_naik_kelas)
                                    {
                                        naik_ke_kelas = (m_kelas.Nama.ToUpper() == "IX" || m_kelas.Nama == "9" ? "Lulus" : kelas_naik.Nama);
                                    }
                                    else
                                    {
                                        naik_ke_kelas = m_kelas.Nama;
                                    }

                                    jumlah_keseluruhan = 0;
                                    jumlah_mapel_rapor = 0;
                                    
                                    lst_kkm_nilai_rapor.Clear();
                                    int id_rapor = 0;
                                    List<decimal> lst_nilai_ipa = new List<decimal>();
                                    lst_nilai_ipa.Clear();
                                    foreach (Rapor_Desain_Det item in lst_rapor_desain_det.FindAll(m => m.Rel_Mapel.Trim() != ""))
                                    {

                                        if (item.Rel_Mapel.Trim() != "")
                                        {

                                            lst_nilai_siswa_det = lst_nilai_siswa_det_by_periode.FindAll(
                                                    m => m.Rel_Mapel == item.Rel_Mapel &&
                                                         m.Rel_Siswa == m.Rel_Siswa
                                                );

                                            //get struktur nilainya
                                            Rapor_StrukturNilai m_struktur_nilai = DAO_Rapor_StrukturNilai.GetATop1ByTABySMByKelasByMapel_Entity(
                                                    tahun_ajaran, semester, m_kelas.Kode.ToString(), item.Rel_Mapel
                                                );

                                            decimal nilai_rapor = 0;
                                            decimal jumlah_nilai_rapor = 0;
                                            decimal count_nilai_rapor = 0;                                            
                                            if (m_struktur_nilai != null)
                                            {
                                                if (m_struktur_nilai.TahunAjaran != null)
                                                {

                                                    lst_kkm_nilai_rapor.Add(new NilaiWithKey
                                                    {
                                                        Key = (jumlah_mapel_rapor + 1).ToString(),
                                                        Nilai = m_struktur_nilai.KKM
                                                    });

                                                    //get struktur nilai det AP
                                                    List<Rapor_StrukturNilai_AP> lst_rapor_struktur_nilai_ap = DAO_Rapor_StrukturNilai_AP.GetAllByHeader_Entity(
                                                            m_struktur_nilai.Kode.ToString()
                                                        );
                                                    lst_nilai_ap.Clear();
                                                    decimal jumlah_nilai_ap = 0;
                                                    decimal nilai_ap = 0;
                                                    int count_ap = 0;
                                                    foreach (var item_rapor_struktur_nilai_ap in lst_rapor_struktur_nilai_ap)
                                                    {

                                                        //get struktur nilai det KD
                                                        List<Rapor_StrukturNilai_KD> lst_rapor_struktur_nilai_kd = DAO_Rapor_StrukturNilai_KD.GetAllByHeader_Entity(
                                                                item_rapor_struktur_nilai_ap.Kode.ToString()
                                                            );
                                                        lst_nilai_kd.Clear();
                                                        foreach (var item_rapor_struktur_nilai_kd in lst_rapor_struktur_nilai_kd)
                                                        {

                                                            //get struktur nilai det KP
                                                            List<Rapor_StrukturNilai_KP> lst_rapor_struktur_nilai_kp = DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(
                                                                item_rapor_struktur_nilai_kd.Kode.ToString()
                                                            );
                                                            decimal jumlah_nilai_kd = 0;
                                                            decimal nilai_kd = 0;
                                                            int count_kd = 0;
                                                            foreach (var item_rapor_struktur_nilai_kp in lst_rapor_struktur_nilai_kp)
                                                            {
                                                                string nilai = "";
                                                                Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(item_rapor_struktur_nilai_kp.Rel_Rapor_KomponenPenilaian.ToString());
                                                                if (m_kp != null)
                                                                {

                                                                    Rapor_NilaiSiswa_Det m_nilai_det = lst_nilai_siswa_det.FindAll(
                                                                        m => m.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper() &&
                                                                             m.Rel_Mapel.ToString().Trim().ToUpper() == m_struktur_nilai.Rel_Mapel.ToString().Trim().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_AP.Trim().ToUpper() == item_rapor_struktur_nilai_ap.Kode.ToString().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_KD.Trim().ToUpper() == item_rapor_struktur_nilai_kd.Kode.ToString().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_KP.Trim().ToUpper() == item_rapor_struktur_nilai_kp.Kode.ToString().ToUpper()
                                                                    ).FirstOrDefault();

                                                                    if (m_nilai_det != null)
                                                                    {
                                                                        if (m_nilai_det.Nilai.Trim() != "")
                                                                        {
                                                                            nilai = m_nilai_det.Nilai.Trim();
                                                                            if (m_nilai_det.PB == null) m_nilai_det.PB = "";
                                                                            if (item_rapor_struktur_nilai_kp.IsAdaPB && m_nilai_det.PB.Trim() != nilai && m_nilai_det.PB.Trim() != "" && Libs.IsAngka(m_nilai_det.PB.Trim().Replace("=", "")))
                                                                            {
                                                                                nilai = m_nilai_det.PB.Trim().Replace("=", "");
                                                                            }

                                                                            if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                                            {
                                                                                nilai_kd += Math.Round((item_rapor_struktur_nilai_kp.BobotNK / 100) * Libs.GetStringToDecimal(nilai), 2, MidpointRounding.AwayFromZero);
                                                                            }
                                                                            else if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                            {
                                                                                jumlah_nilai_kd += Libs.GetStringToDecimal(nilai);
                                                                            }
                                                                            count_kd++;
                                                                        }
                                                                    }

                                                                }

                                                            }

                                                            if (count_kd > 0)
                                                            {
                                                                if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                {
                                                                    nilai_kd = Math.Round(jumlah_nilai_kd / count_kd, 2, MidpointRounding.AwayFromZero);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                nilai_kd = 0;
                                                            }

                                                            //nilai_kd = Math.Round(nilai_kd, Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero);
                                                            nilai_kd = Math.Round(nilai_kd, 2, MidpointRounding.AwayFromZero);
                                                            lst_nilai_kd.Add(new NilaiWithKey
                                                            {
                                                                Key = item_rapor_struktur_nilai_kd.Kode.ToString(),
                                                                Nilai = nilai_kd
                                                            });
                                                            //end get struktur nilai det KP

                                                        }
                                                        //end get struktur nilai det KD

                                                        //get nilai ap
                                                        count_ap = 0;
                                                        nilai_ap = 0;
                                                        jumlah_nilai_ap = 0;
                                                        foreach (var item_nilai_kd in lst_nilai_kd)
                                                        {
                                                            if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                            {
                                                                nilai_ap += Math.Round((DAO_Rapor_StrukturNilai_KD.GetByID_Entity(item_nilai_kd.Key).BobotAP / 100) * item_nilai_kd.Nilai, 2, MidpointRounding.AwayFromZero);
                                                            }
                                                            else if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                jumlah_nilai_ap += item_nilai_kd.Nilai;
                                                            }
                                                            count_ap++;
                                                        }
                                                        if (count_ap > 0)
                                                        {
                                                            if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                nilai_ap = Math.Round(jumlah_nilai_ap / count_ap, 2, MidpointRounding.AwayFromZero);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            nilai_ap = 0;
                                                        }
                                                        nilai_ap = Math.Round(nilai_ap, 2, MidpointRounding.AwayFromZero);
                                                        lst_nilai_ap.Add(new NilaiWithKey
                                                        {
                                                            Key = item_rapor_struktur_nilai_ap.Kode.ToString(),
                                                            Nilai = nilai_ap
                                                        });
                                                        //end get nilai ap

                                                    }
                                                    //end get struktur nilai det AP

                                                    //get nilai rapor
                                                    nilai_rapor = 0;
                                                    jumlah_nilai_rapor = 0;
                                                    count_nilai_rapor = 0;
                                                    foreach (var item_nilai_ap in lst_nilai_ap)
                                                    {
                                                        if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                        {
                                                            nilai_rapor += Math.Round((DAO_Rapor_StrukturNilai_AP.GetByID_Entity(item_nilai_ap.Key).BobotRapor / 100) * item_nilai_ap.Nilai, 2, MidpointRounding.AwayFromZero);
                                                        }
                                                        else if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                        {
                                                            jumlah_nilai_rapor += item_nilai_ap.Nilai;
                                                        }
                                                        count_nilai_rapor++;
                                                    }
                                                    if (count_nilai_rapor > 0)
                                                    {
                                                        if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                        {
                                                            nilai_rapor = Math.Round(jumlah_nilai_rapor / count_nilai_rapor, 3, MidpointRounding.AwayFromZero);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        nilai_rapor = 0;
                                                    }

                                                    if (DAO_Mapel.GetJenisMapel(m_struktur_nilai.Rel_Mapel.ToString()) == Application_Libs.Libs.JENIS_MAPEL.PILIHAN && nilai_rapor == 0)
                                                    {
                                                        nilai_rapor = -99;
                                                        jumlah_mapel_rapor++;
                                                    }
                                                    else
                                                    {
                                                        nilai_rapor = Math.Round(nilai_rapor, Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero);
                                                        jumlah_keseluruhan += nilai_rapor;
                                                        jumlah_mapel_rapor++;
                                                    }
                                                    //end get nilai rapor

                                                }
                                            }
                                            //end get struktur nilai

                                            //cek nilai IPA
                                            if (nilai_rapor != -99)
                                            {
                                                id_rapor++;

                                                if (
                                                    m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_BIOLOGI ||
                                                    m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA
                                                )
                                                {
                                                    lst_nilai_ipa.Add(nilai_rapor);
                                                }

                                                string tanggal_rapor = Libs.GetTanggalIndonesiaFromDate(m_rapor_arsip.TanggalRapor, false);
                                                if (!(m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_BIOLOGI ||
                                                    m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA))
                                                { //biologi
                                                    hasil.Add(new KTSP_RaporNilai
                                                    {
                                                        IDSiswa = m_siswa.Nama + m_siswa.Kode.ToString(),
                                                        Nama = Libs.GetPerbaikiEjaanNama(m_siswa.Nama),
                                                        NIS = m_siswa.NISSekolah,
                                                        NISN = m_siswa.NISN,
                                                        NamaMapel = item.NamaMapelRapor,
                                                        TahunPelajaran = tahun_ajaran,
                                                        Semester = semester,
                                                        Urutan = id_rapor,
                                                        Alamat = "",
                                                        Kelas = m_kelas_det.Nama,
                                                        NamaSekolah = "",
                                                        NilaiRapor = Convert.ToDecimal(nilai_rapor),
                                                        KKM = m_struktur_nilai.KKM,
                                                        Predikat = Libs.Terbilang(Convert.ToInt32(nilai_rapor)).ToLower(),
                                                        WaliKelas = s_walikelas,
                                                        TanggalRapor = Libs.GetTanggalIndonesiaFromDate(m_rapor_arsip.TanggalRapor, false),
                                                        KepalaSekolah = m_rapor_arsip.KepalaSekolah,
                                                        IsNaik = is_naik_kelas,
                                                        NaikKeKelas = naik_ke_kelas,
                                                        Halaman = Libs.GetStringToInteger(halaman),
                                                        TTDGuru = img_ttd_guru
                                                    });
                                                }
                                                else if (m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA) //fisika
                                                {
                                                    nilai_rapor = Math.Round(lst_nilai_ipa.DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero);
                                                    hasil.Add(new KTSP_RaporNilai
                                                    {
                                                        IDSiswa = m_siswa.Nama + m_siswa.Kode.ToString(),
                                                        Nama = Libs.GetPerbaikiEjaanNama(m_siswa.Nama),
                                                        NIS = m_siswa.NISSekolah,
                                                        NISN = m_siswa.NISN,
                                                        NamaMapel = "Ilmu Pengetahuan Alam",
                                                        TahunPelajaran = tahun_ajaran,
                                                        Semester = semester,
                                                        Urutan = id_rapor,
                                                        Alamat = "",
                                                        Kelas = m_kelas_det.Nama,
                                                        NamaSekolah = "",
                                                        NilaiRapor = Convert.ToDecimal(nilai_rapor),
                                                        KKM = m_struktur_nilai.KKM,
                                                        Predikat = Libs.Terbilang(Convert.ToInt32(nilai_rapor)).ToLower(),
                                                        WaliKelas = s_walikelas,
                                                        TanggalRapor = Libs.GetTanggalIndonesiaFromDate(m_rapor_arsip.TanggalRapor, false),
                                                        KepalaSekolah = m_rapor_arsip.KepalaSekolah,
                                                        IsNaik = is_naik_kelas,
                                                        NaikKeKelas = naik_ke_kelas,
                                                        Halaman = Libs.GetStringToInteger(halaman),
                                                        TTDGuru = img_ttd_guru
                                                    });
                                                }
                                            }
                                            //end cek nilai IPA
                                        }

                                    }

                                    nomor++;
                                    //break;
                                }

                            }

                        }
                    }

                }
            }

            return hasil;
        }

        public static List<KURTILAS_RaporNilai> GetNilaiRapor_KURTILAS(
            string tahun_ajaran,
            string semester,
            string rel_kelas_det,
            ref List<RaporLTSCapaianKedisiplinan> ListRaporLTSCapaianKedisiplinan,
            string halaman,
            string rel_siswa,
            string s_lokasi_ttd,
            bool show_qrcode = true
            )
        {
            if (Libs.GetStringToDecimal(tahun_ajaran.Substring(0, 4)) >= 2020) return GetNilaiRapor_KURTILAS_2020(tahun_ajaran, semester, rel_kelas_det, ref ListRaporLTSCapaianKedisiplinan, halaman, rel_siswa, s_lokasi_ttd, show_qrcode);

            System.Drawing.Image img = null;
            string s_loc = s_lokasi_ttd;
            if (File.Exists(s_loc) && s_loc.Trim() != "")
            {
                img = System.Drawing.Image.FromFile(s_loc);
            }
            byte[] img_ttd_guru = (byte[])(new ImageConverter()).ConvertTo(img, typeof(byte[]));

            List<KURTILAS_RaporNilai> hasil = new List<KURTILAS_RaporNilai>();

            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {

                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());
                    if (m_kelas != null)
                    {
                        if (m_kelas.Nama != null)
                        {
                            string s_walikelas = "";
                            List<FormasiGuruKelas> lst_formasi_guru_kelas = DAO_FormasiGuruKelas.GetByUnitByTABySM_Entity(
                                        GetUnitSekolah().Kode.ToString(), tahun_ajaran, semester
                                    ).FindAll(m => m.Rel_KelasDet.ToString().ToUpper() == rel_kelas_det.Trim().ToUpper());
                            if (lst_formasi_guru_kelas != null)
                            {
                                if (lst_formasi_guru_kelas.Count > 0)
                                {
                                    FormasiGuruKelas m_guru_kelas = lst_formasi_guru_kelas.FirstOrDefault();
                                    if (m_guru_kelas != null)
                                    {
                                        if (m_guru_kelas.TahunAjaran != null)
                                        {
                                            Pegawai m_pegawai = DAO_Pegawai.GetByID_Entity(m_guru_kelas.Rel_GuruKelas);
                                            if (m_pegawai != null)
                                            {
                                                if (m_pegawai.Nama != null)
                                                {
                                                    s_walikelas = m_pegawai.Nama;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            Rapor_Desain rapor_desain = DAO_Rapor_Desain.GetByTABySMByKelas_Entity(
                                    tahun_ajaran, semester, rel_kelas_det, DAO_Rapor_Desain.JenisRapor.Semester
                                ).FirstOrDefault();

                            List<Rapor_Desain_Det> lst_rapor_desain_det = DAO_Rapor_Desain_Det.GetAllByHeader_Entity(rapor_desain.Kode.ToString()).FindAll(m => m.Nomor.Trim() != "" && m.Rel_Mapel.Trim() != "");

                            //list cols header
                            
                            //end list cols header
                            if (rapor_desain != null)
                            {
                                List<DAO_Siswa.SiswaDataSimple> lst_siswa = DAO_Siswa.GetAllSiswaDataSimpleByTahunAjaranUnitKelas_Entity(
                                        m_kelas.Rel_Sekolah.ToString(),
                                        rel_kelas_det,
                                        tahun_ajaran,
                                        semester
                                    );
                                List<NilaiWithKey> lst_nilai_ap = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_nilai_kd = new List<NilaiWithKey>();
                                List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT> lst_nilai_siswa_det = new List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT>();
                                List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT> lst_nilai_siswa_det_by_periode = new List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT>();

                                lst_nilai_siswa_det_by_periode = DAO_Rapor_NilaiSiswa_Det.GetAllByTABySMByKelasDet_Entity(
                                        tahun_ajaran, semester, rel_kelas_det
                                    );

                                int id_kolom_ledger = 0;
                                int nomor = 1;
                                int jumlah_awal_col_ledger = 3;
                                int jumlah_mapel_rapor = 0;

                                if (rel_siswa.Trim() != "") lst_siswa = lst_siswa.FindAll(m => rel_siswa.Trim().ToUpper().IndexOf(m.Kode.ToString().Trim().ToUpper() + ";") >= 0);
                                List<NilaiWithKey> lst_all_nilai_rapor = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_kkm_nilai_rapor = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_jumlah_nilai_keseluruhan = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_nilai_rapor = new List<NilaiWithKey>();
                                decimal jumlah_keseluruhan = 0;

                                int id_mulai_looping_rata_rata = id_kolom_ledger + 1;
                                int id_mulai_looping_kkm = jumlah_mapel_rapor + 1;

                                Rapor_Arsip m_rapor_arsip = DAO_Rapor_Arsip.GetAll_Entity().FindAll(
                                            m0 => m0.TahunAjaran == tahun_ajaran &&
                                                  m0.Semester == semester &&
                                                  m0.JenisRapor == "Semester"

                                        ).FirstOrDefault();

                                foreach (DAO_Siswa.SiswaDataSimple m_siswa in lst_siswa.OrderBy(m => m.Nama))
                                {
                                    string s_kelas = DAO_KelasDet.GetKelasForRapor(rel_kelas_det);
                                    string s_info_qr = "";
                                    s_info_qr = "NIS = " + m_siswa.NISSekolah + ", " +
                                                "Nama = " + m_siswa.Nama.Trim().ToUpper() + ", " +
                                                "Unit = SD, " +
                                                "Tahun Pelajaran & Semester = " + tahun_ajaran + " & " + semester + ", " +
                                                "Kelas = " + s_kelas;
                                    byte[] qr_code =
                                        (show_qrcode
                                            ? (byte[])(new ImageConverter()).ConvertTo(QRCodeGenerator.GetQRCode(s_info_qr, 20), typeof(byte[]))
                                            : null
                                        );

                                    bool is_naik_kelas = true;
                                    string naik_ke_kelas = "";
                                    KenaikanKelas kenaikan_kelas = DAO_KenaikanKelas.GetByTAByKelasBySiswa_Entity(tahun_ajaran, rel_kelas_det, m_siswa.Kode.ToString());
                                    if (kenaikan_kelas != null)
                                    {
                                        if (kenaikan_kelas.TahunAjaran != null)
                                        {
                                            is_naik_kelas = kenaikan_kelas.IsNaik;
                                        }
                                    }
                                    Kelas kelas_naik = DAO_Kelas.GetKelasNext(m_kelas.Rel_Sekolah.ToString(), m_kelas.Kode.ToString());
                                    if (is_naik_kelas)
                                    {
                                        naik_ke_kelas = (m_kelas.Nama.ToUpper() == "IX" || m_kelas.Nama == "9" ? "Lulus" : kelas_naik.Nama);
                                    }
                                    else
                                    {
                                        naik_ke_kelas = m_kelas.Nama;
                                    }

                                    jumlah_keseluruhan = 0;
                                    jumlah_mapel_rapor = 0;

                                    lst_kkm_nilai_rapor.Clear();
                                    id_kolom_ledger = jumlah_awal_col_ledger;

                                    List<decimal> lst_nilai_ipa_pengetahuan = new List<decimal>();
                                    List<decimal> lst_nilai_ipa_praktik = new List<decimal>();
                                    List<decimal> lst_kp_tugas = new List<decimal>();
                                    List<decimal> lst_kp_uh_terakhir = new List<decimal>();
                                    List<decimal> lst_kp_uh_non_terakhir = new List<decimal>();

                                    lst_nilai_ipa_pengetahuan.Clear();
                                    lst_nilai_ipa_praktik.Clear();

                                    decimal sikap_sosial = GetSikapSosial(tahun_ajaran, semester, rel_kelas_det, m_siswa.Kode.ToString());
                                    decimal sikap_spiritual = GetSikapSpiritual(tahun_ajaran, semester, rel_kelas_det, m_siswa.Kode.ToString());
                                    string deskripsi_ipa_pengetahuan = "";
                                    string deskripsi_ipa_praktik = "";
                                    //nilai non mulok
                                    foreach (Rapor_Desain_Det item in DAO_Rapor_Desain_Det.GetAllByHeader_Entity(rapor_desain.Kode.ToString()).FindAll(m => m.Nomor.Trim() != "" && m.Rel_Mapel.Trim() != ""))
                                    {

                                        if (item.Rel_Mapel.Trim() != "")
                                        {

                                            lst_nilai_siswa_det = lst_nilai_siswa_det_by_periode.FindAll(
                                                    m => m.Rel_Mapel == item.Rel_Mapel &&
                                                         m.Rel_Siswa == m.Rel_Siswa
                                                );

                                            //get struktur nilainya
                                            Rapor_StrukturNilai m_struktur_nilai = DAO_Rapor_StrukturNilai.GetATop1ByTABySMByKelasByMapel_Entity(
                                                    tahun_ajaran, semester, m_kelas.Kode.ToString(), item.Rel_Mapel
                                                );

                                            decimal nilai_rapor = 0;
                                            decimal jumlah_nilai_rapor = 0;
                                            decimal count_nilai_rapor = 0;

                                            decimal nilai_rapor_pengetahuan = 0;
                                            decimal nilai_rapor_keterampilan = 0;
                                            if (m_struktur_nilai != null)
                                            {
                                                if (m_struktur_nilai.TahunAjaran != null)
                                                {
                                                    lst_kp_tugas.Clear();
                                                    lst_kp_uh_terakhir.Clear();
                                                    lst_kp_uh_non_terakhir.Clear();

                                                    lst_kkm_nilai_rapor.Add(new NilaiWithKey
                                                    {
                                                        Key = (jumlah_mapel_rapor + 1).ToString(),
                                                        Nilai = m_struktur_nilai.KKM
                                                    });
                                                    //-----------nilai pengetahuan---------------
                                                    id_kolom_ledger++;
                                                    //get struktur nilai det AP
                                                    List<Rapor_StrukturNilai_AP> lst_rapor_struktur_nilai_ap = DAO_Rapor_StrukturNilai_AP.GetAllByHeaderByJenisAspekPenilaian_Entity(
                                                            m_struktur_nilai.Kode.ToString(), DAO_Rapor_StrukturNilai_AP.JenisAspekPenilaian.Pengetahuan
                                                        );
                                                    lst_nilai_ap.Clear();
                                                    decimal jumlah_nilai_ap = 0;
                                                    decimal nilai_ap = 0;
                                                    int count_ap = 0;
                                                    foreach (var item_rapor_struktur_nilai_ap in lst_rapor_struktur_nilai_ap)
                                                    {

                                                        //get struktur nilai det KD
                                                        List<Rapor_StrukturNilai_KD> lst_rapor_struktur_nilai_kd = DAO_Rapor_StrukturNilai_KD.GetAllByHeader_Entity(
                                                                item_rapor_struktur_nilai_ap.Kode.ToString()
                                                            );
                                                        lst_nilai_kd.Clear();
                                                        foreach (var item_rapor_struktur_nilai_kd in lst_rapor_struktur_nilai_kd)
                                                        {

                                                            Rapor_KompetensiDasar m_kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(item_rapor_struktur_nilai_kd.Rel_Rapor_KompetensiDasar.ToString());

                                                            //get struktur nilai det KP
                                                            List<Rapor_StrukturNilai_KP> lst_rapor_struktur_nilai_kp = DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(
                                                                item_rapor_struktur_nilai_kd.Kode.ToString()
                                                            );
                                                            decimal jumlah_nilai_kd = 0;
                                                            decimal nilai_kd = 0;
                                                            int count_kd = 0;
                                                            int id_kp = 1;
                                                            foreach (var item_rapor_struktur_nilai_kp in lst_rapor_struktur_nilai_kp)
                                                            {
                                                                string nilai = "";
                                                                Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(item_rapor_struktur_nilai_kp.Rel_Rapor_KomponenPenilaian.ToString());
                                                                if (m_kp != null)
                                                                {

                                                                    Rapor_NilaiSiswa_Det m_nilai_det = lst_nilai_siswa_det.FindAll(
                                                                        m => m.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper() &&
                                                                             m.Rel_Mapel.ToString().Trim().ToUpper() == m_struktur_nilai.Rel_Mapel.ToString().Trim().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_AP.Trim().ToUpper() == item_rapor_struktur_nilai_ap.Kode.ToString().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_KD.Trim().ToUpper() == item_rapor_struktur_nilai_kd.Kode.ToString().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_KP.Trim().ToUpper() == item_rapor_struktur_nilai_kp.Kode.ToString().ToUpper()
                                                                    ).FirstOrDefault();

                                                                    if (m_nilai_det != null)
                                                                    {
                                                                        if (m_nilai_det.Nilai.Trim() != "")
                                                                        {
                                                                            nilai = m_nilai_det.Nilai.Trim();
                                                                            if (m_nilai_det.PB == null) m_nilai_det.PB = "";
                                                                            if (item_rapor_struktur_nilai_kp.IsAdaPB && m_nilai_det.PB.Trim() != nilai && m_nilai_det.PB.Trim() != "" && Libs.IsAngka(m_nilai_det.PB.Trim().Replace("=", "")))
                                                                            {
                                                                                nilai = m_nilai_det.PB.Trim().Replace("=", "");
                                                                            }

                                                                            if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                                            {
                                                                                nilai_kd += Math.Round((item_rapor_struktur_nilai_kp.BobotNK / 100) * Libs.GetStringToDecimal(nilai), 2, MidpointRounding.AwayFromZero);
                                                                            }
                                                                            else if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                            {
                                                                                jumlah_nilai_kd += Libs.GetStringToDecimal(nilai);
                                                                            }
                                                                            count_kd++;
                                                                        }
                                                                    }

                                                                }

                                                                if (nilai.Trim() != "")
                                                                {
                                                                    if (
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "TUGAS" ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PH") >= 0 ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENILAIAN HARIAN") >= 0 ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENGETAHUAN") >= 0
                                                                        )
                                                                    {
                                                                        lst_kp_tugas.Add(
                                                                                Libs.GetStringToDecimal(nilai)
                                                                            );
                                                                    }
                                                                    else if (
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "UH"
                                                                        )
                                                                    {
                                                                        if (id_kp < lst_rapor_struktur_nilai_kp.Count)
                                                                        {
                                                                            lst_kp_uh_non_terakhir.Add(
                                                                                    Libs.GetStringToDecimal(nilai)
                                                                                );
                                                                        }
                                                                        else if (id_kp == lst_rapor_struktur_nilai_kp.Count)
                                                                        {
                                                                            lst_kp_uh_terakhir.Add(
                                                                                    Libs.GetStringToDecimal(nilai)
                                                                                );
                                                                        }
                                                                    }
                                                                    else if (
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PTS") >= 0
                                                                        )
                                                                    {
                                                                        lst_kp_uh_non_terakhir.Add(
                                                                                Libs.GetStringToDecimal(nilai)
                                                                            );
                                                                    }
                                                                    else if (
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PAS") >= 0
                                                                        )
                                                                    {
                                                                        lst_kp_uh_terakhir.Add(
                                                                                Libs.GetStringToDecimal(nilai)
                                                                            );
                                                                    }
                                                                }

                                                                id_kp++;
                                                            }

                                                            if (count_kd > 0)
                                                            {
                                                                if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                {
                                                                    nilai_kd = Math.Round(jumlah_nilai_kd / count_kd, 2, MidpointRounding.AwayFromZero);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                nilai_kd = 0;
                                                            }

                                                            nilai_kd = Math.Round(nilai_kd, 2, MidpointRounding.AwayFromZero);
                                                            lst_nilai_kd.Add(new NilaiWithKey
                                                            {
                                                                Key = item_rapor_struktur_nilai_kd.Kode.ToString(),
                                                                Nilai = nilai_kd
                                                            });
                                                            //end get struktur nilai det KP

                                                        }
                                                        //end get struktur nilai det KD

                                                        //get nilai ap
                                                        count_ap = 0;
                                                        nilai_ap = 0;
                                                        jumlah_nilai_ap = 0;
                                                        foreach (var item_nilai_kd in lst_nilai_kd)
                                                        {
                                                            if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                            {
                                                                nilai_ap += Math.Round((DAO_Rapor_StrukturNilai_KD.GetByID_Entity(item_nilai_kd.Key).BobotAP / 100) * item_nilai_kd.Nilai, 2, MidpointRounding.AwayFromZero);
                                                            }
                                                            else if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                jumlah_nilai_ap += item_nilai_kd.Nilai;
                                                            }
                                                            count_ap++;
                                                        }
                                                        if (count_ap > 0)
                                                        {
                                                            if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                nilai_ap = Math.Round(jumlah_nilai_ap / count_ap, 2, MidpointRounding.AwayFromZero);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            nilai_ap = 0;
                                                        }
                                                        nilai_ap = Math.Round(nilai_ap, 2, MidpointRounding.AwayFromZero);
                                                        lst_nilai_ap.Add(new NilaiWithKey
                                                        {
                                                            Key = item_rapor_struktur_nilai_ap.Kode.ToString(),
                                                            Nilai = nilai_ap
                                                        });
                                                        //end get nilai ap

                                                    }
                                                    //end get struktur nilai det AP

                                                    //get nilai rapor
                                                    nilai_rapor = 0;
                                                    jumlah_nilai_rapor = 0;
                                                    count_nilai_rapor = 0;
                                                    if (m_struktur_nilai.Is_PH_PTS_PAS)
                                                    {
                                                        string s_nilai_ph = "";
                                                        string s_nilai_pts = "";
                                                        string s_nilai_pas = "";

                                                        s_nilai_ph = (
                                                                lst_kp_tugas.Count > 0
                                                                ? Math.Round(lst_kp_tugas.DefaultIfEmpty().Average(), 2, MidpointRounding.AwayFromZero).ToString()
                                                                : "0"
                                                            );
                                                        s_nilai_pts = (
                                                                lst_kp_uh_non_terakhir.Count > 0
                                                                ? Math.Round(lst_kp_uh_non_terakhir.DefaultIfEmpty().Average(), 2, MidpointRounding.AwayFromZero).ToString()
                                                                : "0"
                                                            );
                                                        s_nilai_pas = (
                                                                lst_kp_uh_terakhir.Count > 0
                                                                ? Math.Round(lst_kp_uh_terakhir.DefaultIfEmpty().Average(), 2, MidpointRounding.AwayFromZero).ToString()
                                                                : "0"
                                                            );

                                                        //nilai_rapor = (Libs.GetStringToDecimal(s_nilai_ph) * (m_struktur_nilai.BobotPH / 100)) +
                                                        //              (Libs.GetStringToDecimal(s_nilai_pts) * (m_struktur_nilai.BobotPTS / 100)) +
                                                        //              (Libs.GetStringToDecimal(s_nilai_pas) * (m_struktur_nilai.BobotPAS / 100));
                                                        nilai_rapor = Math.Round((Libs.GetStringToDecimal(s_nilai_ph) * (m_struktur_nilai.BobotPH / 100)), 1, MidpointRounding.AwayFromZero) +
                                                                      Math.Round((Libs.GetStringToDecimal(s_nilai_pts) * (m_struktur_nilai.BobotPTS / 100)), 2, MidpointRounding.AwayFromZero) +
                                                                      Math.Round((Libs.GetStringToDecimal(s_nilai_pas) * (m_struktur_nilai.BobotPAS / 100)), 1, MidpointRounding.AwayFromZero);
                                                    }
                                                    else if (!m_struktur_nilai.Is_PH_PTS_PAS)
                                                    {
                                                        foreach (var item_nilai_ap in lst_nilai_ap)
                                                        {
                                                            if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                            {
                                                                nilai_rapor += item_nilai_ap.Nilai;
                                                            }
                                                            else if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                jumlah_nilai_rapor += item_nilai_ap.Nilai;
                                                            }
                                                            count_nilai_rapor++;
                                                        }
                                                        if (count_nilai_rapor > 0)
                                                        {
                                                            if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                nilai_rapor = Math.Round(jumlah_nilai_rapor / count_nilai_rapor, 3, MidpointRounding.AwayFromZero);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            nilai_rapor = 0;
                                                        }
                                                    }

                                                    nilai_rapor = Math.Round(nilai_rapor, 0, MidpointRounding.AwayFromZero);
                                                    nilai_rapor_pengetahuan = nilai_rapor;
                                                    lst_all_nilai_rapor.Add(new NilaiWithKey
                                                    {
                                                        Key = (id_kolom_ledger).ToString(),
                                                        Nilai = nilai_rapor_pengetahuan
                                                    });
                                                    id_kolom_ledger += 2;
                                                    //end get nilai rapor

                                                    //simpan nilai biologi or fisika pengetahuan
                                                    if (
                                                        m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_BIOLOGI ||
                                                        m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA
                                                    )
                                                    {
                                                        lst_nilai_ipa_pengetahuan.Add(
                                                                Math.Round(nilai_rapor_pengetahuan, Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero)
                                                            );
                                                    }
                                                    //end simpan nilai biologi or fisika pengetahuan
                                                    //-----------end nilai pengetahuan---------------

                                                    //-----------nilai keterampilan---------------
                                                    //get struktur nilai det AP
                                                    lst_rapor_struktur_nilai_ap = DAO_Rapor_StrukturNilai_AP.GetAllByHeaderByJenisAspekPenilaian_Entity(
                                                            m_struktur_nilai.Kode.ToString(), DAO_Rapor_StrukturNilai_AP.JenisAspekPenilaian.Keterampilan
                                                        );
                                                    lst_nilai_ap.Clear();
                                                    jumlah_nilai_ap = 0;
                                                    nilai_ap = 0;
                                                    count_ap = 0;
                                                    foreach (var item_rapor_struktur_nilai_ap in lst_rapor_struktur_nilai_ap)
                                                    {

                                                        //get struktur nilai det KD
                                                        List<Rapor_StrukturNilai_KD> lst_rapor_struktur_nilai_kd = DAO_Rapor_StrukturNilai_KD.GetAllByHeader_Entity(
                                                                item_rapor_struktur_nilai_ap.Kode.ToString()
                                                            );
                                                        lst_nilai_kd.Clear();
                                                        foreach (var item_rapor_struktur_nilai_kd in lst_rapor_struktur_nilai_kd)
                                                        {

                                                            //get struktur nilai det KP
                                                            List<Rapor_StrukturNilai_KP> lst_rapor_struktur_nilai_kp = DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(
                                                                item_rapor_struktur_nilai_kd.Kode.ToString()
                                                            );
                                                            decimal jumlah_nilai_kd = 0;
                                                            decimal nilai_kd = 0;
                                                            int count_kd = 0;
                                                            foreach (var item_rapor_struktur_nilai_kp in lst_rapor_struktur_nilai_kp)
                                                            {
                                                                string nilai = "";
                                                                Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(item_rapor_struktur_nilai_kp.Rel_Rapor_KomponenPenilaian.ToString());
                                                                if (m_kp != null)
                                                                {

                                                                    Rapor_NilaiSiswa_Det m_nilai_det = lst_nilai_siswa_det.FindAll(
                                                                        m => m.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper() &&
                                                                             m.Rel_Mapel.ToString().Trim().ToUpper() == m_struktur_nilai.Rel_Mapel.ToString().Trim().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_AP.Trim().ToUpper() == item_rapor_struktur_nilai_ap.Kode.ToString().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_KD.Trim().ToUpper() == item_rapor_struktur_nilai_kd.Kode.ToString().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_KP.Trim().ToUpper() == item_rapor_struktur_nilai_kp.Kode.ToString().ToUpper()
                                                                    ).FirstOrDefault();

                                                                    if (m_nilai_det != null)
                                                                    {
                                                                        if (m_nilai_det.Nilai.Trim() != "")
                                                                        {
                                                                            nilai = m_nilai_det.Nilai.Trim();
                                                                            if (m_nilai_det.PB == null) m_nilai_det.PB = "";
                                                                            if (item_rapor_struktur_nilai_kp.IsAdaPB && m_nilai_det.PB.Trim() != nilai && m_nilai_det.PB.Trim() != "" && Libs.IsAngka(m_nilai_det.PB.Trim().Replace("=", "")))
                                                                            {
                                                                                nilai = m_nilai_det.PB.Trim().Replace("=", "");
                                                                            }

                                                                            if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                                            {
                                                                                nilai_kd += Math.Round((item_rapor_struktur_nilai_kp.BobotNK / 100) * Libs.GetStringToDecimal(nilai), 2, MidpointRounding.AwayFromZero);
                                                                            }
                                                                            else if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                            {
                                                                                jumlah_nilai_kd += Libs.GetStringToDecimal(nilai);
                                                                            }
                                                                            count_kd++;
                                                                        }
                                                                        //else
                                                                        //{
                                                                        //    nilai = m_nilai_det.Nilai.Trim();
                                                                        //    if (m_nilai_det.PB == null) m_nilai_det.PB = "";
                                                                        //    if (item_rapor_struktur_nilai_kp.IsAdaPB && m_nilai_det.PB.Trim() != nilai && m_nilai_det.PB.Trim() != "" && Libs.IsAngka(m_nilai_det.PB.Trim().Replace("=", "")))
                                                                        //    {
                                                                        //        nilai = m_nilai_det.PB.Trim().Replace("=", "");
                                                                        //    }

                                                                        //    if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                                        //    {
                                                                        //        nilai_kd += Math.Round((item_rapor_struktur_nilai_kp.BobotNK / 100) * Libs.GetStringToDecimal(nilai), 2, MidpointRounding.AwayFromZero);
                                                                        //    }
                                                                        //    else if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                        //    {
                                                                        //        jumlah_nilai_kd += Libs.GetStringToDecimal(nilai);
                                                                        //    }
                                                                        //    count_kd++;
                                                                        //}
                                                                    }

                                                                }

                                                            }

                                                            if (count_kd > 0)
                                                            {
                                                                if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                {
                                                                    nilai_kd = Math.Round(jumlah_nilai_kd / count_kd, 2, MidpointRounding.AwayFromZero);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                nilai_kd = 0;
                                                            }

                                                            nilai_kd = Math.Round(nilai_kd, Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero);
                                                            lst_nilai_kd.Add(new NilaiWithKey
                                                            {
                                                                Key = item_rapor_struktur_nilai_kd.Kode.ToString(),
                                                                Nilai = nilai_kd
                                                            });
                                                            //end get struktur nilai det KP

                                                        }
                                                        //end get struktur nilai det KD

                                                        //get nilai ap
                                                        count_ap = 0;
                                                        nilai_ap = 0;
                                                        jumlah_nilai_ap = 0;
                                                        foreach (var item_nilai_kd in lst_nilai_kd)
                                                        {
                                                            if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                            {
                                                                nilai_ap += Math.Round((DAO_Rapor_StrukturNilai_KD.GetByID_Entity(item_nilai_kd.Key).BobotAP / 100) * item_nilai_kd.Nilai, 2, MidpointRounding.AwayFromZero);
                                                            }
                                                            else if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                jumlah_nilai_ap += item_nilai_kd.Nilai;
                                                            }
                                                            count_ap++;
                                                        }
                                                        if (count_ap > 0)
                                                        {
                                                            if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                nilai_ap = Math.Round(jumlah_nilai_ap / count_ap, 2, MidpointRounding.AwayFromZero);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            nilai_ap = 0;
                                                        }
                                                        nilai_ap = Math.Round(nilai_ap, 2, MidpointRounding.AwayFromZero);
                                                        lst_nilai_ap.Add(new NilaiWithKey
                                                        {
                                                            Key = item_rapor_struktur_nilai_ap.Kode.ToString(),
                                                            Nilai = nilai_ap
                                                        });
                                                        //end get nilai ap
                                                        //get nilai rapor
                                                        nilai_rapor = 0;
                                                        jumlah_nilai_rapor = 0;
                                                        count_nilai_rapor = 0;
                                                        foreach (var item_nilai_ap in lst_nilai_ap)
                                                        {
                                                            if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                            {
                                                                nilai_rapor += item_nilai_ap.Nilai;
                                                            }
                                                            else if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                jumlah_nilai_rapor += item_nilai_ap.Nilai;
                                                            }
                                                            count_nilai_rapor++;
                                                        }
                                                        if (count_nilai_rapor > 0)
                                                        {
                                                            if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                nilai_rapor = Math.Round(jumlah_nilai_rapor / count_nilai_rapor, 3, MidpointRounding.AwayFromZero);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            nilai_rapor = 0;
                                                        }
                                                        nilai_rapor = Math.Round(nilai_rapor, 0, MidpointRounding.AwayFromZero);
                                                        nilai_rapor_keterampilan = nilai_rapor;
                                                        lst_all_nilai_rapor.Add(new NilaiWithKey
                                                        {
                                                            Key = (id_kolom_ledger).ToString(),
                                                            Nilai = nilai_rapor_keterampilan
                                                        });
                                                        //end get nilai rapor
                                                        //-----------end nilai keterampilan---------------

                                                    }
                                                    //end get struktur nilai det AP

                                                    //get nilai rapor
                                                    nilai_rapor = 0;
                                                    jumlah_nilai_rapor = 0;
                                                    count_nilai_rapor = 0;
                                                    foreach (var item_nilai_ap in lst_nilai_ap)
                                                    {
                                                        if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                        {
                                                            nilai_rapor += item_nilai_ap.Nilai;
                                                        }
                                                        else if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                        {
                                                            jumlah_nilai_rapor += item_nilai_ap.Nilai;
                                                        }
                                                        count_nilai_rapor++;
                                                    }
                                                    if (count_nilai_rapor > 0)
                                                    {
                                                        if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                        {
                                                            nilai_rapor = Math.Round(jumlah_nilai_rapor / count_nilai_rapor, 3, MidpointRounding.AwayFromZero);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        nilai_rapor = 0;
                                                    }
                                                    nilai_rapor = Math.Round(nilai_rapor, 0, MidpointRounding.AwayFromZero);
                                                    nilai_rapor_keterampilan = nilai_rapor;
                                                    lst_all_nilai_rapor.Add(new NilaiWithKey
                                                    {
                                                        Key = (id_kolom_ledger).ToString(),
                                                        Nilai = nilai_rapor_keterampilan
                                                    });
                                                    id_kolom_ledger++;
                                                    //end get nilai rapor

                                                    //simpan nilai biologi or fisika pengetahuan
                                                    if (
                                                        m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_BIOLOGI ||
                                                        m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA
                                                    )
                                                    {
                                                        lst_nilai_ipa_praktik.Add(
                                                                Math.Round(nilai_rapor_keterampilan, Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero)
                                                            );
                                                    }
                                                    //end simpan nilai biologi or fisika pengetahuan

                                                    jumlah_mapel_rapor++;
                                                }
                                            }
                                            //end get struktur nilai

                                            //jika fisika tambahkan satu kolom
                                            if (item.Rel_Mapel.Trim().ToUpper() == Constantas_Kode_Mapel.KODE_FISIKA.Trim().ToUpper())
                                            {
                                                lst_kkm_nilai_rapor.Add(new NilaiWithKey
                                                {
                                                    Key = (jumlah_mapel_rapor + 1).ToString(),
                                                    Nilai = -99
                                                });

                                                nilai_rapor_pengetahuan = Math.Round(lst_nilai_ipa_pengetahuan.DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero);
                                                nilai_rapor_keterampilan = Math.Round(lst_nilai_ipa_praktik.DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero);
                                                
                                                jumlah_mapel_rapor++;
                                            }

                                            string tanggal_rapor = Libs.GetTanggalIndonesiaFromDate(m_rapor_arsip.TanggalRapor, false);                                            

                                            if (m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_BIOLOGI ||
                                                    m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA)
                                            {
                                                deskripsi_ipa_pengetahuan += (deskripsi_ipa_pengetahuan.Trim() != "" && m_struktur_nilai.DeskripsiPengetahuan.Trim() != "" ? ", " : "") + m_struktur_nilai.DeskripsiPengetahuan;
                                                deskripsi_ipa_praktik += (deskripsi_ipa_praktik.Trim() != "" && m_struktur_nilai.DeskripsiKeterampilan.Trim() != "" ? ", " : "") + m_struktur_nilai.DeskripsiKeterampilan;
                                            }

                                            if (!(m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_BIOLOGI ||
                                                    m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA))
                                            {   
                                                if (nilai_rapor_pengetahuan > 0)
                                                {
                                                    hasil.Add(new KURTILAS_RaporNilai
                                                    {
                                                        IDSiswa = m_siswa.Nama + m_siswa.Kode.ToString(),
                                                        Nama = Libs.GetPerbaikiEjaanNama(m_siswa.Nama),
                                                        NIS = m_siswa.NISSekolah,
                                                        NISN = m_siswa.NISN,
                                                        NamaMapel = item.NamaMapelRapor,
                                                        TahunPelajaran = tahun_ajaran,
                                                        JenisNilai = "1.Pengetahuan",
                                                        Semester = semester,
                                                        Alamat = "",
                                                        Kelas = m_kelas_det.Nama,
                                                        NamaSekolah = "",
                                                        Nilai = Convert.ToDecimal(nilai_rapor_pengetahuan),
                                                        KKM = m_struktur_nilai.KKM,
                                                        Deskripsi = Libs.GetHTMLSimpleText2(m_struktur_nilai.DeskripsiPengetahuan),
                                                        NilaiSosial = sikap_sosial,
                                                        NilaiSpiritual = sikap_spiritual,
                                                        WaliKelas = s_walikelas,
                                                        TanggalRapor = Libs.GetTanggalIndonesiaFromDate(m_rapor_arsip.TanggalRapor, false),
                                                        KepalaSekolah = m_rapor_arsip.KepalaSekolah,
                                                        IsNaik = is_naik_kelas,
                                                        NaikKeKelas = naik_ke_kelas,
                                                        Halaman = Libs.GetStringToInteger(halaman).ToString(),
                                                        TTDGuru = img_ttd_guru,
                                                        QRCode = qr_code
                                                    });
                                                }

                                                if (nilai_rapor_keterampilan > 0)
                                                {
                                                    hasil.Add(new KURTILAS_RaporNilai
                                                    {
                                                        IDSiswa = m_siswa.Nama + m_siswa.Kode.ToString(),
                                                        Nama = Libs.GetPerbaikiEjaanNama(m_siswa.Nama),
                                                        NIS = m_siswa.NISSekolah,
                                                        NISN = m_siswa.NISN,
                                                        NamaMapel = item.NamaMapelRapor,
                                                        TahunPelajaran = tahun_ajaran,
                                                        JenisNilai = "2.Keterampilan",
                                                        Semester = semester,
                                                        Alamat = "",
                                                        Kelas = m_kelas_det.Nama,
                                                        NamaSekolah = "",
                                                        Nilai = Convert.ToDecimal(nilai_rapor_keterampilan),
                                                        KKM = m_struktur_nilai.KKM,
                                                        Deskripsi = Libs.GetHTMLSimpleText2(m_struktur_nilai.DeskripsiKeterampilan),
                                                        NilaiSosial = sikap_sosial,
                                                        NilaiSpiritual = sikap_spiritual,
                                                        WaliKelas = s_walikelas,
                                                        TanggalRapor = Libs.GetTanggalIndonesiaFromDate(m_rapor_arsip.TanggalRapor, false),
                                                        KepalaSekolah = m_rapor_arsip.KepalaSekolah,
                                                        IsNaik = is_naik_kelas,
                                                        NaikKeKelas = naik_ke_kelas,
                                                        Halaman = Libs.GetStringToInteger(halaman).ToString(),
                                                        TTDGuru = img_ttd_guru,
                                                        QRCode = qr_code
                                                    });
                                                }
                                            }
                                            else if (m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA) //fisika
                                            {
                                                nilai_rapor_pengetahuan = Math.Round(lst_nilai_ipa_pengetahuan.DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero);
                                                nilai_rapor_keterampilan = Math.Round(lst_nilai_ipa_praktik.DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero);

                                                if (nilai_rapor_pengetahuan > 0)
                                                {
                                                    hasil.Add(new KURTILAS_RaporNilai
                                                    {
                                                        IDSiswa = m_siswa.Nama + m_siswa.Kode.ToString(),
                                                        Nama = Libs.GetPerbaikiEjaanNama(m_siswa.Nama),
                                                        NIS = m_siswa.NISSekolah,
                                                        NISN = m_siswa.NISN,
                                                        NamaMapel = "Ilmu Pengetahuan Alam",
                                                        TahunPelajaran = tahun_ajaran,
                                                        JenisNilai = "1.Pengetahuan",
                                                        Semester = semester,
                                                        Alamat = "",
                                                        Kelas = m_kelas_det.Nama,
                                                        NamaSekolah = "",
                                                        Nilai = Convert.ToDecimal(nilai_rapor_pengetahuan),
                                                        KKM = m_struktur_nilai.KKM,
                                                        Deskripsi = Libs.GetHTMLSimpleText2(deskripsi_ipa_pengetahuan),
                                                        NilaiSosial = sikap_sosial,
                                                        NilaiSpiritual = sikap_spiritual,
                                                        TanggalRapor = Libs.GetTanggalIndonesiaFromDate(m_rapor_arsip.TanggalRapor, false),
                                                        KepalaSekolah = m_rapor_arsip.KepalaSekolah,
                                                        IsNaik = is_naik_kelas,
                                                        NaikKeKelas = naik_ke_kelas,
                                                        Halaman = Libs.GetStringToInteger(halaman).ToString(),
                                                        TTDGuru = img_ttd_guru,
                                                        QRCode = qr_code
                                                    });
                                                }

                                                if (nilai_rapor_keterampilan > 0)
                                                {
                                                    hasil.Add(new KURTILAS_RaporNilai
                                                    {
                                                        IDSiswa = m_siswa.Nama + m_siswa.Kode.ToString(),
                                                        Nama = Libs.GetPerbaikiEjaanNama(m_siswa.Nama),
                                                        NIS = m_siswa.NISSekolah,
                                                        NISN = m_siswa.NISN,
                                                        NamaMapel = "Ilmu Pengetahuan Alam",
                                                        TahunPelajaran = tahun_ajaran,
                                                        JenisNilai = "2.Keterampilan",
                                                        Semester = semester,
                                                        Alamat = "",
                                                        Kelas = m_kelas_det.Nama,
                                                        NamaSekolah = "",
                                                        Nilai = Convert.ToDecimal(nilai_rapor_keterampilan),
                                                        KKM = m_struktur_nilai.KKM,
                                                        Deskripsi = Libs.GetHTMLSimpleText2(deskripsi_ipa_praktik),
                                                        NilaiSosial = sikap_sosial,
                                                        NilaiSpiritual = sikap_spiritual,
                                                        TanggalRapor = Libs.GetTanggalIndonesiaFromDate(m_rapor_arsip.TanggalRapor, false),
                                                        KepalaSekolah = m_rapor_arsip.KepalaSekolah,
                                                        IsNaik = is_naik_kelas,
                                                        NaikKeKelas = naik_ke_kelas,
                                                        Halaman = Libs.GetStringToInteger(halaman).ToString(),
                                                        TTDGuru = img_ttd_guru,
                                                        QRCode = qr_code
                                                    });
                                                }
                                            }
                                            //get nilai rapor kurtilas

                                            //end jika fisika                                               
                                        }

                                    }
                                    //end rapor non mulok

                                    lst_jumlah_nilai_keseluruhan.Add(new NilaiWithKey
                                    {
                                        Key = m_siswa.Kode.ToString(),
                                        Nilai = jumlah_keseluruhan
                                    });

                                    ////absen
                                    //string s_sakit = "-";
                                    //string s_izin = "-";
                                    //string s_alpa = "-";
                                    //string s_terlambat = "-";

                                    //List<DAO_SiswaAbsen.AbsenRekapRapor> lst_absen = new List<DAO_SiswaAbsen.AbsenRekapRapor>();
                                    //lst_absen = DAO_SiswaAbsen.GetRekapAbsenRaporBySiswaByPeriode_Entity(
                                    //        m_siswa.Kode.ToString(),
                                    //        (m_rapor_arsip != null ? m_rapor_arsip.TanggalAwalAbsen : DateTime.MinValue),
                                    //        (m_rapor_arsip != null ? m_rapor_arsip.TanggalAkhirAbsen : DateTime.MinValue)
                                    //    );
                                    //foreach (var absen in lst_absen)
                                    //{
                                    //    if (absen.Absen == Libs.JENIS_ABSENSI.SAKIT.Substring(0, 1)) s_sakit = absen.Jumlah.ToString();
                                    //    if (absen.Absen == Libs.JENIS_ABSENSI.IZIN.Substring(0, 1)) s_izin = absen.Jumlah.ToString();
                                    //    if (absen.Absen == Libs.JENIS_ABSENSI.ALPA.Substring(0, 1)) s_alpa = absen.Jumlah.ToString();
                                    //    if (absen.Absen == Libs.JENIS_ABSENSI.TERLAMBAT.Substring(0, 1)) s_terlambat = absen.Jumlah.ToString();
                                    //}
                                    ////end absen

                                    nomor++;
                                }
                                // end foreach siswa

                                for (int i = 1;
                                     i <= (DAO_Rapor_Desain_Det.GetAllByHeader_Entity(rapor_desain.Kode.ToString()).FindAll(m => m.Nomor.Trim() != "" && m.Rel_Mapel.Trim() != "").Count * 4) + (4);
                                     i++)
                                {
                                    decimal rata_rata = Math.Round(lst_all_nilai_rapor.FindAll(m => m.Key == (i + jumlah_awal_col_ledger).ToString()).Select(m => m.Nilai).DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP);                                    
                                }
                                for (int i = 1; i <= (DAO_Rapor_Desain_Det.GetAllByHeader_Entity(rapor_desain.Kode.ToString()).FindAll(m => m.Nomor.Trim() != "" && m.Rel_Mapel.Trim() != "").Count + 1); i++)
                                {
                                    decimal kkm = 0;
                                    NilaiWithKey m_kkm = lst_kkm_nilai_rapor.FindAll(m => m.Key == i.ToString()).FirstOrDefault();
                                    if (m_kkm != null)
                                    {
                                        if (m_kkm.Key != null)
                                        {
                                            kkm = m_kkm.Nilai;
                                        }
                                    }
                                }
                                decimal rata_rata_nilai_keseluruhan = Math.Round(lst_jumlah_nilai_keseluruhan.Select(m => m.Nilai).DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP);
                                decimal rata_rata_nilai_rata_rata_rapor = Math.Round(lst_nilai_rapor.Select(m => m.Nilai).DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP);

                            }

                        }
                    }

                }
            }

            return hasil;
        }

        public static List<KURTILAS_RaporNilai> GetNilaiRapor_KURTILAS_2020(
            string tahun_ajaran,
            string semester,
            string rel_kelas_det,
            ref List<RaporLTSCapaianKedisiplinan> ListRaporLTSCapaianKedisiplinan,
            string halaman,
            string rel_siswa,
            string s_lokasi_ttd,
            bool show_qrcode = true
            )
        {
            ListRaporLTSCapaianKedisiplinan.Clear();

            List<Rapor_KompetensiDasar> lst_kd = DAO_Rapor_KompetensiDasar.GetAll_Entity();
            List<Rapor_KomponenPenilaian> lst_kp = DAO_Rapor_KomponenPenilaian.GetAll_Entity();

            List<DAO_Rapor_NilaiSikapSiswa.Rapor_NilaiSikapSiswa_Lengkap> lst_nilai_sikap =
                DAO_Rapor_NilaiSikapSiswa.GetByTABySMByMapelByKelasDet_Entity(
                        tahun_ajaran, semester, rel_kelas_det
                    );

            System.Drawing.Image img = null;
            string s_loc = s_lokasi_ttd;
            if (File.Exists(s_loc) && s_loc.Trim() != "")
            {
                img = System.Drawing.Image.FromFile(s_loc);
            }
            byte[] img_ttd_guru = (byte[])(new ImageConverter()).ConvertTo(img, typeof(byte[]));

            List<KURTILAS_RaporNilai> hasil = new List<KURTILAS_RaporNilai>();

            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {
                    string s_deskripsi_pengetahuan = "";
                    string s_deskripsi_keterampilan = "";

                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());
                    if (m_kelas != null)
                    {
                        if (m_kelas.Nama != null)
                        {
                            string rel_struktur_nilai_sikap = "";
                            string deskripsi_spiritual = "";
                            string deskripsi_sosial = "";
                            var m_sn_sikap = DAO_Rapor_StrukturNilai.GetAllMapelSikapByTAByKelas_Entity(tahun_ajaran, semester, m_kelas.Kode.ToString()).FirstOrDefault();
                            if (m_sn_sikap != null)
                            {
                                if (m_sn_sikap.TahunAjaran != null)
                                {
                                    rel_struktur_nilai_sikap = m_sn_sikap.Kode.ToString();
                                    deskripsi_spiritual = m_sn_sikap.DeskripsiSikapSpiritual;
                                    deskripsi_sosial = m_sn_sikap.DeskripsiSikapSosial;
                                }
                            }

                            string s_walikelas = "";
                            List<FormasiGuruKelas> lst_formasi_guru_kelas = DAO_FormasiGuruKelas.GetByUnitByTABySM_Entity(
                                        GetUnitSekolah().Kode.ToString(), tahun_ajaran, semester
                                    ).FindAll(m => m.Rel_KelasDet.ToString().ToUpper() == rel_kelas_det.Trim().ToUpper());
                            if (lst_formasi_guru_kelas != null)
                            {
                                if (lst_formasi_guru_kelas.Count > 0)
                                {
                                    FormasiGuruKelas m_guru_kelas = lst_formasi_guru_kelas.FirstOrDefault();
                                    if (m_guru_kelas != null)
                                    {
                                        if (m_guru_kelas.TahunAjaran != null)
                                        {
                                            Pegawai m_pegawai = DAO_Pegawai.GetByID_Entity(m_guru_kelas.Rel_GuruKelas);
                                            if (m_pegawai != null)
                                            {
                                                if (m_pegawai.Nama != null)
                                                {
                                                    s_walikelas = m_pegawai.Nama;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            Rapor_Desain rapor_desain = DAO_Rapor_Desain.GetByTABySMByKelas_Entity(
                                    tahun_ajaran, semester, rel_kelas_det, DAO_Rapor_Desain.JenisRapor.Semester
                                ).FirstOrDefault();

                            List<Rapor_Desain_Det> lst_rapor_desain_det = DAO_Rapor_Desain_Det.GetAllByHeader_Entity(rapor_desain.Kode.ToString()).FindAll(m => m.Nomor.Trim() != "" && m.Rel_Mapel.Trim() != "");

                            //list cols header

                            //end list cols header
                            if (rapor_desain != null)
                            {
                                List<DAO_Siswa.SiswaDataSimple> lst_siswa = DAO_Siswa.GetAllSiswaDataSimpleByTahunAjaranUnitKelas_Entity(
                                        m_kelas.Rel_Sekolah.ToString(),
                                        rel_kelas_det,
                                        tahun_ajaran,
                                        semester
                                    );
                                List<NilaiWithKey> lst_nilai_ap = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_nilai_kd = new List<NilaiWithKey>();
                                List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT> lst_nilai_siswa_det = new List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT>();
                                List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT> lst_nilai_siswa_det_by_periode = new List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT>();

                                lst_nilai_siswa_det_by_periode = DAO_Rapor_NilaiSiswa_Det.GetAllByTABySMByKelasDet_Entity(
                                        tahun_ajaran, semester, rel_kelas_det
                                    );

                                int id_kolom_ledger = 0;
                                int nomor = 1;
                                int jumlah_awal_col_ledger = 3;
                                int jumlah_mapel_rapor = 0;

                                if (rel_siswa.Trim() != "") lst_siswa = lst_siswa.FindAll(m => rel_siswa.Trim().ToUpper().IndexOf(m.Kode.ToString().Trim().ToUpper() + ";") >= 0);
                                List<NilaiWithKey> lst_all_nilai_rapor = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_kkm_nilai_rapor = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_jumlah_nilai_keseluruhan = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_nilai_rapor = new List<NilaiWithKey>();
                                decimal jumlah_keseluruhan = 0;

                                int id_mulai_looping_rata_rata = id_kolom_ledger + 1;
                                int id_mulai_looping_kkm = jumlah_mapel_rapor + 1;

                                Rapor_Arsip m_rapor_arsip = DAO_Rapor_Arsip.GetAll_Entity().FindAll(
                                            m0 => m0.TahunAjaran == tahun_ajaran &&
                                                  m0.Semester == semester &&
                                                  m0.JenisRapor == "Semester"

                                        ).FirstOrDefault();

                                List<Rapor_ProgramTransisi> lst_program_transisi = DAO_Rapor_ProgramTransisi.GetByTABySMByKelasDet(
                                        tahun_ajaran, semester, m_kelas_det.Kode.ToString()
                                    );

                                List<Rapor_NilaiSiswa> lst_nilaisiswa =
                                    DAO_Rapor_NilaiSiswa.GetAllByTABySMByKelasDet_ForReport_Entity(tahun_ajaran, semester, rel_kelas_det);

                                foreach (DAO_Siswa.SiswaDataSimple m_siswa in lst_siswa.OrderBy(m => m.Nama))
                                {
                                    var lst_nilai_sikap_ = lst_nilai_sikap.FindAll(
                                            m0 => m0.Rel_Siswa.ToString().ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim()
                                        );

                                    Rapor_ProgramTransisi m_program_transisi = lst_program_transisi.FindAll(
                                            m0 => m0.Rel_Siswa.ToString().ToUpper().Trim() == m_siswa.Kode.ToString().ToUpper().Trim()
                                        ).FirstOrDefault();

                                    string s_kelas = DAO_KelasDet.GetByID_Entity(rel_kelas_det).Nama;
                                    string s_info_qr = "";
                                    s_info_qr = "NIS = " + m_siswa.NISSekolah + ", " +
                                                "Nama = " + m_siswa.Nama.Trim().ToUpper() + ", " +
                                                "Unit = SMP, " +
                                                "Tahun Pelajaran & Semester = " + tahun_ajaran + " & " + semester + ", " +
                                                "Kelas = " + s_kelas;
                                    byte[] qr_code =
                                        (show_qrcode
                                            ? (byte[])(new ImageConverter()).ConvertTo(QRCodeGenerator.GetQRCode(s_info_qr, 20), typeof(byte[]))
                                            : null
                                        );

                                    bool is_naik_kelas = true;
                                    string naik_ke_kelas = "";
                                    KenaikanKelas kenaikan_kelas = DAO_KenaikanKelas.GetByTAByKelasBySiswa_Entity(tahun_ajaran, rel_kelas_det, m_siswa.Kode.ToString());
                                    if (kenaikan_kelas != null)
                                    {
                                        if (kenaikan_kelas.TahunAjaran != null)
                                        {
                                            is_naik_kelas = kenaikan_kelas.IsNaik;
                                        }
                                    }
                                    Kelas kelas_naik = DAO_Kelas.GetKelasNext(m_kelas.Rel_Sekolah.ToString(), m_kelas.Kode.ToString());
                                    if (is_naik_kelas)
                                    {
                                        naik_ke_kelas = (m_kelas.Nama.ToUpper() == "IX" || m_kelas.Nama == "9" ? "Lulus" : kelas_naik.Nama);
                                    }
                                    else
                                    {
                                        naik_ke_kelas = m_kelas.Nama;
                                    }

                                    jumlah_keseluruhan = 0;
                                    jumlah_mapel_rapor = 0;

                                    lst_kkm_nilai_rapor.Clear();
                                    id_kolom_ledger = jumlah_awal_col_ledger;

                                    List<decimal> lst_nilai_ipa_pengetahuan = new List<decimal>();
                                    List<decimal> lst_nilai_ipa_praktik = new List<decimal>();
                                    List<decimal> lst_kp_tugas = new List<decimal>();
                                    List<decimal> lst_kp_uh_terakhir = new List<decimal>();
                                    List<decimal> lst_kp_uh_non_terakhir = new List<decimal>();

                                    lst_nilai_ipa_pengetahuan.Clear();
                                    lst_nilai_ipa_praktik.Clear();

                                    decimal sikap_sosial = GetSikapSosial(tahun_ajaran, semester, rel_kelas_det, m_siswa.Kode.ToString());
                                    decimal sikap_spiritual = GetSikapSpiritual(tahun_ajaran, semester, rel_kelas_det, m_siswa.Kode.ToString());
                                    string deskripsi_ipa_pengetahuan = "";
                                    string deskripsi_ipa_praktik = "";

                                    int nomor_mapel = 0;
                                    int urut_mapel = 0;
                                    string rel_rapornilaisiswa = "";

                                    //nilai non mulok
                                    //var lst_desain_matpel_rapor = DAO_Rapor_Desain_Det.GetAllByHeader_Entity(rapor_desain.Kode.ToString()).FindAll(m => m.Nomor.Trim() != "" && m.Rel_Mapel.Trim() != "");
                                    var lst_desain_matpel_rapor = lst_rapor_desain_det.FindAll(m => m.Nomor.Trim() != "" && m.Rel_Mapel.Trim() != "");
                                    foreach (Rapor_Desain_Det item in lst_desain_matpel_rapor)
                                    {
                                        rel_rapornilaisiswa = "";
                                        bool ada_nilai_mapel = false;

                                        if (item.Rel_Mapel.Trim() != "")
                                        {
                                            lst_nilai_siswa_det = lst_nilai_siswa_det_by_periode.FindAll(
                                                    m => m.Rel_Mapel == item.Rel_Mapel &&
                                                         m.Rel_Siswa == m.Rel_Siswa
                                                );

                                            //get struktur nilainya
                                            Rapor_StrukturNilai m_struktur_nilai = DAO_Rapor_StrukturNilai.GetATop1ByTABySMByKelasByMapel_Entity(
                                                    tahun_ajaran, semester, m_kelas.Kode.ToString(), item.Rel_Mapel
                                                );

                                            decimal nilai_rapor = 0;
                                            decimal jumlah_nilai_rapor = 0;
                                            decimal count_nilai_rapor = 0;

                                            decimal nilai_rapor_pengetahuan = 0;
                                            decimal nilai_rapor_keterampilan = 0;

                                            s_deskripsi_pengetahuan = "";
                                            s_deskripsi_keterampilan = "";

                                            if (m_struktur_nilai != null)
                                            {
                                                if (m_struktur_nilai.TahunAjaran != null)
                                                {
                                                    lst_kp_tugas.Clear();
                                                    lst_kp_uh_terakhir.Clear();
                                                    lst_kp_uh_non_terakhir.Clear();

                                                    lst_kkm_nilai_rapor.Add(new NilaiWithKey
                                                    {
                                                        Key = (jumlah_mapel_rapor + 1).ToString(),
                                                        Nilai = m_struktur_nilai.KKM
                                                    });
                                                    //-----------nilai pengetahuan---------------
                                                    id_kolom_ledger++;
                                                    //get struktur nilai det AP
                                                    List<Rapor_StrukturNilai_AP> lst_rapor_struktur_nilai_ap = DAO_Rapor_StrukturNilai_AP.GetAllByHeaderByJenisAspekPenilaian_Entity(
                                                            m_struktur_nilai.Kode.ToString(), DAO_Rapor_StrukturNilai_AP.JenisAspekPenilaian.Pengetahuan
                                                        );
                                                    lst_nilai_ap.Clear();
                                                    decimal jumlah_nilai_ap = 0;
                                                    decimal nilai_ap = 0;
                                                    int count_ap = 0;
                                                    foreach (var item_rapor_struktur_nilai_ap in lst_rapor_struktur_nilai_ap)
                                                    {

                                                        //get struktur nilai det KD
                                                        List<Rapor_StrukturNilai_KD> lst_rapor_struktur_nilai_kd = DAO_Rapor_StrukturNilai_KD.GetAllByHeader_Entity(
                                                                item_rapor_struktur_nilai_ap.Kode.ToString()
                                                            );
                                                        lst_nilai_kd.Clear();
                                                        foreach (var item_rapor_struktur_nilai_kd in lst_rapor_struktur_nilai_kd)
                                                        {
                                                            Rapor_KompetensiDasar m_kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(item_rapor_struktur_nilai_kd.Rel_Rapor_KompetensiDasar.ToString());

                                                            //get struktur nilai det KP
                                                            List<Rapor_StrukturNilai_KP> lst_rapor_struktur_nilai_kp = DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(
                                                                item_rapor_struktur_nilai_kd.Kode.ToString()
                                                            );
                                                            decimal jumlah_nilai_kd = 0;
                                                            decimal nilai_kd = 0;
                                                            int count_kd = 0;
                                                            int id_kp = 1;
                                                            foreach (var item_rapor_struktur_nilai_kp in lst_rapor_struktur_nilai_kp)
                                                            {
                                                                string nilai = "";
                                                                Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(item_rapor_struktur_nilai_kp.Rel_Rapor_KomponenPenilaian.ToString());
                                                                if (m_kp != null)
                                                                {
                                                                    Rapor_NilaiSiswa_Det m_nilai_det = lst_nilai_siswa_det.FindAll(
                                                                        m => m.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper() &&
                                                                             m.Rel_Mapel.ToString().Trim().ToUpper() == m_struktur_nilai.Rel_Mapel.ToString().Trim().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_AP.Trim().ToUpper() == item_rapor_struktur_nilai_ap.Kode.ToString().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_KD.Trim().ToUpper() == item_rapor_struktur_nilai_kd.Kode.ToString().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_KP.Trim().ToUpper() == item_rapor_struktur_nilai_kp.Kode.ToString().ToUpper()
                                                                    ).FirstOrDefault();

                                                                    if (m_nilai_det == null)
                                                                    {
                                                                        m_nilai_det = new Rapor_NilaiSiswa_Det();
                                                                        m_nilai_det.Nilai = "0";
                                                                        m_nilai_det.PB = "0";
                                                                    }
                                                                    if (m_nilai_det != null)
                                                                    {
                                                                        if (m_nilai_det.Rel_Siswa != null) rel_rapornilaisiswa = m_nilai_det.Rel_Rapor_NilaiSiswa.ToString();
                                                                        if (m_nilai_det.Nilai.Trim() == "") m_nilai_det.Nilai = "0";
                                                                        if (m_nilai_det.Nilai.Trim() != "")
                                                                        {
                                                                            s_deskripsi_pengetahuan += (s_deskripsi_pengetahuan.Trim() != "" ? "; " : "") +
                                                                                                        item_rapor_struktur_nilai_kp.Materi.Replace("<p>", "").Replace("</p>", "") +
                                                                                                        GetPredikatDeskripsi(m_nilai_det.Nilai);

                                                                            nilai = m_nilai_det.Nilai.Trim();
                                                                            if (m_nilai_det.PB == null) m_nilai_det.PB = "";
                                                                            if (item_rapor_struktur_nilai_kp.IsAdaPB && m_nilai_det.PB.Trim() != nilai && m_nilai_det.PB.Trim() != "" && Libs.IsAngka(m_nilai_det.PB.Trim().Replace("=", "")))
                                                                            {
                                                                                nilai = m_nilai_det.PB.Trim().Replace("=", "");
                                                                            }

                                                                            if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                                            {
                                                                                nilai_kd += Math.Round((item_rapor_struktur_nilai_kp.BobotNK / 100) * Libs.GetStringToDecimal(nilai), 2, MidpointRounding.AwayFromZero);
                                                                            }
                                                                            else if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                            {
                                                                                jumlah_nilai_kd += Libs.GetStringToDecimal(nilai);
                                                                            }
                                                                            count_kd++;
                                                                        }
                                                                        else
                                                                        {
                                                                            s_deskripsi_pengetahuan += (s_deskripsi_pengetahuan.Trim() != "" ? "; " : "") +
                                                                                                        item_rapor_struktur_nilai_kp.Materi.Replace("<p>", "").Replace("</p>", "") +
                                                                                                        GetPredikatDeskripsi("");
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        s_deskripsi_pengetahuan += (s_deskripsi_pengetahuan.Trim() != "" ? "; " : "") +
                                                                                                    item_rapor_struktur_nilai_kp.Materi.Replace("<p>", "").Replace("</p>", "") +
                                                                                                    GetPredikatDeskripsi("");
                                                                    }
                                                                }

                                                                if (nilai.Trim() != "")
                                                                {
                                                                    if (
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "TUGAS" ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PH") >= 0 ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENILAIAN HARIAN") >= 0 ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENGETAHUAN") >= 0
                                                                        )
                                                                    {
                                                                        lst_kp_tugas.Add(
                                                                                Libs.GetStringToDecimal(nilai)
                                                                            );
                                                                    }
                                                                    else if (
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "UH"
                                                                        )
                                                                    {
                                                                        if (id_kp < lst_rapor_struktur_nilai_kp.Count)
                                                                        {
                                                                            lst_kp_uh_non_terakhir.Add(
                                                                                    Libs.GetStringToDecimal(nilai)
                                                                                );
                                                                        }
                                                                        else if (id_kp == lst_rapor_struktur_nilai_kp.Count)
                                                                        {
                                                                            lst_kp_uh_terakhir.Add(
                                                                                    Libs.GetStringToDecimal(nilai)
                                                                                );
                                                                        }
                                                                    }
                                                                    else if (
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PTS") >= 0
                                                                        )
                                                                    {
                                                                        lst_kp_uh_non_terakhir.Add(
                                                                                Libs.GetStringToDecimal(nilai)
                                                                            );
                                                                    }
                                                                    else if (
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PAS") >= 0
                                                                        )
                                                                    {
                                                                        lst_kp_uh_terakhir.Add(
                                                                                Libs.GetStringToDecimal(nilai)
                                                                            );
                                                                    }
                                                                    else if (
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PAT") >= 0
                                                                        )
                                                                    {
                                                                        lst_kp_uh_terakhir.Add(
                                                                                Libs.GetStringToDecimal(nilai)
                                                                            );
                                                                    }
                                                                }

                                                                id_kp++;
                                                            }

                                                            if (count_kd > 0)
                                                            {
                                                                if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                {
                                                                    nilai_kd = Math.Round(jumlah_nilai_kd / count_kd, 2, MidpointRounding.AwayFromZero);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                nilai_kd = 0;
                                                            }

                                                            nilai_kd = Math.Round(nilai_kd, 2, MidpointRounding.AwayFromZero);
                                                            lst_nilai_kd.Add(new NilaiWithKey
                                                            {
                                                                Key = item_rapor_struktur_nilai_kd.Kode.ToString(),
                                                                Nilai = nilai_kd
                                                            });
                                                            //end get struktur nilai det KP

                                                        }
                                                        //end get struktur nilai det KD

                                                        //get nilai ap
                                                        count_ap = 0;
                                                        nilai_ap = 0;
                                                        jumlah_nilai_ap = 0;
                                                        foreach (var item_nilai_kd in lst_nilai_kd)
                                                        {
                                                            if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                            {
                                                                nilai_ap += Math.Round((DAO_Rapor_StrukturNilai_KD.GetByID_Entity(item_nilai_kd.Key).BobotAP / 100) * item_nilai_kd.Nilai, 2, MidpointRounding.AwayFromZero);
                                                            }
                                                            else if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                jumlah_nilai_ap += item_nilai_kd.Nilai;
                                                            }
                                                            count_ap++;
                                                        }
                                                        if (count_ap > 0)
                                                        {
                                                            if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                nilai_ap = Math.Round(jumlah_nilai_ap / count_ap, 2, MidpointRounding.AwayFromZero);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            nilai_ap = 0;
                                                        }
                                                        //pembulatan nilai ap
                                                        nilai_ap = Math.Round(nilai_ap, 2, MidpointRounding.AwayFromZero);
                                                        //end pembulatan nilai ap
                                                        lst_nilai_ap.Add(new NilaiWithKey
                                                        {
                                                            Key = item_rapor_struktur_nilai_ap.Kode.ToString(),
                                                            Nilai = nilai_ap
                                                        });
                                                        //end get nilai ap

                                                    }
                                                    //end get struktur nilai det AP

                                                    //get nilai rapor
                                                    nilai_rapor = 0;
                                                    jumlah_nilai_rapor = 0;
                                                    count_nilai_rapor = 0;
                                                    if (m_struktur_nilai.Is_PH_PTS_PAS)
                                                    {
                                                        string s_nilai_ph = "";
                                                        string s_nilai_pts = "";
                                                        string s_nilai_pas = "";

                                                        s_nilai_ph = (
                                                                lst_kp_tugas.Count > 0
                                                                ? Math.Round(lst_kp_tugas.DefaultIfEmpty().Average(), 2, MidpointRounding.AwayFromZero).ToString()
                                                                : "0"
                                                            );
                                                        s_nilai_pts = (
                                                                lst_kp_uh_non_terakhir.Count > 0
                                                                ? Math.Round(lst_kp_uh_non_terakhir.DefaultIfEmpty().Average(), 2, MidpointRounding.AwayFromZero).ToString()
                                                                : "0"
                                                            );
                                                        s_nilai_pas = (
                                                                lst_kp_uh_terakhir.Count > 0
                                                                ? Math.Round(lst_kp_uh_terakhir.DefaultIfEmpty().Average(), 2, MidpointRounding.AwayFromZero).ToString()
                                                                : "0"
                                                            );

                                                        //nilai_rapor = Math.Round((Libs.GetStringToDecimal(s_nilai_ph) * (m_struktur_nilai.BobotPH / 100)), 1, MidpointRounding.AwayFromZero) +
                                                        //              Math.Round((Libs.GetStringToDecimal(s_nilai_pts) * (m_struktur_nilai.BobotPTS / 100)), 1, MidpointRounding.AwayFromZero) +
                                                        //              Math.Round((Libs.GetStringToDecimal(s_nilai_pas) * (m_struktur_nilai.BobotPAS / 100)), 1, MidpointRounding.AwayFromZero);
                                                        //nilai_rapor = (Libs.GetStringToDecimal(s_nilai_ph) * (m_struktur_nilai.BobotPH / 100)) +
                                                        //              (Libs.GetStringToDecimal(s_nilai_pts) * (m_struktur_nilai.BobotPTS / 100)) +
                                                        //              (Libs.GetStringToDecimal(s_nilai_pas) * (m_struktur_nilai.BobotPAS / 100));
                                                        nilai_rapor = Math.Round((Libs.GetStringToDecimal(s_nilai_ph) * (m_struktur_nilai.BobotPH / 100)), 1, MidpointRounding.AwayFromZero) +
                                                                      Math.Round((Libs.GetStringToDecimal(s_nilai_pts) * (m_struktur_nilai.BobotPTS / 100)), 2, MidpointRounding.AwayFromZero) +
                                                                      Math.Round((Libs.GetStringToDecimal(s_nilai_pas) * (m_struktur_nilai.BobotPAS / 100)), 1, MidpointRounding.AwayFromZero);
                                                    }
                                                    else if (!m_struktur_nilai.Is_PH_PTS_PAS)
                                                    {
                                                        foreach (var item_nilai_ap in lst_nilai_ap)
                                                        {
                                                            if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                            {
                                                                nilai_rapor += item_nilai_ap.Nilai;
                                                            }
                                                            else if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                jumlah_nilai_rapor += item_nilai_ap.Nilai;
                                                            }
                                                            count_nilai_rapor++;
                                                        }
                                                        if (count_nilai_rapor > 0)
                                                        {
                                                            if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                nilai_rapor = Math.Round(jumlah_nilai_rapor / count_nilai_rapor, 3, MidpointRounding.AwayFromZero);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            nilai_rapor = 0;
                                                        }
                                                    }

                                                    nilai_rapor = Math.Round(nilai_rapor, 0, MidpointRounding.AwayFromZero);
                                                    nilai_rapor_pengetahuan = nilai_rapor;
                                                    lst_all_nilai_rapor.Add(new NilaiWithKey
                                                    {
                                                        Key = (id_kolom_ledger).ToString(),
                                                        Nilai = nilai_rapor_pengetahuan
                                                    });
                                                    id_kolom_ledger += 2;
                                                    //end get nilai rapor

                                                    //simpan nilai biologi or fisika pengetahuan
                                                    if (
                                                        m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_BIOLOGI ||
                                                        m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA
                                                    )
                                                    {
                                                        lst_nilai_ipa_pengetahuan.Add(
                                                                Math.Round(nilai_rapor_pengetahuan, Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero)
                                                            );
                                                        //deskripsi_ipa_pengetahuan += (deskripsi_ipa_pengetahuan.Trim() != "" && s_deskripsi_pengetahuan.Trim() != "" ? ", " : "") + s_deskripsi_pengetahuan;
                                                    }
                                                    //end simpan nilai biologi or fisika pengetahuan
                                                    //-----------end nilai pengetahuan---------------

                                                    //-----------nilai keterampilan---------------
                                                    //get struktur nilai det AP
                                                    lst_rapor_struktur_nilai_ap = DAO_Rapor_StrukturNilai_AP.GetAllByHeaderByJenisAspekPenilaian_Entity(
                                                            m_struktur_nilai.Kode.ToString(), DAO_Rapor_StrukturNilai_AP.JenisAspekPenilaian.Keterampilan
                                                        );
                                                    lst_nilai_ap.Clear();
                                                    jumlah_nilai_ap = 0;
                                                    nilai_ap = 0;
                                                    count_ap = 0;
                                                    foreach (var item_rapor_struktur_nilai_ap in lst_rapor_struktur_nilai_ap)
                                                    {

                                                        //get struktur nilai det KD
                                                        List<Rapor_StrukturNilai_KD> lst_rapor_struktur_nilai_kd = DAO_Rapor_StrukturNilai_KD.GetAllByHeader_Entity(
                                                                item_rapor_struktur_nilai_ap.Kode.ToString()
                                                            );
                                                        lst_nilai_kd.Clear();
                                                        foreach (var item_rapor_struktur_nilai_kd in lst_rapor_struktur_nilai_kd)
                                                        {

                                                            //get struktur nilai det KP
                                                            List<Rapor_StrukturNilai_KP> lst_rapor_struktur_nilai_kp = DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(
                                                                item_rapor_struktur_nilai_kd.Kode.ToString()
                                                            );
                                                            decimal jumlah_nilai_kd = 0;
                                                            decimal nilai_kd = 0;
                                                            int count_kd = 0;
                                                            foreach (var item_rapor_struktur_nilai_kp in lst_rapor_struktur_nilai_kp)
                                                            {
                                                                string nilai = "";
                                                                Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(item_rapor_struktur_nilai_kp.Rel_Rapor_KomponenPenilaian.ToString());
                                                                if (m_kp != null)
                                                                {

                                                                    Rapor_NilaiSiswa_Det m_nilai_det = lst_nilai_siswa_det.FindAll(
                                                                        m => m.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper() &&
                                                                             m.Rel_Mapel.ToString().Trim().ToUpper() == m_struktur_nilai.Rel_Mapel.ToString().Trim().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_AP.Trim().ToUpper() == item_rapor_struktur_nilai_ap.Kode.ToString().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_KD.Trim().ToUpper() == item_rapor_struktur_nilai_kd.Kode.ToString().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_KP.Trim().ToUpper() == item_rapor_struktur_nilai_kp.Kode.ToString().ToUpper()
                                                                    ).FirstOrDefault();

                                                                    if (m_nilai_det == null)
                                                                    {
                                                                        m_nilai_det = new Rapor_NilaiSiswa_Det();
                                                                        m_nilai_det.Nilai = "0";
                                                                        m_nilai_det.PB = "0";
                                                                    }
                                                                    if (m_nilai_det != null)
                                                                    {
                                                                        if (m_nilai_det.Rel_Siswa != null) rel_rapornilaisiswa = m_nilai_det.Rel_Rapor_NilaiSiswa.ToString();
                                                                        if (m_nilai_det.Nilai.Trim() == "") m_nilai_det.Nilai = "0";
                                                                        if (m_nilai_det.Nilai.Trim() != "")
                                                                        {
                                                                            s_deskripsi_keterampilan += (s_deskripsi_keterampilan.Trim() != "" ? "; " : "") +
                                                                                                        item_rapor_struktur_nilai_kp.Materi.Replace("<p>", "").Replace("</p>", "") +
                                                                                                        GetPredikatDeskripsi(m_nilai_det.Nilai);

                                                                            nilai = m_nilai_det.Nilai.Trim();
                                                                            if (m_nilai_det.PB == null) m_nilai_det.PB = "";
                                                                            if (item_rapor_struktur_nilai_kp.IsAdaPB && m_nilai_det.PB.Trim() != nilai && m_nilai_det.PB.Trim() != "" && Libs.IsAngka(m_nilai_det.PB.Trim().Replace("=", "")))
                                                                            {
                                                                                nilai = m_nilai_det.PB.Trim().Replace("=", "");
                                                                            }

                                                                            if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                                            {
                                                                                nilai_kd += Math.Round((item_rapor_struktur_nilai_kp.BobotNK / 100) * Libs.GetStringToDecimal(nilai), 2, MidpointRounding.AwayFromZero);
                                                                            }
                                                                            else if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                            {
                                                                                jumlah_nilai_kd += Libs.GetStringToDecimal(nilai);
                                                                            }
                                                                            count_kd++;
                                                                        }
                                                                        else
                                                                        {
                                                                            s_deskripsi_keterampilan += (s_deskripsi_keterampilan.Trim() != "" ? "; " : "") +
                                                                                                        item_rapor_struktur_nilai_kp.Materi.Replace("<p>", "").Replace("</p>", "") +
                                                                                                        GetPredikatDeskripsi("");
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        s_deskripsi_keterampilan += (s_deskripsi_pengetahuan.Trim() != "" ? "; " : "") +
                                                                                                    item_rapor_struktur_nilai_kp.Materi.Replace("<p>", "").Replace("</p>", "") +
                                                                                                    GetPredikatDeskripsi("");
                                                                    }
                                                                }

                                                            }

                                                            if (count_kd > 0)
                                                            {
                                                                if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                {
                                                                    nilai_kd = Math.Round(jumlah_nilai_kd / count_kd, 2, MidpointRounding.AwayFromZero);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                nilai_kd = 0;
                                                            }

                                                            nilai_kd = Math.Round(nilai_kd, Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero);
                                                            lst_nilai_kd.Add(new NilaiWithKey
                                                            {
                                                                Key = item_rapor_struktur_nilai_kd.Kode.ToString(),
                                                                Nilai = nilai_kd
                                                            });
                                                            //end get struktur nilai det KP

                                                        }
                                                        //end get struktur nilai det KD

                                                        //get nilai ap
                                                        count_ap = 0;
                                                        nilai_ap = 0;
                                                        jumlah_nilai_ap = 0;
                                                        foreach (var item_nilai_kd in lst_nilai_kd)
                                                        {
                                                            if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                            {
                                                                nilai_ap += Math.Round((DAO_Rapor_StrukturNilai_KD.GetByID_Entity(item_nilai_kd.Key).BobotAP / 100) * item_nilai_kd.Nilai, 2, MidpointRounding.AwayFromZero);
                                                            }
                                                            else if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                jumlah_nilai_ap += item_nilai_kd.Nilai;
                                                            }
                                                            count_ap++;
                                                        }
                                                        if (count_ap > 0)
                                                        {
                                                            if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                nilai_ap = Math.Round(jumlah_nilai_ap / count_ap, 2, MidpointRounding.AwayFromZero);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            nilai_ap = 0;
                                                        }
                                                        nilai_ap = Math.Round(nilai_ap, 2, MidpointRounding.AwayFromZero);
                                                        lst_nilai_ap.Add(new NilaiWithKey
                                                        {
                                                            Key = item_rapor_struktur_nilai_ap.Kode.ToString(),
                                                            Nilai = nilai_ap
                                                        });
                                                        //end get nilai ap
                                                        //get nilai rapor
                                                        nilai_rapor = 0;
                                                        jumlah_nilai_rapor = 0;
                                                        count_nilai_rapor = 0;
                                                        foreach (var item_nilai_ap in lst_nilai_ap)
                                                        {
                                                            if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                            {
                                                                nilai_rapor += item_nilai_ap.Nilai;
                                                            }
                                                            else if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                jumlah_nilai_rapor += item_nilai_ap.Nilai;
                                                            }
                                                            count_nilai_rapor++;
                                                        }
                                                        if (count_nilai_rapor > 0)
                                                        {
                                                            if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                nilai_rapor = Math.Round(jumlah_nilai_rapor / count_nilai_rapor, 3, MidpointRounding.AwayFromZero);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            nilai_rapor = 0;
                                                        }
                                                        nilai_rapor = Math.Round(nilai_rapor, 0, MidpointRounding.AwayFromZero);
                                                        nilai_rapor_keterampilan = nilai_rapor;
                                                        lst_all_nilai_rapor.Add(new NilaiWithKey
                                                        {
                                                            Key = (id_kolom_ledger).ToString(),
                                                            Nilai = nilai_rapor_keterampilan
                                                        });
                                                        //end get nilai rapor
                                                        //-----------end nilai keterampilan---------------

                                                    }
                                                    //end get struktur nilai det AP

                                                    //get nilai rapor
                                                    nilai_rapor = 0;
                                                    jumlah_nilai_rapor = 0;
                                                    count_nilai_rapor = 0;
                                                    foreach (var item_nilai_ap in lst_nilai_ap)
                                                    {
                                                        if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                        {
                                                            nilai_rapor += item_nilai_ap.Nilai;
                                                        }
                                                        else if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                        {
                                                            jumlah_nilai_rapor += item_nilai_ap.Nilai;
                                                        }
                                                        count_nilai_rapor++;
                                                    }
                                                    if (count_nilai_rapor > 0)
                                                    {
                                                        if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                        {
                                                            nilai_rapor = Math.Round(jumlah_nilai_rapor / count_nilai_rapor, 3, MidpointRounding.AwayFromZero);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        nilai_rapor = 0;
                                                    }
                                                    nilai_rapor = Math.Round(nilai_rapor, 0, MidpointRounding.AwayFromZero);
                                                    nilai_rapor_keterampilan = nilai_rapor;
                                                    lst_all_nilai_rapor.Add(new NilaiWithKey
                                                    {
                                                        Key = (id_kolom_ledger).ToString(),
                                                        Nilai = nilai_rapor_keterampilan
                                                    });
                                                    id_kolom_ledger++;
                                                    //end get nilai rapor

                                                    //simpan nilai biologi or fisika keterampilan
                                                    if (
                                                        m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_BIOLOGI ||
                                                        m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA
                                                    )
                                                    {
                                                        lst_nilai_ipa_praktik.Add(
                                                                Math.Round(nilai_rapor_keterampilan, Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero)
                                                            );
                                                        //deskripsi_ipa_praktik += (deskripsi_ipa_praktik.Trim() != "" && s_deskripsi_keterampilan.Trim() != "" ? ", " : "") + s_deskripsi_keterampilan;
                                                    }
                                                    //end simpan nilai biologi or fisika pengetahuan

                                                    jumlah_mapel_rapor++;
                                                }
                                            }
                                            //end get struktur nilai

                                            //jika fisika tambahkan satu kolom
                                            if (item.Rel_Mapel.Trim().ToUpper() == Constantas_Kode_Mapel.KODE_FISIKA.Trim().ToUpper())
                                            {
                                                lst_kkm_nilai_rapor.Add(new NilaiWithKey
                                                {
                                                    Key = (jumlah_mapel_rapor + 1).ToString(),
                                                    Nilai = -99
                                                });

                                                nilai_rapor_pengetahuan = Math.Round(lst_nilai_ipa_pengetahuan.DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero);
                                                nilai_rapor_keterampilan = Math.Round(lst_nilai_ipa_praktik.DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero);

                                                jumlah_mapel_rapor++;
                                            }

                                            string tanggal_rapor = Libs.GetTanggalIndonesiaFromDate(m_rapor_arsip.TanggalRapor, false);

                                            if (m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_BIOLOGI ||
                                                    m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA)
                                            {
                                                deskripsi_ipa_pengetahuan += (deskripsi_ipa_pengetahuan.Trim() != "" && m_struktur_nilai.DeskripsiPengetahuan.Trim() != "" ? "; " : "") + m_struktur_nilai.DeskripsiPengetahuan;
                                                deskripsi_ipa_praktik += (deskripsi_ipa_praktik.Trim() != "" && m_struktur_nilai.DeskripsiKeterampilan.Trim() != "" ? "; " : "") + m_struktur_nilai.DeskripsiKeterampilan;
                                            }

                                            //nilai sikap
                                            string s_predikat_spiritual_akhir = "";
                                            string s_predikat_sosial_akhir = "";
                                            DAO_Rapor_NilaiSikapSiswa.Rapor_NilaiSikapSiswa_Lengkap m_nilai_sikap_walas = lst_nilai_sikap_.FindAll(
                                                    m0 => m0.Rel_Mapel.ToString().ToUpper().Trim() == ""
                                                ).FirstOrDefault();
                                            var lst_sn_predikat = DAO_Rapor_StrukturNilai.GetPredikatByHeader_Entity(rel_struktur_nilai_sikap).
                                                FindAll(m0 => !(m0.Predikat.Trim() == "" && m0.Deskripsi.Trim() == "")).OrderBy(m0 => m0.Urutan).ToList();
                                            //lst_sn_predikat.
                                            var m_sn_predikat =
                                                lst_sn_predikat.FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == m_nilai_sikap_walas.SikapSpiritualAkhir.ToUpper().Trim()).FirstOrDefault();
                                            if (m_sn_predikat != null)
                                            {
                                                if (m_sn_predikat.Deskripsi != null)
                                                {
                                                    s_predikat_spiritual_akhir = m_sn_predikat.Deskripsi;
                                                }
                                            }

                                            m_sn_predikat =
                                                lst_sn_predikat.FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == m_nilai_sikap_walas.SikapSosialAkhir.ToUpper().Trim()).FirstOrDefault();
                                            if (m_sn_predikat != null)
                                            {
                                                if (m_sn_predikat.Deskripsi != null)
                                                {
                                                    s_predikat_sosial_akhir = m_sn_predikat.Deskripsi;
                                                }
                                            }
                                            //end nilai sikap

                                            if (!(m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_BIOLOGI ||
                                                    m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA))
                                            {
                                                if (nilai_rapor_pengetahuan > 0)
                                                {
                                                    ada_nilai_mapel = true;
                                                    KURTILAS_RaporNilai m_rapornilai = new KURTILAS_RaporNilai();
                                                    if (m_nilai_sikap_walas != null)
                                                    {
                                                        if (m_nilai_sikap_walas.Rel_Siswa != null)
                                                        {
                                                            //m_rapornilai.DeskripsiSpiritual = 
                                                            m_rapornilai.NilaiSpiritualAkhir = s_predikat_spiritual_akhir;
                                                            m_rapornilai.NilaiSosialAkhir = s_predikat_sosial_akhir;
                                                            m_rapornilai.DeskripsiSpiritual = deskripsi_spiritual;
                                                            m_rapornilai.DeskripsiSosial = deskripsi_sosial;
                                                        }
                                                    }

                                                    m_rapornilai.IDSiswa = m_siswa.Nama + m_siswa.Kode.ToString();
                                                    m_rapornilai.Nama = Libs.GetPerbaikiEjaanNama(m_siswa.Nama);
                                                    m_rapornilai.NIS = m_siswa.NISSekolah;
                                                    m_rapornilai.NISN = m_siswa.NISN;
                                                    m_rapornilai.NamaMapel = item.NamaMapelRapor;
                                                    m_rapornilai.TahunPelajaran = tahun_ajaran;
                                                    m_rapornilai.JenisNilai = "1.Pengetahuan";
                                                    m_rapornilai.Semester = semester;
                                                    m_rapornilai.Alamat = "";
                                                    m_rapornilai.Kelas = m_kelas_det.Nama;
                                                    m_rapornilai.NamaSekolah = "";
                                                    m_rapornilai.Nilai = Convert.ToDecimal(nilai_rapor_pengetahuan);
                                                    m_rapornilai.KKM = m_struktur_nilai.KKM;
                                                    m_rapornilai.Deskripsi = Libs.GetHTMLSimpleText2(m_struktur_nilai.DeskripsiPengetahuan);
                                                    //m_rapornilai.Deskripsi = s_deskripsi_pengetahuan;
                                                    m_rapornilai.NilaiSosial = sikap_sosial;
                                                    m_rapornilai.NilaiSpiritual = sikap_spiritual;
                                                    m_rapornilai.WaliKelas = s_walikelas;
                                                    m_rapornilai.TanggalRapor = Libs.GetTanggalIndonesiaFromDate(m_rapor_arsip.TanggalRapor, false);
                                                    m_rapornilai.KepalaSekolah = m_rapor_arsip.KepalaSekolah;
                                                    m_rapornilai.IsNaik = is_naik_kelas;
                                                    m_rapornilai.NaikKeKelas = naik_ke_kelas;
                                                    m_rapornilai.Halaman = Libs.GetStringToInteger(halaman).ToString();
                                                    m_rapornilai.TTDGuru = img_ttd_guru;
                                                    m_rapornilai.QRCode = qr_code;

                                                    //nilai program transisi
                                                    m_rapornilai.LS_JumlahJam = "";
                                                    m_rapornilai.LS_Deskripsi = "";
                                                    m_rapornilai.KW_JumlahJam = "";
                                                    m_rapornilai.KW_Deskripsi = "";
                                                    m_rapornilai.IN_JumlahJam = "";
                                                    m_rapornilai.IN_Deskripsi = "";
                                                    if (m_program_transisi != null)
                                                    {
                                                        if (m_program_transisi.Rel_Siswa != null)
                                                        {
                                                            m_rapornilai.LS_JumlahJam = m_program_transisi.LayananSosial_JumlahJam;
                                                            m_rapornilai.LS_Deskripsi = m_program_transisi.LayananSosial_Keterangan;
                                                            m_rapornilai.KW_JumlahJam = m_program_transisi.Kewirausahaan_JumlahJam;
                                                            m_rapornilai.KW_Deskripsi = m_program_transisi.Kewirausahaan_Keterangan;
                                                            m_rapornilai.IN_JumlahJam = m_program_transisi.Internship_JumlahJam;
                                                            m_rapornilai.IN_Deskripsi = m_program_transisi.Internship_Keterangan;
                                                        }
                                                    }
                                                    //end nilai program transisi

                                                    hasil.Add(m_rapornilai);
                                                }

                                                if (nilai_rapor_keterampilan > 0)
                                                {
                                                    ada_nilai_mapel = true;
                                                    KURTILAS_RaporNilai m_rapornilai = new KURTILAS_RaporNilai();
                                                    if (m_nilai_sikap_walas != null)
                                                    {
                                                        if (m_nilai_sikap_walas.Rel_Siswa != null)
                                                        {
                                                            //m_rapornilai.DeskripsiSpiritual = 
                                                            m_rapornilai.NilaiSpiritualAkhir = s_predikat_spiritual_akhir;
                                                            m_rapornilai.NilaiSosialAkhir = s_predikat_sosial_akhir;
                                                            m_rapornilai.DeskripsiSpiritual = deskripsi_spiritual;
                                                            m_rapornilai.DeskripsiSosial = deskripsi_sosial;
                                                        }
                                                    }

                                                    m_rapornilai.IDSiswa = m_siswa.Nama + m_siswa.Kode.ToString();
                                                    m_rapornilai.Nama = Libs.GetPerbaikiEjaanNama(m_siswa.Nama);
                                                    m_rapornilai.NIS = m_siswa.NISSekolah;
                                                    m_rapornilai.NISN = m_siswa.NISN;
                                                    m_rapornilai.NamaMapel = item.NamaMapelRapor;
                                                    m_rapornilai.TahunPelajaran = tahun_ajaran;
                                                    m_rapornilai.JenisNilai = "2.Keterampilan";
                                                    m_rapornilai.Semester = semester;
                                                    m_rapornilai.Alamat = "";
                                                    m_rapornilai.Kelas = m_kelas_det.Nama;
                                                    m_rapornilai.NamaSekolah = "";
                                                    m_rapornilai.Nilai = Convert.ToDecimal(nilai_rapor_keterampilan);
                                                    m_rapornilai.KKM = m_struktur_nilai.KKM;
                                                    m_rapornilai.Deskripsi = Libs.GetHTMLSimpleText2(m_struktur_nilai.DeskripsiKeterampilan);
                                                    //m_rapornilai.Deskripsi = s_deskripsi_keterampilan;
                                                    m_rapornilai.NilaiSosial = sikap_sosial;
                                                    m_rapornilai.NilaiSpiritual = sikap_spiritual;
                                                    m_rapornilai.WaliKelas = s_walikelas;
                                                    m_rapornilai.TanggalRapor = Libs.GetTanggalIndonesiaFromDate(m_rapor_arsip.TanggalRapor, false);
                                                    m_rapornilai.KepalaSekolah = m_rapor_arsip.KepalaSekolah;
                                                    m_rapornilai.IsNaik = is_naik_kelas;
                                                    m_rapornilai.NaikKeKelas = naik_ke_kelas;
                                                    m_rapornilai.Halaman = Libs.GetStringToInteger(halaman).ToString();
                                                    m_rapornilai.TTDGuru = img_ttd_guru;
                                                    m_rapornilai.QRCode = qr_code;

                                                    //nilai program transisi
                                                    m_rapornilai.LS_JumlahJam = "";
                                                    m_rapornilai.LS_Deskripsi = "";
                                                    m_rapornilai.KW_JumlahJam = "";
                                                    m_rapornilai.KW_Deskripsi = "";
                                                    m_rapornilai.IN_JumlahJam = "";
                                                    m_rapornilai.IN_Deskripsi = "";
                                                    if (m_program_transisi != null)
                                                    {
                                                        if (m_program_transisi.Rel_Siswa != null)
                                                        {
                                                            m_rapornilai.LS_JumlahJam = m_program_transisi.LayananSosial_JumlahJam;
                                                            m_rapornilai.LS_Deskripsi = m_program_transisi.LayananSosial_Keterangan;
                                                            m_rapornilai.KW_JumlahJam = m_program_transisi.Kewirausahaan_JumlahJam;
                                                            m_rapornilai.KW_Deskripsi = m_program_transisi.Kewirausahaan_Keterangan;
                                                            m_rapornilai.IN_JumlahJam = m_program_transisi.Internship_JumlahJam;
                                                            m_rapornilai.IN_Deskripsi = m_program_transisi.Internship_Keterangan;
                                                        }
                                                    }
                                                    //end nilai program transisi

                                                    hasil.Add(m_rapornilai);
                                                }
                                            }
                                            else if (m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA) //fisika
                                            {
                                                nilai_rapor_pengetahuan = Math.Round(lst_nilai_ipa_pengetahuan.DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero);
                                                nilai_rapor_keterampilan = Math.Round(lst_nilai_ipa_praktik.DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero);

                                                if (nilai_rapor_pengetahuan > 0)
                                                {
                                                    ada_nilai_mapel = true;

                                                    KURTILAS_RaporNilai m_rapornilai = new KURTILAS_RaporNilai();
                                                    if (m_nilai_sikap_walas != null)
                                                    {
                                                        if (m_nilai_sikap_walas.Rel_Siswa != null)
                                                        {
                                                            //m_rapornilai.DeskripsiSpiritual = 
                                                            m_rapornilai.NilaiSpiritualAkhir = s_predikat_spiritual_akhir;
                                                            m_rapornilai.NilaiSosialAkhir = s_predikat_sosial_akhir;
                                                            m_rapornilai.DeskripsiSpiritual = deskripsi_spiritual;
                                                            m_rapornilai.DeskripsiSosial = deskripsi_sosial;
                                                        }
                                                    }

                                                    m_rapornilai.IDSiswa = m_siswa.Nama + m_siswa.Kode.ToString();
                                                    m_rapornilai.Nama = Libs.GetPerbaikiEjaanNama(m_siswa.Nama);
                                                    m_rapornilai.NIS = m_siswa.NISSekolah;
                                                    m_rapornilai.NISN = m_siswa.NISN;
                                                    m_rapornilai.NamaMapel = "Ilmu Pengetahuan Alam";
                                                    m_rapornilai.TahunPelajaran = tahun_ajaran;
                                                    m_rapornilai.JenisNilai = "1.Pengetahuan";
                                                    m_rapornilai.Semester = semester;
                                                    m_rapornilai.Alamat = "";
                                                    m_rapornilai.Kelas = m_kelas_det.Nama;
                                                    m_rapornilai.NamaSekolah = "";
                                                    m_rapornilai.Nilai = Convert.ToDecimal(nilai_rapor_pengetahuan);
                                                    m_rapornilai.KKM = m_struktur_nilai.KKM;
                                                    m_rapornilai.Deskripsi = Libs.GetHTMLSimpleText2(deskripsi_ipa_pengetahuan);
                                                    m_rapornilai.NilaiSosial = sikap_sosial;
                                                    m_rapornilai.NilaiSpiritual = sikap_spiritual;
                                                    m_rapornilai.TanggalRapor = Libs.GetTanggalIndonesiaFromDate(m_rapor_arsip.TanggalRapor, false);
                                                    m_rapornilai.KepalaSekolah = m_rapor_arsip.KepalaSekolah;
                                                    m_rapornilai.IsNaik = is_naik_kelas;
                                                    m_rapornilai.NaikKeKelas = naik_ke_kelas;
                                                    m_rapornilai.Halaman = Libs.GetStringToInteger(halaman).ToString();
                                                    m_rapornilai.TTDGuru = img_ttd_guru;
                                                    m_rapornilai.QRCode = qr_code;

                                                    //nilai program transisi
                                                    m_rapornilai.LS_JumlahJam = "";
                                                    m_rapornilai.LS_Deskripsi = "";
                                                    m_rapornilai.KW_JumlahJam = "";
                                                    m_rapornilai.KW_Deskripsi = "";
                                                    m_rapornilai.IN_JumlahJam = "";
                                                    m_rapornilai.IN_Deskripsi = "";
                                                    if (m_program_transisi != null)
                                                    {
                                                        if (m_program_transisi.Rel_Siswa != null)
                                                        {
                                                            m_rapornilai.LS_JumlahJam = m_program_transisi.LayananSosial_JumlahJam;
                                                            m_rapornilai.LS_Deskripsi = m_program_transisi.LayananSosial_Keterangan;
                                                            m_rapornilai.KW_JumlahJam = m_program_transisi.Kewirausahaan_JumlahJam;
                                                            m_rapornilai.KW_Deskripsi = m_program_transisi.Kewirausahaan_Keterangan;
                                                            m_rapornilai.IN_JumlahJam = m_program_transisi.Internship_JumlahJam;
                                                            m_rapornilai.IN_Deskripsi = m_program_transisi.Internship_Keterangan;
                                                        }
                                                    }
                                                    //end nilai program transisi

                                                    hasil.Add(m_rapornilai);
                                                }

                                                if (nilai_rapor_keterampilan > 0)
                                                {
                                                    ada_nilai_mapel = true;
                                                    KURTILAS_RaporNilai m_rapornilai = new KURTILAS_RaporNilai();
                                                    if (m_nilai_sikap_walas != null)
                                                    {
                                                        if (m_nilai_sikap_walas.Rel_Siswa != null)
                                                        {
                                                            //m_rapornilai.DeskripsiSpiritual = 
                                                            m_rapornilai.NilaiSpiritualAkhir = s_predikat_spiritual_akhir;
                                                            m_rapornilai.NilaiSosialAkhir = s_predikat_sosial_akhir;
                                                            m_rapornilai.DeskripsiSpiritual = deskripsi_spiritual;
                                                            m_rapornilai.DeskripsiSosial = deskripsi_sosial;
                                                        }
                                                    }

                                                    m_rapornilai.IDSiswa = m_siswa.Nama + m_siswa.Kode.ToString();
                                                    m_rapornilai.Nama = Libs.GetPerbaikiEjaanNama(m_siswa.Nama);
                                                    m_rapornilai.NIS = m_siswa.NISSekolah;
                                                    m_rapornilai.NISN = m_siswa.NISN;
                                                    m_rapornilai.NamaMapel = "Ilmu Pengetahuan Alam";
                                                    m_rapornilai.TahunPelajaran = tahun_ajaran;
                                                    m_rapornilai.JenisNilai = "2.Keterampilan";
                                                    m_rapornilai.Semester = semester;
                                                    m_rapornilai.Alamat = "";
                                                    m_rapornilai.Kelas = m_kelas_det.Nama;
                                                    m_rapornilai.NamaSekolah = "";
                                                    m_rapornilai.Nilai = Convert.ToDecimal(nilai_rapor_keterampilan);
                                                    m_rapornilai.KKM = m_struktur_nilai.KKM;
                                                    m_rapornilai.Deskripsi = Libs.GetHTMLSimpleText2(deskripsi_ipa_praktik);
                                                    m_rapornilai.NilaiSosial = sikap_sosial;
                                                    m_rapornilai.NilaiSpiritual = sikap_spiritual;
                                                    m_rapornilai.TanggalRapor = Libs.GetTanggalIndonesiaFromDate(m_rapor_arsip.TanggalRapor, false);
                                                    m_rapornilai.KepalaSekolah = m_rapor_arsip.KepalaSekolah;
                                                    m_rapornilai.IsNaik = is_naik_kelas;
                                                    m_rapornilai.NaikKeKelas = naik_ke_kelas;
                                                    m_rapornilai.Halaman = Libs.GetStringToInteger(halaman).ToString();
                                                    m_rapornilai.TTDGuru = img_ttd_guru;
                                                    m_rapornilai.QRCode = qr_code;

                                                    //nilai program transisi
                                                    m_rapornilai.LS_JumlahJam = "";
                                                    m_rapornilai.LS_Deskripsi = "";
                                                    m_rapornilai.KW_JumlahJam = "";
                                                    m_rapornilai.KW_Deskripsi = "";
                                                    m_rapornilai.IN_JumlahJam = "";
                                                    m_rapornilai.IN_Deskripsi = "";
                                                    if (m_program_transisi != null)
                                                    {
                                                        if (m_program_transisi.Rel_Siswa != null)
                                                        {
                                                            m_rapornilai.LS_JumlahJam = m_program_transisi.LayananSosial_JumlahJam;
                                                            m_rapornilai.LS_Deskripsi = m_program_transisi.LayananSosial_Keterangan;
                                                            m_rapornilai.KW_JumlahJam = m_program_transisi.Kewirausahaan_JumlahJam;
                                                            m_rapornilai.KW_Deskripsi = m_program_transisi.Kewirausahaan_Keterangan;
                                                            m_rapornilai.IN_JumlahJam = m_program_transisi.Internship_JumlahJam;
                                                            m_rapornilai.IN_Deskripsi = m_program_transisi.Internship_Keterangan;
                                                        }
                                                    }
                                                    //end nilai program transisi

                                                    hasil.Add(m_rapornilai);
                                                }
                                            }
                                            //get nilai rapor kurtilas

                                            //end jika fisika  
                                            else if (m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_BIOLOGI) //biologi
                                            {
                                                ada_nilai_mapel = true;
                                            }                                             
                                        }

                                        //capaian kedisiplinan
                                        Rapor_NilaiSiswa m_nilaisiswa = lst_nilaisiswa.FindAll(
                                                    m0 => m0.Kode.ToString().ToUpper().Trim() == rel_rapornilaisiswa.ToUpper().Trim()
                                                ).FirstOrDefault();
                                        bool ada_nilai = false;
                                        RaporLTSCapaianKedisiplinan m_rapor_capaian_kedisiplinan = new RaporLTSCapaianKedisiplinan();

                                        if (m_nilaisiswa != null && ada_nilai_mapel)
                                        {
                                            if (m_nilaisiswa.Rel_Siswa != null)
                                            {
                                                ada_nilai = true;
                                                urut_mapel++;
                                                if (item.Poin.Trim() == "") nomor_mapel++;

                                                m_rapor_capaian_kedisiplinan.Rel_Siswa = m_siswa.Nama + m_siswa.Kode.ToString();
                                                m_rapor_capaian_kedisiplinan.KodeKelompokMapel = "";
                                                m_rapor_capaian_kedisiplinan.KelompokMapel = "";
                                                m_rapor_capaian_kedisiplinan.NomorMapel = item.Nomor;
                                                m_rapor_capaian_kedisiplinan.Rel_Mapel = item.Rel_Mapel;
                                                m_rapor_capaian_kedisiplinan.NamaMapel = (item.Poin.Trim() != "" ? item.Poin + " " : "") + item.NamaMapelRapor;
                                                if (tahun_ajaran == "2020/2021" && semester == "2")
                                                {
                                                    m_rapor_capaian_kedisiplinan.Kehadiran = m_nilaisiswa.LTS_CK_KEHADIRAN;
                                                    m_rapor_capaian_kedisiplinan.KetepatanWaktu = m_nilaisiswa.LTS_CK_KETEPATAN_WKT;
                                                    m_rapor_capaian_kedisiplinan.PenggunaanSeragam = m_nilaisiswa.LTS_CK_PENGGUNAAN_SRGM;
                                                    m_rapor_capaian_kedisiplinan.PenggunaanKamera = m_nilaisiswa.LTS_CK_PENGGUNAAN_KMR;
                                                }
                                                else
                                                {
                                                    m_rapor_capaian_kedisiplinan.Kehadiran = m_nilaisiswa.SM_CK_KEHADIRAN;
                                                    m_rapor_capaian_kedisiplinan.KetepatanWaktu = m_nilaisiswa.SM_CK_KETEPATAN_WKT;
                                                    m_rapor_capaian_kedisiplinan.PenggunaanSeragam = m_nilaisiswa.SM_CK_PENGGUNAAN_SRGM;
                                                    m_rapor_capaian_kedisiplinan.PenggunaanKamera = m_nilaisiswa.SM_CK_PENGGUNAAN_KMR;
                                                }
                                                m_rapor_capaian_kedisiplinan.UrutanMapel = urut_mapel;
                                                ListRaporLTSCapaianKedisiplinan.Add(m_rapor_capaian_kedisiplinan);
                                            }
                                        }
                                        if (!ada_nilai)
                                        {
                                            ada_nilai = true;

                                            urut_mapel++;
                                            if (item.Poin.Trim() == "") nomor_mapel++;

                                            m_rapor_capaian_kedisiplinan.Rel_Siswa = m_siswa.Nama + m_siswa.Kode.ToString();
                                            m_rapor_capaian_kedisiplinan.KodeKelompokMapel = "";
                                            m_rapor_capaian_kedisiplinan.KelompokMapel = "";
                                            m_rapor_capaian_kedisiplinan.NomorMapel = item.Nomor;
                                            m_rapor_capaian_kedisiplinan.Rel_Mapel = item.Rel_Mapel;
                                            m_rapor_capaian_kedisiplinan.NamaMapel = (item.Poin.Trim() != "" ? item.Poin + " " : "") +
                                                                                         (
                                                                                            item.Rel_Mapel.Trim() == ""
                                                                                            ? item.NamaMapelRapor.Replace(":", "") + ":"
                                                                                            : item.NamaMapelRapor
                                                                                         );
                                            m_rapor_capaian_kedisiplinan.Kehadiran = "";
                                            m_rapor_capaian_kedisiplinan.KetepatanWaktu = "";
                                            m_rapor_capaian_kedisiplinan.PenggunaanSeragam = "";
                                            m_rapor_capaian_kedisiplinan.PenggunaanKamera = "";
                                            m_rapor_capaian_kedisiplinan.UrutanMapel = urut_mapel;
                                            ListRaporLTSCapaianKedisiplinan.Add(m_rapor_capaian_kedisiplinan);
                                        }
                                        //end capaian kedisiplinan
                                    }
                                    //end rapor non mulok

                                    lst_jumlah_nilai_keseluruhan.Add(new NilaiWithKey
                                    {
                                        Key = m_siswa.Kode.ToString(),
                                        Nilai = jumlah_keseluruhan
                                    });

                                    ////absen
                                    //string s_sakit = "-";
                                    //string s_izin = "-";
                                    //string s_alpa = "-";
                                    //string s_terlambat = "-";

                                    //List<DAO_SiswaAbsen.AbsenRekapRapor> lst_absen = new List<DAO_SiswaAbsen.AbsenRekapRapor>();
                                    //lst_absen = DAO_SiswaAbsen.GetRekapAbsenRaporBySiswaByPeriode_Entity(
                                    //        m_siswa.Kode.ToString(),
                                    //        (m_rapor_arsip != null ? m_rapor_arsip.TanggalAwalAbsen : DateTime.MinValue),
                                    //        (m_rapor_arsip != null ? m_rapor_arsip.TanggalAkhirAbsen : DateTime.MinValue)
                                    //    );
                                    //foreach (var absen in lst_absen)
                                    //{
                                    //    if (absen.Absen == Libs.JENIS_ABSENSI.SAKIT.Substring(0, 1)) s_sakit = absen.Jumlah.ToString();
                                    //    if (absen.Absen == Libs.JENIS_ABSENSI.IZIN.Substring(0, 1)) s_izin = absen.Jumlah.ToString();
                                    //    if (absen.Absen == Libs.JENIS_ABSENSI.ALPA.Substring(0, 1)) s_alpa = absen.Jumlah.ToString();
                                    //    if (absen.Absen == Libs.JENIS_ABSENSI.TERLAMBAT.Substring(0, 1)) s_terlambat = absen.Jumlah.ToString();
                                    //}
                                    ////end absen

                                    nomor++;
                                }
                                // end foreach siswa

                                for (int i = 1;
                                     i <= (DAO_Rapor_Desain_Det.GetAllByHeader_Entity(rapor_desain.Kode.ToString()).FindAll(m => m.Nomor.Trim() != "" && m.Rel_Mapel.Trim() != "").Count * 4) + (4);
                                     i++)
                                {
                                    decimal rata_rata = Math.Round(lst_all_nilai_rapor.FindAll(m => m.Key == (i + jumlah_awal_col_ledger).ToString()).Select(m => m.Nilai).DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP);
                                }
                                for (int i = 1; i <= (DAO_Rapor_Desain_Det.GetAllByHeader_Entity(rapor_desain.Kode.ToString()).FindAll(m => m.Nomor.Trim() != "" && m.Rel_Mapel.Trim() != "").Count + 1); i++)
                                {
                                    decimal kkm = 0;
                                    NilaiWithKey m_kkm = lst_kkm_nilai_rapor.FindAll(m => m.Key == i.ToString()).FirstOrDefault();
                                    if (m_kkm != null)
                                    {
                                        if (m_kkm.Key != null)
                                        {
                                            kkm = m_kkm.Nilai;
                                        }
                                    }
                                }
                                decimal rata_rata_nilai_keseluruhan = Math.Round(lst_jumlah_nilai_keseluruhan.Select(m => m.Nilai).DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP);
                                decimal rata_rata_nilai_rata_rata_rapor = Math.Round(lst_nilai_rapor.Select(m => m.Nilai).DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP);

                            }

                        }
                    }

                }
            }

            return hasil;
        }

        public static List<KTSP_RaporEkskul> GetNilaiEkskul_Entity(string tahun_ajaran, string semester, string rel_kelas_det, string rel_siswa)
        {
            List<KTSP_RaporEkskul> hasil = new List<KTSP_RaporEkskul>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SMP_Rapor_Nilai_SELECT_EKSKUL_BY_SISWA";
                comm.Parameters.AddWithValue("@TahunAjaran", tahun_ajaran);
                comm.Parameters.AddWithValue("@Semester", semester);
                comm.Parameters.AddWithValue("@Rel_KelasDet", rel_kelas_det);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    if (Convert.ToDecimal(row["Prestasi"]) > 0 || row["LTS_CK_KEHADIRAN"].ToString().Trim() != "")
                    {
                        if (
                                rel_siswa.Trim().ToUpper().IndexOf(row["Rel_Siswa"].ToString().ToUpper() + ";") >= 0 ||
                                (rel_siswa.IndexOf(";") < 0 && rel_siswa.Trim().ToUpper() == row["Rel_Siswa"].ToString().ToUpper())
                            )
                        {
                            KTSP_RaporEkskul m = new KTSP_RaporEkskul();
                            m.Rel_Siswa = row["Rel_Siswa"].ToString();
                            m.IDSiswa = row["NamaSiswa"].ToString() + row["Rel_Siswa"].ToString();
                            m.Kegiatan = row["Kegiatan"].ToString();
                            m.Prestasi = Convert.ToDecimal(row["Prestasi"]);
                            m.UraianKompetensi = Libs.GetHTMLSimpleText2(row["UraianKompetensi"].ToString());
                            m.Group = row["Group"].ToString();
                            m.Sakit = row["Sakit"].ToString();
                            m.Izin = row["Izin"].ToString();
                            m.Alpa = row["Alpa"].ToString();
                            m.LTS_CK_KEHADIRAN = (row["LTS_CK_KEHADIRAN"].ToString().Trim() == "" || row["LTS_CK_KEHADIRAN"].ToString().Trim() == "0" ? "-" : row["LTS_CK_KEHADIRAN"].ToString());
                            m.LTS_CK_KETEPATAN_WKT = (row["LTS_CK_KETEPATAN_WKT"].ToString().Trim() == "" || row["LTS_CK_KETEPATAN_WKT"].ToString().Trim() == "0" ? "-" : row["LTS_CK_KETEPATAN_WKT"].ToString());
                            m.LTS_CK_PENGGUNAAN_SRGM = (row["LTS_CK_PENGGUNAAN_SRGM"].ToString().Trim() == "" || row["LTS_CK_PENGGUNAAN_SRGM"].ToString().Trim() == "0" ? "-" : row["LTS_CK_PENGGUNAAN_SRGM"].ToString());
                            m.LTS_CK_PENGGUNAAN_KMR = (row["LTS_CK_PENGGUNAAN_KMR"].ToString().Trim() == "" || row["LTS_CK_PENGGUNAAN_KMR"].ToString().Trim() == "0" ? "-" : row["LTS_CK_PENGGUNAAN_KMR"].ToString());
                            m.SM_CK_KEHADIRAN = (row["SM_CK_KEHADIRAN"].ToString().Trim() == "" || row["SM_CK_KEHADIRAN"].ToString().Trim() == "0" ? "-" : row["SM_CK_KEHADIRAN"].ToString());
                            m.SM_CK_KETEPATAN_WKT = (row["SM_CK_KETEPATAN_WKT"].ToString().Trim() == "" || row["SM_CK_KETEPATAN_WKT"].ToString().Trim() == "0" ? "-" : row["SM_CK_KETEPATAN_WKT"].ToString());
                            m.SM_CK_PENGGUNAAN_SRGM = (row["SM_CK_PENGGUNAAN_SRGM"].ToString().Trim() == "" || row["SM_CK_PENGGUNAAN_SRGM"].ToString().Trim() == "0" ? "-" : row["SM_CK_PENGGUNAAN_SRGM"].ToString());
                            m.SM_CK_PENGGUNAAN_KMR = (row["SM_CK_PENGGUNAAN_KMR"].ToString().Trim() == "" || row["SM_CK_PENGGUNAAN_KMR"].ToString().Trim() == "0" ? "-" : row["SM_CK_PENGGUNAAN_KMR"].ToString());

                            hasil.Add(m);
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

        public static List<KTSP_RaporVolunteer> GetVolunteer(
                string tahun_ajaran,
                string semester,
                string rel_kelas_det,
                string rel_siswa
            )
        {
            List<KTSP_RaporVolunteer> hasil = new List<KTSP_RaporVolunteer>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SMP_Volunteer_SELECT_BY_TA_BY_SM_BY_KELASDET";
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelas_det);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    if (rel_siswa.Trim().ToUpper().IndexOf(row["Rel_Siswa"].ToString().ToUpper() + ";") >= 0)
                    {
                        Siswa m_siswa = DAO_Siswa.GetByKode_Entity(
                            tahun_ajaran,
                            semester,
                            row["Rel_Siswa"].ToString());
                        if (m_siswa != null)
                        {
                            if (m_siswa.Nama != null)
                            {
                                hasil.Add(new KTSP_RaporVolunteer
                                {
                                    IDSiswa = m_siswa.Nama + m_siswa.Kode.ToString(),
                                    Kegiatan = Libs.GetHTMLSimpleText(row["Kegiatan"].ToString()),
                                    JumlahJam = row["JumlahJam"].ToString(),
                                    TanggalKegiatan = Convert.ToDateTime(row["TanggalKegiatan"]),
                                    Keterangan = row["Keterangan"].ToString()
                                });
                            }
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

        public static List<KTSP_RaporCatatan> GetCatatan(
                string tahun_ajaran,
                string semester,
                string rel_kelas_det,
                string rel_siswa
            )
        {
            List<KTSP_RaporCatatan> hasil = new List<KTSP_RaporCatatan>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SMP_Rapor_CatatanSiswa_SELECT_BY_TA_BY_SM_BY_KELASDET";
                comm.Parameters.AddWithValue("@" + NamaField.TahunAjaran, tahun_ajaran);
                comm.Parameters.AddWithValue("@" + NamaField.Semester, semester);
                comm.Parameters.AddWithValue("@" + NamaField.Rel_KelasDet, rel_kelas_det);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    if (rel_siswa.Trim().ToUpper().IndexOf(row["Rel_Siswa"].ToString().Trim().ToUpper() + ";") >= 0)
                    {
                        Siswa m_siswa = DAO_Siswa.GetByKode_Entity(
                            tahun_ajaran,
                            semester,
                            row["Rel_Siswa"].ToString());
                        if (m_siswa != null)
                        {
                            if (m_siswa.Nama != null)
                            {
                                hasil.Add(new KTSP_RaporCatatan
                                {
                                    //IDSiswa = m_siswa.Nama.Trim().ToUpper().Trim() + m_siswa.Kode.ToString(),
                                    IDSiswa = m_siswa.Nama + m_siswa.Kode.ToString(),
                                    Catatan = row["Catatan"].ToString()
                                });
                            }
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

        public static List<KTSP_RaporKetidakhadiran> GetNilaiKetidakhadiran_Entity(string tahun_ajaran, string semester, string rel_kelas_det, string rel_siswa)
        {
            List<KTSP_RaporKetidakhadiran> hasil = new List<KTSP_RaporKetidakhadiran>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_Rapor();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
                if (m_kelas_det != null)
                {
                    if (m_kelas_det.Nama != null)
                    {

                        Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());
                        if (m_kelas != null)
                        {
                            if (m_kelas.Nama != null)
                            {
                                List<DAO_Siswa.SiswaDataSimple> lst_siswa = DAO_Siswa.GetAllSiswaDataSimpleByTahunAjaranUnitKelas_Entity(
                                        m_kelas.Rel_Sekolah.ToString(),
                                        rel_kelas_det,
                                        tahun_ajaran,
                                        semester
                                    );

                                //rekap absensi walas
                                List<SiswaAbsenRekap> lst_absen_rekap = DAO_SiswaAbsenRekap.GetAllByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det).FindAll(m0 => m0.Rel_Mapel.Trim() == "" && m0.Jenis.ToString().ToUpper() == TipeRapor.SEMESTER.ToUpper().Trim());
                                List<SiswaAbsenRekapDet> lst_rekap_det = new List<SiswaAbsenRekapDet>();
                                if (lst_absen_rekap.Count == 1)
                                {
                                    SiswaAbsenRekap m_rekap_absensi = lst_absen_rekap.FirstOrDefault();
                                    if (m_rekap_absensi != null)
                                    {
                                        if (m_rekap_absensi.TahunAjaran != null)
                                        {
                                            lst_rekap_det = DAO_SiswaAbsenRekapDet.GetAllByHeader_Entity(m_rekap_absensi.Kode.ToString());
                                        }
                                    }
                                }
                                //end rekap absensi walas

                                if (rel_siswa.Trim() != "") lst_siswa = lst_siswa.FindAll(m => rel_siswa.Trim().ToUpper().IndexOf(m.Kode.ToString().Trim().ToUpper() + ";") >= 0);

                                foreach (DAO_Siswa.SiswaDataSimple m_siswa in lst_siswa)
                                {
                                    Rapor_Arsip m_rapor_arsip = DAO_Rapor_Arsip.GetAll_Entity().FindAll(
                                    m0 => m0.TahunAjaran == tahun_ajaran &&
                                          m0.Semester == semester &&
                                          m0.JenisRapor == "Semester").FirstOrDefault();

                                    string s_sakit = "-";
                                    string s_izin = "-";
                                    string s_alpa = "-";
                                    string s_terlambat = "-";

                                    List<DAO_SiswaAbsen.AbsenRekapRapor> lst_absen = new List<DAO_SiswaAbsen.AbsenRekapRapor>();
                                    if (m_rapor_arsip != null)
                                    {
                                        lst_absen = DAO_SiswaAbsen.GetRekapAbsenRaporBySiswaByPeriode_Entity(
                                                m_siswa.Kode.ToString(),
                                                (m_rapor_arsip != null ? m_rapor_arsip.TanggalAwalAbsen : DateTime.MinValue),
                                                (m_rapor_arsip != null ? m_rapor_arsip.TanggalAkhirAbsen : DateTime.MinValue)
                                            );
                                    }
                                    foreach (var absen in lst_absen)
                                    {
                                        if (absen.Absen == Libs.JENIS_ABSENSI.SAKIT.Substring(0, 1)) s_sakit = absen.Jumlah.ToString();
                                        if (absen.Absen == Libs.JENIS_ABSENSI.IZIN.Substring(0, 1)) s_izin = absen.Jumlah.ToString();
                                        if (absen.Absen == Libs.JENIS_ABSENSI.ALPA.Substring(0, 1)) s_alpa = absen.Jumlah.ToString();
                                        if (absen.Absen == Libs.JENIS_ABSENSI.TERLAMBAT.Substring(0, 1)) s_terlambat = absen.Jumlah.ToString();
                                    }

                                    if (tahun_ajaran == "2020/2021")
                                    {
                                        if (lst_rekap_det.FindAll(m0 => m0.Rel_Siswa.ToString().Replace(";", "").ToUpper() == m_siswa.Kode.ToString().Replace(";", "").ToUpper().Trim()).Count > 0)
                                        {
                                            SiswaAbsenRekapDet m_rekap_absen_siswa = lst_rekap_det.FindAll(m0 => m0.Rel_Siswa.ToString().Replace(";", "").ToUpper() == m_siswa.Kode.ToString().Replace(";", "").ToUpper().Trim()).FirstOrDefault();
                                            if (m_rekap_absen_siswa != null)
                                            {
                                                if (m_rekap_absen_siswa.Rel_Siswa != null)
                                                {
                                                    s_sakit = m_rekap_absen_siswa.Sakit;
                                                    s_izin = m_rekap_absen_siswa.Izin;
                                                    s_alpa = m_rekap_absen_siswa.Alpa;
                                                    s_terlambat = m_rekap_absen_siswa.Terlambat;
                                                }
                                            }
                                        }
                                    }

                                    string s_kelakuan = "";
                                    string s_kerajinan = "";
                                    string s_kerapihan = "";

                                    List<Rapor_Kepribadian> lst_kepribadian = DAO_Rapor_Kepribadian.GetAllByTABySMByKelasDetSiswa_Entity(tahun_ajaran, semester, rel_kelas_det, m_siswa.Kode.ToString());
                                    Rapor_Kepribadian m_kepribadian = lst_kepribadian.FirstOrDefault();
                                    if (m_kepribadian != null)
                                    {
                                        if (m_kepribadian.TahunAjaran != null)
                                        {
                                            s_kelakuan = m_kepribadian.Kelakuan;
                                            s_kerajinan = m_kepribadian.Kerajinan;
                                            s_kerapihan = m_kepribadian.Kerapihan;
                                        }
                                    }

                                    hasil.Add(
                                            new KTSP_RaporKetidakhadiran
                                            {
                                                IDSiswa = m_siswa.Nama + m_siswa.Kode.ToString(),
                                                Alpa = s_alpa,
                                                Izin = s_izin,
                                                Sakit = s_sakit,
                                                Kelakuan = s_kelakuan,
                                                Kerajinan = s_kerajinan,
                                                Kerapihan = s_kerapihan
                                            }
                                        );
                                }
                            }
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

        public static List<KTSP_DeskripsiRapor> GetDeskripsiRapor_KTSP(
            string tahun_ajaran,
            string semester,
            string rel_kelas_det,
            string rel_siswa
            )
        {
            List<KTSP_DeskripsiRapor> hasil = new List<KTSP_DeskripsiRapor>();

            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {

                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());
                    if (m_kelas != null)
                    {
                        if (m_kelas.Nama != null)
                        {

                            Rapor_Desain rapor_desain = DAO_Rapor_Desain.GetByTABySMByKelas_Entity(
                                    tahun_ajaran, semester, rel_kelas_det, DAO_Rapor_Desain.JenisRapor.Semester
                                ).FirstOrDefault();
                            List<Rapor_Desain_Det> lst_rapor_desain_det = DAO_Rapor_Desain_Det.GetAllByHeader_Entity(rapor_desain.Kode.ToString());

                            if (rapor_desain != null)
                            {

                                List<DAO_Siswa.SiswaDataSimple> lst_siswa = DAO_Siswa.GetAllSiswaDataSimpleByTahunAjaranUnitKelas_Entity(
                                        m_kelas.Rel_Sekolah.ToString(),
                                        rel_kelas_det,
                                        tahun_ajaran,
                                        semester
                                    );
                                List<NilaiWithKey> lst_nilai_ap = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_nilai_kd = new List<NilaiWithKey>();
                                List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT> lst_nilai_siswa_det = new List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT>();
                                List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT> lst_nilai_siswa_det_by_periode = new List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT>();

                                lst_nilai_siswa_det_by_periode = DAO_Rapor_NilaiSiswa_Det.GetAllByTABySMByKelasDet_Entity(
                                        tahun_ajaran, semester, rel_kelas_det
                                    );

                                int nomor = 1;
                                if (rel_siswa.Trim() != "") lst_siswa = lst_siswa.FindAll(m => rel_siswa.Trim().ToUpper().IndexOf(m.Kode.ToString().ToUpper() + ";") >= 0);
                                List<NilaiWithKey> lst_all_nilai_rapor = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_kkm_nilai_rapor = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_jumlah_nilai_keseluruhan = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_nilai_rapor = new List<NilaiWithKey>();
                                decimal jumlah_keseluruhan = 0;
                                decimal jumlah_mapel_rapor = 0;
                                foreach (DAO_Siswa.SiswaDataSimple m_siswa in lst_siswa.OrderBy(m => m.Nama))
                                {
                                    jumlah_keseluruhan = 0;
                                    jumlah_mapel_rapor = 0;

                                    lst_kkm_nilai_rapor.Clear();
                                    int id_rapor = 0;
                                    List<decimal> lst_nilai_ipa = new List<decimal>();
                                    lst_nilai_ipa.Clear();
                                    string deskripsi_ipa = "";
                                    foreach (Rapor_Desain_Det item in lst_rapor_desain_det.FindAll(m => m.Rel_Mapel.Trim() != ""))
                                    {

                                        if (item.Rel_Mapel.Trim() != "")
                                        {

                                            lst_nilai_siswa_det = lst_nilai_siswa_det_by_periode.FindAll(
                                                    m => m.Rel_Mapel == item.Rel_Mapel &&
                                                         m.Rel_Siswa == m.Rel_Siswa
                                                );

                                            //get struktur nilainya
                                            Rapor_StrukturNilai m_struktur_nilai = DAO_Rapor_StrukturNilai.GetATop1ByTABySMByKelasByMapel_Entity(
                                                    tahun_ajaran, semester, m_kelas.Kode.ToString(), item.Rel_Mapel
                                                );

                                            decimal nilai_rapor = 0;
                                            decimal jumlah_nilai_rapor = 0;
                                            decimal count_nilai_rapor = 0;
                                            if (m_struktur_nilai != null)
                                            {
                                                if (m_struktur_nilai.TahunAjaran != null)
                                                {

                                                    lst_kkm_nilai_rapor.Add(new NilaiWithKey
                                                    {
                                                        Key = (jumlah_mapel_rapor + 1).ToString(),
                                                        Nilai = m_struktur_nilai.KKM
                                                    });

                                                    //get struktur nilai det AP
                                                    List<Rapor_StrukturNilai_AP> lst_rapor_struktur_nilai_ap = DAO_Rapor_StrukturNilai_AP.GetAllByHeader_Entity(
                                                            m_struktur_nilai.Kode.ToString()
                                                        );
                                                    lst_nilai_ap.Clear();
                                                    decimal jumlah_nilai_ap = 0;
                                                    decimal nilai_ap = 0;
                                                    int count_ap = 0;
                                                    foreach (var item_rapor_struktur_nilai_ap in lst_rapor_struktur_nilai_ap)
                                                    {

                                                        //get struktur nilai det KD
                                                        List<Rapor_StrukturNilai_KD> lst_rapor_struktur_nilai_kd = DAO_Rapor_StrukturNilai_KD.GetAllByHeader_Entity(
                                                                item_rapor_struktur_nilai_ap.Kode.ToString()
                                                            );
                                                        lst_nilai_kd.Clear();
                                                        foreach (var item_rapor_struktur_nilai_kd in lst_rapor_struktur_nilai_kd)
                                                        {

                                                            //get struktur nilai det KP
                                                            List<Rapor_StrukturNilai_KP> lst_rapor_struktur_nilai_kp = DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(
                                                                item_rapor_struktur_nilai_kd.Kode.ToString()
                                                            );
                                                            decimal jumlah_nilai_kd = 0;
                                                            decimal nilai_kd = 0;
                                                            int count_kd = 0;
                                                            foreach (var item_rapor_struktur_nilai_kp in lst_rapor_struktur_nilai_kp)
                                                            {
                                                                string nilai = "";
                                                                Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(item_rapor_struktur_nilai_kp.Rel_Rapor_KomponenPenilaian.ToString());
                                                                if (m_kp != null)
                                                                {

                                                                    Rapor_NilaiSiswa_Det m_nilai_det = lst_nilai_siswa_det.FindAll(
                                                                        m => m.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper() &&
                                                                             m.Rel_Mapel.ToString().Trim().ToUpper() == m_struktur_nilai.Rel_Mapel.ToString().Trim().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_AP.Trim().ToUpper() == item_rapor_struktur_nilai_ap.Kode.ToString().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_KD.Trim().ToUpper() == item_rapor_struktur_nilai_kd.Kode.ToString().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_KP.Trim().ToUpper() == item_rapor_struktur_nilai_kp.Kode.ToString().ToUpper()
                                                                    ).FirstOrDefault();

                                                                    if (m_nilai_det != null)
                                                                    {
                                                                        if (m_nilai_det.Nilai.Trim() != "")
                                                                        {
                                                                            nilai = m_nilai_det.Nilai.Trim();
                                                                            if (m_nilai_det.PB == null) m_nilai_det.PB = "";
                                                                            if (item_rapor_struktur_nilai_kp.IsAdaPB && m_nilai_det.PB.Trim() != nilai && m_nilai_det.PB.Trim() != "" && Libs.IsAngka(m_nilai_det.PB.Trim().Replace("=", "")))
                                                                            {
                                                                                nilai = m_nilai_det.PB.Trim().Replace("=", "");
                                                                            }

                                                                            if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                                            {
                                                                                nilai_kd += Math.Round((item_rapor_struktur_nilai_kp.BobotNK / 100) * Libs.GetStringToDecimal(nilai), 2, MidpointRounding.AwayFromZero);
                                                                            }
                                                                            else if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                            {
                                                                                jumlah_nilai_kd += Libs.GetStringToDecimal(nilai);
                                                                            }
                                                                            count_kd++;
                                                                        }
                                                                    }

                                                                }

                                                            }

                                                            if (count_kd > 0)
                                                            {
                                                                if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                {
                                                                    nilai_kd = Math.Round(jumlah_nilai_kd / count_kd, 2, MidpointRounding.AwayFromZero);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                nilai_kd = 0;
                                                            }

                                                            //nilai_kd = Math.Round(nilai_kd, Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero);
                                                            nilai_kd = Math.Round(nilai_kd, 2, MidpointRounding.AwayFromZero);
                                                            lst_nilai_kd.Add(new NilaiWithKey
                                                            {
                                                                Key = item_rapor_struktur_nilai_kd.Kode.ToString(),
                                                                Nilai = nilai_kd
                                                            });
                                                            //end get struktur nilai det KP

                                                        }
                                                        //end get struktur nilai det KD

                                                        //get nilai ap
                                                        count_ap = 0;
                                                        nilai_ap = 0;
                                                        jumlah_nilai_ap = 0;
                                                        foreach (var item_nilai_kd in lst_nilai_kd)
                                                        {
                                                            if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                            {
                                                                nilai_ap += Math.Round((DAO_Rapor_StrukturNilai_KD.GetByID_Entity(item_nilai_kd.Key).BobotAP / 100) * item_nilai_kd.Nilai, 2, MidpointRounding.AwayFromZero);
                                                            }
                                                            else if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                jumlah_nilai_ap += item_nilai_kd.Nilai;
                                                            }
                                                            count_ap++;
                                                        }
                                                        if (count_ap > 0)
                                                        {
                                                            if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                nilai_ap = Math.Round(jumlah_nilai_ap / count_ap, 2, MidpointRounding.AwayFromZero);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            nilai_ap = 0;
                                                        }
                                                        nilai_ap = Math.Round(nilai_ap, 2, MidpointRounding.AwayFromZero);
                                                        lst_nilai_ap.Add(new NilaiWithKey
                                                        {
                                                            Key = item_rapor_struktur_nilai_ap.Kode.ToString(),
                                                            Nilai = nilai_ap
                                                        });
                                                        //end get nilai ap

                                                    }
                                                    //end get struktur nilai det AP

                                                    //get nilai rapor
                                                    nilai_rapor = 0;
                                                    jumlah_nilai_rapor = 0;
                                                    count_nilai_rapor = 0;
                                                    foreach (var item_nilai_ap in lst_nilai_ap)
                                                    {
                                                        if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                        {
                                                            nilai_rapor += Math.Round((DAO_Rapor_StrukturNilai_AP.GetByID_Entity(item_nilai_ap.Key).BobotRapor / 100) * item_nilai_ap.Nilai, 2, MidpointRounding.AwayFromZero);
                                                        }
                                                        else if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                        {
                                                            jumlah_nilai_rapor += item_nilai_ap.Nilai;
                                                        }
                                                        count_nilai_rapor++;
                                                    }
                                                    if (count_nilai_rapor > 0)
                                                    {
                                                        if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                        {
                                                            nilai_rapor = Math.Round(jumlah_nilai_rapor / count_nilai_rapor, 3, MidpointRounding.AwayFromZero);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        nilai_rapor = 0;
                                                    }

                                                    if (DAO_Mapel.GetJenisMapel(m_struktur_nilai.Rel_Mapel.ToString()) == Application_Libs.Libs.JENIS_MAPEL.PILIHAN && nilai_rapor == 0)
                                                    {
                                                        nilai_rapor = -99;
                                                        jumlah_mapel_rapor++;
                                                    }
                                                    else
                                                    {
                                                        nilai_rapor = Math.Round(nilai_rapor, Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero);
                                                        jumlah_keseluruhan += nilai_rapor;
                                                        jumlah_mapel_rapor++;
                                                    }
                                                    //end get nilai rapor

                                                }
                                            }
                                            //end get struktur nilai

                                            //cek nilai IPA
                                            if (nilai_rapor != -99)
                                            {
                                                id_rapor++;

                                                if (
                                                    m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_BIOLOGI ||
                                                    m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA
                                                )
                                                {
                                                    lst_nilai_ipa.Add(nilai_rapor);
                                                    deskripsi_ipa += (deskripsi_ipa.Trim() != "" ? "; " : "") +
                                                                     m_struktur_nilai.DeskripsiUmum;
                                                }

                                                if (!(m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_BIOLOGI ||
                                                    m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA))
                                                { //biologi
                                                    hasil.Add(new KTSP_DeskripsiRapor
                                                    {
                                                        IDSiswa = m_siswa.Nama + m_siswa.Kode.ToString(),
                                                        Nama = m_siswa.Nama,
                                                        NIS = m_siswa.NISSekolah,
                                                        NISN = m_siswa.NISN,
                                                        NamaMataPelajaran = item.NamaMapelRapor,
                                                        TahunPelajaran = tahun_ajaran,
                                                        Semester = semester,
                                                        UrutanRapor = id_rapor,
                                                        Alamat = "",
                                                        Kelas = m_kelas_det.Nama,
                                                        NamaSekolah = "",
                                                        Nilai = Convert.ToDecimal(nilai_rapor),
                                                        KKM = m_struktur_nilai.KKM,
                                                        Deskripsi = Libs.GetHTMLSimpleText2(m_struktur_nilai.DeskripsiUmum)
                                                    });
                                                }
                                                else if (m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA) //fisika
                                                {
                                                    nilai_rapor = Math.Round(lst_nilai_ipa.DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero);
                                                    hasil.Add(new KTSP_DeskripsiRapor
                                                    {
                                                        IDSiswa = m_siswa.Nama + m_siswa.Kode.ToString(),
                                                        Nama = m_siswa.Nama,
                                                        NIS = m_siswa.NISSekolah,
                                                        NISN = m_siswa.NISN,
                                                        NamaMataPelajaran = "Ilmu Pengetahuan Alam",
                                                        TahunPelajaran = tahun_ajaran,
                                                        Semester = semester,
                                                        UrutanRapor = id_rapor,
                                                        Alamat = "",
                                                        Kelas = m_kelas_det.Nama,
                                                        NamaSekolah = "",
                                                        Nilai = Convert.ToDecimal(nilai_rapor),
                                                        KKM = m_struktur_nilai.KKM,
                                                        Deskripsi = Libs.GetHTMLSimpleText2(deskripsi_ipa)
                                                    });
                                                }
                                            }
                                            //end cek nilai IPA
                                        }

                                    }

                                    nomor++;
                                }

                            }

                        }
                    }

                }
            }

            return hasil;
        }

        public static string GetHTMLLedger_KTSP(
            System.Web.UI.Page page,
            string tahun_ajaran,
            string semester,
            string rel_kelas_det,
            string rel_siswa = ""
        )
        {
            Rapor_Arsip m_rapor_arsip = DAO_Rapor_Arsip.GetAll_Entity().FindAll(
                    m0 => m0.TahunAjaran == tahun_ajaran &&
                            m0.Semester == semester &&
                            m0.JenisRapor == "Semester"

                ).FirstOrDefault();

            string css_cell_headers = "border-style: solid; border-width: 1px; text-align: center; font-weight: bold; padding: 5px;";
            string css_cell_body = "border-style: solid; border-width: 1px; padding: 3px;";

            string html = "";
            string html_row_header = "<td rowspan=\"2\" style=\"" + css_cell_headers + "\">" +
                                        "No" +
                                     "</td>" +
                                     "<td rowspan=\"2\" style=\"" + css_cell_headers + "\">" +
                                        "NIS" +
                                     "</td>" +
                                     "<td rowspan=\"2\" style=\"" + css_cell_headers + "\">" +
                                        "NAMA LENGKAP" +
                                     "</td>" +
                                     "<td rowspan=\"2\" style=\"" + css_cell_headers + "\">" +
                                        "L/P" +
                                     "</td>";
            string html_row_mapel = "";
            string html_row_body = "";

            string s_walikelas = "";
            List<FormasiGuruKelas> lst_formasi_guru_kelas = DAO_FormasiGuruKelas.GetByUnitByTABySM_Entity(
                        GetUnitSekolah().Kode.ToString(), tahun_ajaran, semester
                    ).FindAll(m => m.Rel_KelasDet.ToString().ToUpper() == rel_kelas_det.Trim().ToUpper());
            if (lst_formasi_guru_kelas != null)
            {
                if (lst_formasi_guru_kelas.Count > 0)
                {
                    FormasiGuruKelas m_guru_kelas = lst_formasi_guru_kelas.FirstOrDefault();
                    if (m_guru_kelas != null)
                    {
                        if (m_guru_kelas.TahunAjaran != null)
                        {
                            Pegawai m_pegawai = DAO_Pegawai.GetByID_Entity(m_guru_kelas.Rel_GuruKelas);
                            if (m_pegawai != null)
                            {
                                if (m_pegawai.Nama != null)
                                {
                                    s_walikelas = m_pegawai.Nama;
                                }
                            }
                        }
                    }
                }
            }

            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {

                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());
                    if (m_kelas != null)
                    {
                        if (m_kelas.Nama != null)
                        {

                            Rapor_Desain rapor_desain = DAO_Rapor_Desain.GetByTABySMByKelas_Entity(
                                    tahun_ajaran, semester, rel_kelas_det, DAO_Rapor_Desain.JenisRapor.Semester
                                ).FirstOrDefault();
                            List<Rapor_Desain_Det> lst_rapor_desain_det = DAO_Rapor_Desain_Det.GetAllByHeader_Entity(rapor_desain.Kode.ToString());

                            //list cols header
                            int jumlah_mapel = 0;
                            html_row_mapel = "";
                            foreach (Rapor_Desain_Det item_desain_det in lst_rapor_desain_det)
                            {
                                html_row_mapel += "<td style=\"" + css_cell_headers + " font-size: 10px; \">" +
                                                  (
                                                    item_desain_det.Alias.Trim() != ""
                                                    ? item_desain_det.Alias
                                                    : item_desain_det.NamaMapelRapor.Trim().ToUpper()
                                                  ) +
                                                  "</td>";
                                jumlah_mapel++;

                                if (
                                    item_desain_det.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA
                                )
                                {
                                    if (item_desain_det.Rel_Mapel.Trim() != "")
                                    {
                                        html_row_mapel += "<td style=\"" + css_cell_headers + " font-size: 10px; \">" +
                                                            "IPA" +
                                                          "</td>";
                                        jumlah_mapel++;
                                    }
                                }
                            }
                            html_row_header += "<td colspan=\"" + jumlah_mapel.ToString() + "\" style=\"" + css_cell_headers + " font-size: 8.5px; \">" +
                                                    "NILAI MATA PELAJARAN" +
                                               "</td>";
                            html_row_header += "<td colspan=\"4\" style=\"" + css_cell_headers + " font-size: 8.5px; \">" +
                                                    "PRESENSI" +
                                               "</td>";
                            html_row_header += "<td colspan=\"2\" style=\"" + css_cell_headers + " font-size: 8.5px; \">" +
                                                    "JUMLAH NILAI<br />" +
                                                    "KESELURUHAN" +
                                               "</td>";
                            html_row_mapel += "<td style=\"" + css_cell_headers + " width: 45px; font-size: 8.5px; \">" +
                                                    "S" +
                                              "</td>" +
                                              "<td style=\"" + css_cell_headers + " width: 45px; font-size: 8.5px; \">" +
                                                    "I" +
                                              "</td>" +
                                              "<td style=\"" + css_cell_headers + " width: 45px; font-size: 8.5px; \">" +
                                                    "A" +
                                              "</td>" +
                                              "<td style=\"" + css_cell_headers + " width: 45px; font-size: 8.5px; \">" +
                                                    "T" +
                                              "</td>";
                            html_row_mapel += "<td style=\"" + css_cell_headers + " width: 45px; font-size: 8.5px; \">" +
                                                    "JUMLAH" +
                                              "</td>" +
                                              "<td style=\"" + css_cell_headers + " width: 45px; font-size: 8.5px; \">" +
                                                    "RATA-RATA" +
                                              "</td>";
                            //end list cols header
                            if (rapor_desain != null)
                            {

                                List<DAO_Siswa.SiswaDataSimple> lst_siswa = DAO_Siswa.GetAllSiswaDataSimpleByTahunAjaranUnitKelas_Entity(
                                        m_kelas.Rel_Sekolah.ToString(),
                                        rel_kelas_det,
                                        tahun_ajaran,
                                        semester
                                    );
                                List<NilaiWithKey> lst_nilai_ap = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_nilai_kd = new List<NilaiWithKey>();
                                List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT> lst_nilai_siswa_det = new List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT>();
                                List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT> lst_nilai_siswa_det_by_periode = new List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT>();

                                lst_nilai_siswa_det_by_periode = DAO_Rapor_NilaiSiswa_Det.GetAllByTABySMByKelasDet_Entity(
                                        tahun_ajaran, semester, rel_kelas_det
                                    );

                                int nomor = 1;
                                if (rel_siswa.Trim() != "") lst_siswa = lst_siswa.FindAll(m => m.Kode.ToString() == rel_siswa);
                                List<NilaiWithKey> lst_all_nilai_rapor = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_kkm_nilai_rapor = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_jumlah_nilai_keseluruhan = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_nilai_rapor = new List<NilaiWithKey>();
                                decimal jumlah_keseluruhan = 0;
                                decimal jumlah_mapel_rapor = 0;
                                int jumlah_mapel_rapor_pembagi = 0;
                                List<decimal> lst_nilai_ipa = new List<decimal>();
                                lst_nilai_ipa.Clear();
                                foreach (DAO_Siswa.SiswaDataSimple m_siswa in lst_siswa.OrderBy(m => m.Nama))
                                {
                                    jumlah_keseluruhan = 0;
                                    jumlah_mapel_rapor = 0;
                                    jumlah_mapel_rapor_pembagi = 0;

                                    string html_row_body_siswa = "";
                                    html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                nomor.ToString() +
                                                           "</td>" +
                                                           "<td style=\"" + css_cell_body + "\">" +
                                                                m_siswa.NISSekolah +
                                                           "</td>" +
                                                           "<td style=\"" + css_cell_body + " white-space: nowrap;\">" +
                                                                Libs.GetPerbaikiEjaanNama(m_siswa.Nama) +
                                                           "</td>" +
                                                           "<td style=\"" + css_cell_body + "\">" +
                                                                m_siswa.JenisKelamin +
                                                           "</td>";

                                    List<decimal> lst_kkm_ipa = new List<decimal>();
                                    lst_kkm_ipa.Clear();

                                    lst_kkm_nilai_rapor.Clear();
                                    lst_nilai_ipa.Clear();
                                    foreach (Rapor_Desain_Det item in lst_rapor_desain_det.FindAll(m => m.Rel_Mapel.Trim() != ""))
                                    {

                                        if (item.Rel_Mapel.Trim() != "")
                                        {

                                            lst_nilai_siswa_det = lst_nilai_siswa_det_by_periode.FindAll(
                                                    m => m.Rel_Mapel == item.Rel_Mapel &&
                                                         m.Rel_Siswa == m.Rel_Siswa
                                                );

                                            //get struktur nilainya
                                            Rapor_StrukturNilai m_struktur_nilai = DAO_Rapor_StrukturNilai.GetATop1ByTABySMByKelasByMapel_Entity(
                                                    tahun_ajaran, semester, m_kelas.Kode.ToString(), item.Rel_Mapel
                                                );

                                            decimal nilai_rapor = 0;
                                            decimal nilai_ipa = 0;                                            
                                            decimal jumlah_nilai_rapor = 0;
                                            decimal count_nilai_rapor = 0;
                                            if (m_struktur_nilai != null)
                                            {
                                                if (m_struktur_nilai.TahunAjaran != null)
                                                {

                                                    lst_kkm_nilai_rapor.Add(new NilaiWithKey
                                                    {
                                                        Key = (jumlah_mapel_rapor + 1).ToString(),
                                                        Nilai = m_struktur_nilai.KKM
                                                    });

                                                    //get struktur nilai det AP
                                                    List<Rapor_StrukturNilai_AP> lst_rapor_struktur_nilai_ap = DAO_Rapor_StrukturNilai_AP.GetAllByHeader_Entity(
                                                            m_struktur_nilai.Kode.ToString()
                                                        );
                                                    lst_nilai_ap.Clear();
                                                    decimal jumlah_nilai_ap = 0;
                                                    decimal nilai_ap = 0;
                                                    int count_ap = 0;
                                                    foreach (var item_rapor_struktur_nilai_ap in lst_rapor_struktur_nilai_ap)
                                                    {

                                                        //get struktur nilai det KD
                                                        List<Rapor_StrukturNilai_KD> lst_rapor_struktur_nilai_kd = DAO_Rapor_StrukturNilai_KD.GetAllByHeader_Entity(
                                                                item_rapor_struktur_nilai_ap.Kode.ToString()
                                                            );
                                                        lst_nilai_kd.Clear();
                                                        foreach (var item_rapor_struktur_nilai_kd in lst_rapor_struktur_nilai_kd)
                                                        {

                                                            //get struktur nilai det KP
                                                            List<Rapor_StrukturNilai_KP> lst_rapor_struktur_nilai_kp = DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(
                                                                item_rapor_struktur_nilai_kd.Kode.ToString()
                                                            );
                                                            decimal jumlah_nilai_kd = 0;
                                                            decimal nilai_kd = 0;
                                                            int count_kd = 0;
                                                            foreach (var item_rapor_struktur_nilai_kp in lst_rapor_struktur_nilai_kp)
                                                            {
                                                                string nilai = "";
                                                                Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(item_rapor_struktur_nilai_kp.Rel_Rapor_KomponenPenilaian.ToString());
                                                                if (m_kp != null)
                                                                {

                                                                    Rapor_NilaiSiswa_Det m_nilai_det = lst_nilai_siswa_det.FindAll(
                                                                        m => m.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper() &&
                                                                             m.Rel_Mapel.ToString().Trim().ToUpper() == m_struktur_nilai.Rel_Mapel.ToString().Trim().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_AP.Trim().ToUpper() == item_rapor_struktur_nilai_ap.Kode.ToString().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_KD.Trim().ToUpper() == item_rapor_struktur_nilai_kd.Kode.ToString().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_KP.Trim().ToUpper() == item_rapor_struktur_nilai_kp.Kode.ToString().ToUpper()
                                                                    ).FirstOrDefault();

                                                                    if (m_nilai_det != null)
                                                                    {
                                                                        if (m_nilai_det.Nilai.Trim() != "")
                                                                        {
                                                                            nilai = m_nilai_det.Nilai.Trim();
                                                                            if (m_nilai_det.PB == null) m_nilai_det.PB = "";
                                                                            if (item_rapor_struktur_nilai_kp.IsAdaPB && m_nilai_det.PB.Trim() != nilai && m_nilai_det.PB.Trim() != "" && Libs.IsAngka(m_nilai_det.PB.Trim().Replace("=", "")))
                                                                            {
                                                                                nilai = m_nilai_det.PB.Trim().Replace("=", "");
                                                                            }

                                                                            if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                                            {
                                                                                nilai_kd += Math.Round((item_rapor_struktur_nilai_kp.BobotNK / 100) * Libs.GetStringToDecimal(nilai), 2, MidpointRounding.AwayFromZero);
                                                                            }
                                                                            else if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                            {
                                                                                jumlah_nilai_kd += Libs.GetStringToDecimal(nilai);
                                                                            }
                                                                            count_kd++;
                                                                        }
                                                                    }

                                                                }

                                                            }

                                                            if (count_kd > 0)
                                                            {
                                                                if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                {
                                                                    nilai_kd = Math.Round(jumlah_nilai_kd / count_kd, 2, MidpointRounding.AwayFromZero);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                nilai_kd = 0;
                                                            }

                                                            nilai_kd = Math.Round(nilai_kd, 2, MidpointRounding.AwayFromZero);
                                                            //nilai_kd = Math.Round(nilai_kd, Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero);
                                                            lst_nilai_kd.Add(new NilaiWithKey {
                                                                Key = item_rapor_struktur_nilai_kd.Kode.ToString(),
                                                                Nilai = nilai_kd
                                                            });
                                                            //end get struktur nilai det KP

                                                        }
                                                        //end get struktur nilai det KD

                                                        //get nilai ap
                                                        count_ap = 0;
                                                        nilai_ap = 0;
                                                        jumlah_nilai_ap = 0;
                                                        foreach (var item_nilai_kd in lst_nilai_kd)
                                                        {
                                                            if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                            {
                                                                nilai_ap += Math.Round((DAO_Rapor_StrukturNilai_KD.GetByID_Entity(item_nilai_kd.Key).BobotAP / 100) * item_nilai_kd.Nilai, 2, MidpointRounding.AwayFromZero);
                                                            }
                                                            else if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                jumlah_nilai_ap += item_nilai_kd.Nilai;
                                                            }
                                                            count_ap++;
                                                        }
                                                        if (count_ap > 0)
                                                        {
                                                            if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                nilai_ap = Math.Round(jumlah_nilai_ap / count_ap, 2, MidpointRounding.AwayFromZero);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            nilai_ap = 0;
                                                        }
                                                        nilai_ap = Math.Round(nilai_ap, 2, MidpointRounding.AwayFromZero);
                                                        lst_nilai_ap.Add(new NilaiWithKey {
                                                            Key = item_rapor_struktur_nilai_ap.Kode.ToString(),
                                                            Nilai = nilai_ap 
                                                        });
                                                        //end get nilai ap

                                                    }
                                                    //end get struktur nilai det AP

                                                    //get nilai rapor
                                                    nilai_rapor = 0;
                                                    nilai_ipa = 0;
                                                    jumlah_nilai_rapor = 0;
                                                    count_nilai_rapor = 0;
                                                    foreach (var item_nilai_ap in lst_nilai_ap)
                                                    {
                                                        if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                        {
                                                            nilai_rapor += Math.Round((DAO_Rapor_StrukturNilai_AP.GetByID_Entity(item_nilai_ap.Key).BobotRapor / 100) * item_nilai_ap.Nilai, 2, MidpointRounding.AwayFromZero);
                                                        }
                                                        else if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                        {
                                                            jumlah_nilai_rapor += item_nilai_ap.Nilai;
                                                        }
                                                        count_nilai_rapor++;
                                                    }
                                                    if (count_nilai_rapor > 0)
                                                    {
                                                        if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                        {
                                                            nilai_rapor = Math.Round(jumlah_nilai_rapor / count_nilai_rapor, 3, MidpointRounding.AwayFromZero);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        nilai_rapor = 0;
                                                    }

                                                    //end get nilai rapor

                                                    if (
                                                        m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_BIOLOGI ||
                                                        m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA
                                                    )
                                                    {
                                                        lst_nilai_ipa.Add(
                                                                nilai_rapor = Math.Round(nilai_rapor, Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero)
                                                            );
                                                        lst_kkm_ipa.Add(m_struktur_nilai.KKM);
                                                    }
                                                    
                                                    if (DAO_Mapel.GetJenisMapel(m_struktur_nilai.Rel_Mapel.ToString()) == Application_Libs.Libs.JENIS_MAPEL.PILIHAN && nilai_rapor == 0)
                                                    {
                                                        nilai_rapor = -99;
                                                        jumlah_mapel_rapor++;
                                                    }
                                                    else
                                                    {
                                                        nilai_rapor = Math.Round(nilai_rapor, Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero);
                                                        jumlah_keseluruhan += nilai_rapor;
                                                        jumlah_mapel_rapor++;
                                                        lst_all_nilai_rapor.Add(new NilaiWithKey
                                                        {
                                                            Key = jumlah_mapel_rapor.ToString(),
                                                            Nilai = nilai_rapor
                                                        });
                                                        lst_nilai_rapor.Add(new NilaiWithKey
                                                        {
                                                            Key = m_siswa.Kode.ToString(),
                                                            Nilai = nilai_rapor
                                                        });
                                                        if (
                                                                !(
                                                                    m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_BIOLOGI &&
                                                                    m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA
                                                                )
                                                            )
                                                        {
                                                            jumlah_mapel_rapor_pembagi++;
                                                        }
                                                    }

                                                    if (m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA) //fisika
                                                    {
                                                        nilai_ipa = Math.Round(lst_nilai_ipa.DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero);
                                                        jumlah_keseluruhan += nilai_ipa;
                                                        jumlah_mapel_rapor++;
                                                        lst_all_nilai_rapor.Add(new NilaiWithKey
                                                        {
                                                            Key = jumlah_mapel_rapor.ToString(),
                                                            Nilai = nilai_ipa
                                                        });
                                                        lst_nilai_rapor.Add(new NilaiWithKey
                                                        {
                                                            Key = m_siswa.Kode.ToString(),
                                                            Nilai = nilai_ipa
                                                        });
                                                        lst_kkm_nilai_rapor.Add(new NilaiWithKey
                                                        {
                                                            Key = jumlah_mapel_rapor.ToString(),
                                                            Nilai = Math.Round(lst_kkm_ipa.DefaultIfEmpty().Average(), 1, MidpointRounding.AwayFromZero)
                                                        });
                                                        jumlah_mapel_rapor_pembagi++;
                                                    }

                                                }
                                            }
                                            //end get struktur nilai

                                            if (nilai_rapor != -99)
                                            {
                                                html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center; " + (nilai_rapor < m_struktur_nilai.KKM ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                            nilai_rapor.ToString() +
                                                                       "</td>";
                                            }
                                            else
                                            {
                                                html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                            "&nbsp;" +
                                                                       "</td>";
                                            }
                                            if (m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA) //fisika
                                            {
                                                if (nilai_ipa != -99)
                                                {
                                                    html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center; " + (nilai_ipa < m_struktur_nilai.KKM ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                                nilai_ipa.ToString() +
                                                                           "</td>";
                                                }
                                                else
                                                {
                                                    html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "&nbsp;" +
                                                                           "</td>";
                                                }
                                            }

                                        }

                                    }

                                    lst_jumlah_nilai_keseluruhan.Add(new NilaiWithKey
                                    {
                                        Key = m_siswa.Kode.ToString(),
                                        Nilai = jumlah_keseluruhan
                                    });

                                    //absen
                                    string s_sakit = "-";
                                    string s_izin = "-";
                                    string s_alpa = "-";
                                    string s_terlambat = "-";

                                    List<DAO_SiswaAbsen.AbsenRekapRapor> lst_absen = new List<DAO_SiswaAbsen.AbsenRekapRapor>();
                                    lst_absen = DAO_SiswaAbsen.GetRekapAbsenRaporBySiswaByPeriode_Entity(
                                            m_siswa.Kode.ToString(),
                                            (m_rapor_arsip != null ? m_rapor_arsip.TanggalAwalAbsen : DateTime.MinValue),
                                            (m_rapor_arsip != null ? m_rapor_arsip.TanggalAkhirAbsen : DateTime.MinValue)
                                        );
                                    foreach (var absen in lst_absen)
                                    {
                                        if (absen.Absen == Libs.JENIS_ABSENSI.SAKIT.Substring(0, 1)) s_sakit = absen.Jumlah.ToString();
                                        if (absen.Absen == Libs.JENIS_ABSENSI.IZIN.Substring(0, 1)) s_izin = absen.Jumlah.ToString();
                                        if (absen.Absen == Libs.JENIS_ABSENSI.ALPA.Substring(0, 1)) s_alpa = absen.Jumlah.ToString();
                                        if (absen.Absen == Libs.JENIS_ABSENSI.TERLAMBAT.Substring(0, 1)) s_terlambat = absen.Jumlah.ToString();
                                    }
                                    html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center; \">" +
                                                                s_sakit +
                                                           "</td>" +
                                                           "<td style=\"" + css_cell_body + " text-align: center; \">" +
                                                                s_izin +
                                                           "</td>" +
                                                           "<td style=\"" + css_cell_body + " text-align: center; \">" +
                                                                s_alpa +
                                                           "</td>" +
                                                           "<td style=\"" + css_cell_body + " text-align: center; \">" +
                                                                s_terlambat +
                                                           "</td>" +
                                                           "<td style=\"" + css_cell_body + " text-align: center; \">" +
                                                                Math.Round(jumlah_keseluruhan, 0).ToString() +
                                                           "</td>" +
                                                           "<td style=\"" + css_cell_body + " text-align: center; \">" +
                                                                (
                                                                    jumlah_mapel_rapor_pembagi == 0
                                                                    ? 0.ToString()
                                                                    : (Math.Round(jumlah_keseluruhan / jumlah_mapel_rapor_pembagi, Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero)).ToString()
                                                                ) +
                                                           "</td>";
                                    //end absen

                                    html_row_body += "<tr>" +
                                                        html_row_body_siswa +
                                                     "</tr>";

                                    nomor++;
                                }
                                // end foreach siswa
                                string html_row_rata_rata = "<td colspan=\"4\" style=\"" + css_cell_body + " font-weight: bold;\">" +
                                                                "RATA-RATA KELAS" +
                                                            "</td>";
                                string html_row_kkm = "<td colspan=\"4\" style=\"" + css_cell_body + " font-weight: bold;\">" +
                                                            "KKM" +
                                                      "</td>";
                                for (int i = 1; i <= jumlah_mapel_rapor; i++)
                                {
                                    decimal rata_rata = Math.Round(lst_all_nilai_rapor.FindAll(m => m.Key == i.ToString()).Select(m => m.Nilai).DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP);
                                    decimal kkm = 0;
                                    NilaiWithKey m_kkm = lst_kkm_nilai_rapor.FindAll(m => m.Key == i.ToString()).FirstOrDefault();
                                    if (m_kkm != null)
                                    {
                                        if (m_kkm.Key != null)
                                        {
                                            kkm = m_kkm.Nilai;
                                        }
                                    }
                                    html_row_rata_rata += "<td style=\"" + css_cell_body + " text-align: center; " + (rata_rata < kkm ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                 Math.Round(rata_rata, 1).ToString() +
                                                          "</td>";
                                    html_row_kkm += "<td style=\"" + css_cell_body + " text-align: center; " + (rata_rata < kkm ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                        Math.Round(kkm, 1).ToString() +
                                                    "</td>";
                                }
                                for (int i = 1; i <= 6; i++)
                                {
                                    if (i <= 4)
                                    {
                                        html_row_rata_rata += "<td style=\"" + css_cell_body + " text-align: right;\">" +
                                                                    "&nbsp;" +
                                                              "</td>";
                                    }
                                    html_row_kkm += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                        "&nbsp;" +
                                                    "</td>";
                                }
                                decimal rata_rata_nilai_keseluruhan = Math.Round(lst_jumlah_nilai_keseluruhan.Select(m => m.Nilai).DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP);
                                decimal rata_rata_nilai_rata_rata_rapor = Math.Round(lst_nilai_rapor.Select(m => m.Nilai).DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP);
                                html_row_rata_rata += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                        Math.Round(rata_rata_nilai_keseluruhan, 2, MidpointRounding.AwayFromZero).ToString() +
                                                      "</td>";
                                html_row_rata_rata += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                        Math.Round(rata_rata_nilai_rata_rata_rapor, 2, MidpointRounding.AwayFromZero).ToString() +
                                                      "</td>";
                                html_row_body += "<tr>" +
                                                    html_row_rata_rata +
                                                 "</tr>" +
                                                 "<tr>" +
                                                    html_row_kkm +
                                                 "</tr>";

                            }

                        }
                    }

                }
            }

            html_row_header = "<tr>" +
                                html_row_header +
                              "</tr>" +
                              "<tr>" +
                                html_row_mapel +
                              "</tr>" +
                              html_row_body;

            html = "<table style=\"border-collapse: collapse;\">" +
                        html_row_header +
                   "</table>" +
                   "<br />" +
                   "<table style=\"width: 100%;\">" +
                    "<tr>" +
                        "<td style=\"width: width: 20%; text-align: left;\">" +
                            "Kepala Sekolah<br />" +
                            "SMPI Al-Izhar Pondok Labu" +
                            "<br /><br /><br />" +
                            "<br /><br /><br />" +
                            m_rapor_arsip.KepalaSekolah +
                        "</td>" +
                        "<td style=\"width: width: 20%; text-align: center;\">" +
                            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                        "</td>" +
                        "<td style=\"width: width: 20%; text-align: center;\">" +
                            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                        "</td>" +
                        "<td style=\"width: width: 20%; text-align: center;\">" +
                            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                        "</td>" +
                        "<td style=\"width: width: 20%; text-align: left;\">" +
                            "Jakarta, " + Libs.GetTanggalIndonesiaFromDate(m_rapor_arsip.TanggalRapor, false) +
                            "<br />" +
                            "Wali Kelas" +
                            "<br /><br /><br />" +
                            "<br /><br /><br />" +
                            s_walikelas +
                        "</td>" +
                    "</tr>" +
                   "</table>" +
                   "<br />" +
                   "<br />";

            return html;
        }

        public static string GetHTMLLedger_KURTILAS(
            System.Web.UI.Page page,
            string tahun_ajaran,
            string semester,
            string rel_kelas_det,
            string rel_siswa = ""
        )
        {
            Rapor_Arsip m_rapor_arsip = DAO_Rapor_Arsip.GetAll_Entity().FindAll(
                    m0 => m0.TahunAjaran == tahun_ajaran &&
                            m0.Semester == semester &&
                            m0.JenisRapor == "Semester"

                ).FirstOrDefault();

            List<SiswaAbsenRekap> lst_absen_rekap = DAO_SiswaAbsenRekap.GetAllByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det).FindAll(m0 => m0.Rel_Mapel.Trim() == "" && m0.Jenis.ToString().ToUpper() == TipeRapor.SEMESTER.ToUpper().Trim());
            List<SiswaAbsenRekapDet> lst_rekap_det = new List<SiswaAbsenRekapDet>();
            if (lst_absen_rekap.Count == 1)
            {
                SiswaAbsenRekap m_rekap_absensi = lst_absen_rekap.FirstOrDefault();
                if (m_rekap_absensi != null)
                {
                    if (m_rekap_absensi.TahunAjaran != null)
                    {
                        lst_rekap_det = DAO_SiswaAbsenRekapDet.GetAllByHeader_Entity(m_rekap_absensi.Kode.ToString());
                    }
                }
            }

            List<Mapel> lst_mapel = DAO_Mapel.GetAllBySekolah_Entity(GetUnitSekolah().Kode.ToString());

            string s_walikelas = "";
            List<FormasiGuruKelas> lst_formasi_guru_kelas = DAO_FormasiGuruKelas.GetByUnitByTABySM_Entity(
                        GetUnitSekolah().Kode.ToString(), tahun_ajaran, semester
                    ).FindAll(m => m.Rel_KelasDet.ToString().ToUpper() == rel_kelas_det.Trim().ToUpper());
            if (lst_formasi_guru_kelas != null)
            {
                if (lst_formasi_guru_kelas.Count > 0)
                {
                    FormasiGuruKelas m_guru_kelas = lst_formasi_guru_kelas.FirstOrDefault();
                    if (m_guru_kelas != null)
                    {
                        if (m_guru_kelas.TahunAjaran != null)
                        {
                            Pegawai m_pegawai = DAO_Pegawai.GetByID_Entity(m_guru_kelas.Rel_GuruKelas);
                            if (m_pegawai != null)
                            {
                                if (m_pegawai.Nama != null)
                                {
                                    s_walikelas = m_pegawai.Nama;
                                }
                            }
                        }
                    }
                }
            }

            string css_cell_headers = "border-style: solid; border-width: 1px; text-align: center; font-weight: bold; padding: 5px;";
            string css_cell_body = "border-style: solid; border-width: 1px; padding: 3px;";

            string html = "";
            string html_row_header = "<td rowspan=\"3\" style=\"" + css_cell_headers + "\">" +
                                        "No" +
                                     "</td>" +
                                     "<td rowspan=\"3\" style=\"" + css_cell_headers + "\">" +
                                        "NIS" +
                                     "</td>" +
                                     "<td rowspan=\"3\" style=\"" + css_cell_headers + "\">" +
                                        "NAMA LENGKAP" +
                                     "</td>" +
                                     "<td rowspan=\"3\" style=\"" + css_cell_headers + "\">" +
                                        "L/P" +
                                     "</td>";
            string html_row_mapel = "";
            string html_row_jenis_nilai = "";
            string html_row_body = "";

            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {

                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());
                    if (m_kelas != null)
                    {
                        if (m_kelas.Nama != null)
                        {
                            Rapor_Desain rapor_desain = DAO_Rapor_Desain.GetByTABySMByKelas_Entity(
                                    tahun_ajaran, semester, rel_kelas_det, DAO_Rapor_Desain.JenisRapor.Semester
                                ).FirstOrDefault();

                            List<Rapor_Desain_Det> lst_rapor_desain_det = DAO_Rapor_Desain_Det.GetAllByHeader_Entity(rapor_desain.Kode.ToString()).FindAll(m => m.Nomor.Trim() != "" && m.Rel_Mapel.Trim() != "");

                            //list cols header
                            int jml_colspan_mapel = 0;
                            int jumlah_mapel = 0;
                            html_row_mapel = "";
                            foreach (Rapor_Desain_Det item_desain_det in lst_rapor_desain_det)
                            {
                                html_row_mapel += "<td colspan=\"4\" style=\"" + css_cell_headers + " font-size: 10px; \">" +
                                                    (
                                                        item_desain_det.Alias.Trim() != ""
                                                        ? item_desain_det.Alias
                                                        : item_desain_det.NamaMapelRapor.Trim().ToUpper()
                                                    ) + 
                                                  "</td>";
                                html_row_jenis_nilai += "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                "P" +
                                                        "</td>" +
                                                        "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                "PR" +
                                                        "</td>" +
                                                        "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                "K" +
                                                        "</td>" +
                                                        "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                "PR" +
                                                        "</td>";
                                jumlah_mapel++;

                                if (
                                    item_desain_det.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA
                                )
                                {
                                    if (item_desain_det.Rel_Mapel.Trim() != "")
                                    {
                                        html_row_mapel += "<td colspan=\"4\" style=\"" + css_cell_headers + " font-size: 10px; \">" +
                                                            "IPA" +
                                                          "</td>";
                                        html_row_jenis_nilai += "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                        "P" +
                                                                "</td>" +
                                                                "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                        "PR" +
                                                                "</td>" +
                                                                "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                        "K" +
                                                                "</td>" +
                                                                "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                        "PR" +
                                                                "</td>";
                                        jumlah_mapel++;
                                    }
                                }
                            }
                            jml_colspan_mapel = (jumlah_mapel * 4);

                            html_row_header += "<td colspan=\"" + jml_colspan_mapel.ToString() + "\" style=\"" + css_cell_headers + " font-size: 8.5px; \">" +
                                                    "NILAI MATA PELAJARAN" +
                                               "</td>";

                            html_row_header += "<td colspan=\"4\" style=\"" + css_cell_headers + " font-size: 8.5px; \">" +
                                                    "PRESENSI" +
                                               "</td>";
                            html_row_mapel += "<td rowspan=\"2\" style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                    "S" +
                                              "</td>" +
                                              "<td rowspan=\"2\" style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                    "I" +
                                              "</td>" +
                                              "<td rowspan=\"2\" style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                    "A" +
                                              "</td>" +
                                              "<td rowspan=\"2\" style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                    "T" +
                                              "</td>";

                            html_row_header += "<td colspan=\"2\" style=\"" + css_cell_headers + " font-size: 8.5px; \">" +
                                                    "RATA-RATA" +
                                               "</td>";
                            html_row_mapel += "<td rowspan=\"2\" style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                    "P" +
                                              "</td>" +
                                              "<td rowspan=\"2\" style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                    "K" +
                                              "</td>";

                            html_row_header += "<td colspan=\"2\" style=\"" + css_cell_headers + " font-size: 8.5px; \">" +
                                                    "JUMLAH" +
                                               "</td>";
                            html_row_mapel += "<td rowspan=\"2\" style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                    "P" +
                                              "</td>" +
                                              "<td rowspan=\"2\" style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                    "K" +
                                              "</td>";
                            //end list cols header
                            if (rapor_desain != null)
                            {

                                List<DAO_Siswa.SiswaDataSimple> lst_siswa = DAO_Siswa.GetAllSiswaDataSimpleByTahunAjaranUnitKelas_Entity(
                                        m_kelas.Rel_Sekolah.ToString(),
                                        rel_kelas_det,
                                        tahun_ajaran,
                                        semester
                                    );

                                List <NilaiWithKey> lst_nilai_ap = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_nilai_kd = new List<NilaiWithKey>();
                                List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT> lst_nilai_siswa_det = new List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT>();
                                List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT> lst_nilai_siswa_det_by_periode = new List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT>();

                                lst_nilai_siswa_det_by_periode = DAO_Rapor_NilaiSiswa_Det.GetAllByTABySMByKelasDet_Entity(
                                        tahun_ajaran, semester, rel_kelas_det
                                    );

                                int id_kolom_ledger = 0;
                                int nomor = 1;
                                int jumlah_awal_col_ledger = 3;
                                int jumlah_mapel_rapor = 0;

                                if (rel_siswa.Trim() != "") lst_siswa = lst_siswa.FindAll(m => m.Kode.ToString() == rel_siswa);
                                List<NilaiWithKey> lst_all_nilai_rapor = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_kkm_nilai_rapor = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_jumlah_nilai_keseluruhan = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_nilai_rapor = new List<NilaiWithKey>();
                                decimal jumlah_keseluruhan = 0;

                                string html_row_rata_rata = "<td colspan=\"4\" style=\"" + css_cell_body + " font-weight: bold;\">" +
                                                                "RATA-RATA KELAS" +
                                                            "</td>";
                                string html_row_kkm = "<td colspan=\"4\" style=\"" + css_cell_body + " font-weight: bold;\">" +
                                                            "KKM" +
                                                      "</td>";

                                int id_mulai_looping_rata_rata = id_kolom_ledger + 1;
                                int id_mulai_looping_kkm = jumlah_mapel_rapor + 1;

                                decimal d_jumlah_pengetahuan = 0;
                                decimal d_jumlah_keterampilan = 0;
                                int i_jumlah_pengetahuan = 0;
                                int i_jumlah_keterampilan = 0;
                                foreach (DAO_Siswa.SiswaDataSimple m_siswa in lst_siswa.OrderBy(m => m.Nama).ToList())//.FindAll(m0 => m0.Nama.IndexOf("Katinka Adjani") >= 0))
                                {
                                    jumlah_keseluruhan = 0;
                                    jumlah_mapel_rapor = 0;

                                    d_jumlah_pengetahuan = 0;
                                    d_jumlah_keterampilan = 0;
                                    i_jumlah_pengetahuan = 0;
                                    i_jumlah_keterampilan = 0;

                                    string html_row_body_siswa = "";
                                    html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                nomor.ToString() +
                                                           "</td>" +
                                                           "<td style=\"" + css_cell_body + "\">" +
                                                                m_siswa.NISSekolah +
                                                           "</td>" +
                                                           "<td style=\"" + css_cell_body + " white-space: nowrap;\">" +
                                                                Libs.GetPerbaikiEjaanNama(m_siswa.Nama) +
                                                           "</td>" +
                                                           "<td style=\"" + css_cell_body + "\">" +
                                                                m_siswa.JenisKelamin +
                                                           "</td>";

                                    lst_kkm_nilai_rapor.Clear();
                                    id_kolom_ledger = jumlah_awal_col_ledger;

                                    List<decimal> lst_nilai_ipa_pengetahuan = new List<decimal>();
                                    List<decimal> lst_nilai_ipa_praktik = new List<decimal>();
                                    List<decimal> lst_kp_tugas = new List<decimal>();
                                    List<decimal> lst_kp_uh_terakhir = new List<decimal>();
                                    List<decimal> lst_kp_uh_non_terakhir = new List<decimal>();

                                    lst_nilai_ipa_pengetahuan.Clear();
                                    lst_nilai_ipa_praktik.Clear();
                                    //nilai non mulok
                                    foreach (Rapor_Desain_Det item in DAO_Rapor_Desain_Det.GetAllByHeader_Entity(rapor_desain.Kode.ToString()).FindAll(m => m.Nomor.Trim() != "" && m.Rel_Mapel.Trim() != ""))
                                    {

                                        if (item.Rel_Mapel.Trim() != "")
                                        {
                                            string jenis_mapel = "";
                                            if (lst_mapel.FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == item.Rel_Mapel.ToUpper().Trim()).Count > 0)
                                            {
                                                jenis_mapel = lst_mapel.FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == item.Rel_Mapel.ToUpper().Trim()).FirstOrDefault().Jenis;
                                            }

                                            lst_nilai_siswa_det = lst_nilai_siswa_det_by_periode.FindAll(
                                                    m => m.Rel_Mapel == item.Rel_Mapel &&
                                                         m.Rel_Siswa == m.Rel_Siswa
                                                );

                                            //get struktur nilainya
                                            Rapor_StrukturNilai m_struktur_nilai = DAO_Rapor_StrukturNilai.GetATop1ByTABySMByKelasByMapel_Entity(
                                                    tahun_ajaran, semester, m_kelas.Kode.ToString(), item.Rel_Mapel
                                                );

                                            decimal nilai_rapor = 0;
                                            decimal jumlah_nilai_rapor = 0;
                                            decimal count_nilai_rapor = 0;

                                            decimal nilai_rapor_pengetahuan = 0;
                                            decimal nilai_rapor_keterampilan = 0;
                                            if (m_struktur_nilai != null)
                                            {
                                                if (m_struktur_nilai.TahunAjaran != null)
                                                {
                                                    lst_kp_tugas.Clear();
                                                    lst_kp_uh_terakhir.Clear();
                                                    lst_kp_uh_non_terakhir.Clear();

                                                    lst_kkm_nilai_rapor.Add(new NilaiWithKey
                                                    {
                                                        Key = (jumlah_mapel_rapor + 1).ToString(),
                                                        Nilai = m_struktur_nilai.KKM
                                                    });
                                                    //-----------nilai pengetahuan---------------
                                                    id_kolom_ledger++;
                                                    //get struktur nilai det AP
                                                    List<Rapor_StrukturNilai_AP> lst_rapor_struktur_nilai_ap = DAO_Rapor_StrukturNilai_AP.GetAllByHeaderByJenisAspekPenilaian_Entity(
                                                            m_struktur_nilai.Kode.ToString(), DAO_Rapor_StrukturNilai_AP.JenisAspekPenilaian.Pengetahuan
                                                        );
                                                    lst_nilai_ap.Clear();
                                                    decimal jumlah_nilai_ap = 0;
                                                    decimal nilai_ap = 0;
                                                    int count_ap = 0;
                                                    foreach (var item_rapor_struktur_nilai_ap in lst_rapor_struktur_nilai_ap)
                                                    {

                                                        //get struktur nilai det KD
                                                        List<Rapor_StrukturNilai_KD> lst_rapor_struktur_nilai_kd = DAO_Rapor_StrukturNilai_KD.GetAllByHeader_Entity(
                                                                item_rapor_struktur_nilai_ap.Kode.ToString()
                                                            );
                                                        lst_nilai_kd.Clear();
                                                        foreach (var item_rapor_struktur_nilai_kd in lst_rapor_struktur_nilai_kd)
                                                        {

                                                            Rapor_KompetensiDasar m_kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(item_rapor_struktur_nilai_kd.Rel_Rapor_KompetensiDasar.ToString());

                                                            //get struktur nilai det KP
                                                            List<Rapor_StrukturNilai_KP> lst_rapor_struktur_nilai_kp = DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(
                                                                item_rapor_struktur_nilai_kd.Kode.ToString()
                                                            );
                                                            decimal jumlah_nilai_kd = 0;
                                                            decimal nilai_kd = 0;
                                                            int count_kd = 0;
                                                            int id_kp = 1;
                                                            foreach (var item_rapor_struktur_nilai_kp in lst_rapor_struktur_nilai_kp)
                                                            {
                                                                string nilai = "";
                                                                Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(item_rapor_struktur_nilai_kp.Rel_Rapor_KomponenPenilaian.ToString());
                                                                if (m_kp != null)
                                                                {

                                                                    Rapor_NilaiSiswa_Det m_nilai_det = lst_nilai_siswa_det.FindAll(
                                                                        m => m.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper() &&
                                                                             m.Rel_Mapel.ToString().Trim().ToUpper() == m_struktur_nilai.Rel_Mapel.ToString().Trim().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_AP.Trim().ToUpper() == item_rapor_struktur_nilai_ap.Kode.ToString().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_KD.Trim().ToUpper() == item_rapor_struktur_nilai_kd.Kode.ToString().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_KP.Trim().ToUpper() == item_rapor_struktur_nilai_kp.Kode.ToString().ToUpper()
                                                                    ).FirstOrDefault();

                                                                    if (m_nilai_det == null)
                                                                    {
                                                                        m_nilai_det = new Rapor_NilaiSiswa_Det();
                                                                        m_nilai_det.Nilai = "0";
                                                                        m_nilai_det.PB = "0";
                                                                    }
                                                                    if (m_nilai_det != null)
                                                                    {
                                                                        if (m_nilai_det.Nilai.Trim() == "") m_nilai_det.Nilai = "0";
                                                                        if (m_nilai_det.Nilai.Trim() != "")
                                                                        {
                                                                            nilai = m_nilai_det.Nilai.Trim();
                                                                            if (m_nilai_det.PB == null) m_nilai_det.PB = "";
                                                                            if (item_rapor_struktur_nilai_kp.IsAdaPB && m_nilai_det.PB.Trim() != nilai && m_nilai_det.PB.Trim() != "" && Libs.IsAngka(m_nilai_det.PB.Trim().Replace("=", "")))
                                                                            {
                                                                                nilai = m_nilai_det.PB.Trim().Replace("=", "");
                                                                            }

                                                                            if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                                            {
                                                                                nilai_kd += Math.Round((item_rapor_struktur_nilai_kp.BobotNK / 100) * Libs.GetStringToDecimal(nilai), 2, MidpointRounding.AwayFromZero);
                                                                            }
                                                                            else if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                            {
                                                                                jumlah_nilai_kd += Libs.GetStringToDecimal(nilai);
                                                                            }
                                                                            count_kd++;
                                                                        }
                                                                    }

                                                                }

                                                                if (nilai.Trim() != "")
                                                                {
                                                                    if (
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "TUGAS" ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PH") >= 0 ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENILAIAN HARIAN") >= 0 ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENGETAHUAN") >= 0
                                                                        )
                                                                    {
                                                                        lst_kp_tugas.Add(
                                                                                Libs.GetStringToDecimal(nilai)
                                                                            );
                                                                    }
                                                                    else if (
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "UH"
                                                                        )
                                                                    {
                                                                        if (id_kp < lst_rapor_struktur_nilai_kp.Count)
                                                                        {
                                                                            lst_kp_uh_non_terakhir.Add(
                                                                                    Libs.GetStringToDecimal(nilai)
                                                                                );
                                                                        }
                                                                        else if (id_kp == lst_rapor_struktur_nilai_kp.Count)
                                                                        {
                                                                            lst_kp_uh_terakhir.Add(
                                                                                    Libs.GetStringToDecimal(nilai)
                                                                                );
                                                                        }
                                                                    }
                                                                    else if (
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PTS") >= 0
                                                                        )
                                                                    {
                                                                        lst_kp_uh_non_terakhir.Add(
                                                                                Libs.GetStringToDecimal(nilai)
                                                                            );
                                                                    }
                                                                    else if (
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PAS") >= 0
                                                                        )
                                                                    {
                                                                        lst_kp_uh_terakhir.Add(
                                                                                Libs.GetStringToDecimal(nilai)
                                                                            );
                                                                    }
                                                                    else if (
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PAT") >= 0
                                                                        )
                                                                    {
                                                                        lst_kp_uh_terakhir.Add(
                                                                                Libs.GetStringToDecimal(nilai)
                                                                            );
                                                                    }
                                                                }

                                                                id_kp++;

                                                            }

                                                            if (count_kd > 0)
                                                            {
                                                                if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                {
                                                                    nilai_kd = Math.Round(jumlah_nilai_kd / count_kd, 2, MidpointRounding.AwayFromZero);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                nilai_kd = 0;
                                                            }

                                                            nilai_kd = Math.Round(nilai_kd, 2, MidpointRounding.AwayFromZero);
                                                            lst_nilai_kd.Add(new NilaiWithKey
                                                            {
                                                                Key = item_rapor_struktur_nilai_kd.Kode.ToString(),
                                                                Nilai = nilai_kd
                                                            });
                                                            //end get struktur nilai det KP

                                                        }
                                                        //end get struktur nilai det KD

                                                        //get nilai ap
                                                        count_ap = 0;
                                                        nilai_ap = 0;
                                                        jumlah_nilai_ap = 0;
                                                        foreach (var item_nilai_kd in lst_nilai_kd)
                                                        {
                                                            if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                            {
                                                                nilai_ap += Math.Round((DAO_Rapor_StrukturNilai_KD.GetByID_Entity(item_nilai_kd.Key).BobotAP / 100) * item_nilai_kd.Nilai, 2, MidpointRounding.AwayFromZero);
                                                            }
                                                            else if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                jumlah_nilai_ap += item_nilai_kd.Nilai;
                                                            }
                                                            count_ap++;
                                                        }
                                                        if (count_ap > 0)
                                                        {
                                                            if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                nilai_ap = Math.Round(jumlah_nilai_ap / count_ap, 2, MidpointRounding.AwayFromZero);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            nilai_ap = 0;
                                                        }
                                                        nilai_ap = Math.Round(nilai_ap, 2, MidpointRounding.AwayFromZero);
                                                        lst_nilai_ap.Add(new NilaiWithKey
                                                        {
                                                            Key = item_rapor_struktur_nilai_ap.Kode.ToString(),
                                                            Nilai = nilai_ap
                                                        });
                                                        //end get nilai ap

                                                    }
                                                    //end get struktur nilai det AP

                                                    //get nilai rapor
                                                    nilai_rapor = 0;
                                                    jumlah_nilai_rapor = 0;
                                                    count_nilai_rapor = 0;
                                                    if (m_struktur_nilai.Is_PH_PTS_PAS)
                                                    {
                                                        string s_nilai_ph = "";
                                                        string s_nilai_pts = "";
                                                        string s_nilai_pas = "";

                                                        s_nilai_ph = (
                                                                lst_kp_tugas.Count > 0
                                                                ? Math.Round(lst_kp_tugas.DefaultIfEmpty().Average(), 2, MidpointRounding.AwayFromZero).ToString()
                                                                : "0"
                                                            );
                                                        s_nilai_pts = (
                                                                lst_kp_uh_non_terakhir.Count > 0
                                                                ? Math.Round(lst_kp_uh_non_terakhir.DefaultIfEmpty().Average(), 2, MidpointRounding.AwayFromZero).ToString()
                                                                : "0"
                                                            );
                                                        s_nilai_pas = (
                                                                lst_kp_uh_terakhir.Count > 0
                                                                ? Math.Round(lst_kp_uh_terakhir.DefaultIfEmpty().Average(), 2, MidpointRounding.AwayFromZero).ToString()
                                                                : "0"
                                                            );

                                                        nilai_rapor = Math.Round((Libs.GetStringToDecimal(s_nilai_ph) * (m_struktur_nilai.BobotPH / 100)), 1, MidpointRounding.AwayFromZero) +
                                                                      Math.Round((Libs.GetStringToDecimal(s_nilai_pts) * (m_struktur_nilai.BobotPTS / 100)), 2, MidpointRounding.AwayFromZero) +
                                                                      Math.Round((Libs.GetStringToDecimal(s_nilai_pas) * (m_struktur_nilai.BobotPAS / 100)), 1, MidpointRounding.AwayFromZero);
                                                    }
                                                    else if (!m_struktur_nilai.Is_PH_PTS_PAS)
                                                    {
                                                        foreach (var item_nilai_ap in lst_nilai_ap)
                                                        {
                                                            if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                            {
                                                                nilai_rapor += item_nilai_ap.Nilai;
                                                            }
                                                            else if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                jumlah_nilai_rapor += item_nilai_ap.Nilai;
                                                            }
                                                            count_nilai_rapor++;
                                                        }
                                                        if (count_nilai_rapor > 0)
                                                        {
                                                            if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                nilai_rapor = Math.Round(jumlah_nilai_rapor / count_nilai_rapor, 3, MidpointRounding.AwayFromZero);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            nilai_rapor = 0;
                                                        }
                                                    }

                                                    nilai_rapor = Math.Round(nilai_rapor, 0, MidpointRounding.AwayFromZero);
                                                    nilai_rapor_pengetahuan = nilai_rapor;
                                                    lst_all_nilai_rapor.Add(new NilaiWithKey
                                                    {
                                                        Key = (id_kolom_ledger).ToString(),
                                                        Nilai = nilai_rapor_pengetahuan,
                                                        JenisMapel = jenis_mapel
                                                    });
                                                    d_jumlah_pengetahuan += nilai_rapor_pengetahuan;
                                                    id_kolom_ledger += 2;
                                                    //end get nilai rapor

                                                    //simpan nilai biologi or fisika pengetahuan
                                                    if (
                                                        m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_BIOLOGI ||
                                                        m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA
                                                    )
                                                    {
                                                        lst_nilai_ipa_pengetahuan.Add(
                                                                Math.Round(nilai_rapor_pengetahuan, Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero)
                                                            );
                                                    }
                                                    //end simpan nilai biologi or fisika pengetahuan
                                                    //-----------end nilai pengetahuan---------------

                                                    //-----------nilai keterampilan---------------
                                                    //get struktur nilai det AP
                                                    lst_rapor_struktur_nilai_ap = DAO_Rapor_StrukturNilai_AP.GetAllByHeaderByJenisAspekPenilaian_Entity(
                                                            m_struktur_nilai.Kode.ToString(), DAO_Rapor_StrukturNilai_AP.JenisAspekPenilaian.Keterampilan
                                                        );
                                                    lst_nilai_ap.Clear();
                                                    jumlah_nilai_ap = 0;
                                                    nilai_ap = 0;
                                                    count_ap = 0;
                                                    foreach (var item_rapor_struktur_nilai_ap in lst_rapor_struktur_nilai_ap)
                                                    {

                                                        //get struktur nilai det KD
                                                        List<Rapor_StrukturNilai_KD> lst_rapor_struktur_nilai_kd = DAO_Rapor_StrukturNilai_KD.GetAllByHeader_Entity(
                                                                item_rapor_struktur_nilai_ap.Kode.ToString()
                                                            );
                                                        lst_nilai_kd.Clear();
                                                        foreach (var item_rapor_struktur_nilai_kd in lst_rapor_struktur_nilai_kd)
                                                        {

                                                            //get struktur nilai det KP
                                                            List<Rapor_StrukturNilai_KP> lst_rapor_struktur_nilai_kp = DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(
                                                                item_rapor_struktur_nilai_kd.Kode.ToString()
                                                            );
                                                            decimal jumlah_nilai_kd = 0;
                                                            decimal nilai_kd = 0;
                                                            int count_kd = 0;
                                                            foreach (var item_rapor_struktur_nilai_kp in lst_rapor_struktur_nilai_kp)
                                                            {
                                                                string nilai = "";
                                                                Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(item_rapor_struktur_nilai_kp.Rel_Rapor_KomponenPenilaian.ToString());
                                                                if (m_kp != null)
                                                                {

                                                                    Rapor_NilaiSiswa_Det m_nilai_det = lst_nilai_siswa_det.FindAll(
                                                                        m => m.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper() &&
                                                                             m.Rel_Mapel.ToString().Trim().ToUpper() == m_struktur_nilai.Rel_Mapel.ToString().Trim().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_AP.Trim().ToUpper() == item_rapor_struktur_nilai_ap.Kode.ToString().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_KD.Trim().ToUpper() == item_rapor_struktur_nilai_kd.Kode.ToString().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_KP.Trim().ToUpper() == item_rapor_struktur_nilai_kp.Kode.ToString().ToUpper()
                                                                    ).FirstOrDefault();

                                                                    if (m_nilai_det == null)
                                                                    {
                                                                        m_nilai_det = new Rapor_NilaiSiswa_Det();
                                                                        m_nilai_det.Nilai = "0";
                                                                        m_nilai_det.PB = "0";
                                                                    }
                                                                    if (m_nilai_det != null)
                                                                    {
                                                                        if (m_nilai_det.Nilai.Trim() == "") m_nilai_det.Nilai = "0";
                                                                        if (m_nilai_det.Nilai.Trim() != "")
                                                                        {
                                                                            nilai = m_nilai_det.Nilai.Trim();
                                                                            if (m_nilai_det.PB == null) m_nilai_det.PB = "";
                                                                            if (item_rapor_struktur_nilai_kp.IsAdaPB && m_nilai_det.PB.Trim() != nilai && m_nilai_det.PB.Trim() != "" && Libs.IsAngka(m_nilai_det.PB.Trim().Replace("=", "")))
                                                                            {
                                                                                nilai = m_nilai_det.PB.Trim().Replace("=", "");
                                                                            }

                                                                            if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                                            {
                                                                                nilai_kd += Math.Round((item_rapor_struktur_nilai_kp.BobotNK / 100) * Libs.GetStringToDecimal(nilai), 2, MidpointRounding.AwayFromZero);
                                                                            }
                                                                            else if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                            {
                                                                                jumlah_nilai_kd += Libs.GetStringToDecimal(nilai);
                                                                            }
                                                                            count_kd++;
                                                                        }
                                                                    }

                                                                }

                                                            }

                                                            if (count_kd > 0)
                                                            {
                                                                if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                {
                                                                    nilai_kd = Math.Round(jumlah_nilai_kd / count_kd, 2, MidpointRounding.AwayFromZero);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                nilai_kd = 0;
                                                            }

                                                            nilai_kd = Math.Round(nilai_kd, Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero);
                                                            lst_nilai_kd.Add(new NilaiWithKey
                                                            {
                                                                Key = item_rapor_struktur_nilai_kd.Kode.ToString(),
                                                                Nilai = nilai_kd
                                                            });
                                                            //end get struktur nilai det KP

                                                        }
                                                        //end get struktur nilai det KD

                                                        //get nilai ap
                                                        count_ap = 0;
                                                        nilai_ap = 0;
                                                        jumlah_nilai_ap = 0;
                                                        foreach (var item_nilai_kd in lst_nilai_kd)
                                                        {
                                                            if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                            {
                                                                nilai_ap += Math.Round((DAO_Rapor_StrukturNilai_KD.GetByID_Entity(item_nilai_kd.Key).BobotAP / 100) * item_nilai_kd.Nilai, 2, MidpointRounding.AwayFromZero);
                                                            }
                                                            else if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                jumlah_nilai_ap += item_nilai_kd.Nilai;
                                                            }
                                                            count_ap++;
                                                        }
                                                        if (count_ap > 0)
                                                        {
                                                            if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                nilai_ap = Math.Round(jumlah_nilai_ap / count_ap, 2, MidpointRounding.AwayFromZero);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            nilai_ap = 0;
                                                        }
                                                        nilai_ap = Math.Round(nilai_ap, 2, MidpointRounding.AwayFromZero);
                                                        lst_nilai_ap.Add(new NilaiWithKey
                                                        {
                                                            Key = item_rapor_struktur_nilai_ap.Kode.ToString(),
                                                            Nilai = nilai_ap
                                                        });
                                                        //end get nilai ap
                                                        //get nilai rapor
                                                        nilai_rapor = 0;
                                                        jumlah_nilai_rapor = 0;
                                                        count_nilai_rapor = 0;
                                                        foreach (var item_nilai_ap in lst_nilai_ap)
                                                        {
                                                            if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                            {
                                                                nilai_rapor += item_nilai_ap.Nilai;
                                                            }
                                                            else if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                jumlah_nilai_rapor += item_nilai_ap.Nilai;
                                                            }
                                                            count_nilai_rapor++;
                                                        }
                                                        if (count_nilai_rapor > 0)
                                                        {
                                                            if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                nilai_rapor = Math.Round(jumlah_nilai_rapor / count_nilai_rapor, 3, MidpointRounding.AwayFromZero);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            nilai_rapor = 0;
                                                        }
                                                        nilai_rapor = Math.Round(nilai_rapor, 0, MidpointRounding.AwayFromZero);
                                                        nilai_rapor_keterampilan = nilai_rapor;
                                                        lst_all_nilai_rapor.Add(new NilaiWithKey
                                                        {
                                                            Key = (id_kolom_ledger).ToString(),
                                                            Nilai = nilai_rapor_keterampilan,
                                                            JenisMapel = jenis_mapel
                                                        });
                                                        d_jumlah_keterampilan += nilai_rapor_keterampilan;
                                                        //end get nilai rapor
                                                        //-----------end nilai keterampilan---------------

                                                    }
                                                    //end get struktur nilai det AP

                                                    //get nilai rapor
                                                    nilai_rapor = 0;
                                                    jumlah_nilai_rapor = 0;
                                                    count_nilai_rapor = 0;
                                                    foreach (var item_nilai_ap in lst_nilai_ap)
                                                    {
                                                        if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                        {
                                                            nilai_rapor += item_nilai_ap.Nilai;
                                                        }
                                                        else if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                        {
                                                            jumlah_nilai_rapor += item_nilai_ap.Nilai;
                                                        }
                                                        count_nilai_rapor++;
                                                    }
                                                    if (count_nilai_rapor > 0)
                                                    {
                                                        if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                        {
                                                            nilai_rapor = Math.Round(jumlah_nilai_rapor / count_nilai_rapor, 3, MidpointRounding.AwayFromZero);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        nilai_rapor = 0;
                                                    }
                                                    nilai_rapor = Math.Round(nilai_rapor, 0, MidpointRounding.AwayFromZero);
                                                    nilai_rapor_keterampilan = nilai_rapor;
                                                    lst_all_nilai_rapor.Add(new NilaiWithKey
                                                    {
                                                        Key = (id_kolom_ledger).ToString(),
                                                        Nilai = nilai_rapor_keterampilan,
                                                        JenisMapel = jenis_mapel
                                                    });
                                                    id_kolom_ledger++;
                                                    //end get nilai rapor

                                                    //simpan nilai biologi or fisika pengetahuan
                                                    if (
                                                        m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_BIOLOGI ||
                                                        m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA
                                                    )
                                                    {
                                                        lst_nilai_ipa_praktik.Add(
                                                                Math.Round(nilai_rapor_keterampilan, Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero)
                                                            );
                                                    }
                                                    //end simpan nilai biologi or fisika pengetahuan

                                                    jumlah_mapel_rapor++;
                                                }
                                            }
                                            //end get struktur nilai
                                            if (DAO_Mapel.GetJenisMapel(m_struktur_nilai.Rel_Mapel.ToString()) == Application_Libs.Libs.JENIS_MAPEL.PILIHAN && nilai_rapor_pengetahuan == 0)
                                            {
                                                html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                            "&nbsp;" +
                                                                       "</td>" +
                                                                       "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                            "&nbsp;" +
                                                                       "</td>";
                                            }
                                            else
                                            {
                                                if (nilai_rapor_keterampilan > 0 && nilai_rapor_pengetahuan == 0)
                                                {
                                                    html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "&nbsp;" +
                                                                           "</td>" +
                                                                           "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "&nbsp;" +
                                                                           "</td>";
                                                }
                                                else
                                                {
                                                    html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center; " + (nilai_rapor_pengetahuan < m_struktur_nilai.KKM ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                                nilai_rapor_pengetahuan.ToString() +
                                                                           "</td>" +
                                                                           "<td style=\"" + css_cell_body + " text-align: center; " + (nilai_rapor_pengetahuan < m_struktur_nilai.KKM ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                                GetPredikatRapor(nilai_rapor_pengetahuan, tahun_ajaran) +
                                                                           "</td>";
                                                    i_jumlah_pengetahuan++;
                                                }                                                
                                            }
                                            if (DAO_Mapel.GetJenisMapel(m_struktur_nilai.Rel_Mapel.ToString()) == Application_Libs.Libs.JENIS_MAPEL.PILIHAN && nilai_rapor_keterampilan == 0)
                                            {
                                                html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                            "&nbsp;" +
                                                                       "</td>" +
                                                                       "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                            "&nbsp;" +
                                                                       "</td>";
                                            }
                                            else
                                            {
                                                if (nilai_rapor_pengetahuan > 0 && nilai_rapor_keterampilan == 0)
                                                {
                                                    html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "&nbsp;" +
                                                                           "</td>" +
                                                                           "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "&nbsp;" +
                                                                           "</td>";
                                                }
                                                else
                                                {
                                                    html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center; " + (nilai_rapor_keterampilan < m_struktur_nilai.KKM ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                                nilai_rapor_keterampilan.ToString() +
                                                                           "</td>" +
                                                                           "<td style=\"" + css_cell_body + " text-align: center; " + (nilai_rapor_keterampilan < m_struktur_nilai.KKM ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                                GetPredikatRapor(nilai_rapor_keterampilan, tahun_ajaran) +
                                                                           "</td>";
                                                    i_jumlah_keterampilan++;
                                                }                                                
                                            }

                                            //jika fisika tambahkan satu kolom
                                            if (item.Rel_Mapel.Trim().ToUpper() == Constantas_Kode_Mapel.KODE_FISIKA.Trim().ToUpper())
                                            {
                                                lst_kkm_nilai_rapor.Add(new NilaiWithKey
                                                {
                                                    Key = (jumlah_mapel_rapor + 1).ToString(),
                                                    Nilai = 70 //-99
                                                });

                                                nilai_rapor_pengetahuan = Math.Round(lst_nilai_ipa_pengetahuan.DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero);
                                                nilai_rapor_keterampilan = Math.Round(lst_nilai_ipa_praktik.DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero);

                                                if (DAO_Mapel.GetJenisMapel(m_struktur_nilai.Rel_Mapel.ToString()) == Application_Libs.Libs.JENIS_MAPEL.PILIHAN && nilai_rapor_pengetahuan == 0)
                                                {
                                                    html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "&nbsp;" +
                                                                           "</td>" +
                                                                           "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "&nbsp;" +
                                                                           "</td>";
                                                }
                                                else
                                                {
                                                    if (nilai_rapor_keterampilan > 0 && nilai_rapor_pengetahuan == 0)
                                                    {
                                                        html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>" +
                                                                               "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>";
                                                    }
                                                    else
                                                    {
                                                        html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center; " + (nilai_rapor_pengetahuan < m_struktur_nilai.KKM ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                                    nilai_rapor_pengetahuan.ToString() +
                                                                               "</td>" +
                                                                               "<td style=\"" + css_cell_body + " text-align: center; " + (nilai_rapor_pengetahuan < m_struktur_nilai.KKM ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                                    GetPredikatRapor(nilai_rapor_pengetahuan, tahun_ajaran) +
                                                                               "</td>";
                                                        i_jumlah_pengetahuan++;
                                                    }
                                                }
                                                if (DAO_Mapel.GetJenisMapel(m_struktur_nilai.Rel_Mapel.ToString()) == Application_Libs.Libs.JENIS_MAPEL.PILIHAN && nilai_rapor_keterampilan == 0)
                                                {
                                                    html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "&nbsp;" +
                                                                           "</td>" +
                                                                           "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "&nbsp;" +
                                                                           "</td>";
                                                }
                                                else
                                                {
                                                    if (nilai_rapor_pengetahuan > 0 && nilai_rapor_keterampilan == 0)
                                                    {
                                                        html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>" +
                                                                               "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>";
                                                    }
                                                    else
                                                    {
                                                        html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center; " + (nilai_rapor_keterampilan < m_struktur_nilai.KKM ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                                    nilai_rapor_keterampilan.ToString() +
                                                                               "</td>" +
                                                                               "<td style=\"" + css_cell_body + " text-align: center; " + (nilai_rapor_keterampilan < m_struktur_nilai.KKM ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                                    GetPredikatRapor(nilai_rapor_keterampilan, tahun_ajaran) +
                                                                               "</td>";
                                                        i_jumlah_keterampilan++;
                                                    }
                                                }

                                                id_kolom_ledger++;
                                                lst_all_nilai_rapor.Add(new NilaiWithKey
                                                {
                                                    Key = (id_kolom_ledger).ToString(),
                                                    Nilai = nilai_rapor_pengetahuan,
                                                    JenisMapel = jenis_mapel
                                                });
                                                d_jumlah_pengetahuan += nilai_rapor_pengetahuan;
                                                id_kolom_ledger += 2;

                                                lst_all_nilai_rapor.Add(new NilaiWithKey
                                                {
                                                    Key = (id_kolom_ledger).ToString(),
                                                    Nilai = nilai_rapor_keterampilan,
                                                    JenisMapel = jenis_mapel
                                                });
                                                d_jumlah_keterampilan += nilai_rapor_keterampilan;
                                                id_kolom_ledger++;
                                                jumlah_mapel_rapor++;
                                            }
                                            //end jika fisika                                               
                                        }

                                    }
                                    //end rapor non mulok
                                    
                                    lst_jumlah_nilai_keseluruhan.Add(new NilaiWithKey
                                    {
                                        Key = m_siswa.Kode.ToString(),
                                        Nilai = jumlah_keseluruhan
                                    });

                                    //absen
                                    string s_sakit = "-";
                                    string s_izin = "-";
                                    string s_alpa = "-";
                                    string s_terlambat = "-";

                                    List<DAO_SiswaAbsen.AbsenRekapRapor> lst_absen = new List<DAO_SiswaAbsen.AbsenRekapRapor>();
                                    lst_absen = DAO_SiswaAbsen.GetRekapAbsenRaporBySiswaByPeriode_Entity(
                                            m_siswa.Kode.ToString(),
                                            (m_rapor_arsip != null ? m_rapor_arsip.TanggalAwalAbsen : DateTime.MinValue),
                                            (m_rapor_arsip != null ? m_rapor_arsip.TanggalAkhirAbsen : DateTime.MinValue)
                                        );
                                    foreach (var absen in lst_absen)
                                    {
                                        if (absen.Absen == Libs.JENIS_ABSENSI.SAKIT.Substring(0, 1)) s_sakit = absen.Jumlah.ToString();
                                        if (absen.Absen == Libs.JENIS_ABSENSI.IZIN.Substring(0, 1)) s_izin = absen.Jumlah.ToString();
                                        if (absen.Absen == Libs.JENIS_ABSENSI.ALPA.Substring(0, 1)) s_alpa = absen.Jumlah.ToString();
                                        if (absen.Absen == Libs.JENIS_ABSENSI.TERLAMBAT.Substring(0, 1)) s_terlambat = absen.Jumlah.ToString();
                                    }

                                    if (tahun_ajaran == "2020/2021")
                                    {
                                        if (lst_rekap_det.FindAll(m0 => m0.Rel_Siswa.ToString().Replace(";", "").ToUpper() == m_siswa.Kode.ToString().Replace(";", "").ToUpper().Trim()).Count > 0)
                                        {
                                            SiswaAbsenRekapDet m_rekap_absen_siswa = lst_rekap_det.FindAll(m0 => m0.Rel_Siswa.ToString().Replace(";", "").ToUpper() == m_siswa.Kode.ToString().Replace(";", "").ToUpper().Trim()).FirstOrDefault();
                                            if (m_rekap_absen_siswa != null)
                                            {
                                                if (m_rekap_absen_siswa.Rel_Siswa != null)
                                                {
                                                    s_sakit = m_rekap_absen_siswa.Sakit;
                                                    s_izin = m_rekap_absen_siswa.Izin;
                                                    s_alpa = m_rekap_absen_siswa.Alpa;
                                                    s_terlambat = m_rekap_absen_siswa.Terlambat;
                                                }
                                            }
                                        }
                                    }
                                    html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center; \">" +
                                                                s_sakit +
                                                           "</td>" +
                                                           "<td style=\"" + css_cell_body + " text-align: center; \">" +
                                                                s_izin +
                                                           "</td>" +
                                                           "<td style=\"" + css_cell_body + " text-align: center; \">" +
                                                                s_alpa +
                                                           "</td>" +
                                                           "<td style=\"" + css_cell_body + " text-align: center; \">" +
                                                                s_terlambat +
                                                           "</td>";
                                    //end absen

                                    //nilai rata-rata & total
                                    decimal d_nilai_pengetahuan = Math.Round(Convert.ToDecimal(d_jumlah_pengetahuan) / Convert.ToDecimal(i_jumlah_pengetahuan), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP_2DES);
                                    decimal d_nilai_keterampilan = Math.Round(Convert.ToDecimal(d_jumlah_keterampilan) / Convert.ToDecimal(i_jumlah_keterampilan), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP_2DES);
                                    html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center; " + (d_nilai_pengetahuan < KKM_GLOBAL ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                d_nilai_pengetahuan +
                                                           "</td>" +
                                                           "<td style=\"" + css_cell_body + " text-align: center; " + (d_nilai_keterampilan < KKM_GLOBAL ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                d_nilai_keterampilan +
                                                           "</td>" +
                                                           "<td style=\"" + css_cell_body + " text-align: center; \">" +
                                                                d_jumlah_pengetahuan +
                                                           "</td>" +
                                                           "<td style=\"" + css_cell_body + " text-align: center; \">" +
                                                                d_jumlah_keterampilan +
                                                           "</td>";

                                    html_row_body += "<tr>" +
                                                        html_row_body_siswa +
                                                     "</tr>";

                                    nomor++;
                                }
                                // end foreach siswa

                                List<Rapor_Desain_Det> lst_rapor = DAO_Rapor_Desain_Det.GetAllByHeader_Entity(rapor_desain.Kode.ToString()).FindAll(m => m.Nomor.Trim() != "" && m.Rel_Mapel.Trim() != "");
                                for (int i = 1; 
                                     i <= (lst_rapor.Count * 4) + (4); 
                                     i++)
                                {
                                    decimal rata_rata = Math.Round(lst_all_nilai_rapor.FindAll(m => m.Key == (i + jumlah_awal_col_ledger).ToString() && ((m.JenisMapel == Libs.JENIS_MAPEL.PILIHAN && m.Nilai > 0) || m.JenisMapel != Libs.JENIS_MAPEL.PILIHAN)).Select(m => m.Nilai).DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP);
                                    html_row_rata_rata += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                (
                                                                    rata_rata > 0
                                                                    ? Math.Round(rata_rata, 0).ToString()
                                                                    : ""
                                                                ) +
                                                          "</td>";
                                }
                                for (int i = 1; i <= (DAO_Rapor_Desain_Det.GetAllByHeader_Entity(rapor_desain.Kode.ToString()).FindAll(m => m.Nomor.Trim() != "" && m.Rel_Mapel.Trim() != "").Count + 1); i++)
                                {
                                    decimal kkm = 0;
                                    NilaiWithKey m_kkm = lst_kkm_nilai_rapor.FindAll(m => m.Key == i.ToString()).FirstOrDefault();
                                    if (m_kkm != null)
                                    {
                                        if (m_kkm.Key != null)
                                        {
                                            kkm = m_kkm.Nilai;
                                        }
                                    }
                                    html_row_kkm += "<td colspan=\"4\" style=\"" + css_cell_body + " text-align: center;\">" +
                                                        (
                                                            kkm == -99
                                                            ? ""
                                                            : Math.Round(kkm, 0).ToString()
                                                        ) +
                                                    "</td>";
                                }
                                decimal rata_rata_nilai_keseluruhan = Math.Round(lst_jumlah_nilai_keseluruhan.Select(m => m.Nilai).DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP);
                                decimal rata_rata_nilai_rata_rata_rapor = Math.Round(lst_nilai_rapor.Select(m => m.Nilai).DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP);
                                html_row_body += "<tr>" +
                                                    html_row_rata_rata +
                                                 "</tr>" +
                                                 "<tr>" +
                                                    html_row_kkm +
                                                 "</tr>";

                            }

                        }
                    }

                }
            }

            html_row_header = "<tr>" +
                                html_row_header +
                              "</tr>" +
                              "<tr>" +
                                html_row_mapel +
                              "</tr>" +
                              "<tr>" +
                                html_row_jenis_nilai +
                              "</tr>" +
                              html_row_body;

            html = "<table style=\"border-collapse: collapse;\">" +
                        html_row_header +
                   "</table>" +
                   "<br />" +
                   "<table style=\"width: 100%;\">" +
                    "<tr>" +
                        "<td style=\"width: width: 20%; text-align: left;\">" +
                            "Mengetahui,<br />" +
                            "Kepala Sekolah" +
                            "<br /><br /><br />" +
                            "<br /><br /><br />" +
                            m_rapor_arsip.KepalaSekolah +
                        "</td>" +
                        "<td style=\"width: width: 20%; text-align: center;\">" +
                            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                        "</td>" +
                        "<td style=\"width: width: 20%; text-align: center;\">" +
                            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                        "</td>" +
                        "<td style=\"width: width: 20%; text-align: center;\">" +
                            "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                        "</td>" +
                        "<td style=\"width: width: 20%; text-align: left;\">" +
                            "Jakarta, " + Libs.GetTanggalIndonesiaFromDate(m_rapor_arsip.TanggalRapor, false) +
                            "<br />" +
                            "Wali Kelas" +
                            "<br /><br /><br />" +
                            "<br /><br /><br />" +
                            s_walikelas +
                        "</td>" +
                    "</tr>" +
                   "</table>" +
                   "<br />" +
                   "<br />";

            return html;
        }

        public static string GetHTMLLedger_KURTILAS_LENGKAP(
            System.Web.UI.Page page,
            string tahun_ajaran,
            string semester,
            string rel_kelas_det,
            string rel_siswa = ""
        )
        {
            List<SiswaAbsenRekap> lst_absen_rekap = DAO_SiswaAbsenRekap.GetAllByTABySMByKelasDet_Entity(tahun_ajaran, semester, rel_kelas_det).FindAll(m0 => m0.Rel_Mapel.Trim() == "" && m0.Jenis.ToString().ToUpper() == TipeRapor.SEMESTER.ToUpper().Trim());
            List<SiswaAbsenRekapDet> lst_rekap_det = new List<SiswaAbsenRekapDet>();
            if (lst_absen_rekap.Count == 1)
            {
                SiswaAbsenRekap m_rekap_absensi = lst_absen_rekap.FirstOrDefault();
                if (m_rekap_absensi != null)
                {
                    if (m_rekap_absensi.TahunAjaran != null)
                    {
                        lst_rekap_det = DAO_SiswaAbsenRekapDet.GetAllByHeader_Entity(m_rekap_absensi.Kode.ToString());
                    }
                }
            }

            List<Mapel> lst_mapel = DAO_Mapel.GetAllBySekolah_Entity(GetUnitSekolah().Kode.ToString());

            string s_walikelas = "";
            List<FormasiGuruKelas> lst_formasi_guru_kelas = DAO_FormasiGuruKelas.GetByUnitByTABySM_Entity(
                        GetUnitSekolah().Kode.ToString(), tahun_ajaran, semester
                    ).FindAll(m => m.Rel_KelasDet.ToString().ToUpper() == rel_kelas_det.Trim().ToUpper());
            if (lst_formasi_guru_kelas != null)
            {
                if (lst_formasi_guru_kelas.Count > 0)
                {
                    FormasiGuruKelas m_guru_kelas = lst_formasi_guru_kelas.FirstOrDefault();
                    if (m_guru_kelas != null)
                    {
                        if (m_guru_kelas.TahunAjaran != null)
                        {
                            Pegawai m_pegawai = DAO_Pegawai.GetByID_Entity(m_guru_kelas.Rel_GuruKelas);
                            if (m_pegawai != null)
                            {
                                if (m_pegawai.Nama != null)
                                {
                                    s_walikelas = m_pegawai.Nama;
                                }
                            }
                        }
                    }
                }
            }

            string css_cell_headers = "border-style: solid; border-width: 1px; text-align: center; font-weight: bold; padding: 5px;";
            string css_cell_body = "border-style: solid; border-width: 1px; padding: 3px;";

            string html = "";
            string html_row_header = "<td rowspan=\"3\" style=\"" + css_cell_headers + "\">" +
                                        "No" +
                                     "</td>" +
                                     "<td rowspan=\"3\" style=\"" + css_cell_headers + "\">" +
                                        "NIS" +
                                     "</td>" +
                                     "<td rowspan=\"3\" style=\"" + css_cell_headers + "\">" +
                                        "NAMA LENGKAP" +
                                     "</td>" +
                                     "<td rowspan=\"3\" style=\"" + css_cell_headers + "\">" +
                                        "L/P" +
                                     "</td>";
            string html_row_mapel = "";
            string html_row_jenis_nilai = "";
            string html_row_body = "";
            string html_tr_row_kkm = "";

            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelas_det);
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {

                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());
                    if (m_kelas != null)
                    {
                        if (m_kelas.Nama != null)
                        {

                            Rapor_Desain rapor_desain = DAO_Rapor_Desain.GetByTABySMByKelas_Entity(
                                    tahun_ajaran, semester, rel_kelas_det, DAO_Rapor_Desain.JenisRapor.Semester
                                ).FirstOrDefault();

                            List<Rapor_Desain_Det> lst_rapor_desain_det = DAO_Rapor_Desain_Det.GetAllByHeader_Entity(rapor_desain.Kode.ToString()).FindAll(m => m.Nomor.Trim() != "" && m.Rel_Mapel.Trim() != "");

                            //list cols header
                            int jml_colspan_mapel = 0;
                            int jumlah_mapel = 0;
                            int jumlah_kolom_ledger = 0;
                            html_row_mapel = "";
                            foreach (Rapor_Desain_Det item_desain_det in lst_rapor_desain_det)
                            {
                                //get struktur nilainya
                                Rapor_StrukturNilai m_struktur_nilai = DAO_Rapor_StrukturNilai.GetATop1ByTABySMByKelasByMapel_Entity(
                                        tahun_ajaran, semester, m_kelas.Kode.ToString(), item_desain_det.Rel_Mapel
                                    );

                                if (m_struktur_nilai.Is_PH_PTS_PAS)
                                {
                                    jumlah_kolom_ledger += 10;
                                    html_row_mapel += "<td colspan=\"10\" style=\"" + css_cell_headers + " font-size: 10px; \">" +
                                                        (
                                                            item_desain_det.Alias.Trim() != ""
                                                            ? item_desain_det.Alias
                                                            : item_desain_det.NamaMapelRapor.Trim().ToUpper()
                                                        ) +
                                                      "</td>";
                                    html_tr_row_kkm += "<td colspan=\"10\" style=\"" + css_cell_headers + " font-size: 10px; \">" +
                                                            "@" + (jumlah_mapel + 1) + "@" +
                                                       "</td>";
                                    html_row_jenis_nilai += "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                    "PH" +
                                                            "</td>" +
                                                            "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                    "PH (" + Math.Round(m_struktur_nilai.BobotPH).ToString() + "%)" +
                                                            "</td>" +
                                                            "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                    "PTS" +
                                                            "</td>" +
                                                            "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                    "PTS (" + Math.Round(m_struktur_nilai.BobotPTS).ToString() + "%)" +
                                                            "</td>" +
                                                            "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                    "PAS" +
                                                            "</td>" +
                                                            "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                    "PAS (" + Math.Round(m_struktur_nilai.BobotPAS).ToString() + "%)" +
                                                            "</td>" +
                                                            "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                    "P" +
                                                            "</td>" +
                                                            "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                    "PR" +
                                                            "</td>" +
                                                            "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                    "K" +
                                                            "</td>" +
                                                            "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                    "PR" +
                                                            "</td>";
                                    jumlah_mapel++;

                                    if (
                                        item_desain_det.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA
                                    )
                                    {
                                        if (item_desain_det.Rel_Mapel.Trim() != "")
                                        {
                                            jumlah_kolom_ledger += 4;
                                            html_row_mapel += "<td colspan=\"4\" style=\"" + css_cell_headers + " font-size: 10px; \">" +
                                                                "IPA" +
                                                              "</td>";
                                            html_tr_row_kkm += "<td colspan=\"4\" style=\"" + css_cell_headers + " font-size: 10px; \">" +
                                                                    "@" + (jumlah_mapel + 1) + "@" +
                                                               "</td>";
                                            html_row_jenis_nilai += "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                            "P" +
                                                                    "</td>" +
                                                                    "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                            "PR" +
                                                                    "</td>" +
                                                                    "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                            "K" +
                                                                    "</td>" +
                                                                    "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                            "PR" +
                                                                    "</td>";
                                            jumlah_mapel++;
                                        }
                                    }
                                }
                                else if (!m_struktur_nilai.Is_PH_PTS_PAS)
                                {
                                    jumlah_kolom_ledger += 4;
                                    html_row_mapel += "<td colspan=\"4\" style=\"" + css_cell_headers + " font-size: 10px; \">" +
                                                        (
                                                            item_desain_det.Alias.Trim() != ""
                                                            ? item_desain_det.Alias
                                                            : item_desain_det.NamaMapelRapor.Trim().ToUpper()
                                                        ) +
                                                      "</td>";
                                    html_tr_row_kkm += "<td colspan=\"4\" style=\"" + css_cell_headers + " font-size: 10px; \">" +
                                                            "@" + (jumlah_mapel + 1) + "@" +
                                                       "</td>";
                                    html_row_jenis_nilai += "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                    "P" +
                                                            "</td>" +
                                                            "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                    "PR" +
                                                            "</td>" +
                                                            "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                    "K" +
                                                            "</td>" +
                                                            "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                    "PR" +
                                                            "</td>";
                                    jumlah_mapel++;

                                    if (
                                        item_desain_det.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA
                                    )
                                    {
                                        if (item_desain_det.Rel_Mapel.Trim() != "")
                                        {
                                            jumlah_kolom_ledger += 4;
                                            html_row_mapel += "<td colspan=\"4\" style=\"" + css_cell_headers + " font-size: 10px; \">" +
                                                                "IPA" +
                                                              "</td>";
                                            html_tr_row_kkm += "<td colspan=\"4\" style=\"" + css_cell_headers + " font-size: 10px; \">" +
                                                                    "@" + (jumlah_mapel + 1) + "@" +
                                                               "</td>";
                                            html_row_jenis_nilai += "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                            "P" +
                                                                    "</td>" +
                                                                    "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                            "PR" +
                                                                    "</td>" +
                                                                    "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                            "K" +
                                                                    "</td>" +
                                                                    "<td style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                                            "PR" +
                                                                    "</td>";
                                            jumlah_mapel++;
                                        }
                                    }
                                }
                            }
                            jml_colspan_mapel = (jumlah_mapel * 4);

                            html_row_mapel += "<td rowspan=\"2\" style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                    "S" +
                                              "</td>" +
                                              "<td rowspan=\"2\" style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                    "I" +
                                              "</td>" +
                                              "<td rowspan=\"2\" style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                    "A" +
                                              "</td>" +
                                              "<td rowspan=\"2\" style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                    "T" +
                                              "</td>";

                            html_row_mapel += "<td rowspan=\"2\" style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                    "P" +
                                              "</td>" +
                                              "<td rowspan=\"2\" style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                    "K" +
                                              "</td>";

                            html_row_mapel += "<td rowspan=\"2\" style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                    "P" +
                                              "</td>" +
                                              "<td rowspan=\"2\" style=\"" + css_cell_headers + " width: 30px; font-size: 8.5px; \">" +
                                                    "K" +
                                              "</td>";
                            //end list cols header
                            if (rapor_desain != null)
                            {

                                List<DAO_Siswa.SiswaDataSimple> lst_siswa = DAO_Siswa.GetAllSiswaDataSimpleByTahunAjaranUnitKelas_Entity(
                                        m_kelas.Rel_Sekolah.ToString(),
                                        rel_kelas_det,
                                        tahun_ajaran,
                                        semester
                                    );
                                List<NilaiWithKey> lst_nilai_ap = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_nilai_kd = new List<NilaiWithKey>();
                                List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT> lst_nilai_siswa_det = new List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT>();
                                List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT> lst_nilai_siswa_det_by_periode = new List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT>();

                                lst_nilai_siswa_det_by_periode = DAO_Rapor_NilaiSiswa_Det.GetAllByTABySMByKelasDet_Entity(
                                        tahun_ajaran, semester, rel_kelas_det
                                    );

                                int id_kolom_ledger = 0;
                                int nomor = 1;
                                int jumlah_awal_col_ledger = 3;
                                int jumlah_mapel_rapor = 0;

                                if (rel_siswa.Trim() != "") lst_siswa = lst_siswa.FindAll(m => m.Kode.ToString() == rel_siswa);
                                List<NilaiWithKey> lst_all_nilai_rapor = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_kkm_nilai_rapor = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_jumlah_nilai_keseluruhan = new List<NilaiWithKey>();
                                List<NilaiWithKey> lst_nilai_rapor = new List<NilaiWithKey>();
                                decimal jumlah_keseluruhan = 0;

                                string html_row_rata_rata = "<td colspan=\"4\" style=\"" + css_cell_body + " font-weight: bold;\">" +
                                                                "RATA-RATA KELAS" +
                                                            "</td>";
                                string html_row_kkm = "<td colspan=\"4\" style=\"" + css_cell_body + " font-weight: bold;\">" +
                                                            "KKM" +
                                                      "</td>";

                                int id_mulai_looping_rata_rata = id_kolom_ledger + 1;
                                int id_mulai_looping_kkm = jumlah_mapel_rapor + 1;
                                int id_siswa = 0;

                                string html_tr_row_rata_rata = "";

                                decimal d_jumlah_pengetahuan = 0;
                                decimal d_jumlah_keterampilan = 0;
                                int i_jumlah_pengetahuan = 0;
                                int i_jumlah_keterampilan = 0;
                                foreach (DAO_Siswa.SiswaDataSimple m_siswa in lst_siswa.OrderBy(m => m.Nama))
                                {
                                    id_siswa++;
                                    if (id_siswa == 1)
                                    {
                                        html_tr_row_rata_rata = "";
                                    }

                                    jumlah_keseluruhan = 0;
                                    jumlah_mapel_rapor = 0;

                                    d_jumlah_pengetahuan = 0;
                                    d_jumlah_keterampilan = 0;
                                    i_jumlah_pengetahuan = 0;
                                    i_jumlah_keterampilan = 0;

                                    string html_row_body_siswa = "";
                                    string html_item = "";

                                    html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                nomor.ToString() +
                                                           "</td>" +
                                                           "<td style=\"" + css_cell_body + "\">" +
                                                                m_siswa.NISSekolah +
                                                           "</td>" +
                                                           "<td style=\"" + css_cell_body + " white-space: nowrap;\">" +
                                                                Libs.GetPerbaikiEjaanNama(m_siswa.Nama) +
                                                           "</td>" +
                                                           "<td style=\"" + css_cell_body + "\">" +
                                                                m_siswa.JenisKelamin +
                                                           "</td>";

                                    lst_kkm_nilai_rapor.Clear();
                                    id_kolom_ledger = jumlah_awal_col_ledger;

                                    List<decimal> lst_nilai_ipa_pengetahuan = new List<decimal>();
                                    List<decimal> lst_nilai_ipa_praktik = new List<decimal>();
                                    List<decimal> lst_kp_tugas = new List<decimal>();
                                    List<decimal> lst_kp_uh_terakhir = new List<decimal>();
                                    List<decimal> lst_kp_uh_non_terakhir = new List<decimal>();

                                    lst_nilai_ipa_pengetahuan.Clear();
                                    lst_nilai_ipa_praktik.Clear();
                                    //nilai non mulok
                                    foreach (Rapor_Desain_Det item in DAO_Rapor_Desain_Det.GetAllByHeader_Entity(rapor_desain.Kode.ToString()).FindAll(m => m.Nomor.Trim() != "" && m.Rel_Mapel.Trim() != ""))
                                    {

                                        if (item.Rel_Mapel.Trim() != "")
                                        {
                                            string jenis_mapel = "";
                                            if (lst_mapel.FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == item.Rel_Mapel.ToUpper().Trim()).Count > 0)
                                            {
                                                jenis_mapel = lst_mapel.FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == item.Rel_Mapel.ToUpper().Trim()).FirstOrDefault().Jenis;
                                            }

                                            lst_nilai_siswa_det = lst_nilai_siswa_det_by_periode.FindAll(
                                                    m => m.Rel_Mapel == item.Rel_Mapel &&
                                                         m.Rel_Siswa == m.Rel_Siswa
                                                );

                                            //get struktur nilainya
                                            Rapor_StrukturNilai m_struktur_nilai = DAO_Rapor_StrukturNilai.GetATop1ByTABySMByKelasByMapel_Entity(
                                                    tahun_ajaran, semester, m_kelas.Kode.ToString(), item.Rel_Mapel
                                                );

                                            decimal nilai_rapor = 0;
                                            decimal jumlah_nilai_rapor = 0;
                                            decimal count_nilai_rapor = 0;

                                            decimal nilai_rapor_pengetahuan = 0;
                                            decimal nilai_rapor_keterampilan = 0;

                                            string s_nilai_ph = "";
                                            string s_nilai_pts = "";
                                            string s_nilai_pas = "";

                                            string s_nilai_ph_bobot = "";
                                            string s_nilai_pts_bobot = "";
                                            string s_nilai_pas_bobot = "";

                                            int id_kolom_ledger_pengetahuan = 0;
                                            int id_kolom_ledger_keterampilan = 0;
                                            if (m_struktur_nilai != null)
                                            {
                                                if (m_struktur_nilai.TahunAjaran != null)
                                                {
                                                    lst_kp_tugas.Clear();
                                                    lst_kp_uh_terakhir.Clear();
                                                    lst_kp_uh_non_terakhir.Clear();

                                                    lst_kkm_nilai_rapor.Add(new NilaiWithKey
                                                    {
                                                        Key = (jumlah_mapel_rapor + 1).ToString(),
                                                        Nilai = m_struktur_nilai.KKM
                                                    });
                                                    //-----------nilai pengetahuan---------------
                                                    id_kolom_ledger++;
                                                    //get struktur nilai det AP
                                                    List<Rapor_StrukturNilai_AP> lst_rapor_struktur_nilai_ap = DAO_Rapor_StrukturNilai_AP.GetAllByHeaderByJenisAspekPenilaian_Entity(
                                                            m_struktur_nilai.Kode.ToString(), DAO_Rapor_StrukturNilai_AP.JenisAspekPenilaian.Pengetahuan
                                                        );
                                                    lst_nilai_ap.Clear();
                                                    decimal jumlah_nilai_ap = 0;
                                                    decimal nilai_ap = 0;
                                                    int count_ap = 0;
                                                    foreach (var item_rapor_struktur_nilai_ap in lst_rapor_struktur_nilai_ap)
                                                    {

                                                        //get struktur nilai det KD
                                                        List<Rapor_StrukturNilai_KD> lst_rapor_struktur_nilai_kd = DAO_Rapor_StrukturNilai_KD.GetAllByHeader_Entity(
                                                                item_rapor_struktur_nilai_ap.Kode.ToString()
                                                            );
                                                        lst_nilai_kd.Clear();
                                                        foreach (var item_rapor_struktur_nilai_kd in lst_rapor_struktur_nilai_kd)
                                                        {

                                                            Rapor_KompetensiDasar m_kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(item_rapor_struktur_nilai_kd.Rel_Rapor_KompetensiDasar.ToString());

                                                            //get struktur nilai det KP
                                                            List<Rapor_StrukturNilai_KP> lst_rapor_struktur_nilai_kp = DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(
                                                                item_rapor_struktur_nilai_kd.Kode.ToString()
                                                            );
                                                            decimal jumlah_nilai_kd = 0;
                                                            decimal nilai_kd = 0;
                                                            int count_kd = 0;
                                                            int id_kp = 1;
                                                            foreach (var item_rapor_struktur_nilai_kp in lst_rapor_struktur_nilai_kp)
                                                            {
                                                                string nilai = "";
                                                                Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(item_rapor_struktur_nilai_kp.Rel_Rapor_KomponenPenilaian.ToString());
                                                                if (m_kp != null)
                                                                {

                                                                    Rapor_NilaiSiswa_Det m_nilai_det = lst_nilai_siswa_det.FindAll(
                                                                        m => m.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper() &&
                                                                             m.Rel_Mapel.ToString().Trim().ToUpper() == m_struktur_nilai.Rel_Mapel.ToString().Trim().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_AP.Trim().ToUpper() == item_rapor_struktur_nilai_ap.Kode.ToString().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_KD.Trim().ToUpper() == item_rapor_struktur_nilai_kd.Kode.ToString().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_KP.Trim().ToUpper() == item_rapor_struktur_nilai_kp.Kode.ToString().ToUpper()
                                                                    ).FirstOrDefault();

                                                                    if (m_nilai_det == null)
                                                                    {
                                                                        m_nilai_det = new Rapor_NilaiSiswa_Det();
                                                                        m_nilai_det.Nilai = "0";
                                                                        m_nilai_det.PB = "0";
                                                                    }
                                                                    if (m_nilai_det != null)
                                                                    {
                                                                        if (m_nilai_det.Nilai.Trim() == "") m_nilai_det.Nilai = "0";
                                                                        if (m_nilai_det.Nilai.Trim() != "")
                                                                        {
                                                                            nilai = m_nilai_det.Nilai.Trim();
                                                                            if (m_nilai_det.PB == null) m_nilai_det.PB = "";
                                                                            if (item_rapor_struktur_nilai_kp.IsAdaPB && m_nilai_det.PB.Trim() != nilai && m_nilai_det.PB.Trim() != "" && Libs.IsAngka(m_nilai_det.PB.Trim().Replace("=", "")))
                                                                            {
                                                                                nilai = m_nilai_det.PB.Trim().Replace("=", "");
                                                                            }
                                                                            
                                                                            if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                                            {
                                                                                nilai_kd += Math.Round((item_rapor_struktur_nilai_kp.BobotNK / 100) * Libs.GetStringToDecimal(nilai), 2, MidpointRounding.AwayFromZero);
                                                                            }
                                                                            else if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                            {
                                                                                jumlah_nilai_kd += Libs.GetStringToDecimal(nilai);
                                                                            }
                                                                            count_kd++;
                                                                        }
                                                                    }

                                                                }

                                                                if (nilai.Trim() != "")
                                                                {
                                                                    if (
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "TUGAS" ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PH") >= 0 ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENILAIAN HARIAN") >= 0 ||
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PENGETAHUAN") >= 0
                                                                        )
                                                                    {
                                                                        lst_kp_tugas.Add(
                                                                                Libs.GetStringToDecimal(nilai)
                                                                            );
                                                                    }
                                                                    else if (
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()) == "UH"
                                                                        )
                                                                    {
                                                                        if (id_kp < lst_rapor_struktur_nilai_kp.Count)
                                                                        {
                                                                            lst_kp_uh_non_terakhir.Add(
                                                                                    Libs.GetStringToDecimal(nilai)
                                                                                );
                                                                        }
                                                                        else if (id_kp == lst_rapor_struktur_nilai_kp.Count)
                                                                        {
                                                                            lst_kp_uh_terakhir.Add(
                                                                                    Libs.GetStringToDecimal(nilai)
                                                                                );
                                                                        }
                                                                    }
                                                                    else if (
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PTS") >= 0
                                                                        )
                                                                    {
                                                                        lst_kp_uh_non_terakhir.Add(
                                                                                Libs.GetStringToDecimal(nilai)
                                                                            );
                                                                    }
                                                                    else if (
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama.Trim().ToUpper()).IndexOf("PAS") >= 0
                                                                        )
                                                                    {
                                                                        lst_kp_uh_terakhir.Add(
                                                                                Libs.GetStringToDecimal(nilai)
                                                                            );
                                                                    }
                                                                }

                                                                id_kp++;

                                                            }

                                                            if (count_kd > 0)
                                                            {
                                                                if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                {
                                                                    nilai_kd = Math.Round(jumlah_nilai_kd / count_kd, 2, MidpointRounding.AwayFromZero);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                nilai_kd = 0;
                                                            }

                                                            nilai_kd = Math.Round(nilai_kd, 2, MidpointRounding.AwayFromZero);
                                                            lst_nilai_kd.Add(new NilaiWithKey
                                                            {
                                                                Key = item_rapor_struktur_nilai_kd.Kode.ToString(),
                                                                Nilai = nilai_kd
                                                            });
                                                            //end get struktur nilai det KP

                                                        }
                                                        //end get struktur nilai det KD

                                                        //get nilai ap
                                                        count_ap = 0;
                                                        nilai_ap = 0;
                                                        jumlah_nilai_ap = 0;
                                                        foreach (var item_nilai_kd in lst_nilai_kd)
                                                        {
                                                            if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                            {
                                                                nilai_ap += Math.Round((DAO_Rapor_StrukturNilai_KD.GetByID_Entity(item_nilai_kd.Key).BobotAP / 100) * item_nilai_kd.Nilai, 2, MidpointRounding.AwayFromZero);
                                                            }
                                                            else if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                jumlah_nilai_ap += item_nilai_kd.Nilai;
                                                            }
                                                            count_ap++;
                                                        }
                                                        if (count_ap > 0)
                                                        {
                                                            if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                nilai_ap = Math.Round(jumlah_nilai_ap / count_ap, 2, MidpointRounding.AwayFromZero);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            nilai_ap = 0;
                                                        }
                                                        nilai_ap = Math.Round(nilai_ap, 2, MidpointRounding.AwayFromZero);
                                                        lst_nilai_ap.Add(new NilaiWithKey
                                                        {
                                                            Key = item_rapor_struktur_nilai_ap.Kode.ToString(),
                                                            Nilai = nilai_ap
                                                        });
                                                        //end get nilai ap

                                                    }
                                                    //end get struktur nilai det AP

                                                    //get nilai rapor
                                                    nilai_rapor = 0;
                                                    jumlah_nilai_rapor = 0;
                                                    count_nilai_rapor = 0;
                                                    if (m_struktur_nilai.Is_PH_PTS_PAS)
                                                    {
                                                        s_nilai_ph = (
                                                                lst_kp_tugas.Count > 0
                                                                ? Math.Round(lst_kp_tugas.DefaultIfEmpty().Average(), 2, MidpointRounding.AwayFromZero).ToString()
                                                                : "0"
                                                            );
                                                        s_nilai_pts = (
                                                                lst_kp_uh_non_terakhir.Count > 0
                                                                ? Math.Round(lst_kp_uh_non_terakhir.DefaultIfEmpty().Average(), 2, MidpointRounding.AwayFromZero).ToString()
                                                                : "0"
                                                            );
                                                        
                                                        s_nilai_pas = (
                                                                lst_kp_uh_terakhir.Count > 0
                                                                ? Math.Round(lst_kp_uh_terakhir.DefaultIfEmpty().Average(), 2, MidpointRounding.AwayFromZero).ToString()
                                                                : "0"
                                                            );

                                                        //s_nilai_ph_bobot = Math.Round(
                                                        //                    (Libs.GetStringToDecimal(s_nilai_ph) * (m_struktur_nilai.BobotPH / 100))
                                                        //                   , 2, MidpointRounding.AwayFromZero).ToString();
                                                        //s_nilai_pts_bobot = Math.Round(
                                                        //                    (Libs.GetStringToDecimal(s_nilai_pts) * (m_struktur_nilai.BobotPTS / 100))
                                                        //                   , 2, MidpointRounding.AwayFromZero).ToString();
                                                        //s_nilai_pas_bobot = Math.Round(
                                                        //                    (Libs.GetStringToDecimal(s_nilai_pas) * (m_struktur_nilai.BobotPAS / 100))
                                                        //                   , 2, MidpointRounding.AwayFromZero).ToString();
                                                        s_nilai_ph_bobot = Math.Round(
                                                                            (Libs.GetStringToDecimal(s_nilai_ph) * (m_struktur_nilai.BobotPH / 100))
                                                                           , 1, MidpointRounding.AwayFromZero).ToString();
                                                        s_nilai_pts_bobot = Math.Round(
                                                                            (Libs.GetStringToDecimal(s_nilai_pts) * (m_struktur_nilai.BobotPTS / 100))
                                                                           , 2, MidpointRounding.AwayFromZero).ToString();
                                                        s_nilai_pas_bobot = Math.Round(
                                                                            (Libs.GetStringToDecimal(s_nilai_pas) * (m_struktur_nilai.BobotPAS / 100))
                                                                           , 1, MidpointRounding.AwayFromZero).ToString();

                                                        nilai_rapor = Libs.GetStringToDecimal(s_nilai_ph_bobot) +
                                                                      Libs.GetStringToDecimal(s_nilai_pts_bobot) +
                                                                      Libs.GetStringToDecimal(s_nilai_pas_bobot);
                                                    }
                                                    else if (!m_struktur_nilai.Is_PH_PTS_PAS)
                                                    {
                                                        foreach (var item_nilai_ap in lst_nilai_ap)
                                                        {
                                                            if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                            {
                                                                nilai_rapor += item_nilai_ap.Nilai;
                                                            }
                                                            else if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                jumlah_nilai_rapor += item_nilai_ap.Nilai;
                                                            }
                                                            count_nilai_rapor++;
                                                        }
                                                        if (count_nilai_rapor > 0)
                                                        {
                                                            if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                nilai_rapor = Math.Round(jumlah_nilai_rapor / count_nilai_rapor, 3, MidpointRounding.AwayFromZero);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            nilai_rapor = 0;
                                                        }
                                                    }

                                                    nilai_rapor = Math.Round(nilai_rapor, 0, MidpointRounding.AwayFromZero);
                                                    nilai_rapor_pengetahuan = nilai_rapor;
                                                    lst_all_nilai_rapor.Add(new NilaiWithKey
                                                    {
                                                        Key = (id_kolom_ledger).ToString(),
                                                        Nilai = nilai_rapor_pengetahuan
                                                    });
                                                    id_kolom_ledger_pengetahuan = id_kolom_ledger;
                                                    d_jumlah_pengetahuan += nilai_rapor_pengetahuan;
                                                    id_kolom_ledger += 2;
                                                    //end get nilai rapor

                                                    //simpan nilai biologi or fisika pengetahuan
                                                    if (
                                                        m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_BIOLOGI ||
                                                        m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA
                                                    )
                                                    {
                                                        lst_nilai_ipa_pengetahuan.Add(
                                                                Math.Round(nilai_rapor_pengetahuan, Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero)
                                                            );
                                                    }
                                                    //end simpan nilai biologi or fisika pengetahuan
                                                    //-----------end nilai pengetahuan---------------

                                                    //-----------nilai keterampilan---------------
                                                    //get struktur nilai det AP
                                                    lst_rapor_struktur_nilai_ap = DAO_Rapor_StrukturNilai_AP.GetAllByHeaderByJenisAspekPenilaian_Entity(
                                                            m_struktur_nilai.Kode.ToString(), DAO_Rapor_StrukturNilai_AP.JenisAspekPenilaian.Keterampilan
                                                        );
                                                    lst_nilai_ap.Clear();
                                                    jumlah_nilai_ap = 0;
                                                    nilai_ap = 0;
                                                    count_ap = 0;
                                                    foreach (var item_rapor_struktur_nilai_ap in lst_rapor_struktur_nilai_ap)
                                                    {

                                                        //get struktur nilai det KD
                                                        List<Rapor_StrukturNilai_KD> lst_rapor_struktur_nilai_kd = DAO_Rapor_StrukturNilai_KD.GetAllByHeader_Entity(
                                                                item_rapor_struktur_nilai_ap.Kode.ToString()
                                                            );
                                                        lst_nilai_kd.Clear();
                                                        foreach (var item_rapor_struktur_nilai_kd in lst_rapor_struktur_nilai_kd)
                                                        {

                                                            //get struktur nilai det KP
                                                            List<Rapor_StrukturNilai_KP> lst_rapor_struktur_nilai_kp = DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(
                                                                item_rapor_struktur_nilai_kd.Kode.ToString()
                                                            );
                                                            decimal jumlah_nilai_kd = 0;
                                                            decimal nilai_kd = 0;
                                                            int count_kd = 0;
                                                            foreach (var item_rapor_struktur_nilai_kp in lst_rapor_struktur_nilai_kp)
                                                            {
                                                                string nilai = "";
                                                                Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(item_rapor_struktur_nilai_kp.Rel_Rapor_KomponenPenilaian.ToString());
                                                                if (m_kp != null)
                                                                {

                                                                    Rapor_NilaiSiswa_Det m_nilai_det = lst_nilai_siswa_det.FindAll(
                                                                        m => m.Rel_Siswa.ToString().Trim().ToUpper() == m_siswa.Kode.ToString().Trim().ToUpper() &&
                                                                             m.Rel_Mapel.ToString().Trim().ToUpper() == m_struktur_nilai.Rel_Mapel.ToString().Trim().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_AP.Trim().ToUpper() == item_rapor_struktur_nilai_ap.Kode.ToString().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_KD.Trim().ToUpper() == item_rapor_struktur_nilai_kd.Kode.ToString().ToUpper() &&
                                                                             m.Rel_Rapor_StrukturNilai_KP.Trim().ToUpper() == item_rapor_struktur_nilai_kp.Kode.ToString().ToUpper()
                                                                    ).FirstOrDefault();

                                                                    if (m_nilai_det == null)
                                                                    {
                                                                        m_nilai_det = new Rapor_NilaiSiswa_Det();
                                                                        m_nilai_det.Nilai = "0";
                                                                        m_nilai_det.PB = "0";
                                                                    }
                                                                    if (m_nilai_det != null)
                                                                    {
                                                                        if (m_nilai_det.Nilai.Trim() == "") m_nilai_det.Nilai = "0";
                                                                        if (m_nilai_det.Nilai.Trim() != "")
                                                                        {
                                                                            nilai = m_nilai_det.Nilai.Trim();
                                                                            if (m_nilai_det.PB == null) m_nilai_det.PB = "";
                                                                            if (item_rapor_struktur_nilai_kp.IsAdaPB && m_nilai_det.PB.Trim() != nilai && m_nilai_det.PB.Trim() != "" && Libs.IsAngka(m_nilai_det.PB.Trim().Replace("=", "")))
                                                                            {
                                                                                nilai = m_nilai_det.PB.Trim().Replace("=", "");
                                                                            }

                                                                            if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                                            {
                                                                                nilai_kd += Math.Round((item_rapor_struktur_nilai_kp.BobotNK / 100) * Libs.GetStringToDecimal(nilai), 2, MidpointRounding.AwayFromZero);
                                                                            }
                                                                            else if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                            {
                                                                                jumlah_nilai_kd += Libs.GetStringToDecimal(nilai);
                                                                            }
                                                                            count_kd++;
                                                                        }
                                                                    }

                                                                }

                                                            }

                                                            if (count_kd > 0)
                                                            {
                                                                if (item_rapor_struktur_nilai_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                                {
                                                                    nilai_kd = Math.Round(jumlah_nilai_kd / count_kd, 2, MidpointRounding.AwayFromZero);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                nilai_kd = 0;
                                                            }

                                                            nilai_kd = Math.Round(nilai_kd, Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero);
                                                            lst_nilai_kd.Add(new NilaiWithKey
                                                            {
                                                                Key = item_rapor_struktur_nilai_kd.Kode.ToString(),
                                                                Nilai = nilai_kd
                                                            });
                                                            //end get struktur nilai det KP

                                                        }
                                                        //end get struktur nilai det KD

                                                        //get nilai ap
                                                        count_ap = 0;
                                                        nilai_ap = 0;
                                                        jumlah_nilai_ap = 0;
                                                        foreach (var item_nilai_kd in lst_nilai_kd)
                                                        {
                                                            if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                            {
                                                                nilai_ap += Math.Round((DAO_Rapor_StrukturNilai_KD.GetByID_Entity(item_nilai_kd.Key).BobotAP / 100) * item_nilai_kd.Nilai, 2, MidpointRounding.AwayFromZero);
                                                            }
                                                            else if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                jumlah_nilai_ap += item_nilai_kd.Nilai;
                                                            }
                                                            count_ap++;
                                                        }
                                                        if (count_ap > 0)
                                                        {
                                                            if (item_rapor_struktur_nilai_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                nilai_ap = Math.Round(jumlah_nilai_ap / count_ap, 2, MidpointRounding.AwayFromZero);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            nilai_ap = 0;
                                                        }
                                                        nilai_ap = Math.Round(nilai_ap, 2, MidpointRounding.AwayFromZero);
                                                        lst_nilai_ap.Add(new NilaiWithKey
                                                        {
                                                            Key = item_rapor_struktur_nilai_ap.Kode.ToString(),
                                                            Nilai = nilai_ap
                                                        });
                                                        //end get nilai ap
                                                        //get nilai rapor
                                                        nilai_rapor = 0;
                                                        jumlah_nilai_rapor = 0;
                                                        count_nilai_rapor = 0;
                                                        foreach (var item_nilai_ap in lst_nilai_ap)
                                                        {
                                                            if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                            {
                                                                nilai_rapor += item_nilai_ap.Nilai;
                                                            }
                                                            else if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                jumlah_nilai_rapor += item_nilai_ap.Nilai;
                                                            }
                                                            count_nilai_rapor++;
                                                        }
                                                        if (count_nilai_rapor > 0)
                                                        {
                                                            if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                            {
                                                                nilai_rapor = Math.Round(jumlah_nilai_rapor / count_nilai_rapor, 3, MidpointRounding.AwayFromZero);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            nilai_rapor = 0;
                                                        }
                                                        nilai_rapor = Math.Round(nilai_rapor, 0, MidpointRounding.AwayFromZero);
                                                        nilai_rapor_keterampilan = nilai_rapor;
                                                        lst_all_nilai_rapor.Add(new NilaiWithKey
                                                        {
                                                            Key = (id_kolom_ledger).ToString(),
                                                            Nilai = nilai_rapor_keterampilan,
                                                            JenisMapel = jenis_mapel
                                                        });
                                                        d_jumlah_keterampilan += nilai_rapor_keterampilan;
                                                        //end get nilai rapor
                                                        //-----------end nilai keterampilan---------------

                                                    }
                                                    //end get struktur nilai det AP

                                                    //get nilai rapor
                                                    nilai_rapor = 0;
                                                    jumlah_nilai_rapor = 0;
                                                    count_nilai_rapor = 0;
                                                    foreach (var item_nilai_ap in lst_nilai_ap)
                                                    {
                                                        if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                        {
                                                            nilai_rapor += item_nilai_ap.Nilai;
                                                        }
                                                        else if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                        {
                                                            jumlah_nilai_rapor += item_nilai_ap.Nilai;
                                                        }
                                                        count_nilai_rapor++;
                                                    }
                                                    if (count_nilai_rapor > 0)
                                                    {
                                                        if (m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                                        {
                                                            nilai_rapor = Math.Round(jumlah_nilai_rapor / count_nilai_rapor, 3, MidpointRounding.AwayFromZero);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        nilai_rapor = 0;
                                                    }
                                                    nilai_rapor = Math.Round(nilai_rapor, 0, MidpointRounding.AwayFromZero);
                                                    nilai_rapor_keterampilan = nilai_rapor;
                                                    lst_all_nilai_rapor.Add(new NilaiWithKey
                                                    {
                                                        Key = (id_kolom_ledger).ToString(),
                                                        Nilai = nilai_rapor_keterampilan,
                                                        JenisMapel = jenis_mapel
                                                    });
                                                    id_kolom_ledger_keterampilan = id_kolom_ledger;
                                                    id_kolom_ledger++;
                                                    //end get nilai rapor

                                                    //simpan nilai biologi or fisika pengetahuan
                                                    if (
                                                        m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_BIOLOGI ||
                                                        m_struktur_nilai.Rel_Mapel.ToString().ToUpper().Trim() == Constantas_Kode_Mapel.KODE_FISIKA
                                                    )
                                                    {
                                                        lst_nilai_ipa_praktik.Add(
                                                                Math.Round(nilai_rapor_keterampilan, Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero)
                                                            );
                                                    }
                                                    //end simpan nilai biologi or fisika pengetahuan

                                                    jumlah_mapel_rapor++;
                                                }
                                            }
                                            //end get struktur nilai

                                            if (m_struktur_nilai.Is_PH_PTS_PAS)
                                            {
                                                if (DAO_Mapel.GetJenisMapel(m_struktur_nilai.Rel_Mapel.ToString()) == Application_Libs.Libs.JENIS_MAPEL.PILIHAN && nilai_rapor_pengetahuan == 0)
                                                {
                                                    html_item = "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                    "&nbsp;" +
                                                                "</td>" +
                                                                "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                    "&nbsp;" +
                                                                "</td>" +
                                                                "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                    "&nbsp;" +
                                                                "</td>" +
                                                                "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                    "&nbsp;" +
                                                                "</td>" +
                                                                "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                    "&nbsp;" +
                                                                "</td>" +
                                                                "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                    "&nbsp;" +
                                                                "</td>" +
                                                                "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                    "&nbsp;" +
                                                                "</td>" +
                                                                "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                    "&nbsp;" +
                                                                "</td>";
                                                    html_tr_row_rata_rata += (id_siswa == 1 ? html_item : "");
                                                    html_row_body_siswa += html_item;
                                                }
                                                else
                                                {
                                                    if (nilai_rapor_keterampilan > 0 && nilai_rapor_pengetahuan == 0)
                                                    {
                                                        html_item = "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                        "&nbsp;" +
                                                                    "</td>" +
                                                                    "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                        "&nbsp;" +
                                                                    "</td>" +
                                                                    "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                        "&nbsp;" +
                                                                    "</td>" +
                                                                    "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                        "&nbsp;" +
                                                                    "</td>" +
                                                                    "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                        "&nbsp;" +
                                                                    "</td>" +
                                                                    "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                        "&nbsp;" +
                                                                    "</td>" +
                                                                    "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                        "&nbsp;" +
                                                                    "</td>" +
                                                                    "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                        "&nbsp;" +
                                                                    "</td>";
                                                        html_tr_row_rata_rata += (id_siswa == 1 ? html_item : "");
                                                        html_row_body_siswa += html_item;
                                                    }
                                                    else
                                                    {
                                                        html_item = "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                        s_nilai_ph +
                                                                    "</td>" +
                                                                    "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                        s_nilai_ph_bobot +
                                                                    "</td>" +
                                                                    "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                        s_nilai_pts +
                                                                    "</td>" +
                                                                    "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                        s_nilai_pts_bobot +
                                                                    "</td>" +
                                                                    "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                        s_nilai_pas +
                                                                    "</td>" +
                                                                    "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                        s_nilai_pas_bobot +
                                                                    "</td>" +
                                                                    "<td style=\"" + css_cell_body + " text-align: center; " + (nilai_rapor_pengetahuan < m_struktur_nilai.KKM ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                        nilai_rapor_pengetahuan.ToString() +
                                                                    "</td>" +
                                                                    "<td style=\"" + css_cell_body + " text-align: center; " + (nilai_rapor_pengetahuan < m_struktur_nilai.KKM ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                        GetPredikatRapor(nilai_rapor_pengetahuan, tahun_ajaran) +
                                                                    "</td>";
                                                        i_jumlah_pengetahuan++;
                                                        if (id_siswa == 1)
                                                        {
                                                            html_tr_row_rata_rata +=
                                                                        "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                            "&nbsp;" +
                                                                        "</td>" +
                                                                        "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                            "&nbsp;" +
                                                                        "</td>" +
                                                                        "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                            "&nbsp;" +
                                                                        "</td>" +
                                                                        "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                            "&nbsp;" +
                                                                        "</td>" +
                                                                        "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                            "&nbsp;" +
                                                                        "</td>" +
                                                                        "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                            "&nbsp;" +
                                                                        "</td>" +
                                                                        "<td style=\"" + css_cell_body + " text-align: center; " + (nilai_rapor_pengetahuan < m_struktur_nilai.KKM ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                            "@" + id_kolom_ledger_pengetahuan.ToString() + "@" +
                                                                        "</td>" +
                                                                        "<td style=\"" + css_cell_body + " text-align: center; " + (nilai_rapor_pengetahuan < m_struktur_nilai.KKM ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                            "&nbsp;" +
                                                                        "</td>";
                                                        }
                                                        html_row_body_siswa += html_item;
                                                    }
                                                }
                                                if (DAO_Mapel.GetJenisMapel(m_struktur_nilai.Rel_Mapel.ToString()) == Application_Libs.Libs.JENIS_MAPEL.PILIHAN && nilai_rapor_keterampilan == 0)
                                                {
                                                    if (id_siswa == 1)
                                                    {
                                                        html_tr_row_rata_rata += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "@" + id_kolom_ledger_keterampilan.ToString() + "@" +
                                                                           "</td>" +
                                                                           "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "&nbsp;" +
                                                                           "</td>";
                                                    }
                                                    html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "&nbsp;" +
                                                                           "</td>" +
                                                                           "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "&nbsp;" +
                                                                           "</td>";
                                                }
                                                else
                                                {
                                                    if (nilai_rapor_pengetahuan > 0 && nilai_rapor_keterampilan == 0)
                                                    {
                                                        if (id_siswa == 1)
                                                        {
                                                            html_tr_row_rata_rata += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>" +
                                                                               "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>";
                                                        }
                                                        html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>" +
                                                                               "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>";
                                                    }
                                                    else
                                                    {
                                                        if (id_siswa == 1)
                                                        {
                                                            html_tr_row_rata_rata += "<td style=\"" + css_cell_body + " text-align: center; " + (nilai_rapor_keterampilan < m_struktur_nilai.KKM ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                                    "@" + id_kolom_ledger_keterampilan.ToString() + "@" +
                                                                               "</td>" +
                                                                               "<td style=\"" + css_cell_body + " text-align: center; " + (nilai_rapor_keterampilan < m_struktur_nilai.KKM ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>";
                                                        }
                                                        html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center; " + (nilai_rapor_keterampilan < m_struktur_nilai.KKM ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                                    nilai_rapor_keterampilan.ToString() +
                                                                               "</td>" +
                                                                               "<td style=\"" + css_cell_body + " text-align: center; " + (nilai_rapor_keterampilan < m_struktur_nilai.KKM ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                                    GetPredikatRapor(nilai_rapor_keterampilan, tahun_ajaran) + 
                                                                               "</td>";
                                                        i_jumlah_keterampilan++;
                                                    }
                                                }
                                            }
                                            else if (!m_struktur_nilai.Is_PH_PTS_PAS)
                                            {
                                                if (DAO_Mapel.GetJenisMapel(m_struktur_nilai.Rel_Mapel.ToString()) == Application_Libs.Libs.JENIS_MAPEL.PILIHAN && nilai_rapor_pengetahuan == 0)
                                                {
                                                    if (id_siswa == 1)
                                                    {
                                                        html_tr_row_rata_rata += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "@" + id_kolom_ledger_pengetahuan + "@" +
                                                                           "</td>" +
                                                                           "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "&nbsp;" +
                                                                           "</td>";
                                                    }
                                                    html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "&nbsp;" +
                                                                           "</td>" +
                                                                           "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "&nbsp;" +
                                                                           "</td>";
                                                }
                                                else
                                                {
                                                    if (nilai_rapor_keterampilan > 0 && nilai_rapor_pengetahuan == 0)
                                                    {
                                                        if (id_siswa == 1)
                                                        {
                                                            html_tr_row_rata_rata += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>" +
                                                                               "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>";
                                                        }
                                                        html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>" +
                                                                               "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>";
                                                    }
                                                    else
                                                    {
                                                        if (id_siswa == 1)
                                                        {
                                                            html_tr_row_rata_rata += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "@" + id_kolom_ledger_pengetahuan + "@" +
                                                                               "</td>" +
                                                                               "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>";
                                                        }
                                                        html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center; " + (nilai_rapor_pengetahuan < m_struktur_nilai.KKM ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                                    nilai_rapor_pengetahuan.ToString() +
                                                                               "</td>" +
                                                                               "<td style=\"" + css_cell_body + " text-align: center; " + (nilai_rapor_pengetahuan < m_struktur_nilai.KKM ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                                    GetPredikatRapor(nilai_rapor_pengetahuan, tahun_ajaran) +
                                                                               "</td>";
                                                        i_jumlah_pengetahuan++;
                                                    }
                                                }
                                                if (DAO_Mapel.GetJenisMapel(m_struktur_nilai.Rel_Mapel.ToString()) == Application_Libs.Libs.JENIS_MAPEL.PILIHAN && nilai_rapor_keterampilan == 0)
                                                {
                                                    if (id_siswa == 1)
                                                    {
                                                        html_tr_row_rata_rata += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "@" + id_kolom_ledger_keterampilan.ToString() + "@" +
                                                                           "</td>" +
                                                                           "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "&nbsp;" +
                                                                           "</td>";
                                                    }
                                                    html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "&nbsp;" +
                                                                           "</td>" +
                                                                           "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "&nbsp;" +
                                                                           "</td>";
                                                }
                                                else
                                                {
                                                    if (nilai_rapor_pengetahuan > 0 && nilai_rapor_keterampilan == 0)
                                                    {
                                                        if (id_siswa == 1)
                                                        {
                                                            html_tr_row_rata_rata += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>" +
                                                                               "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>";
                                                        }
                                                        html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>" +
                                                                               "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>";
                                                    }
                                                    else
                                                    {
                                                        if (id_siswa == 1)
                                                        {
                                                            html_tr_row_rata_rata += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "@" + id_kolom_ledger_keterampilan + "@" +
                                                                               "</td>" +
                                                                               "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>";
                                                        }
                                                        html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center; " + (nilai_rapor_keterampilan < m_struktur_nilai.KKM ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                                    nilai_rapor_keterampilan.ToString() +
                                                                               "</td>" +
                                                                               "<td style=\"" + css_cell_body + " text-align: center; " + (nilai_rapor_keterampilan < m_struktur_nilai.KKM ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                                    GetPredikatRapor(nilai_rapor_keterampilan, tahun_ajaran) +
                                                                               "</td>";
                                                        i_jumlah_keterampilan++;
                                                    }
                                                }
                                            }

                                            //jika fisika tambahkan satu kolom
                                            if (item.Rel_Mapel.Trim().ToUpper() == Constantas_Kode_Mapel.KODE_FISIKA.Trim().ToUpper())
                                            {
                                                lst_kkm_nilai_rapor.Add(new NilaiWithKey
                                                {
                                                    Key = (jumlah_mapel_rapor + 1).ToString(),
                                                    Nilai = 70 //-99
                                                });

                                                nilai_rapor_pengetahuan = Math.Round(lst_nilai_ipa_pengetahuan.DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero);
                                                nilai_rapor_keterampilan = Math.Round(lst_nilai_ipa_praktik.DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP, MidpointRounding.AwayFromZero);

                                                id_kolom_ledger++;
                                                lst_all_nilai_rapor.Add(new NilaiWithKey
                                                {
                                                    Key = (id_kolom_ledger).ToString(),
                                                    Nilai = nilai_rapor_pengetahuan,
                                                    JenisMapel = jenis_mapel
                                                });
                                                d_jumlah_pengetahuan += nilai_rapor_pengetahuan;
                                                id_kolom_ledger_pengetahuan = id_kolom_ledger;
                                                id_kolom_ledger += 2;

                                                if (DAO_Mapel.GetJenisMapel(m_struktur_nilai.Rel_Mapel.ToString()) == Application_Libs.Libs.JENIS_MAPEL.PILIHAN && nilai_rapor_pengetahuan == 0)
                                                {
                                                    if (id_siswa == 1)
                                                    {
                                                        html_tr_row_rata_rata += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "&nbsp;" +
                                                                           "</td>" +
                                                                           "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "&nbsp;" +
                                                                           "</td>";
                                                    }
                                                    html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "&nbsp;" +
                                                                           "</td>" +
                                                                           "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "&nbsp;" +
                                                                           "</td>";
                                                }
                                                else
                                                {
                                                    if (nilai_rapor_keterampilan > 0 && nilai_rapor_pengetahuan == 0)
                                                    {
                                                        if (id_siswa == 1)
                                                        {
                                                            html_tr_row_rata_rata += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>" +
                                                                               "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>";
                                                        }
                                                        html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>" +
                                                                               "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>";
                                                    }
                                                    else
                                                    {
                                                        if (id_siswa == 1)
                                                        {
                                                            html_tr_row_rata_rata += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "@" + id_kolom_ledger_pengetahuan + "@" +
                                                                               "</td>" +
                                                                               "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>";
                                                        }
                                                        html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center; " + (nilai_rapor_pengetahuan < m_struktur_nilai.KKM ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                                    nilai_rapor_pengetahuan.ToString() +
                                                                               "</td>" +
                                                                               "<td style=\"" + css_cell_body + " text-align: center; " + (nilai_rapor_pengetahuan < m_struktur_nilai.KKM ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                                    GetPredikatRapor(nilai_rapor_pengetahuan, tahun_ajaran) +
                                                                               "</td>";
                                                        i_jumlah_pengetahuan++;
                                                    }
                                                }

                                                lst_all_nilai_rapor.Add(new NilaiWithKey
                                                {
                                                    Key = (id_kolom_ledger).ToString(),
                                                    Nilai = nilai_rapor_keterampilan,
                                                    JenisMapel = jenis_mapel
                                                });
                                                d_jumlah_keterampilan += nilai_rapor_keterampilan;
                                                id_kolom_ledger_keterampilan = id_kolom_ledger;
                                                id_kolom_ledger++;

                                                if (DAO_Mapel.GetJenisMapel(m_struktur_nilai.Rel_Mapel.ToString()) == Application_Libs.Libs.JENIS_MAPEL.PILIHAN && nilai_rapor_keterampilan == 0)
                                                {
                                                    if (id_siswa == 1)
                                                    {
                                                        html_tr_row_rata_rata += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "&nbsp;" +
                                                                           "</td>" +
                                                                           "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "&nbsp;" +
                                                                           "</td>";
                                                    }
                                                    html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "&nbsp;" +
                                                                           "</td>" +
                                                                           "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                "&nbsp;" +
                                                                           "</td>";
                                                }
                                                else
                                                {
                                                    if (nilai_rapor_pengetahuan > 0 && nilai_rapor_keterampilan == 0)
                                                    {
                                                        if (id_siswa == 1)
                                                        {
                                                            html_tr_row_rata_rata += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>" +
                                                                               "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>";
                                                        }
                                                        html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>" +
                                                                               "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>";
                                                    }
                                                    else
                                                    {
                                                        if (id_siswa == 1)
                                                        {
                                                            html_tr_row_rata_rata += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "@" + id_kolom_ledger_keterampilan + "@" +
                                                                               "</td>" +
                                                                               "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                                    "&nbsp;" +
                                                                               "</td>";
                                                        }
                                                        html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center; " + (nilai_rapor_keterampilan < m_struktur_nilai.KKM ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                                    nilai_rapor_keterampilan.ToString() +
                                                                               "</td>" +
                                                                               "<td style=\"" + css_cell_body + " text-align: center; " + (nilai_rapor_keterampilan < m_struktur_nilai.KKM ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                                    GetPredikatRapor(nilai_rapor_keterampilan, tahun_ajaran) +
                                                                               "</td>";
                                                        i_jumlah_keterampilan++;
                                                    }
                                                }

                                                jumlah_mapel_rapor++;
                                            }
                                            //end jika fisika                                               
                                        }

                                    }
                                    //end rapor non mulok

                                    lst_jumlah_nilai_keseluruhan.Add(new NilaiWithKey
                                    {
                                        Key = m_siswa.Kode.ToString(),
                                        Nilai = jumlah_keseluruhan
                                    });

                                    //absen
                                    string s_sakit = "-";
                                    string s_izin = "-";
                                    string s_alpa = "-";
                                    string s_terlambat = "-";

                                    Rapor_Arsip m_rapor_arsip = DAO_Rapor_Arsip.GetAll_Entity().FindAll(
                                            m0 => m0.TahunAjaran == tahun_ajaran &&
                                                  m0.Semester == semester &&
                                                  m0.JenisRapor == "Semester"

                                        ).FirstOrDefault();
                                    List<DAO_SiswaAbsen.AbsenRekapRapor> lst_absen = new List<DAO_SiswaAbsen.AbsenRekapRapor>();
                                    lst_absen = DAO_SiswaAbsen.GetRekapAbsenRaporBySiswaByPeriode_Entity(
                                            m_siswa.Kode.ToString(),
                                            (m_rapor_arsip != null ? m_rapor_arsip.TanggalAwalAbsen : DateTime.MinValue),
                                            (m_rapor_arsip != null ? m_rapor_arsip.TanggalAkhirAbsen : DateTime.MinValue)
                                        );
                                    foreach (var absen in lst_absen)
                                    {
                                        if (absen.Absen == Libs.JENIS_ABSENSI.SAKIT.Substring(0, 1)) s_sakit = absen.Jumlah.ToString();
                                        if (absen.Absen == Libs.JENIS_ABSENSI.IZIN.Substring(0, 1)) s_izin = absen.Jumlah.ToString();
                                        if (absen.Absen == Libs.JENIS_ABSENSI.ALPA.Substring(0, 1)) s_alpa = absen.Jumlah.ToString();
                                        if (absen.Absen == Libs.JENIS_ABSENSI.TERLAMBAT.Substring(0, 1)) s_terlambat = absen.Jumlah.ToString();
                                    }

                                    if (tahun_ajaran == "2020/2021")
                                    {
                                        if (lst_rekap_det.FindAll(m0 => m0.Rel_Siswa.ToString().Replace(";", "").ToUpper() == m_siswa.Kode.ToString().Replace(";", "").ToUpper().Trim()).Count > 0)
                                        {
                                            SiswaAbsenRekapDet m_rekap_absen_siswa = lst_rekap_det.FindAll(m0 => m0.Rel_Siswa.ToString().Replace(";", "").ToUpper() == m_siswa.Kode.ToString().Replace(";", "").ToUpper().Trim()).FirstOrDefault();
                                            if (m_rekap_absen_siswa != null)
                                            {
                                                if (m_rekap_absen_siswa.Rel_Siswa != null)
                                                {
                                                    s_sakit = m_rekap_absen_siswa.Sakit;
                                                    s_izin = m_rekap_absen_siswa.Izin;
                                                    s_alpa = m_rekap_absen_siswa.Alpa;
                                                    s_terlambat = m_rekap_absen_siswa.Terlambat;
                                                }
                                            }
                                        }
                                    }

                                    html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center; \">" +
                                                                s_sakit +
                                                           "</td>" +
                                                           "<td style=\"" + css_cell_body + " text-align: center; \">" +
                                                                s_izin +
                                                           "</td>" +
                                                           "<td style=\"" + css_cell_body + " text-align: center; \">" +
                                                                s_alpa +
                                                           "</td>" +
                                                           "<td style=\"" + css_cell_body + " text-align: center; \">" +
                                                                s_terlambat +
                                                           "</td>";
                                    //end absen

                                    //nilai rata-rata & total
                                    decimal d_nilai_pengetahuan = Math.Round(Convert.ToDecimal(d_jumlah_pengetahuan) / Convert.ToDecimal(i_jumlah_pengetahuan), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP_2DES);
                                    decimal d_nilai_keterampilan = Math.Round(Convert.ToDecimal(d_jumlah_keterampilan) / Convert.ToDecimal(i_jumlah_keterampilan), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP_2DES);
                                    html_row_body_siswa += "<td style=\"" + css_cell_body + " text-align: center; " + (d_nilai_pengetahuan < KKM_GLOBAL ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                d_nilai_pengetahuan +
                                                           "</td>" +
                                                           "<td style=\"" + css_cell_body + " text-align: center; " + (d_nilai_keterampilan < KKM_GLOBAL ? " font-weight: bold; color: #9C0006; background-color: #FFC7CE; " : "") + "\">" +
                                                                d_nilai_keterampilan +
                                                           "</td>" +
                                                           "<td style=\"" + css_cell_body + " text-align: center; \">" +
                                                                d_jumlah_pengetahuan +
                                                           "</td>" +
                                                           "<td style=\"" + css_cell_body + " text-align: center; \">" +
                                                                d_jumlah_keterampilan +
                                                           "</td>";

                                    html_row_body += "<tr>" +
                                                        html_row_body_siswa +
                                                     "</tr>";

                                    nomor++;
                                }
                                // end foreach siswa

                                if (html_tr_row_rata_rata.Trim() != "")
                                {
                                    html_row_header += "<td colspan=\"" + jumlah_kolom_ledger.ToString() + "\" style=\"" + css_cell_headers + " font-size: 8.5px; \">" +
                                                            "NILAI MATA PELAJARAN" +
                                                       "</td>";
                                    html_row_header += "<td colspan=\"4\" style=\"" + css_cell_headers + " font-size: 8.5px; \">" +
                                                            "PRESENSI" +
                                                       "</td>";
                                    html_row_header += "<td colspan=\"2\" style=\"" + css_cell_headers + " font-size: 8.5px; \">" +
                                                            "RATA-RATA" +
                                                       "</td>";
                                    html_row_header += "<td colspan=\"2\" style=\"" + css_cell_headers + " font-size: 8.5px; \">" +
                                                            "JUMLAH" +
                                                       "</td>";

                                    for (int i = 1; i <= id_kolom_ledger; i++)
                                    {
                                        decimal rata_rata = Math.Round(lst_all_nilai_rapor.FindAll(m => m.Key == (i + jumlah_awal_col_ledger).ToString()).Where(m => ((m.JenisMapel == Libs.JENIS_MAPEL.PILIHAN && m.Nilai > 0) || m.JenisMapel != Libs.JENIS_MAPEL.PILIHAN)).Select(m => m.Nilai).DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP);
                                        html_tr_row_rata_rata = html_tr_row_rata_rata.Replace(
                                                                    "@" + (i + jumlah_awal_col_ledger).ToString() + "@",
                                                                    (
                                                                        rata_rata > 0
                                                                        ? Math.Round(rata_rata, 0).ToString()
                                                                        : ""
                                                                    )
                                                                );
                                    }
                                    html_row_rata_rata = "<td colspan=\"4\" style=\"" + css_cell_body + " font-weight: bold;\">" +
                                                            "RATA-RATA KELAS" +
                                                         "</td>" +
                                                         html_tr_row_rata_rata;

                                    for (int i = 1; i <= jumlah_mapel_rapor; i++)
                                    {
                                        decimal kkm = 0;
                                        NilaiWithKey m_kkm = lst_kkm_nilai_rapor.FindAll(m => m.Key == i.ToString()).FirstOrDefault();
                                        if (m_kkm != null)
                                        {
                                            if (m_kkm.Key != null)
                                            {
                                                kkm = m_kkm.Nilai;
                                            }
                                        }

                                        html_tr_row_kkm = html_tr_row_kkm.Replace(
                                                                "@" + (i).ToString() + "@",
                                                                (
                                                                    kkm == -99
                                                                    ? ""
                                                                    : Math.Round(kkm, 0).ToString()
                                                                )
                                                            );
                                    }

                                    html_row_kkm = "<td colspan=\"4\" style=\"" + css_cell_body + " font-weight: bold;\">" +
                                                        "KKM" +
                                                   "</td>" +
                                                   html_tr_row_kkm;
                                }
                                else
                                {
                                    html_row_header += "<td colspan=\"" + jml_colspan_mapel.ToString() + "\" style=\"" + css_cell_headers + " font-size: 8.5px; \">" +
                                                            "NILAI MATA PELAJARAN" +
                                                       "</td>";
                                    html_row_header += "<td colspan=\"4\" style=\"" + css_cell_headers + " font-size: 8.5px; \">" +
                                                            "PRESENSI" +
                                                       "</td>";
                                    html_row_header += "<td colspan=\"2\" style=\"" + css_cell_headers + " font-size: 8.5px; \">" +
                                                            "RATA-RATA" +
                                                       "</td>";
                                    html_row_header += "<td colspan=\"2\" style=\"" + css_cell_headers + " font-size: 8.5px; \">" +
                                                            "JUMLAH" +
                                                       "</td>";

                                    for (int i = 1;
                                     i <= (DAO_Rapor_Desain_Det.GetAllByHeader_Entity(rapor_desain.Kode.ToString()).FindAll(m => m.Nomor.Trim() != "" && m.Rel_Mapel.Trim() != "").Count * 4) + (4);
                                     i++)
                                    {
                                        decimal rata_rata = Math.Round(lst_all_nilai_rapor.FindAll(m => m.Key == (i + jumlah_awal_col_ledger).ToString()).Select(m => m.Nilai).DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP);
                                        html_row_rata_rata += "<td style=\"" + css_cell_body + " text-align: center;\">" +
                                                                    (
                                                                        rata_rata > 0
                                                                        ? Math.Round(rata_rata, 0).ToString()
                                                                        : ""
                                                                    ) +
                                                              "</td>";
                                    }

                                    for (int i = 1; i <= (DAO_Rapor_Desain_Det.GetAllByHeader_Entity(rapor_desain.Kode.ToString()).FindAll(m => m.Nomor.Trim() != "" && m.Rel_Mapel.Trim() != "").Count + 1); i++)
                                    {
                                        decimal kkm = 0;
                                        NilaiWithKey m_kkm = lst_kkm_nilai_rapor.FindAll(m => m.Key == i.ToString()).FirstOrDefault();
                                        if (m_kkm != null)
                                        {
                                            if (m_kkm.Key != null)
                                            {
                                                kkm = m_kkm.Nilai;
                                            }
                                        }
                                        html_row_kkm += "<td colspan=\"4\" style=\"" + css_cell_body + " text-align: center;\">" +
                                                            (
                                                                kkm == -99
                                                                ? ""
                                                                : Math.Round(kkm, 0).ToString()
                                                            ) +
                                                        "</td>";
                                    }
                                }

                                decimal rata_rata_nilai_keseluruhan = Math.Round(lst_jumlah_nilai_keseluruhan.Select(m => m.Nilai).DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP);
                                decimal rata_rata_nilai_rata_rata_rapor = Math.Round(lst_nilai_rapor.Select(m => m.Nilai).DefaultIfEmpty().Average(), Constantas.PEMBULATAN_DESIMAL_NILAI_SMP);
                                html_row_body += "<tr>" +
                                                    html_row_rata_rata +
                                                 "</tr>" +
                                                 "<tr>" +
                                                    html_row_kkm +
                                                 "</tr>";

                            }

                        }
                    }

                }
            }

            html_row_header = "<tr>" +
                                html_row_header +
                              "</tr>" +
                              "<tr>" +
                                html_row_mapel +
                              "</tr>" +
                              "<tr>" +
                                html_row_jenis_nilai +
                              "</tr>" +
                              html_row_body;

            html = "<table style=\"border-collapse: collapse;\">" +
                        html_row_header +
                   "</table>" +
                   "<br />" +
                   "<br />";

            return html;
        }
    }
}