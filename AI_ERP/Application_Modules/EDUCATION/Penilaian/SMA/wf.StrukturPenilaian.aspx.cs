using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Entities.Elearning.SMA;
using AI_ERP.Application_DAOs.Elearning.SMA;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.SMA
{
    public partial class wf_StrukturPenilaian : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATASTRUKTURPENILAIAN_SMA";
        public static List<Rapor_NilaiSiswa_KURTILAS_Det_Lengkap> lst_nilai_kurtilas = new List<Rapor_NilaiSiswa_KURTILAS_Det_Lengkap>();
        public static List<Rapor_NilaiSiswa_KTSP_Det_Lengkap> lst_nilai_ktsp = new List<Rapor_NilaiSiswa_KTSP_Det_Lengkap>();

        public enum JenisAction
        {
            Add,
            AddWithMessage,
            AddAPWithMessage,
            AddKDWithMessage,
            AddKDKurtilasWithMessage,
            AddKPWithMessage,
            AddKPWithMessageKURTILAS,
            Edit,
            ShowDataList,
            Update,
            Delete,
            Search,
            DoAdd,
            DoUpdate,
            DoDelete,
            DoSearch,
            DoShowData,
            DoChangePage,
            DoShowBukaSemester,
            DoShowLihatData,
            DoShowInputDeskripsiLTSRapor,
            DoShowInputDeskripsiPerMapel,
            DoShowInputAspekPenilaian,
            DoShowInputKompetensiDasar,
            DoShowInputKompetensiDasarKurtilas,
            DoShowInputKompetensiDasarKurtilasSikap,
            DoShowInputKomponenPenilaian,
            DoShowInputKomponenPenilaianKURTILAS,
            DoShowInputPredikat,
            DoShowConfirmHapus,            
            DoShowInfoAdaStrukturNilai,
            DoShowStrukturNilai,
            DoShowPreviewNilai
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

                string tahun_ajaran = "";
                string semester = "";

                if (cboPeriodeLihatData.Items.Count > 0)
                {
                    if (cboPeriodeLihatData.SelectedValue.Trim() != "")
                    {
                        tahun_ajaran = cboPeriodeLihatData.SelectedValue.Substring(0, 9);
                        semester = cboPeriodeLihatData.SelectedValue.Substring(cboPeriodeLihatData.SelectedValue.Length - 1, 1);
                    }
                }

                string periode = Libs.GetQueryString("p");
                periode = periode.Replace("-", "/");
                if (periode.Trim() != "")
                {
                    if (periode.Length > 9)
                    {
                        tahun_ajaran = periode.Substring(0, 9);
                        semester = periode.Substring(periode.Length - 1, 1);
                    }
                }

                lst_nilai_kurtilas = DAO_Rapor_NilaiSiswa_KURTILAS_Det.GetAllByTABySM_Entity(tahun_ajaran, semester);
                lst_nilai_ktsp = DAO_Rapor_NilaiSiswa_KTSP_Det.GetAllByTABySM_Entity(tahun_ajaran, semester);
                BindListView(true, Libs.GetQ());
            }

            switch (mvMain.ActiveViewIndex)
            {
                case 0:
                    this.Master.ShowHeaderTools = true;
                    break;
                case 1:
                    ShowStrukturNilaiKTSP(txtID.Value);
                    this.Master.ShowHeaderTools = false;
                    break;
                case 2:
                    ShowStrukturNilaiKURTILAS(txtID.Value);
                    this.Master.ShowHeaderTools = false;
                    break;
            }
            
            if (!IsPostBack) this.Master.txtCariData.Text = Libs.GetQ();
        }


        public Sekolah GetUnit()
        {
            return DAO_Sekolah.GetAll_Entity().FindAll(
                m => m.UrutanJenjang == (int)Libs.UnitSekolah.SMA).FirstOrDefault();
        }

        protected void ListDropdown()
        {
            Sekolah sekolah = GetUnit();
            if (sekolah != null)
            {
                if (sekolah.Nama != null)
                {
                    List<Kelas> lst_kelas = DAO_Kelas.GetAllByUnit_Entity(sekolah.Kode.ToString());
                    cboKelas.Items.Clear();
                    cboKelas.Items.Add(new ListItem { Value = "", Text = "Semua" });
                    foreach (Kelas m in lst_kelas)
                    {
                        cboKelas.Items.Add(new ListItem {
                            Value = m.Kode.ToString().ToUpper(),
                            Text = m.Nama
                        });
                    }

                    List<Mapel> lst_mapel = DAO_Mapel.GetAllBySekolah_Entity(sekolah.Kode.ToString());
                    cboMapel.Items.Clear();
                    cboMapel.Items.Add("");
                    foreach (Mapel m in lst_mapel)
                    {
                        cboMapel.Items.Add(new ListItem {
                            Value = m.Kode.ToString(),
                            Text = m.Nama +
                                   (
                                     m.Jenis.Trim() != ""
                                     ? HttpUtility.HtmlDecode("&nbsp;&nbsp;&rarr;&nbsp;&nbsp;") +
                                       m.Jenis
                                     : ""
                                   )
                        });
                    }
                }
            }

            //jenis perhitungan kompetensi dasar
            cboJenisPerhitunganKompetensiDasar.Items.Clear();
            cboJenisPerhitunganKompetensiDasar.Items.Add("");
            cboJenisPerhitunganKompetensiDasar.Items.Add(new ListItem { Value = ((int)Libs.JenisPerhitunganNilai.Bobot).ToString(), Text = Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.Bobot) });
            cboJenisPerhitunganKompetensiDasar.Items.Add(new ListItem { Value = ((int)Libs.JenisPerhitunganNilai.RataRata).ToString(), Text = Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.RataRata) });

            cboJenisPerhitunganKompetensiDasarKURTILAS.Items.Clear();
            cboJenisPerhitunganKompetensiDasarKURTILAS.Items.Add("");
            cboJenisPerhitunganKompetensiDasarKURTILAS.Items.Add(new ListItem { Value = ((int)Libs.JenisPerhitunganNilai.Bobot).ToString(), Text = Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.Bobot) });
            cboJenisPerhitunganKompetensiDasarKURTILAS.Items.Add(new ListItem { Value = ((int)Libs.JenisPerhitunganNilai.RataRata).ToString(), Text = Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.RataRata) });

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
            cboKelas.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtKKM.ClientID + "').focus(); return false; }");
            txtKKM.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKInput.ClientID + "').click(); return false; }");

            txtPoinKompetensiDasar.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtKompetensiDasar.ClientID + "').focus(); return false; }");
            txtBobotRaporPPK.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtBobotRaporP.ClientID + "').focus(); return false; }");
            txtBobotRaporP.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKStrukturNilai.ClientID + "').click(); return false; }");

            cboJenisKomponenPenilaian.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtBobotNKD.ClientID + "').focus(); return false; }");
            txtBobotNKD.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKKomponenPenilaian.ClientID + "').click(); return false; }");

            txtPoinKompetensiDasarSikap.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtKompetensiDasarSikapKURTILAS.ClientID + "').focus(); return false; }");
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            string tahun_ajaran = "";
            string semester = "";

            if (cboPeriodeLihatData.Items.Count > 0)
            {
                if (cboPeriodeLihatData.SelectedValue.Trim() != "")
                {
                    tahun_ajaran = cboPeriodeLihatData.SelectedValue.Substring(0, 9);
                    semester = cboPeriodeLihatData.SelectedValue.Substring(cboPeriodeLihatData.SelectedValue.Length - 1, 1);
                }
            }

            string periode = Libs.GetQueryString("p");
            periode = periode.Replace("-", "/");
            if (periode.Trim() != "")
            {
                if (periode.Length > 9)
                {
                    tahun_ajaran = periode.Substring(0, 9);
                    semester = periode.Substring(periode.Length - 1, 1);
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
            System.Web.UI.WebControls.Literal imgh_tahunajaran = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_tahunajaran");
            System.Web.UI.WebControls.Literal imgh_kelas = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_kelas");
            System.Web.UI.WebControls.Literal imgh_mapel = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_mapel");
            System.Web.UI.WebControls.Literal imgh_kurikulum = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_kurikulum");
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
            imgh_mapel.Text = html_image;
            imgh_kurikulum.Text = html_image;
            imgh_kkm.Text = html_image;

            imgh_tahunajaran.Visible = false;
            imgh_kelas.Visible = false;
            imgh_mapel.Visible = false;
            imgh_kurikulum.Visible = false;
            imgh_kkm.Visible = false;

            switch (e.SortExpression.ToString().Trim())
            {
                case "TahunAjaran":
                    imgh_tahunajaran.Visible = true;
                    break;
                case "Kelas":
                    imgh_kelas.Visible = true;
                    break;
                case "Mapel":
                    imgh_mapel.Visible = true;
                    break;
                case "Kurikulum":
                    imgh_kurikulum.Visible = true;
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
            BindListView(true, Libs.GetQ());
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
            txtBobotRaporPengetahuan.Text = "";
            txtBobotRaporUAS.Text = "";
            txtKKM.Text = "";
            txtDeskripsiPerMapel.Text = "";
            txtDeskripsiPerMapelVal.Value = "";
            txtDeskripsiSikapSosial.Text = "";
            txtDeskripsiSikapSosialVal.Value = "";
            txtDeskripsiSikapSpiritual.Text = "";
            txtDeskripsiSikapSpiritualVal.Value = "";
            chkIsNilaiAkhir.Checked = false;
        }

        protected void btnDoAdd_Click(object sender, EventArgs e)
        {
            cboKurikulum.Enabled = true;
            InitFields();
            txtKeyAction.Value = JenisAction.Add.ToString();
        }

        protected void lnkOKHapus_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID.Value.Trim() != "")
                {
                    var m_sn = DAO_Rapor_StrukturNilai.GetByID_Entity(txtID.Value);
                    if (m_sn.Kurikulum == Libs.JenisKurikulum.SMA.KTSP)
                    {
                        DAO_Rapor_StrukturNilai_KTSP.Delete(txtID.Value, Libs.LOGGED_USER_M.UserID);
                    }
                    else if (
                            m_sn.Kurikulum == Libs.JenisKurikulum.SMA.KURTILAS ||
                            m_sn.Kurikulum == Libs.JenisKurikulum.SMA.KURTILAS_SIKAP
                        )
                    {
                        DAO_Rapor_StrukturNilai_KURTILAS.Delete(txtID.Value, Libs.LOGGED_USER_M.UserID);
                    }
                    BindListView(!IsPostBack, Libs.GetQ().Trim());
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
                Mapel m_mapel = DAO_Mapel.GetByID_Entity(cboMapel.SelectedValue);
                if (m_mapel != null)
                {
                    if (m_mapel.Nama != null)
                    {

                        if (cboKurikulum.SelectedValue == Libs.JenisKurikulum.SMA.KURTILAS &&
                           (
                                m_mapel.Jenis.Trim().ToUpper() == Libs.JENIS_MAPEL.EKSKUL.Trim().ToUpper() ||
                                m_mapel.Jenis.Trim().ToUpper() == Libs.JENIS_MAPEL.EKSTRAKURIKULER.Trim().ToUpper()
                           ))
                        {
                            txtKeyAction.Value = "Untuk mata pelajaran ekstrakurikuler kurikulum yang dipilih adalah \"KURTILAS-EKSTRAKURIKULER\".";
                            return;
                        }

                        if (
                                cboKurikulum.SelectedValue != Libs.JenisKurikulum.SMA.KURTILAS_SIKAP &&
                                m_mapel.Jenis.Trim().ToUpper() == Libs.JENIS_MAPEL.SIKAP.Trim().ToUpper()
                           )
                        {
                            txtKeyAction.Value = "Untuk mata pelajaran sikap kurikulum yang dipilih adalah \"KURTILAS-SIKAP\".";
                            return;
                        }

                        if (cboKurikulum.SelectedValue == Libs.JenisKurikulum.SMA.KTSP)
                        {
                            Rapor_StrukturNilai_KTSP m = new Rapor_StrukturNilai_KTSP();
                            m.TahunAjaran = txtTahunAjaran.Text;
                            m.Semester = cboSemester.SelectedValue;
                            m.Rel_Mapel = new Guid(cboMapel.SelectedValue);
                            m.Rel_Kelas = cboKelas.SelectedValue;
                            m.KKM = Libs.GetStringToDecimal(txtKKM.Text);
                            m.Deskripsi = txtDeskripsiPerMapelVal.Value;
                            m.IsNilaiAkhir = chkIsNilaiAkhir.Checked;
                            m.DeskripsiSikapSpiritual = txtDeskripsiSikapSpiritualVal.Value;
                            m.DeskripsiSikapSosial = txtDeskripsiSikapSosialVal.Value;
                            if (txtID.Value.Trim() != "")
                            {
                                m.Kode = new Guid(txtID.Value);

                                //cek struktur nilai
                                if (
                                        DAO_Rapor_StrukturNilai_KTSP.GetAll_Entity().FindAll(
                                            m0 => m0.TahunAjaran == m.TahunAjaran &&
                                                  m0.Semester == m.Semester &&
                                                  m0.Rel_Mapel == m.Rel_Mapel &&
                                                  m0.Rel_Kelas == m.Rel_Kelas &&
                                                  m0.Kode != m.Kode
                                        ).Count > 0
                                )
                                {
                                    InitFields();
                                    txtKeyAction.Value = JenisAction.DoShowInfoAdaStrukturNilai.ToString();
                                    return;
                                }
                                //end cek

                                DAO_Rapor_StrukturNilai_KTSP.Update(m, Libs.LOGGED_USER_M.UserID);
                                BindListView(!IsPostBack, Libs.GetQ().Trim());
                                txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                            }
                            else
                            {
                                //cek struktur nilai
                                if (
                                        DAO_Rapor_StrukturNilai_KTSP.GetAll_Entity().FindAll(
                                            m0 => m0.TahunAjaran == m.TahunAjaran &&
                                                  m0.Semester == m.Semester &&
                                                  m0.Rel_Mapel == m.Rel_Mapel &&
                                                  m0.Rel_Kelas == m.Rel_Kelas
                                        ).Count > 0
                                )
                                {
                                    InitFields();
                                    txtKeyAction.Value = JenisAction.DoShowInfoAdaStrukturNilai.ToString();
                                    return;
                                }
                                //end cek

                                DAO_Rapor_StrukturNilai_KTSP.Insert(m, Libs.LOGGED_USER_M.UserID);
                                BindListView(!IsPostBack, Libs.GetQ().Trim());
                                InitFields();
                                txtKeyAction.Value = JenisAction.AddWithMessage.ToString();
                            }
                        }
                        else if (
                                cboKurikulum.SelectedValue == Libs.JenisKurikulum.SMA.KURTILAS ||
                                cboKurikulum.SelectedValue == Libs.JenisKurikulum.SMA.KURTILAS_SIKAP
                            )
                        {
                            Rapor_StrukturNilai_KURTILAS m = new Rapor_StrukturNilai_KURTILAS();
                            m.TahunAjaran = txtTahunAjaran.Text;
                            m.Semester = cboSemester.SelectedValue;
                            m.Rel_Mapel = new Guid(cboMapel.SelectedValue);
                            m.Rel_Kelas = cboKelas.SelectedValue;
                            m.KKM = Libs.GetStringToDecimal(txtKKM.Text);
                            m.JenisPerhitungan = "";
                            m.BobotRaporPengetahuan = Libs.GetStringToDecimal(txtBobotRaporPengetahuan.Text);
                            m.BobotRaporUAS = Libs.GetStringToDecimal(txtBobotRaporUAS.Text);
                            m.Deskripsi = txtDeskripsiPerMapelVal.Value;
                            m.IsNilaiAkhir = chkIsNilaiAkhir.Checked;
                            m.DeskripsiSikapSpiritual = txtDeskripsiSikapSpiritualVal.Value;
                            m.DeskripsiSikapSosial = txtDeskripsiSikapSosialVal.Value;
                            if (txtID.Value.Trim() != "")
                            {
                                m.Kode = new Guid(txtID.Value);

                                //cek struktur nilai
                                if (
                                        DAO_Rapor_StrukturNilai_KURTILAS.GetAll_Entity().FindAll(
                                            m0 => m0.TahunAjaran == m.TahunAjaran &&
                                                  m0.Semester == m.Semester &&
                                                  m0.Rel_Mapel == m.Rel_Mapel &&
                                                  m0.Rel_Kelas == m.Rel_Kelas &&
                                                  m0.Kode != m.Kode
                                        ).Count > 0
                                )
                                {
                                    InitFields();
                                    txtKeyAction.Value = JenisAction.DoShowInfoAdaStrukturNilai.ToString();
                                    return;
                                }
                                //end cek

                                DAO_Rapor_StrukturNilai_KURTILAS.Update(m, Libs.LOGGED_USER_M.UserID);
                                BindListView(!IsPostBack, Libs.GetQ().Trim());
                                txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                            }
                            else
                            {
                                //cek struktur nilai
                                if (
                                        DAO_Rapor_StrukturNilai_KURTILAS.GetAll_Entity().FindAll(
                                            m0 => m0.TahunAjaran == m.TahunAjaran &&
                                                  m0.Semester == m.Semester &&
                                                  m0.Rel_Mapel == m.Rel_Mapel &&
                                                  m0.Rel_Kelas == m.Rel_Kelas
                                        ).Count > 0
                                )
                                {
                                    InitFields();
                                    txtKeyAction.Value = JenisAction.DoShowInfoAdaStrukturNilai.ToString();
                                    return;
                                }
                                //end cek

                                DAO_Rapor_StrukturNilai_KURTILAS.Insert(m, Libs.LOGGED_USER_M.UserID);
                                BindListView(!IsPostBack, Libs.GetQ().Trim());
                                InitFields();
                                txtKeyAction.Value = JenisAction.AddWithMessage.ToString();
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void btnShowDetail_Click(object sender, EventArgs e)
        {
            cboKurikulum.Enabled = false;
            if (txtID.Value.Trim() != "")
            {
                var m_sn = DAO_Rapor_StrukturNilai.GetByID_Entity(txtID.Value);
                if (m_sn != null)
                {
                    if (m_sn.Kurikulum != null)
                    {
                        if (m_sn.Kurikulum == Libs.JenisKurikulum.SMA.KTSP)
                        {
                            Rapor_StrukturNilai_KTSP m = DAO_Rapor_StrukturNilai_KTSP.GetByID_Entity(txtID.Value.Trim());
                            if (m != null)
                            {
                                if (m.TahunAjaran != null)
                                {
                                    txtID.Value = m.Kode.ToString();
                                    txtTahunAjaran.Text = m.TahunAjaran;
                                    cboSemester.SelectedValue = m.Semester;
                                    Libs.SelectDropdownListByValue(cboMapel, m.Rel_Mapel.ToString());
                                    cboKelas.SelectedValue = m.Rel_Kelas.ToString().ToUpper();
                                    cboKurikulum.SelectedValue = m_sn.Kurikulum;
                                    txtKKM.Text = m.KKM.ToString();
                                    txtDeskripsiPerMapel.Text = m.Deskripsi;
                                    txtDeskripsiPerMapelVal.Value = m.Deskripsi;
                                    chkIsNilaiAkhir.Checked = m.IsNilaiAkhir;
                                    txtDeskripsiSikapSpiritualVal.Value = m.DeskripsiSikapSpiritual;
                                    txtDeskripsiSikapSpiritual.Text = m.DeskripsiSikapSpiritual;
                                    txtDeskripsiSikapSosialVal.Value = m.DeskripsiSikapSosial;
                                    txtDeskripsiSikapSosial.Text = m.DeskripsiSikapSosial;
                                    BindListView(true, Libs.GetQ());

                                    txtKeyAction.Value = JenisAction.DoShowData.ToString();
                                }
                            }
                        }
                        else if (m_sn.Kurikulum == Libs.JenisKurikulum.SMA.KURTILAS)
                        {
                            Rapor_StrukturNilai_KURTILAS m = DAO_Rapor_StrukturNilai_KURTILAS.GetByID_Entity(txtID.Value.Trim());
                            if (m != null)
                            {
                                if (m.TahunAjaran != null)
                                {
                                    txtID.Value = m.Kode.ToString();
                                    txtTahunAjaran.Text = m.TahunAjaran;
                                    cboSemester.SelectedValue = m.Semester;
                                    Libs.SelectDropdownListByValue(cboMapel, m.Rel_Mapel.ToString());
                                    cboKelas.SelectedValue = m.Rel_Kelas.ToString().ToUpper();
                                    cboKurikulum.SelectedValue = m_sn.Kurikulum;
                                    txtKKM.Text = m.KKM.ToString();
                                    txtBobotRaporPengetahuan.Text = m.BobotRaporPengetahuan.ToString();
                                    txtBobotRaporUAS.Text = m.BobotRaporUAS.ToString();
                                    txtDeskripsiPerMapel.Text = m.Deskripsi;
                                    txtDeskripsiPerMapelVal.Value = m.Deskripsi;
                                    chkIsNilaiAkhir.Checked = m.IsNilaiAkhir;
                                    txtDeskripsiSikapSpiritualVal.Value = m.DeskripsiSikapSpiritual;
                                    txtDeskripsiSikapSpiritual.Text = m.DeskripsiSikapSpiritual;
                                    txtDeskripsiSikapSosialVal.Value = m.DeskripsiSikapSosial;
                                    txtDeskripsiSikapSosial.Text = m.DeskripsiSikapSosial;
                                    BindListView(true, Libs.GetQ());

                                    txtKeyAction.Value = JenisAction.DoShowData.ToString();
                                }
                            }
                        }
                        else if (m_sn.Kurikulum == Libs.JenisKurikulum.SMA.KURTILAS_SIKAP)
                        {
                            Rapor_StrukturNilai_KURTILAS m = DAO_Rapor_StrukturNilai_KURTILAS.GetByID_Entity(txtID.Value.Trim());
                            if (m != null)
                            {
                                if (m.TahunAjaran != null)
                                {
                                    txtID.Value = m.Kode.ToString();
                                    txtTahunAjaran.Text = m.TahunAjaran;
                                    cboSemester.SelectedValue = m.Semester;
                                    Libs.SelectDropdownListByValue(cboMapel, m.Rel_Mapel.ToString());
                                    cboKelas.SelectedValue = m.Rel_Kelas.ToString().ToUpper();
                                    cboKurikulum.SelectedValue = m_sn.Kurikulum;
                                    txtKKM.Text = m.KKM.ToString();
                                    txtBobotRaporPengetahuan.Text = m.BobotRaporPengetahuan.ToString();
                                    txtBobotRaporUAS.Text = m.BobotRaporUAS.ToString();
                                    txtDeskripsiPerMapel.Text = m.Deskripsi;
                                    txtDeskripsiPerMapelVal.Value = m.Deskripsi;
                                    chkIsNilaiAkhir.Checked = m.IsNilaiAkhir;
                                    txtDeskripsiSikapSpiritualVal.Value = m.DeskripsiSikapSpiritual;
                                    txtDeskripsiSikapSpiritual.Text = m.DeskripsiSikapSpiritual;
                                    txtDeskripsiSikapSosialVal.Value = m.DeskripsiSikapSosial;
                                    txtDeskripsiSikapSosial.Text = m.DeskripsiSikapSosial;
                                    BindListView(true, Libs.GetQ());

                                    txtKeyAction.Value = JenisAction.DoShowData.ToString();
                                }
                            }
                        }
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
                var m_sn = DAO_Rapor_StrukturNilai.GetByID_Entity(txtID.Value);
                if (m_sn.Kurikulum == Libs.JenisKurikulum.SMA.KTSP)
                {
                    Rapor_StrukturNilai_KTSP m = DAO_Rapor_StrukturNilai_KTSP.GetByID_Entity(txtID.Value.Trim());
                    if (m != null)
                    {
                        if (m.TahunAjaran != null)
                        {
                            Mapel m_mapel = DAO_Mapel.GetByID_Entity(m_sn.Rel_Mapel.ToString());
                            string s_mapel = "";
                            if (m_mapel != null)
                            {
                                if (m_mapel.Nama != null)
                                {
                                    s_mapel = m_mapel.Nama;
                                }
                            }

                            ltrMsgConfirmHapus.Text = "Hapus <br />" +
                                                        "<span style=\"font-weight: bold;\">" +
                                                            Libs.GetHTMLSimpleText(m.TahunAjaran) +
                                                            " semester " +
                                                            m.Semester +
                                                            (
                                                                s_mapel.Trim() != ""
                                                                ? "<br />" +
                                                                  m_mapel.Nama.Trim()
                                                                : ""
                                                            ) +                                                            
                                                        "</span>?";
                            txtKeyAction.Value = JenisAction.DoShowConfirmHapus.ToString();
                        }
                    }
                }
                else if (
                        m_sn.Kurikulum == Libs.JenisKurikulum.SMA.KURTILAS ||
                        m_sn.Kurikulum == Libs.JenisKurikulum.SMA.KURTILAS_SIKAP
                    )
                {
                    Rapor_StrukturNilai_KURTILAS m = DAO_Rapor_StrukturNilai_KURTILAS.GetByID_Entity(txtID.Value.Trim());
                    if (m != null)
                    {
                        if (m.TahunAjaran != null)
                        {
                            Mapel m_mapel = DAO_Mapel.GetByID_Entity(m_sn.Rel_Mapel.ToString());
                            string s_mapel = "";
                            if (m_mapel != null)
                            {
                                if (m_mapel.Nama != null)
                                {
                                    s_mapel = m_mapel.Nama;
                                }
                            }

                            ltrMsgConfirmHapus.Text = "Hapus <br />" +
                                                        "<span style=\"font-weight: bold;\">\"" +
                                                            Libs.GetHTMLSimpleText(m.TahunAjaran) +
                                                            " semester " +
                                                            m.Semester +
                                                            (
                                                                s_mapel.Trim() != ""
                                                                ? "<br />" +
                                                                  m_mapel.Nama.Trim()
                                                                : ""
                                                            ) +
                                                        "\"</span>?";
                            txtKeyAction.Value = JenisAction.DoShowConfirmHapus.ToString();
                        }
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
            BindListView(true, Libs.GetQ());
            this.Master.ShowHeaderTools = true;
            txtKeyAction.Value = JenisAction.ShowDataList.ToString();
        }

        protected void btnShowStrukturNilai_Click(object sender, EventArgs e)
        {
            ltrCaptionKTSPDet.Text = "";

            var m_sn = DAO_Rapor_StrukturNilai.GetByID_Entity(txtID.Value);
            if (m_sn != null)
            {
                if (m_sn.Kurikulum != null)
                {

                    if (m_sn.Kurikulum == Libs.JenisKurikulum.SMA.KTSP)
                    {

                        Rapor_StrukturNilai_KTSP m = DAO_Rapor_StrukturNilai_KTSP.GetByID_Entity(txtID.Value);
                        if (m != null)
                        {
                            if (m.TahunAjaran != null)
                            {
                                Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.Rel_Mapel.ToString());
                                string kelas = "";
                                if (m.Rel_Kelas.Trim() == "")
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
                                                             kelas +
                                                             "&nbsp;" +
                                                         "</span>" +

                                                         "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                                         "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                                             "&nbsp;" +
                                                             "KKM : &nbsp;" +
                                                             Math.Round(m.KKM, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA).ToString() +
                                                             "&nbsp;" +
                                                         "</span>";

                                        ShowStrukturNilaiKTSP(txtID.Value);
                                        this.Master.ShowHeaderTools = false;
                                        mvMain.ActiveViewIndex = 1;
                                        txtKeyAction.Value = JenisAction.DoShowStrukturNilai.ToString();
                                    }
                                }
                            }
                        }

                    }
                    else if (m_sn.Kurikulum == Libs.JenisKurikulum.SMA.KURTILAS)
                    {

                        Rapor_StrukturNilai_KURTILAS m = DAO_Rapor_StrukturNilai_KURTILAS.GetByID_Entity(txtID.Value);
                        if (m != null)
                        {
                            if (m.TahunAjaran != null)
                            {
                                Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.Rel_Mapel.ToString());
                                string kelas = "";
                                if (m.Rel_Kelas.Trim() == "")
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
                                        ltrCaptionKURTILASDet.Text =
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
                                                             kelas +
                                                             "&nbsp;" +
                                                         "</span>" +

                                                         "&nbsp;&nbsp;<i class=\"fa fa-arrow-right\"></i>&nbsp;&nbsp;" +

                                                         "<span class=\"badge\" style=\"background-color: darkblue;\">" +
                                                             "&nbsp;" +
                                                             "KKM : &nbsp;" +
                                                             Math.Round(m.KKM, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA).ToString() +
                                                             "&nbsp;" +
                                                         "</span>";

                                        ShowStrukturNilaiKURTILAS(txtID.Value);                                        
                                        this.Master.ShowHeaderTools = false;
                                        mvMain.ActiveViewIndex = 2;
                                        txtKeyAction.Value = JenisAction.DoShowStrukturNilai.ToString();
                                    }
                                }
                            }
                        }

                    }

                }
            }
        }

        public static string GetJSPreviewNilaiEkskul(Page page, string kode_sn, string tahun_ajaran, string semester, string rel_kelas, string rel_kelas_det, string rel_mapel)
        {
            Mapel m_mapel = DAO_Mapel.GetByID_Entity(rel_mapel);
            if (m_mapel != null)
            {
                if(m_mapel.Nama != null)
                {
                    string url = "";
                    string s_url = "";
                    if (m_mapel.Jenis == Libs.JENIS_MAPEL.EKSTRAKURIKULER)
                    {
                        var m_sn = DAO_Rapor_StrukturNilai.GetByID_Entity(kode_sn);
                        if (m_sn != null)
                        {
                            if (m_sn.Kurikulum != null)
                            {
                                if (m_sn.IsNilaiAkhir)
                                {
                                    url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA_EKSKUL.ROUTE;
                                }
                                else
                                {
                                    url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA.ROUTE;
                                }
                                
                                s_url = "window.open(" +
                                            "'" +
                                                page.ResolveUrl(
                                                    url +
                                                    "?token=" + Constantas.SMA.TOKEN_NILAI_EKSKUL +
                                                    (tahun_ajaran.Trim() != "" ? "&t=" : "") + RandomLibs.GetRndTahunAjaran(tahun_ajaran) +
                                                    (semester.Trim() != "" ? "&s=" : "") + semester +
                                                    (rel_kelas.Trim() != "" ? "&k=" : "") + rel_kelas +
                                                    (rel_kelas_det.Trim() != "" ? "&kd=" : "") + rel_kelas_det +
                                                    (rel_mapel.Trim() != "" ? "&m=" : "") + rel_mapel
                                                ) +
                                            "', " +
                                            "'_blank' " +
                                        "); " +
                                        "return false; ";
                            }
                        }       
                    }

                    return s_url;
                }
            }
            
            return "#";
        }

        public static string GetJSPreviewNilai(Page page, string tahun_ajaran, string semester, string rel_kelas, string rel_kelas_det, string rel_mapel)
        {
            string url = Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA.ROUTE;
            string s_url = "window.open(" +
                                "'" +
                                    page.ResolveUrl(
                                        url +
                                        "?token=" + Constantas.SMA.TOKEN_PREVIEW_NILAI +
                                        (tahun_ajaran.Trim() != "" ? "&t=" : "") + RandomLibs.GetRndTahunAjaran(tahun_ajaran) +
                                        (semester.Trim() != "" ? "&s=" : "") + semester +
                                        (rel_kelas.Trim() != "" ? "&k=" : "") + rel_kelas +
                                        (rel_kelas_det.Trim() != "" ? "&kd=" : "") + rel_kelas_det +
                                        (rel_mapel.Trim() != "" ? "&m=" : "") + rel_mapel
                                    ) +
                                "', " +
                                "'_blank' " +
                           "); " +
                           "return false; ";

            return s_url;
        }

        protected void InitLinkPreviewNilai(string tahun_ajaran, string semester, string rel_kelas, string rel_mapel)
        {
            //lnkOKPreviewNilaiKTSP.Attributes["onclick"] = GetJSPreviewNilai(this.Page, tahun_ajaran, semester, rel_kelas, rel_mapel);
            //lnkOKPreviewNilaiKURTILAS.Attributes["onclick"] = GetJSPreviewNilai(this.Page, tahun_ajaran, semester, rel_kelas, rel_mapel);
        }

        protected void ShowStrukturNilaiSikapKURTILAS(string rel_strukturnilai)
        {
            string html = "";

            Rapor_StrukturNilai_KURTILAS m_strukturnilai = DAO_Rapor_StrukturNilai_KURTILAS.GetByID_Entity(rel_strukturnilai);
            if (m_strukturnilai != null)
            {
                if (m_strukturnilai.TahunAjaran != null)
                {
                    List<Rapor_StrukturNilai_KURTILAS_Sikap> lst_sikap = DAO_Rapor_StrukturNilai_KURTILAS_Sikap.GetAllByHeader_Entity(rel_strukturnilai);
                    int id_sikap = 1;
                    foreach (var sikap in lst_sikap)
                    {
                        Rapor_KompetensiDasar m_kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(sikap.Rel_Rapor_KompetensiDasar.ToString());
                        if (m_kd != null)
                        {
                            if (m_kd.Nama != null)
                            {
                                html += "<tr>" +
                                            "<td style=\"background-color: white; font-weight: normal; color: #bfbfbf; width: 40px; text-align: center; vertical-align: top;\">" +
                                                id_sikap.ToString() +
                                            "</td>" +
                                            "<td style=\"background-color: white; font-weight: normal; color: #bfbfbf; width: 30px; vertical-align: top;\">" +
                                                "<div class=\"checkbox checkbox-adv\" style=\"margin: 0 auto;\">" +
                                                    "<label for=\"chk_kd_sikap_" + sikap.Kode.ToString().Replace("-", "_") + "\">" +
                                                        "<input value=\"" + sikap.Kode.ToString() + "\" " +
                                                                "class=\"access-hide\" " +
                                                                "id=\"chk_kd_sikap_" + sikap.Kode.ToString().Replace("-", "_") + "\" " +
                                                                "name=\"chk_kd_sikap[]\" " +
                                                                "type=\"checkbox\">" +
                                                        "<span class=\"checkbox-circle\"></span><span class=\"checkbox-circle-check\"></span><span class=\"checkbox-circle-icon icon\">done</span>" +
                                                    "</label>" +
                                                "</div>" +
                                            "</td>" +
                                            "<td style=\"text-align: justify; background-color: white; color: grey; padding-left: 0px;\">" +
                                                "<span style=\"font-weight: bold;\">" +
                                                    (sikap.Poin.Trim() != "" ? sikap.Poin + " " : "") +
                                                    Libs.GetHTMLSimpleText(m_kd.Nama) + 
                                                "</span>" +
                                                "&nbsp;&nbsp;" +
                                                "<label title=\" Ubah Kompetensi Dasar \" onclick=\"" + txtIDKompetensiDasarSikap.ClientID + ".value = '" + sikap.Kode.ToString() + "'; " + btnShowInputEditKompetensiDasarKURTILASSikap.ClientID + ".click();\" title=\" Ubah \" class=\"badge\" style=\"background-color: rgb(136, 153, 52); cursor: pointer; font-weight: normal; font-size: x-small;\">" +
                                                    "&nbsp;" +
                                                    "<i class=\"fa fa-edit\"></i>" +
                                                    "&nbsp;" +
                                                "</label>" +
                                                (
                                                    sikap.Deskripsi.Trim() != ""
                                                    ? "<br />" +
                                                      "<div class=\"text-input\" style=\"margin-top: 10px; border-radius: 3px; padding-top: 0px; padding-bottom: 0px;\">" +
                                                        sikap.Deskripsi +
                                                      "</div>"
                                                    : ""
                                                ) +                                                
                                            "</td>" +
                                        "</tr>";
                                id_sikap++;
                            }
                        }                        
                    }
                }
            }

            if (html == "")
            {
                html = "<tr>" +
                            "<td colspan=\"9\" style=\"background-color: white; padding: 0px;\">" +
                                "<div style=\"padding: 10px;\"><label style=\"margin: 0 auto; display: table;\">..:: Data Kosong ::..</label></div>" +
                            "</td>" +
                       "</tr>";
            }
            else
            {
                html = "<tr>" +
                            "<td colspan=\"9\" style=\"background-color: white; padding: 0px;\">" +
                                "<table style=\"margin: 0px; width: 100%;\">" +
                                    html +
                               "</table>" +
                            "</td>" +
                       "</tr>";
            }

            ltrKurtilasSikap.Text = html;
        }

        protected void ShowStrukturNilaiKURTILAS(string rel_strukturnilai)
        {
            ShowStrukturNilaiSikapKURTILAS(rel_strukturnilai);

            string html = "";
            ltrKURTILASDet.Text = "";

            Rapor_StrukturNilai_KURTILAS m_strukturnilai = DAO_Rapor_StrukturNilai_KURTILAS.GetByID_Entity(rel_strukturnilai);
            if (m_strukturnilai != null)
            {
                if (m_strukturnilai.TahunAjaran != null)
                {
                    InitLinkPreviewNilai(
                            m_strukturnilai.TahunAjaran, m_strukturnilai.Semester, m_strukturnilai.Rel_Kelas, m_strukturnilai.Rel_Mapel.ToString()
                        );

                    html += "<tr>";
                    html += "<td colspan=\"9\" style=\"background-color: #f9f7e2; font-weight: normal; color: grey; padding: 10px;\">";
                    html += "<div class=\"row\">" +
                                "<div class=\"col-xs-12\">" +
                                    "<span style=\"font-weight: bold; color: grey;\">" +
                                        "Bobot Rapor " +
                                        "<span style=\"color: mediumvioletred; border-radius: 0px;\">PENGETAHUAN</span>" +
                                    "</span>" +
                                "</div>" +
                            "</div>" +
                            "<div class=\"row\">" +
                                "<div class=\"col-xs-6\">" +
                                    "<span style=\"font-weight: normal\">Dari Pengetahuan</span> : " +
                                    "<span style=\"font-weight: bold\">" +
                                        Math.Round(m_strukturnilai.BobotRaporPengetahuan, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA).ToString() +
                                    "%</span>" +
                                    "&nbsp;&nbsp;" +
                                    "<span style=\"font-weight: normal\">Dari UAS</span> : " +
                                    "<span style=\"font-weight: bold\">" +
                                        Math.Round(m_strukturnilai.BobotRaporUAS, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA).ToString() +
                                     "%</span>" +
                                "</div>" +
                            "</div>";
                    html += "</td>";
                    html += "</tr>";
                }
            }

            string s_html_deskripsi = "";

            //header
            html += "<tr>";
            html += "<td style=\"background-color: #424242; font-weight: normal; color: white; width: 40px; border-top-style: solid; border-top-color: #363535;\">" +
                        "&nbsp;" +
                    "</td>";
            html += "<td style=\"background-color: #424242; font-weight: normal; color: white; width: 30px; border-top-style: solid; border-top-color: #363535;\">" +
                        "<img src=\"" + ResolveUrl("~/Application_CLibs/images/svg/browser.svg") + "\" style=\"height: 20px; width: 20px;\" />" +
                    "</td>";
            html += "<td colspan=\"6\" style=\"background-color: #424242; font-weight: bold; color: white; padding-left: 0px; border-top-style: solid; border-top-color: #363535;\">" +
                        "Aspek Penilaian (AP)" +
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
                        "Kompetensi Dasar (KD)" +
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
                        "Komponen Penilaian (KP)" +
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
            List<Rapor_StrukturNilai_KURTILAS_AP> lst_aspek_penilaian = DAO_Rapor_StrukturNilai_KURTILAS_AP.GetAllByHeader_Entity(rel_strukturnilai);
            foreach (Rapor_StrukturNilai_KURTILAS_AP struktur_ap in lst_aspek_penilaian)
            {
                Rapor_AspekPenilaian m_ap = DAO_Rapor_AspekPenilaian.GetByID_Entity(struktur_ap.Rel_Rapor_AspekPenilaian.ToString());
                if (m_ap != null)
                {
                    if (m_ap.Nama != null)
                    {
                        span_jenis_perhitungan = "";
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
                                    (struktur_ap.Poin.Trim() != "" ? struktur_ap.Poin + " " : "") +
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
                                    "<label title=\" Tambah Kompetensi Dasar \" onclick=\"ShowProgress(true); " + txtIDAspekPenilaian.ClientID + ".value = '" + struktur_ap.Kode.ToString() + "'; " + btnShowInputKompetensiDasarKURTILAS.ClientID + ".click();\" class=\"badge\" style=\"cursor: pointer; font-weight: normal; font-size: x-small; margin-right: 5px;\">" +
                                        "&nbsp;" +
                                        "<i class=\"fa fa-plus\"></i>" +
                                        "&nbsp;&nbsp;" +
                                        "KD" +
                                        "&nbsp;" +
                                    "</label>" +
                                    "<label title=\" Ubah Aspek Penilaian \" onclick=\"ShowProgress(true); " + txtIDAspekPenilaian.ClientID + ".value = '" + struktur_ap.Kode.ToString() + "'; " + btnShowInputEditAspekPenilaian.ClientID + ".click();\" title=\" Ubah \" class=\"badge\" style=\"cursor: pointer; font-weight: normal; font-size: x-small;\">" +
                                        "&nbsp;" +
                                        "<i class=\"fa fa-edit\"></i>" +
                                        "&nbsp;" +
                                    "</label>" +
                                "</td>";
                        html += "</tr>";


                        //list kd
                        int id_kd = 1;
                        List<Rapor_StrukturNilai_KURTILAS_KD> lst_kompetensi_dasar = DAO_Rapor_StrukturNilai_KURTILAS_KD.GetAllByHeader_Entity(struktur_ap.Kode.ToString());
                        foreach (Rapor_StrukturNilai_KURTILAS_KD struktur_kd in lst_kompetensi_dasar)
                        {
                            Rapor_KompetensiDasar kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(struktur_kd.Rel_Rapor_KompetensiDasar.ToString());
                            if (kd != null)
                            {
                                if (kd.Nama != null)
                                {
                                    s_html_deskripsi = "";
                                    if (struktur_kd.IsKomponenRapor)
                                    {
                                        s_html_deskripsi = "<tr>" +
                                                                "<td colspan=\"2\" style=\"background-color: white; padding: 0px;\">" +
                                                                "</td>" +
                                                                "<td colspan=\"9\" style=\"background-color: white; padding: 0px; padding-left: 60px; padding-right: 20px; padding-bottom: 15px;\">" +
                                                                   "<label style=\"font-weight: normal; color: #bfbfbf; margin-bottom: 7px; font-size: small;\">" +
                                                                        "Deskripsi dalam Rapor" +
                                                                   "</label>" +
                                                                   "<br />" +
                                                                   "<div onclick=\"ShowProgress(true); " + txtJenisDeskripsiRaporLTS.ClientID + ".value = '0'; " + txtIDDeskripsi.ClientID + ".value = '" + struktur_kd.Kode.ToString() + "'; " + btnShowInputDeskripsiLTSRapor.ClientID + ".click();\" class=\"div-like-text-input\" style=\"cursor: pointer; color: #bfbfbf;\">" +
                                                                        (
                                                                            struktur_kd.DeskripsiRapor.Replace("\"", "&#34;").Trim() != ""
                                                                            ? "<div class=\"reset-this\" style=\"width: 100%; padding: 0px; margin: 0px; font-size: small; color: grey; cursor: pointer;\">" +
                                                                                    Libs.GetHTMLNoParagraphDiAwal(struktur_kd.DeskripsiRapor.Trim()) +
                                                                              "</div>"
                                                                            : "Isi Deskripsi..."
                                                                        ) +
                                                                   "</div>" +
                                                                "</td>" +
                                                           "</tr>";
                                    }

                                    html += "<tr>";
                                    html += "<td colspan=\"2\" style=\"background-color: white; padding: 0px;\">" +
                                            "</td>";
                                    html += "<td colspan=\"7\" style=\"background-color: white; padding: 0px;\">" +
                                                "<hr style=\"margin: 0px; border-color: #ebebeb; border-width: 1px;\" />" +
                                            "</td>";
                                    html += "</tr>";

                                    span_jenis_perhitungan = "";
                                    if (struktur_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                    {
                                        span_jenis_perhitungan = "&nbsp;&nbsp;" +
                                                                 "<sup class=\"badge\" style=\"background-color: #40B3D2; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">" +
                                                                    Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.Bobot) +
                                                                 "</sup>";
                                    }
                                    else if (struktur_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                    {
                                        span_jenis_perhitungan = "&nbsp;&nbsp;" +
                                                                 "<sup class=\"badge\" style=\"background-color: #68217A; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">" +
                                                                    Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.RataRata) +
                                                                 "</sup>";
                                    }

                                    string span_kd_komponen_rapor
                                        = (
                                            struktur_kd.IsKomponenRapor
                                            ? "<sup title=\" Komponen Rapor \" class=\"badge\" style=\"background-color: #FF4081; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">" +
                                                "n-KD" +
                                                "&nbsp;" +
                                                "<i class=\"fa fa-check-circle\"></i>" +
                                              "</sup>"
                                            : ""
                                          );

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
                                                        (struktur_kd.Poin.Trim() != "" ? struktur_kd.Poin + " " : "") +
                                                        Libs.GetHTMLSimpleText(kd.Nama) +
                                                        span_jenis_perhitungan +
                                                        span_kd_komponen_rapor +
                                                  "</td>" +
                                                  "<td style=\"background-color: white; font-weight: bold; color: grey; text-align: right;\">" +
                                                        "<span title=\" Bobot Aspek Penilaian \" class=\"badge\" style=\"background-color: #f39100; border-radius: 0px;\">" +
                                                            struktur_kd.BobotAP.ToString() + "%" +
                                                        "</span>" +
                                                  "</td>"
                                                : "<td colspan=\"4\" style=\"background-color: white; font-weight: bold; color: grey; padding-left: 0px; text-align: justify;\">" +
                                                        (struktur_kd.Poin.Trim() != "" ? struktur_kd.Poin + " " : "") +
                                                        Libs.GetHTMLSimpleText(kd.Nama) +
                                                        span_jenis_perhitungan +
                                                        span_kd_komponen_rapor +
                                                  "</td>"
                                            );
                                    html += "<td style=\"background-color: white; font-weight: normal; color: grey; text-align: right; width: 150px;\">" +
                                                "<label title=\" Tambah Komponen Penilaian \" onclick=\"ShowProgress(true); " + txtIDAspekPenilaian.ClientID + ".value = '" + struktur_kd.Rel_Rapor_StrukturNilai_AP.ToString() + "'; " + txtIDKompetensiDasar.ClientID + ".value = '" + struktur_kd.Kode.ToString() + "'; " + btnShowInputKomponenPenilaianKURTILAS.ClientID + ".click();\" class=\"badge\" style=\"background-color: rgb(136, 153, 52); cursor: pointer; font-weight: normal; font-size: x-small; margin-right: 5px;\">" +
                                                    "&nbsp;" +
                                                    "<i class=\"fa fa-plus\"></i>" +
                                                    "&nbsp;&nbsp;" +
                                                    "KP" +
                                                    "&nbsp;" +
                                                "</label>" +
                                                "<label title=\" Ubah Kompetensi Dasar \" onclick=\"ShowProgress(true); " + txtIDAspekPenilaian.ClientID + ".value = '" + struktur_kd.Rel_Rapor_StrukturNilai_AP.ToString() + "'; " + txtIDKompetensiDasar.ClientID + ".value = '" + struktur_kd.Kode.ToString() + "'; " + btnShowInputEditKompetensiDasarKURTILAS.ClientID + ".click();\" title=\" Ubah \" class=\"badge\" style=\"background-color: rgb(136, 153, 52); cursor: pointer; font-weight: normal; font-size: x-small;\">" +
                                                    "&nbsp;" +
                                                    "<i class=\"fa fa-edit\"></i>" +
                                                    "&nbsp;" +
                                                "</label>" +
                                            "</td>";
                                    html += "<td style=\"background-color: white; border-top-color: #424242;\">" +
                                            "</td>";
                                    html += "</tr>" +
                                            s_html_deskripsi;

                                    //komponen penilaian
                                    int id_kp = 0;
                                    List<Rapor_StrukturNilai_KURTILAS_KP> lst_komponen_penilaian =
                                        DAO_Rapor_StrukturNilai_KURTILAS_KP.GetAllByHeader_Entity(struktur_kd.Kode.ToString());

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

                                    foreach (Rapor_StrukturNilai_KURTILAS_KP m in lst_komponen_penilaian)
                                    {
                                        Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(m.Rel_Rapor_KomponenPenilaian.ToString());
                                        if (m_kp != null)
                                        {
                                            if (m_kp.Nama != null)
                                            {
                                                s_html_deskripsi = "<tr>" +
                                                                        "<td colspan=\"2\" style=\"background-color: white; padding: 0px;\">" +
                                                                        "</td>" +
                                                                        "<td colspan=\"9\" style=\"background-color: white; padding: 0px; padding-left: 125px; padding-right: 20px; padding-bottom: 15px;\">" +
                                                                            "<label style=\"font-weight: normal; color: #bfbfbf; margin-bottom: 7px; font-size: small;\">" +
                                                                                "Deskripsi LTS" +
                                                                            "</label>" +
                                                                            "<br />" +
                                                                            "<div onclick=\"ShowProgress(true); " + txtJenisDeskripsiRaporLTS.ClientID + ".value = '2'; " + txtIDDeskripsi.ClientID + ".value = '" + m.Kode.ToString() + "'; " + btnShowInputDeskripsiLTSRapor.ClientID + ".click();\" class=\"div-like-text-input\" style=\"cursor: pointer; color: #bfbfbf;\">" +
                                                                                (
                                                                                    m.Deskripsi.Replace("\"", "&#34;").Trim() != ""
                                                                                    ? "<div class=\"reset-this\" style=\"width: 100%; padding: 0px; margin: 0px; font-size: small; color: grey; cursor: pointer;\">" +
                                                                                            //Libs.GetHTMLNoParagraphDiAwal(m.Deskripsi.Replace("\"", "&#34;").Trim()) +
                                                                                            Libs.GetHTMLNoParagraphDiAwal(m.Deskripsi.Trim()) +
                                                                                      "</div>"
                                                                                    : "Isi Deskripsi..."
                                                                                ) +
                                                                            "</div>" +
                                                                        "</td>" +
                                                                   "</tr>";
                                                s_html_deskripsi = "";
                                                //if (m.IsKomponenRapor && Libs.GetHTMLSimpleText(m_kp.Nama).ToUpper() != Libs.JenisKomponenNilaiKURTILAS.SMA.UAS.ToUpper())
                                                //{
                                                //    s_html_deskripsi += "<tr>" +
                                                //                            "<td colspan=\"2\" style=\"background-color: white; padding: 0px;\">" +
                                                //                            "</td>" +
                                                //                            "<td colspan=\"9\" style=\"background-color: white; padding: 0px; padding-left: 125px; padding-right: 20px; padding-bottom: 15px;\">" +
                                                //                               "<label style=\"font-weight: normal; color: #bfbfbf; margin-bottom: 7px; font-size: small;\">" +
                                                //                                    "Deskripsi dalam Rapor" +
                                                //                               "</label>" +
                                                //                               "<br />" +
                                                //                               "<div onclick=\"ShowProgress(true); " + txtJenisDeskripsiRaporLTS.ClientID + ".value = '1'; " + txtIDDeskripsi.ClientID + ".value = '" + m.Kode.ToString() + "'; " + btnShowInputDeskripsiLTSRapor.ClientID + ".click();\" class=\"div-like-text-input\" style=\"cursor: pointer; color: #bfbfbf;\">" +
                                                //                                   (
                                                //                                        m.DeskripsiRapor.Replace("\"", "&#34;").Trim() != ""
                                                //                                        ? "<div class=\"reset-this\" style=\"width: 100%; padding: 0px; margin: 0px; font-size: small; color: grey; cursor: pointer;\">" +
                                                //                                                Libs.GetHTMLNoParagraphDiAwal(m.DeskripsiRapor.Trim()) +
                                                //                                          "</div>"
                                                //                                        : "Isi Deskripsi..."
                                                //                                   ) +
                                                //                               "</div>" +
                                                //                            "</td>" +
                                                //                       "</tr>";
                                                //}

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
                                                            (
                                                                m.IsKomponenRapor
                                                                ? "<sup title=\" Komponen Rapor \" class=\"badge\" style=\"background-color: #FF4081; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">KR</sup>"
                                                                : ""
                                                            ) +
                                                            "<label class=\"badge\" onclick=\"ShowProgress(true); " + txtIDKompetensiDasar.ClientID + ".value = '" + struktur_kd.Kode.ToString() + "'; " + txtIDKomponenPenilaian.ClientID + ".value = '" + m.Kode.ToString() + "'; " + btnShowInputEditKomponenPenilaianKURTILAS.ClientID + ".click();\" title=\" Ubah \" style=\"background-color: rgba(0, 0, 0, 0.44); cursor: pointer; color: white; font-weight: normal; padding: 5px 7px; font-weight: normal; font-size: x-small; display: initial; float: right;\">" +
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
                                                html += "</tr>" +
                                                        s_html_deskripsi;
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
                ltrKURTILASDet.Text = "<div style=\"padding: 10px;\"><label style=\"margin: 0 auto; display: table;\">..:: Data Kosong ::..</label></div>";
            }
            else
            {
                html = "<table style=\"margin: 0px; width: 100%;\">" +
                       html +
                       "</table>";
                ltrKURTILASDet.Text = html;
            }
        }

        protected void ShowStrukturNilaiKURTILAS_OLD(string rel_strukturnilai)
        {
            ShowStrukturNilaiSikapKURTILAS(rel_strukturnilai);

            string html = "";
            ltrKURTILASDet.Text = "";

            Rapor_StrukturNilai_KURTILAS m_strukturnilai = DAO_Rapor_StrukturNilai_KURTILAS.GetByID_Entity(rel_strukturnilai);
            if (m_strukturnilai != null)
            {
                if (m_strukturnilai.TahunAjaran != null)
                {
                    html += "<tr>";
                    html += "<td colspan=\"9\" style=\"background-color: #f9f7e2; font-weight: normal; color: grey; padding: 10px;\">";
                    html += "<div class=\"row\">" +
                                "<div class=\"col-xs-12\">" +
                                    "<span style=\"font-weight: bold; color: grey;\">" +
                                        "Bobot Rapor " +
                                        "<span style=\"color: mediumvioletred; border-radius: 0px;\">PENGETAHUAN</span>" +
                                    "</span>" +
                                "</div>" +
                            "</div>" +
                            "<div class=\"row\">" +
                                "<div class=\"col-xs-6\">" +
                                    "<span style=\"font-weight: normal\">Dari Pengetahuan</span> : " +
                                    "<span style=\"font-weight: bold\">" + 
                                        Math.Round(m_strukturnilai.BobotRaporPengetahuan, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA).ToString() + 
                                    "%</span>" +
                                    "&nbsp;&nbsp;" +
                                    "<span style=\"font-weight: normal\">Dari UAS</span> : " +
                                    "<span style=\"font-weight: bold\">" +
                                        Math.Round(m_strukturnilai.BobotRaporUAS, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA).ToString() + 
                                     "%</span>" +
                                "</div>" +
                            "</div>";
                    html += "</td>";
                    html += "</tr>";
                }
            }

            string s_html_deskripsi = "";

            //header
            html += "<tr>";
            html += "<td style=\"background-color: #424242; font-weight: normal; color: white; width: 40px; border-top-style: solid; border-top-color: #363535;\">" +
                        "&nbsp;" +
                    "</td>";
            html += "<td style=\"background-color: #424242; font-weight: normal; color: white; width: 30px; border-top-style: solid; border-top-color: #363535;\">" +
                        "<img src=\"" + ResolveUrl("~/Application_CLibs/images/svg/browser.svg") + "\" style=\"height: 20px; width: 20px;\" />" +
                    "</td>";
            html += "<td colspan=\"6\" style=\"background-color: #424242; font-weight: bold; color: white; padding-left: 0px; border-top-style: solid; border-top-color: #363535;\">" +
                        "Aspek Penilaian (AP)" +
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
                        "Kompetensi Dasar (KD)" +
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
                        "Komponen Penilaian (KP)" +
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
            List<Rapor_StrukturNilai_KURTILAS_AP> lst_aspek_penilaian = DAO_Rapor_StrukturNilai_KURTILAS_AP.GetAllByHeader_Entity(rel_strukturnilai);
            foreach (Rapor_StrukturNilai_KURTILAS_AP struktur_ap in lst_aspek_penilaian)
            {
                Rapor_AspekPenilaian m_ap = DAO_Rapor_AspekPenilaian.GetByID_Entity(struktur_ap.Rel_Rapor_AspekPenilaian.ToString());
                if (m_ap != null)
                {
                    if (m_ap.Nama != null)
                    {
                        span_jenis_perhitungan = "";
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
                                    (struktur_ap.Poin.Trim() != "" ? struktur_ap.Poin + " " : "") +
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
                                    "<label title=\" Tambah Kompetensi Dasar \" onclick=\"" + txtIDAspekPenilaian.ClientID + ".value = '" + struktur_ap.Kode.ToString() + "'; " + btnShowInputKompetensiDasarKURTILAS.ClientID + ".click();\" class=\"badge\" style=\"cursor: pointer; font-weight: normal; font-size: x-small; margin-right: 5px;\">" +
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
                        List<Rapor_StrukturNilai_KURTILAS_KD> lst_kompetensi_dasar = DAO_Rapor_StrukturNilai_KURTILAS_KD.GetAllByHeader_Entity(struktur_ap.Kode.ToString());
                        foreach (Rapor_StrukturNilai_KURTILAS_KD struktur_kd in lst_kompetensi_dasar)
                        {
                            Rapor_KompetensiDasar kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(struktur_kd.Rel_Rapor_KompetensiDasar.ToString());
                            if (kd != null)
                            {
                                if (kd.Nama != null)
                                {
                                    s_html_deskripsi = "";
                                    if (struktur_kd.IsKomponenRapor)
                                    {
                                        s_html_deskripsi = "<tr>" +
                                                                "<td colspan=\"2\" style=\"background-color: white; padding: 0px;\">" +
                                                                "</td>" +
                                                                "<td colspan=\"9\" style=\"background-color: white; padding: 0px; padding-left: 60px; padding-right: 20px; padding-bottom: 15px;\">" +
                                                                   "<label style=\"font-weight: normal; color: #bfbfbf; margin-bottom: 7px; font-size: small;\">" +
                                                                        "Deskripsi dalam Rapor" +
                                                                   "</label>" +
                                                                   "<br />" +
                                                                   "<input id=\"txt_" + struktur_kd.Kode.ToString().Replace("-", "") + "\" " +
                                                                          "onkeydown=\"SetAttrDeskripsi('" + struktur_kd.Kode.ToString() + "', 'KD', this.id); SetIsAutosave('0')\" " +
                                                                          "onkeyup=\"SetIsAutosave('1')\" " +
                                                                          "type=\"text\" " +
                                                                          "value=\"" + struktur_kd.DeskripsiRapor.Replace("\"", "&#34;") + "\" " +
                                                                          "class=\"text-input\" />" +
                                                                "</td>" +
                                                           "</tr>";
                                    }

                                    html += "<tr>";
                                    html += "<td colspan=\"2\" style=\"background-color: white; padding: 0px;\">" +
                                            "</td>";
                                    html += "<td colspan=\"7\" style=\"background-color: white; padding: 0px;\">" +
                                                "<hr style=\"margin: 0px; border-color: #ebebeb; border-width: 1px;\" />" +
                                            "</td>";
                                    html += "</tr>";

                                    span_jenis_perhitungan = "";
                                    if (struktur_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                    {
                                        span_jenis_perhitungan = "&nbsp;&nbsp;" +
                                                                 "<sup class=\"badge\" style=\"background-color: #40B3D2; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">" +
                                                                    Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.Bobot) +
                                                                 "</sup>";
                                    }
                                    else if (struktur_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                                    {
                                        span_jenis_perhitungan = "&nbsp;&nbsp;" +
                                                                 "<sup class=\"badge\" style=\"background-color: #68217A; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">" +
                                                                    Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.RataRata) +
                                                                 "</sup>";
                                    }

                                    string span_kd_komponen_rapor 
                                        = (
                                            struktur_kd.IsKomponenRapor
                                            ? "<sup title=\" Komponen Rapor \" class=\"badge\" style=\"background-color: #FF4081; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">" +
                                                "n-KD" +
                                                "&nbsp;" +
                                                "<i class=\"fa fa-check-circle\"></i>" +
                                              "</sup>"
                                            : ""
                                          );

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
                                                        (struktur_kd.Poin.Trim() != "" ? struktur_kd.Poin + " " : "") +
                                                        Libs.GetHTMLSimpleText(kd.Nama) +
                                                        span_jenis_perhitungan +
                                                        span_kd_komponen_rapor +
                                                  "</td>" +
                                                  "<td style=\"background-color: white; font-weight: bold; color: grey; text-align: right;\">" +
                                                        "<span title=\" Bobot Aspek Penilaian \" class=\"badge\" style=\"background-color: #f39100; border-radius: 0px;\">" +
                                                            struktur_kd.BobotAP.ToString() + "%" +
                                                        "</span>" +
                                                  "</td>"
                                                : "<td colspan=\"4\" style=\"background-color: white; font-weight: bold; color: grey; padding-left: 0px; text-align: justify;\">" +
                                                        (struktur_kd.Poin.Trim() != "" ? struktur_kd.Poin + " " : "") +
                                                        Libs.GetHTMLSimpleText(kd.Nama) +
                                                        span_jenis_perhitungan +
                                                        span_kd_komponen_rapor +
                                                  "</td>"
                                            );
                                    html += "<td style=\"background-color: white; font-weight: normal; color: grey; text-align: right; width: 150px;\">" +
                                                "<label title=\" Tambah Komponen Penilaian \" onclick=\"" + txtIDAspekPenilaian.ClientID + ".value = '" + struktur_kd.Rel_Rapor_StrukturNilai_AP.ToString() + "'; " + txtIDKompetensiDasar.ClientID + ".value = '" + struktur_kd.Kode.ToString() + "'; " + btnShowInputKomponenPenilaianKURTILAS.ClientID + ".click();\" class=\"badge\" style=\"background-color: rgb(136, 153, 52); cursor: pointer; font-weight: normal; font-size: x-small; margin-right: 5px;\">" +
                                                    "&nbsp;" +
                                                    "<i class=\"fa fa-plus\"></i>" +
                                                    "&nbsp;&nbsp;" +
                                                    "KP" +
                                                    "&nbsp;" +
                                                "</label>" +
                                                "<label title=\" Ubah Kompetensi Dasar \" onclick=\"" + txtIDAspekPenilaian.ClientID + ".value = '" + struktur_kd.Rel_Rapor_StrukturNilai_AP.ToString() + "'; " + txtIDKompetensiDasar.ClientID + ".value = '" + struktur_kd.Kode.ToString() + "'; " + btnShowInputEditKompetensiDasarKURTILAS.ClientID + ".click();\" title=\" Ubah \" class=\"badge\" style=\"background-color: rgb(136, 153, 52); cursor: pointer; font-weight: normal; font-size: x-small;\">" +
                                                    "&nbsp;" +
                                                    "<i class=\"fa fa-edit\"></i>" +
                                                    "&nbsp;" +
                                                "</label>" +
                                            "</td>";
                                    html += "<td style=\"background-color: white; border-top-color: #424242;\">" +
                                            "</td>";
                                    html += "</tr>" +
                                            s_html_deskripsi;

                                    //komponen penilaian
                                    int id_kp = 0;
                                    List<Rapor_StrukturNilai_KURTILAS_KP> lst_komponen_penilaian =
                                        DAO_Rapor_StrukturNilai_KURTILAS_KP.GetAllByHeader_Entity(struktur_kd.Kode.ToString());

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

                                    foreach (Rapor_StrukturNilai_KURTILAS_KP m in lst_komponen_penilaian)
                                    {
                                        Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(m.Rel_Rapor_KomponenPenilaian.ToString());
                                        if (m_kp != null)
                                        {
                                            if (m_kp.Nama != null)
                                            {
                                                s_html_deskripsi = "<tr>" +
                                                                        "<td colspan=\"2\" style=\"background-color: white; padding: 0px;\">" +
                                                                        "</td>" +
                                                                        "<td colspan=\"9\" style=\"background-color: white; padding: 0px; padding-left: 125px; padding-right: 20px; padding-bottom: 15px;\">" +
                                                                            "<label style=\"font-weight: normal; color: #bfbfbf; margin-bottom: 7px; font-size: small;\">" +
                                                                                "Deskripsi LTS" +
                                                                            "</label>" +
                                                                            "<br />" +
                                                                            "<input id=\"txt_" + m.Kode.ToString().Replace("-", "") + "\" " +
                                                                                    "onkeydown=\"SetAttrDeskripsi('" + m.Kode.ToString() + "', 'KP_ITEM', this.id); SetIsAutosave('0')\" " +
                                                                                    "onkeyup=\"SetIsAutosave('1')\" " +
                                                                                    "type=\"text\" " +
                                                                                    "value=\"" + m.Deskripsi.Replace("\"", "&#34;") + "\" " +
                                                                                    "class=\"text-input\" />" +
                                                                        "</td>" +
                                                                   "</tr>";
                                                if (m.IsKomponenRapor && Libs.GetHTMLSimpleText(m_kp.Nama).ToUpper() != Libs.JenisKomponenNilaiKURTILAS.SMA.UAS.ToUpper())
                                                {
                                                    s_html_deskripsi += "<tr>" +
                                                                            "<td colspan=\"2\" style=\"background-color: white; padding: 0px;\">" +
                                                                            "</td>" +
                                                                            "<td colspan=\"9\" style=\"background-color: white; padding: 0px; padding-left: 125px; padding-right: 20px; padding-bottom: 15px;\">" +
                                                                               "<label style=\"font-weight: normal; color: #bfbfbf; margin-bottom: 7px; font-size: small;\">" +
                                                                                    "Deskripsi dalam Rapor" +
                                                                               "</label>" +
                                                                               "<br />" +
                                                                               "<input id=\"txt_" + m.Kode.ToString().Replace("-", "") + "\" " +
                                                                                      "onkeydown=\"SetAttrDeskripsi('" + m.Kode.ToString() + "', 'KP', this.id); SetIsAutosave('0')\" " +
                                                                                      "onkeyup=\"SetIsAutosave('1')\" " +
                                                                                      "type=\"text\" " +
                                                                                      "value=\"" + m.DeskripsiRapor.Replace("\"", "&#34;") + "\" " +
                                                                                      "class=\"text-input\" />" +
                                                                            "</td>" +
                                                                       "</tr>";
                                                }

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
                                                            (
                                                                m.IsKomponenRapor
                                                                ? "<sup title=\" Komponen Rapor \" class=\"badge\" style=\"background-color: #FF4081; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">KR</sup>"
                                                                : ""
                                                            ) +
                                                            "<label class=\"badge\" onclick=\"" + txtIDKompetensiDasar.ClientID + ".value = '" + struktur_kd.Kode.ToString() + "'; " + txtIDKomponenPenilaian.ClientID + ".value = '" + m.Kode.ToString() + "'; " + btnShowInputEditKomponenPenilaianKURTILAS.ClientID + ".click();\" title=\" Ubah \" style=\"background-color: rgba(0, 0, 0, 0.44); cursor: pointer; color: white; font-weight: normal; padding: 5px 7px; font-weight: normal; font-size: x-small; display: initial; float: right;\">" +
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
                                                html += "</tr>" +
                                                        s_html_deskripsi;
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
                ltrKURTILASDet.Text = "<div style=\"padding: 10px;\"><label style=\"margin: 0 auto; display: table;\">..:: Data Kosong ::..</label></div>";
            }
            else
            {
                html = "<table style=\"margin: 0px; width: 100%;\">" +
                       html +
                       "</table>";
                ltrKURTILASDet.Text = html;
            }
        }

        protected void ShowStrukturNilaiKTSP(string rel_strukturnilai)
        {
            Rapor_StrukturNilai_KTSP m_strukturnilai = DAO_Rapor_StrukturNilai_KTSP.GetByID_Entity(rel_strukturnilai);
            if (m_strukturnilai != null)
            {
                if (m_strukturnilai.TahunAjaran != null)
                {
                    InitLinkPreviewNilai(
                            m_strukturnilai.TahunAjaran, m_strukturnilai.Semester, m_strukturnilai.Rel_Kelas, m_strukturnilai.Rel_Mapel.ToString()
                        );
                }
            }

            List<Rapor_StrukturNilai_KTSP_KD> lst_kompetensi_dasar = DAO_Rapor_StrukturNilai_KTSP_KD.GetAllByHeader_Entity(rel_strukturnilai);
            string html = "";
            ltrKTSPDet.Text = "";

            //header
            html += "<tr>";
            html += "<td style=\"background-color: #424242; font-weight: normal; color: white; width: 40px; border-top-style: solid; border-top-color: #424242;\">" +
                        "<i class='fa fa-hashtag'></i>" +
                    "</td>";
            html += "<td style=\"background-color: #424242; font-weight: normal; color: white; width: 30px; border-top-style: solid; border-top-color: #424242;\">" +
                        "<img src=\"" + ResolveUrl("~/Application_CLibs/images/svg/questions.svg") + "\" style=\"height: 20px; width: 20px;\" />" +
                    "</td>";
            html += "<td colspan=\"4\" style=\"background-color: #424242; font-weight: bold; color: white; padding-left: 0px; border-top-style: solid; border-top-color: #424242;\">" +
                        "Kompetensi Dasar (KD)" +
                    "</td>";
            html += "</tr>";

            html += "<tr>";
            html += "<td colspan=\"2\" style=\"background-color: #4f4f4f;\">" +
                    "</td>";
            html += "<td style=\"background-color: #4f4f4f; font-weight: normal; color: white; width: 30px;\">" +
                        "<img src=\"" + ResolveUrl("~/Application_CLibs/images/svg/test.svg") + "\" style=\"height: 20px; width: 20px;\" />" +
                    "</td>";
            html += "<td style=\"padding-left: 0px; color: white; background-color: #4f4f4f; font-weight: bold;\">" +
                        "Komponen Penilaian (KP)" +
                    "</td>";
            html += "<td style=\"background-color: #4f4f4f;\">" +
                    "</td>";
            html += "<td style=\"background-color: #4f4f4f;\">" +
                    "</td>";
            html += "</tr>";
            //end header

            int id_kd = 1;
            string span_jenis_perhitungan = "";
            foreach (Rapor_StrukturNilai_KTSP_KD m_kd in lst_kompetensi_dasar)
            {
                Rapor_KompetensiDasar kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(m_kd.Rel_Rapor_KompetensiDasar.ToString());
                if (kd != null)
                {
                    if (kd.Nama != null)
                    {
                        span_jenis_perhitungan = "";
                        if (m_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                        {
                            span_jenis_perhitungan = "&nbsp;&nbsp;" +
                                                     "<sup class=\"badge\" style=\"background-color: #40B3D2; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; float: right;\">" +
                                                        Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.Bobot) +
                                                     "</sup>";
                        }
                        else if (m_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.RataRata).ToString())
                        {
                            span_jenis_perhitungan = "&nbsp;&nbsp;" +
                                                     "<sup class=\"badge\" style=\"background-color: #68217A; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial; float: right;\">" +
                                                        Libs.GetDeskripsiJenisPerhitungan(Libs.JenisPerhitunganNilai.RataRata) +
                                                     "</sup>";
                        }

                        html += "<tr>";
                        html += "<td style=\"background-color: white; font-weight: normal; color: #bfbfbf; width: 40px;\">" +
                                    id_kd.ToString() +
                                "</td>";
                        html += "<td style=\"background-color: white; font-weight: normal; color: #bfbfbf; width: 30px;\">" +
                                    "<div class=\"checkbox checkbox-adv\" style=\"margin: 0 auto;\">" +
                                        "<label for=\"chk_kd_" + m_kd.Kode.ToString().Replace("-", "_") + "\">" +
                                            "<input value=\"" + m_kd.Kode.ToString() + "\" " +
                                                    "class=\"access-hide\" " +
                                                    "id=\"chk_kd_" + m_kd.Kode.ToString().Replace("-", "_") + "\" " +
                                                    "name=\"chk_kd[]\" " +
                                                    "type=\"checkbox\">" +
										    "<span class=\"checkbox-circle\"></span><span class=\"checkbox-circle-check\"></span><span class=\"checkbox-circle-icon icon\">done</span>" +
									    "</label>" +
								    "</div>" +
                                "</td>";
                        html += "<td colspan=\"2\" style=\"background-color: white; font-weight: bold; color: grey; padding-left: 0px;\">" + 
                                    (m_kd.Poin.Trim() != "" ? m_kd.Poin + " " : "") +
                                    Libs.GetHTMLSimpleText(kd.Nama) + 
                                "</td>";
                        html += "<td style=\"background-color: white; font-weight: bold; color: grey; text-align: right;\">" +
                                    (
                                        IsMapelEkskul(m_strukturnilai.Rel_Mapel.ToString())
                                        ? ""
                                        : "<span title=\" Bobot Rapor PPK \" class=\"badge\" style=\"background-color: #40B3D2; border-radius: 0px;\">" +
                                                m_kd.BobotRaporPPK.ToString() + "%" +
                                          "</span>" +
                                          "<span title=\" Bobot Rapor Praktik \" class=\"badge\" style=\"background-color: #68217A; border-radius: 0px;\">" +
                                                m_kd.BobotRaporP.ToString() + "%" +
                                          "</span>"
                                    ) +
                                    span_jenis_perhitungan +
                                "</td>";
                        html += "<td style=\"background-color: white; font-weight: normal; color: grey; text-align: right\">" +
                                    "<label title=\" Tambah Komponen Penilaian \" onclick=\"" + txtIDKompetensiDasar.ClientID + ".value = '" + m_kd.Kode.ToString() + "'; " + btnShowInputKomponenPenilaian.ClientID + ".click();\" class=\"badge\" style=\"cursor: pointer; font-weight: normal; font-size: x-small; margin-right: 5px;\">" +
                                        "&nbsp;" +
                                        "<i class=\"fa fa-plus\"></i>" +
                                        "&nbsp;&nbsp;" +
                                        "KP" +
                                        "&nbsp;" +
                                    "</label>" +
                                    "<label onclick=\"" + txtIDKompetensiDasar.ClientID + ".value = '" + m_kd.Kode.ToString() + "'; " + btnShowInputEditKompetensiDasar.ClientID + ".click();\" title=\" Ubah Kompetensi Dasar \" class=\"badge\" style=\"cursor: pointer; font-weight: normal; font-size: x-small;\">" +
                                        "&nbsp;" +
                                        "<i class=\"fa fa-edit\"></i>" +
                                        "&nbsp;" +
                                    "</label>" +
                                "</td>";
                        html += "</tr>";

                        //komponen penilaian
                        int id = 0;
                        List<Rapor_StrukturNilai_KTSP_KP> lst_komponen_penilaian =
                            DAO_Rapor_StrukturNilai_KTSP_KP.GetAllByHeader_Entity(m_kd.Kode.ToString());
                        foreach (Rapor_StrukturNilai_KTSP_KP m in lst_komponen_penilaian)
                        {
                            Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(m.Rel_Rapor_KomponenPenilaian.ToString());
                            if (m_kp != null)
                            {
                                if (m_kp.Nama != null)
                                {
                                    html += "<tr>";
                                    html += "<td colspan=\"2\" style=\"background-color: white; padding: 0px;\">" +
                                            "</td>";
                                    html += "<td colspan=\"4\" style=\"background-color: white; padding: 0px;\">" +
                                            "<hr style=\"margin: 0px; border-color: #ebebeb;\" />" +
                                            "</td>";
                                    html += "</tr>";

                                    string bg_jenis = "#68217A";
                                    switch (m.Jenis)
                                    {
                                        case Libs.JenisKomponenNilaiKTSP.SMA.PPK:
                                            bg_jenis = "#40B3D2";
                                            break;
                                        case Libs.JenisKomponenNilaiKTSP.SMA.PRAKTIK:
                                            bg_jenis = "#68217A";
                                            break;
                                    }
                                    
                                    html += "<tr>";
                                    html += "<td colspan=\"2\" style=\"background-color: white;\">" +
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
                                                (
                                                    !(IsMapelEkskulByStrukturNilaiKTSP(txtID.Value) && m_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                                    ? ""
                                                    : "&nbsp;&nbsp;" +
                                                      "&nbsp;&nbsp;" +
                                                      (
                                                        !IsMapelEkskulByStrukturNilaiKTSP(txtID.Value)
                                                        ? "<sup class=\"badge\" style=\"background-color: " + bg_jenis + "; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">" + m.Jenis + "</sup>"
                                                        : ""
                                                      ) +
                                                      (
                                                            m.BobotNKD > 0
                                                            ? "<sup class=\"badge\" style=\"background-color: #446D8C; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: xx-small; padding-bottom: 2px; padding-top: 2px; display: initial;\">" + m.BobotNKD.ToString() + "%</sup>"
                                                            : ""
                                                      )
                                                ) +

                                                "<sup onclick=\"" + txtIDKompetensiDasar.ClientID + ".value = '" + m_kd.Kode.ToString() + "'; " + txtIDKomponenPenilaian.ClientID + ".value = '" + m.Kode.ToString() + "'; " + btnShowInputEditKomponenPenilaian.ClientID + ".click();\" title=\" Ubah \" style=\"cursor: pointer; color: grey; font-weight: normal; border-radius: 0px; padding-left: 7px; padding-right: 7px; font-weight: bold; font-size: small; padding-bottom: 2px; padding-top: 2px; display: initial; float: right;\">" +
                                                    "&nbsp;" +
                                                    "<i class=\"fa fa-edit\"></i>" +
                                                    "&nbsp;" +
                                                "</sup>" +
                                            "</td>";
                                    html += "<td style=\"background-color: white;\">" +
                                            "</td>";
                                    html += "<td style=\"background-color: white;\">" +
                                            "</td>";
                                    html += "</tr>";
                                }
                            }                            
                            id++;
                        }                        

                        if (id_kd < lst_kompetensi_dasar.Count)
                        {
                            html += "<tr>";
                            html += "<td colspan=\"6\" style=\"background-color: white; padding: 0px;\">" +
                                        "<hr style=\"margin: 0px; border-color: #ebebeb; border-width: " + (lst_komponen_penilaian.Count > 0 ? "2" : "1") + "px;\" />" +
                                    "</td>";
                            html += "</tr>";
                        }
                        
                        id_kd++;
                    }
                }
            }
            if (lst_kompetensi_dasar.Count == 0 || rel_strukturnilai.Trim() == "")
            {
                ltrKTSPDet.Text = "<div style=\"padding: 10px;\"><label style=\"margin: 0 auto; display: table;\">..:: Data Kosong ::..</label></div>";
            }
            else
            {
                html = "<table style=\"margin: 0px; width: 100%;\">" +
                       html +
                       "</table>";
                ltrKTSPDet.Text = html;
            }
        }

        protected void lnkOKStrukturNilai_Click(object sender, EventArgs e)
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
                DAO_Rapor_StrukturNilai_KTSP_KD.Insert(
                    new Rapor_StrukturNilai_KTSP_KD
                    {
                        Poin = txtPoinKompetensiDasar.Text,
                        Rel_Rapor_StrukturNilai = new Guid(txtID.Value),
                        Rel_Rapor_KompetensiDasar = kode_kompetensi_dasar,
                        BobotRaporPPK = Libs.GetStringToDecimal(txtBobotRaporPPK.Text),
                        BobotRaporP = Libs.GetStringToDecimal(txtBobotRaporP.Text),
                        JenisPerhitungan = cboJenisPerhitunganKompetensiDasar.SelectedValue
                    }, Libs.LOGGED_USER_M.UserID);

                ShowStrukturNilaiKTSP(txtID.Value);
                InitFieldsInputKompetensiDasar();
                txtKeyAction.Value = JenisAction.AddKDWithMessage.ToString();
            }
            else
            {
                DAO_Rapor_StrukturNilai_KTSP_KD.Update(
                    new Rapor_StrukturNilai_KTSP_KD
                    {
                        Kode = new Guid(txtIDKompetensiDasar.Value),
                        Poin = txtPoinKompetensiDasar.Text,
                        Rel_Rapor_StrukturNilai = new Guid(txtID.Value),
                        Rel_Rapor_KompetensiDasar = kode_kompetensi_dasar,
                        BobotRaporPPK = Libs.GetStringToDecimal(txtBobotRaporPPK.Text),
                        BobotRaporP = Libs.GetStringToDecimal(txtBobotRaporP.Text),
                        JenisPerhitungan = cboJenisPerhitunganKompetensiDasar.SelectedValue
                    }, Libs.LOGGED_USER_M.UserID);

                ShowStrukturNilaiKTSP(txtID.Value);
                InitFieldsInputKompetensiDasar();
                txtKeyAction.Value = JenisAction.DoUpdate.ToString();
            }
        }

        protected void InitFieldsInputKompetensiDasar()
        {
            txtIDKompetensiDasar.Value = "";
            txtPoinKompetensiDasar.Text = "";
            txtKompetensiDasarVal.Value = "";
            txtKompetensiDasar.Text = "";
            cboJenisPerhitunganKompetensiDasar.SelectedValue = "";
            txtBobotRaporPPK.Text = "";
            txtBobotRaporP.Text = "";
        }

        public static bool IsMapelEkskul(string rel_mapel)
        {
            Mapel m_mapel = DAO_Mapel.GetByID_Entity(rel_mapel);
            if (m_mapel != null)
            {
                if (m_mapel.Nama != null)
                {

                    if (m_mapel.Jenis == Libs.JENIS_MAPEL.EKSKUL ||
                        m_mapel.Jenis == Libs.JENIS_MAPEL.EKSTRAKURIKULER)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }

            return false;
        }

        protected bool IsMapelEkskulByStrukturNilaiKTSP(string rel_sn)
        {
            Rapor_StrukturNilai_KTSP m_sn = DAO_Rapor_StrukturNilai_KTSP.GetByID_Entity(rel_sn);
            if (m_sn != null)
            {
                if (m_sn.TahunAjaran != null)
                {

                    Mapel m_mapel = DAO_Mapel.GetByID_Entity(m_sn.Rel_Mapel.ToString());
                    if (m_mapel != null)
                    {
                        if (m_mapel.Nama != null)
                        {

                            if (m_mapel.Jenis == Libs.JENIS_MAPEL.EKSKUL ||
                                m_mapel.Jenis == Libs.JENIS_MAPEL.EKSTRAKURIKULER)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                        }
                    }

                }
            }

            return false;
        }

        protected bool IsMapelEkskulByStrukturNilaiKURTILAS(string rel_sn)
        {
            Rapor_StrukturNilai_KURTILAS m_sn = DAO_Rapor_StrukturNilai_KURTILAS.GetByID_Entity(rel_sn);
            if (m_sn != null)
            {
                if (m_sn.TahunAjaran != null)
                {

                    Mapel m_mapel = DAO_Mapel.GetByID_Entity(m_sn.Rel_Mapel.ToString());
                    if (m_mapel != null)
                    {
                        if (m_mapel.Nama != null)
                        {

                            if (m_mapel.Jenis == Libs.JENIS_MAPEL.EKSKUL ||
                                m_mapel.Jenis == Libs.JENIS_MAPEL.EKSTRAKURIKULER)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                        }
                    }

                }
            }

            return false;
        }

        protected void ShowInputKompetensiDasar()
        {
            InitFieldsInputKompetensiDasar();

            Rapor_StrukturNilai_KTSP m_sn = DAO_Rapor_StrukturNilai_KTSP.GetByID_Entity(txtID.Value);
            if (m_sn != null)
            {
                if (m_sn.TahunAjaran != null)
                {
                    if (IsMapelEkskul(m_sn.Rel_Mapel.ToString()))
                    {
                        txtBobotRaporPPK.Text = "100";
                        txtBobotRaporP.Text = "0";
                        div_bobot_rapor_ktsp.Attributes["style"] = "position: absolute; left: -10000px; top: -10000px;";
                    }
                    else
                    {
                        div_bobot_rapor_ktsp.Attributes["style"] = "margin-left: 15px; margin-right: 15px;";
                    }
                }
            }

            txtKeyAction.Value = JenisAction.DoShowInputKompetensiDasar.ToString();
        }

        protected void InitFieldsInputKompetensiDasarKURTILAS(string kode = "")
        {
            string html = "";
            string jenis_mapel = "";

            txtIDKompetensiDasar.Value = "";
            txtPoinKompetensiDasarKURTILAS.Text = "";
            ltrKDKurtilas.Text = "";
            cboJenisPerhitunganKompetensiDasarKURTILAS.SelectedValue = "";
            chkNKDIsKomponenRapor.Checked = true;

            Rapor_StrukturNilai_KURTILAS m_sn = DAO_Rapor_StrukturNilai_KURTILAS.GetByID_Entity(txtID.Value.Trim());
            if (m_sn != null)
            {
                if (m_sn.TahunAjaran != null)
                {
                    Mapel m_mapel = DAO_Mapel.GetByID_Entity(m_sn.Rel_Mapel.ToString());
                    if (m_mapel != null)
                    {
                        if (m_mapel.Nama != null)
                        {
                            jenis_mapel = m_mapel.Jenis;
                        }
                    }
                }
            }

            List<Rapor_KompetensiDasar> lst_kd = DAO_Rapor_KompetensiDasar.GetAll_Entity();
            foreach (Rapor_KompetensiDasar m_kd in lst_kd)
            {
                if (
                        Libs.GetHTMLSimpleText(m_kd.Nama).ToUpper() == Libs.JenisKomponenNilaiKURTILAS.SMA.KETERAMPILAN.Trim().ToUpper() ||
                        Libs.GetHTMLSimpleText(m_kd.Nama).ToUpper() == Libs.JenisKomponenNilaiKURTILAS.SMA.PENGETAHUAN.Trim().ToUpper() ||
                        Libs.GetHTMLSimpleText(m_kd.Nama).ToUpper() == Libs.JenisKomponenNilaiKURTILAS.SMA.UAS.Trim().ToUpper()
                   )
                {
                    string s_id = "chk_" + m_kd.Kode.ToString().Replace("-", "_");
                    html += "<div class=\"row\">" +
                                "<div class=\"col-xs-12\">" +
                                    "<div class=\"form-group form-group-label\" style=\"margin-bottom: 5px; padding-bottom: 5px; margin-top: 0px; margin-bottom: 0px;\">" +
                                        "<div class=\"radiobtn radiobtn-adv\" style=\"margin-top: 0px; margin-bottom: 0px;\">" +
                                            "<label for=\"" + s_id + "\" style=\"font-weight: bold; color: grey;\">" +
                                                "<input " + (
                                                                kode.ToUpper().Trim() == m_kd.Kode.ToString().ToUpper().Trim()
                                                                ? " checked "
                                                                : (
                                                                    (
                                                                        (
                                                                            jenis_mapel == Libs.JENIS_MAPEL.EKSTRAKURIKULER ||
                                                                            jenis_mapel == Libs.JENIS_MAPEL.EKSKUL
                                                                        ) &&
                                                                        Libs.GetHTMLSimpleText(m_kd.Nama).ToUpper() == Libs.JenisKomponenNilaiKURTILAS.SMA.PENGETAHUAN.Trim().ToUpper()
                                                                    )
                                                                    ? " checked "
                                                                    : ""
                                                                  )
                                                            ) +
                                                    " onchange=\"ValidateInputKDKurtilasNoProgress();\" " +
                                                    " value=\"" + m_kd.Kode.ToString() + "\" " +
                                                    " name=\"rdo_kd_kurtilas[]\" " +
                                                    " type=\"radio\" " +
                                                    " id=\"" + s_id + "\" " +
                                                    " class=\"access-hide\" />" +

                                                "<span class=\"radiobtn-circle\"></span>" +
                                                "<span class=\"radiobtn-circle-check\"></span>" +
                                                "<span style=\"font-weight: bold; color: black;\">" +
                                                    Libs.GetHTMLSimpleText(m_kd.Nama) +
                                                "</span>" +
                                            "</label>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                            "</div>";
                }
            }

            if (txtID.Value.Trim() != "")
            {   
                if (jenis_mapel == Libs.JENIS_MAPEL.EKSTRAKURIKULER ||
                   jenis_mapel == Libs.JENIS_MAPEL.EKSKUL)
                {
                    div_kd_kurtilas.Attributes["style"] = "position: absolute; left: -10000px; top: -10000px;";
                    div_poin_kd_kurtilas.Attributes["style"] = "position: absolute; left: -10000px; top: -10000px;";
                }
                else
                {
                    div_kd_kurtilas.Attributes["style"] = "margin-left: 15px; margin-right: 15px;";
                    div_poin_kd_kurtilas.Attributes["style"] = "margin-left: 15px; margin-right: 15px;";
                }
            }
            else
            {
                html = "";
            }

            ltrKDKurtilas.Text = html;
        }

        protected void ShowInputKompetensiDasarKURTILAS()
        {
            InitFieldsInputKompetensiDasarKURTILAS();
            txtKeyAction.Value = JenisAction.DoShowInputKompetensiDasarKurtilas.ToString();
        }

        protected void btnShowInputKompetensiDasar_Click(object sender, EventArgs e)
        {
            ShowInputKompetensiDasar();
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
                DAO_Rapor_KomponenPenilaian.Insert(new Rapor_KomponenPenilaian
                {
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
                    DAO_Rapor_StrukturNilai_KTSP_KP.Insert(
                        new Rapor_StrukturNilai_KTSP_KP
                        {
                            Rel_Rapor_StrukturNilai_KTSP_KD = new Guid(txtIDKompetensiDasar.Value),
                            Rel_Rapor_KomponenPenilaian = kode_komponen_penilaian,
                            BobotNKD = Libs.GetStringToDecimal(txtBobotNKD.Text),
                            Jenis = cboJenisKomponenPenilaian.SelectedValue
                        }, Libs.LOGGED_USER_M.UserID
                    );

                    ShowStrukturNilaiKTSP(txtID.Value);
                    InitFieldsInputKomponenPenilaian();
                    txtKeyAction.Value = JenisAction.AddKPWithMessage.ToString();
                }
                else
                {
                    DAO_Rapor_StrukturNilai_KTSP_KP.Update(
                        new Rapor_StrukturNilai_KTSP_KP
                        {
                            Kode = new Guid(txtIDKomponenPenilaian.Value),
                            Rel_Rapor_StrukturNilai_KTSP_KD = new Guid(txtIDKompetensiDasar.Value),
                            Rel_Rapor_KomponenPenilaian = kode_komponen_penilaian,
                            BobotNKD = Libs.GetStringToDecimal(txtBobotNKD.Text),
                            Jenis = cboJenisKomponenPenilaian.SelectedValue
                        }, Libs.LOGGED_USER_M.UserID
                    );

                    ShowStrukturNilaiKTSP(txtID.Value);
                    InitFieldsInputKomponenPenilaian();
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
            }
        }

        protected void InitFieldsInputKomponenPenilaian()
        {
            txtIDKomponenPenilaian.Value = "";
            txtKomponenPenilaianVal.Value = "";
            txtKomponenPenilaian.Text = "";
            Libs.JenisKomponenNilaiKTSP.SMA.ListToDropdown(cboJenisKomponenPenilaian);
            cboJenisKomponenPenilaian.SelectedValue = "";
            if (IsMapelEkskulByStrukturNilaiKTSP(txtID.Value))
            {
                cboJenisKomponenPenilaian.SelectedValue = Libs.JenisKomponenNilaiKTSP.SMA.PPK;
            }
            txtBobotNKD.Text = "";
        }

        protected void InitFieldsInputKomponenPenilaianKURTILAS(bool hapus_id = true)
        {
            if (hapus_id) txtIDKomponenPenilaian.Value = "";
            txtKomponenPenilaianKURTILASVal.Value = "";
            txtKomponenPenilaianKURTILAS.Text = "";
            txtBobotNKDKurtilas.Text = "";
            chkIsKomponenRapor.Checked = false;
        }

        protected void btnShowInputKomponenPenilaian_Click(object sender, EventArgs e)
        {
            InitFieldsInputKomponenPenilaian();
            Rapor_StrukturNilai_KTSP_KD m_sn_kd = DAO_Rapor_StrukturNilai_KTSP_KD.GetByID_Entity(txtIDKompetensiDasar.Value);
            if (m_sn_kd != null)
            {
                if (m_sn_kd.JenisPerhitungan != null)
                {

                    if (IsMapelEkskulByStrukturNilaiKTSP(txtID.Value))
                    {
                        div_jenis_komponen_penilaian_ktsp.Attributes["style"] = "position: absolute; left: -10000px; top: -10000px;";
                        if (m_sn_kd.JenisPerhitungan != ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                        {
                            div_bobot_komponen_penilaian_ktsp.Attributes["style"] = "position: absolute; left: -10000px; top: -10000px;";
                        }
                        else
                        {
                            div_bobot_komponen_penilaian_ktsp.Attributes["style"] = "margin-left: 15px; margin-right: 15px;";
                        }
                        cboJenisKomponenPenilaian.SelectedValue = Libs.JenisKomponenNilaiKTSP.SMA.PPK;
                        txtBobotNKD.Text = "0";
                    }
                    else
                    {
                        div_jenis_komponen_penilaian_ktsp.Attributes["style"] = "margin-left: 15px; margin-right: 15px;";
                        div_bobot_komponen_penilaian_ktsp.Attributes["style"] = "margin-left: 15px; margin-right: 15px;";
                    }
                    txtKeyAction.Value = JenisAction.DoShowInputKomponenPenilaian.ToString();

                }
            }
        }

        protected void btnShowInputEditKompetensiDasar_Click(object sender, EventArgs e)
        {
            if (txtIDKompetensiDasar.Value.Trim() != "")
            {
                Rapor_StrukturNilai_KTSP_KD m_sn_kd = DAO_Rapor_StrukturNilai_KTSP_KD.GetByID_Entity(txtIDKompetensiDasar.Value);
                if (m_sn_kd != null)
                {
                    if (m_sn_kd.Poin != null)
                    {
                        string kompetensi_dasar = "";
                        Rapor_KompetensiDasar m_kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(m_sn_kd.Rel_Rapor_KompetensiDasar.ToString());
                        if (m_kd != null)
                        {
                            if (m_kd.Nama != null)
                            {
                                kompetensi_dasar = m_kd.Nama;
                            }
                        }

                        Rapor_StrukturNilai_KTSP m_sn = DAO_Rapor_StrukturNilai_KTSP.GetByID_Entity(txtID.Value);
                        if (m_sn != null)
                        {
                            if (m_sn.TahunAjaran != null)
                            {
                                if (IsMapelEkskul(m_sn.Rel_Mapel.ToString()))
                                {
                                    div_bobot_rapor_ktsp.Attributes["style"] = "position: absolute; left: -10000px; top: -10000px;";
                                }
                                else
                                {
                                    div_bobot_rapor_ktsp.Attributes["style"] = "margin-left: 15px; margin-right: 15px;";
                                }
                            }
                        }   

                        txtPoinKompetensiDasar.Text = m_sn_kd.Poin;
                        txtKompetensiDasarVal.Value = kompetensi_dasar;
                        txtKompetensiDasar.Text = kompetensi_dasar;
                        txtBobotRaporPPK.Text = m_sn_kd.BobotRaporPPK.ToString();
                        txtBobotRaporP.Text = m_sn_kd.BobotRaporP.ToString();
                        cboJenisPerhitunganKompetensiDasar.SelectedValue = m_sn_kd.JenisPerhitungan;
                        txtKeyAction.Value = JenisAction.DoShowInputKompetensiDasar.ToString();
                    }                    
                }                
            }
        }

        protected void btnShowInputEditKomponenPenilaian_Click(object sender, EventArgs e)
        {
            if (txtIDKomponenPenilaian.Value.Trim() != "")
            {
                Rapor_StrukturNilai_KTSP_KD m_sn_kd = DAO_Rapor_StrukturNilai_KTSP_KD.GetByID_Entity(txtIDKompetensiDasar.Value);
                if (m_sn_kd != null)
                {
                    if (m_sn_kd.JenisPerhitungan != null)
                    {

                        Rapor_StrukturNilai_KTSP_KP m_sn_kp = DAO_Rapor_StrukturNilai_KTSP_KP.GetByID_Entity(txtIDKomponenPenilaian.Value);
                        if (m_sn_kp != null)
                        {
                            if (m_sn_kp.Jenis != null)
                            {
                                string komponen_penilaian = "";
                                Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(m_sn_kp.Rel_Rapor_KomponenPenilaian.ToString());
                                if (m_kp != null)
                                {
                                    if (m_kp.Nama != null)
                                    {
                                        komponen_penilaian = m_kp.Nama;
                                    }
                                }

                                if (IsMapelEkskulByStrukturNilaiKTSP(txtID.Value))
                                {
                                    div_jenis_komponen_penilaian_ktsp.Attributes["style"] = "position: absolute; left: -10000px; top: -10000px;";
                                    if (m_sn_kd.JenisPerhitungan != ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                                    {
                                        div_bobot_komponen_penilaian_ktsp.Attributes["style"] = "position: absolute; left: -10000px; top: -10000px;";
                                    }
                                    else
                                    {
                                        div_bobot_komponen_penilaian_ktsp.Attributes["style"] = "margin-left: 15px; margin-right: 15px;";
                                    }
                                    cboJenisKomponenPenilaian.SelectedValue = Libs.JenisKomponenNilaiKTSP.SMA.PPK;
                                    txtBobotNKD.Text = "0";
                                }
                                else
                                {
                                    div_jenis_komponen_penilaian_ktsp.Attributes["style"] = "margin-left: 15px; margin-right: 15px;";
                                    div_bobot_komponen_penilaian_ktsp.Attributes["style"] = "margin-left: 15px; margin-right: 15px;";
                                }

                                Libs.JenisKomponenNilaiKTSP.SMA.ListToDropdown(cboJenisKomponenPenilaian);
                                txtKomponenPenilaianVal.Value = komponen_penilaian;
                                txtKomponenPenilaian.Text = komponen_penilaian;
                                txtBobotNKD.Text = m_sn_kp.BobotNKD.ToString();
                                cboJenisKomponenPenilaian.SelectedValue = m_sn_kp.Jenis;
                                txtKeyAction.Value = JenisAction.DoShowInputKomponenPenilaian.ToString();
                            }
                        }

                    }
                }              
            }
        }

        protected void lnkOKHapusItemStrukturPenilaian_Click(object sender, EventArgs e)
        {
            bool ada_data = false;
            if (mvMain.ActiveViewIndex == 1) //ktsp
            {
                if (txtParseIDKompetensiDasar.Value.Trim() != "")
                {
                    string[] arr_sel_kompetensi_dasar = txtParseIDKompetensiDasar.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in arr_sel_kompetensi_dasar)
                    {
                        DAO_Rapor_StrukturNilai_KTSP_KD.Delete(item, Libs.LOGGED_USER_M.UserID);
                        ada_data = true;
                    }
                }

                if (txtParseIDKomponenPenilaian.Value.Trim() != "")
                {
                    string[] arr_sel_komponen_penilaian = txtParseIDKomponenPenilaian.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in arr_sel_komponen_penilaian)
                    {
                        DAO_Rapor_StrukturNilai_KTSP_KP.Delete(item, Libs.LOGGED_USER_M.UserID);
                        ada_data = true;
                    }
                }

                if (ada_data) ShowStrukturNilaiKTSP(txtID.Value);
            }
            else if (mvMain.ActiveViewIndex == 2) //kurtilas
            {
                if (txtParseIDAspekPenilaian.Value.Trim() != "")
                {
                    string[] arr_sel_aspek_penilaian = txtParseIDAspekPenilaian.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in arr_sel_aspek_penilaian)
                    {
                        DAO_Rapor_StrukturNilai_KURTILAS_AP.Delete(item, Libs.LOGGED_USER_M.UserID);
                        ada_data = true;
                    }
                }

                if (txtParseIDKompetensiDasarSikap.Value.Trim() != "")
                {
                    string[] arr_sel_kompetensi_dasar_sikap = txtParseIDKompetensiDasarSikap.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in arr_sel_kompetensi_dasar_sikap)
                    {
                        DAO_Rapor_StrukturNilai_KURTILAS_Sikap.Delete(item, Libs.LOGGED_USER_M.UserID);
                        ada_data = true;
                    }
                }

                if (txtParseIDKompetensiDasar.Value.Trim() != "")
                {
                    string[] arr_sel_kompetensi_dasar = txtParseIDKompetensiDasar.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in arr_sel_kompetensi_dasar)
                    {
                        DAO_Rapor_StrukturNilai_KURTILAS_KD.Delete(item, Libs.LOGGED_USER_M.UserID);
                        ada_data = true;
                    }
                }

                if (txtParseIDKomponenPenilaian.Value.Trim() != "")
                {
                    string[] arr_sel_komponen_penilaian = txtParseIDKomponenPenilaian.Value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in arr_sel_komponen_penilaian)
                    {
                        DAO_Rapor_StrukturNilai_KURTILAS_KP.Delete(item, Libs.LOGGED_USER_M.UserID);
                        ada_data = true;
                    }
                }

                if(ada_data) ShowStrukturNilaiKURTILAS(txtID.Value);
            }

            if (ada_data)
            {
                txtParseIDAspekPenilaian.Value = "";
                txtParseIDKompetensiDasar.Value = "";
                txtParseIDKompetensiDasarSikap.Value = "";
                txtParseIDKomponenPenilaian.Value = "";
                txtKeyAction.Value = JenisAction.DoDelete.ToString();
            }
        }

        protected void InitFieldsInputAspekPenilaian(bool hapus_id = true)
        {
            if (hapus_id) txtIDAspekPenilaian.Value = "";
            txtPoinAspekPenilaian.Text = "";
            txtAspekPenilaianVal.Value = "";
            txtAspekPenilaian.Text = "";
        }

        protected void ShowInputAspekPenilaian()
        {
            InitFieldsInputAspekPenilaian();
            if (txtID.Value.Trim() != "")
            {
                Rapor_StrukturNilai_KURTILAS m_struktur = DAO_Rapor_StrukturNilai_KURTILAS.GetByID_Entity(txtID.Value);
                if (m_struktur != null)
                {
                    if (m_struktur.JenisPerhitungan != null)
                    {
                        //get struktur nilai
                        if (txtIDAspekPenilaian.Value.Trim() != "")
                        {
                            Rapor_StrukturNilai_KURTILAS_AP m_struktur_ap = DAO_Rapor_StrukturNilai_KURTILAS_AP.GetByID_Entity(txtIDAspekPenilaian.Value);
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
                                }
                            }
                        }
                        //end get struktur nilai

                        txtKeyAction.Value = JenisAction.DoShowInputAspekPenilaian.ToString();
                    }
                }
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
                    DAO_Rapor_StrukturNilai_KURTILAS_AP.Insert(
                        new Rapor_StrukturNilai_KURTILAS_AP
                        {
                            Poin = txtPoinAspekPenilaian.Text,
                            Rel_Rapor_StrukturNilai = new Guid(txtID.Value),
                            Rel_Rapor_AspekPenilaian = kode_aspek_penilaian,
                            JenisPerhitungan = "",
                            BobotRapor = 0
                        },
                        Libs.LOGGED_USER_M.UserID
                    );

                    ShowStrukturNilaiKURTILAS(txtID.Value);
                    InitFieldsInputAspekPenilaian();
                    txtKeyAction.Value = JenisAction.AddAPWithMessage.ToString();
                }
                else
                {
                    DAO_Rapor_StrukturNilai_KURTILAS_AP.Update(
                        new Rapor_StrukturNilai_KURTILAS_AP
                        {
                            Kode = new Guid(txtIDAspekPenilaian.Value),
                            Poin = txtPoinAspekPenilaian.Text,
                            Rel_Rapor_StrukturNilai = new Guid(txtID.Value),
                            Rel_Rapor_AspekPenilaian = kode_aspek_penilaian,
                            JenisPerhitungan = "",
                            BobotRapor = 0
                        },
                        Libs.LOGGED_USER_M.UserID
                    );

                    ShowStrukturNilaiKURTILAS(txtID.Value);
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
            }
        }

        protected void btnShowDataListFromKURTILASDet_Click(object sender, EventArgs e)
        {
            mvMain.ActiveViewIndex = 0;
            this.Master.ShowHeaderTools = true;
            BindListView(true, Libs.GetQ());
            txtKeyAction.Value = JenisAction.ShowDataList.ToString();
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
                Rapor_StrukturNilai_KURTILAS_AP m_struktur_ap = DAO_Rapor_StrukturNilai_KURTILAS_AP.GetByID_Entity(txtIDAspekPenilaian.Value);
                if (m_struktur_ap != null)
                {
                    if (m_struktur_ap.Poin != null)
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
                        txtKeyAction.Value = JenisAction.DoShowInputAspekPenilaian.ToString();
                    }
                }
            }
        }

        protected void ShowInputOnStrukturPenilaian()
        {
            if (mvMain.ActiveViewIndex == 1) //struktur penilaian ktsp
            {
                ShowInputKompetensiDasar();
            }
            else if (mvMain.ActiveViewIndex == 2) //struktur penilaian kurtilas
            {
                ShowInputAspekPenilaian();
            }
        }

        protected void btnShowInputOnStrukturPenilaian_Click(object sender, EventArgs e)
        {
            ShowInputOnStrukturPenilaian();
        }

        protected void btnShowInputKompetensiDasarKURTILAS_Click(object sender, EventArgs e)
        {
            ShowInputKompetensiDasarKURTILAS();
        }

        protected void btnShowInputEditKompetensiDasarKURTILAS_Click(object sender, EventArgs e)
        {
            if (txtIDKompetensiDasar.Value.Trim() != "")
            {
                Rapor_StrukturNilai_KURTILAS_KD m_struktur_kd = DAO_Rapor_StrukturNilai_KURTILAS_KD.GetByID_Entity(
                    txtIDKompetensiDasar.Value
                );
                if (m_struktur_kd != null)
                {
                    if (m_struktur_kd.Poin != null)
                    {
                        string id_kd = txtIDKompetensiDasar.Value;
                        InitFieldsInputKompetensiDasarKURTILAS(m_struktur_kd.Rel_Rapor_KompetensiDasar.ToString());
                        txtPoinKompetensiDasarKURTILAS.Text = m_struktur_kd.Poin;
                        txtIDKompetensiDasar.Value = id_kd;
                        cboJenisPerhitunganKompetensiDasarKURTILAS.SelectedValue = m_struktur_kd.JenisPerhitungan;
                        chkNKDIsKomponenRapor.Checked = m_struktur_kd.IsKomponenRapor;
                        txtKeyAction.Value = JenisAction.DoShowInputKompetensiDasarKurtilas.ToString();
                    }
                }
            }
        }

        protected void lnkOKKompetensiDasar_Click(object sender, EventArgs e)
        {
            if (txtIDKompetensiDasar.Value.Trim() == "")
            {
                DAO_Rapor_StrukturNilai_KURTILAS_KD.Insert(
                    new Rapor_StrukturNilai_KURTILAS_KD
                    {
                        Poin = txtPoinKompetensiDasarKURTILAS.Text,
                        Rel_Rapor_StrukturNilai_AP = new Guid(txtIDAspekPenilaian.Value),
                        Rel_Rapor_KompetensiDasar = new Guid(txtIDRelKompetensiDasar.Value),
                        BobotAP = 0,
                        JenisPerhitungan = cboJenisPerhitunganKompetensiDasarKURTILAS.SelectedValue,
                        IsKomponenRapor = chkNKDIsKomponenRapor.Checked
                    }, Libs.LOGGED_USER_M.UserID);

                ShowStrukturNilaiKURTILAS(txtID.Value);
                InitFieldsInputKompetensiDasarKURTILAS();
                txtKeyAction.Value = JenisAction.AddKDKurtilasWithMessage.ToString();
            }
            else
            {
                DAO_Rapor_StrukturNilai_KURTILAS_KD.Update(
                    new Rapor_StrukturNilai_KURTILAS_KD
                    {
                        Kode = new Guid(txtIDKompetensiDasar.Value),
                        Poin = txtPoinKompetensiDasarKURTILAS.Text,
                        Rel_Rapor_StrukturNilai_AP = new Guid(txtIDAspekPenilaian.Value),
                        Rel_Rapor_KompetensiDasar = new Guid(txtIDRelKompetensiDasar.Value),
                        BobotAP = 0,
                        JenisPerhitungan = cboJenisPerhitunganKompetensiDasarKURTILAS.SelectedValue,
                        IsKomponenRapor = chkNKDIsKomponenRapor.Checked
                    }, Libs.LOGGED_USER_M.UserID);

                ShowStrukturNilaiKURTILAS(txtID.Value);
                InitFieldsInputKompetensiDasarKURTILAS();
                txtKeyAction.Value = JenisAction.DoUpdate.ToString();
            }
        }

        protected void ShowInputKomponenPenilaianKURTILAS()
        {
            InitFieldsInputKomponenPenilaianKURTILAS();
            if (txtIDKompetensiDasar.Value.Trim() != "")
            {
                Rapor_StrukturNilai_KURTILAS_KD m_struktur_kd = DAO_Rapor_StrukturNilai_KURTILAS_KD.GetByID_Entity(txtIDKompetensiDasar.Value);
                if (m_struktur_kd != null)
                {
                    if (m_struktur_kd.Poin != null)
                    {
                        div_bobot_kd_dari_kp.Visible = false;
                        if (m_struktur_kd.JenisPerhitungan == ((int)Libs.JenisPerhitunganNilai.Bobot).ToString())
                        {
                            div_bobot_kd_dari_kp.Visible = true;
                        }
                        txtKeyAction.Value = JenisAction.DoShowInputKomponenPenilaianKURTILAS.ToString();
                    }
                }
            }
        }

        protected void btnShowInputKomponenPenilaianKURTILAS_Click(object sender, EventArgs e)
        {
            ShowInputKomponenPenilaianKURTILAS();
            
        }

        protected void btnShowInputEditKomponenPenilaianKURTILAS_Click(object sender, EventArgs e)
        {
            InitFieldsInputKomponenPenilaianKURTILAS(false);
            if (txtIDKomponenPenilaian.Value.Trim() != "")
            {
                Rapor_StrukturNilai_KURTILAS_KP m = DAO_Rapor_StrukturNilai_KURTILAS_KP.GetByID_Entity(txtIDKomponenPenilaian.Value);
                if (m != null)
                {
                    if (m.Jenis != null)
                    {
                        Rapor_StrukturNilai_KURTILAS_KD m_struktur_kd = DAO_Rapor_StrukturNilai_KURTILAS_KD.GetByID_Entity(txtIDKompetensiDasar.Value);
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

                        txtKomponenPenilaianKURTILASVal.Value = komponen_penilaian;
                        txtKomponenPenilaianKURTILAS.Text = komponen_penilaian;
                        txtBobotNKDKurtilas.Text = m.BobotNK.ToString();
                        chkIsKomponenRapor.Checked = m.IsKomponenRapor;
                        txtKeyAction.Value = JenisAction.DoShowInputKomponenPenilaianKURTILAS.ToString();
                    }
                }
            }
        }

        protected void lnkOKKomponenPenilaianKURTILAS_Click(object sender, EventArgs e)
        {
            Guid kode_komponen_penilaian = Guid.NewGuid();
            if (DAO_Rapor_KomponenPenilaian.GetAll_Entity().FindAll(m => m.Nama == txtKomponenPenilaianKURTILASVal.Value).Count > 0)
            {
                kode_komponen_penilaian = DAO_Rapor_KomponenPenilaian.GetAll_Entity().FindAll(m => m.Nama == txtKomponenPenilaianKURTILASVal.Value).FirstOrDefault().Kode;
            }
            else
            {
                DAO_Rapor_KomponenPenilaian.Insert(new Rapor_KomponenPenilaian
                {
                    Kode = kode_komponen_penilaian,
                    Nama = txtKomponenPenilaianKURTILASVal.Value,
                    Alias = "",
                    Keterangan = ""
                }, Libs.LOGGED_USER_M.UserID);
            }

            if (txtIDKompetensiDasar.Value.Trim() != "")
            {
                if (txtIDKomponenPenilaian.Value.Trim() == "")
                {
                    DAO_Rapor_StrukturNilai_KURTILAS_KP.Insert(
                        new Rapor_StrukturNilai_KURTILAS_KP
                        {
                            Rel_Rapor_StrukturNilai_KD = new Guid(txtIDKompetensiDasar.Value),
                            Rel_Rapor_KomponenPenilaian = kode_komponen_penilaian,
                            Jenis = "",
                            BobotNK = Libs.GetStringToDecimal(txtBobotNKDKurtilas.Text),
                            IsKomponenRapor = chkIsKomponenRapor.Checked
                        }, Libs.LOGGED_USER_M.UserID
                    );

                    ShowStrukturNilaiKURTILAS(txtID.Value);
                    InitFieldsInputKomponenPenilaianKURTILAS();
                    txtKeyAction.Value = JenisAction.AddKPWithMessageKURTILAS.ToString();
                }
                else
                {
                    DAO_Rapor_StrukturNilai_KURTILAS_KP.Update(
                        new Rapor_StrukturNilai_KURTILAS_KP
                        {
                            Kode = new Guid(txtIDKomponenPenilaian.Value),
                            Rel_Rapor_StrukturNilai_KD = new Guid(txtIDKompetensiDasar.Value),
                            Rel_Rapor_KomponenPenilaian = kode_komponen_penilaian,
                            Jenis = "",
                            BobotNK = Libs.GetStringToDecimal(txtBobotNKDKurtilas.Text),
                            IsKomponenRapor = chkIsKomponenRapor.Checked
                        }, Libs.LOGGED_USER_M.UserID
                    );

                    ShowStrukturNilaiKURTILAS(txtID.Value);
                    InitFieldsInputKomponenPenilaianKURTILAS();
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
            }
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
                DAO_Rapor_StrukturNilai.StrukturNilai m_sn = DAO_Rapor_StrukturNilai.GetByID_Entity(txtID.Value);
                if (m_sn != null)
                {
                    if (m_sn.Kurikulum != null)
                    {

                        if (IsMapelEkskul(m_sn.Rel_Mapel.ToString()) && m_sn.Kurikulum == Libs.JenisKurikulum.SMA.KTSP)
                        {
                            DAO_Rapor_StrukturNilai_KTSP_Predikat.DeleteByHeader(txtID.Value, Libs.LOGGED_USER_M.UserID);
                            List<Rapor_StrukturNilai_KTSP_Predikat> lst_predikat = 
                                DAO_Rapor_StrukturNilai_KTSP_Predikat.GetAllByHeader_Entity(txtID.Value);

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
                            DAO_Rapor_StrukturNilai_KTSP_Predikat.Insert(new Rapor_StrukturNilai_KTSP_Predikat
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
                            txtKodePredikat2.Value = Guid.NewGuid().ToString();
                            if (lst_predikat.FindAll(
                                    m0 => m0.Urutan == 2
                                ).Count > 0)
                            {
                                txtKodePredikat2.Value = lst_predikat.FindAll(
                                    m0 => m0.Urutan == 2
                                ).FirstOrDefault().Kode.ToString();
                            }
                            DAO_Rapor_StrukturNilai_KTSP_Predikat.Insert(new Rapor_StrukturNilai_KTSP_Predikat
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
                            txtKodePredikat3.Value = Guid.NewGuid().ToString();
                            if (lst_predikat.FindAll(
                                    m0 => m0.Urutan == 3
                                ).Count > 0)
                            {
                                txtKodePredikat3.Value = lst_predikat.FindAll(
                                    m0 => m0.Urutan == 3
                                ).FirstOrDefault().Kode.ToString();
                            }
                            DAO_Rapor_StrukturNilai_KTSP_Predikat.Insert(new Rapor_StrukturNilai_KTSP_Predikat
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
                            txtKodePredikat4.Value = Guid.NewGuid().ToString();
                            if (lst_predikat.FindAll(
                                    m0 => m0.Urutan == 4
                                ).Count > 0)
                            {
                                txtKodePredikat4.Value = lst_predikat.FindAll(
                                    m0 => m0.Urutan == 4
                                ).FirstOrDefault().Kode.ToString();
                            }
                            DAO_Rapor_StrukturNilai_KTSP_Predikat.Insert(new Rapor_StrukturNilai_KTSP_Predikat
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
                            DAO_Rapor_StrukturNilai_KTSP_Predikat.Insert(new Rapor_StrukturNilai_KTSP_Predikat
                            {
                                Kode = new Guid(txtKodePredikat4.Value),
                                Rel_Rapor_StrukturNilai = new Guid(txtID.Value),
                                Predikat = txtPredikat5.Text,
                                Minimal = Libs.GetStringToDecimal(txtMinimal5.Text),
                                Maksimal = Libs.GetStringToDecimal(txtMaksimal5.Text),
                                Deskripsi = txtDeskripsi5.Text,
                                Urutan = 5
                            }, Libs.LOGGED_USER_M.UserID);
                        }
                        else
                        {
                            DAO_Rapor_StrukturNilai_KURTILAS_Predikat.DeleteByHeader(txtID.Value, Libs.LOGGED_USER_M.UserID);
                            List<Rapor_StrukturNilai_KTSP_Predikat> lst_predikat =
                                DAO_Rapor_StrukturNilai_KTSP_Predikat.GetAllByHeader_Entity(txtID.Value);

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
                            DAO_Rapor_StrukturNilai_KURTILAS_Predikat.Insert(new Rapor_StrukturNilai_KURTILAS_Predikat
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
                            DAO_Rapor_StrukturNilai_KURTILAS_Predikat.Insert(new Rapor_StrukturNilai_KURTILAS_Predikat
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
                            DAO_Rapor_StrukturNilai_KURTILAS_Predikat.Insert(new Rapor_StrukturNilai_KURTILAS_Predikat
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
                            DAO_Rapor_StrukturNilai_KURTILAS_Predikat.Insert(new Rapor_StrukturNilai_KURTILAS_Predikat
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
                            DAO_Rapor_StrukturNilai_KURTILAS_Predikat.Insert(new Rapor_StrukturNilai_KURTILAS_Predikat
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
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void btnShowInputPredikatPenilaianKURTILAS_Click(object sender, EventArgs e)
        {
            ShowDataPredikatPenilaian();
            txtKeyAction.Value = JenisAction.DoShowInputPredikat.ToString();
        }

        protected void ShowDataPredikatPenilaian()
        {
            InitFieldsPredikatPenilaian();
            var m_sn = DAO_Rapor_StrukturNilai.GetByID_Entity(txtID.Value);
            if (m_sn != null)
            {
                if (m_sn.Kurikulum != null)
                {
                    if (m_sn.Kurikulum == Libs.JenisKurikulum.SMA.KTSP)
                    {

                        Rapor_StrukturNilai_KTSP m = DAO_Rapor_StrukturNilai_KTSP.GetByID_Entity(txtID.Value);
                        if (m != null)
                        {
                            Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.Rel_Mapel.ToString());
                            string kelas = "";
                            if (m.Rel_Kelas.Trim() == "")
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
                                    lblKKM.Text = Math.Round(m.KKM, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA).ToString();
                                }
                            }

                            List<Rapor_StrukturNilai_KTSP_Predikat> lst_predikat = DAO_Rapor_StrukturNilai_KTSP_Predikat.GetAllByHeader_Entity(txtID.Value);
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
                    else if (
                            m_sn.Kurikulum == Libs.JenisKurikulum.SMA.KURTILAS ||
                            m_sn.Kurikulum == Libs.JenisKurikulum.SMA.KURTILAS_SIKAP
                        )
                    {

                        Rapor_StrukturNilai_KURTILAS m = DAO_Rapor_StrukturNilai_KURTILAS.GetByID_Entity(txtID.Value);
                        if (m != null)
                        {
                            Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.Rel_Mapel.ToString());
                            string kelas = "";
                            if (m.Rel_Kelas.Trim() == "")
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
                                    lblKKM.Text = Math.Round(m.KKM, Constantas.PEMBULATAN_DESIMAL_NILAI_SMA).ToString();
                                }
                            }

                            List<Rapor_StrukturNilai_KURTILAS_Predikat> lst_predikat = DAO_Rapor_StrukturNilai_KURTILAS_Predikat.GetAllByHeader_Entity(txtID.Value);
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

        protected void lnkOKKompetensiDasarSikap_Click(object sender, EventArgs e)
        {
            try
            {
                Guid kode_kompetensi_dasar = Guid.NewGuid();
                if (DAO_Rapor_KompetensiDasar.GetAll_Entity().FindAll(m => m.Nama == txtKompetensiDasarSikapKURTILASVal.Value).Count > 0)
                {
                    kode_kompetensi_dasar = DAO_Rapor_KompetensiDasar.GetAll_Entity().FindAll(m => m.Nama == txtKompetensiDasarSikapKURTILASVal.Value).FirstOrDefault().Kode;
                }
                else
                {
                    DAO_Rapor_KompetensiDasar.Insert(new Rapor_KompetensiDasar
                    {
                        Kode = kode_kompetensi_dasar,
                        Nama = txtKompetensiDasarSikapKURTILASVal.Value,
                        Alias = "",
                        Keterangan = ""
                    }, Libs.LOGGED_USER_M.UserID);
                }

                if (txtIDKompetensiDasarSikap.Value == "")
                {
                    DAO_Rapor_StrukturNilai_KURTILAS_Sikap.Insert(
                        new Rapor_StrukturNilai_KURTILAS_Sikap
                        {
                            Rel_Rapor_StrukturNilai = new Guid(txtID.Value),
                            Poin = txtPoinKompetensiDasarSikap.Text,
                            Rel_Rapor_KompetensiDasar = kode_kompetensi_dasar,
                            Deskripsi = txtDeskripsiKompetensiDasarSikapVal.Value
                        },
                        Libs.LOGGED_USER_M.UserID
                    );
                }
                else
                {
                    DAO_Rapor_StrukturNilai_KURTILAS_Sikap.Update(
                        new Rapor_StrukturNilai_KURTILAS_Sikap
                        {
                            Kode = new Guid(txtIDKompetensiDasarSikap.Value),
                            Rel_Rapor_StrukturNilai = new Guid(txtID.Value),
                            Poin = txtPoinKompetensiDasarSikap.Text,
                            Rel_Rapor_KompetensiDasar = kode_kompetensi_dasar,
                            Deskripsi = txtDeskripsiKompetensiDasarSikapVal.Value
                        },
                        Libs.LOGGED_USER_M.UserID
                    );
                }

                ShowStrukturNilaiSikapKURTILAS(txtID.Value);
                txtKeyAction.Value = JenisAction.DoUpdate.ToString();
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void InitInputKompetensiDasarSikap(bool init_kode = true)
        {
            if (init_kode) txtIDKompetensiDasarSikap.Value = "";
            txtPoinKompetensiDasarSikap.Text = "";
            txtKompetensiDasarSikapKURTILAS.Text = "";
            txtKompetensiDasarSikapKURTILASVal.Value = "";
            txtDeskripsiKompetensiDasarSikap.Text = "";
        }

        protected void btnShowInputEditKompetensiDasarKURTILASSikap_Click(object sender, EventArgs e)
        {
            InitInputKompetensiDasarSikap(false);
            if (txtIDKompetensiDasarSikap.Value.Trim() != "")
            {
                Rapor_StrukturNilai_KURTILAS_Sikap m = DAO_Rapor_StrukturNilai_KURTILAS_Sikap.GetByID_Entity(txtIDKompetensiDasarSikap.Value);
                if (m != null)
                {
                    if (m.Poin != null)
                    {
                        string kompetensi_dasar = "";
                        Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(m.Rel_Rapor_KompetensiDasar.ToString());
                        if (m_kp != null)
                        {
                            if (m_kp.Nama != null)
                            {
                                kompetensi_dasar = m_kp.Nama;
                            }
                        }

                        txtPoinKompetensiDasarSikap.Text = m.Poin;
                        txtKompetensiDasarSikapKURTILASVal.Value = kompetensi_dasar;
                        txtKompetensiDasarSikapKURTILAS.Text = kompetensi_dasar;
                        txtDeskripsiKompetensiDasarSikap.Text = m.Deskripsi;
                        txtDeskripsiKompetensiDasarSikapVal.Value = m.Deskripsi;
                        txtKeyAction.Value = JenisAction.DoShowInputKompetensiDasarKurtilasSikap.ToString();
                    }
                }                
            }
        }

        protected void btnShowInputKompetensiDasarKURTILASSikap_Click(object sender, EventArgs e)
        {            
            InitInputKompetensiDasarSikap();
            txtKeyAction.Value = JenisAction.DoShowInputKompetensiDasarKurtilasSikap.ToString();
        }

        protected void bntLihatData_Click(object sender, EventArgs e)
        {
            txtKeyAction.Value = JenisAction.DoShowLihatData.ToString();
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

        protected void lnkOKDeskripsiLTSRapor_Click(object sender, EventArgs e)
        {
            if (txtJenisDeskripsiRaporLTS.Value == "0")
            {
                DAO_Rapor_StrukturNilai_KURTILAS_KD.UpdateDeskripsiRapor(
                        new Guid(txtIDDeskripsi.Value),
                        txtDeskripsiLTSRaporVal.Value,
                        Libs.LOGGED_USER_M.UserID
                    );
                ShowStrukturNilaiKURTILAS(txtID.Value);
                txtKeyAction.Value = JenisAction.DoUpdate.ToString();
            }
            else if (txtJenisDeskripsiRaporLTS.Value == "1")
            {
                DAO_Rapor_StrukturNilai_KURTILAS_KP.UpdateDeskripsiRapor(
                        new Guid(txtIDDeskripsi.Value),
                        txtDeskripsiLTSRaporVal.Value,
                        Libs.LOGGED_USER_M.UserID
                    );
                ShowStrukturNilaiKURTILAS(txtID.Value);
                txtKeyAction.Value = JenisAction.DoUpdate.ToString();
            }
            else if (txtJenisDeskripsiRaporLTS.Value == "2")
            {
                DAO_Rapor_StrukturNilai_KURTILAS_KP.UpdateDeskripsi(
                        new Guid(txtIDDeskripsi.Value),
                        txtDeskripsiLTSRaporVal.Value,
                        Libs.LOGGED_USER_M.UserID
                    );
                ShowStrukturNilaiKURTILAS(txtID.Value);
                txtKeyAction.Value = JenisAction.DoUpdate.ToString();
            }
        }

        protected void btnShowInputDeskripsiLTSRapor_Click(object sender, EventArgs e)
        {
            txtDeskripsiLTSRapor.Text = "";
            txtDeskripsiLTSRaporVal.Value = "";

            string caption_deskripsi = "";
            if (txtJenisDeskripsiRaporLTS.Value == "1" || txtJenisDeskripsiRaporLTS.Value == "2")
            {
                Rapor_StrukturNilai_KURTILAS_KP m0 = DAO_Rapor_StrukturNilai_KURTILAS_KP.GetByID_Entity(txtIDDeskripsi.Value);
                if (m0 != null)
                {
                    if (m0.Jenis != null)
                    {
                        Rapor_KomponenPenilaian m_kp = DAO_Rapor_KomponenPenilaian.GetByID_Entity(m0.Rel_Rapor_KomponenPenilaian.ToString());
                        if (m_kp != null)
                        {
                            if (m_kp.Nama != null)
                            {
                                Rapor_StrukturNilai_KURTILAS_KD m1 = DAO_Rapor_StrukturNilai_KURTILAS_KD.GetByID_Entity(m0.Rel_Rapor_StrukturNilai_KD.ToString());
                                if (m1 != null)
                                {
                                    if (m1.Poin != null)
                                    {
                                        Rapor_KompetensiDasar m_kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(m1.Rel_Rapor_KompetensiDasar.ToString());
                                        if (m_kd != null)
                                        {
                                            if (m_kd.Nama != null)
                                            {
                                                Rapor_StrukturNilai_KURTILAS_AP m2 = DAO_Rapor_StrukturNilai_KURTILAS_AP.GetByID_Entity(m1.Rel_Rapor_StrukturNilai_AP.ToString());
                                                if (m2 != null)
                                                {
                                                    if (m2.JenisPerhitungan != null)
                                                    {
                                                        Rapor_AspekPenilaian m_ap = DAO_Rapor_AspekPenilaian.GetByID_Entity(m2.Rel_Rapor_AspekPenilaian.ToString());
                                                        if (m_ap != null)
                                                        {
                                                            if (m_ap.Nama != null)
                                                            {
                                                                caption_deskripsi = "<div class=\"row\">" +
                                                                                        "<div class=\"col-xs-12\" style=\"color: #bfbfbf;\">" +
                                                                                            "<i class=\"fa fa-hashtag\"></i>&nbsp;&nbsp;" +
                                                                                            "Aspek Penilaian (AP)" +
                                                                                        "</div>" +
                                                                                    "</div>" +
                                                                                    "<div class=\"row\">" +
                                                                                        "<div class=\"col-xs-12\" style=\"font-weight: bold; color: grey; padding-left: 37px; padding-right: 37px;\">" +
                                                                                            Libs.GetHTMLSimpleText(m_ap.Nama) +
                                                                                        "</div>" +
                                                                                    "</div>" +
                                                                                    "<div class=\"row\">" +
                                                                                        "<div class=\"col-xs-12\" style=\"color: #bfbfbf;\">" +
                                                                                            "<i class=\"fa fa-hashtag\"></i>&nbsp;&nbsp;" +
                                                                                            "Kompetensi Dasar (KD)" +
                                                                                        "</div>" +
                                                                                    "</div>" +
                                                                                    "<div class=\"row\">" +
                                                                                        "<div class=\"col-xs-12\" style=\"font-weight: bold; color: grey; padding-left: 37px; padding-right: 37px;\">" +
                                                                                            Libs.GetHTMLSimpleText(m_kd.Nama) +
                                                                                        "</div>" +
                                                                                    "</div>" +
                                                                                    "<div class=\"row\">" +
                                                                                        "<div class=\"col-xs-12\" style=\"color: #bfbfbf;\">" +
                                                                                            "<i class=\"fa fa-hashtag\"></i>&nbsp;&nbsp;" +
                                                                                            "Komponen Penilaian (KP)" +
                                                                                        "</div>" +
                                                                                    "</div>" +
                                                                                    "<div class=\"row\">" +
                                                                                        "<div class=\"col-xs-12\" style=\"font-weight: bold; color: grey; padding-left: 37px; padding-right: 37px;\">" +
                                                                                            Libs.GetHTMLSimpleText(m_kp.Nama) +
                                                                                        "</div>" +
                                                                                    "</div>";

                                                                txtDeskripsiLTSRapor.Text = m0.Deskripsi;
                                                                txtDeskripsiLTSRaporVal.Value = m0.Deskripsi;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Rapor_StrukturNilai_KURTILAS_KD m1 = DAO_Rapor_StrukturNilai_KURTILAS_KD.GetByID_Entity(txtIDDeskripsi.Value);
                if (m1 != null)
                {
                    if (m1.Poin != null)
                    {
                        Rapor_KompetensiDasar m_kd = DAO_Rapor_KompetensiDasar.GetByID_Entity(m1.Rel_Rapor_KompetensiDasar.ToString());
                        if (m_kd != null)
                        {
                            if (m_kd.Nama != null)
                            {
                                Rapor_StrukturNilai_KURTILAS_AP m2 = DAO_Rapor_StrukturNilai_KURTILAS_AP.GetByID_Entity(m1.Rel_Rapor_StrukturNilai_AP.ToString());
                                if (m2 != null)
                                {
                                    if (m2.JenisPerhitungan != null)
                                    {
                                        Rapor_AspekPenilaian m_ap = DAO_Rapor_AspekPenilaian.GetByID_Entity(m2.Rel_Rapor_AspekPenilaian.ToString());
                                        if (m_ap != null)
                                        {
                                            if (m_ap.Nama != null)
                                            {
                                                caption_deskripsi = "<div class=\"row\">" +
                                                                        "<div class=\"col-xs-12\" style=\"color: #bfbfbf;\">" +
                                                                            "<i class=\"fa fa-hashtag\"></i>&nbsp;&nbsp;" +
                                                                            "Aspek Penilaian (AP)" +
                                                                        "</div>" +
                                                                    "</div>" +
                                                                    "<div class=\"row\">" +
                                                                        "<div class=\"col-xs-12\" style=\"font-weight: bold; color: grey; padding-left: 37px; padding-right: 37px;\">" +
                                                                            Libs.GetHTMLSimpleText(m_ap.Nama) +
                                                                        "</div>" +
                                                                    "</div>" +
                                                                    "<div class=\"row\">" +
                                                                        "<div class=\"col-xs-12\" style=\"color: #bfbfbf;\">" +
                                                                            "<i class=\"fa fa-hashtag\"></i>&nbsp;&nbsp;" +
                                                                            "Kompetensi Dasar (KD)" +
                                                                        "</div>" +
                                                                    "</div>" +
                                                                    "<div class=\"row\">" +
                                                                        "<div class=\"col-xs-12\" style=\"font-weight: bold; color: grey; padding-left: 37px; padding-right: 37px;\">" +
                                                                            Libs.GetHTMLSimpleText(m_kd.Nama) +
                                                                        "</div>" +
                                                                    "</div>";

                                                txtDeskripsiLTSRapor.Text = m1.DeskripsiRapor;
                                                txtDeskripsiLTSRaporVal.Value = m1.DeskripsiRapor;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (txtJenisDeskripsiRaporLTS.Value == "0" || txtJenisDeskripsiRaporLTS.Value == "1")
            {
                ltrCaptionDeskripsiLTSRapor.Text = "Deskripsi Rapor";
            }
            else if (txtJenisDeskripsiRaporLTS.Value == "2")
            {   
                ltrCaptionDeskripsiLTSRapor.Text = "Deskripsi LTS";
            }

            ltrInfoDeskripsiLTSRapor.Text = caption_deskripsi;
            txtKeyAction.Value = JenisAction.DoShowInputDeskripsiLTSRapor.ToString();
        }

        protected void btnShowDataList_Click(object sender, EventArgs e)
        {
            mvMain.ActiveViewIndex = 0;
            this.Master.ShowHeaderTools = true;
            BindListView(true, Libs.GetQ());
            txtKeyAction.Value = JenisAction.ShowDataList.ToString();
        }

        public static string GetHTMLKelasMapelDetIsiNilai(string tahun_ajaran, string semester, string rel_kelas, string rel_mapel, string kurikulum)
        {
            string s_html = "";

            if (kurikulum == Libs.JenisKurikulum.SMA.KURTILAS_SIKAP) return s_html;

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
                        (
                            kurikulum == Libs.JenisKurikulum.SMA.KURTILAS
                            ? (
                                lst_nilai_kurtilas.FindAll(m0 => m0.Rel_Mapel.ToString().ToUpper().Trim() == rel_mapel.ToString().ToUpper().Trim() && m0.Rel_KelasDet.ToString().ToUpper().Trim() == item_kelas.Key.ToString().ToUpper().Trim()).Count > 0
                                ? true
                                : false
                              )
                            : (
                                lst_nilai_ktsp.FindAll(m0 => m0.Rel_Mapel.ToString().ToUpper().Trim() == rel_mapel.ToString().ToUpper().Trim() && m0.Rel_KelasDet.ToString().ToUpper().Trim() == item_kelas.Key.ToString().ToUpper().Trim()).Count > 0
                                ? true
                                : false
                              )
                        )
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

        protected void ShowListGuruPreviewPenilaian()
        {
            if (txtID.Value.Trim() != "")
            {
                DAO_Rapor_StrukturNilai.StrukturNilai m = DAO_Rapor_StrukturNilai.GetByID_Entity(txtID.Value);
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        string html = "";
                        int id_row = 0;
                        List<DAO_FormasiGuruMapelDet.FormasiGuruMapelDet_Lengkap> lst_guru = DAO_FormasiGuruMapelDet.GetByTABySMByKelasByMapel_Entity(m.TahunAjaran, m.Semester, m.Rel_Kelas, m.Rel_Mapel.ToString());
                        Mapel m_mapel = DAO_Mapel.GetByID_Entity(m.Rel_Mapel.ToString());
                        if (m_mapel != null && m_mapel.Nama != null)
                        {
                            foreach (DAO_FormasiGuruMapelDet.FormasiGuruMapelDet_Lengkap item_guru in lst_guru)
                            {
                                if (m_mapel.Jenis == Libs.JENIS_MAPEL.LINTAS_MINAT)
                                {
                                    List<string> lst_kelas_det = DAO_FormasiGuruMapelDetSiswa.GetByDistinctKelasDetHeader_Entity(item_guru.Rel_FormasiGuruMapel.ToString());
                                    foreach (var item_kelas_det in lst_kelas_det)
                                    {
                                        KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(item_kelas_det);
                                        if (m_kelas_det != null)
                                        {
                                            if (m_kelas_det.Nama != null)
                                            {
                                                string js_lihat_nilai = "DoScrollPos(); " +
                                                                        "ShowProgress(true); " +
                                                                        "setTimeout(function(){ " +
                                                                                GetJSPreviewNilai(
                                                                                    this.Page, m.TahunAjaran, m.Semester, m.Rel_Kelas, item_kelas_det, m.Rel_Mapel.ToString()
                                                                                ) +
                                                                            "}, " +
                                                                            "10 " +
                                                                        "); " +
                                                                        "setTimeout(function() { ShowProgress(false); }, 1500); " +
                                                                        "return false; ";
                                                
                                                html += "<tr class=\"" + (id_row % 2 == 0 ? "standardrow" : "oddrow") +
                                                                  "\">" +
                                                            "<td style=\"text-align: center; padding: 10px; vertical-align: middle; color: #bfbfbf;\">" +
                                                                (id_row + 1).ToString() + "." +
                                                            "</td>" +
                                                            "<td style=\"font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;\">" +
                                                                "<span style=\"color: #bfbfbf; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                                    item_guru.Rel_Guru.ToString() +
                                                                "</span>" +
                                                                "<br />" +
                                                                "<span style=\"color: grey; font-weight: bold; text-transform: none; text-decoration: none;\">" +
                                                                    item_guru.Guru +
                                                                "</span>" +
                                                                "<br />" +
                                                                "<span style=\"color: rgba(203,96,179,1); font-weight: bold; text-transform: none; text-decoration: none;\">" +
                                                                    m_kelas_det.Nama.Trim() +
                                                                "</span>" +
                                                                "&nbsp;&nbsp;" +
                                                                "<span style=\"color: rgb(255, 148, 38); font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                                    item_guru.JenisKelas +
                                                                "</span>" +
                                                            "</td>" +
                                                            "<td style=\"font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; width: 60px;\">" +
                                                                "<button onclick=\"" + js_lihat_nilai + "\" title=\" Buka \" class=\"btn btn-flat btn-brand-accent waves-attach waves-effect pull-right\" " +
                                                                    "style=\"padding: 0px; padding-left: 5px; padding-right: 5px; float: right;\">" +
                                                                    "<span style=\"color: slategrey;\">" +
                                                                        "<i class=\"fa fa-folder-open\"></i>" +
                                                                    "</span>" +
                                                                "</button>" +
                                                            "</td>" +
                                                        "</tr>";
                                                id_row++;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    string js_lihat_nilai = "DoScrollPos(); " +
                                                            "ShowProgress(true); " +
                                                            "setTimeout(function(){ " +
                                                                    GetJSPreviewNilai(
                                                                        this.Page, m.TahunAjaran, m.Semester, m.Rel_Kelas, item_guru.Rel_KelasDet.ToString(), m.Rel_Mapel.ToString()
                                                                    ) +
                                                                "}, " +
                                                                "10 " +
                                                            "); " +
                                                            "setTimeout(function() { ShowProgress(false); }, 1500); " +
                                                            "return false; ";
                                    
                                    html += "<tr class=\"" + (id_row % 2 == 0 ? "standardrow" : "oddrow") +
                                                      "\">" +
                                                "<td style=\"text-align: center; padding: 10px; vertical-align: middle; color: #bfbfbf;\">" +
                                                    (id_row + 1).ToString() + "." +
                                                "</td>" +
                                                "<td style=\"font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;\">" +
                                                    "<span style=\"color: #bfbfbf; font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                        item_guru.Rel_Guru.ToString() +
                                                    "</span>" +
                                                    "<br />" +
                                                    "<span style=\"color: grey; font-weight: bold; text-transform: none; text-decoration: none;\">" +
                                                        item_guru.Guru +
                                                    "</span>" +
                                                    "<br />" +
                                                    "<span style=\"color: rgba(203,96,179,1); font-weight: bold; text-transform: none; text-decoration: none;\">" +
                                                        item_guru.KelasDet +
                                                    "</span>" +
                                                    "&nbsp;&nbsp;" +
                                                    "<span style=\"color: rgb(255, 148, 38); font-weight: normal; text-transform: none; text-decoration: none;\">" +
                                                        item_guru.JenisKelas +
                                                    "</span>" +
                                                "</td>" +
                                                "<td style=\"font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; width: 60px;\">" +
                                                    "<button onclick=\"" + js_lihat_nilai + "\" title=\" Buka \" class=\"btn btn-flat btn-brand-accent waves-attach waves-effect pull-right\" " +
                                                        "style=\"padding: 0px; padding-left: 5px; padding-right: 5px; float: right;\">" +
                                                        "<span style=\"color: slategrey;\">" +
                                                            "<i class=\"fa fa-folder-open\"></i>" +
                                                        "</span>" +
                                                    "</button>" +
                                                "</td>" +
                                            "</tr>";
                                    id_row++;
                                }
                            }
                        }

                        if (html.Trim() != "")
                        {
                            html = "<table style=\"margin: 0px; width: 100%;\">" +
                                        html +
                                   "</table>";
                        }
                        else
                        {
                            html = "<table style=\"margin: 0px; width: 100%;\">" +
                                        "<tr>" +
                                            "<td style=\"padding: 15px; background-color: white; text-align: center; padding: 25px; color: rgb(255, 148, 38);\">..:: <i class=\"fa fa-exclamation-triangle\"></i> Data Kosong ::..</td>" +
                                        "</tr>" +
                                   "</table>";
                        }
                        ltrListMengajarPreviewPenilaian.Text = html;
                    }
                }
            }
        }

        protected void ShowListGuruPreviewPenilaianPramuka()
        {
            if (txtID.Value.Trim() != "")
            {
                DAO_Rapor_StrukturNilai.StrukturNilai m = DAO_Rapor_StrukturNilai.GetByID_Entity(txtID.Value);
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        List<FormasiGuruKelas> lst_formasi = DAO_FormasiGuruKelas.GetByUnitByTABySM_Entity(
                                GetUnit().Kode.ToString(),
                                m.TahunAjaran,
                                m.Semester
                            );

                        string html = "";
                        int id_row = 0;
                        foreach (FormasiGuruKelas item_formasi in lst_formasi)
                        {
                            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(
                                    item_formasi.Rel_KelasDet
                                );

                            if (m_kelas_det.Rel_Kelas.ToString().ToUpper().Trim() == m.Rel_Kelas.ToString().ToUpper().Trim())
                            {
                                string js_lihat_nilai = "DoScrollPos(); " +
                                                                "ShowProgress(true); " +
                                                                "setTimeout(function(){ " +
                                                                        GetJSPreviewNilaiEkskul(
                                                                            this.Page,
                                                                            txtID.Value,
                                                                            m.TahunAjaran,
                                                                            m.Semester,
                                                                            m.Rel_Kelas,
                                                                            item_formasi.Rel_KelasDet.ToString(),
                                                                            m.Rel_Mapel.ToString()
                                                                        ) +
                                                                    "}, " +
                                                                    "10 " +
                                                                "); " +
                                                                "setTimeout(function() { ShowProgress(false); }, 1500); " +
                                                                "return false; ";

                                html += "<tr class=\"" + (id_row % 2 == 0 ? "standardrow" : "oddrow") +
                                                  "\">" +
                                            "<td style=\"text-align: center; padding: 10px; vertical-align: middle; color: #bfbfbf; width: 30px;\">" +
                                                (id_row + 1).ToString() + "." +
                                            "</td>" +
                                            "<td style=\"font-weight: bold; padding: 10px; vertical-align: middle; text-align: left;\">" +
                                                "<span style=\"color: rgba(203,96,179,1); font-weight: bold; text-transform: none; text-decoration: none;\">" +
                                                    m_kelas_det.Nama +
                                                "</span>" +
                                            "</td>" +
                                            "<td style=\"font-weight: bold; padding: 10px; vertical-align: middle; text-align: left; width: 60px;\">" +
                                                "<button onclick=\"" + js_lihat_nilai + "\" title=\" Buka \" class=\"btn btn-flat btn-brand-accent waves-attach waves-effect pull-right\" " +
                                                    "style=\"padding: 0px; padding-left: 5px; padding-right: 5px; float: right;\">" +
                                                    "<span style=\"color: slategrey;\">" +
                                                        "<i class=\"fa fa-folder-open\"></i>" +
                                                    "</span>" +
                                                "</button>" +
                                            "</td>" +
                                        "</tr>";
                                id_row++;
                            }
                        }

                        if (html.Trim() != "")
                        {
                            html = "<table style=\"margin: 0px; width: 100%;\">" +
                                        html +
                                   "</table>";
                        }
                        else
                        {
                            html = "<table style=\"margin: 0px; width: 100%;\">" +
                                        "<tr>" +
                                            "<td style=\"padding: 15px; background-color: white; text-align: center; padding: 25px; color: rgb(255, 148, 38);\">..:: <i class=\"fa fa-exclamation-triangle\"></i> Data Kosong ::..</td>" +
                                        "</tr>" +
                                   "</table>";
                        }
                        ltrListMengajarPreviewPenilaian.Text = html;
                    }
                }
            }
        }

        protected void btnShowPreviewNilai_Click(object sender, EventArgs e)
        {
            ShowListGuruPreviewPenilaian();
            txtKeyAction.Value = JenisAction.DoShowPreviewNilai.ToString();
        }

        protected void lnkOKPreviewNilaiKURTILAS_Click(object sender, EventArgs e)
        {
            ShowListGuruPreviewPenilaian();
            txtKeyAction.Value = JenisAction.DoShowPreviewNilai.ToString();
        }

        protected void btnShowPreviewNilaiKelasPerwalian_Click(object sender, EventArgs e)
        {
            ShowListGuruPreviewPenilaianPramuka();
            txtKeyAction.Value = JenisAction.DoShowPreviewNilai.ToString();
        }
    }
}
