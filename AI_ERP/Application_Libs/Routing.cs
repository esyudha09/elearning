using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

using AI_ERP.Application_Masters;

namespace AI_ERP.Application_Libs
{
    public static class Routing
    {
        public enum UnitSekolah
        {
            KB,
            TK,
            SD, 
            SMP,
            SMA
        }

        public static class URL
        {
            private static string GetRouteName(string url)
            {
                return url.Replace("~/", "");
            }

            public static class LOGIN
            {
                public const string ROUTE = "~/";
                public const string FILE = "~/Default.aspx";

                public static string RouteName { get { return GetRouteName(ROUTE); } }
            }

            public static class BERANDA
            {
                public const string ROUTE = "~/b";
                public const string FILE = "~/wf.Dashboard.aspx";

                public static string RouteName { get { return GetRouteName(ROUTE); } }
            }
            
            public static class BERANDA_SISWA
            {
                public const string ROUTE = "~/bs";
                public const string FILE = "~/wf.Dashboard.Siswa.aspx";

                public static string RouteName { get { return GetRouteName(ROUTE); } }
            }

            public static class UBAH_PASSWORD
            {
                public const string ROUTE = "~/up";
                public const string FILE = "~/wf.UbahPassword.aspx";

                public static string RouteName { get { return GetRouteName(ROUTE); } }
            }

            public static class RPT_ABSENSI_SISWA
            {
                public const string ROUTE = "~/rpt-presensi";
                public const string FILE = "~/Application_Modules/EDUCATION/Elearning/wf.Admin.Rpt.AbsensiSiswa.aspx";

                public static string RouteName { get { return GetRouteName(ROUTE); } }
            }

            public static class DOWNLOAD
            {
                public const string ROUTE = "~/download";
                public const string FILE = "~/Application_Resources/Download.aspx";

                public static string RouteName { get { return GetRouteName(ROUTE); } }
            }

            public static class LOGOUT
            {
                public const string ROUTE = "~/lo";
                public const string FILE = "~/Logout.aspx";

                public static string RouteName { get { return GetRouteName(ROUTE); } }
            }

            //apis
            public static class APIS
            {
                public static class _GENERAL
                {
                    public static class ABSENSI_SISWA
                    {
                        public static class DO_SAVE
                        {
                            public const string ID = "4F2692A5-7456-46EF-9E11-9795982CB1AD";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT._GENERAL + "0/as";
                            public const string FILE = "~/APIs/Elearning/_GENERAL/AbsensiSiswa/DoSave.svc";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }

                        public static class DO_VALIDATE
                        {
                            public const string ID = "44A1DA1A-E371-41F4-902D-16AC3DD0061D";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT._GENERAL + "1/vldt";
                            public const string FILE = "~/APIs/Elearning/_GENERAL/AbsensiSiswa/DoSave.svc";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }
                    }
                }

                public static class KB
                {
                    public static class NILAI_SISWA
                    {
                        public static class DO_SAVE
                        {
                            public const string ID = "D2629C55-F96A-4EF6-8333-4FDBBA9CB7C9";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.KB + "/ns";
                            public const string FILE = "~/APIs/Elearning/KB/NilaiSiswa/DoSave.svc";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }
                    }
                }

                public static class TK
                {
                    public static class NILAI_SISWA
                    {
                        public static class DO_SAVE
                        {
                            public const string ID = "A8D25081-0316-47AC-9C2E-65003F98C19C";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.TK + "/ns";
                            public const string FILE = "~/APIs/Elearning/TK/NilaiSiswa/DoSave.svc";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }
                    }
                }

                public static class SD
                {
                    public static class NILAI_SISWA
                    {
                        public static class DO_SAVE
                        {
                            public const string ID = "CCDBC53E-FDDB-4871-B49C-DE41C92F91A3";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SD + "/ns";
                            public const string FILE = "~/APIs/Elearning/SD/NilaiSiswa/DoSave.svc";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }
                    }
                }

                public static class SMP
                {
                    public static class NILAI_SISWA
                    {
                        public static class DO_SAVE
                        {
                            public const string ID = "3EFBFA7E-AD6A-4FAA-B177-E190C27C4402";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "/ns";
                            public const string FILE = "~/APIs/Elearning/SMP/NilaiSiswa/DoSave.svc";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }
                    }
                }

                public static class SMA
                {
                    public static class STRUKTUR_NILAI_SISWA
                    {
                        public static class DO_SAVE
                        {
                            public const string ID = "A11383A9-35C5-45FA-B53F-5E746AE1E2A1";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMA + "/dr";
                            public const string FILE = "~/APIs/Elearning/SMA/StrukturNilai/DoSaveDeskripsi.svc";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }
                    }

                    public static class NILAI_SISWA
                    {
                        public static class DO_SAVE
                        {
                            public const string ID = "0A5DCE5D-B8FC-4760-8386-8DDC25DC0889";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMA + "/ns";
                            public const string FILE = "~/APIs/Elearning/SMA/NilaiSiswa/DoSave.svc";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }
                    }
                }
            }
            //end apis

            public static class APPLIACTION_MODULES
            {
                //education modules
                public static class EDUCATION {

                    public const string dir_name = "e";

                    public static class Elearning
                    {
                        public static class LINK_PEMBELAJARAN_EKSTERNAL
                        {
                            public const string ID = "C6079C8C-DF39-4A51-9A77-907EA9D14298";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/link-pembelajaran-eksternal";
                            public const string FILE = "~/Application_Modules/EDUCATION/Elearning/wf.LinkPembelajaranEksternal.aspx";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }


                        public static class LIST_ABSENSI_SISWA
                        {
                            public const string ID = "88733987-F372-4398-8BCB-048D653959CA";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/presensi-siswa";
                            public const string FILE = "~/Application_Modules/EDUCATION/Elearning/wf.ListAbsensiSiswa.aspx";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }

