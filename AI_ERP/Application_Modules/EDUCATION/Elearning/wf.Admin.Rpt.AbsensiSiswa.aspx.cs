using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_Entities.Elearning;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_DAOs.Elearning;

namespace AI_ERP.Application_Modules.EDUCATION.Elearning
{
    public partial class wf_Admin_Rpt_AbsensiSiswa : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGERPTABSENSISISWA";
        public enum JenisAction
        {
            DoDownloadRekapAbsensi
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }
            else
            {
                if (Libs.GetQueryString("token") != "8kfdsjhfsdf2fsdf234fdf" && Libs.GetQueryString("unit").Trim() == "")
                {
                    Libs.RedirectToBeranda(this.Page);
                }
            }

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" + ResolveUrl("~/Application_CLibs/images/svg/circular-graphic.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Rekapitulasi Presensi Murid & Kedisiplinan";

            this.Master.ShowHeaderTools = false;
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {
                txtTglAbsenPerHari.Text = Libs.GetTanggalIndonesiaFromDate(DateTime.Now, false);
                txtTglAbsenPerPeriode_Dari.Text = Libs.GetTanggalIndonesiaFromDate(DateTime.Now, false);
                txtTglAbsenPerPeriode_Sampai.Text = Libs.GetTanggalIndonesiaFromDate(DateTime.Now.AddDays(7), false);

                txtFilter_Unit.Value = "";
                txtFilter_UnitVal.Value = "";
                txtFilter_Kelas.Value = txtFilter_Unit.Value;
                txtFilter_KelasVal.Value = "";
                txtFilter_Mapel.Value = txtFilter_Unit.Value;
                txtFilter_MapelVal.Value = "";
                txtFilter_Guru.Value = txtFilter_Unit.Value;
                txtFilter_GuruVal.Value = "";
                txtFilter_Siswa1.Value = txtFilter_Unit.Value;
                txtFilter_Siswa1Val.Value = "";

                if (Libs.LOGGED_USER_M.UserID == "ira.intasari" ||
                    Libs.LOGGED_USER_M.UserID == "diajeng.ayu" ||
                    Libs.LOGGED_USER_M.UserID == "madinah" ||

                    Libs.LOGGED_USER_M.UserID == "naily.tanjung" ||
                    Libs.LOGGED_USER_M.UserID == "maryamah" ||
                    Libs.LOGGED_USER_M.UserID == "wiwid.fajarianto" ||

                    Libs.LOGGED_USER_M.UserID == "lulu" ||
                    Libs.LOGGED_USER_M.UserID == "endang.purwaningsih" ||
                    Libs.LOGGED_USER_M.UserID == "dian.safarulloh" ||
                    Libs.LOGGED_USER_M.UserID == "agus.riyadi" ||

                    Libs.LOGGED_USER_M.UserID == "hadi.saputro" ||
                    Libs.LOGGED_USER_M.UserID == "gugum.prayoga" ||
                    Libs.LOGGED_USER_M.UserID == "gina.zahra" ||
                    Libs.LOGGED_USER_M.UserID == "wuri.andayani" ||

                    Libs.LOGGED_USER_M.UserID == "muh.ridwan" ||
                    Libs.LOGGED_USER_M.UserID == "maruto.santoso" ||
                    Libs.LOGGED_USER_M.UserID == "diah.sulistiowati" ||
                    Libs.LOGGED_USER_M.UserID == "leni.mulyani")
                {
                    rdoGroupByMapel.Visible = false;
                    rdoGroupByGuru.Visible = false;
                    rdoGroupBySiswa.Visible = false;
                    rdoDetailAbsensi.Visible = false;

                    div_by_mapel.Attributes["style"] = "display: none;";
                    div_by_guru.Attributes["style"] = "display: none;";
                    div_by_siswa.Attributes["style"] = "display: none;";
                    div_by_detail_absensi.Attributes["style"] = "display: none;";

                    rdoGroupByMapelBySiswa.Checked = true;
                }
                else
                {
                    rdoGroupByMapel.Checked = true;
                }

                ListPresensiDanKedisiplinan();
                InitFilter();
                InitClientInput();
                InitEnabledFilter();

                rdoGroupByUnit.Attributes["onclick"] = btnInitEnabledFilter.ClientID + ".click();";
                rdoGroupByKelas.Attributes["onclick"] = btnInitEnabledFilter.ClientID + ".click();";
                rdoGroupByMapel.Attributes["onclick"] = btnInitEnabledFilter.ClientID + ".click();";
                rdoGroupByGuru.Attributes["onclick"] = btnInitEnabledFilter.ClientID + ".click();";
                rdoGroupBySiswa.Attributes["onclick"] = btnInitEnabledFilter.ClientID + ".click();";
                rdoGroupByMapelBySiswa.Attributes["onclick"] = btnInitEnabledFilter.ClientID + ".click();";
                rdoDetailAbsensi.Attributes["onclick"] = btnInitEnabledFilter.ClientID + ".click();";
            }
        }

        protected void InitClientInput()
        {
            txtFilter_Unit.Attributes.Add(
                    "onchange",
                    "document.getElementById('" + txtFilter_UnitVal.ClientID + "').value = GetSelectedDataList('dl_unit', this.id);" +
                    "document.getElementById('" + txtFilter_Kelas.ClientID + "').value = '';" +
                    "document.getElementById('" + txtFilter_KelasVal.ClientID + "').value = '';" +
                    "document.getElementById('" + txtFilter_Mapel.ClientID + "').value = '';" +
                    "document.getElementById('" + txtFilter_MapelVal.ClientID + "').value = '';" +
                    "ListDropDown();"
                );
            txtFilter_Kelas.Attributes.Add(
                    "onchange",
                    "document.getElementById('" + txtFilter_KelasVal.ClientID + "').value = GetSelectedDataList('dl_kelas', this.id);" +
                    "document.getElementById('" + txtFilter_Siswa1.ClientID + "').value = '';" +
                    "document.getElementById('" + txtFilter_Siswa1Val.ClientID + "').value = '';" +
                    "ListDropDown();"
                );
            txtFilter_Mapel.Attributes.Add(
                    "onchange",
                    "document.getElementById('" + txtFilter_MapelVal.ClientID + "').value = GetSelectedDataList('dl_mapel', this.id);" +
                    "ListDropDown();"
                );
            txtFilter_Siswa1.Attributes.Add(
                    "onchange",
                    "document.getElementById('" + txtFilter_Siswa1Val.ClientID + "').value = GetSelectedDataList('dl_siswa_1', this.id);" +
                    "ListDropDown();"
                );
            txtFilter_Guru.Attributes.Add(
                    "onchange",
                    "document.getElementById('" + txtFilter_GuruVal.ClientID + "').value = GetSelectedDataList('dl_guru', this.id);" +
                    "ListDropDown();"
                );
        }

        protected void InitFilter()
        {
            string html = "";
            string s_unit = Libs.GetQueryString("unit").Trim().ToUpper();
            string s_kelas = Libs.GetQueryString("kd").Trim().ToUpper();
            string s_mapel = Libs.GetQueryString("m").Trim().ToUpper();
            string tahun_ajaran = (
                    rdoPilihHari.Checked
                    ? Libs.GetTahunAjaranByTanggal(Libs.GetDateFromTanggalIndonesiaStr(txtTglAbsenPerHari.Text))
                    : Libs.GetTahunAjaranByTanggal(Libs.GetDateFromTanggalIndonesiaStr(txtTglAbsenPerPeriode_Dari.Text))
                );
            string semester = (
                    rdoPilihHari.Checked
                    ? Libs.GetSemesterByTanggal(Libs.GetDateFromTanggalIndonesiaStr(txtTglAbsenPerHari.Text)).ToString()
                    : Libs.GetSemesterByTanggal(Libs.GetDateFromTanggalIndonesiaStr(txtTglAbsenPerPeriode_Dari.Text)).ToString()
                );
           
            //unit sekolah
            List<Sekolah> lst_sekolah = DAO_Sekolah.GetAll_Entity().OrderBy(m0 => m0.UrutanJenjang).ToList();
            if (s_unit != "")
            {
                lst_sekolah = lst_sekolah.FindAll(
                    m0 => m0.Kode.ToString().Trim().ToUpper() == s_unit).ToList();

                txtFilter_UnitVal.Value = s_unit;
                txtFilter_Unit.Value = lst_sekolah.FirstOrDefault().Nama;
                txtFilter_Unit.Attributes["disabled"] = "disabled";
            }

            if (txtFilter_UnitVal.Value.Trim() != "")
            {
                s_unit = txtFilter_UnitVal.Value.Trim();
            }
            List<FormasiGuruKelas> lst_formasi_guru_kelas = new List<FormasiGuruKelas>();
            List<FormasiGuruMapel> lst_formasi_guru_mapel = new List<FormasiGuruMapel>();

            if (s_unit.Trim() != "")
            {
                lst_formasi_guru_kelas = DAO_FormasiGuruKelas.GetByUnitByTABySM_Entity(
                       s_unit,
                       tahun_ajaran,
                       semester
                   );

                lst_formasi_guru_mapel = DAO_FormasiGuruMapel.GetByUnitByTABySM_Entity(
                    s_unit,
                    tahun_ajaran,
                    semester
                );
            }
            
            html += "<datalist id=\"dl_unit\">";
            html += "<option selected data-value=\"\" value=\"\"></option>";
            foreach (Sekolah m in lst_sekolah)
            {
                html += "<option " + (m.Kode.ToString().ToUpper() == s_unit.Trim().ToUpper() ? "selected" : "") + " data-value=\"" + m.Kode.ToString() + "\" " + 
                                (s_unit == m.Kode.ToString().Trim().ToUpper() ? " selected " : "") + ">" + m.Nama + "</option>";
            }
            html += "</datalist>";

            //kelas
            html += "<datalist id=\"dl_kelas\">";
            html += "<option selected data-value=\"\" value=\"\"></option>";
            if (s_unit.Trim() != "")
            {
                if (s_kelas.Trim() != "")
                {
                    lst_formasi_guru_kelas = lst_formasi_guru_kelas.FindAll(m0 => m0.Rel_KelasDet.ToUpper().Trim() == s_unit.ToUpper().Trim());

                    txtFilter_KelasVal.Value = s_kelas;
                    txtFilter_Kelas.Value = DAO_KelasDet.GetByID_Entity(s_kelas).Nama;
                    txtFilter_Kelas.Attributes["disabled"] = "disabled";
                }
                else
                {
                    foreach (var item in lst_formasi_guru_kelas.Select(m0 => m0.Rel_KelasDet).Distinct().ToList())
                    {
                        KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(item);
                        if (m_kelas_det != null)
                        {
                            if (m_kelas_det.Nama != null)
                            {
                                html += "<option data-value=\"" + m_kelas_det.Kode.ToString() + "\">" + m_kelas_det.Nama + "</option>";
                            }
                        }
                    }
                }
            }
            html += "</datalist>";

            //mata pelajaran
            html += "<datalist id=\"dl_mapel\">";
            if (s_unit.Trim() != "")
            {
                List<Mapel> lst_mapel = DAO_Mapel.GetDistinctByTABySMBySekolah_Entity(tahun_ajaran, semester, s_unit).OrderBy(m0 => m0.Nama).ToList();
                html += "<option selected data-value=\"\" value=\"\"></option>";
                if (s_unit.Trim() != "")
                {
                    if (s_mapel.Trim() != "")
                    {
                        txtFilter_MapelVal.Value = s_mapel;
                        txtFilter_Mapel.Value = DAO_Mapel.GetByID_Entity(s_mapel).Nama;
                        txtFilter_Mapel.Attributes["disabled"] = "disabled";
                    }
                    else
                    {
                        foreach (Mapel m_mapel in lst_mapel)
                        {
                            html += "<option data-value=\"" + m_mapel.Kode.ToString() + "\">" + m_mapel.Nama + "</option>";
                        }
                    }
                }
            }
            html += "</datalist>";

            //guru
            List<string> lst_nip = new List<string>();
            foreach (var item in lst_formasi_guru_kelas)
            {
                if (lst_nip.FindAll(m => m == item.Rel_GuruKelas).Count == 0)
                {
                    lst_nip.Add(item.Rel_GuruKelas);
                }
                if (lst_nip.FindAll(m => m == item.Rel_GuruPendamping).Count == 0)
                {
                    lst_nip.Add(item.Rel_GuruPendamping);
                }
            }
            foreach (var item in lst_formasi_guru_mapel)
            {
                foreach (var item_det in DAO_FormasiGuruMapelDet.GetByHeader_Entity(item.Kode.ToString()))
                {
                    if (lst_nip.FindAll(m => m == item_det.Rel_Guru).Count == 0)
                    {
                        lst_nip.Add(item_det.Rel_Guru);
                    }
                }
            }

            Dictionary<string, string> lst_pegawai = new Dictionary<string, string>();
            if (s_unit.Trim() != "")
            {
                foreach (var item in DAO_Pegawai.GetByUnit_Entity(s_unit))
                {
                    if (lst_pegawai.ContainsKey(item.Kode.ToString()) == false && lst_nip.FindAll(m0 => m0 == item.Kode.ToString()).Count > 0)
                    {
                        lst_pegawai.Add(item.Kode, item.Nama);
                    }
                }
            }
            lst_pegawai = lst_pegawai.OrderBy(m0 => m0.Value).ToDictionary(m => m.Key, m => m.Value);
            html += "<datalist id=\"dl_guru\">";
            html += "<option selected data-value=\"\" value=\"\"></option>";
            if (s_unit.Trim() != "")
            {
                if (s_kelas.Trim() != "")
                {
                    txtFilter_GuruVal.Value = Libs.LOGGED_USER_M.NoInduk;
                    txtFilter_Guru.Value = DAO_Pegawai.GetByID_Entity(Libs.LOGGED_USER_M.NoInduk).Nama;
                    txtFilter_Guru.Attributes["disabled"] = "disabled";
                }
                else
                {
                    foreach (var m_guru in lst_pegawai)
                    {
                        html += "<option data-value=\"" + m_guru.Key.ToString() + "\">" + m_guru.Value + "</option>";
                    }
                }
            }
            html += "</datalist>";

            //siswa
            if (s_unit.Trim() != "")
            {
                if (txtFilter_KelasVal.Value.Trim() != "")
                {
                    List<DAO_Siswa.SiswaDataSimple> lst_siswa = DAO_Siswa.GetAllSiswaDataSimpleByTahunAjaranUnitKelas_Entity(
                        s_unit, txtFilter_KelasVal.Value, tahun_ajaran, semester
                    ).OrderBy(m0 => m0.Nama).ToList();
                    html += "<datalist id=\"dl_siswa_1\">";
                    html += "<option data-value=\"\" value=\"\"></option>";
                    if (s_unit.Trim() != "")
                    {
                        foreach (var m_siswa in lst_siswa)
                        {
                            html += "<option data-value=\"" + m_siswa.Kode.ToString() + "\">" + m_siswa.Nama.ToUpper().Trim() + "</option>";
                        }
                    }
                    html += "</datalist>";
                    html += "<datalist id=\"dl_siswa_2\">";
                    html += "<option data-value=\"\" value=\"\"></option>";
                    if (s_unit.Trim() != "")
                    {
                        foreach (var m_siswa in lst_siswa)
                        {
                            html += "<option data-value=\"" + m_siswa.Kode.ToString() + "\">" + m_siswa.Nama.ToUpper().Trim() + "</option>";
                        }
                    }
                    html += "</datalist>";
                }
                else
                {
                    List<Siswa> lst_siswa = DAO_Siswa.GetByUnit_Entity(
                        s_unit, tahun_ajaran, semester
                    ).OrderBy(m0 => m0.Nama).ToList();
                    html += "<datalist id=\"dl_siswa_1\">";
                    html += "<option data-value=\"\" value=\"\"></option>";
                    if (s_unit.Trim() != "")
                    {
                        foreach (var m_siswa in lst_siswa)
                        {
                            html += "<option data-value=\"" + m_siswa.Kode.ToString() + "\">" + m_siswa.Nama.ToUpper().Trim() + "</option>";
                        }
                    }
                    html += "</datalist>";
                    html += "<datalist id=\"dl_siswa_2\">";
                    html += "<option data-value=\"\" value=\"\"></option>";
                    if (s_unit.Trim() != "")
                    {
                        foreach (var m_siswa in lst_siswa)
                        {
                            html += "<option data-value=\"" + m_siswa.Kode.ToString() + "\">" + m_siswa.Nama.ToUpper().Trim() + "</option>";
                        }
                    }
                    html += "</datalist>";
                }
            }
            else
            {
                html += "<datalist id=\"dl_siswa_1\">";
                html += "</datalist>";
                html += "<datalist id=\"dl_siswa_2\">";
                html += "</datalist>";
            }

            ltrDataList.Text = html;

            if (!IsPostBack)
            {
                //CheckBox kedisiplinan
                string s_html = "";
                List<string> lst_kode_kedisiplinan = new List<string>();
                List<KedisiplinanSetup> lst_kedisiplinan = new List<KedisiplinanSetup>();

                if (s_unit.Trim() != "" && txtFilter_KelasVal.Value.Trim() != "")
                {
                    KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(txtFilter_KelasVal.Value.Trim());
                    if (m_kelas_det != null)
                    {
                        if (m_kelas_det.Nama != null)
                        {
                            lst_kedisiplinan = DAO_KedisiplinanSetup.GetByTABySMBySekolahByKelas_Entity(tahun_ajaran, semester, s_unit, m_kelas_det.Rel_Kelas.ToString());

                            foreach (var item_kedisiplinan_setup in lst_kedisiplinan)
                            {
                                if (lst_kode_kedisiplinan.FindAll(m1 => m1 == item_kedisiplinan_setup.Rel_Kedisiplinan_01.ToUpper().Trim()).Count == 0)
                                {
                                    lst_kode_kedisiplinan.Add(item_kedisiplinan_setup.Rel_Kedisiplinan_01);
                                }
                                if (lst_kode_kedisiplinan.FindAll(m1 => m1 == item_kedisiplinan_setup.Rel_Kedisiplinan_02.ToUpper().Trim()).Count == 0)
                                {
                                    lst_kode_kedisiplinan.Add(item_kedisiplinan_setup.Rel_Kedisiplinan_02);
                                }
                                if (lst_kode_kedisiplinan.FindAll(m1 => m1 == item_kedisiplinan_setup.Rel_Kedisiplinan_03.ToUpper().Trim()).Count == 0)
                                {
                                    lst_kode_kedisiplinan.Add(item_kedisiplinan_setup.Rel_Kedisiplinan_03);
                                }
                                if (lst_kode_kedisiplinan.FindAll(m1 => m1 == item_kedisiplinan_setup.Rel_Kedisiplinan_04.ToUpper().Trim()).Count == 0)
                                {
                                    lst_kode_kedisiplinan.Add(item_kedisiplinan_setup.Rel_Kedisiplinan_04);
                                }
                                if (lst_kode_kedisiplinan.FindAll(m1 => m1 == item_kedisiplinan_setup.Rel_Kedisiplinan_05.ToUpper().Trim()).Count == 0)
                                {
                                    lst_kode_kedisiplinan.Add(item_kedisiplinan_setup.Rel_Kedisiplinan_05);
                                }
                                if (lst_kode_kedisiplinan.FindAll(m1 => m1 == item_kedisiplinan_setup.Rel_Kedisiplinan_06.ToUpper().Trim()).Count == 0)
                                {
                                    lst_kode_kedisiplinan.Add(item_kedisiplinan_setup.Rel_Kedisiplinan_06);
                                }
                                if (lst_kode_kedisiplinan.FindAll(m1 => m1 == item_kedisiplinan_setup.Rel_Kedisiplinan_07.ToUpper().Trim()).Count == 0)
                                {
                                    lst_kode_kedisiplinan.Add(item_kedisiplinan_setup.Rel_Kedisiplinan_07);
                                }
                                if (lst_kode_kedisiplinan.FindAll(m1 => m1 == item_kedisiplinan_setup.Rel_Kedisiplinan_08.ToUpper().Trim()).Count == 0)
                                {
                                    lst_kode_kedisiplinan.Add(item_kedisiplinan_setup.Rel_Kedisiplinan_08);
                                }
                                if (lst_kode_kedisiplinan.FindAll(m1 => m1 == item_kedisiplinan_setup.Rel_Kedisiplinan_09.ToUpper().Trim()).Count == 0)
                                {
                                    lst_kode_kedisiplinan.Add(item_kedisiplinan_setup.Rel_Kedisiplinan_09);
                                }
                                if (lst_kode_kedisiplinan.FindAll(m1 => m1 == item_kedisiplinan_setup.Rel_Kedisiplinan_10.ToUpper().Trim()).Count == 0)
                                {
                                    lst_kode_kedisiplinan.Add(item_kedisiplinan_setup.Rel_Kedisiplinan_10);
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (Sekolah m in lst_sekolah)
                    {
                        List<Kelas> lst_kelas = DAO_Kelas.GetAllByUnit_Entity(m.Kode.ToString()).FindAll(m0 => m0.IsAktif == true).OrderBy(m0 => m0.UrutanLevel).ToList();
                        foreach (var item_kelas in lst_kelas)
                        {
                            lst_kedisiplinan = DAO_KedisiplinanSetup.GetBySekolahByKelas_Entity(m.Kode.ToString(), item_kelas.Kode.ToString());

                            foreach (var item_kedisiplinan_setup in lst_kedisiplinan)
                            {
                                if (lst_kode_kedisiplinan.FindAll(m1 => m1 == item_kedisiplinan_setup.Rel_Kedisiplinan_01.ToUpper().Trim()).Count == 0)
                                {
                                    lst_kode_kedisiplinan.Add(item_kedisiplinan_setup.Rel_Kedisiplinan_01);
                                }
                                if (lst_kode_kedisiplinan.FindAll(m1 => m1 == item_kedisiplinan_setup.Rel_Kedisiplinan_02.ToUpper().Trim()).Count == 0)
                                {
                                    lst_kode_kedisiplinan.Add(item_kedisiplinan_setup.Rel_Kedisiplinan_02);
                                }
                                if (lst_kode_kedisiplinan.FindAll(m1 => m1 == item_kedisiplinan_setup.Rel_Kedisiplinan_03.ToUpper().Trim()).Count == 0)
                                {
                                    lst_kode_kedisiplinan.Add(item_kedisiplinan_setup.Rel_Kedisiplinan_03);
                                }
                                if (lst_kode_kedisiplinan.FindAll(m1 => m1 == item_kedisiplinan_setup.Rel_Kedisiplinan_04.ToUpper().Trim()).Count == 0)
                                {
                                    lst_kode_kedisiplinan.Add(item_kedisiplinan_setup.Rel_Kedisiplinan_04);
                                }
                                if (lst_kode_kedisiplinan.FindAll(m1 => m1 == item_kedisiplinan_setup.Rel_Kedisiplinan_05.ToUpper().Trim()).Count == 0)
                                {
                                    lst_kode_kedisiplinan.Add(item_kedisiplinan_setup.Rel_Kedisiplinan_05);
                                }
                                if (lst_kode_kedisiplinan.FindAll(m1 => m1 == item_kedisiplinan_setup.Rel_Kedisiplinan_06.ToUpper().Trim()).Count == 0)
                                {
                                    lst_kode_kedisiplinan.Add(item_kedisiplinan_setup.Rel_Kedisiplinan_06);
                                }
                                if (lst_kode_kedisiplinan.FindAll(m1 => m1 == item_kedisiplinan_setup.Rel_Kedisiplinan_07.ToUpper().Trim()).Count == 0)
                                {
                                    lst_kode_kedisiplinan.Add(item_kedisiplinan_setup.Rel_Kedisiplinan_07);
                                }
                                if (lst_kode_kedisiplinan.FindAll(m1 => m1 == item_kedisiplinan_setup.Rel_Kedisiplinan_08.ToUpper().Trim()).Count == 0)
                                {
                                    lst_kode_kedisiplinan.Add(item_kedisiplinan_setup.Rel_Kedisiplinan_08);
                                }
                                if (lst_kode_kedisiplinan.FindAll(m1 => m1 == item_kedisiplinan_setup.Rel_Kedisiplinan_09.ToUpper().Trim()).Count == 0)
                                {
                                    lst_kode_kedisiplinan.Add(item_kedisiplinan_setup.Rel_Kedisiplinan_09);
                                }
                                if (lst_kode_kedisiplinan.FindAll(m1 => m1 == item_kedisiplinan_setup.Rel_Kedisiplinan_10.ToUpper().Trim()).Count == 0)
                                {
                                    lst_kode_kedisiplinan.Add(item_kedisiplinan_setup.Rel_Kedisiplinan_10);
                                }
                            }
                        }
                    }
                }

                List<Kedisiplinan> lst_kedisiplinan_all = DAO_Kedisiplinan.GetAll_Entity();
                foreach (var item in lst_kode_kedisiplinan)
                {
                    Kedisiplinan m = lst_kedisiplinan_all.FindAll(m0 => m0.Kode.ToString().ToUpper().Trim() == item.ToUpper().Trim()).FirstOrDefault();
                    if (m != null)
                    {
                        if (m.Keterangan != null)
                        {
                            string chk_id = "chk_" + m.Kode.ToString().Replace("-", "_");
                            s_html += "<div class=\"row\">" +
                                        "<div class=\"col-xs-12\" style=\"text-align: left; padding-top: 15px; padding-bottom: 15px;\">" +
                                            "<div class=\"checkbox checkbox-adv\">" +
                                                "<label for=\"" + chk_id + "\">" +
                                                    "<input " +
                                                            "name=\"chk_pilih_kedisiplinan[]\" " +
                                                            "class=\"access-hide\" " +
                                                            "id=\"" + chk_id + "\" " +
                                                            "checked=\"checked\" " +
                                                            "value=\"" + m.Kode.ToString() + "\" " +
                                                            "type=\"checkbox\">" +
                                                    "<span class=\"checkbox-circle\"></span><span class=\"checkbox-circle-check\"></span><span class=\"checkbox-circle-icon icon\">done</span>" +
                                                    "<span style=\"font-weight: bold; font-size: 14px; color: black;\">" +
                                                        "&nbsp;&nbsp;" +
                                                        m.Keterangan +
                                                    "</span>" +
                                                "</label>" +
                                            "</div>" +
                                        "</div>" +
                                    "</div>";
                        }
                    }
                }

                ltrFilterKedisiplinan.Text = s_html;
            }
        }

        protected void ListPresensiDanKedisiplinan()
        {
            string chk_id_hadir = "chk_id_hadir";
            string chk_id_sakit = "chk_id_sakit";
            string chk_id_izin = "chk_id_izin";
            string chk_id_alpa = "chk_id_alpa";

            ltrFilterPresensi.Text =
                "<div class=\"row\">" +
                    "<div class=\"col-xs-12\" style=\"text-align: left; padding-top: 15px; padding-bottom: 15px;\">" +
                        "<div class=\"checkbox checkbox-adv\">" +
                            "<label for=\"" + chk_id_hadir + "\">" +
                                "<input " +
                                        "name=\"chk_pilih_presensi[]\" " +
                                        "class=\"access-hide\" " +
                                        "id=\"" + chk_id_hadir + "\" " +
                                        "checked=\"checked\" " +
                                        "type=\"checkbox\">" +
                                "<span class=\"checkbox-circle\"></span><span class=\"checkbox-circle-check\"></span><span class=\"checkbox-circle-icon icon\">done</span>" +
                                "<span style=\"font-weight: bold; font-size: 14px; color: black;\">" +
                                    "&nbsp;&nbsp;" +
                                    Libs.JENIS_ABSENSI.HADIR +
                                "</span>" +
                            "</label>" +
                        "</div>" +
                    "</div>" +
                "</div>" +
                "<div class=\"row\">" +
                    "<div class=\"col-xs-12\" style=\"text-align: left; padding-top: 15px; padding-bottom: 15px;\">" +
                        "<div class=\"checkbox checkbox-adv\">" +
                            "<label for=\"" + chk_id_sakit + "\">" +
                                "<input " +
                                        "name=\"chk_pilih_presensi[]\" " +
                                        "class=\"access-hide\" " +
                                        "id=\"" + chk_id_sakit + "\" " +
                                        "checked=\"checked\" " +
                                        "type=\"checkbox\">" +
                                "<span class=\"checkbox-circle\"></span><span class=\"checkbox-circle-check\"></span><span class=\"checkbox-circle-icon icon\">done</span>" +
                                "<span style=\"font-weight: bold; font-size: 14px; color: black;\">" +
                                    "&nbsp;&nbsp;" +
                                    Libs.JENIS_ABSENSI.SAKIT +
                                "</span>" +
                            "</label>" +
                        "</div>" +
                    "</div>" +
                "</div>" +

                "<div class=\"row\">" +
                    "<div class=\"col-xs-12\" style=\"text-align: left; padding-top: 15px; padding-bottom: 15px;\">" +
                        "<div class=\"checkbox checkbox-adv\">" +
                            "<label for=\"" + chk_id_izin + "\">" +
                                "<input " +
                                        "name=\"chk_pilih_presensi[]\" " +
                                        "class=\"access-hide\" " +
                                        "id=\"" + chk_id_izin + "\" " +
                                        "checked=\"checked\" " +
                                        "type=\"checkbox\">" +
                                "<span class=\"checkbox-circle\"></span><span class=\"checkbox-circle-check\"></span><span class=\"checkbox-circle-icon icon\">done</span>" +
                                "<span style=\"font-weight: bold; font-size: 14px; color: black;\">" +
                                    "&nbsp;&nbsp;" +
                                    Libs.JENIS_ABSENSI.IZIN +
                                "</span>" +
                            "</label>" +
                        "</div>" +
                    "</div>" +
                "</div>" +
                "<div class=\"row\">" +
                    "<div class=\"col-xs-12\" style=\"text-align: left; padding-top: 15px; padding-bottom: 15px;\">" +
                        "<div class=\"checkbox checkbox-adv\">" +
                            "<label for=\"" + chk_id_alpa + "\">" +
                                "<input " +
                                        "name=\"chk_pilih_presensi[]\" " +
                                        "id=\"" + chk_id_alpa + "\" " +
                                        "checked=\"checked\" " +
                                        "class=\"access-hide\" " +
                                        "type=\"checkbox\">" +
                                "<span class=\"checkbox-circle\"></span><span class=\"checkbox-circle-check\"></span><span class=\"checkbox-circle-icon icon\">done</span>" +
                                "<span style=\"font-weight: bold; font-size: 14px; color: black;\">" +
                                    "&nbsp;&nbsp;" +
                                    Libs.JENIS_ABSENSI.ALPA +
                                "</span>" +
                            "</label>" +
                        "</div>" +
                    "</div>" +
                "</div>";

            string html_chk = "";
            string html_chk_kedisiplinan = "";

            KedisiplinanSetup m_kedisiplinan_setup = new KedisiplinanSetup();
            if (rdoPilihHari.Checked)
            {
                DAO_KedisiplinanSetup.GetByTABySMBySekolahByKelas_Entity(
                                    Libs.GetTahunAjaranByTanggal(Libs.GetDateFromTanggalIndonesiaStr(txtTglAbsenPerHari.Text)),
                                    Libs.GetSemesterByTanggal(Libs.GetDateFromTanggalIndonesiaStr(txtTglAbsenPerHari.Text)).ToString(),
                                    txtFilter_Unit.Value,
                                    txtFilter_Kelas.Value
                                ).FirstOrDefault();
            }
            else
            {
                DAO_KedisiplinanSetup.GetByTABySMBySekolahByKelas_Entity(
                                    Libs.GetTahunAjaranByTanggal(Libs.GetDateFromTanggalIndonesiaStr(txtTglAbsenPerPeriode_Dari.Text)),
                                    Libs.GetSemesterByTanggal(Libs.GetDateFromTanggalIndonesiaStr(txtTglAbsenPerPeriode_Sampai.Text)).ToString(),
                                    txtFilter_Unit.Value,
                                    txtFilter_Kelas.Value
                                ).FirstOrDefault();
            }
                
            if (m_kedisiplinan_setup != null)
            {
                if (m_kedisiplinan_setup.TahunAjaran != null)
                {
                    int id_kedisiplinan = 1;
                    foreach (var prop in m_kedisiplinan_setup.GetType().GetProperties())
                    {
                        if (prop.Name.IndexOf("Rel_Kedisiplinan_") >= 0)
                        {
                            var val = prop.GetValue(m_kedisiplinan_setup, null);
                            if (val != null)
                            {
                                if (val.ToString() != "")
                                {
                                    Kedisiplinan m_kedisiplinan = DAO_Kedisiplinan.GetByID_Entity(
                                                val.ToString()
                                            );
                                    if (m_kedisiplinan != null)
                                    {
                                        if (m_kedisiplinan.Keterangan != null)
                                        {
                                            string chk_id = "chk_" +
                                                            m_kedisiplinan.Alias + "_" +
                                                            m_kedisiplinan.Kode.ToString().Replace("-", "_");
                                            
                                            html_chk_kedisiplinan +=
                                                        (
                                                            id_kedisiplinan % 2 == 1 ||
                                                            id_kedisiplinan == 1
                                                            ? (
                                                                id_kedisiplinan % 2 == 1 &&
                                                                id_kedisiplinan != 1
                                                                ? "</div>"
                                                                : ""
                                                              ) +
                                                              "<div class=\"row\" style=\"margin-right: 0px; margin-left: 0px;\">"
                                                            : ""
                                                        ) + "<div class=\"col-xs-6\" style=\"margin-right: 0px; padding-right: 0px;\">" +
                                                                "<div class=\"checkbox checkbox-adv\">" +
                                                                    "<label for=\"" + chk_id + "\">" +
                                                                        "<input " +
                                                                                "name=\"chk_pilih_kedisiplinan[]\" " +
                                                                                "class=\"access-hide\" " +
                                                                                "id=\"" + chk_id + "\" " +
                                                                                "checked=\"checked\" " +
                                                                                "value=\"" + m_kedisiplinan.Kode.ToString() + "\" " +
                                                                                "type=\"checkbox\" />" +
                                                                        "<span class=\"checkbox-circle\"></span><span class=\"checkbox-circle-check\"></span><span class=\"checkbox-circle-icon icon\">done</span>" +
                                                                        "<span style=\"font-weight: bold; font-size: 14px; color: black;\">" +
                                                                            m_kedisiplinan.Keterangan.Trim() +
                                                                        "</span>" +
                                                                    "</label>" +
                                                                "</div>" +
                                                            "</div>";

                                            id_kedisiplinan++;

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (html_chk_kedisiplinan.Trim() != "")
            {
                html_chk = "<div class=\"row\">" +
                                html_chk_kedisiplinan +
                                "</div>" +
                           "</div>";
            }

            ltrFilterKedisiplinan.Text = html_chk;
        }

        protected void InitEnabledFilter()
        {
            //init filter
            txtFilter_Unit.Attributes["disabled"] = "disabled";
            txtFilter_Kelas.Attributes["disabled"] = "disabled";
            txtFilter_Mapel.Attributes["disabled"] = "disabled";
            txtFilter_Guru.Attributes["disabled"] = "disabled";
            txtFilter_Siswa1.Attributes["disabled"] = "disabled";

            div_unit.Attributes["style"] = "display: none;";
            div_kelas.Attributes["style"] = "display: none;";
            div_mapel.Attributes["style"] = "display: none;";
            div_guru.Attributes["style"] = "display: none;";
            div_siswa.Attributes["style"] = "display: none;";

            if (rdoGroupByUnit.Checked)
            {
                txtFilter_Unit.Attributes.Remove("disabled");

                div_unit.Attributes["style"] = "";

                txtFilter_Kelas.Value = "";
                txtFilter_KelasVal.Value = "";
                txtFilter_Mapel.Value = "";
                txtFilter_MapelVal.Value = "";
                txtFilter_Guru.Value = "";
                txtFilter_GuruVal.Value = "";
                txtFilter_Siswa1.Value = "";
                txtFilter_Siswa1Val.Value = "";
            }
            else if (rdoGroupByKelas.Checked)
            {
                txtFilter_Unit.Attributes.Remove("disabled");
                txtFilter_Kelas.Attributes.Remove("disabled");

                div_unit.Attributes["style"] = "";
                div_kelas.Attributes["style"] = "";

                txtFilter_Mapel.Value = "";
                txtFilter_MapelVal.Value = "";
                txtFilter_Guru.Value = "";
                txtFilter_GuruVal.Value = "";
                txtFilter_Siswa1.Value = "";
                txtFilter_Siswa1Val.Value = "";
            }
            else if (rdoGroupByMapel.Checked)
            {
                txtFilter_Unit.Attributes.Remove("disabled");
                txtFilter_Mapel.Attributes.Remove("disabled");

                div_unit.Attributes["style"] = "";
                div_mapel.Attributes["style"] = "";

                txtFilter_Kelas.Value = "";
                txtFilter_KelasVal.Value = "";
                txtFilter_Guru.Value = "";
                txtFilter_GuruVal.Value = "";
                txtFilter_Siswa1.Value = "";
                txtFilter_Siswa1Val.Value = "";
            }
            else if (rdoGroupByGuru.Checked)
            {
                txtFilter_Unit.Attributes.Remove("disabled");
                txtFilter_Guru.Attributes.Remove("disabled");

                div_unit.Attributes["style"] = "";
                div_guru.Attributes["style"] = "";

                txtFilter_Kelas.Value = "";
                txtFilter_KelasVal.Value = "";
                txtFilter_Mapel.Value = "";
                txtFilter_MapelVal.Value = "";
                txtFilter_Siswa1.Value = "";
                txtFilter_Siswa1Val.Value = "";
            }
            else if (rdoGroupBySiswa.Checked)
            {
                txtFilter_Unit.Attributes.Remove("disabled");
                txtFilter_Siswa1.Attributes.Remove("disabled");
                
                div_unit.Attributes["style"] = "";
                div_siswa.Attributes["style"] = "";

                txtFilter_Kelas.Value = "";
                txtFilter_KelasVal.Value = "";
                txtFilter_Mapel.Value = "";
                txtFilter_MapelVal.Value = "";
                txtFilter_Guru.Value = "";
                txtFilter_GuruVal.Value = "";
            }
            else if (rdoGroupByMapelBySiswa.Checked)
            {
                txtFilter_Unit.Attributes.Remove("disabled");
                txtFilter_Kelas.Attributes.Remove("disabled");
                txtFilter_Mapel.Attributes.Remove("disabled");
                txtFilter_Guru.Attributes.Remove("disabled");
                txtFilter_Siswa1.Attributes.Remove("disabled");
                
                div_unit.Attributes["style"] = "";
                div_kelas.Attributes["style"] = "";
                div_mapel.Attributes["style"] = "";
                div_guru.Attributes["style"] = "";
                div_siswa.Attributes["style"] = "";
            }
            else if (rdoDetailAbsensi.Checked)
            {
                txtFilter_Unit.Attributes.Remove("disabled");
                txtFilter_Kelas.Attributes.Remove("disabled");
                txtFilter_Mapel.Attributes.Remove("disabled");
                txtFilter_Guru.Attributes.Remove("disabled");
                txtFilter_Siswa1.Attributes.Remove("disabled");

                div_unit.Attributes["style"] = "";
                div_kelas.Attributes["style"] = "";
                div_mapel.Attributes["style"] = "";
                div_guru.Attributes["style"] = "";
                div_siswa.Attributes["style"] = "";
            }
            //end init filter
        }

        protected void lnkOKDownload_Click(object sender, EventArgs e)
        {
                
        }

        protected void btnDownloadExcel_Click(object sender, EventArgs e)
        {
            if (rdoPilihPeriode.Checked)
            {
                DateTime dt_1 = Libs.GetDateFromTanggalIndonesiaStr(txtTglAbsenPerPeriode_Dari.Text);
                DateTime dt_2 = Libs.GetDateFromTanggalIndonesiaStr(txtTglAbsenPerPeriode_Sampai.Text);
                if (Math.Abs((dt_1 - dt_2).Days + 1) > 31)
                {
                    txtKeyAction.Value = "Periode presensi maksimal 31 Hari.";
                    return;
                }
            }
            txtKeyAction.Value = JenisAction.DoDownloadRekapAbsensi.ToString();
        }

        protected void btnShowDataList_Click(object sender, EventArgs e)
        {
            InitFilter();
        }

        protected void btnInitEnabledFilter_Click(object sender, EventArgs e)
        {
            InitEnabledFilter();
        }
    }
}