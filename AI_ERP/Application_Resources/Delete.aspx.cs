using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using AI_ERP.Application_Libs;

namespace AI_ERP.Application_Resources
{
    public partial class Delete : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string id = AI_ERP.Application_Libs.Libs.GetQueryString("id");
                string id2 = AI_ERP.Application_Libs.Libs.GetQueryString("id2");
                string jenis = AI_ERP.Application_Libs.Libs.GetQueryString("jenis");
                string file = AI_ERP.Application_Libs.Libs.GetNamaFileFromURL(
                                AI_ERP.Application_Libs.Libs.GetQueryString("file").Trim()
                              );
                string savepath = "";

                if (jenis == Libs.JENIS_UPLOAD.MATERI_PEMBELAJARAN)
                {
                    savepath = Libs.GetFolderElearningMateriPembelajaran(id, id2);
                }
                else if (jenis == Libs.JENIS_UPLOAD.PENDIDIKAN_NON_FORMAL)
                {
                    savepath = Libs.GetFolderPendidikanNonFormal(id, id2);
                }
                else if (jenis == Libs.JENIS_UPLOAD.RIWAYAT_KESEHATAN)
                {
                    savepath = Libs.GetFolderRiwayatKesehatan(id, id2);
                }
                else if (jenis == Libs.JENIS_UPLOAD.RIWAYAT_MCU)
                {
                    savepath = Libs.GetFolderRiwayatMCU(id, id2);
                }
                else if (jenis == Libs.JENIS_UPLOAD.FILE_PENDUKUNG)
                {
                    savepath = Libs.GetFolderFilePendukung(id, id2);
                }
                else if (jenis == Libs.JENIS_UPLOAD.RAPOR)
                {
                    string rel_siswa = AI_ERP.Application_Libs.Libs.GetQueryString("sw");
                    string tahun_ajaran = AI_ERP.Application_Libs.Libs.GetQueryString("ta");
                    string semester = AI_ERP.Application_Libs.Libs.GetQueryString("sm");
                    string rel_kelasdet = AI_ERP.Application_Libs.Libs.GetQueryString("kd");
                    string unit = AI_ERP.Application_Libs.Libs.GetQueryString("un");
                    string tipe_rapor = AI_ERP.Application_Libs.Libs.GetQueryString("tr");
                    id = rel_siswa;

                    if (tipe_rapor.ToUpper().Trim() == TipeRapor.LTS)
                    {
                        savepath = Libs.GetLokasiFolderFileLTS(
                            rel_siswa, tahun_ajaran, semester, rel_kelasdet, (Libs.UnitSekolah)Libs.GetStringToInteger(unit)
                        );
                    }
                    else if (tipe_rapor.ToUpper().Trim() == TipeRapor.SEMESTER)
                    {
                        savepath = Libs.GetLokasiFolderFileRapor(
                            rel_siswa, tahun_ajaran, semester, rel_kelasdet, (Libs.UnitSekolah)Libs.GetStringToInteger(unit)
                        );
                    }
                }
                else if (jenis == "file_rapor")
                {
                    string[] arr_id2 = id2.Split(new string[] { ";" }, StringSplitOptions.None);
                    if (arr_id2.Length == 3)
                    {
                        string tahun_ajaran = arr_id2[0].Replace("_", "/");
                        string semester = arr_id2[1];
                        string rel_kelasdet = arr_id2[2];
                        AI_ERP.Application_Entities.KelasDet m_kelas_det = AI_ERP.Application_DAOs.DAO_KelasDet.GetByID_Entity(rel_kelasdet);
                        if (m_kelas_det != null)
                        {
                            if (m_kelas_det.Nama != null)
                            {
                                AI_ERP.Application_Entities.Kelas m_kelas = AI_ERP.Application_DAOs.DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());
                                if (m_kelas != null)
                                {
                                    if (m_kelas.Nama != null)
                                    {
                                        AI_ERP.Application_Entities.Sekolah m_sekolah = AI_ERP.Application_DAOs.DAO_Sekolah.GetByID_Entity(m_kelas.Rel_Sekolah.ToString());
                                        if (m_sekolah != null)
                                        {
                                            if (m_sekolah.Nama != null)
                                            {
                                                savepath = Libs.GetLokasiFolderFileRapor(
                                                    id, tahun_ajaran, semester, rel_kelasdet,
                                                    (AI_ERP.Application_Libs.Libs.UnitSekolah)m_sekolah.UrutanJenjang
                                                );
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (savepath.Trim() != "")
                {
                    if (file.Trim() != "")
                    {
                        string kode_tr = Libs.GetQueryString("kode_tr").Trim();

                        if (id.Trim() == "" || savepath.Trim() == "") return;

                        System.IO.File.Delete(Server.MapPath(savepath + "/" + file));

                        if (jenis == Libs.JENIS_UPLOAD.FILE_PENDUKUNG)
                        {
                            ClientScript.RegisterStartupScript(this.GetType()
                                , @"AutoShowListFilePendukung"
                                , @"if (typeof window.parent.ShowUploadedFilesPendukung === 'function') window.parent.ShowUploadedFilesPendukung();"
                                , true
                            );
                        }
                        else if (jenis == Libs.JENIS_UPLOAD.RAPOR)
                        {
                            ClientScript.RegisterStartupScript(this.GetType()
                                , @"RefreshListFileRapor"
                                , @"if (typeof window.parent.RefreshListFileRapor === 'function') window.parent.RefreshListFileRapor();"
                                , true
                            );
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType()
                                , @"AutoSizeFrame"
                                , @"if (typeof window.parent.ResizeFrameDel === 'function') window.parent.ResizeFrameDel();"
                                , true
                            );
                        }

                        if (kode_tr != "")
                        {
                            Page.ClientScript.RegisterStartupScript(
                                this.GetType()
                                , @"FileDeleted"
                                , @"parent.document.getElementById('" + kode_tr + "').style = 'display: none';"
                                , true
                            );
                        }
                    }
                    else if (file.Trim() == "")
                    {
                        //delete folder header or det di materi pembelajaran
                        System.IO.Directory.Delete(Server.MapPath(savepath), true);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}