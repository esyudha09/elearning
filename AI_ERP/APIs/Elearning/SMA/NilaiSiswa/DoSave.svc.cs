using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities.Elearning;
using AI_ERP.Application_DAOs.Elearning;
using AI_ERP.Application_Entities.Elearning.SMA;
using AI_ERP.Application_DAOs.Elearning.SMA;

namespace AI_ERP.APIs.Elearning.SMA.NilaiSiswa
{
    public class DoSave : IDoSave
    {
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public string[] Do(
                string j,
                string t,
                string sm,
                string k,
                string kdt,
                string s,
                string n,
                string ap,
                string kd,
                string kp,
                string mp,
                string nr_p,
                string nr_ppk,
                string nr_prd,
                string nr_k,
                string pnr_p,
                string pnr_k,
                string lts_hd,
                string lts_maxhd,
                string lts_lk,
                string lts_rj,
                string lts_rpkb,
                string lts_ck_hd,
                string lts_ck_kw,
                string lts_ck_ps,
                string lts_ck_pk,
                string ssid
            )
        {
            List<string> hasil = new List<string>();

            string jenis = j;
            string tahun_ajaran = RandomLibs.GetParseTahunAjaran(t);
            string semester = sm;
            string rel_kelas = k;
            string rel_kelasdet = kdt;
            string rel_siswa = s;
            string nilai = n;
            string rel_sn_ap = ap;
            string rel_sn_kd = kd;
            string rel_sn_kp = kp;
            string rel_mapel = mp;
            string predikat_pengetahuan = pnr_p;
            string predikat_keterampilan = pnr_k;
            string lts_hadir = lts_hd;
            string lts_maxhadir = lts_maxhd;
            string lts_kelakuan = lts_lk;
            string lts_kerajinan = lts_rj;
            string lts_kerapihan = lts_rpkb;
            string s_ssid = Libs.Dekrip(ssid);

            if (
                jenis == Libs.Enkrip(Libs.JenisKurikulum.SMA.KTSP) &&
                tahun_ajaran.Trim() != "" && semester.Trim() != "" &&
                rel_siswa.Trim() != ""
            )
            {
                List<Rapor_Nilai> lst_nilai = DAO_Rapor_Nilai.GetAllByTABySMByKelasDetByMapel_Entity(
                        tahun_ajaran, semester, rel_kelasdet, rel_mapel
                    );
                //System.Threading.Thread.Sleep(500);

                //add di master nilai
                Guid kode = Guid.NewGuid();
                if (lst_nilai.Count == 0)
                {
                    DAO_Rapor_Nilai.Insert(new Rapor_Nilai
                    {
                        Kode = kode,
                        TahunAjaran = tahun_ajaran,
                        Semester = semester,
                        Rel_Kelas = rel_kelas,
                        Rel_KelasDet = rel_kelasdet,
                        Rel_Mapel = rel_mapel,
                        Kurikulum = ""
                    }, s_ssid);
                    //System.Threading.Thread.Sleep(500);
                }
                else
                {
                    kode = lst_nilai.FirstOrDefault().Kode;
                }

                //add di nilai ktsp
                Guid kode_nilai_siswa = Guid.NewGuid();
                List<Rapor_NilaiSiswa_KTSP> lst_nilai_by_siswa = DAO_Rapor_NilaiSiswa_KTSP.GetAllByHeaderBySiswa_Entity(
                        kode.ToString(), rel_siswa
                    );
                //System.Threading.Thread.Sleep(500);

                if (lst_nilai_by_siswa.Count == 0)
                {
                    DAO_Rapor_NilaiSiswa_KTSP.Insert(new Rapor_NilaiSiswa_KTSP
                    {
                        Kode = kode_nilai_siswa,
                        Rel_Rapor_Nilai = kode,
                        Rel_Siswa = rel_siswa,
                        Rapor_PPK = nr_ppk,
                        Rapor_P = nr_p,
                        Rapor_Sikap = nr_prd,
                        LTS_HD = lts_hadir,
                        LTS_MAX_HD = lts_maxhadir,
                        LTS_LK = lts_kelakuan,
                        LTS_RJ = lts_kerajinan,
                        LTS_RPKB = lts_kerapihan,
                        LTS_CK_KEHADIRAN = lts_ck_hd,
                        LTS_CK_KETEPATAN_WKT = lts_ck_kw,
                        LTS_CK_PENGGUNAAN_SRGM = lts_ck_ps,
                        LTS_CK_PENGGUNAAN_KMR = lts_ck_pk
                    }, s_ssid);

                    //System.Threading.Thread.Sleep(500);
                }
                else
                {
                    kode_nilai_siswa = lst_nilai_by_siswa.FirstOrDefault().Kode;

                    DAO_Rapor_NilaiSiswa_KTSP.Update(new Rapor_NilaiSiswa_KTSP
                    {
                        Kode = kode_nilai_siswa,
                        Rel_Rapor_Nilai = kode,
                        Rel_Siswa = rel_siswa,
                        Rapor_PPK = nr_ppk,
                        Rapor_P = nr_p,
                        Rapor_Sikap = nr_prd,
                        LTS_HD = lts_hadir,
                        LTS_MAX_HD = lts_maxhadir,
                        LTS_LK = lts_kelakuan,
                        LTS_RJ = lts_kerajinan,
                        LTS_RPKB = lts_kerapihan,
                        LTS_CK_KEHADIRAN = lts_ck_hd,
                        LTS_CK_KETEPATAN_WKT = lts_ck_kw,
                        LTS_CK_PENGGUNAAN_SRGM = lts_ck_ps,
                        LTS_CK_PENGGUNAAN_KMR = lts_ck_pk
                    }, s_ssid);

                    //System.Threading.Thread.Sleep(500);
                }

                //nilai detail siswa ktsp
                if (rel_sn_kd.Trim().Length == 36 &&
                    rel_sn_kp.Trim().Length == 36)
                {
                    Guid kode_nilai_siswa_det = Guid.NewGuid();
                    List<Rapor_NilaiSiswa_KTSP_Det> lst_nilai_siswa_det = DAO_Rapor_NilaiSiswa_KTSP_Det.GetAllByTABySMByKelasDetByMapelByKDByKPBySiswa_Entity(
                            tahun_ajaran, semester, rel_kelasdet, rel_mapel, rel_sn_kd, rel_sn_kp, rel_siswa
                        );
                    //System.Threading.Thread.Sleep(500);

                    if (lst_nilai_siswa_det.Count == 0)
                    {
                        DAO_Rapor_NilaiSiswa_KTSP_Det.Insert(new Rapor_NilaiSiswa_KTSP_Det
                        {
                            Kode = kode_nilai_siswa_det,
                            Rel_Rapor_NilaiSiswa_KTSP = kode_nilai_siswa,
                            Rel_Rapor_StrukturNilai_KTSP_KD = rel_sn_kd,
                            Rel_Rapor_StrukturNilai_KTSP_KP = rel_sn_kp,
                            Rel_Siswa = rel_siswa,
                            Nilai = nilai
                        }, s_ssid);

                        //System.Threading.Thread.Sleep(500);
                    }
                    else
                    {
                        kode_nilai_siswa_det = lst_nilai_siswa_det.FirstOrDefault().Kode;

                        DAO_Rapor_NilaiSiswa_KTSP_Det.Update(new Rapor_NilaiSiswa_KTSP_Det
                        {
                            Kode = kode_nilai_siswa_det,
                            Rel_Rapor_NilaiSiswa_KTSP = kode_nilai_siswa,
                            Rel_Rapor_StrukturNilai_KTSP_KD = rel_sn_kd,
                            Rel_Rapor_StrukturNilai_KTSP_KP = rel_sn_kp,
                            Rel_Siswa = rel_siswa,
                            Nilai = nilai
                        }, s_ssid);

                        //System.Threading.Thread.Sleep(500);
                    }
                }
            }
            else if (
                jenis == Libs.Enkrip(Libs.JenisKurikulum.SMA.KURTILAS) &&
                tahun_ajaran.Trim() != "" && semester.Trim() != "" && rel_kelasdet.Trim() != "" &&
                rel_siswa.Trim() != ""
            )
            {
                List<Rapor_Nilai> lst_nilai = DAO_Rapor_Nilai.GetAllByTABySMByKelasDetByMapel_Entity(
                        tahun_ajaran, semester, rel_kelasdet, rel_mapel
                    );

                //System.Threading.Thread.Sleep(500);

                //add di master nilai
                Guid kode = Guid.NewGuid();
                if (lst_nilai.Count == 0)
                {
                    DAO_Rapor_Nilai.Insert(new Rapor_Nilai
                    {
                        Kode = kode,
                        TahunAjaran = tahun_ajaran,
                        Semester = semester,
                        Rel_Kelas = rel_kelas,
                        Rel_KelasDet = rel_kelasdet,
                        Rel_Mapel = rel_mapel,
                        Kurikulum = ""
                    }, s_ssid);

                    //System.Threading.Thread.Sleep(500);
                }
                else
                {
                    kode = lst_nilai.FirstOrDefault().Kode;
                }

                //add di nilai kurtilas
                Guid kode_nilai_siswa = Guid.NewGuid();
                List<Rapor_NilaiSiswa_KURTILAS> lst_nilai_by_siswa = DAO_Rapor_NilaiSiswa_KURTILAS.GetAllByHeaderBySiswa_Entity(
                        kode.ToString(), rel_siswa
                    );

                //System.Threading.Thread.Sleep(500);

                if (lst_nilai_by_siswa.Count == 0)
                {
                    DAO_Rapor_NilaiSiswa_KURTILAS.Insert(new Rapor_NilaiSiswa_KURTILAS
                    {
                        Kode = kode_nilai_siswa,
                        Rel_Rapor_Nilai = kode,
                        Rel_Siswa = rel_siswa,
                        Rapor_Pengetahuan = nr_p,
                        Rapor_Keterampilan = nr_k,
                        Predikat_Pengetahuan = predikat_pengetahuan,
                        Predikat_Keterampilan = predikat_keterampilan,
                        LTS_HD = lts_hadir,
                        LTS_MAX_HD = lts_maxhadir,
                        LTS_LK = lts_kelakuan,
                        LTS_RJ = lts_kerajinan,
                        LTS_RPKB = lts_kerapihan,
                        LTS_CK_KEHADIRAN = lts_ck_hd,
                        LTS_CK_KETEPATAN_WKT = lts_ck_kw,
                        LTS_CK_PENGGUNAAN_SRGM = lts_ck_ps,
                        LTS_CK_PENGGUNAAN_KMR = lts_ck_pk
                    }, s_ssid);

                    //System.Threading.Thread.Sleep(500);
                }
                else
                {
                    kode_nilai_siswa = lst_nilai_by_siswa.FirstOrDefault().Kode;

                    DAO_Rapor_NilaiSiswa_KURTILAS.Update(new Rapor_NilaiSiswa_KURTILAS
                    {
                        Kode = kode_nilai_siswa,
                        Rel_Rapor_Nilai = kode,
                        Rel_Siswa = rel_siswa,
                        Rapor_Pengetahuan = nr_p,
                        Rapor_Keterampilan = nr_k,
                        Predikat_Pengetahuan = predikat_pengetahuan,
                        Predikat_Keterampilan = predikat_keterampilan,
                        LTS_HD = lts_hadir,
                        LTS_MAX_HD = lts_maxhadir,
                        LTS_LK = lts_kelakuan,
                        LTS_RJ = lts_kerajinan,
                        LTS_RPKB = lts_kerapihan,
                        LTS_CK_KEHADIRAN = lts_ck_hd,
                        LTS_CK_KETEPATAN_WKT = lts_ck_kw,
                        LTS_CK_PENGGUNAAN_SRGM = lts_ck_ps,
                        LTS_CK_PENGGUNAAN_KMR = lts_ck_pk
                    }, s_ssid);

                    //System.Threading.Thread.Sleep(500);
                }

                //nilai detail siswa kurtilas
                if (rel_sn_kd.Trim().Length == 36 &&
                    rel_sn_kp.Trim().Length == 36)
                {
                    Guid kode_nilai_siswa_det = Guid.NewGuid();
                    List<Rapor_NilaiSiswa_KURTILAS_Det> lst_nilai_siswa_det = DAO_Rapor_NilaiSiswa_KURTILAS_Det.GetAllByTABySMByKelasDetByMapelByAPByKDByKPBySiswa_Entity(
                            tahun_ajaran, semester, rel_kelasdet, rel_mapel, rel_sn_ap, rel_sn_kd, rel_sn_kp, rel_siswa
                        );

                    //System.Threading.Thread.Sleep(500);

                    //System.Threading.Thread.Sleep(500);
                    if (lst_nilai_siswa_det.Count == 0)
                    {
                        DAO_Rapor_NilaiSiswa_KURTILAS_Det.Insert(new Rapor_NilaiSiswa_KURTILAS_Det
                        {
                            Kode = kode_nilai_siswa_det,
                            Rel_Rapor_NilaiSiswa_KURTILAS = kode_nilai_siswa,
                            Rel_Rapor_StrukturNilai_KURTILAS_AP = rel_sn_ap,
                            Rel_Rapor_StrukturNilai_KURTILAS_KD = rel_sn_kd,
                            Rel_Rapor_StrukturNilai_KURTILAS_KP = rel_sn_kp,
                            Rel_Siswa = rel_siswa,
                            Nilai = nilai
                        }, s_ssid);


                        //System.Threading.Thread.Sleep(500);
                    }
                    else
                    {
                        kode_nilai_siswa_det = lst_nilai_siswa_det.FirstOrDefault().Kode;

                        DAO_Rapor_NilaiSiswa_KURTILAS_Det.Update(new Rapor_NilaiSiswa_KURTILAS_Det
                        {
                            Kode = kode_nilai_siswa_det,
                            Rel_Rapor_NilaiSiswa_KURTILAS = kode_nilai_siswa,
                            Rel_Rapor_StrukturNilai_KURTILAS_AP = rel_sn_ap,
                            Rel_Rapor_StrukturNilai_KURTILAS_KD = rel_sn_kd,
                            Rel_Rapor_StrukturNilai_KURTILAS_KP = rel_sn_kp,
                            Rel_Siswa = rel_siswa,
                            Nilai = nilai
                        }, s_ssid);


                        //System.Threading.Thread.Sleep(500);
                    }
                }
            }

            return hasil.ToArray();
        }

