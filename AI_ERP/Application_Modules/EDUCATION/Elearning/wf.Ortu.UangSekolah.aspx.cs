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
    public partial class wf_Ortu_UangSekolah : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[Constantas.NAMA_SESSION_LOGIN] == null)
            {
                Libs.RedirectToLogin(this.Page);
            }

            this.Master.HeaderTittle = "<i class=\"fa fa-money\"></i>&nbsp;&nbsp;" +
                                       "Uang Sekolah";
            this.Master.ShowHeaderTools = false;
            this.Master.HeaderCardVisible = false;
            ShowUangSekolah();
        }

        protected void ShowUangSekolah()
        {
            List<UserOrtuDet> lst_user_ortu_det = DAO_UserOrtuDet.SelectByUserID(Libs.LOGGED_USER_M.UserID);
            foreach (UserOrtuDet m_det in lst_user_ortu_det)
            {
                ltrListBiayaSekolah.Text = "";
                Siswa m = DAO_Siswa.GetByID_Entity(
                    Libs.GetTahunAjaranNow(),
                    Libs.GetSemesterByTanggal(DateTime.Now).ToString(),
                    m_det.NIS);
                if (m != null)
                {
                    List<SiswaBiayaDanDibayar> lst_biaya = DAO_SiswaBiaya.GetAllAsTagihan_Entity(DateTime.Now, m_det.NIS);
                    decimal tagihan = 0;
                    foreach (SiswaBiayaDanDibayar item in lst_biaya)
                    {
                        tagihan += (item.Jumlah + item.Denda) - (item.Dibayar + item.DendaDibayar);
                    }
                    lblJumlahTagihan.Text = Libs.GetFormatBilangan(tagihan) + ",-";

                    //list biaya
                    int id_tahun_ajaran = 1;
                    List<string> lst_tahun_ajaran = DAO_SiswaBiaya.GetDistinctTahunAjaranBySiswa(m.NIS).OrderByDescending(t => t).ToList();
                    foreach (string tahun_ajaran in lst_tahun_ajaran)
                    {
                        string list_biaya = "<table style=\"width: 100%;\">";
                        list_biaya += "<thead>" +
                                        "<tr>" +
                                            "<td style=\"font-weight: bold; text-align: center; background-color: #F5F5F5;\">" +
                                                "<i class=\"fa fa-hashtag\"></i>" +
                                            "</td>" +
                                            "<td style=\"font-weight: bold; background-color: #F5F5F5;\">" +
                                                "ITEM BIAYA" +
                                            "</td>" +
                                            "<td style=\"font-weight: bold; text-align: right; background-color: #F5F5F5;\">" +
                                                "BIAYA" +
                                            "</td>" +
                                            "<td style=\"font-weight: bold; text-align: right; background-color: #F5F5F5;\">" +
                                                "DENDA" +
                                            "</td>" +
                                            "<td style=\"font-weight: bold; text-align: right; background-color: #F5F5F5;\">" +
                                                "DIBAYAR" +
                                            "</td>" +
                                            "<td style=\"font-weight: bold; text-align: right; background-color: #F5F5F5;\">" +
                                                "KURANG BAYAR" +
                                            "</td>" +
                                        "</tr>" +
                                      "</thead>";

                        list_biaya += "<tbody>";
                        List<SiswaBiayaDanDibayar> lst_siswa_biaya = DAO_SiswaBiaya.GetBySiswaByTahunAjaran_Entity(m.NIS, tahun_ajaran, DateTime.Now);
                        int id = 0;
                        int nomor_biaya = 0;
                        bool ada_data_biaya = false;
                        string th_ajaran_for_id = tahun_ajaran.Replace("/", "");
                        foreach (SiswaBiayaDanDibayar item in lst_siswa_biaya)
                        {
                            decimal jumlah_denda = item.Denda;
                            decimal jumlah_tagihan = item.Jumlah + jumlah_denda;
                            decimal jumlah_tagihan_dibayar = item.Dibayar;

                            decimal jumlah_dibayar = (
                                    jumlah_tagihan_dibayar > (item.Jumlah + jumlah_denda)
                                    ? (item.Jumlah + jumlah_denda)
                                    : jumlah_tagihan_dibayar
                                );

                            ItemBiaya item_biaya = DAO_ItemBiaya.GetByID_Entity(item.Rel_ItemBiaya.ToString());

                            if (item.Jumlah > 0 || (item.Jumlah == 0 && !item_biaya.IsBiayaTerbuka))
                            {
                                ada_data_biaya = true;

                                decimal sisa = (
                                                    item.IsBebas
                                                    ? 0
                                                    : (
                                                        item.IsBebasDenda
                                                        ? (jumlah_tagihan - jumlah_denda) - jumlah_dibayar
                                                        : jumlah_tagihan - jumlah_dibayar
                                                      )
                                               );
                                if (sisa < 0) sisa = 0;

                                string warnabg = (id % 2 == 0 ? "#ffffff" : "#fbfbfb");
                                string s_id = item.Kode.ToString().Replace("-", "_");
                                string id_check_bebas = "bebas_" + s_id;
                                string id_check_bebas_denda = "bebas_denda_" + s_id;
                                string txt_biaya_id = "txtbiaya_" + s_id;
                                string lbl_sisa = "lbl_sisa_" + s_id;
                                string css_textbox = " border-style: solid; border-width: 1px; border-color: #E1E1E1; ";
                                string css_txt_biaya_edit =
                                            txt_biaya_id + ".style.paddingRight = '5px'; " +
                                            txt_biaya_id + ".style.borderStyle = 'solid'; " +
                                            txt_biaya_id + ".style.background = 'white'; " +
                                            txt_biaya_id + ".disabled = false; ";
                                string css_txt_biaya_noedit =
                                            txt_biaya_id + ".style.paddingRight = '0px'; " +
                                            txt_biaya_id + ".style.borderStyle = 'none'; " +
                                            txt_biaya_id + ".style.background = 'transparent'; " +
                                            txt_biaya_id + ".disabled = true; ";
                                bool is_bisa_diedit = false;

                                list_biaya += "<tr>" +
                                                "<td colspan=\"6\" style=\"padding: 0px; background-color: " + warnabg + ";\">" +
                                                    "<hr style=\"margin: 0px;\" />" +
                                                "</td>" +
                                              "</tr>";
                                list_biaya += "<tr>" +
                                                "<td style=\"text-align: center; background-color: " + warnabg + ";\">" +
                                                    (id + 1).ToString() + "." +
                                                "</td>" +
                                                "<td style=\"background-color: " + warnabg + "; font-weight: bold;\">" +
                                                    item_biaya.Nama +
                                                    (
                                                        item.Keterangan.Trim() != ""
                                                        ? "<br /><label style=\"font-weight: normal; color: grey;\">" + item.Keterangan + " </label>" +
                                                          "<label class=\"pull-right\" style=\"font-weight: normal; color: grey;\">" + item.TahunAjaran + "</label>"
                                                        : ""
                                                    ) +
                                                "</td>" +
                                                "<td style=\"text-align: right; background-color: " + warnabg + "; font-weight: bold; color: #C40000;\">" +
                                                    (
                                                        is_bisa_diedit
                                                        ? "<input title=\"" + (item_biaya.IsBulanan ? "Bulanan" : "") + "\" " +
                                                                  "id=\"" + txt_biaya_id + "\"" +
                                                                  "name=\"txtBiaya_" + th_ajaran_for_id + "[]\" " +
                                                                  "onblur=\"this.value = SetTandaPemisahTitikUseDesimal(this.value, 0); CheckIfBiayaChanged(" + nomor_biaya.ToString() + ", " + th_ajaran_for_id + ", this.value); \" " +
                                                                  "onfocus=\"this.value = GetPureNumber('', this.value); \" " +
                                                                  "onkeydown=\"return SetInputNumberOnly(event);\" " +
                                                                  "type=\"textbox\" " +
                                                                  "style=\"" + css_textbox + " text-align: right; font-weight: bold; padding: 5px; \" " +
                                                                  "value=\"" + Libs.GetFormatBilangan(item.Jumlah) + "\" />" +
                                                          "<input type=\"hidden\" name=\"txtKetBiaya_" + th_ajaran_for_id + "[]\" value=\"" + item.Keterangan + "\" />" +
                                                          "<input type=\"hidden\" name=\"txtDenda_" + th_ajaran_for_id + "[]\" value=\"" + jumlah_denda + "\" />" +
                                                          "<input type=\"hidden\" name=\"txtDibayar_" + th_ajaran_for_id + "[]\" value=\"" + jumlah_dibayar + "\" />" +
                                                          "<input type=\"hidden\" name=\"txtIDBiaya_" + th_ajaran_for_id + "[]\" value=\"" + item.Rel_ItemBiaya.ToString() + "\" />" +
                                                          "<input type=\"hidden\" name=\"txtIDSiswaBiaya_" + th_ajaran_for_id + "[]\" value=\"" + item.Kode.ToString() + "\" />"

                                                        : Libs.GetFormatBilangan(item.Jumlah)
                                                    ) +
                                                "</td>" +
                                                "<td style=\"text-align: right; background-color: " + warnabg + "; font-weight: bold; color: #C40000;\">" +
                                                    Libs.GetFormatBilangan(jumlah_denda) +
                                                "</td>" +
                                                "<td style=\"text-align: right; background-color: " + warnabg + "; font-weight: bold; color: #027B02;\">" +
                                                    Libs.GetFormatBilangan(jumlah_dibayar) +
                                                "</td>" +
                                                "<td style=\"text-align: right; background-color: " + warnabg + "; font-weight: bold; color: #D78A0D;\">" +
                                                    "<label " + (is_bisa_diedit ? " name=\"lbl_sisa_" + th_ajaran_for_id + "[]\"" : "") + " id=\"" + lbl_sisa + "\" style=\"font-weight: bold;\">" +
                                                        Libs.GetFormatBilangan(sisa) +
                                                    "</label>" +
                                                "</td>" +
                                              "</tr>";

                                if (is_bisa_diedit) nomor_biaya++;
                                id++;
                            }
                        }
                        list_biaya += "</tbody>";
                        list_biaya += "</table>";

                        if (ada_data_biaya)
                        {
                            string id_ui = "ui_tile_" + th_ajaran_for_id;
                            ltrListBiayaSekolah.Text += "<div class=\"tile tile-collapse\">" +
                                                            "<div data-target=\"#" + id_ui + "\" data-toggle=\"tile\" style=\"background-color: " + (id_tahun_ajaran % 2 == 0 ? "#ffffff" : "#fbfbfb") + ";\">" +
                                                                "<div class=\"tile-side pull-left\" data-ignore=\"tile\" style=\"width: 80px;\">" +
                                                                    "<div class=\"col-xs-2\" style=\"padding-top: 5px;\">" +
                                                                        "<label style=\"margin: 0 auto; display: table;\">" +
                                                                            id_tahun_ajaran.ToString() + ". " +
                                                                        "</label>" +
                                                                    "</div>" +
                                                                    "<div class=\"col-xs-10\" style=\"margin-right: 0px; padding-right: 0px;\">" +
                                                                        "<div class=\"avatar avatar-sm\">" +
                                                                            "<i class=\"fa fa-calendar\"></i>" +
                                                                        "</div>" +
                                                                    "</div>" +
                                                                "</div>" +
                                                                "<div class=\"tile-inner\">" +
                                                                    "<div class=\"text-overflow\" style=\"font-weight: bold;\">" + tahun_ajaran + "</div>" +
                                                                "</div>" +
                                                            "</div>" +
                                                            "<div class=\"tile-active-show collapse\" id=\"" + id_ui + "\" style=\"height: 0px;\">" +
                                                                "<div class=\"tile-sub\" style=\"padding-left: 0px; padding-right: 0px;\">" +
                                                                    list_biaya +
                                                                "</div>" +
                                                                "<div class=\"tile-footer\">" +
                                                                    "<div class=\"tile-footer-btn pull-left\">" +
                                                                        (
                                                                            m.TahunAjaran.Trim() == tahun_ajaran.Trim()
                                                                            ? "<a class=\"btn btn-flat waves-attach waves-effect\" href=\"javascript:void(0)\" style=\"display: none;\"><span class=\"icon\">check</span>&nbsp;OK</a>"
                                                                            : ""
                                                                        ) +
                                                                        "<a class=\"btn btn-flat waves-attach waves-effect\" data-toggle=\"tile\" href=\"#" + id_ui + "\"><span class=\"icon\">close</span>&nbsp;Tutup</a>" +
                                                                    "</div>" +
                                                                "</div>" +
                                                            "</div>" +
                                                        "</div>";

                            id_tahun_ajaran++;
                        }
                    }
                }
            }

        }
    }
}