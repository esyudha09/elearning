using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Entities;
using AI_ERP.Application_DAOs;
using AI_ERP.Application_Libs;

namespace AI_ERP.Application_Modules.EDUCATION.Elearning
{
    public partial class wf_Ortu_MutasiKantin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            this.Master.HeaderTittle = "<i class=\"fa fa-money\"></i>&nbsp;&nbsp;" +
                                       "Transaksi Kantin";
            this.Master.ShowHeaderTools = false;
            this.Master.HeaderCardVisible = false;
            ShowUangKantin();
        }

        protected void ShowUangKantin()
        {
            ltrListTransaksiKantin.Text = "";

            List<UserOrtuDet> lst_user_ortu_det = DAO_UserOrtuDet.SelectByUserID(Libs.LOGGED_USER_M.UserID);
            foreach (UserOrtuDet m_det in lst_user_ortu_det)
            {
                ltrListTransaksiKantin.Text = "";
                Siswa m = DAO_Siswa.GetByID_Entity(
                    Libs.GetTahunAjaranNow(),
                    Libs.GetSemesterByTanggal(DateTime.Now).ToString(),
                    m_det.NIS
                );
                if (m != null)
                {
                    string list_biaya = "<table style=\"width: 100%;\">";
                    list_biaya += "<thead>" +
                                    "<tr>" +
                                        "<td style=\"font-weight: bold; text-align: center; background-color: #F5F5F5;\">" +
                                            "<i class=\"fa fa-hashtag\"></i>" +
                                        "</td>" +
                                        "<td style=\"font-weight: bold; background-color: #F5F5F5;\">" +
                                            "JENIS" +
                                        "</td>" +
                                        "<td style=\"font-weight: bold; background-color: #F5F5F5;\">" +
                                            "COUNTER" +
                                        "</td>" +
                                        "<td style=\"font-weight: bold; text-align: right; background-color: #F5F5F5;\">" +
                                            "TANGGAL" +
                                        "</td>" +
                                        "<td style=\"font-weight: bold; text-align: right; background-color: #F5F5F5;\">" +
                                            "SALDO AWAL" +
                                        "</td>" +
                                        "<td style=\"font-weight: bold; text-align: right; background-color: #F5F5F5;\">" +
                                            "JUMLAH" +
                                        "</td>" +
                                        "<td style=\"font-weight: bold; text-align: right; background-color: #F5F5F5;\">" +
                                            "SALDO AKHIR" +
                                        "</td>" +
                                    "</tr>" +
                                  "</thead>";

                    list_biaya += "<tbody>";

                    int id = 0;
                    foreach (_MutasiKantin item in _DAO_MutasiKantin.GetByNIS_Entity(m_det.NIS))
                    {
                        string warnabg = (id % 2 == 0 ? "#ffffff" : "#fbfbfb");
                        list_biaya +=   "<tr>" +
                                            "<td style=\"font-weight: normal; text-align: center; background-color: " + warnabg + ";\">" +
                                                (id + 1).ToString() +
                                            "</td>" +
                                            "<td style=\"font-weight: normal; background-color: " + warnabg + ";\">" +
                                                item.Keterangan +
                                            "</td>" +
                                            "<td style=\"font-weight: normal; background-color: " + warnabg + ";\">" +
                                                item.NoKasir +
                                            "</td>" +
                                            "<td style=\"font-weight: normal; text-align: right; background-color: " + warnabg + ";\">" +
                                                item.Tanggal.ToString("dd/MM/yyyy HH:mm:ss") +
                                            "</td>" +
                                            "<td style=\"font-weight: normal; text-align: right; background-color: " + warnabg + ";\">" +
                                                Libs.GetFormatBilangan(item.SaldoAwal) +
                                            "</td>" +
                                            "<td style=\"font-weight: normal; text-align: right; background-color: " + warnabg + ";\">" +
                                                Libs.GetFormatBilangan(item.Total) +
                                            "</td>" +
                                            "<td style=\"font-weight: normal; text-align: right; background-color: " + warnabg + ";\">" +
                                                Libs.GetFormatBilangan(item.SaldoAkhir) +
                                            "</td>" +
                                        "</tr>";

                        if (id == 0)
                        {
                            lblSaldoUangKantin.Text = Libs.GetFormatBilangan(item.SaldoAkhir) + ",-";
                        }

                        id++;
                    }

                    list_biaya += "</tbody>";
                    list_biaya += "</table>";

                    ltrListTransaksiKantin.Text = list_biaya;
                }
            }
        }
    }
}