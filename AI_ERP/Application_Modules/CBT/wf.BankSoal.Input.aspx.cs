using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using System.IO;
using static AI_ERP.Application_Libs.Routing.URL.APPLIACTION_MODULES.CBT;
using AI_ERP.Application_Entities.Elearning.SMA;
using AI_ERP.Application_DAOs.Elearning.SMA;
using System.Globalization;

namespace AI_ERP.Application_Modules.CBT
{
    public partial class wf_BankSoal_Input : System.Web.UI.Page
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
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }
            //if (!DAO_Sekolah.IsValidTokenUnit(Libs.GetQueryString("unit"), Libs.GetQueryString("token")))
            //{
            //    Libs.RedirectToBeranda(this.Page);
            //}

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" + ResolveUrl("~/Application_CLibs/images/svg/document.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Form Input Soal";

            this.Master.ShowHeaderTools = false;
            this.Master.HeaderCardVisible = false;
            if (!string.IsNullOrEmpty(Libs.GetQueryString("rs")))
            {
                fromBankSoal.Attributes.Add("style", "display:none");
                fromDesignSoal.Attributes.Add("style", "display:block");
            }
            else
            {
                fromBankSoal.Attributes.Add("style", "display:block");
                fromDesignSoal.Attributes.Add("style", "display:none");
            }

            
            if (!IsPostBack)
            {
                
                SetSelectAP();
                InitKeyEventClient();
                var IdSoal = Libs.GetQueryString("id");
                if (!string.IsNullOrEmpty(IdSoal))
                {
                    getData(IdSoal);
                }

             

            }
            
        }


        private void SetSelectAP()
        {
            var sn = Libs.GetQueryString("sn");

            if (QS.GetUnit() == "SMA")
            {
                List<Rapor_StrukturNilai_KURTILAS_AP> lst_aspek_penilaian = DAO_Rapor_StrukturNilai_KURTILAS_AP.GetAllByHeader_Entity(sn);

                cboAP.Items.Add("");
                foreach (Rapor_StrukturNilai_KURTILAS_AP m in lst_aspek_penilaian)
                {
                    cboAP.Items.Add(new ListItem
                    {
                        Value = m.Rel_Rapor_AspekPenilaian.ToString(),
                        Text = Libs.GetHTMLSimpleText(m.KompetensiDasar.Trim())
                    });
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







        protected void InitFields()
        {
            txtID.Value = "";
            txtSoal.Text = "";
            txtJwbEssay.Text = "";
            txtJwbGanda1.Text = "";
            txtJwbGanda2.Text = "";
            txtJwbGanda3.Text = "";
            txtJwbGanda4.Text = "";
            txtJwbGanda5.Text = "";
            ChkJwbGanda1.Checked = false;
            ChkJwbGanda2.Checked = false;
            ChkJwbGanda3.Checked = false;
            ChkJwbGanda4.Checked = false;
            ChkJwbGanda5.Checked = false;

            ImagePrev.Attributes.Add("style", "display:none");
            AudioPrev.Attributes.Add("style", "display:none");
            VideoPrev.Attributes.Add("style", "display:none");
            hdSourceAudio.Value = "";
            hdSourceVideo.Value = "";
            ImagePrev.Src = "";
        }



        protected void lnkOKHapus_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID.Value.Trim() != "")
                {
                    DAO_CBT_BankSoal.Delete(txtID.Value, Libs.LOGGED_USER_M.UserID);

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
                if (txtNama.Text != "")
                {
                    CBT_BankSoal m = new CBT_BankSoal();
                    if (FileUpload1.HasFile)
                    {

                        string folderPath = Server.MapPath("~/Application_Resources/FileSoal");
                        var fileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-") + FileUpload1.FileName;
                        var fileExt = Path.GetExtension(FileUpload1.FileName);

                        //Check whether Directory (Folder) exists.
                        if (!Directory.Exists(folderPath))
                        {
                            //If Directory (Folder) does not exists Create it.
                            Directory.CreateDirectory(folderPath);
                        }

                        FileUpload1.SaveAs(folderPath + "/" + fileName);

                        if (fileExt == ".png" || fileExt == ".jpg" || fileExt == ".jpeg")
                        {
                            m.FileImage = fileName;
                            m.FileAudio = "";
                            m.FileVideo = "";
                        }
                        else if (fileExt == ".mp3")
                        {
                            m.FileImage = "";
                            m.FileAudio = fileName;
                            m.FileVideo = "";
                        }
                        else if (fileExt == ".mp4")
                        {
                            m.FileImage = "";
                            m.FileAudio = "";
                            m.FileVideo = fileName;
                        }
                    }

                    m.Rel_Mapel = Libs.GetQueryString("m");
                    m.Rel_Kelas = Libs.GetQueryString("k");
                    m.Rel_Guru = Libs.LOGGED_USER_M.NoInduk;
                    m.Nama = txtNama.Text;
                    m.Rel_Rapor_AspekPenilaian = cboAP.SelectedValue;
                    m.Soal = txtSoal.Text;
                    m.Jenis = cboJenis.SelectedValue;

                    List<CBT_BankSoalJawabGanda> list_JwbGanda = new List<CBT_BankSoalJawabGanda>();


                    bool[] arrJwbGandaChk = { ChkJwbGanda1.Checked, ChkJwbGanda2.Checked, ChkJwbGanda3.Checked, ChkJwbGanda4.Checked, ChkJwbGanda5.Checked };
                    string[] arrKodeJwbGanda = { hdKodejwbGanda1.Value, hdKodejwbGanda2.Value, hdKodejwbGanda3.Value, hdKodejwbGanda4.Value, hdKodejwbGanda5.Value };
                    string[] arrJwbGanda = { txtJwbGanda1.Text, txtJwbGanda2.Text, txtJwbGanda3.Text, txtJwbGanda4.Text, txtJwbGanda5.Text };

                    if (m.Jenis == "ganda")
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            var j = new CBT_BankSoalJawabGanda
                            {
                                Kode = arrKodeJwbGanda[i] != "" ? new Guid(arrKodeJwbGanda[i]) : Guid.NewGuid(),
                                Jawaban = arrJwbGanda[i],
                                Urut = i + 1
                            };

                            list_JwbGanda.Add(j);

                            if (arrJwbGandaChk[i])
                                m.Rel_JwbGanda = j.Kode.ToString();
                        }
                    }

                    m.ListJwbGanda = list_JwbGanda;


                    if (txtID.Value.Trim() != "")
                    {

                        m.JwbEssay = txtJwbEssay.Text;
                        m.Kode = new Guid(txtID.Value);
                        DAO_CBT_BankSoal.Update(m, Libs.LOGGED_USER_M.UserID);
                        //InitFields();
                        getData(txtID.Value);
                        txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                    }
                    else
                    {
                        m.Kode = Guid.NewGuid();
                        m.JwbEssay = txtJwbEssay.Text;
                        DAO_CBT_BankSoal.Insert(m, Libs.LOGGED_USER_M.UserID);

                        if (!string.IsNullOrEmpty(Libs.GetQueryString("rs")))
                        {

                            CBT_DesignSoal d = new CBT_DesignSoal();
                            d.Rel_RumahSoal = Libs.GetQueryString("rs");
                            d.Rel_BankSoal = m.Kode.ToString();
                            DAO_CBT_DesignSoal.Insert(d, Libs.LOGGED_USER_M.UserID);

                            // btnBackToDesignSoal_Click(null, null);
                        }
                        //else
                        //{
                        //    btnBackToSoal_Click(null, null);
                        //}

                        //InitFields();
                        getData(m.Kode.ToString());

                        txtKeyAction.Value = JenisAction.AddWithMessage.ToString();
                    }


                }
            }
            catch (Exception ex)
            {
                txtKeyAction.Value = ex.Message;
            }
        }

        protected void getData(string idsoal)
        {
            InitFields();
            CBT_BankSoal m = DAO_CBT_BankSoal.GetByID_Entity(idsoal.Trim());

            if (m != null)
            {
                if (m.Soal != null)
                {
                                   
                    string folderPath = "~/Application_Resources/FileSoal/";
                    string folderPath2 = "../Application_Resources/FileSoal/";


                    

                    if (m.FileImage != "")
                    {
                        ImagePrev.Attributes.Add("style", "display:block;");
                        ImagePrev.Src = folderPath + m.FileImage.ToString();
                    }
                    if (m.FileAudio != "")
                    {
                        AudioPrev.Attributes.Add("style", "display:block;");
                        hdSourceAudio.Value = folderPath2 + m.FileAudio.ToString();
                    }
                    if (m.FileVideo != "")
                    {
                        VideoPrev.Attributes.Add("style", "display:block;");
                        hdSourceVideo.Value = folderPath2 + m.FileVideo.ToString();
                    }


                    txtID.Value = m.Kode.ToString();
                    txtNama.Text = m.Nama.ToString();
                    cboAP.SelectedValue = m.Rel_Rapor_AspekPenilaian.ToString();
                    txtSoal.Text = m.Soal.ToString();
                    txtJwbEssay.Text = m.JwbEssay.ToString();

                    cboJenis.SelectedValue = m.Jenis.ToString();

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




                    if (m.Rel_JwbGanda.ToString() == m.ListJwbGanda[0].Kode.ToString())
                    {
                        ChkJwbGanda1.Checked = true;
                    }
                    else if (m.Rel_JwbGanda.ToString() == m.ListJwbGanda[1].Kode.ToString())
                    {
                        ChkJwbGanda2.Checked = true;
                    }
                    else if (m.Rel_JwbGanda.ToString() == m.ListJwbGanda[2].Kode.ToString())
                    {
                        ChkJwbGanda3.Checked = true;
                    }
                    else if (m.Rel_JwbGanda.ToString() == m.ListJwbGanda[3].Kode.ToString())
                    {
                        ChkJwbGanda4.Checked = true;
                    }
                    else if (m.Rel_JwbGanda.ToString() == m.ListJwbGanda[4].Kode.ToString())
                    {
                        ChkJwbGanda5.Checked = true;
                    }

                    
                    txtKeyAction.Value = JenisAction.DoShowData.ToString();
                }

                


            }
        }

        protected void btnShowConfirmDelete_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                CBT_BankSoal m = DAO_CBT_BankSoal.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.Soal != null)
                    {
                        ltrMsgConfirmHapus.Text = "Hapus <span style=\"font-weight: bold;\">\"" +
                                                            Libs.GetHTMLSimpleText(m.Soal) +
                                                      "\"</span>?";
                        txtKeyAction.Value = JenisAction.DoShowConfirmHapus.ToString();
                    }
                }
            }
        }

        protected void btnBackToSoal_Click(object sender, EventArgs e)
        {
            var m = Libs.GetQueryString("m");
            var unit = Libs.GetQueryString("u");
            var kelas = Libs.GetQueryString("k");

            Response.Redirect(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.CBT.SOAL.ROUTE + "?&m=" + m + "&k=" + kelas + "&u=" + unit + "&sn=" + Libs.GetQueryString("sn")

                        )
                );
        }

        protected void btnBackToMapel_Click(object sender, EventArgs e)
        {
            var m = Libs.GetQueryString("m");
            var kp = Libs.GetQueryString("kp");
            var kur = Libs.GetQueryString("kur");
            var unit = Libs.GetQueryString("u");
            Response.Redirect(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.CBT.MAPEL.ROUTE // + "?&m=" + m + "&kp=" + kp + "&kur=" + kur + "&u=" + unit
                        )
                );
        }

        protected void btnBackToKelas_Click(object sender, EventArgs e)
        {
            var m = Libs.GetQueryString("m");
            var unit = Libs.GetQueryString("u");
            Response.Redirect(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.CBT.STRUKTUR_PENILAIAN_SMA.ROUTE + "?&m=" + m + "&u=" + unit
                        )
                );
        }

        protected void btnBackToStrukturNilai_Click(object sender, EventArgs e)
        {
            var m = Libs.GetQueryString("m");
            var strukturNilai = Libs.GetQueryString("sn");
            Response.Redirect(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.CBT.STRUKTUR_PENILAIAN_SMA.ROUTE + "?&m=" + m + "&sn=" + strukturNilai + "&u=" + Libs.GetQueryString("u")
                        )
                );
        }

        protected void btnBackToFormRumahSoal_Click(object sender, EventArgs e)
        {
            var m = Libs.GetQueryString("m");
            var kp = Libs.GetQueryString("kp");
            var kur = Libs.GetQueryString("kur");
            var sn = Libs.GetQueryString("sn");
            Response.Redirect(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.CBT.RUMAH_SOAL_INPUT.ROUTE + "?&m=" + m + "&kp=" + kp + "&kur=" + kur + "&sn=" + sn + "&u=" + Libs.GetQueryString("u")
                        )
                );
        }

        protected void btnBackToDesignSoal_Click(object sender, EventArgs e)
        {
            var m = Libs.GetQueryString("m");
            var kp = Libs.GetQueryString("kp");
            var kur = Libs.GetQueryString("kur");
            var sn = Libs.GetQueryString("sn");
            var rs = Libs.GetQueryString("rs");
            Response.Redirect(
                    ResolveUrl(
                            Routing.URL.APPLIACTION_MODULES.CBT.DESIGN_SOAL.ROUTE + "?&m=" + m + "&kp=" + kp + "&kur=" + kur + "&sn=" + sn + "&rs=" + rs + "&u=" + Libs.GetQueryString("u")
                        )
                );
        }
        
    }
}