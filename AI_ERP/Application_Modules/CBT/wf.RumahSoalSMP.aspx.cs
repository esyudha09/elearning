using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Entities.Elearning.SMP;
using AI_ERP.Application_DAOs.Elearning.SMP;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.SMP
{
    public partial class wf_RumahSoal_SMP : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATASTRUKTURPENILAIAN_SMP";
        public static List<Rapor_NilaiSiswa_Det_Extend> lst_nilai = new List<Rapor_NilaiSiswa_Det_Extend>();

        public enum JenisAction
        {
            Add,
            AddWithMessage,
            AddAPWithMessage,
            AddKDWithMessage,
            AddKPWithMessage,
            Edit,
            Update,
            Delete,
            Search,
            ShowDataList,
            DoAdd,
            DoUpdate,
            DoDelete,
            DoSearch,
            DoChangePage,
            DoShowData,
            DoShowBukaSemester,
            DoShowLihatData,
            DoShowInputAspekPenilaian,
            DoShowInputKompetensiDasar,
            DoShowInputKomponenPenilaian,
            DoShowConfirmHapus,
            DoShowInputDeskripsiKTSP,
            DoShowInputDeskripsiKURTILAS,
            DoShowInputPredikat,
            DoShowStrukturNilai
        }

        public const string JenisActKP_EDIT = "341t";
        public const string JenisActKP_ADDEDIT = "a44341t";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" + ResolveUrl("~/Application_CLibs/images/svg/browser-2.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Struktur Penilaian";

            this.Master.ShowHeaderTools = true;
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {
                txtKompetensiDasar.NamaControl.ValidationGroup = "vldInputStrukturNilai";
                //InitKeyEventClient();
                ListDropdown();
                div_kembali_ke_header.Attributes["style"] = "background-color: #00198d; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 5; position: fixed; bottom: 33px; right: 50px; width: 160px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;";

                //lst_nilai = DAO_Rapor_NilaiSiswa_Det.GetAllByTABySM_Entity(
                //        GetTahunAjaran(), GetSemester()
                //    );
                BindListView(true, this.Master.txtCariData.Text);
            }

            switch (mvMain.ActiveViewIndex)
            {
                case 0:
                    this.Master.ShowHeaderTools = true;
                    break;
                case 1:
                    ShowStrukturNilai(txtID.Value);
                    this.Master.ShowHeaderTools = false;
                    break;
            }

            //if (DAO_FormasiGuruMapel.IsSebagaiGuru(Libs.LOGGED_USER_M.NoInduk, (int)Libs.UnitSekolah.SMP))
            //{
            //    //btnDoAdd.Visible = false;
            //    //div_button_settings_struktur_penilaian.Visible = false;                
            //    //div_hapus_struktur_nilai.Visible = false;
            //    //div_kembali_ke_header.Attributes["style"] = "background-color: #00198d; box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); background-image: none; color: white; display: block; z-index: 5; position: fixed; bottom: 33px; right: 20px; width: 120px; border-radius: 25px; padding: 8px; margin: 0px; height: 35px;";
            //}

            if (!IsPostBack) this.Master.txtCariData.Text = Libs.GetQ();
        }

        public static string GetHTMLKelasMapelDetIsiNilai(string tahun_ajaran, string semester, string rel_kelas, string rel_mapel)
        {
            string s_html = "";
            Dictionary<string, string> lst_kelasdet = new Dictionary<string, string>();
            List<DAO_FormasiGuruMapelDet.FormasiGuruMapelDet_Lengkap> lst_guru = DAO_FormasiGuruMapelDet.GetByTABySMByKelasByMapel_Entity(
                    tahun_ajaran, semester, rel_kelas, rel_mapel
                );

            Mapel m_mapel = DAO_Mapel.GetByID_Entity(rel_mapel);
            if (m_mapel != null && m_mapel.Nama != null)
            {
                foreach (DAO_FormasiGuruMapelDet.FormasiGuruMapelDet_Lengkap item_guru in lst_guru)
                {
                    KelasDet m_kelas_det = new KelasDet();
                    if (m_mapel.Jenis == Libs.JENIS_MAPEL.LINTAS_MINAT)
                    {
                        List<string> lst_kelas_det = DAO_FormasiGuruMapelDetSiswa.GetByDistinctKelasDetHeader_Entity(item_guru.Rel_FormasiGuruMapel.ToString());
                        foreach (var item_kelas_det in lst_kelas_det)
                        {
                            m_kelas_det = DAO_KelasDet.GetByID_Entity(item_kelas_det);
                            if (m_kelas_det != null)
                            {
                                if (m_kelas_det.Nama != null)
                                {
                                    if (!lst_kelasdet.ContainsKey(m_kelas_det.Kode.ToString()))
                                    {
                                        lst_kelasdet.Add(m_kelas_det.Kode.ToString(), m_kelas_det.Nama);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        m_kelas_det = DAO_KelasDet.GetByID_Entity(item_guru.Rel_KelasDet.ToString());
                        if (m_kelas_det != null)
                        {
                            if (m_kelas_det.Nama != null)
                            {
                                if (!lst_kelasdet.ContainsKey(m_kelas_det.Kode.ToString()))
                                {
                                    lst_kelasdet.Add(m_kelas_det.Kode.ToString(), m_kelas_det.Nama);
                                }
                            }
                        }
                    }
                }
            }

            foreach (var item_kelas in lst_kelasdet)
            {
                bool is_isi_nilai = (
                        lst_nilai.FindAll(m0 => m0.Rel_Mapel.ToString().ToUpper().Trim() == rel_mapel.ToString().ToUpper().Trim() && m0.Rel_KelasDet.ToString().ToUpper().Trim() == item_kelas.Key.ToString().ToUpper().Trim()).Count > 0
                        ? true
                        : false
                    );

                s_html += "<div class=\"tooltip\">" +
                              "<i class=\"fa fa-circle\" style=\"margin-right: 3px; color: " + (is_isi_nilai ? "green" : "darkorange") + "; font-size: x-small;\"></i>" +
                              "<div class=\"top\" style=\"width: 130px;\">" +
                                "<label style=\"display: table; font-weight: bold; width: 100%;\">" +
                                    item_kelas.Value + 
                                "</label>" +
                                (
                                    is_isi_nilai
                                    ? "<label style=\"display: table; font-weight: normal; font-size: x-small; width: 100%;\">" +
                                        "Sudah isi Nilai" +
                                      "</label>"
                                    : "<label style=\"display: table; font-weight: normal; font-size: x-small; width: 100%;\">" +
                                        "Belum isi Nilai" +
                                      "</label>"
                                ) +
                                "<i></i>" +
                              "</div>" +
                          "</div>";
            }

            return s_html;
        }

        protected Sekolah GetSekolah()
        {
            Sekolah sekolah = DAO_Sekolah.GetAll_Entity().FindAll(
                m => m.UrutanJenjang == (int)Libs.UnitSekolah.SMP).FirstOrDefault();

            return sekolah;
        }

        protected void ListDropdown()
        {
            Sekolah sekolah = GetSekolah();
            if (sekolah != null)
            {
                if (sekolah.Nama != null)
                {
                    List<Kelas> lst_kelas = DAO_Kelas.GetAllByUnit_Entity(sekolah.Kode.ToString());
                    cboKelas.Items.Clear();
                    cboKelas.Items.Add("");
                    foreach (Kelas m in lst_kelas)
                    {
                        if (m.IsAktif)
                        {
                            cboKelas.Items.Add(new ListItem
                            {
                                Value = m.Kode.ToString(),
                                Text = m.Nama
                            });
                        }
                    }

                    cboKelasKTSP.Items.Clear();
                    cboKelasKTSP.Items.Add("");
                    foreach (Kelas m in lst_kelas)
                    {
                        if (m.IsAktif)
                        {
                            cboKelasKTSP.Items.Add(new ListItem
                            {
                                Value = m.Kode.ToString(),
                                Text = m.Nama
                            });
                        }
                    }

                    cboKelasKURTILAS.Items.Clear();
                    cboKelasKURTILAS.Items.Add("");
                    foreach (Kelas m in lst_kelas)
                    {
                        if (m.IsAktif)
                        {
                            cboKelasKURTILAS.Items.Add(new ListItem
                            {
                                Value = m.Kode.ToString(),
                                Text = m.Nama
                            });
                        }
                    }

                    List<Mapel> lst_mapel = DAO_Mapel.GetAllBySekolah_Entity(sekolah.Kode.ToString()).OrderBy(m => m.Nama).ToList();
                    cboMapel.Items.Clear();
                    cboMapel.Items.Add("");
                    foreach (Mapel m in lst_mapel)
                    {
                        cboMapel.Items.Add(new ListItem
                        {
                            Value = m.Kode.ToString(),
                            Text = m.Nama
                        });
                    }

                    cboMapelKURTILAS.Items.Clear();
                    cboMapelKURTILAS.Items.Add("");
                    foreach (Mapel m in lst_mapel)
                    {
                        cboMapelKURTILAS.Items.Add(new ListItem
                        {
                            Value = m.Kode.ToString(),
                            Text = m.Nama
                        });
                    }

                    cboMapelKTSP.Items.Clear();
                    cboMapelKTSP.Items.Add("");
                    foreach (Mapel m in lst_mapel)
                    {
                        cboMapelKTSP.Items.Add(new ListItem
                        {
                            Value = m.Kode.ToString(),
                            Text = m.Nama
                        });
                    }
                }
            }

            //jenis perhitungan rapor
            cboJenisPerhitunganRapor.Items.Clear();
            cboJenisPerhitunganRapor.Items.Add("");
            cboJenisPerhitunganRapor.Items.Add(new ListItem { Value = ((int)Libs.JenisPerhitunganNilai.Bobot).ToString(), Text = Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.Bobot) });
            cboJenisPerhitunganRapor.Items.Add(new ListItem { Value = ((int)Libs.JenisPerhitunganNilai.RataRata).ToString(), Text = Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.RataRata) });

            //jenis perhitungan aspek penilaian
            cboJenisPerhitunganAspekPenilaian.Items.Clear();
            cboJenisPerhitunganAspekPenilaian.Items.Add("");
            cboJenisPerhitunganAspekPenilaian.Items.Add(new ListItem { Value = ((int)Libs.JenisPerhitunganNilai.Bobot).ToString(), Text = Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.Bobot) });
            cboJenisPerhitunganAspekPenilaian.Items.Add(new ListItem { Value = ((int)Libs.JenisPerhitunganNilai.RataRata).ToString(), Text = Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.RataRata) });

            //jenis perhitungan kompetensi dasar
            cboJenisPerhitunganKompetensiDasar.Items.Clear();
            cboJenisPerhitunganKompetensiDasar.Items.Add("");
            cboJenisPerhitunganKompetensiDasar.Items.Add(new ListItem { Value = ((int)Libs.JenisPerhitunganNilai.Bobot).ToString(), Text = Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.Bobot) });
            cboJenisPerhitunganKompetensiDasar.Items.Add(new ListItem { Value = ((int)Libs.JenisPerhitunganNilai.RataRata).ToString(), Text = Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.RataRata) });

            var lst_periode = DAO_Rapor_StrukturNilai.GetDistinctTahunAjaranPeriode_Entity();
            cboPeriodeLihatData.Items.Clear();
            foreach (var item in lst_periode)
            {
                cboPeriodeLihatData.Items.Add(new ListItem
                {
                    Value = item.TahunAjaran.ToString() + item.Semester.ToString(),
                    Text = item.TahunAjaran + " semester " + item.Semester
                });
            }
        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
            txtTahunAjaran.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboSemester.ClientID + "').focus(); return false; }");
            cboSemester.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboMapel.ClientID + "').focus(); return false; }");
            cboMapel.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboKelas.ClientID + "').focus(); return false; }");
            cboMapel.Attributes.Add("onchange", "ValidateInputByMapel();");
            cboKelas.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtKKM.ClientID + "').focus(); return false; }");
            txtKKM.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboJenisPerhitunganRapor.ClientID + "').focus(); return false; }");
            cboJenisPerhitunganRapor.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKInput.ClientID + "').click(); return false; }");

            txtPoinKompetensiDasar.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtKompetensiDasar.NamaClientID + "').focus(); return false; }");
            txtBobotAP.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKKompetensiDasar.ClientID + "').click(); return false; }");

            txtBobotNKD.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKKomponenPenilaian.ClientID + "').click(); return false; }");

            txtPoinAspekPenilaian.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtAspekPenilaian.NamaClientID + "').focus(); return false; }");
            cboJenisPerhitunganAspekPenilaian.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKAspekPenilaian.ClientID + "').click(); return false; }");
        }

        protected string GetTahunAjaran()
        {
            string tahun_ajaran = "";
            
            if (cboPeriodeLihatData.Items.Count > 0)
            {
                if (cboPeriodeLihatData.SelectedValue.Trim() != "")
                {
                    tahun_ajaran = cboPeriodeLihatData.SelectedValue.Substring(0, 9);
                }
            }

            string periode = Libs.GetQueryString("p");
            periode = periode.Replace("-", "/");
            if (periode.Trim() != "")
            {
                if (periode.Length > 9)
                {
                    tahun_ajaran = periode.Substring(0, 9);
                }
            }

            return tahun_ajaran;
        }

        protected string GetSemester()
        {
            string semester = "";

            if (cboPeriodeLihatData.Items.Count > 0)
            {
                if (cboPeriodeLihatData.SelectedValue.Trim() != "")
                {
                    semester = cboPeriodeLihatData.SelectedValue.Substring(cboPeriodeLihatData.SelectedValue.Length - 1, 1);
                }
            }

            string periode = Libs.GetQueryString("p");
            periode = periode.Replace("-", "/");
            if (periode.Trim() != "")
            {
                if (periode.Length > 9)
                {
                    semester = periode.Substring(cboPeriodeLihatData.SelectedValue.Length - 1, 1);
                }
            }

            return semester;
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            string tahun_ajaran = GetTahunAjaran();
            string semester = GetSemester();

            string periode = Libs.GetQueryString("p");
            periode = periode.Replace("-", "/");

            sql_ds.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;

            if (tahun_ajaran.Trim() == "" && semester.Trim() == "")
            {
                if (DAO_FormasiGuruMapel.IsSebagaiGuru(Libs.LOGGED_USER_M.NoInduk, (int)Libs.UnitSekolah.SMP) && !Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit())
                {
                    if (keyword.Trim() != "")
                    {
                        sql_ds.SelectParameters.Clear();
                        sql_ds.SelectParameters.Add("nama", keyword);
                        sql_ds.SelectParameters.Add("Rel_Guru", Libs.LOGGED_USER_M.NoInduk);
                        sql_ds.SelectCommand = DAO_Rapor_StrukturNilai.SP_SELECT_ALL_BY_GURU_FOR_SEARCH;
                    }
                    else
                    {
                        sql_ds.SelectParameters.Clear();
                        sql_ds.SelectParameters.Add("Rel_Guru", Libs.LOGGED_USER_M.NoInduk);
                        sql_ds.SelectCommand = DAO_Rapor_StrukturNilai.SP_SELECT_ALL_BY_GURU;
                    }
                    if (isbind) lvData.DataBind();
                }
                else
                {
                    if (keyword.Trim() != "")
                    {
                        sql_ds.SelectParameters.Clear();
                        sql_ds.SelectParameters.Add("nama", keyword);
                        sql_ds.SelectCommand = DAO_Rapor_StrukturNilai.SP_SELECT_ALL_FOR_SEARCH;
                    }
                    else
                    {
                        sql_ds.SelectParameters.Clear();
                        sql_ds.SelectCommand = DAO_Rapor_StrukturNilai.SP_SELECT_ALL;
                    }
                    if (isbind) lvData.DataBind();
                }
            }
            else
            {
                if (DAO_FormasiGuruMapel.IsSebagaiGuru(Libs.LOGGED_USER_M.NoInduk, (int)Libs.UnitSekolah.SMP) && !Libs.URL_IDENTIFIER.IsAdaIDUrlIdAdminUnit())
                {
                    if (keyword.Trim() != "")
                    {
                        sql_ds.SelectParameters.Clear();
                        sql_ds.SelectParameters.Add("nama", keyword);
                        sql_ds.SelectParameters.Add("Rel_Guru", Libs.LOGGED_USER_M.NoInduk);
                        sql_ds.SelectParameters.Add("TahunAjaran", tahun_ajaran);
                        sql_ds.SelectParameters.Add("Semester", semester);
                        sql_ds.SelectCommand = DAO_Rapor_StrukturNilai.SP_SELECT_ALL_BY_GURU_BY_TA_BY_SM_FOR_SEARCH;
                    }
                    else
                    {
                        sql_ds.SelectParameters.Clear();
                        sql_ds.SelectParameters.Add("Rel_Guru", Libs.LOGGED_USER_M.NoInduk);
                        sql_ds.SelectParameters.Add("TahunAjaran", tahun_ajaran);
                        sql_ds.SelectParameters.Add("Semester", semester);
                        sql_ds.SelectCommand = DAO_Rapor_StrukturNilai.SP_SELECT_ALL_BY_GURU_BY_TA_BY_SM;
                    }
                    if (isbind) lvData.DataBind();
                }
                else
                {
                    if (keyword.Trim() != "")
                    {
                        sql_ds.SelectParameters.Clear();
                        sql_ds.SelectParameters.Add("nama", keyword);
                        sql_ds.SelectParameters.Add("TahunAjaran", tahun_ajaran);
                        sql_ds.SelectParameters.Add("Semester", semester);
                        sql_ds.SelectCommand = DAO_Rapor_StrukturNilai.SP_SELECT_ALL_BY_TA_BY_SM_FOR_SEARCH;
                    }
                    else
                    {
                        sql_ds.SelectParameters.Clear();
                        sql_ds.SelectParameters.Add("TahunAjaran", tahun_ajaran);
                        sql_ds.SelectParameters.Add("Semester", semester);
                        sql_ds.SelectCommand = DAO_CBT_RumahSoalSMA.SP_SMP_Rapor_StrukturNilai_SELECT_ALL_BY_TA_BY_MAPEL;
                    }
                    if (isbind) lvData.DataBind();
                }
            }

            if (periode.Trim() != "")
            {
                Libs.SelectDropdownListByValue(cboPeriodeLihatData, periode);
            }
        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {
            System.Web.UI.WebControls.Literal imgh_tahunajaran = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_tahunajaran");
            System.Web.UI.WebControls.Literal imgh_kelas = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_kelas");
            System.Web.UI.WebControls.Literal imgh_kkm = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_kkm");

            string html_image = "";
            if (e.SortDirection == SortDirection.Ascending)
            {
                html_image = "&nbsp;&nbsp;<i class=\"fa fa-chevron-up\" style=\"color: white;\"></i>";
            }
            else
            {
                html_image = "&nbsp;&nbsp;<i class=\"fa fa-chevron-down\" style=\"color: white;\"></i>";
            }

            imgh_tahunajaran.Text = html_image;
            imgh_kelas.Text = html_image;
            imgh_kkm.Text = html_image;

            imgh_tahunajaran.Visible = false;
            imgh_kelas.Visible = false;
            imgh_kkm.Visible = false;

            switch (e.SortExpression.ToString().Trim())
            {
                case "TahunAjaran":
                    imgh_tahunajaran.Visible = true;
                    break;
                case "Kelas":
                    imgh_kelas.Visible = true;
                    break;
                case "KKM":
                    imgh_kkm.Visible = true;
                    break;
            }

            int pageindex = int.Parse(Math.Ceiling(Convert.ToDecimal(dpData.StartRowIndex / 20)).ToString());
            pageindex--;
            this.Session[SessionViewDataName] = (pageindex < 0 ? 0 : pageindex);
        }

        protected void lvData_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            this.Session[SessionViewDataName] = e.StartRowIndex;
            BindListView(true, this.Master.txtCariData.Text);
            txtKeyAction.Value = JenisAction.DoChangePage.ToString();
        }

        protected void DoRefresh(bool refresh_all = false)
        {
            if (refresh_all)
            {
                Response.Redirect(Libs.FILE_PAGE_URL);
            }
            else
            {
                string periode = GetPeriode();
                periode = periode.Replace("/", "-");

                Response.Redirect(Libs.FILE_PAGE_URL + (periode != "" ? "?p=" + periode : ""));
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            DoRefresh();
        }

        protected void InitFields()
        {
            txtID.Value = "";
            ListDropdown();
            cboKelas.SelectedValue = "";
            cboMapel.SelectedValue = "";
            chkKelasVII.Checked = false;
            chkKelasVIII.Checked = false;
            chkKelasIX.Checked = false;
            txtKKM.Text = "";
            chkIs_PH_PTS_PAS.Checked = false;
            txtBobotPH.Text = "";
            txtBobotPTS.Text = "";
            txtBobotPAS.Text = "";
            txtDeskripsiSikapSosial.Text = "";
            txtDeskripsiSikapSosialVal.Value = "";
            txtDeskripsiSikapSpiritual.Text = "";
            txtDeskripsiSikapSpiritualVal.Value = "";
        }

        protected void btnDoAdd_Click(object sender, EventArgs e)
        {
            InitFields();

            vldMapel.Enabled = true;
            List<Mapel> lst_mapel = DAO_Mapel.GetAllBySekolah_Entity(GetSekolah().Kode.ToString()).FindAll(m0 => DAO_Mapel.GetJenisMapelByJenis(m0.Jenis) != Application_Libs.Libs.JENIS_MAPEL.EKSTRAKURIKULER).OrderBy(m => m.Nama).ToList();
            cboMapel.Items.Clear();
            cboMapel.Items.Add("");
            foreach (Mapel m in lst_mapel)
            {
                cboMapel.Items.Add(new ListItem
                {
                    Value = m.Kode.ToString(),
                    Text = m.Nama
                });
            }

            ShowKelasEkskul(false);
            EnableInputHeader(true);
            txtKeyAction.Value = JenisAction.Add.ToString();
        }

        protected void ShowKelasEkskul(bool show)
        {
            div_kelas_non_ekskul.Visible = !show;
            div_kelas_ekskul.Visible = show;
        }

        protected void lnkOKHapus_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID.Value.Trim() != "")
                {
                    DAO_Rapor_StrukturNilai.Delete(txtID.Value, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, this.Master.txtCariData.Text);
                    txtKeyAction.Value = JenisAction.DoDelete.ToString();
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void lnkOKInput_Click(object sender, EventArgs e)
        {
            try
            {
                //if (DAO_Rapor_StrukturNilai.GetMengajar_Entity(
                //        txtTahunAjaran.Text, cboSemester.Text, Libs.LOGGED_USER_M.NoInduk, cboMapel.SelectedValue, cboKelas.SelectedValue).Count > 0
                //){
                Rapor_StrukturNilai m = new Rapor_StrukturNilai();
                m.TahunAjaran = txtTahunAjaran.Text;
                m.Semester = cboSemester.SelectedValue;
                m.Rel_Mapel = new Guid(cboMapel.SelectedValue);

                m.IsNilaiAkhir = false;
                m.DeskripsiSikapSpiritual = txtDeskripsiSikapSpiritualVal.Value;
                m.DeskripsiSikapSosial = txtDeskripsiSikapSosialVal.Value;

                Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.Rel_Mapel.ToString());
                if (DAO_Mapel.GetJenisMapelByJenis(m_mapel.Jenis) == Application_Libs.Libs.JENIS_MAPEL.EKSTRAKURIKULER)
                {
                    m.Rel_Kelas = (!chkKelasVII.Checked ? new Guid(Constantas.GUID_NOL) : new Guid(GetKelasVII()));
                    m.Rel_Kelas2 = (!chkKelasVIII.Checked ? Constantas.GUID_NOL : GetKelasVIII());
                    m.Rel_Kelas3 = (!chkKelasIX.Checked ? Constantas.GUID_NOL : GetKelasIX());
                }
                else
                {
                    m.Rel_Kelas = new Guid(cboKelas.SelectedValue);
                    m.Rel_Kelas2 = Constantas.GUID_NOL;
                    m.Rel_Kelas3 = Constantas.GUID_NOL;
                }
                
                m.KKM = Libs.GetStringToDecimal(txtKKM.Text);
                m.JenisPerhitungan = cboJenisPerhitunganRapor.SelectedValue;
                m.Is_PH_PTS_PAS = chkIs_PH_PTS_PAS.Checked;
                m.BobotPH = Libs.GetStringToDecimal(txtBobotPH.Text);
                m.BobotPTS = Libs.GetStringToDecimal(txtBobotPTS.Text);
                m.BobotPAS = Libs.GetStringToDecimal(txtBobotPAS.Text);
                if (txtID.Value.Trim() != "")
                {
                    m.Kode = new Guid(txtID.Value);
                    DAO_Rapor_StrukturNilai.Update(m, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, this.Master.txtCariData.Text);
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
                else
                {
                    DAO_Rapor_StrukturNilai.Insert(m, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, this.Master.txtCariData.Text);
                    InitFields();
                    txtKeyAction.Value = JenisAction.AddWithMessage.ToString();
                }
                //}
                //else
                //{
                //    txtKeyAction.Value = "Input data struktur mata pelajaran tidak valid.";
                //}
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void EnableInputHeader(bool is_enable)
        {
            txtTahunAjaran.Enabled = is_enable;
            cboSemester.Enabled = is_enable;
            cboMapel.Enabled = is_enable;
            cboKelas.Enabled = is_enable;
            cboJenisPerhitunganRapor.Enabled = is_enable;
        }

        public static bool LockInput()
        {
            //if (DateTime.Now > Convert.ToDateTime("2018-09-26")) return true;
            return false;
        }

        protected string GetKelasVII()
        {
            Kelas m = DAO_Kelas.GetAllByUnit_Entity(GetSekolah().Kode.ToString()).FindAll(m0 => m0.UrutanLevel == 11).FirstOrDefault();

            if (m != null)
            {
                if (m.Nama != null)
                {
                    return (m.Kode.ToString());
                }
            }

            return "";
        }

        protected string GetKelasVIII()
        {
            Kelas m = DAO_Kelas.GetAllByUnit_Entity(GetSekolah().Kode.ToString()).FindAll(m0 => m0.UrutanLevel == 12).FirstOrDefault();

            if (m != null)
            {
                if (m.Nama != null)
                {
                    return (m.Kode.ToString());
                }
            }

            return "";
        }

        protected string GetKelasIX()
        {
            Kelas m = DAO_Kelas.GetAllByUnit_Entity(GetSekolah().Kode.ToString()).FindAll(m0 => m0.UrutanLevel == 13).FirstOrDefault();

            if (m != null)
            {
                if (m.Nama != null)
                {
                    return (m.Kode.ToString());
                }
            }

            return "";
        }

        protected bool IsKelasVII(string kode)
        {
            Kelas m = DAO_Kelas.GetByID_Entity(kode);

            if (m != null)
            {
                if (m.Nama != null)
                {
                    return (m.UrutanLevel == 11 ? true : false);
                }
            }

            return false;
        }

        protected bool IsKelasVIII(string kode)
        {
            Kelas m = DAO_Kelas.GetByID_Entity(kode);

            if (m != null)
            {
                if (m.Nama != null)
                {
                    return (m.UrutanLevel == 12 ? true : false);
                }
            }

            return false;
        }

        protected bool IsKelasIX(string kode)
        {
            Kelas m = DAO_Kelas.GetByID_Entity(kode);

            if (m != null)
            {
                if (m.Nama != null)
                {
                    return (m.UrutanLevel == 13 ? true : false);
                }
            }

            return false;
        }

        protected void btnShowDetail_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                Rapor_StrukturNilai m = DAO_Rapor_StrukturNilai.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.Rel_Mapel.ToString());
                        if (m_mapel != null)
                        {
                            if (m_mapel.Nama != null)
                            {
                                if (DAO_Mapel.GetJenisMapelByJenis(m_mapel.Jenis) == Application_Libs.Libs.JENIS_MAPEL.EKSTRAKURIKULER)
                                {
                                    vldMapel.Enabled = false;

                                    cboKelas.SelectedValue = "";
                                    chkKelasVII.Checked = IsKelasVII(m.Rel_Kelas.ToString());
                                    chkKelasVIII.Checked = IsKelasVIII(m.Rel_Kelas2.ToString());
                                    chkKelasIX.Checked = IsKelasIX(m.Rel_Kelas3.ToString());

                                    txtID.Value = m.Kode.ToString();
                                    txtTahunAjaran.Text = m.TahunAjaran;
                                    cboSemester.SelectedValue = m.Semester;
                                    cboMapel.SelectedValue = m.Rel_Mapel.ToString();                                    
                                    txtKKM.Text = m.KKM.ToString();
                                    chkIs_PH_PTS_PAS.Checked = m.Is_PH_PTS_PAS;
                                    txtBobotPH.Text = m.BobotPH.ToString();
                                    txtBobotPTS.Text = m.BobotPTS.ToString();
                                    txtBobotPAS.Text = m.BobotPAS.ToString();
                                    cboJenisPerhitunganRapor.SelectedValue = m.JenisPerhitungan;

                                    ShowKelasEkskul(true);
                                }
                                else
                                {
                                    vldMapel.Enabled = true;

                                    cboKelas.SelectedValue = m.Rel_Kelas.ToString();
                                    chkKelasVII.Checked = false;
                                    chkKelasVIII.Checked = false;
                                    chkKelasIX.Checked = false;

                                    txtID.Value = m.Kode.ToString();
                                    txtTahunAjaran.Text = m.TahunAjaran;
                                    cboSemester.SelectedValue = m.Semester;
                                    cboMapel.SelectedValue = m.Rel_Mapel.ToString();                                    
                                    txtKKM.Text = m.KKM.ToString();
                                    chkIs_PH_PTS_PAS.Checked = m.Is_PH_PTS_PAS;
                                    txtBobotPH.Text = m.BobotPH.ToString();
                                    txtBobotPTS.Text = m.BobotPTS.ToString();
                                    txtBobotPAS.Text = m.BobotPAS.ToString();
                                    cboJenisPerhitunganRapor.SelectedValue = m.JenisPerhitungan;

                                    txtDeskripsiSikapSpiritualVal.Value = m.DeskripsiSikapSpiritual;
                                    txtDeskripsiSikapSpiritual.Text = m.DeskripsiSikapSpiritual;
                                    txtDeskripsiSikapSosialVal.Value = m.DeskripsiSikapSosial;
                                    txtDeskripsiSikapSosial.Text = m.DeskripsiSikapSosial;

                                    ShowKelasEkskul(false);
                                }
                            }
                        }

                        //EnableInputHeader(!DAO_FormasiGuruMapel.IsSebagaiGuru(Libs.LOGGED_USER_M.NoInduk, (int)Libs.UnitSekolah.SMP));
                        EnableInputHeader(true);
                        txtKeyAction.Value = JenisAction.DoShowData.ToString();
                    }
                }
            }
        }

        protected string GetPeriode()
        {
            string periode = "";
            if (cboPeriodeLihatData.Items.Count > 0)
            {
                if (cboPeriodeLihatData.SelectedValue.Trim() != "")
                {
                    periode = cboPeriodeLihatData.SelectedValue;
                }
            }
            return periode;
        }

        protected void btnDoCari_Click(object sender, EventArgs e)
        {
            string periode = GetPeriode();
            periode = periode.Replace("/", "-");
            this.Session[SessionViewDataName] = 0;

            //Response.Redirect(
            //        Libs.FILE_PAGE_URL +
            //        (this.Master.txtCariData.Text.Trim() != "" ? "?q=" + this.Master.txtCariData.Text : "") +
            //        (periode != "" ? "&p=" + periode : "")
            //    );
            BindListView(true, this.Master.txtCariData.Text);
        }

        protected void btnShowConfirmDelete_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                Rapor_StrukturNilai m = DAO_Rapor_StrukturNilai.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        ltrMsgConfirmHapus.Text = "Hapus <span style=\"font-weight: bold;\">\"" +
                                                            Libs.GetHTMLSimpleText(m.TahunAjaran) +
                                                      "\"</span>?";
                        txtKeyAction.Value = JenisAction.DoShowConfirmHapus.ToString();
                    }
                }
            }
        }

        protected void btnShowDataListFromKTSPDet_Click(object sender, EventArgs e)
        {
            BindListView(true, this.Master.txtCariData.Text);
            mvMain.ActiveViewIndex = 0;
            this.Master.ShowHeaderTools = true;
            txtKeyAction.Value = JenisAction.ShowDataList.ToString();
        }

        protected void btnShowStrukturNilai_Click(object sender, EventArgs e)
        {
            ltrCaptionKTSPDet.Text = "";
            Rapor_StrukturNilai m = DAO_Rapor_StrukturNilai.GetByID_Entity(txtID.Value);
            if (m != null)
            {
                if (m.TahunAjaran != null)
                {
                    Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.Rel_Mapel.ToString());
                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(m.Rel_Kelas.ToString());

                    if (m.Rel_Kelas.ToString() != Constantas.GUID_NOL)
                    {
                        m_kelas = DAO_Kelas.GetByID_Entity(m.Rel_Kelas.ToString());
                    }
                    else if (m.Rel_Kelas2.ToString() != Constantas.GUID_NOL)
                    {
                        m_kelas = DAO_Kelas.GetByID_Entity(m.Rel_Kelas2.ToString());
                    }
                    else if (m.Rel_Kelas3.ToString() != Constantas.GUID_NOL)
                    {
                        m_kelas = DAO_Kelas.GetByID_Entity(m.Rel_Kelas3.ToString());
                    }
                    if (m_mapel != null && m_kelas != null)
                    {
                        if (m_mapel.Nama != null && m_kelas.Nama != null)
                        {
                            ltrCaptionKTSPDet.Text =
                                             "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                             "&nbsp;" +
                                             m.TahunAjaran +
                                             "&nbsp;" +
                                             "</span>" +

                                             "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                             "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                             "&nbsp;" +
                                             "Sm." + m.Semester +
                                             "&nbsp;" +
                                             "</span>" +

                                             "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                             "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                             "&nbsp;" +
                                             m_mapel.Nama +
                                             "&nbsp;" +
                                             "</span>" +

                                             "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                             "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                             "&nbsp;" +
                                             "Kelas&nbsp;" +
                                             m_kelas.Nama +
                                             "&nbsp;" +
                                             "</span>" +

                                             "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                             "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                             "&nbsp;" +
                                             "KKM : &nbsp;" +
                                             m.KKM.ToString() +
                                             "&nbsp;" +
                                             "</span>" +

                                             "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                             "<span title=\" Jenis Perhitungan Rapor \" class=\"badge\" style=\"background-color: black; color: yellow;\">" +
                                             "&nbsp;" +
                                             (
                                                m.JenisPerhitungan.ToString() == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString()
                                                ? Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.Bobot)
                                                : (
                                                    m.JenisPerhitungan.ToString() == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString()
                                                    ? Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.RataRata)
                                                    : ""
                                                  )
                                             ) +
                                             "&nbsp;" +
                                             "</span>";

                            ShowStrukturNilai(txtID.Value);
                            this.Master.ShowHeaderTools = false;
                            mvMain.ActiveViewIndex = 1;
                            txtKeyAction.Value = JenisAction.DoShowStrukturNilai.ToString();
                        }
                    }
                }
            }
        }

        protected void ShowStrukturNilai(string rel_strukturnilai)
        {
            string html = "";
            bool is_sebagai_guru = DAO_FormasiGuruMapel.IsSebagaiGuru(Libs.LOGGED_USER_M.NoInduk, (int)Libs.UnitSekolah.SMP);
            Rapor_StrukturNilai m_sn = DAO_Rapor_StrukturNilai.GetByID_Entity(rel_strukturnilai);
            List<Rapor_NilaiSiswa_Det> lst_rapor_nilai_det = 
                DAO_Rapor_NilaiSiswa_Det.GetAllByTABySMByKelasByMapel_Entity(m_sn.TahunAjaran, m_sn.Semester, m_sn.Rel_Kelas.ToString(), m_sn.Rel_Mapel.ToString());

            ltrStrukturNilaiDet.Text = "";

            //header
            html += "<tr>";
            html += "<td style=\"background-color: #424242; font-weight: normal; color: white; width: 40px; border-top-style: solid; border-top-color: #363535;\">" +
                        "<i class='fa fa-hashtag'></i>" +
                    "</td>";
            html += "<td style=\"background-color: #424242; font-weight: normal; color: white; width: 30px; border-top-style: solid; border-top-color: #363535;\">" +
                        "<img src=\"" + ResolveUrl("~/Application_CLibs/images/svg/browser.svg") + "\" style=\"height: 20px; width: 20px;\" />" +
                    "</td>";
            html += "<td colspan=\"6\" style=\"background-color: #424242; font-weight: bold; color: white; padding-left: 0px; border-top-style: solid; border-top-color: #363535;\">" +
                        "Aspek Penilaian <span class=\"badge\">AP</span>" +
                    "</td>";
            html += "<td style=\"background-color: #424242; border-top-style: solid; border-top-color: #363535;\">" +
                    "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=\"2\" style=\"background-color: #484848; border-top-style: solid; border-top-color: #424242;\">" +
                    "</td>";
            html += "<td style=\"background-color: #484848; font-weight: normal; color: white; width: 30px; border-top-style: solid; border-top-color: #424242;\">" +
                        "<img src=\"" + ResolveUrl("~/Application_CLibs/images/svg/questions.svg") + "\" style=\"height: 20px; width: 20px;\" />" +
                    "</td>";
            html += "<td colspan=\"3\" style=\"background-color: #484848; font-weight: bold; color: white; padding-left: 0px; border-top-style: solid; border-top-color: #424242;\">" +
                        "Kompetensi Dasar <span class=\"badge\">KD</span>" +
                    "</td>";
            html += "<td style=\"background-color: #484848; border-top-style: solid; border-top-color: #424242;\">" +
                    "</td>";
            html += "<td style=\"background-color: #484848; border-top-style: solid; border-top-color: #424242;\">" +
                    "</td>";
            html += "<td style=\"background-color: #484848; border-top-style: solid; border-top-color: #424242;\">" +
                    "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=\"4\" style=\"background-color: #4f4f4f; border-top-color: #424242;\">" +
                    "</td>";
            html += "<td style=\"background-color: #4f4f4f; font-weight: normal; color: white; width: 30px; border-top-color: #424242;\">" +
                        "<img src=\"" + ResolveUrl("~/Application_CLibs/images/svg/test.svg") + "\" style=\"height: 20px; width: 20px;\" />" +
                    "</td>";
            html += "<td style=\"padding-left: 0px; color: white; background-color: #4f4f4f; font-weight: bold; border-top-color: #424242;\">" +
                        "Komponen Penilaian <span class=\"badge\">KP</span>" +
                    "</td>";
            html += "<td style=\"background-color: #4f4f4f; border-top-color: #424242;\">" +
                    "</td>";
            html += "<td style=\"background-color: #4f4f4f; border-top-color: #424242;\">" +
                    "</td>";
            html += "<td style=\"background-color: #4f4f4f; border-top-color: #424242;\">" +
                    "</td>";
            html += "</tr>";
            //end header

            //list ap
            int id_ap = 1;
            string span_jenis_perhitungan = "";
            List<Rapor_StrukturNilai_AP> lst_aspek_penilaian = DAO_Rapor_StrukturNilai_AP.GetAllByHeader_Entity(rel_strukturnilai);
            foreach (Rapor_StrukturNilai_AP struktur_ap in lst_aspek_penilaian)
            {
                Rapor_AspekPenilaian m_ap = DAO_Rapor_AspekPenilaian.GetByID_Entity(struktur_ap.Rel_Rapor_AspekPenilaian.ToString());
                if (m_ap != null)
                {
                    if (m_ap.Nama != null)
                    {
                        if (struktur_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                        {
                            span_jenis_perhitungan = "&nbsp;&nbsp;" +
                                                     "<sup class=\"badge\" style=\"background-color: #40B3D2; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;\">" +
                                                        Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.Bobot) +
                                                     "</sup>";
                        }
                        else if (struktur_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                        {
                            span_jenis_perhitungan = "&nbsp;&nbsp;" +
                                                     "<sup class=\"badge\" style=\"background-color: #68217A; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;\">" +
                                                        Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.RataRata) +
                                                     "</sup>";
                        }

                        html += "<tr>";
                        html += "<td style=\"background-color: white; font-weight: normal; color: #bfbfbf; width: 40px;\">" +
                                    id_ap.ToString() +
                                "</td>";
                        is_sebagai_guru = false;
                        if (is_sebagai_guru)
                        {
                            html += "<td colspan=\"6\" style=\"background-color: white; font-weight: bold; color: grey; padding-left: 0px; text-align: justify;\">" +
                                        (struktur_ap.Poin.Trim() != "" ? struktur_ap.Poin + " : " : "") +
                                        Libs.GetHTMLSimpleText(m_ap.Nama) +
                                        span_jenis_perhitungan +
                                    "</td>";
                            html += "<td colspan=\"2\" style=\"background-color: white; font-weight: bold; color: grey; text-align: right;\">" +
                                    (
                                        struktur_ap.BobotRapor > 0
                                        ? "<span title=\" Bobot Rapor \" class=\"badge\" style=\"background-color: #f39100; border-radius: 0px;\">" +
                                                struktur_ap.BobotRapor.ToString() + "%" +
                                          "</span>"
                                        : ""
                                    ) +
                                "</td>";
                        }
                        else
                        {
                            html += "<td style=\"background-color: white; font-weight: normal; color: #bfbfbf; width: 30px;\">" +
                                        "<div class=\"checkbox checkbox-adv\" style=\"margin: 0 auto;\">" +
                                            "<label for=\"chk_ap_" + struktur_ap.Kode.ToString().Replace("-", "_") + "\">" +
                                                "<input value=\"" + struktur_ap.Kode.ToString() + "\" " +
                                                        "class=\"access-hide\" " +
                                                        "id=\"chk_ap_" + struktur_ap.Kode.ToString().Replace("-", "_") + "\" " +
                                                        "name=\"chk_ap[]\" " +
                                                        "type=\"checkbox\">" +
                                                "<span class=\"checkbox-circle\"></span><span class=\"checkbox-circle-check\"></span><span class=\"checkbox-circle-icon icon\">done</span>" +
                                            "</label>" +
                                        "</div>" +
                                    "</td>";
                            html += "<td colspan=\"5\" style=\"background-color: white; font-weight: bold; color: grey; padding-left: 0px; text-align: justify;\">" +
                                        (struktur_ap.Poin.Trim() != "" ? struktur_ap.Poin + " : " : "") +
                                        Libs.GetHTMLSimpleText(m_ap.Nama) +
                                        span_jenis_perhitungan +
                                    "</td>";
                            html += "<td style=\"background-color: white; font-weight: bold; color: grey; text-align: right;\">" +
                                        (
                                            struktur_ap.BobotRapor > 0
                                            ? "<span title=\" Bobot Rapor \" class=\"badge\" style=\"background-color: #f39100; border-radius: 0px;\">" +
                                                    struktur_ap.BobotRapor.ToString() + "%" +
                                              "</span>"
                                            : ""
                                        ) +
                                    "</td>";
                            html += "<td style=\"background-color: white; font-weight: normal; color: grey; text-align: right; width: 150px;\">" +
                                        "<label title=\" Tambah Kompetensi Dasar \" onclick=\"" + txtIDAspekPenilaian.ClientID + ".value = '" + struktur_ap.Kode.ToString() + "'; " + btnShowInputKompetensiDasar.ClientID + ".click();\" class=\"badge\" style=\"font-size: x-small; background-color: white; cursor: pointer; color: green; font-weight: normal; padding: 5px 7px; font-weight: normal; font-size: x-small; display: initial; border-style: solid; border-color: #a5caa5; border-width: 1px; margin-right: 5px;\">" +
                                            "&nbsp;" +
                                            "<i class=\"fa fa-plus\"></i>" +
                                            "&nbsp;&nbsp;" +
                                            "Tambah KD" +
                                            "&nbsp;" +
                                        "</label>" +
                                        "<label title=\" Ubah Aspek Penilaian \" onclick=\"" + txtIDAspekPenilaian.ClientID + ".value = '" + struktur_ap.Kode.ToString() + "'; " + btnShowInputEditAspekPenilaian.ClientID + ".click();\" title=\" Ubah \" class=\"badge\" style=\"font-size: x-small; background-color: white; cursor: pointer; color: green; font-weight: normal; padding: 5px 7px; font-weight: normal; font-size: x-small; display: initial; border-style: solid; border-color: #a5caa5; border-width: 1px;\">" +
                                            "&nbsp;" +
                                            "<i class=\"fa fa-edit\"></i>" +
                                            "&nbsp;" +
                                            "Ubah AP" +
                                            "&nbsp;" +
                                        "</label>" +
                                    "</td>";
                        }     
                        html += "</tr>";


                        //list kd
                        int id_kd = 1;
                        List<Rapor_StrukturNilai_KD> lst_kompetensi_dasar = DAO_Rapor_StrukturNilai_KD.GetAllByHeader_Entity(struktur_ap.Kode.ToString());
                        foreach (Rapor_StrukturNilai_KD struktur_kd in lst_kompetensi_dasar)
                        {
                            Rapor_KompetensiDasar kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(struktur_kd.Rel_Rapor_KompetensiDasar.ToString());
                            if (kd != null)
                            {
                                if (kd.Nama != null)
                                {
                                    html += "<tr>";
                                    html += "<td colspan=\"2\" style=\"background-color: white; padding: 0px;\">" +
                                            "</td>";
                                    html += "<td colspan=\"7\" style=\"background-color: white; padding: 0px;\">" +
                                                "<hr style=\"margin: 0px; border-color: #ebebeb; border-width: 1px;\" />" +
                                            "</td>";
                                    html += "</tr>";

                                    if (struktur_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                    {
                                        span_jenis_perhitungan = "&nbsp;&nbsp;" +
                                                                 "<sup class=\"badge\" style=\"background-color: #40B3D2; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;\">" +
                                                                    Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.Bobot) +
                                                                 "</sup>";
                                    }
                                    else if (struktur_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                    {
                                        span_jenis_perhitungan = "&nbsp;&nbsp;" +
                                                                 "<sup class=\"badge\" style=\"background-color: #68217A; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; border-radius: 3px;\">" +
                                                                    Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.RataRata) +
                                                                 "</sup>";
                                    }

                                    html += "<tr>";
                                    html += "<td colspan=\"2\" style=\"background-color: white; font-weight: normal; color: #bfbfbf; width: 40px;\">" +
                                            "</td>";
                                    if (is_sebagai_guru)
                                    {
                                        html += (
                                                    struktur_kd.BobotAP > 0
                                                    ? "<td colspan=\"4\" style=\"background-color: white; font-weight: bold; color: grey; padding-left: 0px; text-align: justify;\">" +
                                                            (struktur_kd.Poin.Trim() != "" ? struktur_kd.Poin + " : " : "") +
                                                            Libs.GetHTMLSimpleText(kd.Nama) +
                                                            span_jenis_perhitungan +
                                                      "</td>" +
                                                      "<td style=\"background-color: white; font-weight: bold; color: grey; text-align: right; padding-right: 5px;\">" +
                                                            "<span title=\" Bobot Aspek Penilaian \" class=\"badge\" style=\"background-color: #f39100; border-radius: 0px;\">" +
                                                                struktur_kd.BobotAP.ToString() + "%" +
                                                            "</span>" +
                                                      "</td>"
                                                    : "<td colspan=\"5\" style=\"background-color: white; font-weight: bold; color: grey; padding-left: 0px; text-align: justify;\">" +
                                                            (struktur_kd.Poin.Trim() != "" ? struktur_kd.Poin + " : " : "") +
                                                            Libs.GetHTMLSimpleText(kd.Nama) +
                                                            span_jenis_perhitungan +
                                                      "</td>"
                                                );
                                    }
                                    else
                                    {
                                        html += "<td style=\"background-color: white; font-weight: normal; color: #bfbfbf; width: 30px;\">" +
                                                    "<div class=\"checkbox checkbox-adv\" style=\"margin: 0 auto;\">" +
                                                        "<label for=\"chk_kd_" + struktur_kd.Kode.ToString().Replace("-", "_") + "\">" +
                                                            "<input value=\"" + struktur_kd.Kode.ToString() + "\" " +
                                                                    "class=\"access-hide\" " +
                                                                    "id=\"chk_kd_" + struktur_kd.Kode.ToString().Replace("-", "_") + "\" " +
                                                                    "name=\"chk_kd[]\" " +
                                                                    "type=\"checkbox\">" +
                                                            "<span class=\"checkbox-circle\"></span><span class=\"checkbox-circle-check\"></span><span class=\"checkbox-circle-icon icon\">done</span>" +
                                                        "</label>" +
                                                    "</div>" +
                                                "</td>";
                                        html += (
                                                    struktur_kd.BobotAP > 0
                                                    ? "<td colspan=\"3\" style=\"background-color: white; font-weight: bold; color: grey; padding-left: 0px; text-align: justify;\">" +
                                                            (struktur_kd.Poin.Trim() != "" ? struktur_kd.Poin + " : " : "") +
                                                            Libs.GetHTMLSimpleText(kd.Nama) +
                                                            span_jenis_perhitungan +
                                                      "</td>" +
                                                      "<td style=\"background-color: white; font-weight: bold; color: grey; text-align: right; padding-right: 5px;\">" +
                                                            "<span title=\" Bobot Aspek Penilaian \" class=\"badge\" style=\"background-color: #f39100; border-radius: 0px;\">" +
                                                                struktur_kd.BobotAP.ToString() + "%" +
                                                            "</span>" +
                                                      "</td>"
                                                    : "<td colspan=\"4\" style=\"background-color: white; font-weight: bold; color: grey; padding-left: 0px; text-align: justify;\">" +
                                                            (struktur_kd.Poin.Trim() != "" ? struktur_kd.Poin + " : " : "") +
                                                            Libs.GetHTMLSimpleText(kd.Nama) +
                                                            span_jenis_perhitungan +
                                                      "</td>"
                                                );
                                    }                                    
                                    html += "<td style=\"background-color: white; font-weight: normal; color: grey; text-align: " + (!is_sebagai_guru ? "right" : "left") + "; width: 150px; padding-left: 0px;\">" +
                                                "<label title=\" Komponen Penilaian \" " +
                                                       "onclick=\"" + txtIDAspekPenilaian.ClientID + ".value = '" + struktur_kd.Rel_Rapor_StrukturNilai_AP.ToString() + "'; " + 
                                                                      txtIDKompetensiDasar.ClientID + ".value = '" + struktur_kd.Kode.ToString() + "'; " +
                                                                      txtActKP.ClientID + ".value = '" + JenisActKP_ADDEDIT + "'; " +
                                                                      btnShowInputKomponenPenilaian.ClientID + ".click();\" " +
                                                       "class=\"badge\" style =\"font-size: x-small; background-color: white; cursor: pointer; color: green; font-weight: normal; padding: 5px 7px; font-weight: normal; font-size: x-small; display: initial; border-style: solid; border-color: #a5caa5; border-width: 1px; margin-right: 5px;\">" +
                                                     "&nbsp;" +
                                                    "<i class=\"fa fa-edit\"></i>" +
                                                    "&nbsp;&nbsp;" +
                                                    "Ubah KP" +
                                                    "&nbsp;" +
                                                "</label>" +
                                                (
                                                    is_sebagai_guru
                                                    ? ""
                                                    : "<label title=\" Ubah Kompetensi Dasar \" onclick=\"" + txtIDAspekPenilaian.ClientID + ".value = '" + struktur_kd.Rel_Rapor_StrukturNilai_AP.ToString() + "'; " + txtIDKompetensiDasar.ClientID + ".value = '" + struktur_kd.Kode.ToString() + "'; " + btnShowInputEditKompetensiDasar.ClientID + ".click();\" title=\" Ubah \" class=\"badge\" style=\"font-size: x-small; background-color: white; cursor: pointer; color: green; font-weight: normal; padding: 5px 7px; font-weight: normal; font-size: x-small; display: initial; border-style: solid; border-color: #a5caa5; border-width: 1px;\">" +
                                                         "&nbsp;" +
                                                        "<i class=\"fa fa-edit\"></i>" +
                                                        "&nbsp;" +
                                                        "Ubah KD" +
                                                        "&nbsp;" +
                                                      "</label>"
                                                ) +
                                            "</td>";
                                    html += "<td style=\"background-color: white; border-top-color: #424242;\">" +
                                            "</td>";
                                    html += "</tr>";

                                    //komponen penilaian
                                    int id_kp = 0;
                                    List<Rapor_StrukturNilai_KP> lst_komponen_penilaian =
                                        DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(struktur_kd.Kode.ToString());

                                    if (id_kd <= lst_kompetensi_dasar.Count && lst_komponen_penilaian.Count > 0)
                                    {
                                        html += "<tr>";
                                        html += "<td colspan=\"2\" style=\"background-color: white; padding: 0px;\">" +
                                                "</td>";
                                        html += "<td colspan=\"7\" style=\"background-color: white; padding: 0px;\">" +
                                                    "<hr style=\"margin: 0px; border-color: #ebebeb; border-width: 1px;\" />" +
                                                "</td>";
                                        html += "</tr>";
                                    }

                                    foreach (Rapor_StrukturNilai_KP m in lst_komponen_penilaian)
                                    {
                                        bool is_ada_nilai = (
                                                lst_rapor_nilai_det.FindAll(
                                                    m0 => m0.Rel_Rapor_StrukturNilai_AP.Trim().ToUpper() == struktur_ap.Kode.ToString().Trim().ToUpper() &&
                                                          m0.Rel_Rapor_StrukturNilai_KD.Trim().ToUpper() == struktur_kd.Kode.ToString().Trim().ToUpper() &&
                                                          m0.Rel_Rapor_StrukturNilai_KP.Trim().ToUpper() == m.Kode.ToString().Trim().ToUpper()
                                                ).Count > 0
                                                ? true : false
                                            );

                                        Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(m.Rel_Rapor_KomponenPenilaian.ToString());
                                        if (m_kp != null)
                                        {
                                            if (m_kp.Nama != null)
                                            {
                                                if (id_kp > 0)
                                                {
                                                    html += "<tr>";
                                                    html += "<td colspan=\"4\" style=\"background-color: white; padding: 0px;\">" +
                                                            "</td>";
                                                    html += "<td colspan=\"5\" style=\"background-color: white; padding: 0px;\">" +
                                                            "<hr style=\"margin: 0px; border-color: #ebebeb;\" />" +
                                                            "</td>";
                                                    html += "</tr>";
                                                }

                                                html += "<tr>";
                                                html += "<td colspan=\"4\" style=\"background-color: white;\">" +
                                                        "</td>";
                                                html += "<td style=\"background-color: white; font-weight: normal; color: #bfbfbf; width: 30px; vertical-align: top;\">" +
                                                            "<i class=\"fa fa-hashtag\"></i>" +
                                                        "</td>";
                                                html += "<td colspan=\"4\" style=\"padding-left: 0px; color: black; background-color: white; vertical-align: top;\">" +
                                                            "<label class=\"badge\"" +
                                                                   "onclick=\"" + txtIDKompetensiDasar.ClientID + ".value = '" + struktur_kd.Kode.ToString() + "'; " +
                                                                                  txtIDKomponenPenilaian.ClientID + ".value = '" + m.Kode.ToString() + "'; " +
                                                                                  txtActKP.ClientID + ".value = '" + JenisActKP_EDIT + "'; " +
                                                                                  btnShowInputEditKomponenPenilaian.ClientID + ".click();\" " +
                                                                   "title=\" Ubah \" " +
                                                                   "style=\"font-size: x-small; background-color: white; cursor: pointer; color: green; font-weight: normal; padding: 5px 7px; font-weight: normal; font-size: x-small; display: initial; border-style: solid; border-color: #a5caa5; border-width: 1px;\">" +
                                                                "&nbsp;" +
                                                                "<i class=\"fa fa-edit\"></i>" +
                                                                "&nbsp;" +
                                                                "Ubah" +
                                                                "&nbsp;" +
                                                            "</label>" +
                                                            "&nbsp;&nbsp;" +
                                                            (
                                                                m.KodeKD.Trim() != ""
                                                                ? "<span style=\"font-weight: normal; color: #bfbfbf;\">" +
                                                                     m.KodeKD +
                                                                     "&nbsp;&nbsp;" +
                                                                  "</span>"
                                                                : ""
                                                            ) +
                                                            "<span style=\"font-weight: bold; \">" +
                                                                Libs.GetHTMLSimpleText(m_kp.Nama) +
                                                            "</span>" +
                                                            "&nbsp;&nbsp;" +
                                                            "&nbsp;&nbsp;" +
                                                            (
                                                                m.IsAdaPB
                                                                ? "<sup title=\" Ada Perbaikan \" class=\"badge\" style=\"background-color: green; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">PB</sup>"
                                                                : ""
                                                            ) +
                                                            (
                                                                m.IsLTS
                                                                ? "<sup title=\" Tampilkan di LTS \" class=\"badge\" style=\"background-color: red; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">LTS</sup>"
                                                                : ""
                                                            ) +
                                                            (
                                                                is_ada_nilai
                                                                ? "<sup title=\" Sudah Isi Nilai \" class=\"badge\" style=\"background-color: #a0a000; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">Sudah Isi Nilai</sup>"
                                                                : ""
                                                            ) +
                                                            (
                                                                m.BobotNK > 0
                                                                ? "<sup class=\"badge\" style=\"background-color: #f39100; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">" + m.BobotNK.ToString() + "%</sup>"
                                                                : ""
                                                            ) +
                                                            (
                                                                Libs.GetHTMLSimpleText(m.Materi.Trim()) != "" && Libs.GetHTMLSimpleText(m.Deskripsi.Trim()) != ""
                                                                ? (
                                                                        Libs.GetHTMLSimpleText(m.Materi.Trim()) != ""
                                                                        ? "<div style=\"padding: 10px; border-color: #dcecf5; border-width: 1px; border-radius: 5px; background-color: #E7F7FF; border-style: solid; margin-top: 10px; border-bottom-left-radius: 0px; border-bottom-right-radius: 0px;\">" +
                                                                              "<label style=\"font-weight: bold; color: grey;\"><i class=\"fa fa-info-circle\"></i>&nbsp;Materi</label>" +
                                                                              "<br />" +
                                                                              Libs.GetHTMLSimpleText(m.Materi.Trim()) +
                                                                          "</div>"
                                                                        : ""
                                                                  ) +
                                                                  (
                                                                        Libs.GetHTMLSimpleText(m.Deskripsi.Trim()) != ""
                                                                        ? "<div style=\"padding: 10px; border-color: #dcecf5; border-width: 1px; border-radius: 5px; background-color: #E7F7FF; border-style: solid; margin-top: 0px; border-top-left-radius: 0px; border-top-right-radius: 0px; border-top-style: none;\">" +
                                                                              "<label style=\"font-weight: bold; color: grey;\"><i class=\"fa fa-info-circle\"></i>&nbsp;Deskripsi</label>" +
                                                                              "<br />" +
                                                                              Libs.GetHTMLSimpleText(m.Deskripsi.Trim()) +
                                                                          "</div>"
                                                                        : ""
                                                                  )
                                                                : (
                                                                        Libs.GetHTMLSimpleText(m.Materi.Trim()) != ""
                                                                        ? "<div style=\"padding: 10px; border-color: #dcecf5; border-width: 1px; border-radius: 5px; background-color: #E7F7FF; border-style: solid; margin-top: 10px;\">" +
                                                                              "<label style=\"font-weight: bold; color: grey;\"><i class=\"fa fa-info-circle\"></i>&nbsp;Materi</label>" +
                                                                              "<br />" +
                                                                              Libs.GetHTMLSimpleText(m.Materi.Trim()) +
                                                                          "</div>"
                                                                        : ""
                                                                  ) +
                                                                  (
                                                                        Libs.GetHTMLSimpleText(m.Deskripsi.Trim()) != ""
                                                                        ? "<div style=\"padding: 10px; border-color: #dcecf5; border-width: 1px; border-radius: 5px; background-color: #E7F7FF; border-style: solid; margin-top: 10px;\">" +
                                                                              "<label style=\"font-weight: bold; color: grey;\"><i class=\"fa fa-info-circle\"></i>&nbsp;Deskripsi</label>" +
                                                                              "<br />" +
                                                                              Libs.GetHTMLSimpleText(m.Deskripsi.Trim()) +
                                                                          "</div>"
                                                                        : ""
                                                                  )
                                                            ) +
                                                        "</td>";
                                                html += "</tr>";
                                            }
                                        }
                                        id_kp++;
                                    }
                                    id_kd++;
                                }
                            }
                        }

                        if (id_ap < lst_aspek_penilaian.Count)
                        {
                            html += "<tr>";
                            html += "<td colspan=\"9\" style=\"background-color: white; padding: 0px;\">" +
                                        "<hr style=\"margin: 0px; border-color: #ebebeb; border-width: 2px;\" />" +
                                    "</td>";
                            html += "</tr>";
                        }

                        id_ap++;
                    }
                }
            }

            if (lst_aspek_penilaian.Count == 0 || rel_strukturnilai.Trim() == "")
            {
                ltrStrukturNilaiDet.Text = "<div style=\"padding: 10px;\"><label style=\"margin: 0 auto; display: table;\">..:: Data Kosong ::..</label></div>";
            }
            else
            {
                html = "<table style=\"margin: 0px; width: 100%;\">" +
                       html +
                       "</table>";
                ltrStrukturNilaiDet.Text = html;
            }
        }

        protected void lnkOKKompetensiDasar_Click(object sender, EventArgs e)
        {
            if (txtIDKompetensiDasar.Value.Trim() == "")
            {
                DAO_Rapor_StrukturNilai_KD.Insert(
                    new Rapor_StrukturNilai_KD
                    {
                        Poin = txtPoinKompetensiDasar.Text,
                        Rel_Rapor_StrukturNilai_AP = new Guid(txtIDAspekPenilaian.Value),
                        Rel_Rapor_KompetensiDasar = new Guid(txtKompetensiDasar.Value),
                        BobotAP = Libs.GetStringToDecimal(txtBobotAP.Text),
                        JenisPerhitungan = cboJenisPerhitunganKompetensiDasar.SelectedValue
                    }, Libs.LOGGED_USER_M.UserID);

                ShowStrukturNilai(txtID.Value);
                InitFieldsInputKompetensiDasar();
                txtKeyAction.Value = JenisAction.AddKDWithMessage.ToString();
            }
            else
            {
                DAO_Rapor_StrukturNilai_KD.Update(
                    new Rapor_StrukturNilai_KD
                    {
                        Kode = new Guid(txtIDKompetensiDasar.Value),
                        Poin = txtPoinKompetensiDasar.Text,
                        Rel_Rapor_StrukturNilai_AP = new Guid(txtIDAspekPenilaian.Value),
                        Rel_Rapor_KompetensiDasar = new Guid(txtKompetensiDasar.Value),
                        BobotAP = Libs.GetStringToDecimal(txtBobotAP.Text),
                        JenisPerhitungan = cboJenisPerhitunganKompetensiDasar.SelectedValue
                    }, Libs.LOGGED_USER_M.UserID);

                ShowStrukturNilai(txtID.Value);
                InitFieldsInputKompetensiDasar();
                txtKeyAction.Value = JenisAction.DoUpdate.ToString();
            }
        }

        protected void InitFieldsInputKompetensiDasar(bool hapus_id = true)
        {
            if (hapus_id) txtIDKompetensiDasar.Value = "";
            txtPoinKompetensiDasar.Text = "";
            txtKompetensiDasar.Value = "";
            cboJenisPerhitunganKompetensiDasar.SelectedValue = "";
            txtBobotAP.Text = "";
        }

        protected void btnShowInputKompetensiDasar_Click(object sender, EventArgs e)
        {
            InitFieldsInputKompetensiDasar();
            if (txtIDAspekPenilaian.Value.Trim() != "")
            {
                Rapor_StrukturNilai_AP m_struktur_ap = DAO_Rapor_StrukturNilai_AP.GetByID_Entity(txtIDAspekPenilaian.Value);
                if (m_struktur_ap != null)
                {
                    if (m_struktur_ap.JenisPerhitungan != null)
                    {
                        div_bobot_ap_dari_kd.Visible = false;
                        if (m_struktur_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                        {
                            div_bobot_ap_dari_kd.Visible = true;
                        }
                        txtKeyAction.Value = JenisAction.DoShowInputKompetensiDasar.ToString();
                    }
                }
            }
        }

        protected void lnkOKKomponenPenilaian_Click(object sender, EventArgs e)
        {
            if (txtIDKompetensiDasar.Value.Trim() != "")
            {
                List<Rapor_StrukturNilai_KP> lst_struktur_kp = DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(txtIDKompetensiDasar.Value);
                string[] arr_kode = txtParseListKomponenPenilaian.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                if (txtParseListKomponenPenilaian.Value.Trim() != "")
                {
                    if (txtIDKomponenPenilaian.Value.Trim() == "")
                    {
                        //delete sisanya
                        foreach (var item in lst_struktur_kp)
                        {
                            if (arr_kode.ToList().FindAll(m => m.ToUpper().Trim() == item.Rel_Rapor_KomponenPenilaian.ToString().ToUpper().Trim()).Count == 0)
                            {
                                DAO_Rapor_StrukturNilai_KP.Delete(item.Kode.ToString(), Libs.LOGGED_USER_M.NoInduk);
                            }
                        }

                        foreach (string kode_kp in arr_kode)
                        {
                            if (lst_struktur_kp.FindAll(m => m.Rel_Rapor_KomponenPenilaian == new Guid(kode_kp)).Count == 0)
                            {
                                DAO_Rapor_StrukturNilai_KP.Insert(
                                    new Rapor_StrukturNilai_KP
                                    {
                                        Rel_Rapor_StrukturNilai_KD = new Guid(txtIDKompetensiDasar.Value),
                                        Rel_Rapor_KomponenPenilaian = new Guid(kode_kp),
                                        Jenis = "",
                                        BobotNK = Libs.GetStringToDecimal(txtBobotNKD.Text),
                                        IsAdaPB = chkIsAdaPB.Checked
                                    }, Libs.LOGGED_USER_M.UserID
                                );
                            }
                            else
                            {
                                Guid kode_update = lst_struktur_kp.FindAll(m => m.Rel_Rapor_KomponenPenilaian == new Guid(kode_kp)).FirstOrDefault().Kode;
                                Rapor_StrukturNilai_KP m_rapor_kp = DAO_Rapor_StrukturNilai_KP.GetByID_Entity(kode_update.ToString());

                                if (m_rapor_kp != null)
                                {
                                    if (m_rapor_kp.Jenis != null)
                                    {
                                        DAO_Rapor_StrukturNilai_KP.Update(
                                            new Rapor_StrukturNilai_KP
                                            {
                                                Kode = kode_update,
                                                Rel_Rapor_StrukturNilai_KD = new Guid(txtIDKompetensiDasar.Value),
                                                Rel_Rapor_KomponenPenilaian = new Guid(kode_kp),
                                                Jenis = "",
                                                BobotNK = Libs.GetStringToDecimal(txtBobotNKD.Text),
                                                IsAdaPB = chkIsAdaPB.Checked,
                                                Deskripsi = m_rapor_kp.Deskripsi,
                                                Materi = m_rapor_kp.Materi,
                                                KodeKD = m_rapor_kp.KodeKD
                                            }, Libs.LOGGED_USER_M.UserID
                                        );
                                    }
                                }                                
                            }                            
                        }
                        
                        ShowStrukturNilai(txtID.Value);
                        InitFieldsInputKomponenPenilaian();
                        txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                    }
                    else
                    {
                        //delete sisanya
                        if (txtActKP.Value != JenisActKP_EDIT)
                        {
                            foreach (var item in lst_struktur_kp)
                            {
                                if (arr_kode.ToList().FindAll(m => m.ToUpper().Trim() == item.Rel_Rapor_KomponenPenilaian.ToString().ToUpper().Trim()).Count == 0)
                                {
                                    DAO_Rapor_StrukturNilai_KP.Delete(item.Kode.ToString(), Libs.LOGGED_USER_M.NoInduk);
                                }
                            }
                        }

                        foreach (string kode_kp in arr_kode)
                        {
                            DAO_Rapor_StrukturNilai_KP.Update(
                                new Rapor_StrukturNilai_KP
                                {
                                    Kode = new Guid(txtIDKomponenPenilaian.Value),
                                    Rel_Rapor_StrukturNilai_KD = new Guid(txtIDKompetensiDasar.Value),
                                    Rel_Rapor_KomponenPenilaian = new Guid(kode_kp),
                                    Jenis = "",
                                    BobotNK = Libs.GetStringToDecimal(txtBobotNKD.Text),
                                    IsAdaPB = chkIsAdaPB.Checked,
                                    IsLTS = chkIsLTS.Checked,
                                    Materi = txtMateriKPVal.Value,
                                    Deskripsi = txtDeskripsiKPVal.Value,
                                    KodeKD = txtKodeKD.Text
                                }, Libs.LOGGED_USER_M.UserID
                            );
                        }
                        
                        ShowStrukturNilai(txtID.Value);
                        InitFieldsInputKomponenPenilaian();
                        txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                    }
                }

            }
        }

        protected void InitFieldsInputKomponenPenilaian(bool hapus_id = true)
        {
            if (hapus_id) txtIDKomponenPenilaian.Value = "";

            //list komponen penilaian
            List<Rapor_KomponenPenilaian> lst_kp = DAO_Rapor_KomponenPenilaian.GetAll_Entity();
            ltrKomponenPenilaian.Text = "";
            int id = 1;

            List<Rapor_StrukturNilai_KP> lst_struktur_kp = DAO_Rapor_StrukturNilai_KP.GetAllByHeader_Entity(txtIDKompetensiDasar.Value);

            int nomor = 1;
            int nomor_ada_nilai = 0;
            foreach (var item in lst_kp)
            {
                if (lst_struktur_kp.FindAll(m => m.Rel_Rapor_KomponenPenilaian == item.Kode).Count > 0)
                {
                    Rapor_StrukturNilai_KP m_sn_kp = lst_struktur_kp.FindAll(m => m.Rel_Rapor_KomponenPenilaian == item.Kode).FirstOrDefault();
                    if (m_sn_kp != null)
                    {
                        if (m_sn_kp.Materi != null)
                        {

                            Rapor_StrukturNilai_KD m_sn_kd = DAO_Rapor_StrukturNilai_KD.GetByID_Entity(m_sn_kp.Rel_Rapor_StrukturNilai_KD.ToString());
                            if (m_sn_kd != null)
                            {

                                if (m_sn_kd.Poin != null)
                                {

                                    bool is_ada_nilai = (
                                            DAO_Rapor_NilaiSiswa_Det.GetAllByStrukturNilai_Entity(
                                                m_sn_kd.Rel_Rapor_StrukturNilai_AP.ToString(),
                                                m_sn_kd.Kode.ToString(),
                                                m_sn_kp.Kode.ToString()
                                            ).Count > 0
                                            ? true : false
                                        );
                                    if (is_ada_nilai) nomor_ada_nilai = nomor;

                                }

                            }

                        }
                    }
                }
                nomor++;
            }

            nomor = 1;
            foreach (var item in lst_kp)
            {
                string id_chk = "chk_" + 
                                item.Kode.ToString().Replace("-", "_");

                ltrKomponenPenilaian.Text += "<div class=\"row\" style=\"margin-left: 15px; margin-right: 15px;\">" +
                                                "<div class=\"col-xs-12\">" +
					                                "<div class=\"form-group form-group-label\" style=\"margin-top: 0px; margin-bottom: 0px;\">" +
						                                "<label for=\"" + id_chk + "\" style=\"color: #B7770D; font-size: small;\">" +
                                                            "<div class=\"checkbox switch\" style=\"padding-bottom: 0px;\">" +
									                            "<label for=\"" + id_chk + "\" style=\"color: grey; font-weight: bold;\">" +
                                                                    "<input onchange=\"SelectKomponenPenilaian(this.lang, this.checked)\" " +
                                                                        (
                                                                            lst_struktur_kp.FindAll(m=>m.Rel_Rapor_KomponenPenilaian == item.Kode).Count > 0
                                                                            ? " checked "
                                                                            : ""
                                                                        ) +
                                                                        "lang=\"" + 
                                                                        (
                                                                            Libs.IsAngka(Libs.GetHTMLSimpleText(item.Nama))
                                                                            ? id.ToString()
                                                                            : "0"
                                                                        ) + "\" value=\"" + item.Kode.ToString() + "\" id=\"" + id_chk + "\" " +
                                                                            "class=\"access-hide\" " +
                                                                            "name=\"chkIsAdaPB[]\" " +
                                                                            "type=\"checkbox\" " +
                                                                            (
                                                                                nomor > nomor_ada_nilai
                                                                                ? ""
                                                                                : "" //"disabled=\"disabled\" "
                                                                            ) +
                                                                    "/>" +
										                            "<span class=\"switch-toggle\"></span>" +
                                                                    Libs.GetHTMLSimpleText(item.Nama) +
                                                                    (
                                                                        nomor > nomor_ada_nilai
                                                                        ? ""
                                                                        : "&nbsp;&nbsp;<sup title=\" Sudah Isi Nilai \" class=\"badge\" style=\"background-color: #a0a000; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">Sudah Isi Nilai</sup>"
                                                                    ) +
									                            "</label>" +
								                            "</div>" +
						                                "</label>" +
					                                "</div>" +
				                                "</div>" +
                                            "</div>";

                nomor++;
                id++;
            }
            //end list

            if (ltrKomponenPenilaian.Text.Trim() != "")
            {
                Rapor_StrukturNilai_KD _m_kp = DAO_Rapor_StrukturNilai_KD.GetByID_Entity(txtIDKompetensiDasar.Value);

                if (_m_kp != null)
                {

                    if (_m_kp.Poin != null)
                    {

                        Rapor_StrukturNilai_AP _m_ap = DAO_Rapor_StrukturNilai_AP.GetByID_Entity(_m_kp.Rel_Rapor_StrukturNilai_AP.ToString());
                        if (_m_ap != null)
                        {

                            if (_m_ap.Poin != null)
                            {

                                Rapor_AspekPenilaian m_ap = DAO_Rapor_AspekPenilaian.GetByID_Entity(_m_ap.Rel_Rapor_AspekPenilaian.ToString());
                                if (m_ap != null)
                                {

                                    if (m_ap.Nama != null)
                                    {

                                        Rapor_KompetensiDasar m_kp = DAO_Rapor_KompetensiDasar.GetByID_Entity(_m_kp.Rel_Rapor_KompetensiDasar.ToString());
                                        if (m_kp != null)
                                        {

                                            if (m_kp.Nama != null)
                                            {

                                                string s_label = "";

                                                s_label = "<span class=\"badge\">" + Libs.GetHTMLSimpleText(m_ap.Nama) + "</span>" +
                                                          "&nbsp;" +
                                                          "<i class=\"fa fa-arrow-right\"></i>" +
                                                          "&nbsp;" +
                                                          "<span class=\"badge\">" + Libs.GetHTMLSimpleText(m_kp.Nama) + "</span>" +
                                                          "<br /><br />";

                                                ltrKomponenPenilaian.Text = s_label +
                                                                            "<label class=\"label-input\" style=\"text-transform: none;\">Jumlah Kegiatan</label>" +
                                                                            ltrKomponenPenilaian.Text;

                                            }

                                        }

                                    }

                                }

                            }

                        }

                    }

                }
            }

            txtBobotNKD.Text = "";
        }

        protected void ShowInputKomponenPenilaian()
        {
            InitFieldsInputKomponenPenilaian();
            if (txtIDKompetensiDasar.Value.Trim() != "")
            {
                Rapor_StrukturNilai_KD m_struktur_kd = DAO_Rapor_StrukturNilai_KD.GetByID_Entity(txtIDKompetensiDasar.Value);
                if (m_struktur_kd != null)
                {
                    if (m_struktur_kd.Poin != null)
                    {
                        div_bobot_kd_dari_kp.Visible = false;
                        if (m_struktur_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                        {
                            div_bobot_kd_dari_kp.Visible = true;
                        }
                        chkIsAdaPB.Checked = false;
                        txtKeyAction.Value = JenisAction.DoShowInputKomponenPenilaian.ToString();
                    }
                }
            }
        }

        protected void btnShowInputKomponenPenilaian_Click(object sender, EventArgs e)
        {
            div_tampilan_lts.Visible = false;
            div_deskripsi_kp.Visible = false;
            div_kode_kd.Visible = false;
            div_materi_kp.Visible = false;
            ShowInputKomponenPenilaian();
            lnkOKKomponenPenilaian.Attributes["onclick"] = "return ListSelectedKomponenPenilaian();";
        }

        protected void btnShowInputEditKompetensiDasar_Click(object sender, EventArgs e)
        {
            InitFieldsInputKompetensiDasar(false);
            if (txtIDKompetensiDasar.Value.Trim() != "")
            {
                Rapor_StrukturNilai_KD m_struktur_kd = DAO_Rapor_StrukturNilai_KD.GetByID_Entity(txtIDKompetensiDasar.Value);
                if (m_struktur_kd != null)
                {
                    if (m_struktur_kd.Poin != null)
                    {
                        div_bobot_ap_dari_kd.Visible = false;
                        Rapor_StrukturNilai_AP m_struktur_ap = DAO_Rapor_StrukturNilai_AP.GetByID_Entity(m_struktur_kd.Rel_Rapor_StrukturNilai_AP.ToString());
                        if (m_struktur_ap != null)
                        {
                            if (m_struktur_ap.Poin != null)
                            {
                                if (m_struktur_ap.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                {
                                    div_bobot_ap_dari_kd.Visible = true;
                                }
                            }
                        }

                        txtPoinKompetensiDasar.Text = m_struktur_kd.Poin;
                        txtKompetensiDasar.Value = m_struktur_kd.Rel_Rapor_KompetensiDasar.ToString();
                        cboJenisPerhitunganKompetensiDasar.SelectedValue = m_struktur_kd.JenisPerhitungan;
                        txtBobotAP.Text = m_struktur_kd.BobotAP.ToString();
                        txtKeyAction.Value = JenisAction.DoShowInputKompetensiDasar.ToString();
                    }
                }
            }
        }

        protected void btnShowInputEditKomponenPenilaian_Click(object sender, EventArgs e)
        {
            InitFieldsInputKomponenPenilaian(false);
            lnkOKKomponenPenilaian.Attributes["onclick"] = "";
            if (txtIDKomponenPenilaian.Value.Trim() != "")
            {
                bool is_ada_nilai = false;
                Rapor_StrukturNilai_KP m = DAO_Rapor_StrukturNilai_KP.GetByID_Entity(txtIDKomponenPenilaian.Value);
                if (m != null)
                {
                    if (m.Jenis != null)
                    {
                        txtParseListKomponenPenilaian.Value = m.Rel_Rapor_KomponenPenilaian.ToString() + ";";
                        
                        Rapor_StrukturNilai_KD m_struktur_kd = DAO_Rapor_StrukturNilai_KD.GetByID_Entity(txtIDKompetensiDasar.Value);
                        div_bobot_kd_dari_kp.Visible = false;
                        if (m_struktur_kd != null)
                        {
                            if (m_struktur_kd.Poin != null)
                            {
                                if (m_struktur_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                {
                                    div_bobot_kd_dari_kp.Visible = true;
                                }
                            }
                        }

                        string nama_kp = "";
                        Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(m.Rel_Rapor_KomponenPenilaian.ToString());
                        if (m_kp != null)
                        {
                            if (m_kp.Nama != null)
                            {
                                nama_kp = Libs.GetHTMLSimpleText(m_kp.Nama);
                            }
                        }

                        Rapor_StrukturNilai_KD _m_kd = DAO_Rapor_StrukturNilai_KD.GetByID_Entity(txtIDKompetensiDasar.Value);

                        if (_m_kd != null)
                        {

                            if (_m_kd.Poin != null)
                            {

                                Rapor_StrukturNilai_AP _m_ap = DAO_Rapor_StrukturNilai_AP.GetByID_Entity(_m_kd.Rel_Rapor_StrukturNilai_AP.ToString());
                                if (_m_ap != null)
                                {

                                    if (_m_ap.Poin != null)
                                    {

                                        Rapor_AspekPenilaian m_ap_ = DAO_Rapor_AspekPenilaian.GetByID_Entity(_m_ap.Rel_Rapor_AspekPenilaian.ToString());
                                        if (m_ap_ != null)
                                        {

                                            if (m_ap_.Nama != null)
                                            {

                                                Rapor_KompetensiDasar m_kp_ = DAO_Rapor_KompetensiDasar.GetByID_Entity(_m_kd.Rel_Rapor_KompetensiDasar.ToString());
                                                if (m_kp_ != null)
                                                {

                                                    if (m_kp_.Nama != null)
                                                    {

                                                        string s_label = "";

                                                        s_label = "<span class=\"badge\">" + Libs.GetHTMLSimpleText(m_ap_.Nama) + "</span>" +
                                                                  "&nbsp;" +
                                                                  "<i class=\"fa fa-arrow-right\"></i>" +
                                                                  "&nbsp;" +
                                                                  "<span class=\"badge\">" + Libs.GetHTMLSimpleText(m_kp_.Nama) + "</span>" +
                                                                  "<br /><br />";

                                                        ltrKomponenPenilaian.Text = s_label +
                                                                                    "<label class=\"label-input\" style=\"text-transform: none;\">Kegiatan Ke</label>" +
                                                                                    "<div class=\"row\">" +
                                                                                        "<div class=\"col-xs-6\">" +
                                                                                            "<div class=\"form-group form-group-label\" style=\"margin-bottom: 5px; padding-bottom: 5px; margin-top: 10px;\">" +
                                                                                                "<input disabled value=\"" + nama_kp + "\" type=\"text\" class=\"form-control\" id=\"txtKP\" />" +
                                                                                            "</div>" +
                                                                                        "</div>" +
                                                                                    "</div>";

                                                    }

                                                }

                                            }

                                        }

                                    }

                                }

                                is_ada_nilai = (
                                    DAO_Rapor_NilaiSiswa_Det.GetAllByStrukturNilai_Entity(
                                        _m_ap.Kode.ToString(),
                                        _m_kd.Kode.ToString(),
                                        m.Kode.ToString()
                                    ).Count > 0
                                    ? true : false
                                );

                            }

                        }
                        
                        txtBobotNKD.Text = m.BobotNK.ToString();
                        chkIsAdaPB.Checked = m.IsAdaPB;
                        chkIsLTS.Checked = m.IsLTS;
                        //chkIsAdaPB.Disabled = is_ada_nilai;
                        txtMateriKP.Text = m.Materi;
                        txtMateriKPVal.Value = m.Materi;
                        txtDeskripsiKP.Text = m.Deskripsi;
                        txtDeskripsiKPVal.Value = m.Deskripsi;
                        txtKodeKD.Text = m.KodeKD;
                        div_tampilan_lts.Visible = true;
                        div_deskripsi_kp.Visible = true;
                        div_kode_kd.Visible = true;
                        div_materi_kp.Visible = true;
                        txtKeyAction.Value = JenisAction.DoShowInputKomponenPenilaian.ToString();
                    }
                }
            }
        }

        protected void lnkOKHapusItemStrukturPenilaian_Click(object sender, EventArgs e)
        {
            bool ada_data = false;
            if (txtParseIDAspekPenilaian.Value.Trim() != "")
            {
                string[] arr_sel_aspek_penilaian = txtParseIDAspekPenilaian.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in arr_sel_aspek_penilaian)
                {
                    DAO_Rapor_StrukturNilai_AP.Delete(item, Libs.LOGGED_USER_M.UserID);
                    ada_data = true;
                }
            }

            if (txtParseIDKompetensiDasar.Value.Trim() != "")
            {
                string[] arr_sel_kompetensi_dasar = txtParseIDKompetensiDasar.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in arr_sel_kompetensi_dasar)
                {
                    DAO_Rapor_StrukturNilai_KD.Delete(item, Libs.LOGGED_USER_M.UserID);
                    ada_data = true;
                }
            }

            if (txtParseIDKomponenPenilaian.Value.Trim() != "")
            {
                string[] arr_sel_komponen_penilaian = txtParseIDKomponenPenilaian.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in arr_sel_komponen_penilaian)
                {
                    DAO_Rapor_StrukturNilai_KP.Delete(item, Libs.LOGGED_USER_M.UserID);
                    ada_data = true;
                }
            }

            if (ada_data)
            {
                ShowStrukturNilai(txtID.Value);
                txtParseIDAspekPenilaian.Value = "";
                txtParseIDKompetensiDasar.Value = "";
                txtParseIDKomponenPenilaian.Value = "";
                txtKeyAction.Value = JenisAction.DoDelete.ToString();
            }
        }

        protected void lnkOKAspekPenilaian_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                if (txtIDAspekPenilaian.Value.Trim() == "")
                {
                    DAO_Rapor_StrukturNilai_AP.Insert(
                        new Rapor_StrukturNilai_AP
                        {
                            Poin = txtPoinAspekPenilaian.Text,
                            Rel_Rapor_StrukturNilai = new Guid(txtID.Value),
                            Rel_Rapor_AspekPenilaian = new Guid(txtAspekPenilaian.Value),
                            JenisPerhitungan = cboJenisPerhitunganAspekPenilaian.SelectedValue,
                            BobotRapor = Libs.GetStringToDecimal(txtBobotRapor.Text)
                        },
                        Libs.LOGGED_USER_M.UserID
                    );

                    ShowStrukturNilai(txtID.Value);
                    InitFieldsInputAspekPenilaian();
                    txtKeyAction.Value = JenisAction.AddAPWithMessage.ToString();
                }
                else
                {
                    DAO_Rapor_StrukturNilai_AP.Update(
                        new Rapor_StrukturNilai_AP
                        {
                            Kode = new Guid(txtIDAspekPenilaian.Value),
                            Poin = txtPoinAspekPenilaian.Text,
                            Rel_Rapor_StrukturNilai = new Guid(txtID.Value),
                            Rel_Rapor_AspekPenilaian = new Guid(txtAspekPenilaian.Value),
                            JenisPerhitungan = cboJenisPerhitunganAspekPenilaian.SelectedValue,
                            BobotRapor = Libs.GetStringToDecimal(txtBobotRapor.Text)
                        },
                        Libs.LOGGED_USER_M.UserID
                    );

                    ShowStrukturNilai(txtID.Value);
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
            }
        }

        protected void InitFieldsInputAspekPenilaian(bool hapus_id = true)
        {
            if (hapus_id) txtIDAspekPenilaian.Value = "";
            txtPoinAspekPenilaian.Text = "";
            txtAspekPenilaian.Value = "";
            cboJenisPerhitunganAspekPenilaian.SelectedValue = "";
            txtBobotRapor.Text = "";
        }

        protected void ShowInputAspekPenilaian()
        {
            InitFieldsInputAspekPenilaian();
            if (txtID.Value.Trim() != "")
            {
                Rapor_StrukturNilai m_struktur = DAO_Rapor_StrukturNilai.GetByID_Entity(txtID.Value);
                if (m_struktur != null)
                {
                    if (m_struktur.JenisPerhitungan != null)
                    {
                        //get struktur nilai
                        if (txtIDAspekPenilaian.Value.Trim() != "")
                        {
                            Rapor_StrukturNilai_AP m_struktur_ap = DAO_Rapor_StrukturNilai_AP.GetByID_Entity(txtIDAspekPenilaian.Value);
                            if (m_struktur_ap != null)
                            {
                                if (m_struktur_ap.JenisPerhitungan != null)
                                {
                                    txtPoinAspekPenilaian.Text = m_struktur_ap.Poin;
                                    txtAspekPenilaian.Value = m_struktur_ap.Rel_Rapor_AspekPenilaian.ToString();
                                    cboJenisPerhitunganAspekPenilaian.SelectedValue = m_struktur_ap.JenisPerhitungan.ToString(); ;
                                    txtBobotRapor.Text = m_struktur_ap.BobotRapor.ToString();
                                }
                            }
                        }
                        //end get struktur nilai

                        div_bobot_rapor_dari_ap.Visible = false;
                        if (m_struktur.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                        {
                            div_bobot_rapor_dari_ap.Visible = true;
                        }
                        txtKeyAction.Value = JenisAction.DoShowInputAspekPenilaian.ToString();
                    }
                }
            }
        }

        protected void btnShowInputAspekPenilaian_Click(object sender, EventArgs e)
        {
            ShowInputAspekPenilaian();
        }

        protected void btnShowInputEditAspekPenilaian_Click(object sender, EventArgs e)
        {
            InitFieldsInputAspekPenilaian(false);
            if (txtIDAspekPenilaian.Value.Trim() != "")
            {
                Rapor_StrukturNilai_AP m_struktur_ap = DAO_Rapor_StrukturNilai_AP.GetByID_Entity(txtIDAspekPenilaian.Value);
                if (m_struktur_ap != null)
                {
                    if (m_struktur_ap.Poin != null)
                    {
                        div_bobot_rapor_dari_ap.Visible = false;
                        Rapor_StrukturNilai m_struktur = DAO_Rapor_StrukturNilai.GetByID_Entity(m_struktur_ap.Rel_Rapor_StrukturNilai.ToString());
                        if (m_struktur != null)
                        {
                            if (m_struktur.TahunAjaran != null)
                            {
                                if (m_struktur.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                {
                                    div_bobot_rapor_dari_ap.Visible = true;
                                }
                            }
                        }

                        txtPoinAspekPenilaian.Text = m_struktur_ap.Poin;
                        txtAspekPenilaian.Value = m_struktur_ap.Rel_Rapor_AspekPenilaian.ToString();
                        cboJenisPerhitunganAspekPenilaian.SelectedValue = m_struktur_ap.JenisPerhitungan;
                        txtBobotRapor.Text = m_struktur_ap.BobotRapor.ToString();
                        txtKeyAction.Value = JenisAction.DoShowInputAspekPenilaian.ToString();
                    }
                }
            }
        }

        protected void lnkOKDeskripsiKURTILAS_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                DAO_Rapor_StrukturNilai.UpdateDeskripsiKURTILAS(txtID.Value, txtDeskripsiPengetahuanVal.Value, txtDeskripsiKeterampilanVal.Value);
                //BindListView(!IsPostBack, Libs.GetQ().Trim());
                txtKeyAction.Value = JenisAction.DoUpdate.ToString();
            }
        }

        protected void lnkOKDeskripsiKTSP_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                DAO_Rapor_StrukturNilai.UpdateDeskripsiKTSP(txtID.Value, txtDeskripsiMapelVal.Value);
                //BindListView(!IsPostBack, Libs.GetQ().Trim());
                txtKeyAction.Value = JenisAction.DoUpdate.ToString();
            }
        }

        protected void btnShowInputEditDeskripsiKURTILAS_Click(object sender, EventArgs e)
        {
            txtDeskripsiMapel.Text = "";
            txtDeskripsiPengetahuan.Text = "";
            txtDeskripsiPengetahuanVal.Value = "";
            txtDeskripsiKeterampilan.Text = "";
            txtDeskripsiKeterampilanVal.Value = "";

            if (txtID.Value.Trim() != "")
            {
                Rapor_StrukturNilai m_sn = DAO_Rapor_StrukturNilai.GetByID_Entity(
                        txtID.Value
                    );
                if (m_sn != null)
                {
                    if (m_sn.TahunAjaran != null)
                    {
                        if (m_sn.Rel_Kelas.ToString() != Constantas.GUID_NOL)
                        {
                            cboKelasKURTILAS.SelectedValue = m_sn.Rel_Kelas.ToString();
                        }
                        else if (m_sn.Rel_Kelas2.ToString() != Constantas.GUID_NOL)
                        {
                            cboKelasKURTILAS.SelectedValue = m_sn.Rel_Kelas2.ToString();
                        }
                        else if (m_sn.Rel_Kelas3.ToString() != Constantas.GUID_NOL)
                        {
                            cboKelasKURTILAS.SelectedValue = m_sn.Rel_Kelas3.ToString();
                        }

                        txtKelasKurtilas.Text = DAO_Rapor_StrukturNilai.GetNamaKelasEkskul(m_sn.Kode.ToString());
                        cboMapelKURTILAS.SelectedValue = m_sn.Rel_Mapel.ToString();
                        txtDeskripsiPengetahuan.Text = m_sn.DeskripsiPengetahuan;
                        txtDeskripsiPengetahuanVal.Value = m_sn.DeskripsiPengetahuan;
                        txtDeskripsiKeterampilan.Text = m_sn.DeskripsiKeterampilan;
                        txtDeskripsiKeterampilanVal.Value = m_sn.DeskripsiKeterampilan;
                        txtKeyAction.Value = JenisAction.DoShowInputDeskripsiKURTILAS.ToString();
                    }
                }                
            }            
        }

        protected void btnShowInputEditDeskripsiKTSP_Click(object sender, EventArgs e)
        {
            txtDeskripsiMapel.Text = "";
            txtDeskripsiMapelVal.Value = "";
            txtDeskripsiPengetahuan.Text = "";
            txtDeskripsiPengetahuanVal.Value = "";
            txtDeskripsiKeterampilan.Text = "";
            txtDeskripsiKeterampilanVal.Value = "";

            if (txtID.Value.Trim() != "")
            {
                Rapor_StrukturNilai m_sn = DAO_Rapor_StrukturNilai.GetByID_Entity(
                        txtID.Value
                    );
                if (m_sn != null)
                {
                    if (m_sn.TahunAjaran != null)
                    {
                        cboKelasKTSP.SelectedValue = m_sn.Rel_Kelas.ToString();
                        txtKelasKTSP.Text = DAO_Rapor_StrukturNilai.GetNamaKelasEkskul(m_sn.Kode.ToString());
                        cboMapelKTSP.SelectedValue = m_sn.Rel_Mapel.ToString();
                        txtDeskripsiMapel.Text = m_sn.DeskripsiUmum;
                        txtDeskripsiMapelVal.Value = m_sn.DeskripsiUmum;
                        txtKeyAction.Value = JenisAction.DoShowInputDeskripsiKTSP.ToString();
                    }
                }
            }            
        }

        protected void btnBukaSemester_Click(object sender, EventArgs e)
        {
            DAO_Rapor_StrukturNilai.TahunAjaranSemester m = DAO_Rapor_StrukturNilai.GetNextPeriode_Entity();
            DAO_Rapor_StrukturNilai.TahunAjaranSemester m_cur = DAO_Rapor_StrukturNilai.GetMaxPeriode_Entity();
            txtBukaSemester.Text = m.TahunAjaran + " semester " + m.Semester;

            lnkOKBukaSemester.Visible = true;
            if (Libs.GetSemesterByTanggal(DateTime.Now).ToString() == m_cur.Semester &&
               m_cur.TahunAjaran == Libs.GetTahunAjaranNow())
            {
                lnkOKBukaSemester.Visible = false;
            }

            txtTahunAjaranNew.Value = m.TahunAjaran;
            txtSemesterNew.Value = m.Semester;
            txtTahunAjaranOld.Value = m_cur.TahunAjaran;
            txtSemesterOld.Value = m_cur.Semester;

            txtKeyAction.Value = JenisAction.DoShowBukaSemester.ToString();
        }

        protected void bntLihatData_Click(object sender, EventArgs e)
        {
            txtKeyAction.Value = JenisAction.DoShowLihatData.ToString();
        }

        protected void lnkOKLihatData_Click(object sender, EventArgs e)
        {
            if (Libs.GetQueryString("p").Trim() != "")
            {
                string periode = GetPeriode();
                periode = periode.Replace("/", "-");

                Response.Redirect(
                        Libs.FILE_PAGE_URL +
                        (Libs.GetQueryString("q").Trim() != "" ? "?q=" + Libs.GetQueryString("q") : "") +
                        (periode != "" && (Libs.GetQueryString("q").Trim() != "" ? "?q=" + Libs.GetQueryString("q") : "") != "" ? "&p=" + periode : "") +
                        (periode != "" && (Libs.GetQueryString("q").Trim() != "" ? "?q=" + Libs.GetQueryString("q") : "") == "" ? "?p=" + periode : "")
                    );
            }
            else
            {
                BindListView(!IsPostBack, this.Master.txtCariData.Text);
            }
        }

        protected void btnDoRefreshBukaSemester_Click(object sender, EventArgs e)
        {
            DoRefresh(true);
        }
        
        protected void btnDoAddEkskul_Click(object sender, EventArgs e)
        {
            InitFields();

            vldMapel.Enabled = false;
            List<Mapel> lst_mapel = DAO_Mapel.GetAllBySekolah_Entity(GetSekolah().Kode.ToString()).FindAll(m0 => DAO_Mapel.GetJenisMapelByJenis(m0.Jenis) == Application_Libs.Libs.JENIS_MAPEL.EKSTRAKURIKULER).OrderBy(m => m.Nama).ToList();
            cboMapel.Items.Clear();
            cboMapel.Items.Add("");
            foreach (Mapel m in lst_mapel)
            {
                cboMapel.Items.Add(new ListItem
                {
                    Value = m.Kode.ToString(),
                    Text = m.Nama
                });
            }

            ShowKelasEkskul(true);

            EnableInputHeader(true);
            txtKeyAction.Value = JenisAction.Add.ToString();
        }

        protected void lnkOKPredikatPenilaian_Click(object sender, EventArgs e)
        {
            DoUpdatePredikat();
            txtKeyAction.Value = JenisAction.DoUpdate.ToString();
        }

        protected void DoUpdatePredikat()
        {
            try
            {
                Rapor_StrukturNilai m_sn = DAO_Rapor_StrukturNilai.GetByID_Entity(txtID.Value);
                if (m_sn != null)
                {
                    if (m_sn.TahunAjaran != null)
                    {

                        DAO_Rapor_StrukturNilai_Predikat.DeleteByHeader(txtID.Value, Libs.LOGGED_USER_M.UserID);
                        List<Rapor_StrukturNilai_Predikat> lst_predikat =
                            DAO_Rapor_StrukturNilai_Predikat.GetAllByHeader_Entity(txtID.Value);

                        //predikat 1
                        txtKodePredikat1.Value = Guid.NewGuid().ToString();
                        if (lst_predikat.FindAll(
                                m0 => m0.Urutan == 1
                            ).Count > 0)
                        {
                            txtKodePredikat1.Value = lst_predikat.FindAll(
                                m0 => m0.Urutan == 1
                            ).FirstOrDefault().Kode.ToString();
                        }
                        DAO_Rapor_StrukturNilai_Predikat.Insert(new Rapor_StrukturNilai_Predikat
                        {
                            Kode = new Guid(txtKodePredikat1.Value),
                            Rel_Rapor_StrukturNilai = new Guid(txtID.Value),
                            Predikat = txtPredikat1.Text,
                            Minimal = Libs.GetStringToDecimal(txtMinimal1.Text),
                            Maksimal = Libs.GetStringToDecimal(txtMaksimal1.Text),
                            Deskripsi = txtDeskripsi1.Text,
                            Urutan = 1
                        }, Libs.LOGGED_USER_M.UserID);

                        //predikat 2
                        if (lst_predikat.FindAll(
                                m0 => m0.Urutan == 2
                            ).Count > 0)
                        {
                            txtKodePredikat2.Value = lst_predikat.FindAll(
                                m0 => m0.Urutan == 2
                            ).FirstOrDefault().Kode.ToString();
                        }
                        txtKodePredikat2.Value = Guid.NewGuid().ToString();
                        DAO_Rapor_StrukturNilai_Predikat.Insert(new Rapor_StrukturNilai_Predikat
                        {
                            Kode = new Guid(txtKodePredikat2.Value),
                            Rel_Rapor_StrukturNilai = new Guid(txtID.Value),
                            Predikat = txtPredikat2.Text,
                            Minimal = Libs.GetStringToDecimal(txtMinimal2.Text),
                            Maksimal = Libs.GetStringToDecimal(txtMaksimal2.Text),
                            Deskripsi = txtDeskripsi2.Text,
                            Urutan = 2
                        }, Libs.LOGGED_USER_M.UserID);

                        //predikat 3
                        if (lst_predikat.FindAll(
                                m0 => m0.Urutan == 3
                            ).Count > 0)
                        {
                            txtKodePredikat3.Value = lst_predikat.FindAll(
                                m0 => m0.Urutan == 3
                            ).FirstOrDefault().Kode.ToString();
                        }
                        txtKodePredikat3.Value = Guid.NewGuid().ToString();
                        DAO_Rapor_StrukturNilai_Predikat.Insert(new Rapor_StrukturNilai_Predikat
                        {
                            Kode = new Guid(txtKodePredikat3.Value),
                            Rel_Rapor_StrukturNilai = new Guid(txtID.Value),
                            Predikat = txtPredikat3.Text,
                            Minimal = Libs.GetStringToDecimal(txtMinimal3.Text),
                            Maksimal = Libs.GetStringToDecimal(txtMaksimal3.Text),
                            Deskripsi = txtDeskripsi3.Text,
                            Urutan = 3
                        }, Libs.LOGGED_USER_M.UserID);

                        //predikat 4
                        if (lst_predikat.FindAll(
                                m0 => m0.Urutan == 4
                            ).Count > 0)
                        {
                            txtKodePredikat4.Value = lst_predikat.FindAll(
                                m0 => m0.Urutan == 4
                            ).FirstOrDefault().Kode.ToString();
                        }
                        txtKodePredikat4.Value = Guid.NewGuid().ToString();
                        DAO_Rapor_StrukturNilai_Predikat.Insert(new Rapor_StrukturNilai_Predikat
                        {
                            Kode = new Guid(txtKodePredikat4.Value),
                            Rel_Rapor_StrukturNilai = new Guid(txtID.Value),
                            Predikat = txtPredikat4.Text,
                            Minimal = Libs.GetStringToDecimal(txtMinimal4.Text),
                            Maksimal = Libs.GetStringToDecimal(txtMaksimal4.Text),
                            Deskripsi = txtDeskripsi4.Text,
                            Urutan = 4
                        }, Libs.LOGGED_USER_M.UserID);

                        //predikat 5
                        txtKodePredikat5.Value = Guid.NewGuid().ToString();
                        if (lst_predikat.FindAll(
                                m0 => m0.Urutan == 5
                            ).Count > 0)
                        {
                            txtKodePredikat5.Value = lst_predikat.FindAll(
                                m0 => m0.Urutan == 5
                            ).FirstOrDefault().Kode.ToString();
                        }
                        DAO_Rapor_StrukturNilai_Predikat.Insert(new Rapor_StrukturNilai_Predikat
                        {
                            Kode = new Guid(txtKodePredikat5.Value),
                            Rel_Rapor_StrukturNilai = new Guid(txtID.Value),
                            Predikat = txtPredikat5.Text,
                            Minimal = Libs.GetStringToDecimal(txtMinimal5.Text),
                            Maksimal = Libs.GetStringToDecimal(txtMaksimal5.Text),
                            Deskripsi = txtDeskripsi5.Text,
                            Urutan = 5
                        }, Libs.LOGGED_USER_M.UserID);

                    }
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void ShowDataPredikatPenilaian()
        {
            InitFieldsPredikatPenilaian();
            var m_sn = DAO_Rapor_StrukturNilai.GetByID_Entity(txtID.Value);
            if (m_sn != null)
            {
                if (m_sn.TahunAjaran != null)
                {
                    Rapor_StrukturNilai m = DAO_Rapor_StrukturNilai.GetByID_Entity(txtID.Value);
                    if (m != null)
                    {
                        Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.Rel_Mapel.ToString());
                        string kelas = "";
                        if (m.Rel_Kelas.ToString().Trim() == "")
                        {
                            kelas = "(Semua)";
                        }
                        else
                        {
                            Kelas m_kelas = DAO_Kelas.GetByID_Entity(m.Rel_Kelas.ToString());
                            if (m_kelas != null)
                            {
                                if (m_mapel.Nama != null && m_kelas.Nama != null)
                                {
                                    kelas = m_kelas.Nama;
                                }
                            }
                        }
                        if (m_mapel != null)
                        {
                            if (m_mapel.Nama != null)
                            {
                                lblTahunAjaranPredikat.Text = m.TahunAjaran + " semester " + m.Semester;
                                lblMapelPredikat.Text = m_mapel.Nama;
                                lblKelasPredikat.Text = kelas;
                                lblKKM.Text = Math.Round(m.KKM, Constantas.PEMBULATAN_DESIMAL_NILAI_SMP).ToString();
                            }
                        }

                        List<Rapor_StrukturNilai_Predikat> lst_predikat = DAO_Rapor_StrukturNilai_Predikat.GetAllByHeader_Entity(txtID.Value);
                        foreach (var m_ktsp_p in lst_predikat)
                        {
                            switch (m_ktsp_p.Urutan)
                            {
                                case 1:
                                    txtPredikat1.Text = m_ktsp_p.Predikat;
                                    txtMinimal1.Text = (m_ktsp_p.Minimal <= 0 && txtPredikat1.Text.Trim() == "" ? "" : m_ktsp_p.Minimal.ToString());
                                    txtMaksimal1.Text = (m_ktsp_p.Maksimal <= 0 && txtPredikat1.Text.Trim() == "" ? "" : m_ktsp_p.Maksimal.ToString());
                                    txtDeskripsi1.Text = m_ktsp_p.Deskripsi.ToString();
                                    break;
                                case 2:
                                    txtPredikat2.Text = m_ktsp_p.Predikat;
                                    txtMinimal2.Text = (m_ktsp_p.Minimal <= 0 && txtPredikat2.Text.Trim() == "" ? "" : m_ktsp_p.Minimal.ToString());
                                    txtMaksimal2.Text = (m_ktsp_p.Maksimal <= 0 && txtPredikat2.Text.Trim() == "" ? "" : m_ktsp_p.Maksimal.ToString());
                                    txtDeskripsi2.Text = m_ktsp_p.Deskripsi.ToString();
                                    break;
                                case 3:
                                    txtPredikat3.Text = m_ktsp_p.Predikat;
                                    txtMinimal3.Text = (m_ktsp_p.Minimal <= 0 && txtPredikat3.Text.Trim() == "" ? "" : m_ktsp_p.Minimal.ToString());
                                    txtMaksimal3.Text = (m_ktsp_p.Maksimal <= 0 && txtPredikat3.Text.Trim() == "" ? "" : m_ktsp_p.Maksimal.ToString());
                                    txtDeskripsi3.Text = m_ktsp_p.Deskripsi.ToString();
                                    break;
                                case 4:
                                    txtPredikat4.Text = m_ktsp_p.Predikat;
                                    txtMinimal4.Text = (m_ktsp_p.Minimal <= 0 && txtPredikat4.Text.Trim() == "" ? "" : m_ktsp_p.Minimal.ToString());
                                    txtMaksimal4.Text = (m_ktsp_p.Maksimal <= 0 && txtPredikat4.Text.Trim() == "" ? "" : m_ktsp_p.Maksimal.ToString());
                                    txtDeskripsi4.Text = m_ktsp_p.Deskripsi.ToString();
                                    break;
                                case 5:
                                    txtPredikat5.Text = m_ktsp_p.Predikat;
                                    txtMinimal5.Text = (m_ktsp_p.Minimal <= 0 && txtPredikat5.Text.Trim() == "" ? "" : m_ktsp_p.Minimal.ToString());
                                    txtMaksimal5.Text = (m_ktsp_p.Maksimal <= 0 && txtPredikat5.Text.Trim() == "" ? "" : m_ktsp_p.Maksimal.ToString());
                                    txtDeskripsi5.Text = m_ktsp_p.Deskripsi.ToString();
                                    break;
                            }
                        }
                    }
                }
            }
        }

        protected void InitFieldsPredikatPenilaian()
        {
            txtPredikat1.Text = "A";
            txtPredikat2.Text = "B";
            txtPredikat3.Text = "C";
            txtPredikat4.Text = "D";
            txtPredikat5.Text = "";

            txtMinimal1.Text = "";
            txtMinimal2.Text = "";
            txtMinimal3.Text = "";
            txtMinimal4.Text = "";
            txtMinimal5.Text = "";

            txtMaksimal1.Text = "";
            txtMaksimal2.Text = "";
            txtMaksimal3.Text = "";
            txtMaksimal4.Text = "";
            txtMaksimal5.Text = "";

            txtDeskripsi1.Text = "Sangat Baik";
            txtDeskripsi2.Text = "Baik";
            txtDeskripsi3.Text = "Cukup";
            txtDeskripsi4.Text = "Kurang";
            txtDeskripsi5.Text = "";
        }

        protected void btnShowInputPredikatPenilaian_Click(object sender, EventArgs e)
        {
            ShowDataPredikatPenilaian();
            txtKeyAction.Value = JenisAction.DoShowInputPredikat.ToString();
        }
    }
}