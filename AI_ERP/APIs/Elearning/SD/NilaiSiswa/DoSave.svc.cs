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
using AI_ERP.Application_Entities.Elearning.SD;
using AI_ERP.Application_DAOs.Elearning.SD;

namespace AI_ERP.APIs.Elearning.SD.NilaiSiswa
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
                string ms,
                string nr,
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
            string rel_mapel_sikap = ms;
            string s_ssid = Libs.Dekrip(ssid);

            if (
                tahun_ajaran.Trim() != "" && semester.Trim() != "" && rel_kelasdet.Trim() != "" &&
                rel_siswa.Trim() != ""
            )
            {

                if (rel_mapel_sikap.Trim() == "")
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

                    //add di nilai
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
                            LTS_CK_KEHADIRAN = lts_ck_hd,
                            LTS_CK_KETEPATAN_WKT = lts_ck_kw,
                            LTS_CK_PENGGUNAAN_SRGM = lts_ck_ps,
                            LTS_CK_PENGGUNAAN_KMR = lts_ck_pk
                        }, s_ssid);
                    }

                    if (rel_sn_kd.Trim() != "")
                    {
                        //nilai detail siswa
                        Guid kode_nilai_siswa_det = Guid.NewGuid();
                        List<Rapor_NilaiSiswa_Det> lst_nilai_siswa_det = DAO_Rapor_NilaiSiswa_Det.GetAllByTABySMByKelasDetByMapelByAPByKDByKPBySiswa_Entity(
                                tahun_ajaran, semester, rel_kelasdet, rel_mapel, rel_sn_ap, rel_sn_kd, rel_sn_kp, rel_siswa
                            );
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
                else if(rel_mapel_sikap.Trim() != "")
                {

                    List<Rapor_Sikap> lst_nilai = DAO_Rapor_Sikap.GetAllByTABySMByKelasDetByMapel_Entity(
                            tahun_ajaran, semester, rel_kelasdet, rel_mapel, rel_mapel_sikap
                        );

                    //add di master nilai
                    Guid kode = Guid.NewGuid();
                    if (lst_nilai.Count == 0)
                    {
                        DAO_Rapor_Sikap.Insert(new Rapor_Sikap
                        {
                            Kode = kode,
                            TahunAjaran = tahun_ajaran,
                            Semester = semester,
                            Rel_Kelas = rel_kelas,
                            Rel_KelasDet = rel_kelasdet,
                            Rel_Mapel = rel_mapel,
                            Rel_MapelSikap = rel_mapel_sikap,
                            Kurikulum = ""
                        }, s_ssid);
                    }
                    else
                    {
                        kode = lst_nilai.FirstOrDefault().Kode;
                    }

                    //add di nilai
                    Guid kode_nilai_siswa = Guid.NewGuid();
                    List<Rapor_SikapSiswa> lst_nilai_by_siswa = DAO_Rapor_SikapSiswa.GetAllByHeaderBySiswa_Entity(
                            kode.ToString(), rel_siswa
                        );

                    if (lst_nilai_by_siswa.Count == 0)
                    {
                        DAO_Rapor_SikapSiswa.Insert(new Rapor_SikapSiswa
                        {
                            Kode = kode_nilai_siswa,
                            Rel_Rapor_Sikap = kode,
                            Rel_Siswa = rel_siswa,
                            Rapor = nr
                        }, s_ssid);
                    }
                    else
                    {
                        kode_nilai_siswa = lst_nilai_by_siswa.FirstOrDefault().Kode;

                        DAO_Rapor_SikapSiswa.Update(new Rapor_SikapSiswa
                        {
                            Kode = kode_nilai_siswa,
                            Rel_Rapor_Sikap = kode,
                            Rel_Siswa = rel_siswa,
                            Rapor = nr
                        }, s_ssid);
                    }

                    //nilai detail siswa
                    Guid kode_nilai_siswa_det = Guid.NewGuid();
                    List<Rapor_SikapSiswa_Det> lst_nilai_siswa_det = DAO_Rapor_SikapSiswa_Det.GetAllByTABySMByKelasDetByMapelByAPByKDByKPBySiswa_Entity(
                            tahun_ajaran, semester, rel_kelasdet, rel_mapel, rel_sn_ap, rel_sn_kd, rel_sn_kp, rel_siswa
                        );
                    if (lst_nilai_siswa_det.Count == 0)
                    {
                        DAO_Rapor_SikapSiswa_Det.Insert(new Rapor_SikapSiswa_Det
                        {
                            Kode = kode_nilai_siswa_det,
                            Rel_Rapor_SikapSiswa = kode_nilai_siswa,
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

                        DAO_Rapor_SikapSiswa_Det.Update(new Rapor_SikapSiswa_Det
                        {
                            Kode = kode_nilai_siswa_det,
                            Rel_Rapor_SikapSiswa = kode_nilai_siswa,
                            Rel_Rapor_StrukturNilai_AP = rel_sn_ap,
                            Rel_Rapor_StrukturNilai_KD = rel_sn_kd,
                            Rel_Rapor_StrukturNilai_KP = rel_sn_kp,
                            Rel_Siswa = rel_siswa,
                            Nilai = nilai
                        }, s_ssid);
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

            string kelas1 = "";
            string kelas2 = "";
            string kelas3 = "";
            string kelas4 = "";
            string kelas5 = "";
            string kelas6 = "";

            string[] arr_kelas = rel_kelas.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            if (arr_kelas.Length == 1)
            {
                kelas1 = arr_kelas[0];
            }
            else if (arr_kelas.Length == 2)
            {
                kelas1 = arr_kelas[0];
                kelas2 = arr_kelas[1];
            }
            else if (arr_kelas.Length == 3)
            {
                kelas1 = arr_kelas[0];
                kelas2 = arr_kelas[1];
                kelas3 = arr_kelas[2];
            }
            else if (arr_kelas.Length == 4)
            {
                kelas1 = arr_kelas[0];
                kelas2 = arr_kelas[1];
                kelas3 = arr_kelas[2];
                kelas4 = arr_kelas[3];
            }
            else if (arr_kelas.Length == 5)
            {
                kelas1 = arr_kelas[0];
                kelas2 = arr_kelas[1];
                kelas3 = arr_kelas[2];
                kelas4 = arr_kelas[3];
                kelas5 = arr_kelas[4];
            }
            else if (arr_kelas.Length == 6)
            {
                kelas1 = arr_kelas[0];
                kelas2 = arr_kelas[1];
                kelas3 = arr_kelas[2];
                kelas4 = arr_kelas[3];
                kelas5 = arr_kelas[4];
                kelas6 = arr_kelas[5];
            }

            if (
                tahun_ajaran.Trim() != "" && semester.Trim() != "" && kelas1.Trim() != "" &&
                rel_siswa.Trim() != ""
            )
            {

                List<Rapor_Ekskul> lst_nilai = DAO_Rapor_Ekskul.GetAllByTABySMByKelasByMapel_Entity(
                            tahun_ajaran, semester, 
                            kelas1, kelas2, kelas3, kelas4, kelas5, kelas6,
                            rel_mapel
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
                        Rel_Kelas = kelas1,
                        Rel_Kelas2 = kelas2,
                        Rel_Kelas3 = kelas3,
                        Rel_Kelas4 = kelas4,
                        Rel_Kelas5 = kelas5,
                        Rel_Kelas6 = kelas6,
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

                    DAO_Rapor_EkskulSiswa.Update(new Rapor_EkskulSiswa
                    {
                        Kode = kode_nilai_siswa,
                        Rel_Rapor_Ekskul = kode,
                        Rel_Siswa = rel_siswa,
                        Rapor = nr,
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
                    //nilai detail siswa
                    Guid kode_nilai_siswa_det = Guid.NewGuid();
                    List<Rapor_EkskulSiswa_Det> lst_nilai_siswa_det = DAO_Rapor_EkskulSiswa_Det.GetAllByTABySMByKelasByMapelByAPByKDByKPBySiswa_Entity(
                            tahun_ajaran, semester,
                            kelas1, kelas2, kelas3, kelas4, kelas5, kelas6,
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
            }

            return hasil.ToArray();
        }

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public string[] DoSaveVolunteer(
                string j,
                string t,
                string sm,
                string k,
                string s,
                string n,
                string vset,
                string ssid
            )
        {
            List<string> hasil = new List<string>();

            string jenis = j;
            string tahun_ajaran = RandomLibs.GetParseTahunAjaran(t);
            string semester = sm;
            string rel_kelas_det = k;
            string rel_siswa = s;
            string nilai = n;
            string volunteer_settings = vset; 
            string s_ssid = Libs.Dekrip(ssid);

            if (
                tahun_ajaran.Trim() != "" && semester.Trim() != "" && 
                rel_siswa.Trim() != ""
            )
            {

                List<Rapor_Volunteer> lst_nilai = DAO_Rapor_Volunteer.GetAllByTABySMByKelasByMapel_Entity(
                        tahun_ajaran, sm, rel_kelas_det
                    );

                //add di master nilai
                Guid kode = Guid.NewGuid();
                if (lst_nilai.Count == 0)
                {
                    DAO_Rapor_Volunteer.Insert(new Rapor_Volunteer
                    {
                        Kode = kode,
                        TahunAjaran = tahun_ajaran,
                        Semester = semester,
                        Rel_KelasDet = rel_kelas_det,
                        Kurikulum = ""
                    }, s_ssid);
                }
                else
                {
                    kode = lst_nilai.FirstOrDefault().Kode;
                }

                //add di nilai
                Guid kode_nilai_siswa = Guid.NewGuid();
                List<Rapor_VolunteerSiswa> lst_nilai_by_siswa = DAO_Rapor_VolunteerSiswa.GetAllByHeaderBySiswa_Entity(
                        kode.ToString(), rel_siswa
                    );
                lst_nilai_by_siswa = lst_nilai_by_siswa.FindAll(m => m.Rel_Rapor_Volunteer_Settings_Det == vset).ToList();

                if (lst_nilai_by_siswa.Count == 0)
                {
                    DAO_Rapor_VolunteerSiswa.Insert(new Rapor_VolunteerSiswa
                    {
                        Kode = kode_nilai_siswa,
                        Rel_Rapor_Volunteer = kode,
                        Rel_Siswa = rel_siswa,
                        Rel_Rapor_Volunteer_Settings_Det = vset,
                        Nilai = n
                    }, s_ssid);
                }
                else
                {
                    kode_nilai_siswa = lst_nilai_by_siswa.FirstOrDefault().Kode;

                    DAO_Rapor_VolunteerSiswa.Update(new Rapor_VolunteerSiswa
                    {
                        Kode = kode_nilai_siswa,
                        Rel_Rapor_Volunteer = kode,
                        Rel_Siswa = rel_siswa,
                        Rel_Rapor_Volunteer_Settings_Det = vset,
                        Nilai = n
                    }, s_ssid);
                }

            }

            return hasil.ToArray();
        }

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public string[] DoSaveSikapSemester(
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
                string ms,
                string nr,
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
            string rel_mapel_sikap = ms;
            string s_ssid = Libs.Dekrip(ssid);

            if (
                tahun_ajaran.Trim() != "" && semester.Trim() != "" && rel_kelasdet.Trim() != "" &&
                rel_siswa.Trim() != "" && rel_sn_kd.Trim() != ""
            )
            {

                if (rel_mapel_sikap.Trim() != "")
                {

                    List<Rapor_SikapSemester> lst_nilai = DAO_Rapor_SikapSemester.GetAllByTABySMByKelasDetByMapel_Entity(
                            tahun_ajaran, semester, rel_kelasdet, rel_mapel, rel_mapel_sikap
                        );

                    //add di master nilai
                    Guid kode = Guid.NewGuid();
                    if (lst_nilai.Count == 0)
                    {
                        DAO_Rapor_SikapSemester.Insert(new Rapor_SikapSemester
                        {
                            Kode = kode,
                            TahunAjaran = tahun_ajaran,
                            Semester = semester,
                            Rel_Kelas = rel_kelas,
                            Rel_KelasDet = rel_kelasdet,
                            Rel_Mapel = rel_mapel,
                            Rel_MapelSikap = rel_mapel_sikap,
                            Kurikulum = ""
                        }, s_ssid);
                    }
                    else
                    {
                        kode = lst_nilai.FirstOrDefault().Kode;
                    }

                    //add di nilai
                    Guid kode_nilai_siswa = Guid.NewGuid();
                    List<Rapor_SikapSemesterSiswa> lst_nilai_by_siswa = DAO_Rapor_SikapSemesterSiswa.GetAllByHeaderBySiswa_Entity(
                            kode.ToString(), rel_siswa
                        );

                    if (lst_nilai_by_siswa.Count == 0)
                    {
                        DAO_Rapor_SikapSemesterSiswa.Insert(new Rapor_SikapSemesterSiswa
                        {
                            Kode = kode_nilai_siswa,
                            Rel_Rapor_Sikap = kode,
                            Rel_Siswa = rel_siswa,
                            Rapor = nr
                        }, s_ssid);
                    }
                    else
                    {
                        kode_nilai_siswa = lst_nilai_by_siswa.FirstOrDefault().Kode;

                        DAO_Rapor_SikapSemesterSiswa.Update(new Rapor_SikapSemesterSiswa
                        {
                            Kode = kode_nilai_siswa,
                            Rel_Rapor_Sikap = kode,
                            Rel_Siswa = rel_siswa,
                            Rapor = nr
                        }, s_ssid);
                    }

                    //nilai detail siswa
                    Guid kode_nilai_siswa_det = Guid.NewGuid();
                    List<Rapor_SikapSemesterSiswa_Det> lst_nilai_siswa_det = DAO_Rapor_SikapSemesterSiswa_Det.GetAllByTABySMByKelasDetByMapelByAPByKDByKPBySiswa_Entity(
                            tahun_ajaran, semester, rel_kelasdet, rel_mapel, rel_sn_ap, rel_sn_kd, rel_sn_kp, rel_siswa
                        );
                    if (lst_nilai_siswa_det.Count == 0)
                    {
                        DAO_Rapor_SikapSemesterSiswa_Det.Insert(new Rapor_SikapSemesterSiswa_Det
                        {
                            Kode = kode_nilai_siswa_det,
                            Rel_Rapor_SikapSiswa = kode_nilai_siswa,
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

                        DAO_Rapor_SikapSemesterSiswa_Det.Update(new Rapor_SikapSemesterSiswa_Det
                        {
                            Kode = kode_nilai_siswa_det,
                            Rel_Rapor_SikapSiswa = kode_nilai_siswa,
                            Rel_Rapor_StrukturNilai_AP = rel_sn_ap,
                            Rel_Rapor_StrukturNilai_KD = rel_sn_kd,
                            Rel_Rapor_StrukturNilai_KP = rel_sn_kp,
                            Rel_Siswa = rel_siswa,
                            Nilai = nilai
                        }, s_ssid);
                    }

                }

            }

            return hasil.ToArray();
        }
    }
}