        public string[] DoUpdateEkskul(
                string t,
                string sm,
                string s,
                string mp,
                string n,
                string lts_hd,
                string sakit,
                string izin,
                string alpa
            )
        {
            List<string> hasil = new List<string>();

            string tahun_ajaran = RandomLibs.GetParseTahunAjaran(t);
            string semester = sm;
            string rel_siswa = s;
            string nilai = n;
            string rel_mapel = mp;
            
            DAO_Rapor_NilaiEkskulSiswa.SaveNilaiEkskul(
                    tahun_ajaran,
                    semester,
                    rel_mapel,
                    rel_siswa,
                    nilai,
                    lts_hd,
                    sakit,
                    izin,
                    alpa
                );

            return hasil.ToArray();
        }

        public string[] DoUpdateLTSHDEkskul(
                string t,
                string sm,
                string s,
                string mp,
                string n
            )
        {
            List<string> hasil = new List<string>();

            string tahun_ajaran = RandomLibs.GetParseTahunAjaran(t);
            string semester = sm;
            string rel_siswa = s;
            string nilai = n;
            string rel_mapel = mp;

            DAO_Rapor_NilaiEkskulSiswa.SaveLTSHDEkskul(
                    tahun_ajaran,
                    semester,
                    rel_mapel,
                    rel_siswa,
                    nilai
                );

            return hasil.ToArray();
        }

