using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_Entities.Elearning.SMA;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_DAOs.Elearning.SMA;

namespace AI_ERP.Application_Modules.MASTER
{
    public partial class wf_Pengaturan_SMA : System.Web.UI.Page
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
            DoShowDataTemplateEmailLTS
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" + ResolveUrl("~/Application_CLibs/images/svg/002-website.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Pengaturan Umum";

            this.Master.ShowHeaderTools = false;
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {
                this.Master.txtCariData.Text = Libs.GetQ();
                ShowData();
            }
        }

        protected void ShowData()
        {
            InitFields();
            List<PengaturanSMA> lst = DAO_PengaturanSMA.GetAll_Entity();
            if (lst.Count > 0)
            {
                PengaturanSMA m = lst.FirstOrDefault();
                if (m != null)
                {
                    if (m.HeaderKop != null)
                    {
                        txtTemplateAlamatKopEmail.Text = m.HeaderAlamat;
                        txtTemplateAlamatKopEmailVal.Value = m.HeaderAlamat;

                        txtTemplateJudulKopEmail.Text = m.HeaderKop;
                        txtTemplateJudulKopEmailVal.Value = m.HeaderKop;

                        chkIsTestEmail.Checked = m.IsTestEmail;
                        txtEmailTestEmail.Text = m.TestEmail;
                        txtTeksLinkLTS.Text = m.TeksLinkLTS;
                        txtExpiredLinkHari.Text = m.ExpiredLinkLTSHari.ToString();
                        txtExpiredLinkJam.Text = m.ExpiredLinkLTSJam.ToString();
                        txtExpiredLinkMenit.Text = m.ExpiredLinkLTSMenit.ToString();
                        txtTemplateKontenHTMLLinkExpired.Text = m.TemplateHTMLLinkExpired;
                        txtTemplateKontenHTMLLinkExpiredVal.Value = m.TemplateHTMLLinkExpired;

                        cboJenisFileRapor.SelectedValue = m.JenisFileRapor;
                    }
                }
            }
        }

        protected void lnkOKHapus_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID.Value.Trim() != "")
                {
                    DAO_Rapor_Pengaturan.Delete(txtID.Value);
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
            txtTemplateAlamatKopEmail.Text = "";
            txtTemplateAlamatKopEmailVal.Value = "";

            txtTemplateJudulKopEmail.Text = "";
            txtTemplateJudulKopEmailVal.Value = "";

            chkIsTestEmail.Checked = false;
            txtEmailTestEmail.Text = "";
            txtTeksLinkLTS.Text = "";
            txtExpiredLinkHari.Text = "";
            txtExpiredLinkJam.Text = "";
            txtExpiredLinkMenit.Text = "";

            txtTemplateKontenHTMLLinkExpiredVal.Value = "";
            txtTemplateKontenHTMLLinkExpired.Text = "";
        }

        protected void btnOKSavePengaturan_Click(object sender, EventArgs e)
        {
            try
            {
                List<PengaturanSMA> lst = DAO_PengaturanSMA.GetAll_Entity();
                if (lst.Count > 0)
                {
                    PengaturanSMA m = lst.FirstOrDefault();
                    if (m != null)
                    {
                        DAO_PengaturanSMA.Update(new PengaturanSMA
                        {
                            Kode = m.Kode,
                            HeaderKop = txtTemplateJudulKopEmailVal.Value,
                            HeaderAlamat = txtTemplateAlamatKopEmailVal.Value,
                            HeaderLogo = "",
                            IsTestEmail = chkIsTestEmail.Checked,
                            TestEmail = txtEmailTestEmail.Text,
                            TeksLinkLTS = txtTeksLinkLTS.Text,
                            ExpiredLinkLTSHari = Libs.GetStringToInteger(txtExpiredLinkHari.Text),
                            ExpiredLinkLTSJam = Libs.GetStringToInteger(txtExpiredLinkJam.Text),
                            ExpiredLinkLTSMenit = Libs.GetStringToInteger(txtExpiredLinkMenit.Text),
                            TemplateHTMLLinkExpired = txtTemplateKontenHTMLLinkExpiredVal.Value,
                            JenisFileRapor = cboJenisFileRapor.SelectedValue
                    });
                        ShowData();
                        mvMain.ActiveViewIndex = 0;
                        txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                    }
                    else
                    {
                        DAO_PengaturanSMA.Insert(new PengaturanSMA
                        {
                            Kode = Guid.NewGuid(),
                            HeaderKop = txtTemplateJudulKopEmailVal.Value,
                            HeaderAlamat = txtTemplateAlamatKopEmailVal.Value,
                            HeaderLogo = "",
                            IsTestEmail = chkIsTestEmail.Checked,
                            TestEmail = txtEmailTestEmail.Text,
                            TeksLinkLTS = txtTeksLinkLTS.Text,
                            ExpiredLinkLTSHari = Libs.GetStringToInteger(txtExpiredLinkHari.Text),
                            ExpiredLinkLTSJam = Libs.GetStringToInteger(txtExpiredLinkJam.Text),
                            ExpiredLinkLTSMenit = Libs.GetStringToInteger(txtExpiredLinkMenit.Text),
                            TemplateHTMLLinkExpired = txtTemplateKontenHTMLLinkExpiredVal.Value,
                            JenisFileRapor = cboJenisFileRapor.SelectedValue
                        });
                        ShowData();
                        mvMain.ActiveViewIndex = 0;
                        txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                    }
                }
                else
                {
                    DAO_PengaturanSMA.Insert(new PengaturanSMA
                    {
                        Kode = Guid.NewGuid(),
                        HeaderKop = txtTemplateJudulKopEmailVal.Value,
                        HeaderAlamat = txtTemplateAlamatKopEmailVal.Value,
                        HeaderLogo = "",
                        IsTestEmail = chkIsTestEmail.Checked,
                        TestEmail = txtEmailTestEmail.Text,
                        TeksLinkLTS = txtTeksLinkLTS.Text,
                        ExpiredLinkLTSHari = Libs.GetStringToInteger(txtExpiredLinkHari.Text),
                        ExpiredLinkLTSJam = Libs.GetStringToInteger(txtExpiredLinkJam.Text),
                        ExpiredLinkLTSMenit = Libs.GetStringToInteger(txtExpiredLinkMenit.Text),
                        TemplateHTMLLinkExpired = txtTemplateKontenHTMLLinkExpiredVal.Value,
                        JenisFileRapor = cboJenisFileRapor.SelectedValue
                    });
                    ShowData();
                    mvMain.ActiveViewIndex = 0;
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }
    }
}