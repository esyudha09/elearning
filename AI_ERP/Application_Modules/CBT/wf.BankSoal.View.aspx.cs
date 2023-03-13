using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;

namespace AI_ERP.Application_Modules.CBT
{
    public partial class wf_BankSoal_View : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATAMAPEL";

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
            DoChangePage,
            DoShowConfirmHapus
        }

        private static class QS
        {
            public static string GetUnit()
            {
                if (Libs.GetQueryString("u").Trim() != "")
                {
                    return Libs.GetQueryString("u");
                }
                else
                {
                    return Libs.GetQueryString("unit");
                }
            }

            public static string GetToken()
            {
                return Libs.GetQueryString("token");
            }

            public static string GetKelas()
            {
                return Libs.GetQueryString("k");
            }

            public static string GetTahunAjaran()
            {
                string t = Libs.GetQueryString("t");
                return RandomLibs.GetParseTahunAjaran(t);
            }

            public static string GetURLVariable()
            {
                string s_url_var = "";
                s_url_var += (Libs.GetQueryString("m").Trim() != "" ? "m=" + Libs.GetQueryString("m").Trim() : "");
                s_url_var += (s_url_var.Trim() != "" && QS.GetToken().Trim() != "" ? "&" : "") + (QS.GetToken().Trim() != "" ? "token=" : "") + QS.GetToken().Trim();

                return (
                            Libs.GetQueryString("m").Trim() != "" || QS.GetToken().Trim() != ""
                        ? "?" : "") +
                        s_url_var;
            }
        }

        protected bool IsByAdminUnit()
        {
            return (QS.GetUnit().Trim() != "" && QS.GetToken().Trim() != "" &&
                    DAO_Sekolah.IsValidTokenUnit(Libs.GetQueryString("unit"), Libs.GetQueryString("token")) ? true : false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            //{
            //    Libs.RedirectToLogin(this.Page);
            //}
            //if (!DAO_Sekolah.IsValidTokenUnit(Libs.GetQueryString("unit"), Libs.GetQueryString("token")))
            //{
            //    Libs.RedirectToBeranda(this.Page);
            //}

            if (!IsPostBack)
            {
                //InitInput();
                InitKeyEventClient();
                var IdSoal = Libs.GetQueryString("id");
                if (!string.IsNullOrEmpty(IdSoal))
                {
                    getData(IdSoal);
                }
            }




        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            //this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
            //cboUnit.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtNama.ClientID + "').focus(); return false; }");
            //txtSoal.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtSoal.ClientID + "').focus(); return false; }");
            //txtJwbEssay.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtSoal.ClientID + "').focus(); return false; }");
            //cboJenis.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + txtKeterangan.ClientID + "').focus(); return false; }");
            //txtKeterangan.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKInput.ClientID + "').click(); return false; }");
        }

        //private void InitInput()
        //{
        //    cboUnit.Items.Clear();
        //    cboUnit.Items.Add("");
        //    List<Sekolah> lst_sekolah = DAO_Sekolah.GetAll_Entity().OrderBy(m => m.UrutanJenjang).ToList();
        //    div_input_filter_unit.Visible = true;
        //    if (IsByAdminUnit())
        //    {
        //        cboUnit.Items.Clear();
        //        lst_sekolah = lst_sekolah.FindAll(m => m.Kode.ToString().ToUpper().Trim() == QS.GetUnit().ToUpper().Trim()).ToList();
        //        div_input_filter_unit.Visible = false;
        //    }
        //    foreach (Sekolah m in lst_sekolah)
        //    {
        //        cboUnit.Items.Add(new ListItem
        //        {
        //            Value = m.Kode.ToString().ToUpper(),
        //            Text = m.Nama
        //        });
        //    }

        //    Libs.JENIS_MAPEL.ListToDropdown(cboJenis, true, QS.GetUnit());
        //}








        //protected void InitFields()
        //{
        //    txtID.Value = "";
        //    txtSoal.Text = "";
        //    txtJwbEssay.Text = "";
        //    txtJwbGanda1.Text = "";
        //    txtJwbGanda2.Text = "";
        //    txtJwbGanda3.Text = "";
        //    txtJwbGanda4.Text = "";
        //    txtJwbGanda5.Text = "";
        //    //chkEssay.Checked = false;
        //    //chkGanda.Checked = false;
        //}







        protected void getData(string idsoal)
        {

            CBT_BankSoal m = DAO_CBT_BankSoal.GetByID_Entity(idsoal.Trim());

            if (m != null)
            {
                if (m.Soal != null)
                {
                    txtSoal.Text = m.Soal.ToString();

                    if (m.Jenis == "essay")
                    {
                        EssayDiv.Attributes.Add("style", "display:block");
                        GandaDiv.Attributes.Add("style", "display:none");
                    }
                    else if (m.Jenis == "ganda")
                    {
                        EssayDiv.Attributes.Add("style", "display:none");
                        GandaDiv.Attributes.Add("style", "display:block");
                    }

                    string folderPath = "~/Application_Resources/ImageSoal/";
                    string folderPath2 = "../Application_Resources/ImageSoal/";

                    if (m.FileImage != "")
                    {

                        FileImageID.Attributes.Add("style", "display:block");
                        FileImageID.Src = folderPath + m.FileImage.ToString();
                    }
                    if (m.FileAudio != "")
                    {

                        FileAudioID.Attributes.Add("style", "display:block");
                        //FileAudioID.Attributes.Add("src", folderPath + m.FileImage.ToString());
                        hdSourceAudio.Value = folderPath2 + m.FileAudio.ToString();
                    }
                     if (m.FileVideo != "")
                    {

                        FileVideoID.Attributes.Add("style", "display:block");
                        //    FileVideoID.Attributes["Src"] = folderPath + m.FileImage.ToString();
                        hdSourceVideo.Value = folderPath2 + m.FileVideo.ToString();
                    }

                    hdKodejwbGanda1.Value = m.ListJwbGanda[0].Kode.ToString();
                    hdKodejwbGanda2.Value = m.ListJwbGanda[1].Kode.ToString();
                    hdKodejwbGanda3.Value = m.ListJwbGanda[2].Kode.ToString();
                    hdKodejwbGanda4.Value = m.ListJwbGanda[3].Kode.ToString();
                    hdKodejwbGanda5.Value = m.ListJwbGanda[4].Kode.ToString();

                    txtJwbGanda1.Text = m.ListJwbGanda[0].Jawaban.ToString();
                    txtJwbGanda2.Text = m.ListJwbGanda[1].Jawaban.ToString();
                    txtJwbGanda3.Text = m.ListJwbGanda[2].Jawaban.ToString();
                    txtJwbGanda4.Text = m.ListJwbGanda[3].Jawaban.ToString();
                    txtJwbGanda5.Text = m.ListJwbGanda[4].Jawaban.ToString();

                    ChkJwbGanda1.Checked = false;
                    ChkJwbGanda2.Checked = false;
                    ChkJwbGanda3.Checked = false;
                    ChkJwbGanda4.Checked = false;
                    ChkJwbGanda5.Checked = false;


                    //var jwb = lstJwb.Where(x => x.Rel_DesignSoal.ToUpper() == ds.ToUpper()).FirstOrDefault();
                    //if (jwb != null)
                    //{
                    //    //txtJwbEssayVal.Value = jwb.JwbEssay;
                    //    txtJwbEssay.Text = jwb.JwbEssay;

                    //    //.Rel_JwbGanda;
                    //    if (hdKodejwbGanda1.Value == jwb.Rel_JwbGanda.ToString())
                    //    {
                    //        ChkJwbGanda1.Checked = true;
                    //    }
                    //    else if (hdKodejwbGanda2.Value == jwb.Rel_JwbGanda.ToString())
                    //    {
                    //        ChkJwbGanda2.Checked = true;
                    //    }
                    //    else if (hdKodejwbGanda3.Value == jwb.Rel_JwbGanda.ToString())
                    //    {
                    //        ChkJwbGanda3.Checked = true;
                    //    }
                    //    else if (hdKodejwbGanda4.Value == jwb.Rel_JwbGanda.ToString())
                    //    {
                    //        ChkJwbGanda4.Checked = true;
                    //    }
                    //    else if (hdKodejwbGanda5.Value == jwb.Rel_JwbGanda.ToString())
                    //    {
                    //        ChkJwbGanda5.Checked = true;
                    //    }

                    //}
                }
            }
        }







        protected void btnBackToMenu_Click(object sender, EventArgs e)
        {
            Response.Redirect(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.CBT.SOAL.ROUTE +
                            QS.GetURLVariable()
                        )
                );
        }
    }
}