        public string[] DoUpdateSakitEkskul(
                string t,
                string sm,
                string s,
                string mp,
                string n
            )
        {
            List<string> hasil = new List<string>();

            string tahun_ajaran = RandomLibs.GetParseTahunAjaran(t);
            string semester = sm;
            string rel_siswa = s;
            string nilai = n;
            string rel_mapel = mp;

            DAO_Rapor_NilaiEkskulSiswa.SaveSakitEkskul(
                    tahun_ajaran,
                    semester,
                    rel_mapel,
                    rel_siswa,
                    nilai
                );

            return hasil.ToArray();
        }

        public string[] DoUpdateIzinEkskul(
                string t,
                string sm,
                string s,
                string mp,
                string n
            )
        {
            List<string> hasil = new List<string>();

            string tahun_ajaran = RandomLibs.GetParseTahunAjaran(t);
            string semester = sm;
            string rel_siswa = s;
            string nilai = n;
            string rel_mapel = mp;

            DAO_Rapor_NilaiEkskulSiswa.SaveIzinEkskul(
                    tahun_ajaran,
                    semester,
                    rel_mapel,
                    rel_siswa,
                    nilai
                );

            return hasil.ToArray();
        }

        public string[] DoUpdateAlpaEkskul(
                string t,
                string sm,
                string s,
                string mp,
                string n
            )
        {
            List<string> hasil = new List<string>();

            string tahun_ajaran = RandomLibs.GetParseTahunAjaran(t);
            string semester = sm;
            string rel_siswa = s;
            string nilai = n;
            string rel_mapel = mp;

            DAO_Rapor_NilaiEkskulSiswa.SaveAlpaEkskul(
                    tahun_ajaran,
                    semester,
                    rel_mapel,
                    rel_siswa,
                    nilai
                );

            return hasil.ToArray();
        }

        public string[] DoUpdateSikap(
                string t,
                string sm,
                string s,
                string mp,
                string kd,
                string ssp,
                string sss
            )
        {
            List<string> hasil = new List<string>();

            string tahun_ajaran = RandomLibs.GetParseTahunAjaran(t);
            string semester = sm;
            string rel_siswa = s;
            string rel_mapel = mp;
            string rel_kelasdet = kd;
            string sikap_spiritual = ssp;
            string sikap_sosial = sss;

            DAO_Rapor_NilaiSikapSiswa.SaveNilaiSikap(
                    tahun_ajaran,
                    semester,
                    rel_mapel,
                    rel_kelasdet,
                    rel_siswa,
                    sikap_spiritual,
                    sikap_sosial
                );

            return hasil.ToArray();
        }
    }
}
