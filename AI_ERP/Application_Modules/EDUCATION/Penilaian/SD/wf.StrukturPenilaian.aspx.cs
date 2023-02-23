using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Entities.Elearning.SD;
using AI_ERP.Application_DAOs.Elearning.SD;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.SD
{
    public partial class wf_StrukturPenilaian : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATASTRUKTURPENILAIAN_SD";
        public static List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT> lst_nilai = new List<DAO_Rapor_NilaiSiswa_Det.Rapor_NilaiSiswa_Det_EXT>();

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
            DoShowData,
            DoChangePage,
            DoShowStrukturNilai,
            DoShowBukaSemester,
            DoShowLihatData,
            DoShowInputAspekPenilaian,
            DoShowInputKompetensiDasar,
            DoShowInputKomponenPenilaian,
            DoShowConfirmHapus,
            DoShowInputDownloadKD
        }

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
                InitKeyEventClient();
                ListDropdown();

                lst_nilai = DAO_Rapor_NilaiSiswa_Det.GetAllByTABySM_Entity(
                        GetTahunAjaran(), GetSemester()
                    );
            }

            switch (mvMain.ActiveViewIndex)
            {
                case 0:
                    this.Master.ShowHeaderTools = true;
                    BindListView(true, Libs.GetQ());
                    break;
                case 1:
                    this.Master.ShowHeaderTools = false;
                    ShowStrukturNilai(txtID.Value);
                    break;
                case 2:
                    break;
                default:
                    this.Master.ShowHeaderTools = true;
                    BindListView(true, Libs.GetQ());
                    break;
            }
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

            if (lst_kelasdet.Count == 0)
            {

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

        protected void ListDropdown()
        {
            Sekolah sekolah = DAO_Sekolah.GetAll_Entity().FindAll(
                m => m.UrutanJenjang == (int)Libs.UnitSekolah.SD).FirstOrDefault();
            if (sekolah != null)
            {
                if (sekolah.Nama != null)
                {
                    List<Kelas> lst_kelas = DAO_Kelas.GetAllByUnit_Entity(sekolah.Kode.ToString());
                    cboKelas.Items.Clear();
                    cboKelas.Items.Add("");
                    cboKelas2.Items.Clear();
                    cboKelas2.Items.Add("");
                    cboKelas3.Items.Clear();
                    cboKelas3.Items.Add("");
                    cboKelas4.Items.Clear();
                    cboKelas4.Items.Add("");
                    cboKelas5.Items.Clear();
                    cboKelas5.Items.Add("");
                    cboKelas6.Items.Clear();
                    cboKelas6.Items.Add("");

                    cboKelasKD.Items.Clear();
                    cboKelasKD.Items.Add("");
                    foreach (Kelas m in lst_kelas)
                    {
                        if (m.IsAktif)
                        {
                            cboKelas.Items.Add(new ListItem
                            {
                                Value = m.Kode.ToString(),
                                Text = m.Nama
                            });
                            cboKelas2.Items.Add(new ListItem
                            {
                                Value = m.Kode.ToString(),
                                Text = m.Nama
                            });
                            cboKelas3.Items.Add(new ListItem
                            {
                                Value = m.Kode.ToString(),
                                Text = m.Nama
                            });
                            cboKelas4.Items.Add(new ListItem
                            {
                                Value = m.Kode.ToString(),
                                Text = m.Nama
                            });
                            cboKelas5.Items.Add(new ListItem
                            {
                                Value = m.Kode.ToString(),
                                Text = m.Nama
                            });
                            cboKelas6.Items.Add(new ListItem
                            {
                                Value = m.Kode.ToString(),
                                Text = m.Nama
                            });
                            cboKelasKD.Items.Add(new ListItem
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

            cboKurikulum.Items.Clear();
            cboKurikulum.Items.Add("");
            cboKurikulum.Items.Add(new ListItem { Value = Libs.JenisKurikulum.SD.KTSP, Text = Libs.JenisKurikulum.SD.KTSP });
            cboKurikulum.Items.Add(new ListItem { Value = Libs.JenisKurikulum.SD.KURTILAS, Text = Libs.JenisKurikulum.SD.KURTILAS });

            var lst_periode = DAO_Rapor_StrukturNilai.GetDistinctTahunAjaranPeriode_Entity();
            cboPeriodeLihatData.Items.Clear();
            foreach (var item in lst_periode)
            {
                cboPeriodeLihatData.Items.Add(new ListItem {
                    Value = item.TahunAjaran.ToString() + item.Semester.ToString(),
                    Text = item.TahunAjaran + " semester " + item.Semester
                });
            }

            cboPeriodeKD.Items.Clear();
            foreach (var item in lst_periode)
            {
                cboPeriodeKD.Items.Add(new ListItem
                {
                    Value = item.TahunAjaran.ToString() + item.Semester.ToString(),
                    Text = item.TahunAjaran + " semester " + item.Semester
                });
            }
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
        
        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
            txtTahunAjaran.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboSemester.ClientID + "').focus(); return false; }");
            cboSemester.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboMapel.ClientID + "').focus(); return false; }");
            cboMapel.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboKelas.ClientID + "').focus(); return false; }");
            cboKelas.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtKKM.ClientID + "').focus(); return false; }");
            txtKKM.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboJenisPerhitunganRapor.ClientID + "').focus(); return false; }");
            cboJenisPerhitunganRapor.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKInput.ClientID + "').click(); return false; }");

            txtPoinKompetensiDasar.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtKompetensiDasar.ClientID + "').focus(); return false; }");
            txtBobotAP.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKKompetensiDasar.ClientID + "').click(); return false; }");

            txtBobotNKD.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKKomponenPenilaian.ClientID + "').click(); return false; }");

            txtPoinAspekPenilaian.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtAspekPenilaian.ClientID + "').focus(); return false; }");
            cboJenisPerhitunganAspekPenilaian.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKAspekPenilaian.ClientID + "').click(); return false; }");
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            string tahun_ajaran = GetTahunAjaran();
            string semester = GetSemester();

            string periode = Libs.GetQueryString("p");
            periode = periode.Replace("-", "/");
            if (periode.Trim() != "")
            {
                if (periode.Length > 9)
                {
                    tahun_ajaran = periode.Substring(0, 9);
                    semester = periode.Substring(cboPeriodeLihatData.SelectedValue.Length - 1, 1);
                }
            }            

            if (tahun_ajaran.Trim() == "" && semester.Trim() == "")
            {
                sql_ds.ConnectionString = Libs.GetConnectionString_Rapor();
                sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
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
            else
            {
                sql_ds.ConnectionString = Libs.GetConnectionString_Rapor();
                sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
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
                    sql_ds.SelectCommand = DAO_Rapor_StrukturNilai.SP_SELECT_ALL_BY_TA_BY_SM;
                }
                if (isbind) lvData.DataBind();
            }

            if (periode.Trim() != "")
            {
                Libs.SelectDropdownListByValue(cboPeriodeLihatData, periode);
            }
        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {
            System.Web.UI.WebControls.Literal imgh_kelas = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_kelas");
            System.Web.UI.WebControls.Literal imgh_jenisperhitungan = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_jenisperhitungan");
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

            imgh_kelas.Text = html_image;
            imgh_jenisperhitungan.Text = html_image;
            imgh_kkm.Text = html_image;

            imgh_kelas.Visible = false;
            imgh_jenisperhitungan.Visible = false;
            imgh_kkm.Visible = false;

            switch (e.SortExpression.ToString().Trim())
            {
                case "Kelas":
                    imgh_kelas.Visible = true;
                    break;
                case "JenisPerhitungan":
                    imgh_jenisperhitungan.Visible = true;
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
            txtKKM.Text = "";
            chkPisahkanKP.Checked = false;
            chkPisahkanKPNoLTS.Checked = false;
        }

        protected void btnDoAdd_Click(object sender, EventArgs e)
        {
            InitFields();
            txtKeyAction.Value = JenisAction.Add.ToString();
        }

        protected void lnkOKHapus_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID.Value.Trim() != "")
                {
                    DAO_Rapor_StrukturNilai.Delete(txtID.Value, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, Libs.GetQ());
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
                Rapor_StrukturNilai m = new Rapor_StrukturNilai();
                m.Kode = Guid.NewGuid();
                m.TahunAjaran = txtTahunAjaran.Text;
                m.Semester = cboSemester.SelectedValue;
                m.Rel_Mapel = new Guid(cboMapel.SelectedValue);
                m.Rel_Kelas = new Guid(cboKelas.SelectedValue);
                m.Rel_Kelas2 = cboKelas2.SelectedValue;
                m.Rel_Kelas3 = cboKelas3.SelectedValue;
                m.Rel_Kelas4 = cboKelas4.SelectedValue;
                m.Rel_Kelas5 = cboKelas5.SelectedValue;
                m.Rel_Kelas6 = cboKelas6.SelectedValue;
                m.Kurikulum = cboKurikulum.SelectedValue;
                m.KKM = Libs.GetStringToDecimal(txtKKM.Text);
                m.JenisPerhitungan = cboJenisPerhitunganRapor.SelectedValue;
                m.IsKelompokanKP = chkPisahkanKP.Checked;
                m.IsKelompokanKPNoLTS = chkPisahkanKPNoLTS.Checked;
                if (txtID.Value.Trim() != "")
                {
                    m.Kode = new Guid(txtID.Value);
                    DAO_Rapor_StrukturNilai.Update(m, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, Libs.GetQ());
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
                else
                {
                    DAO_Rapor_StrukturNilai.Insert(m, Libs.LOGGED_USER_M.UserID);
                    BindListView(!IsPostBack, Libs.GetQ());
                    InitFields();
                    txtKeyAction.Value = JenisAction.AddWithMessage.ToString();
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
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
                        txtID.Value = m.Kode.ToString();
                        txtTahunAjaran.Text = m.TahunAjaran;
                        cboSemester.SelectedValue = m.Semester;
                        cboMapel.SelectedValue = m.Rel_Mapel.ToString();
                        cboKelas.SelectedValue = m.Rel_Kelas.ToString();
                        cboKelas2.SelectedValue = m.Rel_Kelas2.ToString();
                        cboKelas3.SelectedValue = m.Rel_Kelas3.ToString();
                        cboKelas4.SelectedValue = m.Rel_Kelas4.ToString();
                        cboKelas5.SelectedValue = m.Rel_Kelas5.ToString();
                        cboKelas6.SelectedValue = m.Rel_Kelas6.ToString();
                        cboKurikulum.SelectedValue = m.Kurikulum;
                        txtKKM.Text = m.KKM.ToString();
                        cboJenisPerhitunganRapor.SelectedValue = m.JenisPerhitungan;
                        chkPisahkanKP.Checked = m.IsKelompokanKP;
                        chkPisahkanKPNoLTS.Checked = m.IsKelompokanKPNoLTS;
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

            Response.Redirect(
                    Libs.FILE_PAGE_URL + 
                    (this.Master.txtCariData.Text.Trim() != "" ? "?q=" + this.Master.txtCariData.Text : "") +
                    (periode != "" ? "&p=" + periode : "")
                );
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

        protected void lvListKTSPDet_Sorting(object sender, ListViewSortEventArgs e)
        {

        }

        protected void lvListKTSPDet_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {

        }

        protected void btnShowDataListFromKTSPDet_Click(object sender, EventArgs e)
        {
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
                                    "<label title=\" Tambah Kompetensi Dasar \" onclick=\"" + txtIDAspekPenilaian.ClientID + ".value = '" + struktur_ap.Kode.ToString() + "'; " + btnShowInputKompetensiDasar.ClientID + ".click();\" class=\"badge\" style=\"cursor: pointer; font-weight: normal; font-size: x-small; margin-right: 5px;\">" +
                                        "&nbsp;" +
                                        "<i class=\"fa fa-plus\"></i>" +
                                        "&nbsp;&nbsp;" +
                                        "KD" +
                                        "&nbsp;" +
                                    "</label>" +
                                    "<label title=\" Ubah Aspek Penilaian \" onclick=\"" + txtIDAspekPenilaian.ClientID + ".value = '" + struktur_ap.Kode.ToString() + "'; " + btnShowInputEditAspekPenilaian.ClientID + ".click();\" title=\" Ubah \" class=\"badge\" style=\"cursor: pointer; font-weight: normal; font-size: x-small;\">" +
                                        "&nbsp;" +
                                        "<i class=\"fa fa-edit\"></i>" +
                                        "&nbsp;" +
                                    "</label>" +
                                "</td>";
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
                                                "<hr style=\"margin: 0px; border-color: #ebebeb; border-width: 3px;\" />" +
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
                                                        Libs.GetHTMLNoParagraphDiAwal(kd.Nama) +
                                                        span_jenis_perhitungan +
                                                  "</td>" +
                                                  "<td style=\"background-color: white; font-weight: bold; color: grey; text-align: right;\">" +
                                                        "<span title=\" Bobot Aspek Penilaian \" class=\"badge\" style=\"background-color: #f39100; border-radius: 0px;\">" +
                                                            struktur_kd.BobotAP.ToString() + "%" +
                                                        "</span>" +
                                                  "</td>"
                                                : "<td colspan=\"4\" style=\"background-color: white; font-weight: bold; color: grey; padding-left: 0px; text-align: justify;\">" +
                                                        (struktur_kd.Poin.Trim() != "" ? struktur_kd.Poin + " : " : "") +
                                                        Libs.GetHTMLNoParagraphDiAwal(kd.Nama) +
                                                        span_jenis_perhitungan +
                                                        (
                                                            struktur_kd.Deskripsi.Trim() != ""
                                                            ? "<hr style=\"margin: 0px;border-color: #ffd486;border-width: 1px;margin-top: 10px;margin-bottom: 10px;\">" +
                                                              Libs.GetHTMLNoParagraphDiAwal(struktur_kd.Deskripsi.Trim())
                                                            : ""
                                                        ) +
                                                  "</td>"
                                            );
                                    html += "<td style=\"background-color: white; font-weight: normal; color: grey; text-align: right; width: 150px;\">" +
                                                "<label title=\" Tambah Komponen Penilaian \" onclick=\"" + txtIDAspekPenilaian.ClientID + ".value = '" + struktur_kd.Rel_Rapor_StrukturNilai_AP.ToString() + "'; " + txtIDKompetensiDasar.ClientID + ".value = '" + struktur_kd.Kode.ToString() + "'; " + btnShowInputKomponenPenilaian.ClientID + ".click();\" class=\"badge\" style=\"background-color: rgb(136, 153, 52); cursor: pointer; font-weight: normal; font-size: x-small; margin-right: 5px;\">" +
                                                    "&nbsp;" +
                                                    "<i class=\"fa fa-plus\"></i>" +
                                                    "&nbsp;&nbsp;" +
                                                    "KP" +
                                                    "&nbsp;" +
                                                "</label>" +
                                                "<label title=\" Ubah Kompetensi Dasar \" onclick=\"" + txtIDAspekPenilaian.ClientID + ".value = '" + struktur_kd.Rel_Rapor_StrukturNilai_AP.ToString() + "'; " + txtIDKompetensiDasar.ClientID + ".value = '" + struktur_kd.Kode.ToString() + "'; " + btnShowInputEditKompetensiDasar.ClientID + ".click();\" title=\" Ubah \" class=\"badge\" style=\"background-color: rgb(136, 153, 52); cursor: pointer; font-weight: normal; font-size: x-small;\">" +
                                                    "&nbsp;" +
                                                    "<i class=\"fa fa-edit\"></i>" +
                                                    "&nbsp;" +
                                                "</label>" +
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
                                        html += "<td colspan=\"3\" style=\"background-color: white; padding: 0px;\">" +
                                                "</td>";
                                        html += "<td colspan=\"6\" style=\"background-color: white; padding: 0px;\">" +
                                                    "<hr style=\"margin: 0px; border-color: #ebebeb; border-width: 1px;\" />" +
                                                "</td>";
                                        html += "</tr>";
                                    }

                                    foreach (Rapor_StrukturNilai_KP m in lst_komponen_penilaian)
                                    {
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
                                                html += "<td style=\"background-color: white; font-weight: normal; color: #bfbfbf; width: 30px;\">" +
                                                            "<div class=\"checkbox checkbox-adv\" style=\"margin: 0 auto;\">" +
                                                                "<label for=\"chk_kp_" + m.Kode.ToString().Replace("-", "_") + "\">" +
                                                                    "<input value=\"" + m.Kode.ToString() + "\" " +
                                                                            "class=\"access-hide\" " +
                                                                            "id=\"chk_kp_" + m.Kode.ToString().Replace("-", "_") + "\" " +
                                                                            "name=\"chk_kp[]\" " +
                                                                            "type=\"checkbox\">" +
                                                                    "<span class=\"checkbox-circle\"></span><span class=\"checkbox-circle-check\"></span><span class=\"checkbox-circle-icon icon\">done</span>" +
                                                                "</label>" +
                                                            "</div>" +
                                                        "</td>";
                                                html += "<td style=\"padding-left: 0px; color: black; background-color: white; font-weight: bold;\">" +
                                                            Libs.GetHTMLSimpleText(m_kp.Nama) +
                                                            "&nbsp;&nbsp;" +
                                                            "&nbsp;&nbsp;" +
                                                            (
                                                                m.BobotNK > 0
                                                                ? "<sup class=\"badge\" style=\"background-color: #f39100; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">" + m.BobotNK.ToString() + "%</sup>"
                                                                : ""
                                                            ) +
                                                            "<label class=\"badge\" onclick=\"" + txtIDKompetensiDasar.ClientID + ".value = '" + struktur_kd.Kode.ToString() + "'; " + txtIDKomponenPenilaian.ClientID + ".value = '" + m.Kode.ToString() + "'; " + btnShowInputEditKomponenPenilaian.ClientID + ".click();\" title=\" Ubah \" style=\"background-color: rgba(0, 0, 0, 0.44); cursor: pointer; color: white; font-weight: normal; padding: 5px 7px; font-weight: normal; font-size: x-small; display: initial; float: right;\">" +
                                                                "&nbsp;" +
                                                                "<i class=\"fa fa-edit\"></i>" +
                                                                "&nbsp;" +
                                                            "</label>" +
                                                        "</td>";
                                                html += "<td style=\"background-color: white;\">" +
                                                        "</td>";
                                                html += "<td style=\"background-color: white;\">" +
                                                        "</td>";
                                                html += "<td style=\"background-color: white;\">" +
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

                        if (struktur_ap.IsAdaPAT_UKK)
                        {
                            if (struktur_ap.Bobot_Non_PAT_UKK > 0)
                            {
                                html += "<tr>";
                                html += "<td colspan=\"2\" style=\"background-color: white; padding: 0px;\">" +
                                        "</td>";
                                html += "<td colspan=\"8\" style=\"background-color: white; padding: 0px;\">" +
                                            "<hr style=\"margin: 0px; border-color: #ebebeb; border-width: 1px;\" />" +
                                        "</td>";
                                html += "</tr>";

                                html += "<tr>";
                                html += "<td colspan=\"2\" style=\"background-color: white; padding: 0px;\">" +
                                        "</td>";
                                html += "<td colspan=\"8\" style=\"background-color: white; padding: 15px; font-weight: bold; color: grey;\">" +
                                            "&nbsp;<i class=\"fa fa-tags\"></i>&nbsp;&nbsp;&nbsp;" +
                                            "&nbsp;&nbsp;" +
                                            "KD " + Libs.GetHTMLSimpleText(m_ap.Nama) +
                                            "&nbsp;&nbsp;" +
                                            "&nbsp;&nbsp;" +
                                            (
                                                struktur_ap.Bobot_Non_PAT_UKK > 0
                                                ? "<sup class=\"badge\" style=\"background-color: #f39100; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">" +
                                                        struktur_ap.Bobot_Non_PAT_UKK.ToString() +
                                                  "%</sup>"
                                                : ""
                                            ) +
                                        "</td>";
                                html += "</tr>";
                            }

                            if (struktur_ap.Bobot_Non_PAT_UKK > 0 && struktur_ap.Bobot_PAT_UKK > 0)
                            {
                                html += "<tr>";
                                html += "<td colspan=\"2\" style=\"background-color: white; padding: 0px;\">" +
                                        "</td>";
                                html += "<td colspan=\"8\" style=\"background-color: white; padding: 0px;\">" +
                                            "<hr style=\"margin: 0px; border-color: #ebebeb; border-width: 1px;\" />" +
                                        "</td>";
                                html += "</tr>";

                                html += "<tr>";
                                html += "<td colspan=\"2\" style=\"background-color: white; padding: 0px;\">" +
                                        "</td>";
                                html += "<td colspan=\"8\" style=\"background-color: white; padding: 15px; font-weight: bold; color: grey;\">" +
                                            "&nbsp;<i class=\"fa fa-tags\"></i>&nbsp;&nbsp;&nbsp;" +
                                            "&nbsp;&nbsp;" +
                                            "PAT/UKK" +
                                            "&nbsp;&nbsp;" +
                                            "&nbsp;&nbsp;" +
                                            (
                                                struktur_ap.Bobot_PAT_UKK > 0
                                                ? "<sup class=\"badge\" style=\"background-color: #f39100; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">" +
                                                        struktur_ap.Bobot_PAT_UKK.ToString() +
                                                  "%</sup>"
                                                : ""
                                            ) +
                                        "</td>";
                                html += "</tr>";
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

            Rapor_StrukturNilai m_struktur_nilai = DAO_Rapor_StrukturNilai.GetByID_Entity(rel_strukturnilai);
            if (m_struktur_nilai != null)
            {
                if (m_struktur_nilai.TahunAjaran != null)
                {
                    if (m_struktur_nilai.IsKelompokanKP && m_struktur_nilai.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                    {
                        List<DAO_Rapor_StrukturNilai.DistinctKP> lst_kp = DAO_Rapor_StrukturNilai.GetDistinctKP_Entity(m_struktur_nilai.Kode.ToString());
                        List<Rapor_StrukturNilai_KPKelompok> lst_kpkelompok = DAO_Rapor_StrukturNilai_KPKelompok.GetAllByHeader_Entity(
                                            m_struktur_nilai.Kode.ToString()
                                        );

                        string html_kd = "";
                        foreach (var item in lst_kp)
                        {
                            Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(item.Kode);
                            if (m_kp != null)
                            {
                                if (m_kp.Nama != null)
                                {
                                    string s_nilai = "";
                                    Rapor_StrukturNilai_KPKelompok m_kpkelompok = lst_kpkelompok.FindAll(
                                            m => m.Rel_Rapor_KomponenPenilaian == new Guid(item.Kode)
                                        ).FirstOrDefault();
                                    if (m_kpkelompok != null)
                                    {
                                        if (m_kpkelompok.Rel_Rapor_StrukturNilai != null)
                                        {
                                            s_nilai = m_kpkelompok.Bobot.ToString();
                                        }
                                    }

                                    html_kd += "<div class=\"col-xs-2\" style=\"padding: 10px;\">" +
                                                    "Bobot " +
                                                    "&nbsp;" +
                                                    "<span style=\"font-weight: bold;\">" +
                                                        Libs.GetHTMLSimpleText(m_kp.Nama) +
                                                    "</span>" +
                                                    "&nbsp;" +
                                                    "(%)" +
                                                    "<input value=\"" + s_nilai + "\" id=\"" + m_kp.Kode.ToString() + "\" name=\"txtBobotKP[]\" type=\"text\" style=\"font-weight: bold; width: 100%; text-align: right; border-style: solid; border-width: 1px; border-color: #ebebeb; padding: 10px; outline: none; border-radius: 3px;\" />" +
                                               "</div>";
                                }
                            }
                        }

                        if (html_kd.Trim() != "")
                        {
                            html += "<tr>";
                            html += "<td colspan=\"9\" style=\"background-color: white; padding: 0px;\">" +
                                        "<hr style=\"margin: 0px; border-color: #ebebeb; border-width: 2px;\" />" +
                                    "</td>";
                            html += "</tr>";
                            html += "<tr>";
                            html += "<td colspan=\"9\" style=\"background-color: #f4f4f4; padding: 0px;\">" +
                                        "<div class=\"row\" style=\"margin-left: 15px; margin-right: 15px;\">" +
                                            html_kd +
                                            "<button onclick=\"ParseBobotKP(); " + btnSaveBobotKP.ClientID + ".click();\" style=\"font-size: small; font-weight: bold; text-transform: none; margin-top: 20px; margin-right: 15px;\" class=\"btn btn-brand waves-attach waves-light waves-effect pull-right\">" +
                                                "&nbsp;&nbsp;Simpan Bobot Nilai&nbsp;&nbsp;" +
                                            "</button>" +
                                        "</div>" +
                                    "</td>";
                            html += "</tr>";
                        }
                        if (DAO_Mapel.GetByID_Entity(m_struktur_nilai.Rel_Mapel.ToString()).Jenis == Libs.JENIS_MAPEL.SIKAP)
                        {
                            html += "<tr>";
                            html += "<td colspan=\"9\" style=\"background-color: white; padding: 0px;\">" +
                                        "<hr style=\"margin: 0px; border-color: #ebebeb; border-width: 2px;\" />" +
                                    "</td>";
                            html += "</tr>";
                            html += "<tr>";
                            html += "<td colspan=\"9\" style=\"background-color: #f4f4f4; padding: 10px; font-weight: bold;\">" +
                                        "Pengaturan bobot nilai sikap" +
                                    "</td>";
                            html += "</tr>";
                            html += "<tr>";
                            html += "<td colspan=\"9\" style=\"background-color: #f4f4f4; padding: 0px;\">" +
                                        "<div class=\"row\" style=\"margin-left: 15px; margin-right: 15px;\">" +
                                            "<div class=\"col-xs-4\" style=\"padding: 10px;\">" +
                                                "Bobot " +
                                                "&nbsp;" +
                                                "<span style=\"font-weight: bold;\">" +
                                                    "Guru Kelas" +
                                                "</span>" +
                                                "&nbsp;" +
                                                "(%)" +
                                                "<input value=\"" + m_struktur_nilai.BobotSikapGuruKelas.ToString() + "\" " +
                                                       "lang=\"" + m_struktur_nilai.Kode.ToString() + "\" " +
                                                       "name=\"txtBobotSikap[]\" " +
                                                       "type=\"text\" " +
                                                       "style=\"font-weight: bold; width: 100%; text-align: right; border-style: solid; border-width: 1px; border-color: #ebebeb; padding: 10px; outline: none; border-radius: 3px;\" />" +
                                            "</div>" +
                                            "<div class=\"col-xs-4\" style=\"padding: 10px;\">" +
                                                "Bobot " +
                                                "&nbsp;" +
                                                "<span style=\"font-weight: bold;\">" +
                                                    "Guru Bidang Studi" +
                                                "</span>" +
                                                "&nbsp;" +
                                                "(%)" +
                                                "<input value=\"" + m_struktur_nilai.BobotSikapGuruMapel.ToString() + "\" " +
                                                       "lang=\"" + m_struktur_nilai.Kode.ToString() + "\" " +
                                                       "name=\"txtBobotSikap[]\" " +
                                                       "type=\"text\" " +
                                                       "style=\"font-weight: bold; width: 100%; text-align: right; border-style: solid; border-width: 1px; border-color: #ebebeb; padding: 10px; outline: none; border-radius: 3px;\" />" +
                                            "</div>" +
                                            "<button onclick=\"ParseBobotSikap(); " + btnSaveBobotSikap.ClientID + ".click();\" style=\"font-size: small; font-weight: bold; text-transform: none; margin-top: 20px; margin-right: 15px;\" class=\"btn btn-brand waves-attach waves-light waves-effect pull-right\">" +
                                                "&nbsp;&nbsp;Simpan Bobot Nilai&nbsp;&nbsp;" +
                                            "</button>" +
                                        "</div>" +
                                    "</td>";
                            html += "</tr>";
                        }
                    }
                    else
                    {
                        if (DAO_Mapel.GetByID_Entity(m_struktur_nilai.Rel_Mapel.ToString()).Jenis == Libs.JENIS_MAPEL.SIKAP)
                        {
                            html += "<tr>";
                            html += "<td colspan=\"9\" style=\"background-color: white; padding: 0px;\">" +
                                        "<hr style=\"margin: 0px; border-color: #ebebeb; border-width: 2px;\" />" +
                                    "</td>";
                            html += "</tr>";
                            html += "<tr>";
                            html += "<td colspan=\"9\" style=\"background-color: #f4f4f4; padding: 10px; font-weight: bold;\">" +
                                        "<i class=\"fa fa-cog\"></i>&nbsp;&nbsp;" +
                                        "Pengaturan bobot nilai sikap" +
                                    "</td>";
                            html += "</tr>";
                            html += "<tr>";
                            html += "<td colspan=\"9\" style=\"background-color: #f4f4f4; padding: 0px;\">" +
                                        "<div class=\"row\" style=\"margin-left: 15px; margin-right: 15px;\">" +
                                            "<div class=\"col-xs-4\" style=\"padding: 10px;\">" +
                                                "Bobot " +
                                                "&nbsp;" +
                                                "<span style=\"font-weight: bold;\">" +
                                                    "Guru Kelas" +
                                                "</span>" +
                                                "&nbsp;" +
                                                "(%)" +
                                                "<input value=\"" + m_struktur_nilai.BobotSikapGuruKelas.ToString() + "\" " +
                                                       "lang=\"" + m_struktur_nilai.Kode.ToString() + "\" " +
                                                       "name=\"txtBobotSikap[]\" " +
                                                       "type=\"text\" " +
                                                       "style=\"font-weight: bold; width: 100%; text-align: right; border-style: solid; border-width: 1px; border-color: #ebebeb; padding: 10px; outline: none; border-radius: 3px;\" />" +
                                            "</div>" +
                                            "<div class=\"col-xs-4\" style=\"padding: 10px;\">" +
                                                "Bobot " +
                                                "&nbsp;" +
                                                "<span style=\"font-weight: bold;\">" +
                                                    "Guru Bidang Studi" +
                                                "</span>" +
                                                "&nbsp;" +
                                                "(%)" +
                                                "<input value=\"" + m_struktur_nilai.BobotSikapGuruMapel.ToString() + "\" " +
                                                       "lang=\"" + m_struktur_nilai.Kode.ToString() + "\" " +
                                                       "name=\"txtBobotSikap[]\" " +
                                                       "type=\"text\" " +
                                                       "style=\"font-weight: bold; width: 100%; text-align: right; border-style: solid; border-width: 1px; border-color: #ebebeb; padding: 10px; outline: none; border-radius: 3px;\" />" +
                                            "</div>" +
                                            "<button onclick=\"ParseBobotSikap(); " + btnSaveBobotSikap.ClientID + ".click();\" style=\"font-size: small; font-weight: bold; text-transform: none; margin-top: 20px; margin-right: 15px;\" class=\"btn btn-brand waves-attach waves-light waves-effect pull-right\">" +
                                                "&nbsp;&nbsp;Simpan Bobot Nilai&nbsp;&nbsp;" +
                                            "</button>" +
                                        "</div>" +
                                    "</td>";
                            html += "</tr>";
                        }
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
            Guid kode_kompetensi_dasar = Guid.NewGuid();
            if (DAO_Rapor_KompetensiDasar.GetAll_Entity().FindAll(m => m.Nama == txtKompetensiDasarVal.Value).Count > 0)
            {
                kode_kompetensi_dasar = DAO_Rapor_KompetensiDasar.GetAll_Entity().FindAll(m => m.Nama == txtKompetensiDasarVal.Value).FirstOrDefault().Kode;
            }
            else
            {
                DAO_Rapor_KompetensiDasar.Insert(new Rapor_KompetensiDasar
                {
                    Kode = kode_kompetensi_dasar,
                    Nama = txtKompetensiDasarVal.Value,
                    Alias = "",
                    Keterangan = ""
                }, Libs.LOGGED_USER_M.UserID);
            }

            if (txtIDKompetensiDasar.Value.Trim() == "")
            {
                DAO_Rapor_StrukturNilai_KD.Insert(
                    new Rapor_StrukturNilai_KD
                    {
                        Poin = txtPoinKompetensiDasar.Text,
                        Rel_Rapor_StrukturNilai_AP = new Guid(txtIDAspekPenilaian.Value),
                        Rel_Rapor_KompetensiDasar = kode_kompetensi_dasar,
                        BobotAP = Libs.GetStringToDecimal(txtBobotAP.Text),
                        JenisPerhitungan = cboJenisPerhitunganKompetensiDasar.SelectedValue,
                        Deskripsi = txtKompetensiDasarDeskripsiVal.Value
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
                        Rel_Rapor_KompetensiDasar = kode_kompetensi_dasar,
                        BobotAP = Libs.GetStringToDecimal(txtBobotAP.Text),
                        JenisPerhitungan = cboJenisPerhitunganKompetensiDasar.SelectedValue,
                        Urutan = Libs.GetStringToInteger(txtUrutan.Text),
                        Deskripsi = txtKompetensiDasarDeskripsiVal.Value
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
            txtKompetensiDasarVal.Value = "";
            txtKompetensiDasar.Text = "";
            txtKompetensiDasarDeskripsiVal.Value = "";
            txtKompetensiDasarDeskripsi.Text = "";
            cboJenisPerhitunganKompetensiDasar.SelectedValue = "";
            txtUrutan.Text = "";
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
            Guid kode_komponen_penilaian = Guid.NewGuid();
            if (DAO_Rapor_KomponenPenilaian.GetAll_Entity().FindAll(m => m.Nama == txtKomponenPenilaianVal.Value).Count > 0)
            {
                kode_komponen_penilaian = DAO_Rapor_KomponenPenilaian.GetAll_Entity().FindAll(m => m.Nama == txtKomponenPenilaianVal.Value).FirstOrDefault().Kode;
            }
            else
            {
                DAO_Rapor_KomponenPenilaian.Insert(new Rapor_KomponenPenilaian {
                    Kode = kode_komponen_penilaian,
                    Nama = txtKomponenPenilaianVal.Value,
                    Alias = "",
                    Keterangan = ""
                }, Libs.LOGGED_USER_M.UserID);
            }

            if (txtIDKompetensiDasar.Value.Trim() != "")
            {
                if (txtIDKomponenPenilaian.Value.Trim() == "")
                {
                    DAO_Rapor_StrukturNilai_KP.Insert(
                        new Rapor_StrukturNilai_KP
                        {
                            Rel_Rapor_StrukturNilai_KD = new Guid(txtIDKompetensiDasar.Value),
                            Rel_Rapor_KomponenPenilaian = kode_komponen_penilaian,
                            Jenis = "",
                            BobotNK = Libs.GetStringToDecimal(txtBobotNKD.Text)
                        }, Libs.LOGGED_USER_M.UserID
                    );

                    ShowStrukturNilai(txtID.Value);
                    InitFieldsInputKomponenPenilaian();
                    txtKeyAction.Value = JenisAction.AddKPWithMessage.ToString();
                }
                else
                {
                    DAO_Rapor_StrukturNilai_KP.Update(
                        new Rapor_StrukturNilai_KP
                        {
                            Kode = new Guid(txtIDKomponenPenilaian.Value),
                            Rel_Rapor_StrukturNilai_KD = new Guid(txtIDKompetensiDasar.Value),
                            Rel_Rapor_KomponenPenilaian = kode_komponen_penilaian,
                            Jenis = "",
                            BobotNK = Libs.GetStringToDecimal(txtBobotNKD.Text)
                        }, Libs.LOGGED_USER_M.UserID
                    );

                    ShowStrukturNilai(txtID.Value);
                    InitFieldsInputKomponenPenilaian();
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
            }
        }

        protected void InitFieldsInputKomponenPenilaian(bool hapus_id = true)
        {
            if (hapus_id) txtIDKomponenPenilaian.Value = "";
            txtKomponenPenilaianVal.Value = "";
            txtKomponenPenilaian.Text = "";
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
                        txtKeyAction.Value = JenisAction.DoShowInputKomponenPenilaian.ToString();
                    }
                }
            }
        }

        protected void btnShowInputKomponenPenilaian_Click(object sender, EventArgs e)
        {
            ShowInputKomponenPenilaian();
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

                        string kompetensi_dasar = "";
                        Rapor_KompetensiDasar m_kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(m_struktur_kd.Rel_Rapor_KompetensiDasar.ToString());
                        if (m_kd != null)
                        {
                            if (m_kd.Nama != null)
                            {
                                kompetensi_dasar = m_kd.Nama;
                            }
                        }

                        txtPoinKompetensiDasar.Text = m_struktur_kd.Poin;
                        txtKompetensiDasarVal.Value = kompetensi_dasar;
                        txtKompetensiDasar.Text = kompetensi_dasar;
                        txtKompetensiDasarDeskripsiVal.Value = m_struktur_kd.Deskripsi;
                        txtKompetensiDasarDeskripsi.Text = m_struktur_kd.Deskripsi;
                        cboJenisPerhitunganKompetensiDasar.SelectedValue = m_struktur_kd.JenisPerhitungan;
                        txtUrutan.Text = m_struktur_kd.Urutan.ToString();
                        txtBobotAP.Text = m_struktur_kd.BobotAP.ToString();
                        txtKeyAction.Value = JenisAction.DoShowInputKompetensiDasar.ToString();
                    }
                }
            }
        }

        protected void btnShowInputEditKomponenPenilaian_Click(object sender, EventArgs e)
        {
            InitFieldsInputKomponenPenilaian(false);
            if (txtIDKomponenPenilaian.Value.Trim() != "")
            {
                Rapor_StrukturNilai_KP m = DAO_Rapor_StrukturNilai_KP.GetByID_Entity(txtIDKomponenPenilaian.Value);
                if (m != null)
                {
                    if (m.Jenis != null)
                    {
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

                        string komponen_penilaian = "";
                        Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(m.Rel_Rapor_KomponenPenilaian.ToString());
                        if (m_kp != null)
                        {
                            if (m_kp.Nama != null)
                            {
                                komponen_penilaian = m_kp.Nama;
                            }
                        }

                        txtKomponenPenilaianVal.Value = komponen_penilaian;
                        txtKomponenPenilaian.Text = komponen_penilaian;
                        txtBobotNKD.Text = m.BobotNK.ToString();
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
            Guid kode_aspek_penilaian = Guid.NewGuid();
            if (DAO_Rapor_AspekPenilaian.GetAll_Entity().FindAll(m => m.Nama == txtAspekPenilaianVal.Value).Count > 0)
            {
                kode_aspek_penilaian = DAO_Rapor_AspekPenilaian.GetAll_Entity().FindAll(m => m.Nama == txtAspekPenilaianVal.Value).FirstOrDefault().Kode;
            }
            else
            {
                DAO_Rapor_AspekPenilaian.Insert(new Rapor_AspekPenilaian
                {
                    Kode = kode_aspek_penilaian,
                    Nama = txtAspekPenilaianVal.Value,
                    Alias = "",
                    Keterangan = ""
                }, Libs.LOGGED_USER_M.UserID);
            }

            if (txtID.Value.Trim() != "")
            {
                if (txtIDAspekPenilaian.Value.Trim() == "")
                {
                    DAO_Rapor_StrukturNilai_AP.Insert(
                        new Rapor_StrukturNilai_AP
                        {
                            Poin = txtPoinAspekPenilaian.Text,
                            Rel_Rapor_StrukturNilai = new Guid(txtID.Value),
                            Rel_Rapor_AspekPenilaian = kode_aspek_penilaian,
                            JenisPerhitungan = cboJenisPerhitunganAspekPenilaian.SelectedValue,
                            BobotRapor = Libs.GetStringToDecimal(txtBobotRapor.Text),
                            IsAdaPAT_UKK = chkIsPAT_UKK.Checked,
                            Bobot_PAT_UKK = Libs.GetStringToDecimal(txtBobotPATUKK.Text),
                            Bobot_Non_PAT_UKK = Libs.GetStringToDecimal(txtBobotKDNonUKK.Text)
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
                            Rel_Rapor_AspekPenilaian = kode_aspek_penilaian,
                            JenisPerhitungan = cboJenisPerhitunganAspekPenilaian.SelectedValue,
                            BobotRapor = Libs.GetStringToDecimal(txtBobotRapor.Text),
                            IsAdaPAT_UKK = chkIsPAT_UKK.Checked,
                            Bobot_PAT_UKK = Libs.GetStringToDecimal(txtBobotPATUKK.Text),
                            Bobot_Non_PAT_UKK = Libs.GetStringToDecimal(txtBobotKDNonUKK.Text)
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
            if(hapus_id) txtIDAspekPenilaian.Value = "";
            txtPoinAspekPenilaian.Text = "";
            txtAspekPenilaianVal.Value = "";
            txtAspekPenilaian.Text = "";
            cboJenisPerhitunganAspekPenilaian.SelectedValue = "";
            txtBobotRapor.Text = "";
            chkIsPAT_UKK.Checked = false;
            txtBobotPATUKK.Text = "";
            txtBobotKDNonUKK.Text = "";
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
                                    string aspek_penilaian = "";
                                    Rapor_AspekPenilaian m_ap = DAO_Rapor_AspekPenilaian.GetByID_Entity(m_struktur_ap.Rel_Rapor_AspekPenilaian.ToString());
                                    if (m_ap != null)
                                    {
                                        if (m_ap.Nama != null)
                                        {
                                            aspek_penilaian = m_ap.Nama;
                                        }
                                    }

                                    txtPoinAspekPenilaian.Text = m_struktur_ap.Poin;
                                    txtAspekPenilaian.Text = aspek_penilaian;
                                    txtAspekPenilaianVal.Value = aspek_penilaian;
                                    cboJenisPerhitunganAspekPenilaian.SelectedValue = m_struktur_ap.JenisPerhitungan.ToString(); ;
                                    txtBobotRapor.Text = m_struktur_ap.BobotRapor.ToString();
                                    chkIsPAT_UKK.Checked = m_struktur_ap.IsAdaPAT_UKK;
                                    txtBobotPATUKK.Text = m_struktur_ap.Bobot_PAT_UKK.ToString();
                                    txtBobotKDNonUKK.Text = m_struktur_ap.Bobot_Non_PAT_UKK.ToString();
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

                        string aspek_penilaian = "";
                        Rapor_AspekPenilaian m_ap = DAO_Rapor_AspekPenilaian.GetByID_Entity(m_struktur_ap.Rel_Rapor_AspekPenilaian.ToString());
                        if (m_ap != null)
                        {
                            if (m_ap.Nama != null)
                            {
                                aspek_penilaian = m_ap.Nama;
                            }
                        }

                        txtPoinAspekPenilaian.Text = m_struktur_ap.Poin;
                        txtAspekPenilaian.Text = aspek_penilaian;
                        txtAspekPenilaianVal.Value = aspek_penilaian;
                        cboJenisPerhitunganAspekPenilaian.SelectedValue = m_struktur_ap.JenisPerhitungan;
                        txtBobotRapor.Text = m_struktur_ap.BobotRapor.ToString();
                        chkIsPAT_UKK.Checked = m_struktur_ap.IsAdaPAT_UKK;
                        txtBobotPATUKK.Text = m_struktur_ap.Bobot_PAT_UKK.ToString();
                        txtBobotKDNonUKK.Text = m_struktur_ap.Bobot_Non_PAT_UKK.ToString();
                        txtKeyAction.Value = JenisAction.DoShowInputAspekPenilaian.ToString();
                    }
                }
            }
        }

        protected void btnSaveBobotKP_Click(object sender, EventArgs e)
        {
            if (txtParseBobotKP.Value.Trim() != "")
            {
                DAO_Rapor_StrukturNilai_KPKelompok.DeleteByHeader(txtID.Value);
                string[] arr_bobot_kp = txtParseBobotKP.Value.Split(new string[] { ";" }, StringSplitOptions.None);
                foreach (string item in arr_bobot_kp)
                {
                    string[] arr_item = item.Split(new string[] { "|" }, StringSplitOptions.None);
                    if (arr_item.Length == 3)
                    {
                        DAO_Rapor_StrukturNilai_KPKelompok.Insert(new Rapor_StrukturNilai_KPKelompok
                        {
                            Kode = Guid.NewGuid(),
                            Rel_Rapor_StrukturNilai = new Guid(txtID.Value),
                            Rel_Rapor_KomponenPenilaian = new Guid(arr_item[0]),
                            Bobot = Libs.GetStringToDecimal(arr_item[1]),
                            Urutan = Libs.GetStringToInteger(arr_item[2])
                        });

                        txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                    }
                }
            }
            txtParseBobotKP.Value = "";
        }

        protected void btnSaveBobotSikap_Click(object sender, EventArgs e)
        {
            if (txtParseBobotSikap.Value.Trim() != "")
            {
                string[] arr_bobot_sikap = txtParseBobotSikap.Value.Split(new string[] { ";" }, StringSplitOptions.None);
                string s_bobot_sikap_guru_kelas = "";
                string s_bobot_sikap_guru_mapel = "";
                int id = 1;
                foreach (string item in arr_bobot_sikap)
                {
                    string[] arr_item = item.Split(new string[] { "|" }, StringSplitOptions.None);
                    if (arr_item.Length == 3)
                    {
                        if (id == 1)
                        {
                            s_bobot_sikap_guru_kelas = Libs.GetStringToDecimal(arr_item[1]).ToString();
                        }
                        else if (id == 2)
                        {
                            s_bobot_sikap_guru_mapel = Libs.GetStringToDecimal(arr_item[1]).ToString();
                        }

                        id++;
                    }
                }

                DAO_Rapor_StrukturNilai.UpdateBobotSikap(
                        txtID.Value, Libs.GetStringToDecimal(s_bobot_sikap_guru_kelas), Libs.GetStringToDecimal(s_bobot_sikap_guru_mapel) 
                    );

                txtKeyAction.Value = JenisAction.DoUpdate.ToString();
            }
            txtParseBobotSikap.Value = "";
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
                BindListView(!IsPostBack, Libs.GetQ());
            }            
        }

        protected void btnDoRefreshBukaSemester_Click(object sender, EventArgs e)
        {
            DoRefresh(true);
        }

        protected void btnDownloadKD_Click(object sender, EventArgs e)
        {
            txtKeyAction.Value = JenisAction.DoShowInputDownloadKD.ToString();
        }        
    }
}