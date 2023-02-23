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

namespace AI_ERP.Application_Modules.MASTER
{
    public partial class wf_Pengaturan_SD : System.Web.UI.Page
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
            }
        }

        protected void btnDoAdd_Click(object sender, EventArgs e)
        {
            txtID.Value = "";
            InitFields();
            txtKeyAction.Value = JenisAction.Add.ToString();
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

                        txtKeyAction.Value = JenisAction.DoShowData.ToString();
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
                        Kode = Guid.NewGuid()
                    });
                }
                else
                {
                    DAO_Rapor_Pengaturan.Update(new Rapor_Pengaturan
                    {
                        Kode = new Guid(txtID.Value)
                    });
                }

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
                //DAO_Rapor_Pengaturan.UpdateEmailLTS(txtID.Value, txtTemplateEmailPengumumanLTSVal.Value);
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
        }

        protected void btnOKSavePengaturan_Click(object sender, EventArgs e)
        {

        }
    }
}