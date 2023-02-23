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
using AI_ERP.Application_Entities.Elearning.KB;
using AI_ERP.Application_DAOs.Elearning.KB;

namespace AI_ERP.APIs.Elearning.KB.NilaiSiswa
{
    public class DoSave : IDoSave
    {
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public string[] Do(string t, string sm, string s, string kd, string pp, string kr, string ds, string ns)
        {
            List<string> hasil = new List<string>();

            DAO_Rapor_Nilai.SaveNilai(
                    t, sm, s, pp, kd, kr, ds, ns
                );

            return hasil.ToArray();
        }

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public string[] DoPF(string t, string sm, string s, string kd, string bb, string tb, string lk, string u)
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
            string usia = u;

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

                if (lst_nilai.Count > 0) { 
                    kode = lst_nilai.FirstOrDefault().Kode;

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
                            TinggiBadan = tinggi_badan,
                            BeratBadan = berat_badan,
                            LingkarKepala = lingkar_kepala,
                            Usia = usia
                        });
                    }
                    else
                    {
                        var m_nilai_siswa = lst_nilai_siswa.FirstOrDefault();
                        m_nilai_siswa.TinggiBadan = tinggi_badan;
                        m_nilai_siswa.BeratBadan = berat_badan;
                        m_nilai_siswa.LingkarKepala = lingkar_kepala;
                        m_nilai_siswa.Usia = usia;

                        if (m_nilai_siswa != null)
                        {
                            if (m_nilai_siswa.BeratBadan != null)
                            {
                                DAO_Rapor_NilaiSiswa.Update(m_nilai_siswa);
                            }
                        }
                    }
                }
                
            }

            return hasil.ToArray();
        }
    }
}
