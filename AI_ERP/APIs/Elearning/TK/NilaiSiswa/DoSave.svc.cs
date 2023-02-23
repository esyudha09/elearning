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
using AI_ERP.Application_Entities.Elearning.TK;
using AI_ERP.Application_DAOs.Elearning.TK;

namespace AI_ERP.APIs.Elearning.TK.NilaiSiswa
{
    public class DoSave : IDoSave
    {
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public string[] Do(string t, string sm, string s, string kd, string pp, string kr, string ds)
        {
            List<string> hasil = new List<string>();
            Guid kode = Guid.NewGuid();

            string tahun_ajaran = t;
            string semester = sm;
            string rel_siswa = s;
            string rel_rapordesigndet = pp;
            string rel_kelasdet = kd;
            string rel_kriteria = kr;
            string deskripsi = ds;

            if (
                tahun_ajaran.Trim() != "" &&
                semester.Trim() != "" &&
                rel_siswa.Trim() != "" &&
                rel_kelasdet.Trim() != ""
            )
            {
                //update header
                List<Rapor_Nilai> lst_nilai = DAO_Rapor_Nilai.GetAllByTABySMByKelasDet_Entity(
                        tahun_ajaran, semester, rel_kelasdet
                    );

                if (lst_nilai.Count == 0)
                {
                    DAO_Rapor_Nilai.Insert(new Rapor_Nilai
                    {
                        Kode = kode,
                        TahunAjaran = tahun_ajaran,
                        Semester = semester,
                        Rel_KelasDet = rel_kelasdet,
                        IsLocked = false,
                        IsPosted = false
                    });
                }
                else
                {
                    kode = lst_nilai.FirstOrDefault().Kode;
                }

                //update detail 1
                Guid kode_nilai_siswa = Guid.NewGuid();
                List<Rapor_NilaiSiswa> lst_nilai_siswa = DAO_Rapor_NilaiSiswa.GetByHeader_Entity(kode.ToString());
                lst_nilai_siswa = lst_nilai_siswa.FindAll(m => m.Rel_Siswa == rel_siswa);

                if (lst_nilai_siswa.Count == 0)
                {
                    DAO_Rapor_NilaiSiswa.Insert(new Rapor_NilaiSiswa {
                        Kode = kode_nilai_siswa,
                        Rel_Rapor_Nilai = kode,
                        Rel_Siswa = rel_siswa,
                        IsLocked = false,
                        IsPosted = false,
                        TinggiBadan = "",
                        BeratBadan = "",
                        LingkarKepala = "",
                        LTS_CK_KEHADIRAN = "",
                        LTS_CK_KETEPATAN_WKT = "",
                        LTS_CK_PENGGUNAAN_SRGM = "",
                        LTS_CK_PENGGUNAAN_KMR = ""
                    });
                }
                else
                {
                    kode_nilai_siswa = lst_nilai_siswa.FirstOrDefault().Kode;
                }

                //update detail 2 (nilai)
                Guid kode_nilai_siswa_det = Guid.NewGuid();
                List<Rapor_NilaiSiswa_Det> lst_nilai_siswa_det = DAO_Rapor_NilaiSiswa_Det.GetPoinPenilaian_Entity(
                        kode_nilai_siswa.ToString(), rel_siswa, rel_rapordesigndet
                    );

                if (lst_nilai_siswa_det.Count == 0)
                {
                    DAO_Rapor_NilaiSiswa_Det.Insert(new Rapor_NilaiSiswa_Det {
                        Kode = kode_nilai_siswa_det,
                        Rel_Siswa = rel_siswa,
                        Rel_Rapor_NilaiSiswa = kode_nilai_siswa,
                        Rel_Rapor_DesignDet = rel_rapordesigndet,
                        Rel_Rapor_Kriteria = rel_kriteria,
                        Deskripsi = deskripsi.Replace(Constantas.HASHTAG_REP, "#").
                                              Replace("'", "&#39;")
                    });
                }
                else
                {
                    kode_nilai_siswa_det = lst_nilai_siswa_det.FirstOrDefault().Kode;
                    DAO_Rapor_NilaiSiswa_Det.Update(new Rapor_NilaiSiswa_Det
                    {
                        Kode = kode_nilai_siswa_det,
                        Rel_Siswa = rel_siswa,
                        Rel_Rapor_NilaiSiswa = kode_nilai_siswa,
                        Rel_Rapor_DesignDet = rel_rapordesigndet,
                        Rel_Rapor_Kriteria = rel_kriteria,
                        Deskripsi = deskripsi.Replace(Constantas.HASHTAG_REP, "#").
                                              Replace("'", "&#39;")
                    });
                }
            }

            return hasil.ToArray();
        }

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public string[] DoPF(string t, string sm, string s, string kd, string bb, string tb, string lk)
        {
            List<string> hasil = new List<string>();
            Guid kode = Guid.NewGuid();

            string tahun_ajaran = t;
            string semester = sm;
            string rel_siswa = s;
            string rel_kelasdet = kd;
            string berat_badan = bb;
            string tinggi_badan = tb;
            string lingkar_kepala = lk;

            if (
                tahun_ajaran.Trim() != "" &&
                semester.Trim() != "" &&
                rel_siswa.Trim() != "" &&
                rel_kelasdet.Trim() != ""
            )
            {
                //update header
                List<Rapor_Nilai> lst_nilai = DAO_Rapor_Nilai.GetAllByTABySMByKelasDet_Entity(
                        tahun_ajaran, semester, rel_kelasdet
                    );

                if (lst_nilai.Count == 0)
                {
                    DAO_Rapor_Nilai.Insert(new Rapor_Nilai
                    {
                        Kode = kode,
                        TahunAjaran = tahun_ajaran,
                        Semester = semester,
                        Rel_KelasDet = rel_kelasdet,
                        IsLocked = false,
                        IsPosted = false
                    });
                }
                else
                {
                    kode = lst_nilai.FirstOrDefault().Kode;
                }

                //update detail 1
                Guid kode_nilai_siswa = Guid.NewGuid();
                List<Rapor_NilaiSiswa> lst_nilai_siswa = DAO_Rapor_NilaiSiswa.GetByHeader_Entity(kode.ToString());
                lst_nilai_siswa = lst_nilai_siswa.FindAll(m => m.Rel_Siswa == rel_siswa);

                if (lst_nilai_siswa.Count == 0)
                {
                    DAO_Rapor_NilaiSiswa.Insert(new Rapor_NilaiSiswa
                    {
                        Kode = kode_nilai_siswa,
                        Rel_Rapor_Nilai = kode,
                        Rel_Siswa = rel_siswa,
                        IsLocked = false,
                        IsPosted = false,
                        TinggiBadan = tb,
                        BeratBadan = bb,
                        LingkarKepala = lk,
                        LTS_CK_KEHADIRAN = "",
                        LTS_CK_KETEPATAN_WKT = "",
                        LTS_CK_PENGGUNAAN_SRGM = "",
                        LTS_CK_PENGGUNAAN_KMR = ""
                    });
                }
                else
                {
                    var m_nilai_siswa = lst_nilai_siswa.FirstOrDefault();
                    m_nilai_siswa.TinggiBadan = tb;
                    m_nilai_siswa.BeratBadan = bb;
                    m_nilai_siswa.LingkarKepala = lk;

                    if (m_nilai_siswa != null)
                    {
                        if (m_nilai_siswa.BeratBadan != null)
                        {
                            DAO_Rapor_NilaiSiswa.Update(m_nilai_siswa);
                        }
                    }
                }
            }

            return hasil.ToArray();
        }

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public string[] DoCK(string t, string sm, string s, string kd, string lst_ck_abs)
        {
            List<string> hasil = new List<string>();
            Guid kode = Guid.NewGuid();

            string tahun_ajaran = t;
            string semester = sm;
            string rel_siswa = s;
            string rel_kelasdet = kd;

            if (
                tahun_ajaran.Trim() != "" &&
                semester.Trim() != "" &&
                rel_siswa.Trim() != "" &&
                rel_kelasdet.Trim() != ""
            )
            {
                //update header
                List<Rapor_Nilai> lst_nilai = DAO_Rapor_Nilai.GetAllByTABySMByKelasDet_Entity(
                        tahun_ajaran, semester, rel_kelasdet
                    );

                if (lst_nilai.Count == 0)
                {
                    DAO_Rapor_Nilai.Insert(new Rapor_Nilai
                    {
                        Kode = kode,
                        TahunAjaran = tahun_ajaran,
                        Semester = semester,
                        Rel_KelasDet = rel_kelasdet,
                        IsLocked = false,
                        IsPosted = false
                    });
                }
                else
                {
                    kode = lst_nilai.FirstOrDefault().Kode;
                }

                //update detail 1
                Guid kode_nilai_siswa = Guid.NewGuid();
                List<Rapor_NilaiSiswa> lst_nilai_siswa = DAO_Rapor_NilaiSiswa.GetByHeader_Entity(kode.ToString());
                lst_nilai_siswa = lst_nilai_siswa.FindAll(m => m.Rel_Siswa == rel_siswa);

                if (lst_nilai_siswa.Count == 0)
                {
                    DAO_Rapor_NilaiSiswa.Insert(new Rapor_NilaiSiswa
                    {
                        Kode = kode_nilai_siswa,
                        Rel_Rapor_Nilai = kode,
                        Rel_Siswa = rel_siswa,
                        IsLocked = false,
                        IsPosted = false,
                        TinggiBadan = "",
                        BeratBadan = "",
                        LingkarKepala = "",
                        LTS_CK_KEHADIRAN = lst_ck_abs,
                        LTS_CK_KETEPATAN_WKT = "",
                        LTS_CK_PENGGUNAAN_SRGM = "",
                        LTS_CK_PENGGUNAAN_KMR = ""
                    });
                }
                else
                {
                    var m_nilai_siswa = lst_nilai_siswa.FirstOrDefault();
                    m_nilai_siswa.LTS_CK_KEHADIRAN = lst_ck_abs;

                    if (m_nilai_siswa != null)
                    {
                        if (m_nilai_siswa.BeratBadan != null)
                        {
                            DAO_Rapor_NilaiSiswa.Update(m_nilai_siswa);
                        }
                    }
                }
            }

            return hasil.ToArray();
        }
    }
}
