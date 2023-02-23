using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_Entities.Elearning.SD;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_DAOs.Elearning.SD;

namespace AI_ERP.Application_Modules.EDUCATION.Penilaian.SD
{
    public partial class wf_PengaturanRapor : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEPENGATURANSD";

        public enum JenisAction
        {
            Add,
            AddWithMessage,
            Edit,
            Update,
            Delete,
            Search,
            DoAdd,
            DoUpdate,
            DoDelete,
            DoSearch,
            DoShowData,
            DoShowConfirmHapus,
            DoShowDataTemplateEmailLTS,
            DoShowDataTemplateEmailRapor
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" + ResolveUrl("~/Application_CLibs/images/svg/check-mark.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Pengaturan";

            this.Master.ShowHeaderTools = false;
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {
                ListDropdownKurikulum();
                this.Master.txtCariData.Text = Libs.GetQ();
            }
            BindListView(!IsPostBack, Libs.GetQ());
        }

        protected void btnDoAdd_Click(object sender, EventArgs e)
        {
            txtID.Value = "";
            InitFields();
            txtKeyAction.Value = JenisAction.Add.ToString();
        }

        protected void ListDropdownKurikulum()
        {
            cboKurikulumRaporLevel1.Items.Add("");
            cboKurikulumRaporLevel1.Items.Add(new ListItem { Value = Libs.JenisKurikulum.SD.KTSP, Text = Libs.JenisKurikulum.SD.KTSP });
            cboKurikulumRaporLevel1.Items.Add(new ListItem { Value = Libs.JenisKurikulum.SD.KURTILAS, Text = Libs.JenisKurikulum.SD.KURTILAS });

            cboKurikulumRaporLevel2.Items.Add("");
            cboKurikulumRaporLevel2.Items.Add(new ListItem { Value = Libs.JenisKurikulum.SD.KTSP, Text = Libs.JenisKurikulum.SD.KTSP });
            cboKurikulumRaporLevel2.Items.Add(new ListItem { Value = Libs.JenisKurikulum.SD.KURTILAS, Text = Libs.JenisKurikulum.SD.KURTILAS });

            cboKurikulumRaporLevel3.Items.Add("");
            cboKurikulumRaporLevel3.Items.Add(new ListItem { Value = Libs.JenisKurikulum.SD.KTSP, Text = Libs.JenisKurikulum.SD.KTSP });
            cboKurikulumRaporLevel3.Items.Add(new ListItem { Value = Libs.JenisKurikulum.SD.KURTILAS, Text = Libs.JenisKurikulum.SD.KURTILAS });

            cboKurikulumRaporLevel4.Items.Add("");
            cboKurikulumRaporLevel4.Items.Add(new ListItem { Value = Libs.JenisKurikulum.SD.KTSP, Text = Libs.JenisKurikulum.SD.KTSP });
            cboKurikulumRaporLevel4.Items.Add(new ListItem { Value = Libs.JenisKurikulum.SD.KURTILAS, Text = Libs.JenisKurikulum.SD.KURTILAS });

            cboKurikulumRaporLevel5.Items.Add("");
            cboKurikulumRaporLevel5.Items.Add(new ListItem { Value = Libs.JenisKurikulum.SD.KTSP, Text = Libs.JenisKurikulum.SD.KTSP });
            cboKurikulumRaporLevel5.Items.Add(new ListItem { Value = Libs.JenisKurikulum.SD.KURTILAS, Text = Libs.JenisKurikulum.SD.KURTILAS });

            cboKurikulumRaporLevel6.Items.Add("");
            cboKurikulumRaporLevel6.Items.Add(new ListItem { Value = Libs.JenisKurikulum.SD.KTSP, Text = Libs.JenisKurikulum.SD.KTSP });
            cboKurikulumRaporLevel6.Items.Add(new ListItem { Value = Libs.JenisKurikulum.SD.KURTILAS, Text = Libs.JenisKurikulum.SD.KURTILAS });
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            sql_ds.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            if (keyword.Trim() != "")
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("nama", keyword);
                sql_ds.SelectCommand = DAO_Rapor_Pengaturan.SP_SELECT_ALL_FOR_SEARCH;
            }
            else
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectCommand = DAO_Rapor_Pengaturan.SP_SELECT_ALL;
            }
            if (isbind) lvData.DataBind();
        }

        protected void lnkOKHapus_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID.Value.Trim() != "")
                {
                    DAO_Rapor_Pengaturan.Delete(txtID.Value);
                    BindListView(!IsPostBack, Libs.GetQ().Trim());
                    txtKeyAction.Value = JenisAction.DoDelete.ToString();
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void lvData_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {

        }

        protected void InitFields()
        {
            txtTahunAjaran.Text = "";
            txtSemester.Text = "";
            txtNamaKepalaSekolah.Text = "";
            cboKurikulumRaporLevel1.SelectedValue = "";
            cboKurikulumRaporLevel2.SelectedValue = "";
            cboKurikulumRaporLevel3.SelectedValue = "";
            cboKurikulumRaporLevel4.SelectedValue = "";
            cboKurikulumRaporLevel5.SelectedValue = "";
            cboKurikulumRaporLevel6.SelectedValue = "";
        }

        protected void btnShowDetail_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                Rapor_Pengaturan m = DAO_Rapor_Pengaturan.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        InitFields();
                        txtTahunAjaran.Text = m.TahunAjaran;
                        txtSemester.Text = m.Semester;
                        txtNamaKepalaSekolah.Text = m.KepalaSekolah;
                        cboKurikulumRaporLevel1.SelectedValue = m.KurikulumRaporLevel1;
                        cboKurikulumRaporLevel2.SelectedValue = m.KurikulumRaporLevel2;
                        cboKurikulumRaporLevel3.SelectedValue = m.KurikulumRaporLevel3;
                        cboKurikulumRaporLevel4.SelectedValue = m.KurikulumRaporLevel4;
                        cboKurikulumRaporLevel5.SelectedValue = m.KurikulumRaporLevel5;
                        cboKurikulumRaporLevel6.SelectedValue = m.KurikulumRaporLevel6;
                        txtKeyAction.Value = JenisAction.DoShowData.ToString();
                    }
                }
            }
        }

        protected void btnShowConfirmDelete_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                Rapor_Pengaturan m = DAO_Rapor_Pengaturan.GetByID_Entity(txtID.Value.Trim());
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

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            Response.Redirect(Libs.FILE_PAGE_URL);
        }

        protected void lnkOKInput_Click(object sender, EventArgs e)
        {
            try
            {
                Rapor_Pengaturan m = new Rapor_Pengaturan();
                if (txtID.Value.Trim() == "")
                {
                    DAO_Rapor_Pengaturan.Insert(new Rapor_Pengaturan
                    {
                        Kode = Guid.NewGuid(),
                        TahunAjaran = txtTahunAjaran.Text,
                        Semester = txtSemester.Text,
                        KepalaSekolah = txtNamaKepalaSekolah.Text,
                        KurikulumRaporLevel1 = cboKurikulumRaporLevel1.SelectedValue,
                        KurikulumRaporLevel2 = cboKurikulumRaporLevel2.SelectedValue,
                        KurikulumRaporLevel3 = cboKurikulumRaporLevel3.SelectedValue,
                        KurikulumRaporLevel4 = cboKurikulumRaporLevel4.SelectedValue,
                        KurikulumRaporLevel5 = cboKurikulumRaporLevel5.SelectedValue,
                        KurikulumRaporLevel6 = cboKurikulumRaporLevel6.SelectedValue
                    });
                }
                else
                {
                    DAO_Rapor_Pengaturan.Update(new Rapor_Pengaturan
                    {
                        Kode = new Guid(txtID.Value),
                        TahunAjaran = txtTahunAjaran.Text,
                        Semester = txtSemester.Text,
                        KepalaSekolah = txtNamaKepalaSekolah.Text,
                        KurikulumRaporLevel1 = cboKurikulumRaporLevel1.SelectedValue,
                        KurikulumRaporLevel2 = cboKurikulumRaporLevel2.SelectedValue,
                        KurikulumRaporLevel3 = cboKurikulumRaporLevel3.SelectedValue,
                        KurikulumRaporLevel4 = cboKurikulumRaporLevel4.SelectedValue,
                        KurikulumRaporLevel5 = cboKurikulumRaporLevel5.SelectedValue,
                        KurikulumRaporLevel6 = cboKurikulumRaporLevel6.SelectedValue
                    });
                }

                BindListView(true, Libs.GetQ().Trim());
                txtKeyAction.Value = JenisAction.DoUpdate.ToString();
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void btnOKSaveTemplateEmailLTS_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime tgl_buka_link_rapor = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalBukaLinkLTS.Text);
                DAO_Rapor_Pengaturan.UpdateEmailLTS(
                    txtID.Value, 
                    txtTemplateEmailPengumumanLTSVal.Value,
                    (
                        txtTanggalBukaLinkLTS.Text.Trim() == ""
                        ? DateTime.MinValue
                        : new DateTime(
                                tgl_buka_link_rapor.Year,
                                tgl_buka_link_rapor.Month,
                                tgl_buka_link_rapor.Day,
                                Libs.GetStringToInteger(cboJamBukaLinkLTS.SelectedValue.Substring(0, 2)),
                                Libs.GetStringToInteger(cboJamBukaLinkLTS.SelectedValue.Substring(cboJamBukaLinkLTS.SelectedValue.Length - 2)),
                                0
                            )
                    )
                );
                mvMain.ActiveViewIndex = 0;
                txtKeyAction.Value = JenisAction.DoUpdate.ToString();
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void lnkBatalPengaturanTemplateEmailLTS_Click(object sender, EventArgs e)
        {
            mvMain.ActiveViewIndex = 0;
        }

        protected void btnShowTemplateEmailLTS_Click(object sender, EventArgs e)
        {
            Rapor_Pengaturan m = DAO_Rapor_Pengaturan.GetByID_Entity(txtID.Value);
            ltrInfoTemplateEmailLTS.Text = "";
            if (m != null)
            {
                if (m.TahunAjaran != null)
                {
                    ltrInfoTemplateEmailLTS.Text = "<span style=\"font-weight: normal;\">Tahun Pelajaran</span>&nbsp;" +
                                                   "<span style=\"font-weight: bold;\">" + m.TahunAjaran + "</span>&nbsp;" +
                                                   "<span style=\"font-weight: normal;\">Semester</span>&nbsp;" +
                                                   "<span style=\"font-weight: bold;\">" + m.Semester + "</span>&nbsp;";
                    if (m.TanggalBukaLinkLTS == DateTime.MinValue)
                    {
                        txtTanggalBukaLinkLTS.Text = "";
                    }
                    else
                    {
                        txtTanggalBukaLinkLTS.Text = Libs.GetTanggalIndonesiaFromDate(m.TanggalBukaLinkLTS, false);
                    }
                    Libs.ListJamToDropdown(cboJamBukaLinkLTS);
                    cboJamBukaLinkLTS.SelectedValue = Libs.GetJamFromTanggal(m.TanggalBukaLinkLTS);
                    txtTemplateEmailPengumumanLTS.Text = m.TemplateEmailLTS;
                    txtTemplateEmailPengumumanLTSVal.Value = m.TemplateEmailLTS;
                    txtKeyAction.Value = JenisAction.DoShowDataTemplateEmailLTS.ToString();
                    mvMain.ActiveViewIndex = 1;
                }
            }
        }

        protected void btnShowTemplateEmailRapor_Click(object sender, EventArgs e)
        {
            Rapor_Pengaturan m = DAO_Rapor_Pengaturan.GetByID_Entity(txtID.Value);
            ltrInfoTemplateEmailRapor.Text = "";
            if (m != null)
            {
                if (m.TahunAjaran != null)
                {
                    ltrInfoTemplateEmailRapor.Text = "<span style=\"font-weight: normal;\">Tahun Pelajaran</span>&nbsp;" +
                                                     "<span style=\"font-weight: bold;\">" + m.TahunAjaran + "</span>&nbsp;" +
                                                     "<span style=\"font-weight: normal;\">Semester</span>&nbsp;" +
                                                     "<span style=\"font-weight: bold;\">" + m.Semester + "</span>&nbsp;";
                    if (m.TanggalBukaLinkRapor == DateTime.MinValue)
                    {
                        txtTanggalBukaLinkRapor.Text = "";
                    }
                    else
                    {
                        txtTanggalBukaLinkRapor.Text = Libs.GetTanggalIndonesiaFromDate(m.TanggalBukaLinkRapor, false);
                    }
                    Libs.ListJamToDropdown(cboJamBukaLinkRapor);
                    cboJamBukaLinkRapor.SelectedValue = Libs.GetJamFromTanggal(m.TanggalBukaLinkRapor);
                    txtTemplateEmailPengumumanRapor.Text = m.TemplateEmailRapor;
                    txtTemplateEmailPengumumanRaporVal.Value = m.TemplateEmailRapor;
                    txtKeyAction.Value = JenisAction.DoShowDataTemplateEmailRapor.ToString();
                    mvMain.ActiveViewIndex = 2;
                }
            }
        }

        protected void lnkBatalPengaturanTemplateEmailRapor_Click(object sender, EventArgs e)
        {
            mvMain.ActiveViewIndex = 0;
        }

        protected void btnOKSaveTemplateEmailRapor_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime tgl_buka_link_rapor = Libs.GetDateFromTanggalIndonesiaStr(txtTanggalBukaLinkRapor.Text);
                DAO_Rapor_Pengaturan.UpdateEmailRapor(
                    txtID.Value,
                    txtTemplateEmailPengumumanRaporVal.Value,
                    (
                        txtTanggalBukaLinkRapor.Text.Trim() == ""
                        ? DateTime.MinValue
                        : new DateTime(
                                tgl_buka_link_rapor.Year,
                                tgl_buka_link_rapor.Month,
                                tgl_buka_link_rapor.Day,
                                Libs.GetStringToInteger(cboJamBukaLinkRapor.SelectedValue.Substring(0, 2)),
                                Libs.GetStringToInteger(cboJamBukaLinkRapor.SelectedValue.Substring(cboJamBukaLinkRapor.SelectedValue.Length - 2)),
                                0
                            )
                    )
                );
                mvMain.ActiveViewIndex = 0;
                txtKeyAction.Value = JenisAction.DoUpdate.ToString();
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }
    }
}