                        public static class ORTU_PROFIL_SISWA
                        {
                            public const string ID = "0A3C6F6A-F51F-41DF-8569-AA888619D26A";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/profil-siswa";
                            public const string FILE = "~/Application_Modules/EDUCATION/Elearning/wf.Ortu.ProfilSiswa.aspx";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }

                        public static class ORTU_ABSENSI_SISWA
                        {
                            public const string ID = "F8D7B6E1-F541-4E33-AD6F-DEA3FB568B32";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/las-NdYWuo9OFAw";
                            public const string FILE = "~/Application_Modules/EDUCATION/Elearning/wf.Ortu.AbsensiSiswa.aspx";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }

                        public static class ORTU_NILAI_SISWA
                        {
                            public const string ID = "31DA727B-B236-4CB1-A341-95394C84D049";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/nilai-siswa";
                            public const string FILE = "~/Application_Modules/EDUCATION/Elearning/wf.Ortu.NilaiSiswa.aspx";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }

                        public static class ORTU_UANG_SEKOLAH
                        {
                            public const string ID = "A2F9E325-5E03-429D-AD45-975E7D2FE995";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/uang-sekolah";
                            public const string FILE = "~/Application_Modules/EDUCATION/Elearning/wf.Ortu.UangSekolah.aspx";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }

                        public static class ORTU_TRANSAKSI_KANTIN
                        {
                            public const string ID = "F073D8AD-5CA2-4021-95C8-3A63692778F8";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/transaksi-kantin";
                            public const string FILE = "~/Application_Modules/EDUCATION/Elearning/wf.Ortu.MutasiKantin.aspx";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }

                        public static class ORTU_KALENDER_AKADEMIK
                        {
                            public const string ID = "0EF3CAF8-CF1B-4300-A95C-3D55904F961C";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/kalender-akademik";
                            public const string FILE = "~/Application_Modules/EDUCATION/Elearning/wf.Ortu.KalenderAkademik.aspx";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }

                        public static class ORTU_TUGAS_SISWA
                        {
                            public const string ID = "6433B404-4584-4FF8-AFD8-EFE5CA165CC7";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/tugas-siswa";
                            public const string FILE = "~/Application_Modules/EDUCATION/Elearning/wf.Ortu.Tugas.aspx";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }

                        public static class ORTU_MATERI
                        {
                            public const string ID = "4B36860A-9472-4016-B078-834FE1FFC056";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/materi-pembelajaran";
                            public const string FILE = "~/Application_Modules/EDUCATION/Elearning/wf.Ortu.MateriPembelajaran.aspx";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }

                        public static class GURU_TIMELINE
                        {
                            public const string ID = "B48045AC-09DF-4843-A9E6-38E51626F7DE";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND.GURU + "/tl";
                            public const string FILE = "~/Application_Modules/EDUCATION/Elearning/wf.Guru.Timeline.aspx";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }

                        public static class GURU_DATASISWA
                        {
                            public const string ID = "49CFD826-AF3D-44A3-B7C8-41B1911B9D9D";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND.GURU + "/s";
                            public const string FILE = "~/Application_Modules/EDUCATION/Elearning/wf.Guru.DataSiswa.aspx";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }

                        public static class GURU_DATASISWACATATAN
                        {
                            public const string ID = "C7438565-6C19-4327-854A-2BD18656CC01";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND.GURU + "/cs";
                            public const string FILE = "~/Application_Modules/EDUCATION/Elearning/wf.Guru.DataSiswaCatatan.aspx";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }

                        public static class GURU_DATASISWACATATANEDIT
                        {
                            public const string ID = "A0FB43C3-4D15-49C6-AB68-9F05DF7FF5D3";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND.GURU + "/cse";
                            public const string FILE = "~/Application_Modules/EDUCATION/Elearning/wf.Guru.DataSiswaCatatanEdit.aspx";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }

                        public static class GURU_PRAOTA
                        {
                            public const string ID = "5F94D00C-A12D-4C3A-A00A-D0D948BFC993";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND.GURU + "/po";
                            public const string FILE = "~/Application_Modules/EDUCATION/Elearning/wf.Guru.Praota.aspx";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }
                    }

                    public static class Penilaian
                    {
                        public static class ALL
                        {
                            public static class PREVIEW_LTS_DAN_RAPOR
                            {
                                public const string ID = "F811E770-00A5-425B-A544-6EAF747F43D5";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" +
                                                                   RandomLibs.RND_UNIT.KB +
                                                                   RandomLibs.RND_UNIT.TK +
                                                                   RandomLibs.RND_UNIT.SD +
                                                                   RandomLibs.RND_UNIT.SMP +
                                                                   RandomLibs.RND_UNIT.SMA +
                                                            "m1/pldr";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/ALL/wf.PreviewLTSAndRapor.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class FILE_RAPOR
                            {
                                public const string ID = "433986DE-CEBC-430B-BA50-032E6F3A00A8";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + 
                                                                   RandomLibs.RND_UNIT.KB +
                                                                   RandomLibs.RND_UNIT.TK +
                                                                   RandomLibs.RND_UNIT.SD +
                                                                   RandomLibs.RND_UNIT.SMP +
                                                                   RandomLibs.RND_UNIT.SMA +
                                                            "0/fr";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/ALL/wf.FileRapor.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class FILE_RAPOR_VIEW
                            {
                                public const string ID = "58695A35-B158-4984-9B37-2CAB53D2F093";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" +
                                                                   RandomLibs.RND_UNIT.KB +
                                                                   RandomLibs.RND_UNIT.TK +
                                                                   RandomLibs.RND_UNIT.SD +
                                                                   RandomLibs.RND_UNIT.SMP +
                                                                   RandomLibs.RND_UNIT.SMA +
                                                            "1/frv";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/ALL/wf.FileRaporView.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }
                        }

