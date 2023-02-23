using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Entities;

namespace AI_ERP
{
    public partial class wf_UbahPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            if (!IsPostBack)
            {
                this.Master.HeaderTittle = "<i class=\"fa fa-lock\"></i>&nbsp;&nbsp;" +
                                           "Ubah Password";
                this.Master.HeaderCardVisible = false;
                this.Master.ShowHeaderTools = false;
            }
        }
                
        protected void lnkSaveAkun_Click(object sender, EventArgs e)
        {
            UserLogin usr = (UserLogin)Session[Constantas.NAMA_SESSION_LOGIN];
            UserLogin _usr = DAO_UserLogin.SelectLogin(usr.UserID, txtPasswordLama.Text);
            lblInfoSimpan.ForeColor = System.Drawing.Color.Red;
            if (_usr == null)
            {
                div_infosimpan.Visible = true;
                lblInfoSimpan.Text = "Password lama tidak valid";
                txtPasswordLama.Focus();
                return;
            }
            else
            {
                if (txtPasswordBaru.Text != txtPasswordBaruTulis.Text)
                {
                    div_infosimpan.Visible = true;
                    lblInfoSimpan.Text = "Konfirmasi password baru tidak valid";
                    txtPasswordLama.Focus();
                    return;
                }
            }
            if (DAO_UserLogin.Update(new UserLogin
            {
                NoInduk = usr.NoInduk,
                UserID = usr.UserID,
                Password = Libs.Encryptdata(txtPasswordBaru.Text)
            }))
            {
                lblInfoSimpan.ForeColor = System.Drawing.Color.Green;
                div_infosimpan.Visible = true;
                lblInfoSimpan.Text = "Password telah diubah";
            }
        }
    }
}