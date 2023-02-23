using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Entities.Elearning.SD;
using AI_ERP.Application_DAOs.Elearning.SD;

namespace AI_ERP.Application_Modules.__LOADER
{
    public partial class wf_BukaSemester : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DoCreateSemester();
        }

        protected void DoCreateSemesterUnit(string rel_sekolah, string tahun_ajaran, string semester, string tahun_ajaran_before, string semester_before)
        {
            Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(rel_sekolah);
            if (m_sekolah != null)
            {
                if (m_sekolah.Nama != null)
                {
                    DoCreateFormasiGuru(rel_sekolah, tahun_ajaran, semester, tahun_ajaran_before, semester_before);

                    if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.KB)
                    {
                        DoCreateSemester_KB(rel_sekolah, tahun_ajaran, semester, tahun_ajaran_before, semester_before);
                    }
                    else if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.TK)
                    {
                        DoCreateSemester_TK(rel_sekolah, tahun_ajaran, semester, tahun_ajaran_before, semester_before);
                    }
                    else if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SD)
                    {
                        DoCreateSemester_SD(rel_sekolah, tahun_ajaran, semester, tahun_ajaran_before, semester_before);
                    }
                    else if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMP)
                    {
                        DoCreateSemester_SMP(rel_sekolah, tahun_ajaran, semester, tahun_ajaran_before, semester_before);
                    }
                    else if (m_sekolah.UrutanJenjang == (int)Libs.UnitSekolah.SMA)
                    {
                        DoCreateSemester_SMA(rel_sekolah, tahun_ajaran, semester, tahun_ajaran_before, semester_before);
                    }

                    //biodata siswa
                    DAO_Siswa.CreateBackUp(rel_sekolah, tahun_ajaran_before, semester_before, tahun_ajaran, semester);
                    //end biodata siswa
                }
            }
        }

        protected void DoCreateFormasiGuru(string rel_sekolah, string tahun_ajaran, string semester, string tahun_ajaran_before, string semester_before)
        {
            if (semester == "2")
            {
                tahun_ajaran_before = tahun_ajaran;
                semester_before = "1"; 
            }

            //delete formasi guru kelas
            List<FormasiGuruKelas> lst_fgk_del = DAO_FormasiGuruKelas.GetByUnitByTABySM_Entity(
                    rel_sekolah, tahun_ajaran, semester
                );
            //foreach (var item in lst_fgk_del)
            //{
            //    DAO_FormasiGuruKelas.Delete(item.Kode.ToString(), Libs.LOGGED_USER_M.UserID);
            //}
            //end delete formasi guru matpel

            //formasi guru kelas
            List<FormasiGuruKelas> lst_fgk = DAO_FormasiGuruKelas.GetByUnitByTABySM_Entity(
                    rel_sekolah, tahun_ajaran_before, semester_before
                );
            foreach (var item in lst_fgk)
            {
                if (lst_fgk_del.FindAll(
                        m0 => m0.Rel_Sekolah == item.Rel_Sekolah &&
                              m0.TahunAjaran.Trim() == item.TahunAjaran.Trim() &&
                              m0.Semester.Trim() == item.Semester.Trim() &&
                              m0.Rel_KelasDet.ToUpper().Trim() == item.Rel_KelasDet.ToUpper().Trim()
                    ).Count == 0)
                {
                    DAO_FormasiGuruKelas.Insert(new FormasiGuruKelas
                    {
                        Kode = Guid.NewGuid(),
                        Rel_GuruKelas = item.Rel_GuruKelas,
                        Rel_GuruPendamping = item.Rel_GuruPendamping,
                        Rel_KelasDet = item.Rel_KelasDet,
                        Rel_Sekolah = item.Rel_Sekolah,
                        TahunAjaran = tahun_ajaran,
                        Semester = semester
                    }, Libs.LOGGED_USER_M.UserID);
                }
            }
            //end formasi guru kelas

            //delete formasi guru matpel
            List<FormasiGuruMapel> lst_fgm_del = DAO_FormasiGuruMapel.GetByUnitByTABySM_Entity(
                    rel_sekolah, tahun_ajaran, semester
                );
            //foreach (var item in lst_fgm_del)
            //{
            //    DAO_FormasiGuruMapel.Delete(item.Kode.ToString(), Libs.LOGGED_USER_M.UserID);
            //}
            //end delete formasi guru matpel

            //formasi guru mata pelajaran
            List<FormasiGuruMapel> lst_fgm = DAO_FormasiGuruMapel.GetByUnitByTABySM_Entity(
                    rel_sekolah, tahun_ajaran_before, semester_before
                );
            foreach (var item in lst_fgm)
            {
                if (lst_fgm_del.FindAll(
                        m0 => m0.Rel_Sekolah == item.Rel_Sekolah &&
                              m0.TahunAjaran.Trim() == item.TahunAjaran.Trim() &&
                              m0.Semester.Trim() == item.Semester.Trim() &&
                              m0.Rel_Mapel.ToUpper().Trim() == item.Rel_Mapel.ToUpper().Trim() &&
                              m0.Rel_Kelas.ToUpper().Trim() == item.Rel_Kelas.ToUpper().Trim()
                    ).Count == 0)
                {
                    string kode = Guid.NewGuid().ToString();
                    DAO_FormasiGuruMapel.Insert(new FormasiGuruMapel
                    {
                        Kode = new Guid(kode),
                        TahunAjaran = tahun_ajaran,
                        Semester = semester,
                        Rel_Sekolah = new Guid(rel_sekolah),
                        Rel_Kelas = item.Rel_Kelas,
                        Rel_Mapel = item.Rel_Mapel
                    }, Libs.LOGGED_USER_M.UserID);

                    List<FormasiGuruMapelDetSiswa> lst_fgm_det_siswa = DAO_FormasiGuruMapelDetSiswa.GetByHeader_Entity(item.Kode.ToString());
                    foreach (var item_fgm_det_siswa in lst_fgm_det_siswa)
                    {
                        DAO_FormasiGuruMapelDetSiswa.Insert(new FormasiGuruMapelDetSiswa
                        {
                            Kode = Guid.NewGuid(),
                            Rel_FormasiGuruMapel = new Guid(kode),
                            Rel_Siswa = item_fgm_det_siswa.Rel_Siswa,
                            Urutan = item_fgm_det_siswa.Urutan
                        }, Libs.LOGGED_USER_M.UserID);
                    }

                    //formasi guru mata pelajaran det
                    List<FormasiGuruMapelDet> lst_fgm_det = DAO_FormasiGuruMapelDet.GetByHeader_Entity(item.Kode.ToString());
                    foreach (var item_fgm_det in lst_fgm_det)
                    {
                        if (DAO_FormasiGuruMapelDet.GetByHeader_Entity(kode).FindAll(m => m.Rel_Guru == item_fgm_det.Rel_Guru && m.Rel_KelasDet == item_fgm_det.Rel_KelasDet).Count == 0)
                        {
                            Guid kode_fgm_det = Guid.NewGuid();
                            DAO_FormasiGuruMapelDet.Insert(new FormasiGuruMapelDet
                            {
                                Kode = Guid.NewGuid(),
                                Rel_FormasiGuruMapel = new Guid(kode),
                                Rel_KelasDet = item_fgm_det.Rel_KelasDet,
                                Rel_Guru = item_fgm_det.Rel_Guru,
                                Keterangan = item_fgm_det.Keterangan,
                                Urutan = item_fgm_det.Urutan
                            }, Libs.LOGGED_USER_M.UserID);

                            if (semester.Trim().ToUpper() == semester_before.Trim().ToUpper())
                            {
                                List<FormasiGuruMapelDetSiswaDet> lst_fgm_det_siswa_det = DAO_FormasiGuruMapelDetSiswaDet.GetByHeader_Entity(item.Kode.ToString());
                                foreach (var item_fgm_det_siswa in lst_fgm_det_siswa_det)
                                {
                                    DAO_FormasiGuruMapelDetSiswaDet.Insert(new FormasiGuruMapelDetSiswaDet
                                    {
                                        Kode = Guid.NewGuid(),
                                        Rel_FormasiGuruMapelDet = kode_fgm_det.ToString(),
                                        Rel_Siswa = item_fgm_det_siswa.Rel_Siswa,
                                        Urutan = item_fgm_det_siswa.Urutan
                                    });
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void DoCreateSemester_KB(string rel_sekolah, string tahun_ajaran, string semester, string tahun_ajaran_before, string semester_before)
        {
            //desain rapor
            List<Application_Entities.Elearning.KB.Rapor_Design> lst_design_rapor =
                Application_DAOs.Elearning.KB.DAO_Rapor_Design.GetAll_Entity().FindAll(m => m.TahunAjaran == tahun_ajaran_before && m.Semester == semester_before);
            List<Application_Entities.Elearning.KB.Rapor_Design> lst_design_rapor_new =
                Application_DAOs.Elearning.KB.DAO_Rapor_Design.GetAll_Entity().FindAll(m => m.TahunAjaran == tahun_ajaran && m.Semester == semester);

            foreach (var item_design_rapor in lst_design_rapor)
            {
                if (lst_design_rapor_new.FindAll(
                        m0 => m0.Rel_Kelas.ToString().ToUpper().Trim() == item_design_rapor.Rel_Kelas.ToString().ToUpper().Trim()
                    ).Count == 0)
                {

                    string kode = Guid.NewGuid().ToString();
                    Application_DAOs.Elearning.KB.DAO_Rapor_Design.Insert(new Application_Entities.Elearning.KB.Rapor_Design
                    {
                        Kode = new Guid(kode),
                        TahunAjaran = tahun_ajaran,
                        Semester = semester,
                        JenisRapor = item_design_rapor.JenisRapor,
                        Rel_Kelas = item_design_rapor.Rel_Kelas,
                        Keterangan = item_design_rapor.Keterangan,
                        IsLocked = item_design_rapor.IsLocked,
                        TipeRapor = item_design_rapor.TipeRapor
                    }, Libs.LOGGED_USER_M.UserID);

                    //desain rapor kriteria
                    List<Application_Entities.Elearning.KB.Rapor_DesignKriteria> lst_design_kriteria =
                        Application_DAOs.Elearning.KB.DAO_Rapor_DesignKriteria.GetByHeader_Entity(item_design_rapor.Kode.ToString());
                    foreach (var item_design_kriteria in lst_design_kriteria)
                    {
                        Application_DAOs.Elearning.KB.DAO_Rapor_DesignKriteria.Insert(new Application_Entities.Elearning.KB.Rapor_DesignKriteria
                        {
                            Kode = Guid.NewGuid(),
                            Rel_Rapor_Design = new Guid(kode),
                            Rel_Rapor_Kriteria = item_design_kriteria.Rel_Rapor_Kriteria,
                            Urut = item_design_kriteria.Urut
                        }, Libs.LOGGED_USER_M.UserID);
                    }
                    //end desain rapor kriteria

                    List<Application_Entities.Elearning.KB.Rapor_DesignDet> lst_design_rapor_det =
                        Application_DAOs.Elearning.KB.DAO_Rapor_DesignDet.GetByHeader_Entity(item_design_rapor.Kode.ToString());

                    foreach (var item_design_rapor_det in lst_design_rapor_det)
                    {
                        Application_DAOs.Elearning.KB.DAO_Rapor_DesignDet.InsertLengkap(new Application_Entities.Elearning.KB.Rapor_DesignDet
                        {
                            Kode = Guid.NewGuid(),
                            Rel_Rapor_Design = new Guid(kode),
                            Poin = item_design_rapor_det.Poin,
                            Rel_KomponenRapor = item_design_rapor_det.Rel_KomponenRapor,
                            JenisKomponen = item_design_rapor_det.JenisKomponen,
                            Urut = item_design_rapor_det.Urut,
                            IsNewPage = item_design_rapor_det.IsNewPage,
                            IsLockGuruKelas = item_design_rapor_det.IsLockGuruKelas
                        }, Libs.LOGGED_USER_M.UserID);
                    }
                }
            }

            //desain rapor ekskul
            List<Application_Entities.Elearning.KB.Rapor_DesignEkskul> lst_design_rapor_ekskul =
                Application_DAOs.Elearning.KB.DAO_Rapor_DesignEkskul.GetAll_Entity().FindAll(m => m.TahunAjaran == tahun_ajaran_before && m.Semester == semester_before);
            List<Application_Entities.Elearning.KB.Rapor_DesignEkskul> lst_design_rapor_ekskul_new =
                Application_DAOs.Elearning.KB.DAO_Rapor_DesignEkskul.GetAll_Entity().FindAll(m => m.TahunAjaran == tahun_ajaran && m.Semester == semester);

            foreach (var item_design_rapor in lst_design_rapor_ekskul)
            {
                if (lst_design_rapor_ekskul_new.FindAll(
                        m0 => m0.Rel_Mapel.ToString().ToUpper().Trim() == item_design_rapor.Rel_Mapel.ToString().ToUpper().Trim() &&
                              m0.Rel_Kelas.ToString().ToUpper().Trim() == item_design_rapor.Rel_Kelas.ToString().ToUpper().Trim()
                    ).Count == 0)
                {
                    string kode = Guid.NewGuid().ToString();
                    Application_DAOs.Elearning.KB.DAO_Rapor_DesignEkskul.Insert(new Application_Entities.Elearning.KB.Rapor_DesignEkskul
                    {
                        Kode = new Guid(kode),
                        TahunAjaran = tahun_ajaran,
                        Semester = semester,
                        Rel_Mapel = item_design_rapor.Rel_Mapel,
                        Rel_Kelas = item_design_rapor.Rel_Kelas
                    }, Libs.LOGGED_USER_M.UserID);

                    List<Application_Entities.Elearning.KB.Rapor_DesignEkskulDet> lst_design_rapor_det =
                        Application_DAOs.Elearning.KB.DAO_Rapor_DesignEkskulDet.GetByHeader_Entity(item_design_rapor.Kode.ToString());

                    foreach (var item_design_rapor_det in lst_design_rapor_det)
                    {
                        Application_DAOs.Elearning.KB.DAO_Rapor_DesignEkskulDet.InsertLengkap(new Application_Entities.Elearning.KB.Rapor_DesignEkskulDet
                        {
                            Kode = Guid.NewGuid(),
                            Rel_Rapor_DesignEkskul = new Guid(kode),
                            Poin = item_design_rapor_det.Poin,
                            Rel_KomponenRapor = item_design_rapor_det.Rel_KomponenRapor,
                            JenisKomponen = item_design_rapor_det.JenisKomponen,
                            Urut = item_design_rapor_det.Urut
                        }, Libs.LOGGED_USER_M.UserID);
                    }
                }
            }

            //copy file rapor LTS
            string rapor_sc = Libs.GetNamaPeriodeReportRapor(tahun_ajaran_before, semester_before, "LTS.rpt");
            string rapor_ds = Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "LTS.rpt");
            Libs.CopyFile(@"Application_Reports\Penilaian\KB\" + rapor_sc, @"Application_Reports\Penilaian\KB\" + rapor_ds);
            //end copy file rapor LTS

            //copy file rapor SEMESTER
            rapor_sc = Libs.GetNamaPeriodeReportRapor(tahun_ajaran_before, semester_before, "RAPOR.rpt");
            rapor_ds = Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR.rpt");
            Libs.CopyFile(@"Application_Reports\Penilaian\KB\" + rapor_sc, @"Application_Reports\Penilaian\KB\" + rapor_ds);
            //end copy file rapor
        }

        protected void DoCreateSemester_TK(string rel_sekolah, string tahun_ajaran, string semester, string tahun_ajaran_before, string semester_before)
        {
            //desain rapor
            List<Application_Entities.Elearning.TK.Rapor_Design> lst_design_rapor =
                Application_DAOs.Elearning.TK.DAO_Rapor_Design.GetAll_Entity().FindAll(m => m.TahunAjaran == tahun_ajaran_before && m.Semester == semester_before);
            List<Application_Entities.Elearning.TK.Rapor_Design> lst_design_rapor_new =
                Application_DAOs.Elearning.TK.DAO_Rapor_Design.GetAll_Entity().FindAll(m => m.TahunAjaran == tahun_ajaran && m.Semester == semester);

            foreach (var item_design_rapor in lst_design_rapor)
            {
                if (lst_design_rapor_new.FindAll(
                        m0 => m0.Rel_Kelas.ToString().ToUpper().Trim() == item_design_rapor.Rel_Kelas.ToString().ToUpper().Trim()
                    ).Count == 0)
                {
                    string kode = Guid.NewGuid().ToString();
                    Application_DAOs.Elearning.TK.DAO_Rapor_Design.Insert(new Application_Entities.Elearning.TK.Rapor_Design
                    {
                        Kode = new Guid(kode),
                        TahunAjaran = tahun_ajaran,
                        Semester = semester,
                        Rel_Kelas = item_design_rapor.Rel_Kelas,
                        IsLocked = item_design_rapor.IsLocked,
                        TipeRapor = item_design_rapor.TipeRapor
                    }, Libs.LOGGED_USER_M.UserID);

                    //desain rapor kriteria
                    List<Application_Entities.Elearning.TK.Rapor_DesignKriteria> lst_design_kriteria =
                        Application_DAOs.Elearning.TK.DAO_Rapor_DesignKriteria.GetByHeader_Entity(item_design_rapor.Kode.ToString());
                    foreach (var item_design_kriteria in lst_design_kriteria)
                    {
                        Application_DAOs.Elearning.TK.DAO_Rapor_DesignKriteria.Insert(new Application_Entities.Elearning.TK.Rapor_DesignKriteria
                        {
                            Kode = Guid.NewGuid(),
                            Rel_Rapor_Design = new Guid(kode),
                            Rel_Rapor_Kriteria = item_design_kriteria.Rel_Rapor_Kriteria,
                            Urut = item_design_kriteria.Urut
                        }, Libs.LOGGED_USER_M.UserID);
                    }
                    //end desain rapor kriteria

                    //desain rapor item ekskul
                    List<Application_Entities.Elearning.TK.Rapor_DesignDetEkskul> lst_design_det_ekskul =
                        Application_DAOs.Elearning.TK.DAO_Rapor_DesignDetEkskul.GetByHeader_Entity(item_design_rapor.Kode.ToString());
                    foreach (var item_design_kriteria in lst_design_det_ekskul)
                    {
                        Application_DAOs.Elearning.TK.DAO_Rapor_DesignDetEkskul.Insert(new Application_Entities.Elearning.TK.Rapor_DesignDetEkskul
                        {
                            Kode = Guid.NewGuid(),
                            Rel_Rapor_Design = new Guid(kode),
                            Rel_KelasDet = item_design_kriteria.Rel_KelasDet,
                            Rel_Siswa = item_design_kriteria.Rel_Siswa,
                            Poin = item_design_kriteria.Poin,
                            Rel_KomponenRapor = item_design_kriteria.Rel_KomponenRapor,
                            JenisKomponen = item_design_kriteria.JenisKomponen,
                            Urut = item_design_kriteria.Urut
                        }, Libs.LOGGED_USER_M.UserID);
                    }
                    //end desain rapor item ekskul

                    List<Application_Entities.Elearning.TK.Rapor_DesignDet> lst_design_rapor_det =
                        Application_DAOs.Elearning.TK.DAO_Rapor_DesignDet.GetByHeader_Entity(item_design_rapor.Kode.ToString());

                    foreach (var item_design_rapor_det in lst_design_rapor_det)
                    {
                        Application_DAOs.Elearning.TK.DAO_Rapor_DesignDet.InsertLengkap(new Application_Entities.Elearning.TK.Rapor_DesignDet
                        {
                            Kode = Guid.NewGuid(),
                            Rel_Rapor_Design = new Guid(kode),
                            Poin = item_design_rapor_det.Poin,
                            Rel_KomponenRapor = item_design_rapor_det.Rel_KomponenRapor,
                            JenisKomponen = item_design_rapor_det.JenisKomponen,
                            Urut = item_design_rapor_det.Urut,
                            IsNewPage = item_design_rapor_det.IsNewPage,
                            IsLockGuruKelas = item_design_rapor_det.IsLockGuruKelas
                        }, Libs.LOGGED_USER_M.UserID);
                    }
                }
            }

            //desain rapor ekskul
            List<Application_Entities.Elearning.TK.Rapor_DesignEkskul> lst_design_rapor_ekskul =
                Application_DAOs.Elearning.TK.DAO_Rapor_DesignEkskul.GetAll_Entity().FindAll(m => m.TahunAjaran == tahun_ajaran_before && m.Semester == semester_before);
            List<Application_Entities.Elearning.TK.Rapor_DesignEkskul> lst_design_rapor_ekskul_new =
                Application_DAOs.Elearning.TK.DAO_Rapor_DesignEkskul.GetAll_Entity().FindAll(m => m.TahunAjaran == tahun_ajaran && m.Semester == semester);

            foreach (var item_design_rapor in lst_design_rapor_ekskul)
            {
                if (lst_design_rapor_ekskul_new.FindAll(
                        m0 => m0.Rel_Mapel.ToString().ToUpper().Trim() == item_design_rapor.Rel_Mapel.ToString().ToUpper().Trim() &&
                              m0.Rel_Kelas.ToString().ToUpper().Trim() == item_design_rapor.Rel_Kelas.ToString().ToUpper().Trim()
                    ).Count == 0)
                {
                    string kode = Guid.NewGuid().ToString();
                    Application_DAOs.Elearning.TK.DAO_Rapor_DesignEkskul.Insert(new Application_Entities.Elearning.TK.Rapor_DesignEkskul
                    {
                        Kode = new Guid(kode),
                        TahunAjaran = tahun_ajaran,
                        Semester = semester,
                        Rel_Mapel = item_design_rapor.Rel_Mapel,
                        Rel_Kelas = item_design_rapor.Rel_Kelas
                    }, Libs.LOGGED_USER_M.UserID);

                    List<Application_Entities.Elearning.TK.Rapor_DesignEkskulDet> lst_design_rapor_det =
                        Application_DAOs.Elearning.TK.DAO_Rapor_DesignEkskulDet.GetByHeader_Entity(item_design_rapor.Kode.ToString());

                    foreach (var item_design_rapor_det in lst_design_rapor_det)
                    {
                        Application_DAOs.Elearning.TK.DAO_Rapor_DesignEkskulDet.InsertLengkap(new Application_Entities.Elearning.TK.Rapor_DesignEkskulDet
                        {
                            Kode = Guid.NewGuid(),
                            Rel_Rapor_DesignEkskul = new Guid(kode),
                            Poin = item_design_rapor_det.Poin,
                            Rel_KomponenRapor = item_design_rapor_det.Rel_KomponenRapor,
                            JenisKomponen = item_design_rapor_det.JenisKomponen,
                            Urut = item_design_rapor_det.Urut
                        }, Libs.LOGGED_USER_M.UserID);
                    }
                }
            }

            //copy file rapor LTS
            string rapor_sc = Libs.GetNamaPeriodeReportRapor(tahun_ajaran_before, semester_before, "LTS.rpt");
            string rapor_ds = Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "LTS.rpt");
            Libs.CopyFile(@"Application_Reports\Penilaian\TK\" + rapor_sc, @"Application_Reports\Penilaian\TK\" + rapor_ds);
            //end copy file rapor

            //copy file rapor SEMESTER
            rapor_sc = Libs.GetNamaPeriodeReportRapor(tahun_ajaran_before, semester_before, "RAPOR.rpt");
            rapor_ds = Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR.rpt");
            Libs.CopyFile(@"Application_Reports\Penilaian\TK\" + rapor_sc, @"Application_Reports\Penilaian\TK\" + rapor_ds);
            //end copy file rapor
        }

        protected void DoCreateSemester_SD(string rel_sekolah, string tahun_ajaran, string semester, string tahun_ajaran_before, string semester_before)
        {
            //struktur penilaian
            List<AI_ERP.Application_Entities.Elearning.SD.Rapor_StrukturNilai> lst_sn = 
                AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_StrukturNilai.GetAllByTABySM_Entity(tahun_ajaran_before, semester_before);
            List<AI_ERP.Application_Entities.Elearning.SD.Rapor_StrukturNilai> lst_sn_new =
                AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_StrukturNilai.GetAllByTABySM_Entity(tahun_ajaran, semester);

            foreach (var item_sn in lst_sn)
            {
                string s_mapel = "";
                Mapel m_mapel = DAO_Mapel.GetByID_Entity(item_sn.Rel_Mapel.ToString());
                if (m_mapel != null)
                {
                    if (m_mapel.Nama != null)
                    {
                        s_mapel = m_mapel.Nama;
                        if (s_mapel.Trim() != "")
                        {
                            if (lst_sn_new.FindAll(
                                    m0 => m0.Rel_Kelas.ToString().ToUpper().Trim() == item_sn.Rel_Kelas.ToString().ToUpper().Trim() &&
                                          m0.Rel_Kelas2.ToString().ToUpper().Trim() == item_sn.Rel_Kelas2.ToString().ToUpper().Trim() &&
                                          m0.Rel_Kelas3.ToString().ToUpper().Trim() == item_sn.Rel_Kelas3.ToString().ToUpper().Trim() &&
                                          m0.Rel_Kelas4.ToString().ToUpper().Trim() == item_sn.Rel_Kelas4.ToString().ToUpper().Trim() &&
                                          m0.Rel_Kelas5.ToString().ToUpper().Trim() == item_sn.Rel_Kelas5.ToString().ToUpper().Trim() &&
                                          m0.Rel_Kelas6.ToString().ToUpper().Trim() == item_sn.Rel_Kelas6.ToString().ToUpper().Trim()
                                ).Count == 0)
                            {
                                string kode_sn = Guid.NewGuid().ToString();
                                //struktur KTSP
                                AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_StrukturNilai.Insert(new AI_ERP.Application_Entities.Elearning.SD.Rapor_StrukturNilai
                                {
                                    Kode = new Guid(kode_sn),
                                    TahunAjaran = tahun_ajaran,
                                    Semester = semester,
                                    Rel_Kelas = item_sn.Rel_Kelas,
                                    Rel_Kelas2 = item_sn.Rel_Kelas2,
                                    Rel_Kelas3 = item_sn.Rel_Kelas3,
                                    Rel_Kelas4 = item_sn.Rel_Kelas4,
                                    Rel_Kelas5 = item_sn.Rel_Kelas5,
                                    Rel_Kelas6 = item_sn.Rel_Kelas6,
                                    Rel_Mapel = item_sn.Rel_Mapel,
                                    KKM = item_sn.KKM,
                                    JenisPerhitungan = item_sn.JenisPerhitungan,
                                    IsKelompokanKP = item_sn.IsKelompokanKP,
                                    IsKelompokanKPNoLTS = item_sn.IsKelompokanKPNoLTS
                                }, Libs.LOGGED_USER_M.UserID);

                                List<AI_ERP.Application_Entities.Elearning.SD.Rapor_StrukturNilai_AP> lst_sn_ap =
                                    AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_StrukturNilai_AP.GetAllByHeader_Entity(item_sn.Kode.ToString());
                                foreach (var item_ap in lst_sn_ap)
                                {
                                    string kode_ap = Guid.NewGuid().ToString();
                                    AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_StrukturNilai_AP.Insert(new AI_ERP.Application_Entities.Elearning.SD.Rapor_StrukturNilai_AP
                                    {
                                        Kode = new Guid(kode_ap),
                                        Rel_Rapor_StrukturNilai = new Guid(kode_sn),
                                        Poin = item_ap.Poin,
                                        Rel_Rapor_AspekPenilaian = item_ap.Rel_Rapor_AspekPenilaian,
                                        JenisPerhitungan = item_ap.JenisPerhitungan,
                                        BobotRapor = item_ap.BobotRapor,
                                        IsAdaPAT_UKK = item_ap.IsAdaPAT_UKK,
                                        Bobot_PAT_UKK = item_ap.Bobot_PAT_UKK,
                                        Bobot_Non_PAT_UKK = item_ap.Bobot_Non_PAT_UKK,
                                        Urutan = item_ap.Urutan
                                    }, Libs.LOGGED_USER_M.UserID);

                                    List<AI_ERP.Application_Entities.Elearning.SD.Rapor_StrukturNilai_KD> lst_sn_kd =
                                        AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_StrukturNilai_KD.GetAllByHeader_Entity(item_ap.Kode.ToString());
                                    foreach (var item_kd in lst_sn_kd)
                                    {
                                        string kode_kd = Guid.NewGuid().ToString();
                                        AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_StrukturNilai_KD.Insert(new AI_ERP.Application_Entities.Elearning.SD.Rapor_StrukturNilai_KD
                                        {
                                            Kode = new Guid(kode_kd),
                                            Rel_Rapor_StrukturNilai_AP = new Guid(kode_ap),
                                            Rel_Rapor_KompetensiDasar = item_kd.Rel_Rapor_KompetensiDasar,
                                            JenisPerhitungan = item_kd.JenisPerhitungan,
                                            BobotAP = item_kd.BobotAP,
                                            Poin = item_kd.Poin,
                                            Urutan = item_kd.Urutan,
                                            Deskripsi = item_kd.Deskripsi
                                        }, Libs.LOGGED_USER_M.UserID);

                                        List<AI_ERP.Application_Entities.Elearning.SD.Rapor_StrukturNilai_KP> lst_sn_kp =
                                            AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(item_kd.Kode.ToString());
                                        foreach (var item_kp in lst_sn_kp)
                                        {
                                            AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_StrukturNilai_KP.Insert(new AI_ERP.Application_Entities.Elearning.SD.Rapor_StrukturNilai_KP
                                            {
                                                Kode = Guid.NewGuid(),
                                                Rel_Rapor_StrukturNilai_KD = new Guid(kode_kd),
                                                Rel_Rapor_KomponenPenilaian = item_kp.Rel_Rapor_KomponenPenilaian,
                                                Jenis = item_kp.Jenis,
                                                BobotNK = item_kp.BobotNK,
                                                Urutan = item_kp.Urutan
                                            }, Libs.LOGGED_USER_M.UserID);
                                        }

                                    }
                                }

                                List<AI_ERP.Application_Entities.Elearning.SD.Rapor_StrukturNilai_KPKelompok> lst_sn_kp_kelompok =
                                    AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_StrukturNilai_KPKelompok.GetAllByHeader_Entity(item_sn.Kode.ToString());
                                foreach (var item_sn_kelompok in lst_sn_kp_kelompok)
                                {
                                    AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_StrukturNilai_KPKelompok.Insert(new AI_ERP.Application_Entities.Elearning.SD.Rapor_StrukturNilai_KPKelompok
                                    {
                                        Kode = Guid.NewGuid(),
                                        Rel_Rapor_StrukturNilai = new Guid(kode_sn),
                                        Rel_Rapor_KomponenPenilaian = item_sn_kelompok.Rel_Rapor_KomponenPenilaian,
                                        Bobot = item_sn_kelompok.Bobot,
                                        Urutan = item_sn_kelompok.Urutan
                                    });
                                }
                                //end struktur KTSP

                                //struktur KURTILAS
                                List<AI_ERP.Application_Entities.Elearning.SD.Rapor_StrukturNilai_KURTILAS_AP> lst_sn_ap_kurtilas =
                                    AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_StrukturNilai_KURTILAS_AP.GetAllByHeader_Entity(item_sn.Kode.ToString());
                                foreach (var item_ap in lst_sn_ap_kurtilas)
                                {
                                    string kode_ap = Guid.NewGuid().ToString();
                                    AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_StrukturNilai_KURTILAS_AP.Insert(new AI_ERP.Application_Entities.Elearning.SD.Rapor_StrukturNilai_KURTILAS_AP
                                    {
                                        Kode = new Guid(kode_ap),
                                        Rel_Rapor_StrukturNilai = new Guid(kode_sn),
                                        Poin = item_ap.Poin,
                                        Rel_Rapor_AspekPenilaian = item_ap.Rel_Rapor_AspekPenilaian,
                                        JenisPerhitungan = item_ap.JenisPerhitungan,
                                        BobotRapor = item_ap.BobotRapor,
                                        Urutan = item_ap.Urutan
                                    }, Libs.LOGGED_USER_M.UserID);

                                    List<AI_ERP.Application_Entities.Elearning.SD.Rapor_StrukturNilai_KURTILAS_KD> lst_sn_kd_kurtilas =
                                        AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_StrukturNilai_KURTILAS_KD.GetAllByHeader_Entity(item_ap.Kode.ToString());
                                    foreach (var item_kd in lst_sn_kd_kurtilas)
                                    {
                                        string kode_kd = Guid.NewGuid().ToString();
                                        AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_StrukturNilai_KURTILAS_KD.Insert(new AI_ERP.Application_Entities.Elearning.SD.Rapor_StrukturNilai_KURTILAS_KD
                                        {
                                            Kode = new Guid(kode_kd),
                                            Rel_Rapor_StrukturNilai_KURTILAS_AP = new Guid(kode_ap),
                                            Rel_Rapor_KompetensiDasar = item_kd.Rel_Rapor_KompetensiDasar,
                                            JenisPerhitungan = item_kd.JenisPerhitungan,
                                            BobotAP = item_kd.BobotAP,
                                            Poin = item_kd.Poin,
                                            Urutan = item_kd.Urutan
                                        }, Libs.LOGGED_USER_M.UserID);

                                        List<AI_ERP.Application_Entities.Elearning.SD.Rapor_StrukturNilai_KURTILAS_KP> lst_sn_kp_kurtilas =
                                            AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_StrukturNilai_KURTILAS_KP.GetAllByHeader_Entity(item_kd.Kode.ToString());
                                        foreach (var item_kp in lst_sn_kp_kurtilas)
                                        {
                                            AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_StrukturNilai_KURTILAS_KP.Insert(new AI_ERP.Application_Entities.Elearning.SD.Rapor_StrukturNilai_KURTILAS_KP
                                            {
                                                Kode = Guid.NewGuid(),
                                                Rel_Rapor_StrukturNilai_KURTILAS_KD = new Guid(kode_kd),
                                                Rel_Rapor_KomponenPenilaian = item_kp.Rel_Rapor_KomponenPenilaian,
                                                Jenis = item_kp.Jenis,
                                                BobotNK = item_kp.BobotNK,
                                                Urutan = item_kp.Urutan
                                            }, Libs.LOGGED_USER_M.UserID);
                                        }

                                    }
                                }

                                List<AI_ERP.Application_Entities.Elearning.SD.Rapor_StrukturNilai_KURTILAS_KPKelompok> lst_sn_kp_kurtilas_kelompok =
                                    AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_StrukturNilai_KURTILAS_KPKelompok.GetAllByHeader_Entity(item_sn.Kode.ToString());
                                foreach (var item_sn_kelompok in lst_sn_kp_kurtilas_kelompok)
                                {
                                    AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_StrukturNilai_KURTILAS_KPKelompok.Insert(new AI_ERP.Application_Entities.Elearning.SD.Rapor_StrukturNilai_KURTILAS_KPKelompok
                                    {
                                        Kode = Guid.NewGuid(),
                                        Rel_Rapor_StrukturNilai = new Guid(kode_sn),
                                        Rel_Rapor_KomponenPenilaian = item_sn_kelompok.Rel_Rapor_KomponenPenilaian,
                                        Bobot = item_sn_kelompok.Bobot,
                                        Urutan = item_sn_kelompok.Urutan
                                    });
                                }
                            }
                            //end struktur KURTILAS
                        }
                    }
                }
            }
            //end struktur penilaian

            //desain rapor LTS
            List<AI_ERP.Application_Entities.Elearning.SD.Rapor_Desain> lst_desain_lts =
                    AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_Desain.GetByTABySM_Entity(tahun_ajaran_before, semester_before);
            List<AI_ERP.Application_Entities.Elearning.SD.Rapor_Desain> lst_desain_lts_new =
                    AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_Desain.GetByTABySM_Entity(tahun_ajaran, semester);

            foreach (var item_desain_lts in lst_desain_lts.FindAll(m => m.JenisRapor == "LTS"))
            {
                if (lst_desain_lts_new.FindAll(
                        m0 => m0.Rel_Kelas.ToString().Trim().ToUpper() == item_desain_lts.Rel_Kelas.Trim().ToUpper() &&
                              m0.JenisRapor.Trim().ToUpper() == item_desain_lts.JenisRapor.Trim().ToUpper()
                    ).Count == 0)
                {
                    string kode_lts = Guid.NewGuid().ToString();
                    AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_Desain.Insert(new AI_ERP.Application_Entities.Elearning.SD.Rapor_Desain
                    {
                        Kode = new Guid(kode_lts),
                        TahunAjaran = tahun_ajaran,
                        Semester = semester,
                        JenisRapor = item_desain_lts.JenisRapor,
                        Rel_Kelas = item_desain_lts.Rel_Kelas
                    });

                    List<AI_ERP.Application_Entities.Elearning.SD.Rapor_Desain_Det> lst_desain_lts_det =
                        AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_Desain_Det.GetAllByHeader_Entity(item_desain_lts.Kode.ToString());
                    foreach (var item_desain_det in lst_desain_lts_det)
                    {
                        AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_Desain_Det.Insert(new AI_ERP.Application_Entities.Elearning.SD.Rapor_Desain_Det
                        {
                            Kode = Guid.NewGuid(),
                            Rel_Rapor_Desain = new Guid(kode_lts),
                            Nomor = item_desain_det.Nomor,
                            Rel_Mapel = item_desain_det.Rel_Mapel,
                            Poin = item_desain_det.Poin,
                            NamaMapelRapor = item_desain_det.NamaMapelRapor,
                            Urutan = item_desain_det.Urutan
                        });
                    }
                }
            }
            //end desain rapor LTS

            //desain rapor semester
            List<AI_ERP.Application_Entities.Elearning.SD.Rapor_Desain> lst_desain_rapor =
                    AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_Desain.GetByTABySM_Entity(tahun_ajaran_before, semester_before);
            List<AI_ERP.Application_Entities.Elearning.SD.Rapor_Desain> lst_desain_rapor_new =
                    AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_Desain.GetByTABySM_Entity(tahun_ajaran, semester);

            foreach (var item_desain_lts in lst_desain_lts.FindAll(m => m.JenisRapor == "Semester"))
            {
                if (lst_desain_rapor_new.FindAll(
                        m0 => m0.Rel_Kelas.ToString().Trim().ToUpper() == item_desain_lts.Rel_Kelas.ToString().Trim().ToUpper() &&
                              m0.JenisRapor.ToString().Trim().ToUpper() == item_desain_lts.JenisRapor.ToString().Trim().ToUpper()
                    ).Count == 0)
                {
                    string kode_lts = Guid.NewGuid().ToString();
                    AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_Desain.Insert(new AI_ERP.Application_Entities.Elearning.SD.Rapor_Desain
                    {
                        Kode = new Guid(kode_lts),
                        TahunAjaran = tahun_ajaran,
                        Semester = semester,
                        JenisRapor = item_desain_lts.JenisRapor,
                        Rel_Kelas = item_desain_lts.Rel_Kelas
                    });

                    List<AI_ERP.Application_Entities.Elearning.SD.Rapor_Desain_Det> lst_desain_det =
                        AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_Desain_Det.GetAllByHeader_Entity(item_desain_lts.Kode.ToString());
                    foreach (var item_desain_det in lst_desain_det)
                    {
                        AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_Desain_Det.Insert(new AI_ERP.Application_Entities.Elearning.SD.Rapor_Desain_Det
                        {
                            Kode = Guid.NewGuid(),
                            Rel_Rapor_Desain = new Guid(kode_lts),
                            Nomor = item_desain_det.Nomor,
                            Rel_Mapel = item_desain_det.Rel_Mapel,
                            Poin = item_desain_det.Poin,
                            Urutan = item_desain_det.Urutan
                        });
                    }
                }
            }
            //end desain rapor semester

            //proses rapor
            List<AI_ERP.Application_Entities.Elearning.SD.Rapor_Arsip> lst_proses_rapor =
                    AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_Arsip.GetAll_Entity().FindAll(m => m.TahunAjaran == tahun_ajaran_before && m.Semester == semester_before).ToList();
            List<AI_ERP.Application_Entities.Elearning.SD.Rapor_Arsip> lst_proses_rapor_new =
                    AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_Arsip.GetAll_Entity().FindAll(m => m.TahunAjaran == tahun_ajaran && m.Semester == semester).ToList();
            foreach (var item_proses_rapor in lst_proses_rapor)
            {
                if (lst_proses_rapor_new.FindAll(
                    m0 => m0.JenisRapor.Trim().ToUpper() == item_proses_rapor.JenisRapor.Trim().ToUpper()
                ).Count == 0)
                {
                    AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_Arsip.Insert(new AI_ERP.Application_Entities.Elearning.SD.Rapor_Arsip
                    {
                        Kode = Guid.NewGuid(),
                        TahunAjaran = tahun_ajaran,
                        Semester = semester,
                        TanggalRapor = (
                            semester == "1"
                            ? new DateTime(DateTime.Now.Year, 12, 1)
                            : new DateTime(DateTime.Now.Year, 5, 20)
                        ),
                        TanggalClosing = (
                            semester == "1"
                            ? new DateTime(DateTime.Now.Year, 11, 20)
                            : new DateTime(DateTime.Now.Year, 5, 10)
                        ),
                        TanggalAwalAbsen = (
                            semester == "1"
                            ? new DateTime(DateTime.Now.Year, 7, 1)
                            : new DateTime(DateTime.Now.Year, 1, 1)
                        ),
                        TanggalAkhirAbsen = (
                            semester == "1"
                            ? new DateTime(DateTime.Now.Year, 11, 20)
                            : new DateTime(DateTime.Now.Year, 5, 10)
                        ),
                        KepalaSekolah = item_proses_rapor.KepalaSekolah,
                        IsArsip = item_proses_rapor.IsArsip,
                        JenisRapor = item_proses_rapor.JenisRapor,
                        Keterangan = item_proses_rapor.Keterangan
                    });
                }
            }
            //end proses rapor

            //pengaturan volunteer
            List<AI_ERP.Application_Entities.Elearning.SD.Rapor_Volunteer_Settings> lst_volunteer_settings =
                    AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_Volunteer_Settings.GetAll_Entity().FindAll(m => m.TahunAjaran == tahun_ajaran_before && m.Semester == semester_before).ToList();
            List<AI_ERP.Application_Entities.Elearning.SD.Rapor_Volunteer_Settings> lst_volunteer_settings_new =
                    AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_Volunteer_Settings.GetAll_Entity().FindAll(m => m.TahunAjaran == tahun_ajaran && m.Semester == semester).ToList();

            foreach (var item_volunteer_settings in lst_volunteer_settings)
            {
                if (lst_volunteer_settings_new.Count == 0)
                {
                    string kode_volunteer_settings = Guid.NewGuid().ToString();
                    AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_Volunteer_Settings.Insert(new AI_ERP.Application_Entities.Elearning.SD.Rapor_Volunteer_Settings
                    {
                        Kode = new Guid(kode_volunteer_settings),
                        TahunAjaran = tahun_ajaran,
                        Semester = semester
                    }, Libs.LOGGED_USER_M.UserID);

                    List<AI_ERP.Application_Entities.Elearning.SD.Rapor_Volunteer_Settings_Det> lst_volunteer_settings_det =
                        AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_Volunteer_Settings_Det.GetByHeader_Entity(item_volunteer_settings.Kode.ToString());
                    foreach (var item_volunteer_settings_det in lst_volunteer_settings_det)
                    {
                        string kode_volunteer_settings_det = Guid.NewGuid().ToString();
                        AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_Volunteer_Settings_Det.Insert(new AI_ERP.Application_Entities.Elearning.SD.Rapor_Volunteer_Settings_Det
                        {
                            Kode = new Guid(kode_volunteer_settings_det),
                            Rel_Rapor_Volunteer_Settings = new Guid(kode_volunteer_settings),
                            Durasi = item_volunteer_settings_det.Durasi,
                            Rel_Mapel = item_volunteer_settings_det.Rel_Mapel,
                            Urutan = item_volunteer_settings_det.Urutan
                        }, Libs.LOGGED_USER_M.UserID);
                    }
                }
            }
            //end pengaturan volunteer

            //pengaturan umum
            List<AI_ERP.Application_Entities.Elearning.SD.Rapor_Pengaturan> lst_pengaturan =
                    AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_Pengaturan.GetAll_Entity().FindAll(m => m.TahunAjaran == tahun_ajaran_before && m.Semester == semester_before);
            List<AI_ERP.Application_Entities.Elearning.SD.Rapor_Pengaturan> lst_pengaturan_new =
                    AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_Pengaturan.GetAll_Entity().FindAll(m => m.TahunAjaran == tahun_ajaran && m.Semester == semester);

            foreach (var item_pengaturan in lst_pengaturan)
            {
                if (lst_pengaturan_new.Count == 0)
                {
                    AI_ERP.Application_DAOs.Elearning.SD.DAO_Rapor_Pengaturan.Insert(new AI_ERP.Application_Entities.Elearning.SD.Rapor_Pengaturan
                    {
                        Kode = Guid.NewGuid(),
                        TahunAjaran = tahun_ajaran,
                        Semester = semester,
                        KepalaSekolah = item_pengaturan.KepalaSekolah,
                        KurikulumRaporLevel1 = item_pengaturan.KurikulumRaporLevel1,
                        KurikulumRaporLevel2 = item_pengaturan.KurikulumRaporLevel2,
                        KurikulumRaporLevel3 = item_pengaturan.KurikulumRaporLevel3,
                        KurikulumRaporLevel4 = item_pengaturan.KurikulumRaporLevel4,
                        KurikulumRaporLevel5 = item_pengaturan.KurikulumRaporLevel5,
                        KurikulumRaporLevel6 = item_pengaturan.KurikulumRaporLevel6
                    });
                }
            }
            //end pengaturan umum

            //copy file rapor
            string rapor_sc = Libs.GetNamaPeriodeReportRapor(tahun_ajaran_before, semester_before, "RAPOR_KTSP.rpt");
            string rapor_ds = Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KTSP.rpt");
            Libs.CopyFile(@"Application_Reports\Penilaian\SD\" + rapor_sc, @"Application_Reports\Penilaian\SD\" + rapor_ds);

            rapor_sc = Libs.GetNamaPeriodeReportRapor(tahun_ajaran_before, semester_before, "RAPOR_KTSP_URAIAN.rpt");
            rapor_ds = Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KTSP_URAIAN.rpt");
            Libs.CopyFile(@"Application_Reports\Penilaian\SD\" + rapor_sc, @"Application_Reports\Penilaian\SD\" + rapor_ds);

            rapor_sc = Libs.GetNamaPeriodeReportRapor(tahun_ajaran_before, semester_before, "RAPOR_KURTILAS.rpt");
            rapor_ds = Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KURTILAS.rpt");
            Libs.CopyFile(@"Application_Reports\Penilaian\SD\" + rapor_sc, @"Application_Reports\Penilaian\SD\" + rapor_ds);

            rapor_sc = Libs.GetNamaPeriodeReportRapor(tahun_ajaran_before, semester_before, "RAPOR_KURTILAS_URAIAN.rpt");
            rapor_ds = Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KURTILAS_URAIAN.rpt");
            Libs.CopyFile(@"Application_Reports\Penilaian\SD\" + rapor_sc, @"Application_Reports\Penilaian\SD\" + rapor_ds);
            //end copy file rapor
        }

        protected void DoCreateSemester_SMP(string rel_sekolah, string tahun_ajaran, string semester, string tahun_ajaran_before, string semester_before)
        {
            //struktur penilaian ekskul
            if (semester == "2")
            {
                AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_StrukturNilai.CreateBukaSemesterEkskul(tahun_ajaran, semester, tahun_ajaran, "1");
            }
            else
            {
                AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_StrukturNilai.CreateBukaSemesterEkskul(tahun_ajaran, semester, tahun_ajaran_before, semester_before);

            }
            //end struktur penilaian ekskul

            //struktur penilaian
            List<AI_ERP.Application_Entities.Elearning.SMP.Rapor_StrukturNilai> lst_sn =
                AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_StrukturNilai.GetAllByTABySM_Entity(tahun_ajaran_before, semester_before);
            List<AI_ERP.Application_Entities.Elearning.SMP.Rapor_StrukturNilai> lst_sn_new =
                AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_StrukturNilai.GetAllByTABySM_Entity(tahun_ajaran, semester);

            foreach (var item_sn in lst_sn)
            {
                if (
                    DAO_Mapel.GetByID_Entity(item_sn.Rel_Mapel.ToString()).Jenis.ToUpper().Trim() != Libs.JENIS_MAPEL.EKSTRAKURIKULER.ToUpper().Trim() &&
                    DAO_Mapel.GetByID_Entity(item_sn.Rel_Mapel.ToString()).Jenis.ToUpper().Trim() != Libs.JENIS_MAPEL.EKSKUL.ToUpper().Trim()
                )
                {
                    if (lst_sn_new.FindAll(
                            m0 => m0.Rel_Mapel.ToString().ToUpper().Trim() == item_sn.Rel_Mapel.ToString().ToUpper().Trim() &&
                                  m0.Rel_Kelas.ToString().ToUpper().Trim() == item_sn.Rel_Kelas.ToString().ToUpper().Trim()
                        ).Count == 0)
                    {
                        string kode_sn = Guid.NewGuid().ToString();
                        //struktur nilai
                        AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_StrukturNilai.Insert(new Application_Entities.Elearning.SMP.Rapor_StrukturNilai
                        {
                            Kode = new Guid(kode_sn),
                            TahunAjaran = tahun_ajaran,
                            Semester = semester,
                            Rel_Kelas = item_sn.Rel_Kelas,
                            Rel_Mapel = item_sn.Rel_Mapel,
                            Rel_Kelas2 = item_sn.Rel_Kelas2,
                            Rel_Kelas3 = item_sn.Rel_Kelas3,
                            KKM = item_sn.KKM,
                            JenisPerhitungan = item_sn.JenisPerhitungan,
                            Is_PH_PTS_PAS = item_sn.Is_PH_PTS_PAS,
                            DeskripsiUmum = item_sn.DeskripsiUmum,
                            DeskripsiPengetahuan = item_sn.DeskripsiPengetahuan,
                            DeskripsiKeterampilan = item_sn.DeskripsiKeterampilan,
                            BobotPAS = item_sn.BobotPAS,
                            BobotPH = item_sn.BobotPH,
                            BobotPTS = item_sn.BobotPTS,
                            DeskripsiSikapSosial = item_sn.DeskripsiSikapSosial,
                            DeskripsiSikapSpiritual = item_sn.DeskripsiSikapSpiritual
                        }, Libs.LOGGED_USER_M.UserID);

                        List<AI_ERP.Application_Entities.Elearning.SMP.Rapor_StrukturNilai_AP> lst_sn_ap =
                        AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_StrukturNilai_AP.GetAllByHeader_Entity(item_sn.Kode.ToString());
                        foreach (var item_ap in lst_sn_ap)
                        {
                            string kode_ap = Guid.NewGuid().ToString();
                            AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_StrukturNilai_AP.InsertLengkap(new AI_ERP.Application_Entities.Elearning.SMP.Rapor_StrukturNilai_AP
                            {
                                Kode = new Guid(kode_ap),
                                Rel_Rapor_StrukturNilai = new Guid(kode_sn),
                                Poin = item_ap.Poin,
                                Rel_Rapor_AspekPenilaian = item_ap.Rel_Rapor_AspekPenilaian,
                                JenisPerhitungan = item_ap.JenisPerhitungan,
                                BobotRapor = item_ap.BobotRapor,
                                Urutan = item_ap.Urutan
                            }, Libs.LOGGED_USER_M.UserID);

                            List<AI_ERP.Application_Entities.Elearning.SMP.Rapor_StrukturNilai_KD> lst_sn_kd =
                                AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_StrukturNilai_KD.GetAllByHeader_Entity(item_ap.Kode.ToString());
                            foreach (var item_kd in lst_sn_kd)
                            {
                                string kode_kd = Guid.NewGuid().ToString();
                                AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_StrukturNilai_KD.InsertLengkap(new AI_ERP.Application_Entities.Elearning.SMP.Rapor_StrukturNilai_KD
                                {
                                    Kode = new Guid(kode_kd),
                                    Rel_Rapor_StrukturNilai_AP = new Guid(kode_ap),
                                    Rel_Rapor_KompetensiDasar = item_kd.Rel_Rapor_KompetensiDasar,
                                    JenisPerhitungan = item_kd.JenisPerhitungan,
                                    BobotAP = item_kd.BobotAP,
                                    Poin = item_kd.Poin,
                                    Urutan = item_kd.Urutan
                                }, Libs.LOGGED_USER_M.UserID);

                                List<AI_ERP.Application_Entities.Elearning.SMP.Rapor_StrukturNilai_KP> lst_sn_kp =
                                    AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(item_kd.Kode.ToString());
                                foreach (var item_kp in lst_sn_kp)
                                {
                                    string kode_kp = Guid.NewGuid().ToString();
                                    AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_StrukturNilai_KP.InsertLengkap(new AI_ERP.Application_Entities.Elearning.SMP.Rapor_StrukturNilai_KP
                                    {
                                        Kode = new Guid(kode_kp),
                                        Rel_Rapor_StrukturNilai_KD = new Guid(kode_kd),
                                        Rel_Rapor_KomponenPenilaian = item_kp.Rel_Rapor_KomponenPenilaian,
                                        Jenis = item_kp.Jenis,
                                        BobotNK = item_kp.BobotNK,
                                        Urutan = item_kp.Urutan,
                                        Materi = item_kp.Materi,
                                        Deskripsi = item_kp.Deskripsi,
                                        IsAdaPB = item_kp.IsAdaPB,
                                        KodeKD = item_kp.KodeKD,
                                        IsLTS = item_kp.IsLTS
                                    }, Libs.LOGGED_USER_M.UserID);
                                }

                            }
                        }
                    }
                    //end struktur nilai
                }
            }
            //end struktur penilaian
            
            //desain rapor LTS
            List<AI_ERP.Application_Entities.Elearning.SMP.Rapor_Desain> lst_desain_lts =
                    AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_Desain.GetByTABySM_Entity(tahun_ajaran_before, semester_before);
            List<AI_ERP.Application_Entities.Elearning.SMP.Rapor_Desain> lst_desain_lts_new =
                    AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_Desain.GetByTABySM_Entity(tahun_ajaran, semester);

            foreach (var item_desain_lts in lst_desain_lts.FindAll(m => m.JenisRapor == "LTS"))
            {
                if (lst_desain_lts_new.FindAll(
                        m0 => m0.Rel_Kelas.ToString().ToUpper().Trim() == item_desain_lts.Rel_Kelas.ToString().ToUpper().Trim() &&
                              m0.JenisRapor.ToString().ToUpper().Trim() == item_desain_lts.JenisRapor.ToString().ToUpper().Trim()
                    ).Count == 0)
                {
                    string kode_lts = Guid.NewGuid().ToString();
                    AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_Desain.Insert(new AI_ERP.Application_Entities.Elearning.SMP.Rapor_Desain
                    {
                        Kode = new Guid(kode_lts),
                        TahunAjaran = tahun_ajaran,
                        Semester = semester,
                        JenisRapor = item_desain_lts.JenisRapor,
                        Rel_Kelas = item_desain_lts.Rel_Kelas
                    });

                    List<AI_ERP.Application_Entities.Elearning.SMP.Rapor_Desain_Det> lst_desain_lts_det =
                        AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_Desain_Det.GetAllByHeader_Entity(item_desain_lts.Kode.ToString());
                    foreach (var item_desain_det in lst_desain_lts_det)
                    {
                        AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_Desain_Det.Insert(new AI_ERP.Application_Entities.Elearning.SMP.Rapor_Desain_Det
                        {
                            Kode = Guid.NewGuid(),
                            Rel_Rapor_Desain = new Guid(kode_lts),
                            Nomor = item_desain_det.Nomor,
                            Rel_Mapel = item_desain_det.Rel_Mapel,
                            Poin = item_desain_det.Poin,
                            NamaMapelRapor = item_desain_det.NamaMapelRapor,
                            Alias = item_desain_det.Alias,
                            Urutan = item_desain_det.Urutan
                        });
                    }
                }
            }
            //end desain rapor LTS

            //desain rapor semester
            List<AI_ERP.Application_Entities.Elearning.SMP.Rapor_Desain> lst_desain_rapor =
                    AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_Desain.GetByTABySM_Entity(tahun_ajaran_before, semester_before);
            List<AI_ERP.Application_Entities.Elearning.SMP.Rapor_Desain> lst_desain_rapor_new =
                    AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_Desain.GetByTABySM_Entity(tahun_ajaran, semester);

            foreach (var item_desain_rapor in lst_desain_lts.FindAll(m => m.JenisRapor == "Semester"))
            {
                if (lst_desain_rapor_new.FindAll(
                        m0 => m0.Rel_Kelas.ToString().ToUpper().Trim() == item_desain_rapor.Rel_Kelas.ToString().ToUpper().Trim() &&
                              m0.JenisRapor.ToString().ToUpper().Trim() == item_desain_rapor.JenisRapor.ToString().ToUpper().Trim()
                    ).Count == 0)
                {
                    string kode_lts = Guid.NewGuid().ToString();
                    AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_Desain.Insert(new AI_ERP.Application_Entities.Elearning.SMP.Rapor_Desain
                    {
                        Kode = new Guid(kode_lts),
                        TahunAjaran = tahun_ajaran,
                        Semester = semester,
                        JenisRapor = item_desain_rapor.JenisRapor,
                        Rel_Kelas = item_desain_rapor.Rel_Kelas
                    });

                    List<AI_ERP.Application_Entities.Elearning.SMP.Rapor_Desain_Det> lst_desain_det =
                        AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_Desain_Det.GetAllByHeader_Entity(item_desain_rapor.Kode.ToString());
                    foreach (var item_desain_det in lst_desain_det)
                    {
                        AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_Desain_Det.Insert(new AI_ERP.Application_Entities.Elearning.SMP.Rapor_Desain_Det
                        {
                            Kode = Guid.NewGuid(),
                            Rel_Rapor_Desain = new Guid(kode_lts),
                            Nomor = item_desain_det.Nomor,
                            Rel_Mapel = item_desain_det.Rel_Mapel,
                            Poin = item_desain_det.Poin,
                            Alias = item_desain_det.Alias,
                            NamaMapelRapor = item_desain_det.NamaMapelRapor,
                            Urutan = item_desain_det.Urutan
                        });
                    }
                }
            }
            //end desain rapor semester

            //proses rapor
            List<AI_ERP.Application_Entities.Elearning.SMP.Rapor_Arsip> lst_proses_rapor =
                    AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_Arsip.GetAll_Entity().FindAll(m => m.TahunAjaran == tahun_ajaran_before && m.Semester == semester_before).ToList();
            List<AI_ERP.Application_Entities.Elearning.SMP.Rapor_Arsip> lst_proses_rapor_new =
                    AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_Arsip.GetAll_Entity().FindAll(m => m.TahunAjaran == tahun_ajaran && m.Semester == semester).ToList();

            foreach (var item_proses_rapor in lst_proses_rapor)
            {
                if (lst_proses_rapor_new.FindAll(
                        m0 => m0.JenisRapor.Trim().ToUpper() == item_proses_rapor.JenisRapor.Trim().ToUpper()
                    ).Count == 0)
                {
                    AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_Arsip.Insert(new AI_ERP.Application_Entities.Elearning.SMP.Rapor_Arsip
                    {
                        Kode = Guid.NewGuid(),
                        TahunAjaran = tahun_ajaran,
                        Semester = semester,
                        TanggalRapor = (
                                semester == "1"
                                ? new DateTime(DateTime.Now.Year, 12, 1)
                                : new DateTime(DateTime.Now.Year, 5, 20)
                            ),
                        TanggalClosing = (
                                semester == "1"
                                ? new DateTime(DateTime.Now.Year, 11, 20)
                                : new DateTime(DateTime.Now.Year, 5, 10)
                            ),
                        TanggalAwalAbsen = (
                                semester == "1"
                                ? new DateTime(DateTime.Now.Year, 7, 1)
                                : new DateTime(DateTime.Now.Year, 1, 1)
                            ),
                        TanggalAkhirAbsen = (
                                semester == "1"
                                ? new DateTime(DateTime.Now.Year, 11, 20)
                                : new DateTime(DateTime.Now.Year, 5, 10)
                            ),
                        KepalaSekolah = item_proses_rapor.KepalaSekolah,
                        IsArsip = item_proses_rapor.IsArsip,
                        JenisRapor = item_proses_rapor.JenisRapor,
                        Keterangan = item_proses_rapor.Keterangan
                    });
                }
            }
            //end proses rapor

            //pengaturan umum
            List<AI_ERP.Application_Entities.Elearning.SMP.Rapor_Pengaturan> lst_pengaturan =
                    AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_Pengaturan.GetAll_Entity().FindAll(m => m.TahunAjaran == tahun_ajaran_before && m.Semester == semester_before);
            List<AI_ERP.Application_Entities.Elearning.SMP.Rapor_Pengaturan> lst_pengaturan_new =
                    AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_Pengaturan.GetAll_Entity().FindAll(m => m.TahunAjaran == tahun_ajaran && m.Semester == semester);

            foreach (var item_pengaturan in lst_pengaturan)
            {
                if (lst_pengaturan_new.Count == 0)
                {
                    AI_ERP.Application_DAOs.Elearning.SMP.DAO_Rapor_Pengaturan.Insert(new AI_ERP.Application_Entities.Elearning.SMP.Rapor_Pengaturan
                    {
                        Kode = Guid.NewGuid(),
                        TahunAjaran = tahun_ajaran,
                        Semester = semester,
                        KepalaSekolah = item_pengaturan.KepalaSekolah,
                        KurikulumRaporLevel7 = item_pengaturan.KurikulumRaporLevel7,
                        KurikulumRaporLevel8 = item_pengaturan.KurikulumRaporLevel8,
                        KurikulumRaporLevel9 = item_pengaturan.KurikulumRaporLevel9
                    });
                }
            }
            //end pengaturan umum

            //copy file rapor
            string rapor_sc = Libs.GetNamaPeriodeReportRapor(tahun_ajaran_before, semester_before, "BIODATA.rpt");
            string rapor_ds = Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "BIODATA.rpt");
            Libs.CopyFile(@"Application_Reports\Penilaian\SMP\" + rapor_sc, @"Application_Reports\Penilaian\SMP\" + rapor_ds);

            rapor_sc = Libs.GetNamaPeriodeReportRapor(tahun_ajaran_before, semester_before, "RAPOR_KTSP.rpt");
            rapor_ds = Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KTSP.rpt");
            Libs.CopyFile(@"Application_Reports\Penilaian\SMP\" + rapor_sc, @"Application_Reports\Penilaian\SMP\" + rapor_ds);

            rapor_sc = Libs.GetNamaPeriodeReportRapor(tahun_ajaran_before, semester_before, "RAPOR_KURTILAS.rpt");
            rapor_ds = Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KURTILAS.rpt");
            Libs.CopyFile(@"Application_Reports\Penilaian\SMP\" + rapor_sc, @"Application_Reports\Penilaian\SMP\" + rapor_ds);

            rapor_sc = Libs.GetNamaPeriodeReportRapor(tahun_ajaran_before, semester_before, "RAPOR_KTSP_IX.rpt");
            rapor_ds = Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KTSP_IX.rpt");
            Libs.CopyFile(@"Application_Reports\Penilaian\SMP\" + rapor_sc, @"Application_Reports\Penilaian\SMP\" + rapor_ds);

            rapor_sc = Libs.GetNamaPeriodeReportRapor(tahun_ajaran_before, semester_before, "RAPOR_KURTILAS_IX.rpt");
            rapor_ds = Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KURTILAS_IX.rpt");
            Libs.CopyFile(@"Application_Reports\Penilaian\SMP\" + rapor_sc, @"Application_Reports\Penilaian\SMP\" + rapor_ds);
            //end copy file rapor
        }

        protected void DoCreateSemester_SMA(string rel_sekolah, string tahun_ajaran, string semester, string tahun_ajaran_before, string semester_before)
        {
            //formasi ekskul
            List<AI_ERP.Application_Entities.Elearning.SMA.FormasiEkskul> lst_formasi_ekskul =
                AI_ERP.Application_DAOs.Elearning.SMA.DAO_FormasiEkskul.GetByTABySM_Entity(tahun_ajaran_before, semester_before);
            List<AI_ERP.Application_Entities.Elearning.SMA.FormasiEkskul> lst_formasi_ekskul_new =
                AI_ERP.Application_DAOs.Elearning.SMA.DAO_FormasiEkskul.GetByTABySM_Entity(tahun_ajaran, semester);

            foreach (var item_ekskul in lst_formasi_ekskul)
            {
                if (lst_formasi_ekskul_new.FindAll(
                        m0 => m0.Rel_Guru.Trim().ToUpper() == item_ekskul.Rel_Guru.Trim().ToUpper() &&
                              m0.Rel_Mapel.Trim().ToUpper() == item_ekskul.Rel_Mapel.Trim().ToUpper()
                    ).Count == 0)
                {
                    string kode_formasi = Guid.NewGuid().ToString();
                    AI_ERP.Application_DAOs.Elearning.SMA.DAO_FormasiEkskul.Insert(new Application_Entities.Elearning.SMA.FormasiEkskul {
                        Kode = new Guid(kode_formasi),
                        Rel_Guru = item_ekskul.Rel_Guru,
                        Rel_Mapel = item_ekskul.Rel_Mapel,
                        Semester = semester,
                        TahunAjaran = tahun_ajaran
                    });

                    List<AI_ERP.Application_Entities.Elearning.SMA.FormasiEkskulDet> lst_formasi_ekskul_det =
                        AI_ERP.Application_DAOs.Elearning.SMA.DAO_FormasiEkskulDet.GetByHeader_Entity(item_ekskul.Kode.ToString());
                    foreach (var item_ekskul_det in lst_formasi_ekskul_det)
                    {
                        AI_ERP.Application_DAOs.Elearning.SMA.DAO_FormasiEkskulDet.Insert(new Application_Entities.Elearning.SMA.FormasiEkskulDet {
                            Kode = Guid.NewGuid(),
                            Rel_FormasiEkskul = kode_formasi,
                            Rel_Siswa = item_ekskul_det.Rel_Siswa,                            
                            Keterangan = item_ekskul_det.Keterangan,
                            Urutan = item_ekskul_det.Urutan
                        });
                    }
                }
            }
            //end formasi ekskul

            //struktur penilaian KTSP
            List <AI_ERP.Application_Entities.Elearning.SMA.Rapor_StrukturNilai_KTSP> lst_sn =
                AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_StrukturNilai_KTSP.GetAllByTABySM_Entity(tahun_ajaran_before, semester_before);
            List<AI_ERP.Application_Entities.Elearning.SMA.Rapor_StrukturNilai_KTSP> lst_sn_new =
                AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_StrukturNilai_KTSP.GetAllByTABySM_Entity(tahun_ajaran, semester);

            foreach (var item_sn in lst_sn)
            {
                if (lst_sn_new.FindAll(
                        m0 => m0.Rel_Kelas.ToString().ToUpper().Trim() == item_sn.Rel_Kelas.ToString().ToUpper().Trim() &&
                              m0.Rel_Mapel.ToString().ToUpper().Trim() == item_sn.Rel_Mapel.ToString().ToUpper().Trim()
                    ).Count == 0)
                {
                    string kode_sn = Guid.NewGuid().ToString();
                    //struktur nilai
                    AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_StrukturNilai_KTSP.Insert(new Application_Entities.Elearning.SMA.Rapor_StrukturNilai_KTSP
                    {
                        Kode = new Guid(kode_sn),
                        TahunAjaran = tahun_ajaran,
                        Semester = semester,
                        Rel_Kelas = item_sn.Rel_Kelas,
                        Rel_Mapel = item_sn.Rel_Mapel,
                        KKM = item_sn.KKM,
                        Deskripsi = item_sn.Deskripsi,
                        IsNilaiAkhir = item_sn.IsNilaiAkhir,
                        DeskripsiSikapSosial = item_sn.DeskripsiSikapSosial,
                        DeskripsiSikapSpiritual = item_sn.DeskripsiSikapSpiritual,
                    }, Libs.LOGGED_USER_M.UserID);

                    List<AI_ERP.Application_Entities.Elearning.SMA.Rapor_StrukturNilai_KTSP_KD> lst_sn_kd =
                            AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_StrukturNilai_KTSP_KD.GetAllByHeader_Entity(item_sn.Kode.ToString());
                    foreach (var item_kd in lst_sn_kd)
                    {
                        string kode_kd = Guid.NewGuid().ToString();
                        AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_StrukturNilai_KTSP_KD.InsertLengkap(new AI_ERP.Application_Entities.Elearning.SMA.Rapor_StrukturNilai_KTSP_KD
                        {
                            Kode = new Guid(kode_kd),
                            Rel_Rapor_StrukturNilai = new Guid(kode_sn),
                            Rel_Rapor_KompetensiDasar = item_kd.Rel_Rapor_KompetensiDasar,
                            BobotRaporP = item_kd.BobotRaporP,
                            BobotRaporPPK = item_kd.BobotRaporPPK,
                            JenisPerhitungan = item_kd.JenisPerhitungan,
                            Poin = item_kd.Poin,
                            Urutan = item_kd.Urutan
                        }, Libs.LOGGED_USER_M.UserID);

                        List<AI_ERP.Application_Entities.Elearning.SMA.Rapor_StrukturNilai_KTSP_KP> lst_sn_kp =
                            AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_StrukturNilai_KTSP_KP.GetAllByHeader_Entity(item_kd.Kode.ToString());
                        foreach (var item_kp in lst_sn_kp)
                        {
                            string kode_kp = Guid.NewGuid().ToString();
                            AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_StrukturNilai_KTSP_KP.InsertLengkap(new AI_ERP.Application_Entities.Elearning.SMA.Rapor_StrukturNilai_KTSP_KP
                            {
                                Kode = new Guid(kode_kp),
                                Rel_Rapor_StrukturNilai_KTSP_KD = new Guid(kode_kd),
                                Rel_Rapor_KomponenPenilaian = item_kp.Rel_Rapor_KomponenPenilaian,
                                Jenis = item_kp.Jenis,
                                BobotNKD = item_kp.BobotNKD,
                                Urutan = item_kp.Urutan
                            }, Libs.LOGGED_USER_M.UserID);
                        }

                    }
                }
            }
            //end struktur penilaian KTSP

            //struktur penilaian KURTILAS
            List<AI_ERP.Application_Entities.Elearning.SMA.Rapor_StrukturNilai_KURTILAS> lst_sn_kurtilas =
                AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_StrukturNilai_KURTILAS.GetAllByTABySM_Entity(tahun_ajaran_before, semester_before);
            List<AI_ERP.Application_Entities.Elearning.SMA.Rapor_StrukturNilai_KURTILAS> lst_sn_kurtilas_new =
                AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_StrukturNilai_KURTILAS.GetAllByTABySM_Entity(tahun_ajaran, semester);

            foreach (var item_sn in lst_sn_kurtilas)
            {
                if (lst_sn_kurtilas_new.FindAll(
                        m0 => m0.Rel_Kelas.ToString().ToUpper().Trim() == item_sn.Rel_Kelas.ToString().ToUpper().Trim() &&
                              m0.Rel_Mapel.ToString().ToUpper().Trim() == item_sn.Rel_Mapel.ToString().ToUpper().Trim()
                    ).Count == 0)
                {
                    string kode_sn = Guid.NewGuid().ToString();
                    //struktur nilai
                    AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_StrukturNilai_KURTILAS.InsertLengkap(new Application_Entities.Elearning.SMA.Rapor_StrukturNilai_KURTILAS
                    {
                        Kode = new Guid(kode_sn),
                        TahunAjaran = tahun_ajaran,
                        Semester = semester,
                        Rel_Kelas = item_sn.Rel_Kelas,
                        Rel_Mapel = item_sn.Rel_Mapel,
                        KKM = item_sn.KKM,
                        JenisPerhitungan = item_sn.JenisPerhitungan,
                        BobotRaporPengetahuan = item_sn.BobotRaporPengetahuan,
                        BobotRaporUAS = item_sn.BobotRaporUAS,
                        Deskripsi = item_sn.Deskripsi,
                        IsNilaiAkhir = item_sn.IsNilaiAkhir,
                        DeskripsiSikapSosial = item_sn.DeskripsiSikapSosial,
                        DeskripsiSikapSpiritual = item_sn.DeskripsiSikapSpiritual
                    }, Libs.LOGGED_USER_M.UserID);

                    List<AI_ERP.Application_Entities.Elearning.SMA.Rapor_StrukturNilai_KURTILAS_AP> lst_sn_ap =
                        AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_StrukturNilai_KURTILAS_AP.GetAllByHeader_Entity(item_sn.Kode.ToString());
                    foreach (var item_ap in lst_sn_ap)
                    {
                        string kode_ap = Guid.NewGuid().ToString();
                        AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_StrukturNilai_KURTILAS_AP.InsertLengkap(new AI_ERP.Application_Entities.Elearning.SMA.Rapor_StrukturNilai_KURTILAS_AP
                        {
                            Kode = new Guid(kode_ap),
                            Rel_Rapor_StrukturNilai = new Guid(kode_sn),
                            Poin = item_ap.Poin,
                            Rel_Rapor_AspekPenilaian = item_ap.Rel_Rapor_AspekPenilaian,
                            JenisPerhitungan = item_ap.JenisPerhitungan,
                            BobotRapor = item_ap.BobotRapor,
                            Urutan = item_ap.Urutan
                        }, Libs.LOGGED_USER_M.UserID);

                        List<AI_ERP.Application_Entities.Elearning.SMA.Rapor_StrukturNilai_KURTILAS_KD> lst_sn_kd =
                            AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_StrukturNilai_KURTILAS_KD.GetAllByHeader_Entity(item_ap.Kode.ToString());
                        foreach (var item_kd in lst_sn_kd)
                        {
                            string kode_kd = Guid.NewGuid().ToString();
                            AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_StrukturNilai_KURTILAS_KD.InsertLengkap(new AI_ERP.Application_Entities.Elearning.SMA.Rapor_StrukturNilai_KURTILAS_KD
                            {
                                Kode = new Guid(kode_kd),
                                Rel_Rapor_StrukturNilai_AP = new Guid(kode_ap),
                                Rel_Rapor_KompetensiDasar = item_kd.Rel_Rapor_KompetensiDasar,
                                JenisPerhitungan = item_kd.JenisPerhitungan,
                                BobotAP = item_kd.BobotAP,
                                Poin = item_kd.Poin,
                                Urutan = item_kd.Urutan,
                                IsKomponenRapor = item_kd.IsKomponenRapor,
                                DeskripsiRapor = item_kd.DeskripsiRapor
                            }, Libs.LOGGED_USER_M.UserID);

                            List<AI_ERP.Application_Entities.Elearning.SMA.Rapor_StrukturNilai_KURTILAS_KP> lst_sn_kp =
                                AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_StrukturNilai_KURTILAS_KP.GetAllByHeader_Entity(item_kd.Kode.ToString());
                            foreach (var item_kp in lst_sn_kp)
                            {
                                string kode_kp = Guid.NewGuid().ToString();
                                AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_StrukturNilai_KURTILAS_KP.InsertLengkap(new AI_ERP.Application_Entities.Elearning.SMA.Rapor_StrukturNilai_KURTILAS_KP
                                {
                                    Kode = new Guid(kode_kp),
                                    Rel_Rapor_StrukturNilai_KD = new Guid(kode_kd),
                                    Rel_Rapor_KomponenPenilaian = item_kp.Rel_Rapor_KomponenPenilaian,                                    
                                    BobotNK = item_kp.BobotNK,
                                    Jenis = item_kp.Jenis,
                                    IsKomponenRapor = item_kp.IsKomponenRapor,
                                    Deskripsi = item_kp.Deskripsi,
                                    DeskripsiRapor = item_kp.DeskripsiRapor,                                    
                                    Urutan = item_kp.Urutan
                                }, Libs.LOGGED_USER_M.UserID);
                            }

                        }
                    }
                }
                //end struktur nilai KURTILAS
            }
            //end struktur penilaian KURTILAS

            //struktur nilai predikat KTSP
            foreach (var item_sn_ktsp in lst_sn)
            {
                var _lst_sn_ktsp_new = lst_sn_new.FindAll(
                        m0 => m0.Rel_Kelas.ToString().ToUpper().Trim() == item_sn_ktsp.Rel_Kelas.ToString().ToUpper().Trim() &&
                              m0.Rel_Mapel.ToString().ToUpper().Trim() == item_sn_ktsp.Rel_Mapel.ToString().ToUpper().Trim()
                    );

                if (_lst_sn_ktsp_new.Count == 1)
                {
                    string s_kode_header = _lst_sn_ktsp_new.FirstOrDefault().Kode.ToString();
                    var lst_sn_predikat = AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_StrukturNilai_KTSP_Predikat.GetAllByHeader_Entity(item_sn_ktsp.Kode.ToString());
                    var lst_sn_predikat_new = AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_StrukturNilai_KTSP_Predikat.GetAllByHeader_Entity(s_kode_header);
                    foreach (var item_sn_predikat in lst_sn_predikat)
                    {
                        string s_kode_item = Guid.NewGuid().ToString();
                        var lst_sn_predikat_new_ = lst_sn_predikat_new.FindAll(
                                m0 => m0.Urutan.ToString().ToUpper().Trim() == item_sn_predikat.Urutan.ToString().ToUpper().Trim()
                            );
                        if (lst_sn_predikat_new_.Count == 1)
                        {
                            s_kode_item = lst_sn_predikat_new_.FirstOrDefault().Kode.ToString();
                            AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_StrukturNilai_KTSP_Predikat.Update(new Application_Entities.Elearning.SMA.Rapor_StrukturNilai_KTSP_Predikat
                            {
                                Kode = new Guid(s_kode_item),
                                Rel_Rapor_StrukturNilai = new Guid(s_kode_header),
                                Minimal = item_sn_predikat.Minimal,
                                Maksimal = item_sn_predikat.Maksimal,
                                Predikat = item_sn_predikat.Predikat,
                                Urutan = item_sn_predikat.Urutan,
                                Deskripsi = item_sn_predikat.Deskripsi
                            }, Libs.LOGGED_USER_M.UserID);
                        }
                        else
                        {
                            AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_StrukturNilai_KTSP_Predikat.Insert(new Application_Entities.Elearning.SMA.Rapor_StrukturNilai_KTSP_Predikat
                            {
                                Kode = new Guid(s_kode_item),
                                Rel_Rapor_StrukturNilai = new Guid(s_kode_header),
                                Minimal = item_sn_predikat.Minimal,
                                Maksimal = item_sn_predikat.Maksimal,
                                Predikat = item_sn_predikat.Predikat,
                                Urutan = item_sn_predikat.Urutan,
                                Deskripsi = item_sn_predikat.Deskripsi
                            }, Libs.LOGGED_USER_M.UserID);
                        }
                    }
                }
            }
            //end struktur nilai predikat KTSP

            //struktur nilai predikat KURTILAS
            foreach (var item_sn_kurtilas in lst_sn_kurtilas)
            {
                var _lst_sn_kurtilas_new = lst_sn_kurtilas_new.FindAll(
                        m0 => m0.Rel_Kelas.ToString().ToUpper().Trim() == item_sn_kurtilas.Rel_Kelas.ToString().ToUpper().Trim() &&
                              m0.Rel_Mapel.ToString().ToUpper().Trim() == item_sn_kurtilas.Rel_Mapel.ToString().ToUpper().Trim()
                    );

                if (_lst_sn_kurtilas_new.Count == 1)
                {
                    string s_kode_header = _lst_sn_kurtilas_new.FirstOrDefault().Kode.ToString();
                    var lst_sn_predikat = AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_StrukturNilai_KURTILAS_Predikat.GetAllByHeader_Entity(item_sn_kurtilas.Kode.ToString());
                    var lst_sn_predikat_new = AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_StrukturNilai_KURTILAS_Predikat.GetAllByHeader_Entity(s_kode_header);
                    foreach (var item_sn_predikat in lst_sn_predikat)
                    {
                        string s_kode_item = Guid.NewGuid().ToString();
                        var lst_sn_predikat_new_ = lst_sn_predikat_new.FindAll(
                                m0 => m0.Urutan.ToString().ToUpper().Trim() == item_sn_predikat.Urutan.ToString().ToUpper().Trim()
                            );
                        if (lst_sn_predikat_new_.Count == 1)
                        {
                            s_kode_item = lst_sn_predikat_new_.FirstOrDefault().Kode.ToString();
                            AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_StrukturNilai_KURTILAS_Predikat.Update(new Application_Entities.Elearning.SMA.Rapor_StrukturNilai_KURTILAS_Predikat
                            {
                                Kode = new Guid(s_kode_item),
                                Rel_Rapor_StrukturNilai = new Guid(s_kode_header),
                                Minimal = item_sn_predikat.Minimal,
                                Maksimal = item_sn_predikat.Maksimal,
                                Predikat = item_sn_predikat.Predikat,
                                Urutan = item_sn_predikat.Urutan,
                                Deskripsi = item_sn_predikat.Deskripsi
                            }, Libs.LOGGED_USER_M.UserID);
                        }
                        else
                        {
                            AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_StrukturNilai_KURTILAS_Predikat.Insert(new Application_Entities.Elearning.SMA.Rapor_StrukturNilai_KURTILAS_Predikat
                            {
                                Kode = new Guid(s_kode_item),
                                Rel_Rapor_StrukturNilai = new Guid(s_kode_header),
                                Minimal = item_sn_predikat.Minimal,
                                Maksimal = item_sn_predikat.Maksimal,
                                Predikat = item_sn_predikat.Predikat,
                                Urutan = item_sn_predikat.Urutan,
                                Deskripsi = item_sn_predikat.Deskripsi
                            }, Libs.LOGGED_USER_M.UserID);
                        }
                    }
                }
            }
            //end struktur nilai predikat KURTILAS

            //desain rapor LTS
            List<AI_ERP.Application_Entities.Elearning.SMA.Rapor_Desain> lst_desain_lts =
                    AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_Desain.GetByTABySM_Entity(tahun_ajaran_before, semester_before);
            List<AI_ERP.Application_Entities.Elearning.SMA.Rapor_Desain> lst_desain_lts_new =
                    AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_Desain.GetByTABySM_Entity(tahun_ajaran, semester);

            foreach (var item_desain_lts in lst_desain_lts.FindAll(m => m.JenisRapor == "LTS"))
            {
                if (lst_desain_lts_new.FindAll(
                        m0 => m0.Rel_Kelas.ToString().ToUpper().Trim() == item_desain_lts.Rel_Kelas.ToString().ToUpper().Trim() &&
                              m0.JenisRapor.ToString().ToUpper().Trim() == item_desain_lts.JenisRapor.ToString().ToUpper().Trim()
                    ).Count == 0)
                {
                    string kode_lts = Guid.NewGuid().ToString();
                    AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_Desain.Insert(new AI_ERP.Application_Entities.Elearning.SMA.Rapor_Desain
                    {
                        Kode = new Guid(kode_lts),
                        TahunAjaran = tahun_ajaran,
                        Semester = semester,
                        JenisRapor = item_desain_lts.JenisRapor,
                        Rel_Kelas = item_desain_lts.Rel_Kelas
                    });

                    List<AI_ERP.Application_Entities.Elearning.SMA.Rapor_Desain_Det> lst_desain_lts_det =
                        AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_Desain_Det.GetAllByHeader_Entity(item_desain_lts.Kode.ToString());
                    foreach (var item_desain_det in lst_desain_lts_det)
                    {
                        AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_Desain_Det.Insert(new AI_ERP.Application_Entities.Elearning.SMA.Rapor_Desain_Det
                        {
                            Kode = Guid.NewGuid(),
                            Rel_Rapor_Desain = new Guid(kode_lts),
                            Nomor = item_desain_det.Nomor,
                            Rel_Mapel = item_desain_det.Rel_Mapel,
                            Poin = item_desain_det.Poin,
                            NamaMapelRapor = item_desain_det.NamaMapelRapor,
                            Alias = item_desain_det.Alias,
                            Urutan = item_desain_det.Urutan
                        });
                    }
                }
            }
            //end desain rapor LTS

            //desain rapor semester
            List<AI_ERP.Application_Entities.Elearning.SMA.Rapor_Desain> lst_desain_rapor =
                    AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_Desain.GetByTABySM_Entity(tahun_ajaran_before, semester_before);
            List<AI_ERP.Application_Entities.Elearning.SMA.Rapor_Desain> lst_desain_rapor_new =
                    AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_Desain.GetByTABySM_Entity(tahun_ajaran, semester);

            foreach (var item_desain_rapor in lst_desain_lts.FindAll(m => m.JenisRapor == "Semester"))
            {
                if (lst_desain_rapor_new.FindAll(
                        m0 => m0.Rel_Kelas.ToString().ToUpper().Trim() == item_desain_rapor.Rel_Kelas.ToString().ToUpper().Trim() &&
                              m0.JenisRapor.ToString().ToUpper().Trim() == item_desain_rapor.JenisRapor.ToString().ToUpper().Trim()
                    ).Count == 0)
                {
                    string kode_lts = Guid.NewGuid().ToString();
                    AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_Desain.Insert(new AI_ERP.Application_Entities.Elearning.SMA.Rapor_Desain
                    {
                        Kode = new Guid(kode_lts),
                        TahunAjaran = tahun_ajaran,
                        Semester = semester,
                        JenisRapor = item_desain_rapor.JenisRapor,
                        Rel_Kelas = item_desain_rapor.Rel_Kelas
                    });

                    List<AI_ERP.Application_Entities.Elearning.SMA.Rapor_Desain_Det> lst_desain_det =
                        AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_Desain_Det.GetAllByHeader_Entity(item_desain_rapor.Kode.ToString());
                    foreach (var item_desain_det in lst_desain_det)
                    {
                        AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_Desain_Det.Insert(new AI_ERP.Application_Entities.Elearning.SMA.Rapor_Desain_Det
                        {
                            Kode = Guid.NewGuid(),
                            Rel_Rapor_Desain = new Guid(kode_lts),
                            Nomor = item_desain_det.Nomor,
                            Rel_Mapel = item_desain_det.Rel_Mapel,
                            Poin = item_desain_det.Poin,
                            Alias = item_desain_det.Alias,
                            NamaMapelRapor = item_desain_det.NamaMapelRapor,
                            Urutan = item_desain_det.Urutan
                        });
                    }
                }
            }
            //end desain rapor semester

            //proses rapor
            List<AI_ERP.Application_Entities.Elearning.SMA.Rapor_Arsip> lst_proses_rapor =
                    AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_Arsip.GetAll_Entity().FindAll(m => m.TahunAjaran == tahun_ajaran_before && m.Semester == semester_before).ToList();
            List<AI_ERP.Application_Entities.Elearning.SMA.Rapor_Arsip> lst_proses_rapor_new =
                    AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_Arsip.GetAll_Entity().FindAll(m => m.TahunAjaran == tahun_ajaran && m.Semester == semester).ToList();

            foreach (var item_proses_rapor in lst_proses_rapor)
            {
                if (lst_proses_rapor_new.FindAll(
                        m0 => m0.JenisRapor.Trim().ToUpper() == item_proses_rapor.JenisRapor.Trim().ToUpper()
                    ).Count == 0)
                {
                    AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_Arsip.Insert(new AI_ERP.Application_Entities.Elearning.SMA.Rapor_Arsip
                    {
                        Kode = Guid.NewGuid(),
                        TahunAjaran = tahun_ajaran,
                        Semester = semester,
                        TanggalRapor = (
                                semester == "1"
                                ? new DateTime(DateTime.Now.Year, 12, 1)
                                : new DateTime(DateTime.Now.Year, 5, 20)
                            ),
                        TanggalClosing = (
                                semester == "1"
                                ? new DateTime(DateTime.Now.Year, 11, 20)
                                : new DateTime(DateTime.Now.Year, 5, 10)
                            ),
                        TanggalAwalAbsen = (
                                semester == "1"
                                ? new DateTime(DateTime.Now.Year, 7, 1)
                                : new DateTime(DateTime.Now.Year, 1, 1)
                            ),
                        TanggalAkhirAbsen = (
                                semester == "1"
                                ? new DateTime(DateTime.Now.Year, 11, 20)
                                : new DateTime(DateTime.Now.Year, 5, 10)
                            ),
                        KepalaSekolah = item_proses_rapor.KepalaSekolah,
                        IsArsip = item_proses_rapor.IsArsip,
                        JenisRapor = item_proses_rapor.JenisRapor,
                        Keterangan = item_proses_rapor.Keterangan
                    });
                }
            }
            //end proses rapor

            //pengaturan umum
            List<AI_ERP.Application_Entities.Elearning.SMA.Rapor_Pengaturan> lst_pengaturan =
                    AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_Pengaturan.GetAll_Entity().FindAll(m => m.TahunAjaran == tahun_ajaran_before && m.Semester == semester_before);
            List<AI_ERP.Application_Entities.Elearning.SMA.Rapor_Pengaturan> lst_pengaturan_new =
                    AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_Pengaturan.GetAll_Entity().FindAll(m => m.TahunAjaran == tahun_ajaran && m.Semester == semester);

            foreach (var item_pengaturan in lst_pengaturan)
            {
                if (lst_pengaturan_new.Count == 0)
                {
                    AI_ERP.Application_DAOs.Elearning.SMA.DAO_Rapor_Pengaturan.Insert(new AI_ERP.Application_Entities.Elearning.SMA.Rapor_Pengaturan
                    {
                        Kode = Guid.NewGuid(),
                        TahunAjaran = tahun_ajaran,
                        Semester = semester,
                        KepalaSekolah = item_pengaturan.KepalaSekolah,                        
                        KurikulumRaporLevel10 = item_pengaturan.KurikulumRaporLevel10,
                        KurikulumRaporLevel11 = item_pengaturan.KurikulumRaporLevel11,
                        KurikulumRaporLevel12 = item_pengaturan.KurikulumRaporLevel12
                    });
                }
            }
            //end pengaturan umum

            //copy file rapor
            string rapor_sc = Libs.GetNamaPeriodeReportRapor(tahun_ajaran_before, semester_before, "BIODATA.rpt");
            string rapor_ds = Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "BIODATA.rpt");
            Libs.CopyFile(@"Application_Reports\Penilaian\SMA\" + rapor_sc, @"Application_Reports\Penilaian\SMA\" + rapor_ds);

            rapor_sc = Libs.GetNamaPeriodeReportRapor(tahun_ajaran_before, semester_before, "RAPOR_KTSP.rpt");
            rapor_ds = Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KTSP.rpt");
            Libs.CopyFile(@"Application_Reports\Penilaian\SMA\" + rapor_sc, @"Application_Reports\Penilaian\SMA\" + rapor_ds);

            rapor_sc = Libs.GetNamaPeriodeReportRapor(tahun_ajaran_before, semester_before, "RAPOR_KURTILAS.rpt");
            rapor_ds = Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KURTILAS.rpt");
            Libs.CopyFile(@"Application_Reports\Penilaian\SMA\" + rapor_sc, @"Application_Reports\Penilaian\SMA\" + rapor_ds);

            //rapor_sc = Libs.GetNamaPeriodeReportRapor(tahun_ajaran_before, semester_before, "RAPOR_KTSP_XII.rpt");
            //rapor_ds = Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KTSP_XII.rpt");
            //Libs.CopyFile(@"Application_Reports\Penilaian\SMA\" + rapor_sc, @"Application_Reports\Penilaian\SMA\" + rapor_ds);

            rapor_sc = Libs.GetNamaPeriodeReportRapor(tahun_ajaran_before, semester_before, "RAPOR_KURTILAS_X.rpt");
            rapor_ds = Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KURTILAS_X.rpt");
            Libs.CopyFile(@"Application_Reports\Penilaian\SMA\" + rapor_sc, @"Application_Reports\Penilaian\SMA\" + rapor_ds);

            rapor_sc = Libs.GetNamaPeriodeReportRapor(tahun_ajaran_before, semester_before, "RAPOR_KURTILAS_XI.rpt");
            rapor_ds = Libs.GetNamaPeriodeReportRapor(tahun_ajaran, semester, "RAPOR_KURTILAS_XI.rpt");
            Libs.CopyFile(@"Application_Reports\Penilaian\SMA\" + rapor_sc, @"Application_Reports\Penilaian\SMA\" + rapor_ds);
            //end copy file rapor
        }

        protected void DoCreateSemester()
        {
            string message = "Proses buka semester selesai.";

            try
            {
                string tahun = Libs.GetQueryString("t");
                string semester = Libs.GetQueryString("s");
                string rel_sekolah = Libs.GetQueryString("u");
                string tahun_sebelumnya = Libs.GetQueryString("tc");
                string semester_sebelumnya = Libs.GetQueryString("sc");

                //jika berhasil create semester
                if (DAO_BukaSemester.GetAll_Entity().FindAll(
                        m => m.TahunAjaran == tahun &&
                             m.Semester == semester &&
                             m.Rel_Sekolah == rel_sekolah
                    ).Count == 0)
                {
                    DAO_BukaSemester.Insert(new BukaSemester
                    {
                        Kode = Guid.NewGuid(),
                        Rel_Sekolah = rel_sekolah,
                        TahunAjaran = tahun,
                        Semester = semester,
                        Tanggal = DateTime.Now,
                        UserID = Libs.LOGGED_USER_M.UserID
                    });

                    DoCreateSemesterUnit(rel_sekolah, tahun, semester, tahun_sebelumnya, semester_sebelumnya);
                    Response.Redirect(ResolveUrl(Routing.URL.APPLIACTION_MODULES.LOADER.BUKA_SEMESTER_INFO.ROUTE + "?m=" + Regex.Replace(message, @"\t|\n|\r", " ")), false);
                }
                else
                {
                    message = "Pengaturan untuk tahun pelajaran " +
                              tahun +
                              " semester " +
                              semester +
                              " " +
                              "sudah dibuat.";
                    Response.Redirect(ResolveUrl(Routing.URL.APPLIACTION_MODULES.LOADER.BUKA_SEMESTER_ERROR.ROUTE + "?em=" + Regex.Replace(message, @"\t|\n|\r", " ")), false);
                }
                //end jika berhasil create semester
            }
            catch (Exception ex)
            {
                message = "ERROR : " + ex.Message;
                Response.Redirect(ResolveUrl(Routing.URL.APPLIACTION_MODULES.LOADER.BUKA_SEMESTER_ERROR.ROUTE + "?em=" + Regex.Replace(message, @"\t|\n|\r", " ")), false);
            }
        }
    }
}