                        public static class KB
                        {
                            public static class BUAT_NILAI_RAPOR_STANDAR
                            {
                                public const string ID = "5D75E063-09BF-45D8-83F9-3D5FC4E80EFD";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.KB + "0/cns";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/KB/_wf.CreateNilaiRaporStandar.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class KATEGORI_PENCAPAIAN
                            {
                                public const string ID = "850B19F2-CC2E-485F-8E9C-4D307F1A4B76";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.KB + "0/kp";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/KB/wf.KategoriPencapaian.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class SUB_KATEGORI_PENCAPAIAN
                            {
                                public const string ID = "B0FC84F7-7FB1-4BEE-8C59-6CA5CA9608E6";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.KB + "1/skp";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/KB/wf.SubKategoriPencapaian.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class POIN_KATEGORI_PENCAPAIAN
                            {
                                public const string ID = "652EE985-C4C5-4356-9973-F47D52464CEC";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.KB + "2/pkp";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/KB/wf.PoinKategoriPencapaian.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class KRITERIA_PENILAIAN
                            {
                                public const string ID = "8FB06523-5419-491B-B879-46633245B15E";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.KB + "3/krp";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/KB/wf.KriteriaPenilaian.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class DESAIN_RAPOR
                            {
                                public const string ID = "C451BB1B-A696-4100-9ADC-3B233A568D00";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.KB + "4/dr";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/KB/wf.DesainRapor.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class LIST_NILAI_SISWA
                            {
                                public const string ID = "07750344-E86F-4701-ABCB-200E0983F573";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.KB + "5/lns";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/KB/wf.NilaiRapor.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class NILAI_SISWA
                            {
                                public const string ID = "96CB34CF-6AC1-48A1-ACB5-8C7B9ACA7CA8";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.KB + "6/ns";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/KB/wf.NilaiRapor.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class NILAI_SISWA_PRINT
                            {
                                public const string ID = "7A440A4B-9971-49C7-8A0F-7A8CBB886DCA";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.KB + "7/RAPOR KB";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/KB/wf.NilaiRaporPrint.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class PENGATURAN_EKSKUL
                            {
                                public const string ID = "058893EB-002E-4167-87A4-F32CBD1358E4";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.KB + "8/pe";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/KB/wf.PengaturanEkskul.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class LIST_NILAI_EKSKUL
                            {
                                public const string ID = "14CEF611-6695-4D9E-9DA3-216D18795D49";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.KB + "9/lnse";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/KB/wf.NilaiRaporEkskul.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class PROSES_RAPOR
                            {
                                public const string ID = "F4AF9049-ECED-4F89-91F6-42B7AEE5FDA1";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.KB + "10/prl";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/KB/wf.ProsesRapor.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }
                        }

                        public static class TK
                        {
                            public static class BUAT_NILAI_RAPOR_STANDAR
                            {
                                public const string ID = "B7CBAEE2-A3A1-4302-BEE0-17865F8C86FB";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.TK + "0/cns";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/TK/_wf.CreateNilaiRaporStandar.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class KATEGORI_PENCAPAIAN
                            {
                                public const string ID = "D1B78E01-2049-481C-9956-3A9B9DA6CF0D";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.TK + "0/kp";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/TK/wf.KategoriPencapaian.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class SUB_KATEGORI_PENCAPAIAN
                            {
                                public const string ID = "1C09BA93-AE16-4A44-85AB-6CAB96AD90D5";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.TK + "1/skp";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/TK/wf.SubKategoriPencapaian.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class POIN_KATEGORI_PENCAPAIAN
                            {
                                public const string ID = "DE7946CA-CFCE-4015-A229-5344044E40A4";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.TK + "2/pkp";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/TK/wf.PoinKategoriPencapaian.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class KRITERIA_PENILAIAN
                            {
                                public const string ID = "31852E82-3E5E-4175-B9C7-C5F87BEEC9CC";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.TK + "3/krp";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/TK/wf.KriteriaPenilaian.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class DESAIN_RAPOR
                            {
                                public const string ID = "5666C83D-6F08-47F5-83B5-970FFD2C1ECB";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.TK + "4/dr";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/TK/wf.DesainRapor.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class LIST_NILAI_SISWA
                            {
                                public const string ID = "67280794-83D3-41BC-B82E-8236F51EC565";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.TK + "5/lns";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/TK/wf.NilaiRapor.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }
                            public static class NILAI_SISWA
                            {
                                public const string ID = "F0E37E53-CAED-4425-B1EC-814E4EDB82F4";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.TK + "6/ns";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/TK/wf.NilaiRapor.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class NILAI_SISWA_PRINT
                            {
                                public const string ID = "562F9C14-07E6-43C4-940E-2B15D89269C8";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.TK + "7/RAPOR TK";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/TK/wf.NilaiRaporPrint.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class PENGATURAN_EKSKUL
                            {
                                public const string ID = "230287CE-0F29-4DB9-A796-F0DE963993AD";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.TK + "8/pe";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/TK/wf.PengaturanEkskul.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class NILAI_EKSKUL
                            {
                                public const string ID = "449FE930-F6BC-4FA4-8E4F-A2256F70982D";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.TK + "9/nse";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/TK/wf.NilaiRaporEkskul.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class LIST_NILAI_EKSKUL
                            {
                                public const string ID = "B2DA49AD-E570-497E-B31C-35FD7AFFB19B";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.TK + "10/lnse";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/TK/wf.NilaiRaporEkskul.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class PROSES_RAPOR
                            {
                                public const string ID = "3C4D9DBE-A660-4B9C-BD3F-FE837293E8E4";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.TK + "11/prl";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/TK/wf.ProsesRapor.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }
                        }

                        public static class SD
                        {
                            public static class ASPEK_PENILAIAN
                            {
                                public const string ID = "7616FCAF-FD98-42E2-AAE2-B8592695EA20";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SD + "0/ap";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SD/wf.AspekPenilaian.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class KOMPETENSI_DASAR
                            {
                                public const string ID = "EC7B5E34-AAD1-446B-B73B-AB448169CB07";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SD + "1/kd";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SD/wf.KompetensiDasar.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }
                            public static class KOMPONEN_PENILAIAN
                            {
                                public const string ID = "E2595C6B-FBAF-49DA-95EB-9D1D42FBAD20";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SD + "2/kp";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SD/wf.KomponenPenilaian.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }
                            public static class STRUKTUR_PENILAIAN
                            {
                                public const string ID = "7DA23E80-D242-4030-B9E2-D755FCED20CE";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SD + "3/sp";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SD/wf.StrukturPenilaian.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }
                            public static class DESAIN_RAPOR
                            {
                                public const string ID = "0AE83CF9-5099-4F0D-8EA6-9CCC8CFDA0C5";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SD + "4/dr";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SD/wf.DesainRapor.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class DESAIN_RAPOR_LTS
                            {
                                public const string ID = "358D726F-2230-40BF-B903-7D2779A83449";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SD + "5/drl";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SD/wf.DesainRaporLTS.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class RAPOR_LTS
                            {
                                public const string ID = "F218D27F-F767-4D73-B3B5-89A034C75165";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SD + "6/nrl";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SD/wf.LTS.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class NILAI_SISWA
                            {
                                public const string ID = "A1826806-3C15-4CB0-8822-3AC7350C0162";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SD + "7/ns";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SD/wf.NilaiSiswa.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class NILAI_SISWA_PRINT
                            {
                                public const string ID = "80347CC8-583A-47A0-B851-E599094270CE";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SD + "8/RAPOR SD";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SD/wf.NilaiRaporPrint.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class NILAI_SISWA_LTS
                            {
                                public const string ID = "D753179C-7521-413F-B794-921E39F28DC6";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SD + "9/nsl";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SD/wf.NilaiSiswaLTS.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class PROSES_RAPOR
                            {
                                public const string ID = "E4721AB5-6DFC-4EC0-A40B-DB34472EE3AA";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SD + "10/prl";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SD/wf.ProsesRapor.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class LIST_NILAI_SISWA
                            {
                                public const string ID = "1CBC4946-0977-41C2-BC4C-BD2A927F66D7";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "11/lns";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SD/wf.ListNilaiSiswa.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class LIST_NILAI_EKSKUL
                            {
                                public const string ID = "3D1A5235-4912-4076-A563-6F29F44E41D2";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "12/lnse";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SD/wf.ListNilaiSiswaEkskul.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class VOLUNTEER
                            {
                                public const string ID = "B5810329-FE11-4DBE-AAC6-BA04BF0F7787";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SD + "13/vs";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SD/wf.NilaiSiswaVolunteer.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class PENGATURAN_VOLUNTEER
                            {
                                public const string ID = "47D6A4FC-CB73-48AE-97F4-69F8624A6C0C";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SD + "14/volsgs";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SD/wf.VolunteerSettings.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class PENGATURAN_RAPOR_SD
                            {
                                public const string ID = "16A5A49A-53A4-423D-A38B-5AD8AD21D299";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SD + "15/pngtrpt";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SD/wf.PengaturanRapor.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class CETAK_LTS
                            {
                                public const string ID = "503CC7EB-B6B2-4968-89CF-719F027D20C2";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SD + "15/cl";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SD/wf.NilaiSiswaLTSPrint.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class PILIH_EKSKUL
                            {
                                public const string ID = "16D9539E-97B1-49F3-A790-B5B2AD503EBC";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SD + "16/pekl";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SD/wf.PilihEkstrakurikuler.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class NILAI_EKSKUL
                            {
                                public const string ID = "3FDFFFC2-415E-4B4B-BD76-E9908BB8A3D3";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SD + "17/nse";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SD/wf.NilaiSiswaEkskul.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class CATATAN_SISWA
                            {
                                public const string ID = "736016A1-7D96-46CD-8EE7-82E468F5F561";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SD + "18/csw";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SD/wf.NilaiCatatanSiswa.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class NILAI_SISWA_SIKAP_SEMESTER
                            {
                                public const string ID = "AF26EEA6-55A0-4D3C-9504-2F78A72857DA";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SD + "19/nss";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SD/wf.NilaiSiswaSikap.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class LIHAT_LEDGER
                            {
                                public const string ID = "F0B51AF2-DF0A-449B-93C7-CE4A4081D5D8";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SD + "20/llgr";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SD/wf.NilaiSiswaLedger.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class CREATE_REPORT
                            {
                                public const string ID = "C218DB0F-ECF2-4548-8EA8-540F443DCC51";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SD + "21/ctrt";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SD/_wf.CreateReport.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }
                        }

                        public static class SMP
                        {
                            public static class ASPEK_PENILAIAN
                            {
                                public const string ID = "B41C30FD-FA6F-4777-938A-4CABE5E7262D";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "0/ap";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMP/wf.AspekPenilaian.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class KOMPETENSI_DASAR
                            {
                                public const string ID = "B267A76B-E92B-42D8-858E-873AE3A9C7E9";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "1/kd";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMP/wf.KompetensiDasar.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }
                            public static class KOMPONEN_PENILAIAN
                            {
                                public const string ID = "A383FAE4-2C90-4BF4-87D4-CD6157D3F21E";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "2/kp";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMP/wf.KomponenPenilaian.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }
                            public static class STRUKTUR_PENILAIAN
                            {
                                public const string ID = "3EFED196-CBE1-4076-9905-9127B5DD6729";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "2/sp";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMP/wf.StrukturPenilaian.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class FORMASI_EKSKUL
                            {
                                public const string ID = "68D2C3C9-7ED0-410F-98C2-8E0161F404A5";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "3/fe";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMP/wf.FormasiSiswaEkskul.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class NILAI_SISWA
                            {
                                public const string ID = "3CE605F3-AC57-4899-B8A4-8E3BD226EED1";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "3/ns";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMP/wf.NilaiSiswa.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class DESAIN_RAPOR_LTS
                            {
                                public const string ID = "CA73F202-B497-4A2B-95A3-7D760B110DF1";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "4/drl";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMP/wf.DesainRaporLTS.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class DESAIN_RAPOR
                            {
                                public const string ID = "84051E89-68BD-4F78-9493-DACA8FC01141";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "5/dr";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMP/wf.DesainRapor.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class NILAI_SISWA_LTS
                            {
                                public const string ID = "66960B34-5A22-4B7B-90A6-3DCD96727D90";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "6/nsl";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMP/wf.NilaiSiswaLTS.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class PROSES_RAPOR
                            {
                                public const string ID = "27259F98-46B9-41DF-B7ED-8E67D34BC19D";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "7/prls";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMP/wf.ProsesRapor.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class LIST_NILAI_SISWA
                            {
                                public const string ID = "67499B11-82EB-4AB5-8833-43CC66648247";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "8/lns";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMP/wf.ListNilaiSiswa.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class CETAK_LTS
                            {
                                public const string ID = "89344C48-15DA-45D9-8DB3-6E60F33C39F5";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "9/cl";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMP/wf.NilaiSiswaLTSPrint.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class VOLUNTEER
                            {
                                public const string ID = "0873ECBF-0166-4A4A-BE2E-25D0670F9CB0";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "10/vs";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMP/wf.Volunteer.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class CATATAN_SISWA
                            {
                                public const string ID = "DB191794-A402-424F-8140-2AC62DFFD9B9";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "11/csw";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMP/wf.NilaiCatatanSiswa.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class LIHAT_LEDGER
                            {
                                public const string ID = "A6761847-2887-42F9-A333-AABFE1ED47FE";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "12/llgr";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMP/wf.NilaiSiswaLedger.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class CREATE_REPORT
                            {
                                public const string ID = "1AAA1EDE-0EE2-4B72-B9C2-A028A67BDE3D";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "13/ctrt";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMP/_wf.CreateReport.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class KEPRIBADIAN_SISWA
                            {
                                public const string ID = "F2FCA446-0DE9-47A0-B216-3568CC5FE683";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "14/nks";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMP/wf.NilaiKepribadianSiswa.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class DOWNLOAD_REPORT_RAPOR
                            {
                                public const string ID = "66BF6013-975D-4DD6-9A12-B9D688F3A196";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "15/dlr";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMP/_wf.Download.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class NILAI_SISWA_PRINT
                            {
                                public const string ID = "24F6E8AA-E088-41B2-AFC2-0880354CCB20";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "16/RAPOR SMP";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMP/wf.NilaiRaporPrint.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class NILAI_SISWA_SIKAP
                            {
                                public const string ID = "C9C22033-B0FA-4DB0-93AF-B9A63A832E22";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "17/nskp";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMP/wf.NilaiSiswaSikap.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }
                        }

                        public static class SMA
                        {
                            public static class ASPEK_PENILAIAN
                            {
                                public const string ID = "D44E983A-14BF-4052-AA37-B81FDBD99225";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMA + "0/ap";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMA/wf.AspekPenilaian.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class KOMPETENSI_DASAR
                            {
                                public const string ID = "8C70144B-E276-45AA-BEE8-0CC34B351BFE";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMA + "1/kd";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMA/wf.KompetensiDasar.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }
                            public static class KOMPONEN_PENILAIAN
                            {
                                public const string ID = "3A36561D-F329-4D57-BF02-0B41520E5DF7";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMA + "2/kp";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMA/wf.KomponenPenilaian.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }
                            public static class STRUKTUR_PENILAIAN
                            {
                                public const string ID = "4ADB4D3F-B06D-4F39-A340-046913890600";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMA + "2/sp";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMA/wf.StrukturPenilaian.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }
                            public static class FORMASI_EKSKUL
                            {
                                public const string ID = "0201EE79-83B9-4666-8031-8827238C6F7C";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMA + "3/fe";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMA/wf.FormasiSiswaEkskul.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }
                            public static class NILAI_SISWA
                            {
                                public const string ID = "1EABACA4-52EE-424D-8618-B02B6A7BB3F1";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMA + "4/ns";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMA/wf.NilaiSiswa.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class NILAI_SISWA_SIKAP
                            {
                                public const string ID = "750FEBD1-F3EC-4627-9505-CD48D5105A4C";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMA + "5/nskp";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMA/wf.NilaiSiswaSikap.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class NILAI_SISWA_EKSKUL
                            {
                                public const string ID = "E9073268-54D2-480F-AC29-E840A45D1CCD";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMA + "6/ne";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMA/wf.NilaiSiswaEkskul.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }
                            public static class DESAIN_RAPOR
                            {
                                public const string ID = "77C3B83D-092A-4C91-83B4-B174437302A9";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMA + "7/dr";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMA/wf.DesainRapor.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }
                            public static class PROSES_RAPOR
                            {
                                public const string ID = "1FA3CCB2-33A9-45F2-9F3B-6834E1BA1B0F";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMA + "8/prl";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMA/wf.ProsesRapor.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class PENGATURAN_RAPOR_SMA
                            {
                                public const string ID = "B35CB38E-898B-4DE1-B7B9-27C0141B8889";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMA + "9/pngtrpt";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMA/wf.PengaturanRapor.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class LIST_NILAI_SISWA
                            {
                                public const string ID = "6DF9E249-FD13-4F01-9CBB-07242FB4383C";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMA + "10/lns";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMA/wf.ListNilaiSiswa.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }
                            public static class DESAIN_RAPOR_LTS
                            {
                                public const string ID = "6DF9E249-FD13-4F01-9CBB-07242FB4383C";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMA + "11/drl";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMA/wf.DesainRaporLTS.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }
                            public static class LIHAT_LEDGER
                            {
                                public const string ID = "1A37D49B-B65D-4E04-9BD0-C78996D35EBE";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMA + "12/ln";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMA/wf.NilaiSiswaLedger.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }
                            public static class NILAI_SISWA_LTS
                            {
                                public const string ID = "429D2A12-1D38-4ABB-A6E1-D96AD6F57356";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMA + "13/nsl";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMA/wf.NilaiSiswaLTS.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }
                            public static class NILAI_SISWA_PRINT
                            {
                                public const string ID = "747025FB-3FF8-4A07-9A69-2A6527F498DE";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMA + "14/RAPOR SMA";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMA/wf.NilaiRaporPrint.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }
                            public static class CREATE_REPORT
                            {
                                public const string ID = "29647DF7-7D89-4734-A011-4AD83BC2354D";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMA + "15/ctrt";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMA/_wf.CreateReport.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }
                            public static class VOLUNTEER
                            {
                                public const string ID = "352D1F6C-C67E-4E6D-9334-D2C7B751F38A";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "16/vs";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMA/wf.Volunteer.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }

                            public static class CATATAN_SISWA
                            {
                                public const string ID = "428B4DBD-27F2-4C83-A052-AC7066194219";
                                public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "17/csw";
                                public const string FILE = "~/Application_Modules/EDUCATION/Penilaian/SMA/wf.NilaiCatatanSiswa.aspx";

                                public static string RouteName { get { return GetRouteName(ROUTE); } }
                            }
                        }
                    }
                }
                //end education modules

                //loader
                public static class LOADER
                {
                    public static class BUKA_SEMESTER
                    {
                        public const string ID = "CE9A75EB-1DF7-40B2-A8B4-E6631379A0EA";
                        public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND.LOADER + "/bs";
                        public const string FILE = "~/Application_Modules/__LOADER/wf.BukaSemester.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class BUKA_SEMESTER_ERROR
                    {
                        public const string ID = "CBB2D865-D415-4D48-8292-BE5DABB5CD3C";
                        public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND.LOADER + "/bse";
                        public const string FILE = "~/Application_Modules/__LOADER/wf.BukaSemesterError.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class BUKA_SEMESTER_INFO
                    {
                        public const string ID = "B13C0C4B-DC42-42A5-9140-C57019DEA0AA";
                        public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND.LOADER + "/bsi";
                        public const string FILE = "~/Application_Modules/__LOADER/wf.BukaSemesterInfo.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class ABSEN_SISWA
                    {
                        public const string ID = "6DBCDAB0-E3ED-4786-9872-EF5D09054403";
                        public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND.LOADER + "/as";
                        public const string FILE = "~/Application_Modules/__LOADER/wf.SiswaAbsen.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class ABSEN_MAPEL_SISWA
                    {
                        public const string ID = "D9096995-5AEA-423A-B2D3-408162AB181D";
                        public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND.LOADER + "/ams";
                        public const string FILE = "~/Application_Modules/__LOADER/wf.SiswaAbsenMapel.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class SD
                    {
                        public static class NILAI_SISWA
                        {
                            public const string ID = "FA60C176-1D58-497F-AED1-61BF08B8DCAB";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SD + "-" + RandomLibs.RND.LOADER + "/ns";
                            public const string FILE = "~/Application_Modules/__LOADER/SD/wf.NilaiSiswa.aspx";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }

                        public static class BUKA_SEMESTER
                        {
                            public const string ID = "050C458C-0828-428F-BC10-9BE8A5179977";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SD + "-" + RandomLibs.RND.LOADER + "/bs";
                            public const string FILE = "~/Application_Modules/__LOADER/SD/wf.BukaSemester.aspx";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }
                    }
                                        
                    public static class SMP
                    {
                        public static class NILAI_SISWA
                        {
                            public const string ID = "94AE0242-B9C9-4832-A3AA-430F3B471598";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "-" + RandomLibs.RND.LOADER + "/ns";
                            public const string FILE = "~/Application_Modules/__LOADER/SMP/wf.NilaiSiswa.aspx";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }

                        public static class BUKA_SEMESTER
                        {
                            public const string ID = "C7A32D5F-BA73-468D-AAAF-D0E528E22E60";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "-" + RandomLibs.RND.LOADER + "/bs";
                            public const string FILE = "~/Application_Modules/__LOADER/SMP/wf.BukaSemester.aspx";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }
                    }

                    public static class SMA
                    {
                        public static class NILAI_SISWA
                        {
                            public const string ID = "E21C684A-37B5-4C4A-9374-BA415D491AC8";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMA + "-" + RandomLibs.RND.LOADER + "/ns";
                            public const string FILE = "~/Application_Modules/__LOADER/SMA/wf.NilaiSiswa.aspx";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }

                        public static class BUKA_SEMESTER
                        {
                            public const string ID = "E9A97DDB-04C9-4A98-9D6A-5464FF361708";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMA + "-" + RandomLibs.RND.LOADER + "/bs";
                            public const string FILE = "~/Application_Modules/__LOADER/SMA/wf.BukaSemester.aspx";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }
                    }

                    public static class ALL_UNIT
                    {
                        public static class DOWNLOAD_FILE
                        {
                            public const string ID = "AB8FD8A8-BA0C-44E0-B79D-A304D4D32D96";
                            public const string ROUTE = "~/d/" + RandomLibs.RND.DOWNLOAD + "/df";
                            public const string FILE = "~/Application_Resources/Download.aspx";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }

                        public static class DELETE_FILE
                        {
                            public const string ID = "0DC6299F-C464-4814-85FA-DB2072589489";
                            public const string ROUTE = "~/d/" + RandomLibs.RND.DELETE + "/delf";
                            public const string FILE = "~/Application_Resources/Delete.aspx";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }
                    }
                }
                //end loader

                public static class GURU
                {
                    public static class SMP
                    {
                        public static class MATERI_PEMBELAJARAN
                        {
                            public const string ID = "53CB75E0-1C4F-42EB-B6FA-DCC8CEE4B107";
                            public const string ROUTE = "~/" + APPLIACTION_MODULES.EDUCATION.dir_name + "/" + RandomLibs.RND_UNIT.SMP + "/mp";
                            public const string FILE = "~/Application_Modules/EDUCATION/Elearning/wf.Guru.Materi.aspx";

                            public static string RouteName { get { return GetRouteName(ROUTE); } }
                        }
                    }
                }

                //master modules
                public static class MASTER
                {
                    public static class DIVISI
                    {
                        public const string ID = "A4CCDA31-FB4C-4400-A20C-1888AED4D74E";
                        public const string ROUTE = "~/m/d";
                        public const string FILE = "~/Application_Modules/MASTER/wf.Divisi.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class UNIT_NON_SEKOLAH
                    {
                        public const string ID = "0A5F7ED6-8D70-4F23-AA53-6BF9909F7309";
                        public const string ROUTE = "~/m/uns";
                        public const string FILE = "~/Application_Modules/MASTER/wf.NonSekolah.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class UNIT_SEKOLAH
                    {
                        public const string ID = "DF8B83FF-A2C7-4C6C-BBB7-775B5187E91B";
                        public const string ROUTE = "~/m/us";
                        public const string FILE = "~/Application_Modules/MASTER/wf.Sekolah.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class KELAS
                    {
                        public const string ID = "7F05794D-345B-4016-B929-C0047815ECB7";
                        public const string ROUTE = "~/m/k";
                        public const string FILE = "~/Application_Modules/MASTER/wf.Kelas.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class SISWA
                    {
                        public const string ID = "153B7D64-9AB5-44F3-8CC1-5CAFF48CCEEE";
                        public const string ROUTE = "~/m/s";
                        public const string FILE = "~/Application_Modules/MASTER/wf.Siswa.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class SISWA_INPUT
                    {
                        public const string ID = "21580918-C9BC-4027-95C4-28462E8AB41E";
                        public const string ROUTE = "~/m/is";
                        public const string FILE = "~/Application_Modules/MASTER/wf.Siswa.Input.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class PEGAWAI
                    {
                        public const string ID = "A60BB0CF-84B7-4463-82A9-83B0C272980E";
                        public const string ROUTE = "~/m/p";
                        public const string FILE = "~/Application_Modules/MASTER/wf.Pegawai.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class PEGAWAI_INPUT
                    {
                        public const string ID = "479C04CE-45DD-4B21-B2F8-E651BAA0E6F9";
                        public const string ROUTE = "~/m/pi";
                        public const string FILE = "~/Application_Modules/MASTER/wf.Pegawai.Input.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class MAPEL
                    {
                        public const string ID = "33343819-48D2-4358-A33C-C384FD94BC3D";
                        public const string ROUTE = "~/m/m";
                        public const string FILE = "~/Application_Modules/MASTER/wf.Mapel.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }
                    
                    public static class RUANG_KELAS
                    {
                        public const string ID = "FFA2F140-F393-4D8D-BDD5-E1D447F33172";
                        public const string ROUTE = "~/m/rk";
                        public const string FILE = "~/Application_Modules/MASTER/wf.RuangKelas.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class FORMASI_GURU_KELAS
                    {
                        public const string ID = "36A50A6E-F9F1-4FEB-BB27-A0BA6D4AAB09";
                        public const string ROUTE = "~/m/fgk";
                        public const string FILE = "~/Application_Modules/MASTER/wf.FormasiGuruKelas.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class FORMASI_GURU_MAPEL
                    {
                        public const string ID = "8C26E531-E90F-4C48-BA24-F28C17165339";
                        public const string ROUTE = "~/m/fgm";
                        public const string FILE = "~/Application_Modules/MASTER/wf.FormasiGuruMapel.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class FORMASI_WALI_KELAS
                    {
                        public const string ID = "C3EAB477-8009-4383-9AA6-68A29CB75627";
                        public const string ROUTE = "~/m/fwk";
                        public const string FILE = "~/Application_Modules/MASTER/wf.FormasiWaliKelas.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class JADWAL_MAPEL
                    {
                        public const string ID = "E93862F7-7854-4E2C-A7F6-C001F63DE364";
                        public const string ROUTE = "~/m/jmp";
                        public const string FILE = "~/Application_Modules/MASTER/wf.MapelJadwal.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class DETAIL_JADWAL_MAPEL
                    {
                        public const string ID = "33343819-48D2-4358-A33C-C384FD94BC3D";
                        public const string ROUTE = "~/m/dm";
                        public const string FILE = "~/Application_Modules/MASTER/wf.JadwalMapelDetail.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class PENGATURAN_SD
                    {
                        public const string ID = "0BDBDA78-FB62-4405-B95C-34D1EEDF7A5F";
                        public const string ROUTE = "~/m/pngt/" + RandomLibs.RND_UNIT.SD;
                        public const string FILE = "~/Application_Modules/MASTER/wf.Pengaturan.SD.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class PENGATURAN_SMP
                    {
                        public const string ID = "AB11E955-1248-44C5-8CDC-D4A4F55651B9";
                        public const string ROUTE = "~/m/pngt/" + RandomLibs.RND_UNIT.SMP;
                        public const string FILE = "~/Application_Modules/MASTER/wf.Pengaturan.SMP.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class PENGATURAN_SMA
                    {
                        public const string ID = "9A41FEC3-E7A5-48E4-9127-CE3334D817BF";
                        public const string ROUTE = "~/m/pngt/" + RandomLibs.RND_UNIT.SMA;
                        public const string FILE = "~/Application_Modules/MASTER/wf.Pengaturan.SMA.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class USER_MANAGEMENT
                    {
                        public const string ID = "31E4FF8F-9CFF-4E97-AB20-7C13945176F8";
                        public const string ROUTE = "~/m/um/";
                        public const string FILE = "~/Application_Modules/MASTER/wf.UserManagement.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }
                }
                //end master modules

                //library/perpustakaan
                public static class LIBRARY
                {
                    public static class PENGATURAN_KUNJUNGAN_PERPUSTAKAAN
                    {
                        public const string ID = "A626CA70-47F2-430F-ADC0-F92578B0EE2E";
                        public const string ROUTE = "~/l/5b4bfd466ec62/mpkp";
                        public const string FILE = "~/Application_Modules/MASTER/Perpustakaan/wf.PengaturanJadwalKunjungan.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class PENGATURAN_KUNJUNGAN_PERPUSTAKAAN_RUTIN
                    {
                        public const string ID = "7002EA31-FB00-4EA3-A828-302A089D738C";
                        public const string ROUTE = "~/l/97hgjfft86djlje/pjkr";
                        public const string FILE = "~/Application_Modules/MASTER/Perpustakaan/wf.PengaturanJadwalKunjunganRutin.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class LIST_KUNJUNGAN_PERPUSTAKAAN
                    {
                        public const string ID = "B47367EB-8B5A-42CD-A221-432A6E8747D0";
                        public const string ROUTE = "~/l/85tjwqwreydf/lkp";
                        public const string FILE = "~/Application_Modules/LIBRARY/wf.ListKunjunganPerpustakaan.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class KUNJUNGAN_PERPUSTAKAAN
                    {
                        public const string ID = "2D13092E-C917-4ACF-933F-FCF6A757F6C7";
                        public const string ROUTE = "~/l/93notypjdshkljfs/kp";
                        public const string FILE = "~/Application_Modules/LIBRARY/wf.KunjunganPerpustakaan.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }
                }
                //end library/perpustakaan

                public static class LINK_OPENER
                {
                    public const string JENIS_LO_LINK_PEMBELAJARAN_EKSTERNAL = "ADA5F3D5-1B0F-46BF-9611-C2FFBD00D6C9";

                    public const string ID = "7E138075-5F2E-4BCA-A162-F420EFAD03FF";
                    public const string ROUTE = "~/1s7ELhiOrgQ/link";
                    public const string FILE = "~/Application_Modules/wf.LinkOpener.aspx";

                    public static string RouteName { get { return GetRouteName(ROUTE); } }
                }

                public static class CBT
                {
                    public static class MAPEL
                    {
                        public const string ID = "ff5c86c3-1f1d-4cfa-9578-988095bfa648";
                        public const string ROUTE = "~/cbt/m";
                        public const string FILE = "~/Application_Modules/CBT/wf.MapelCBT.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class SOAL
                    {
                        public const string ID = "ff5c86c3-1f1d-4cfa-9578-988095bfa648";
                        public const string ROUTE = "~/cbt/s";
                        public const string FILE = "~/Application_Modules/CBT/wf.BankSoal.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }
                    public static class SOAL_VIEW
                    {
                        public const string ID = "ff5c86c3-1f1d-4cfa-9578-988095bfa648";
                        public const string ROUTE = "~/cbt/sv";
                        public const string FILE = "~/Application_Modules/CBT/wf.BankSoal.View.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class SOAL_INPUT
                    {
                        public const string ID = "ff5c86c3-1f1d-4cfa-9578-988095bfa648";
                        public const string ROUTE = "~/cbt/si";
                        public const string FILE = "~/Application_Modules/CBT/wf.BankSoal.Input.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class RUMAH_SOAL_SMP
                    {
                        public const string ID = "ff5c86c3-1f1d-4cfa-9578-988095bfa648";
                        public const string ROUTE = "~/cbt/rssmp";
                        public const string FILE = "~/Application_Modules/CBT/wf.RumahSoalSMP.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class RUMAH_SOAL_SMA
                    {
                        public const string ID = "ff5c86c3-1f1d-4cfa-9578-988095bfa648";
                        public const string ROUTE = "~/cbt/rssma";
                        public const string FILE = "~/Application_Modules/CBT/wf.RumahSoalSMA.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class RUMAH_SOAL_INPUT
                    {
                        public const string ID = "ff5c86c3-1f1d-4cfa-9578-988095bfa648";
                        public const string ROUTE = "~/cbt/rsi";
                        public const string FILE = "~/Application_Modules/CBT/wf.RumahSoal.Input.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }

                    public static class DESIGN_SOAL
                    {
                        public const string ID = "ff5c86c3-1f1d-4cfa-9578-988095bfa648";
                        public const string ROUTE = "~/cbt/ds";
                        public const string FILE = "~/Application_Modules/CBT/wf.DesignSoal.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }
                    
                    public static class MAPEL_SISWA
                    {
                        public const string ID = "ff5c86c3-1f1d-4cfa-9578-988095bfa648";
                        public const string ROUTE = "~/cbt/ms";
                        public const string FILE = "~/Application_Modules/CBT/wf.MapelCBT.Siswa.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }
                    
                    public static class START_ATTEMPT
                    {
                        public const string ID = "ff5c86c3-1f1d-4cfa-9578-988095bfa648";
                        public const string ROUTE = "~/cbt/sa";
                        public const string FILE = "~/Application_Modules/CBT/wf.StartAttempt.aspx";

                        public static string RouteName { get { return GetRouteName(ROUTE); } }
                    }
                }
            }

        }
    }
}