using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;

namespace AI_ERP.Application_Modules.EDUCATION.Elearning
{
    public partial class wf_Guru_Materi : System.Web.UI.Page
    {
        public const string SessionViewDataName = "PAGEDATAGURUMATERI";

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
            DoShowConfirmPublish
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            this.Master.HeaderTittle = "<img style=\"height: 28px; width: 28px; display: initial;\" src=\"" + 
                                            ResolveUrl("~/Application_CLibs/images/svg/school-material-0.svg") + "\">" +
                                       "&nbsp;&nbsp;" +
                                       "Materi Pembelajaran";

            this.Master.ShowHeaderTools = true;
            this.Master.HeaderCardVisible = false;

            if (!IsPostBack)
            {
                InitInput();
                InitKeyEventClient();
                this.Master.txtCariData.Text = Libs.GetQ();
            }
            BindListView(!IsPostBack, Libs.GetQ());
        }

        private void InitKeyEventClient()
        {
            if (IsPostBack) return;

            string sKeyEnter = "if(event.keyCode == 13){";
            this.Master.txtCariData.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + btnDoCari.ClientID + "').click(); return false; }");
            cboTahunAjaran.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboSemester.ClientID + "').focus(); return false; }");
            cboSemester.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboKelas.ClientID + "').focus(); return false; }");
            cboSemester.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + cboMataPelajaran.ClientID + "').focus(); return false; }");
            cboMataPelajaran.Attributes.Add("onkeydown", sKeyEnter + "document.getElementById('" + lnkOKInput.ClientID + "').click(); return false; }");

            cboMataPelajaran.Attributes.Add("onchange", txtMapel.ClientID + ".value = this.value; return false;");
            cboKelas.Attributes.Add("onchange", txtKelas.ClientID + ".value = this.value; return false;");
            cboGuru.Attributes.Add("onchange", txtGuru.ClientID + ".value = this.value; return false;");

            cboTahunAjaran.Attributes["onchange"] = "ShowKelasByPeriode(); ShowMapelByPeriodeKelas();";
            cboSemester.Attributes["onchange"] = "ShowKelasByPeriode(); ShowMapelByPeriodeKelas();";
            cboKelas.Attributes["onchange"] = "ShowGuruByKelas();";
        }

        protected void InitInput()
        {
            //unit untuk contoh
            Sekolah m_sekolah = DAO_Sekolah.GetAll_Entity().FindAll(
                    m => m.UrutanJenjang == (int)Libs.UnitSekolah.SMP
                ).First();

            List<string> lst_tahun_ajaran = 
                DAO_FormasiGuruMapel.GetAll_Entity().Select(m => m.TahunAjaran).Distinct().ToList();
            lst_tahun_ajaran = lst_tahun_ajaran.OrderByDescending(m => m).ToList();
            cboTahunAjaran.Items.Clear();
            foreach (string tahun_ajaran in lst_tahun_ajaran)
            {
                cboTahunAjaran.Items.Add(new ListItem {
                    Value = tahun_ajaran,
                    Text = tahun_ajaran
                });
            }

            List<FormasiGuruMapel> lst_formasi_for_kelas = DAO_FormasiGuruMapel.GetByUnit_Entity(m_sekolah.Kode.ToString());

            txtParseListMataPelajaran.Value = "";
            txtParseListKelas.Value = "";            
            foreach (FormasiGuruMapel formasi in lst_formasi_for_kelas)
            {
                Kelas m_kelas = DAO_Kelas.GetByID_Entity(formasi.Rel_Kelas.ToString());
                if (m_kelas != null)
                {
                    if (m_kelas.Nama != null)
                    {
                        txtParseListKelas.Value += (formasi.TahunAjaran + formasi.Semester) + "->";
                        txtParseListKelas.Value += m_kelas.Kode +
                                                   "|" +
                                                   m_kelas.Nama;
                    }
                }

                Mapel m_mapel = DAO_Mapel.GetByID_Entity(formasi.Rel_Mapel.ToString());
                if (m_mapel != null)
                {
                    if (m_mapel.Nama != null)
                    {
                        txtParseListMataPelajaran.Value += (formasi.TahunAjaran + formasi.Semester + formasi.Rel_Kelas.ToString()) + "->";
                        txtParseListMataPelajaran.Value += m_mapel.Kode +
                                                           "|" +
                                                           m_mapel.Nama;
                    }
                }
            }


            txtParseListGuru.Value = "";
            List<FormasiGuruMapelDet> lst_formasi = DAO_FormasiGuruMapelDet.GetByUnit_Entity(m_sekolah.Kode.ToString());
            foreach (FormasiGuruMapelDet formasi in lst_formasi)
            {
                FormasiGuruMapel m_formasi_head = DAO_FormasiGuruMapel.GetByID_Entity(formasi.Rel_FormasiGuruMapel.ToString());
                if (m_formasi_head != null)
                {
                    if (m_formasi_head.TahunAjaran != null)
                    {
                        Pegawai m_pegawai = DAO_Pegawai.GetByID_Entity(formasi.Rel_Guru);
                        if (m_pegawai != null)
                        {
                            if (m_pegawai.Nama != null)
                            {
                                txtParseListGuru.Value += m_formasi_head.Rel_Kelas.ToString() + "->";
                                txtParseListGuru.Value += m_pegawai.Kode +
                                                          "|" +
                                                          m_pegawai.Nama +
                                                          ";";
                            }
                        }
                    }
                }
            }
        }

        private void BindListView(bool isbind = true, string keyword = "")
        {
            sql_ds.ConnectionString = Libs.GetConnectionString_ERP();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            if (keyword.Trim() != "")
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("nama", keyword);
                sql_ds.SelectCommand = DAO_Pembelajaran.SP_SELECT_ALL_FOR_SEARCH;
            }
            else
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectCommand = DAO_Pembelajaran.SP_SELECT_ALL;
            }
            if (isbind) lvData.DataBind();
        }

        protected void lvData_Sorting(object sender, ListViewSortEventArgs e)
        {
            System.Web.UI.WebControls.Literal imgh_tahunajaran = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_tahunajaran");
            System.Web.UI.WebControls.Literal imgh_semester = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_semester");
            System.Web.UI.WebControls.Literal imgh_kelas = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_kelas");
            System.Web.UI.WebControls.Literal imgh_guru = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_guru");
            System.Web.UI.WebControls.Literal imgh_mapel = (System.Web.UI.WebControls.Literal)lvData.FindControl("imgh_mapel");

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
            imgh_semester.Text = html_image;
            imgh_kelas.Text = html_image;
            imgh_guru.Text = html_image;
            imgh_mapel.Text = html_image;

            imgh_tahunajaran.Visible = false;
            imgh_semester.Visible = false;
            imgh_kelas.Visible = false;
            imgh_guru.Visible = false;
            imgh_mapel.Visible = false;

            switch (e.SortExpression.ToString().Trim())
            {
                case "TahunAjaran":
                    imgh_tahunajaran.Visible = true;
                    break;
                case "Semester":
                    imgh_semester.Visible = true;
                    break;
                case "Kelas":
                    imgh_kelas.Visible = true;
                    break;
                case "Guru":
                    imgh_guru.Visible = true;
                    break;
                case "Mapel":
                    imgh_mapel.Visible = true;
                    break;
            }

            int pageindex = int.Parse(Math.Ceiling(Convert.ToDecimal(dpData.StartRowIndex / 20)).ToString());
            pageindex--;
            this.Session[SessionViewDataName] = (pageindex < 0 ? 0 : pageindex);
        }

        protected void lvData_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {

        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            Response.Redirect(Libs.FILE_PAGE_URL);
        }

        protected void InitFields()
        {
            txtID.Value = "";
            cboKelas.SelectedValue = "";
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
                    DAO_Pembelajaran.Delete(txtID.Value);
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
                Pembelajaran m = new Pembelajaran();
                m.TahunAjaran = cboTahunAjaran.SelectedValue;
                m.Semester = cboSemester.SelectedValue;
                m.Rel_Kelas = txtKelas.Value;
                m.Rel_Pegawai = txtGuru.Value;
                m.Rel_Mapel = txtMapel.Value;
                if (txtID.Value.Trim() != "")
                {
                    m.Kode = new Guid(txtID.Value);
                    DAO_Pembelajaran.Update(m);
                    BindListView(!IsPostBack, Libs.GetQ().Trim());
                    txtKeyAction.Value = JenisAction.DoUpdate.ToString();
                }
                else
                {
                    DAO_Pembelajaran.Insert(m);
                    BindListView(!IsPostBack, Libs.GetQ().Trim());
                    InitFields();
                    txtKeyAction.Value = JenisAction.DoAdd.ToString();
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
                Pembelajaran m = DAO_Pembelajaran.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        cboTahunAjaran.SelectedValue = m.TahunAjaran;
                        cboSemester.SelectedValue = m.Semester;
                        txtKelas.Value = m.Rel_Kelas;
                        txtMapel.Value = m.Rel_Mapel;
                        txtGuru.Value = m.Rel_Pegawai;
                        cboKelas.SelectedValue = m.Rel_Kelas;
                        cboMataPelajaran.SelectedValue = m.Rel_Mapel;
                        cboGuru.SelectedValue = m.Rel_Pegawai;
                        txtKeyAction.Value = JenisAction.DoShowData.ToString();
                    }
                }
            }
        }

        protected void btnDoCari_Click(object sender, EventArgs e)
        {
            Response.Redirect(Libs.FILE_PAGE_URL + (this.Master.txtCariData.Text.Trim() != "" ? "?q=" + this.Master.txtCariData.Text : ""));
        }

        protected void btnShowConfirmDelete_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                Pembelajaran m = DAO_Pembelajaran.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        Pegawai m_pegawai = DAO_Pegawai.GetByID_Entity(m.Rel_Pegawai);
                        Kelas m_kelas = DAO_Kelas.GetByID_Entity(m.Rel_Kelas);

                        ltrMsgConfirmHapus.Text = "Hapus <span style=\"font-weight: bold;\">\"" +
                                                            Libs.GetHTMLSimpleText(m.TahunAjaran) +
                                                      "\"</span><br />" +
                                                      "Semester : " + m.Semester + "<br />" +
                                                      "Kelas : " + m_kelas.Nama + "<br />" +
                                                      "Guru : " + m_pegawai.Nama +
                                                      "?";
                        txtKeyAction.Value = JenisAction.DoShowConfirmHapus.ToString();
                    }
                }
            }
        }

        protected void btnShowMateriPembelajaran_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                Pembelajaran m = DAO_Pembelajaran.GetByID_Entity(txtID.Value);
                if (m != null)
                {
                    if (m.Materi != null)
                    {
                        txtTemplateMateri.Text = m.Materi;
                        txtTemplateMateriVal.Value = m.Materi;
                    }
                }

                mvMain.ActiveViewIndex = 1;
                this.Master.ShowHeaderTools = false;
            }
        }

        protected void lnkBatalPengaturanMateri_Click(object sender, EventArgs e)
        {
            ShowDataList();
        }

        protected void ShowDataList()
        {
            mvMain.ActiveViewIndex = 0;
            this.Master.ShowHeaderTools = true;
        }

        protected void btnOKSave_Click(object sender, EventArgs e)
        {
            DAO_Pembelajaran.UpdateMateri(
                txtID.Value,
                txtTemplateMateriVal.Value
            );
            ShowDataList();
        }

        protected void btnShowConfirmPublish_Click(object sender, EventArgs e)
        {
            if (txtID.Value.Trim() != "")
            {
                Pembelajaran m = DAO_Pembelajaran.GetByID_Entity(txtID.Value.Trim());
                if (m != null)
                {
                    if (m.TahunAjaran != null)
                    {
                        Pegawai m_pegawai = DAO_Pegawai.GetByID_Entity(m.Rel_Pegawai);
                        Kelas m_kelas = DAO_Kelas.GetByID_Entity(m.Rel_Kelas);

                        ltrMsgConfirmHapus.Text = "Publish Materi Pembelajaran <span style=\"font-weight: bold;\">\"" +
                                                            Libs.GetHTMLSimpleText(m.TahunAjaran) +
                                                      "\"</span><br />" +
                                                      "Semester : " + m.Semester + "<br />" +
                                                      "Kelas : " + m_kelas.Nama + "<br />" +
                                                      "Guru : " + m_pegawai.Nama +
                                                      "?";
                        txtKeyAction.Value = JenisAction.DoShowConfirmPublish.ToString();
                    }
                }
            }
        }

        protected void btnOKPublish_Click(object sender, EventArgs e)
        {
            DAO_Pembelajaran.UpdatePublished(
                txtID.Value,
                true
            );
            BindListView(true, Libs.GetQ());
            txtKeyAction.Value = JenisAction.DoUpdate.ToString();
        }
    }
}