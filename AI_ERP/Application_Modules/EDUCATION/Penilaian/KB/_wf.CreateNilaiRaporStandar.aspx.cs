using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities.Elearning.KB;
using AI_ERP.Application_DAOs.Elearning.KB;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.KB
{
    public partial class _wf_CreateNilaiRaporStandar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DoCreateNilaiStandar();
            }
        }

        protected List<Siswa> GetListSiswa(string rel_sekolah, string rel_kelasdet, string tahun_ajaran, string semester)
        {
            return DAO_Siswa.GetByRombel_Entity(
                        rel_sekolah,
                        rel_kelasdet,
                        tahun_ajaran,
                        semester
                    );
        }

        protected void DoCreateNilaiStandar()
        {
            string rel_rapordesign = Libs.GetQueryString("rd");
            string rel_kelasdet = Libs.GetQueryString("kd");
            string rel_kriteria = Libs.GetQueryString("kt");
            string rel_siswa = Libs.GetQueryString("s");
            string rel_mapel = Libs.GetQueryString("m");

            if (rel_rapordesign.Trim() != "" && rel_kelasdet.Trim() != "")
            {
                var m_rapordesign = DAO_Rapor_Design.GetByID_Entity(rel_rapordesign);
                var lst_nilaistandar = DAO_Rapor_NilaiStandar.GetByRaporDesignByKelasDet(rel_rapordesign, rel_kelasdet);
                if (lst_nilaistandar.Count == 0 || (rel_siswa.Trim() != "" && lst_nilaistandar.Count > 0))
                {
                    if (m_rapordesign != null)
                    {
                        if (m_rapordesign.TahunAjaran != null)
                        {
                            if (lst_nilaistandar.Count == 0)
                            {
                                DAO_Rapor_NilaiStandar.Insert(new Rapor_NilaiStandar
                                {
                                    Kode = Guid.NewGuid(),
                                    Rel_Rapor_Design = new Guid(rel_rapordesign),
                                    Rel_KelasDet = new Guid(rel_kelasdet),
                                    Rel_Rapor_Kriteria = new Guid(rel_kriteria),
                                    Rel_Mapel = rel_mapel
                                });
                            }

                            List<Rapor_Nilai> lst_nilai = DAO_Rapor_Nilai.GetAllByTABySMByKelasDet_Entity(
                                m_rapordesign.TahunAjaran, m_rapordesign.Semester, rel_kelasdet
                            );

                            Guid kode = Guid.NewGuid();
                            if (lst_nilai.Count == 0)
                            {
                                DAO_Rapor_Nilai.Insert(new Rapor_Nilai
                                {
                                    Kode = kode,
                                    TahunAjaran = m_rapordesign.TahunAjaran,
                                    Semester = m_rapordesign.Semester,
                                    Rel_KelasDet = rel_kelasdet,
                                    IsLocked = false,
                                    IsPosted = false
                                });
                            }
                            else
                            {
                                kode = lst_nilai.FirstOrDefault().Kode;
                            }

                            //get desain rapor
                            var lst_rapor_desain = DAO_Rapor_DesignDet.GetByHeader_Entity(rel_rapordesign);
                            //get desain rapor ekskul
                            var lst_rapor_desain_ekskul = DAO_Rapor_DesignDetEkskul.GetByHeader_Entity(rel_rapordesign);

                            List<Rapor_NilaiSiswa_Det> lst_nilai_siswa_det = DAO_Rapor_NilaiSiswa_Det.GetByHeader_Entity(
                                        kode.ToString()
                                    );

                            //get list siswa
                            var lst_siswa = GetListSiswa(
                                DAO_Sekolah.GetAll_Entity().FindAll(
                                    m => m.UrutanJenjang == (int)Libs.UnitSekolah.KB).FirstOrDefault().Kode.ToString(),
                                         rel_kelasdet, m_rapordesign.TahunAjaran, m_rapordesign.Semester
                                );
                            if (rel_siswa.Trim() != "")
                            {
                                lst_siswa = lst_siswa.FindAll(m => m.Kode.ToString().Trim().ToUpper() == rel_siswa.Trim().ToUpper());
                            }

                            int id_siswa = 0;
                            foreach (Siswa m_siswa in lst_siswa)
                            {
                                //nilai per siswa
                                //update detail 1
                                Guid kode_nilai_siswa = Guid.NewGuid();
                                List<Rapor_NilaiSiswa> lst_nilai_siswa = DAO_Rapor_NilaiSiswa.GetByHeader_Entity(kode.ToString());
                                lst_nilai_siswa = lst_nilai_siswa.FindAll(m => m.Rel_Siswa == m_siswa.Kode.ToString());

                                if (lst_nilai_siswa.Count == 0)
                                {
                                    DAO_Rapor_NilaiSiswa.Insert(new Rapor_NilaiSiswa
                                    {
                                        Kode = kode_nilai_siswa,
                                        Rel_Rapor_Nilai = kode,
                                        Rel_Siswa = m_siswa.Kode.ToString(),
                                        IsLocked = false,
                                        IsPosted = false,
                                        BeratBadan = "",
                                        TinggiBadan = "",
                                        LingkarKepala = "",
                                        Usia = ""
                                    });
                                }
                                else
                                {
                                    kode_nilai_siswa = lst_nilai_siswa.FirstOrDefault().Kode;
                                }
                                //end nilai per siswa

                                DAO_Rapor_NilaiSiswa_Det.CreateNilaiStandar(
                                        kode_nilai_siswa.ToString(), m_siswa.Kode.ToString(), rel_kriteria, rel_rapordesign
                                    );

                                id_siswa++;
                                //end get desain rapor ekskul
                            }
                            //end get list siswa
                        }
                    }
                }
            }
        }
    }
}