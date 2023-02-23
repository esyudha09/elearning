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
using AI_ERP.Application_Entities.Elearning.SMP;
using AI_ERP.Application_DAOs.Elearning.SMP;

namespace AI_ERP.APIs.Elearning.SMP.NilaiSiswa
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
                string nr,
                string pb,
                string lts_hd,
                string lts_maxhd,
                string sakit,
                string izin,
                string alpa,
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
            string lts_hadir = lts_hd;
            string lts_hadir_max = lts_maxhd;
            string s_pb = (pb == null ? "" : pb.ToString());
            string s_ssid = Libs.Dekrip(ssid);

            if (
                tahun_ajaran.Trim() != "" && semester.Trim() != "" && rel_kelasdet.Trim() != "" &&
                rel_siswa.Trim() != "" 
            )
            {
                List<Rapor_Nilai> lst_nilai = DAO_Rapor_Nilai.GetAllByTABySMByKelasDetByMapel_Entity(
                        tahun_ajaran, semester, rel_kelasdet, rel_mapel
                    );

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
                }
                else
                {
                    kode = lst_nilai.FirstOrDefault().Kode;
                }
                
                //add di nilai kurtilas
                Guid kode_nilai_siswa = Guid.NewGuid();
                List<Rapor_NilaiSiswa> lst_nilai_by_siswa = DAO_Rapor_NilaiSiswa.GetAllByHeaderBySiswa_Entity(
                        kode.ToString(), rel_siswa
                    );

                if (lst_nilai_by_siswa.Count == 0)
                {
                    DAO_Rapor_NilaiSiswa.Insert(new Rapor_NilaiSiswa
                    {
                        Kode = kode_nilai_siswa,
                        Rel_Rapor_Nilai = kode,
                        Rel_Siswa = rel_siswa,
                        Rapor = nr,
                        LTS_HD = lts_hd,
                        LTS_MAX_HD = lts_maxhd,
                        Sakit = sakit,
                        Izin = izin,
                        Alpa = alpa,
                        LTS_CK_KEHADIRAN = lts_ck_hd,
                        LTS_CK_KETEPATAN_WKT = lts_ck_kw,
                        LTS_CK_PENGGUNAAN_SRGM = lts_ck_ps,
                        LTS_CK_PENGGUNAAN_KMR = lts_ck_pk
                    }, s_ssid);
                }
                else
                {
                    kode_nilai_siswa = lst_nilai_by_siswa.FirstOrDefault().Kode;

                    DAO_Rapor_NilaiSiswa.Update(new Rapor_NilaiSiswa
                    {
                        Kode = kode_nilai_siswa,
                        Rel_Rapor_Nilai = kode,
                        Rel_Siswa = rel_siswa,
                        Rapor = nr,
                        LTS_HD = lts_hd,
                        LTS_MAX_HD = lts_maxhd,
                        Sakit = sakit,
                        Izin = izin,
                        Alpa = alpa,
                        LTS_CK_KEHADIRAN = lts_ck_hd,
                        LTS_CK_KETEPATAN_WKT = lts_ck_kw,
                        LTS_CK_PENGGUNAAN_SRGM = lts_ck_ps,
                        LTS_CK_PENGGUNAAN_KMR = lts_ck_pk
                    }, s_ssid);
                }

                if (rel_sn_kd.Trim() != "")
                {

                    //nilai detail siswa kurtilas
                    Guid kode_nilai_siswa_det = Guid.NewGuid();
                    List<Rapor_NilaiSiswa_Det> lst_nilai_siswa_det = DAO_Rapor_NilaiSiswa_Det.GetAllByTABySMByKelasDetByMapelByAPByKDByKPBySiswa_Entity(
                            tahun_ajaran, semester, rel_kelasdet, rel_mapel, rel_sn_ap, rel_sn_kd, rel_sn_kp, rel_siswa
                        );
                    if (lst_nilai_siswa_det.Count > 1)
                    {
                        DAO_Rapor_NilaiSiswa_Det.DeleteByTABySMByKelasDetByMapelByAPByKDByKPBySiswa_Entity(
                            tahun_ajaran, semester, rel_kelasdet, rel_mapel, rel_sn_ap, rel_sn_kd, rel_sn_kp, rel_siswa
                        );
                        lst_nilai_siswa_det.Clear();
                    }
                    if (lst_nilai_siswa_det.Count == 0)
                    {
                        DAO_Rapor_NilaiSiswa_Det.Insert(new Rapor_NilaiSiswa_Det
                        {
                            Kode = kode_nilai_siswa_det,
                            Rel_Rapor_NilaiSiswa = kode_nilai_siswa,
                            Rel_Rapor_StrukturNilai_AP = rel_sn_ap,
                            Rel_Rapor_StrukturNilai_KD = rel_sn_kd,
                            Rel_Rapor_StrukturNilai_KP = rel_sn_kp,
                            Rel_Siswa = rel_siswa,
                            Nilai = nilai
                        }, s_ssid);
                    }
                    else
                    {
                        kode_nilai_siswa_det = lst_nilai_siswa_det.FirstOrDefault().Kode;
                        if (s_pb.Trim() != "")
                        {
                            DAO_Rapor_NilaiSiswa_Det.UpdatePB(new Rapor_NilaiSiswa_Det
                            {
                                Kode = kode_nilai_siswa_det,
                                Rel_Rapor_NilaiSiswa = kode_nilai_siswa,
                                Rel_Rapor_StrukturNilai_AP = rel_sn_ap,
                                Rel_Rapor_StrukturNilai_KD = rel_sn_kd,
                                Rel_Rapor_StrukturNilai_KP = rel_sn_kp,
                                Rel_Siswa = rel_siswa,
                                PB = nilai
                            }, s_ssid);
                        }
                        else
                        {
                            DAO_Rapor_NilaiSiswa_Det.Update(new Rapor_NilaiSiswa_Det
                            {
                                Kode = kode_nilai_siswa_det,
                                Rel_Rapor_NilaiSiswa = kode_nilai_siswa,
                                Rel_Rapor_StrukturNilai_AP = rel_sn_ap,
                                Rel_Rapor_StrukturNilai_KD = rel_sn_kd,
                                Rel_Rapor_StrukturNilai_KP = rel_sn_kp,
                                Rel_Siswa = rel_siswa,
                                Nilai = nilai
                            }, s_ssid);
                        }
                    }
                }
            }

            return hasil.ToArray();
        }

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public string[] DoSaveEkskul(
                string j,
                string t,
                string sm,
                string k,
                string s,
                string n,
                string ap,
                string kd,
                string kp,
                string mp,
                string nr,
                string sk,
                string iz,
                string al,
                string ssid
            )
        {
            List<string> hasil = new List<string>();

            string jenis = j;
            string tahun_ajaran = RandomLibs.GetParseTahunAjaran(t);
            string semester = sm;
            string rel_kelas = k;
            string rel_siswa = s;
            string nilai = n;
            string rel_sn_ap = ap;
            string rel_sn_kd = kd;
            string rel_sn_kp = kp;
            string rel_mapel = mp;
            string sakit = sk;
            string izin = iz;
            string alpa = al;
            string s_ssid = Libs.Dekrip(ssid);
            
            if (
                tahun_ajaran.Trim() != "" && semester.Trim() != "" && rel_kelas.Trim() != "" &&
                rel_siswa.Trim() != "" && rel_sn_kd.Trim() != ""
            )
            {

                List<Rapor_Ekskul> lst_nilai = DAO_Rapor_Ekskul.GetAllByTABySMByKelasByMapel_Entity(
                            tahun_ajaran, semester, rel_kelas, rel_mapel
                        );

                //add di master nilai
                Guid kode = Guid.NewGuid();
                if (lst_nilai.Count == 0)
                {
                    DAO_Rapor_Ekskul.Insert(new Rapor_Ekskul
                    {
                        Kode = kode,
                        TahunAjaran = tahun_ajaran,
                        Semester = semester,
                        Rel_Kelas = rel_kelas,                        
                        Rel_Mapel = rel_mapel,
                        Kurikulum = ""
                    }, s_ssid);
                }
                else
                {
                    kode = lst_nilai.FirstOrDefault().Kode;
                }

                //add di nilai
                Guid kode_nilai_siswa = Guid.NewGuid();
                List<Rapor_EkskulSiswa> lst_nilai_by_siswa = DAO_Rapor_EkskulSiswa.GetAllByHeaderBySiswa_Entity(
                        kode.ToString(), rel_siswa
                    );

                if (lst_nilai_by_siswa.Count == 0)
                {
                    DAO_Rapor_EkskulSiswa.Insert(new Rapor_EkskulSiswa
                    {
                        Kode = kode_nilai_siswa,
                        Rel_Rapor_Ekskul = kode,
                        Rel_Siswa = rel_siswa,
                        Rapor = nr,
                        Sakit = sakit,
                        Izin = izin,
                        Alpa = alpa
                    }, s_ssid);
                }
                else
                {
                    kode_nilai_siswa = lst_nilai_by_siswa.FirstOrDefault().Kode;

                    DAO_Rapor_EkskulSiswa.Update(new Rapor_EkskulSiswa
                    {
                        Kode = kode_nilai_siswa,
                        Rel_Rapor_Ekskul = kode,
                        Rel_Siswa = rel_siswa,
                        Rapor = nr,
                        Sakit = sakit,
                        Izin = izin,
                        Alpa = alpa
                    }, s_ssid);
                }

                //nilai detail siswa
                Guid kode_nilai_siswa_det = Guid.NewGuid();
                List<Rapor_EkskulSiswa_Det> lst_nilai_siswa_det = DAO_Rapor_EkskulSiswa_Det.GetAllByTABySMByKelasByMapelByAPByKDByKPBySiswa_Entity(
                        tahun_ajaran, semester,rel_kelas,
                        rel_mapel, rel_sn_ap, rel_sn_kd, rel_sn_kp, rel_siswa
                    );
                if (lst_nilai_siswa_det.Count == 0)
                {
                    DAO_Rapor_EkskulSiswa_Det.Insert(new Rapor_EkskulSiswa_Det
                    {
                        Kode = kode_nilai_siswa_det,
                        Rel_Rapor_EkskulSiswa = kode_nilai_siswa,
                        Rel_Rapor_StrukturNilai_AP = rel_sn_ap,
                        Rel_Rapor_StrukturNilai_KD = rel_sn_kd,
                        Rel_Rapor_StrukturNilai_KP = rel_sn_kp,
                        Rel_Siswa = rel_siswa,
                        Nilai = nilai
                    }, s_ssid);
                }
                else
                {
                    kode_nilai_siswa_det = lst_nilai_siswa_det.FirstOrDefault().Kode;

                    DAO_Rapor_EkskulSiswa_Det.Update(new Rapor_EkskulSiswa_Det
                    {
                        Kode = kode_nilai_siswa_det,
                        Rel_Rapor_EkskulSiswa = kode_nilai_siswa,
                        Rel_Rapor_StrukturNilai_AP = rel_sn_ap,
                        Rel_Rapor_StrukturNilai_KD = rel_sn_kd,
                        Rel_Rapor_StrukturNilai_KP = rel_sn_kp,
                        Rel_Siswa = rel_siswa,
                        Nilai = nilai
                    }, s_ssid);
                }

            }

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
