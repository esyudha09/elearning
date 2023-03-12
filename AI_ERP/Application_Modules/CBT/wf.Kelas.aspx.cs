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

namespace AI_ERP.Application_Modules.CBT
{
    public partial class wf_Kelas : System.Web.UI.Page
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


                string tahun_ajaran = "";
                string semester = "";


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


                //lst_nilai_kurtilas = DAO_Rapor_NilaiSiswa_KURTILAS_Det.GetAllByTABySM_Entity(tahun_ajaran, semester);
                //lst_nilai_ktsp = DAO_Rapor_NilaiSiswa_KTSP_Det.GetAllByTABySM_Entity(tahun_ajaran, semester);
           

            }

            
               
                    this.Master.ShowHeaderTools = true;

            BindListView(true, Libs.GetQ());
           
        }


        public Sekolah GetUnit()
        {
            return DAO_Sekolah.GetAll_Entity().FindAll(
                m => m.UrutanJenjang == (int)Libs.UnitSekolah.SMA).FirstOrDefault();
        }



        private void BindListView(bool isbind = true, string keyword = "")
        {



            string unit = Libs.GetQueryString("u");

            sql_ds.ConnectionString = Libs.GetConnectionString_Rapor();
            sql_ds.SelectCommandType = SqlDataSourceCommandType.StoredProcedure;
            if (keyword.Trim() != "")
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("nama", keyword);
                sql_ds.SelectParameters.Add("TahunAjaran", Libs.GetTahunAjaranNow());
                sql_ds.SelectParameters.Add("Rel_Mapel", Libs.GetQueryString("m"));
                if (unit == "SMA")
                {
                    sql_ds.SelectCommand = DAO_CBT_RumahSoalSMA.SP_SMA_Rapor_StrukturNilai_SELECT_ALL_BY_TA_BY_MAPEL_FOR_SEARCH;
                }
            }
            else
            {
                sql_ds.SelectParameters.Clear();
                sql_ds.SelectParameters.Add("TahunAjaran", Libs.GetTahunAjaranNow());
                //sql_ds.SelectParameters.Add("Semester", semester);
                sql_ds.SelectParameters.Add("Rel_Mapel", Libs.GetQueryString("m"));

                sql_ds.SelectCommand = DAO_CBT_RumahSoalSMA.SP_SMA_Rapor_StrukturNilai_SELECT_ALL_BY_TA_BY_MAPEL;
            }
            if (isbind) lvData.DataBind();

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


        protected void btnShowStrukturNilai_Click(object sender, EventArgs e)
        {
            var mapel = Libs.GetQueryString("m");
            var unit = Libs.GetQueryString("u");
            if (unit == "SMA")
            {
                Response.Redirect(Routing.URL.APPLIACTION_MODULES.CBT.STRUKTUR_PENILAIAN_SMA2.ROUTE + "?m=" + mapel + "&u=" + unit+ "&sn=" + txtID.Value);
            }
            //else if (unit == "SMP")
            //{
            //    Response.Redirect(Routing.URL.APPLIACTION_MODULES.CBT.STRUKTUR_NILAI.ROUTE + "?m=" + mapel + "&u=" + unit);
            //}

        }
































        protected void btnShowDataList_Click(object sender, EventArgs e)
        {
            mvMain.ActiveViewIndex = 0;
            this.Master.ShowHeaderTools = true;
            BindListView(true, Libs.GetQ());
            txtKeyAction.Value = JenisAction.ShowDataList.ToString();
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
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }





    }
}
