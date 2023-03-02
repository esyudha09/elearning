using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;
using System.ServiceModel.Activation;

using AI_ERP.Application_Libs;

namespace AI_ERP
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.MapPageRoute(
                "Login",
                Routing.URL.LOGIN.RouteName,
                Routing.URL.LOGIN.FILE
            );

            RouteTable.Routes.MapPageRoute(
                "Beranda",
                Routing.URL.BERANDA.RouteName,
                Routing.URL.BERANDA.FILE
            );

            RouteTable.Routes.MapPageRoute(
                "Beranda Siswa",
                Routing.URL.BERANDA_SISWA.RouteName,
                Routing.URL.BERANDA_SISWA.FILE
            );

            RouteTable.Routes.MapPageRoute(
                "GantiPassword",
                Routing.URL.UBAH_PASSWORD.RouteName,
                Routing.URL.UBAH_PASSWORD.FILE
            );

            RouteTable.Routes.MapPageRoute(
                "RptAbsensiSiswa",
                Routing.URL.RPT_ABSENSI_SISWA.RouteName,
                Routing.URL.RPT_ABSENSI_SISWA.FILE
            );

            RouteTable.Routes.MapPageRoute(
                "Logout",
                Routing.URL.LOGOUT.RouteName,
                Routing.URL.LOGOUT.FILE
            );

            
            RouteTable.Routes.MapPageRoute(
                "ElearningLinkPembelajaranEksternal",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.LINK_PEMBELAJARAN_EKSTERNAL.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.LINK_PEMBELAJARAN_EKSTERNAL.FILE
            );

            RouteTable.Routes.MapPageRoute(
                "ElearningListAbsensiSiswa",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.LIST_ABSENSI_SISWA.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.LIST_ABSENSI_SISWA.FILE
            );

            RouteTable.Routes.MapPageRoute(
                "ElearningOrtuProfilSiswa",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.ORTU_PROFIL_SISWA.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.ORTU_PROFIL_SISWA.FILE
            );

            RouteTable.Routes.MapPageRoute(
                "ElearningOrtuAbsensiSiswa",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.ORTU_ABSENSI_SISWA.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.ORTU_ABSENSI_SISWA.FILE
            );

            RouteTable.Routes.MapPageRoute(
                "ElearningOrtuNilaiSiswa",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.ORTU_NILAI_SISWA.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.ORTU_NILAI_SISWA.FILE
            );

            RouteTable.Routes.MapPageRoute(
                "ElearningOrtuUangSekolah",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.ORTU_UANG_SEKOLAH.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.ORTU_UANG_SEKOLAH.FILE
            );

            RouteTable.Routes.MapPageRoute(
                "ElearningOrtuMutasiKantin",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.ORTU_TRANSAKSI_KANTIN.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.ORTU_TRANSAKSI_KANTIN.FILE
            );

            RouteTable.Routes.MapPageRoute(
                "ElearningOrtuKalenderAkademik",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.ORTU_KALENDER_AKADEMIK.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.ORTU_KALENDER_AKADEMIK.FILE
            );

            RouteTable.Routes.MapPageRoute(
                "ElearningOrtuTugas",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.ORTU_TUGAS_SISWA.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.ORTU_TUGAS_SISWA.FILE
            );

            RouteTable.Routes.MapPageRoute(
                "ElearningOrtuMateri",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.ORTU_MATERI.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.ORTU_MATERI.FILE
            );
                        
            //guru
            RouteTable.Routes.MapPageRoute(
                "TimeLineGuru",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_TIMELINE.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_TIMELINE.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "DataSiswaGuru",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_DATASISWA.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_DATASISWA.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "DataSiswaCatatanGuru",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_DATASISWACATATAN.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_DATASISWACATATAN.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "DataSiswaGuruPraota",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_PRAOTA.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_PRAOTA.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "ElearningGuruCatatanEdit",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_DATASISWACATATANEDIT.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Elearning.GURU_DATASISWACATATANEDIT.FILE
            );

            //nilai siswa kb
            RouteTable.Routes.MapPageRoute(
                "BuatNilaiStandar_KB",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.BUAT_NILAI_RAPOR_STANDAR.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.BUAT_NILAI_RAPOR_STANDAR.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "DesainRapor_KB",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.DESAIN_RAPOR.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.DESAIN_RAPOR.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "NilaiSiswa_KB",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.NILAI_SISWA.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.NILAI_SISWA.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "PengaturanEkskul_KB",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.PENGATURAN_EKSKUL.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.PENGATURAN_EKSKUL.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "NilaiSiswaPrint_KB",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.NILAI_SISWA_PRINT.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.NILAI_SISWA_PRINT.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "PenilaianKategoriPencapaian_KB",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.KATEGORI_PENCAPAIAN.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.KATEGORI_PENCAPAIAN.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "PenilaianSubKategoriPencapaian_KB",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.SUB_KATEGORI_PENCAPAIAN.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.SUB_KATEGORI_PENCAPAIAN.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "PenilaianPoinKategoriPencapaian_KB",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.POIN_KATEGORI_PENCAPAIAN.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.POIN_KATEGORI_PENCAPAIAN.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "KriteriaPenilaian_KB",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.KRITERIA_PENILAIAN.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.KRITERIA_PENILAIAN.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "ListNilaiSiswa_KB",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.LIST_NILAI_SISWA.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.LIST_NILAI_SISWA.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "ListNilaiSiswaEkskul_KB",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.LIST_NILAI_EKSKUL.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.LIST_NILAI_EKSKUL.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "ProsesRapor_KB",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.PROSES_RAPOR.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.KB.PROSES_RAPOR.FILE
            );
            //end nilai siswa tk

            //nilai siswa tk
            RouteTable.Routes.MapPageRoute(
                "BuatNilaiStandar_TK",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.BUAT_NILAI_RAPOR_STANDAR.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.BUAT_NILAI_RAPOR_STANDAR.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "DesainRapor_TK",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.DESAIN_RAPOR.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.DESAIN_RAPOR.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "NilaiSiswa_TK",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.NILAI_SISWA.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.NILAI_SISWA.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "NilaiSiswaPrint_TK",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.NILAI_SISWA_PRINT.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.NILAI_SISWA_PRINT.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "PenilaianKategoriPencapaian_TK",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.KATEGORI_PENCAPAIAN.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.KATEGORI_PENCAPAIAN.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "PenilaianSubKategoriPencapaian_TK",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.SUB_KATEGORI_PENCAPAIAN.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.SUB_KATEGORI_PENCAPAIAN.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "PenilaianPoinKategoriPencapaian_TK",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.POIN_KATEGORI_PENCAPAIAN.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.POIN_KATEGORI_PENCAPAIAN.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "KriteriaPenilaian_TK",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.KRITERIA_PENILAIAN.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.KRITERIA_PENILAIAN.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "ListNilaiSiswa_TK",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.LIST_NILAI_SISWA.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.LIST_NILAI_SISWA.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "PengaturanEkskul_TK",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.PENGATURAN_EKSKUL.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.PENGATURAN_EKSKUL.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "NilaiSiswaEkskul_TK",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.NILAI_EKSKUL.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.NILAI_EKSKUL.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "ListNilaiSiswaEkskul_TK",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.LIST_NILAI_EKSKUL.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.LIST_NILAI_EKSKUL.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "ProsesRapor_TK",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.PROSES_RAPOR.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.TK.PROSES_RAPOR.FILE
            );
            //end nilai siswa tk

            //nilai siswa sd
            RouteTable.Routes.MapPageRoute(
                "NilaiSiswa_SD",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_SISWA.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_SISWA.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "NilaiSiswaLTS_SD",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_SISWA_LTS.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_SISWA_LTS.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "ProsesRapor_SD",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.PROSES_RAPOR.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.PROSES_RAPOR.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "ListNilaiSiswa_SD",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.LIST_NILAI_SISWA.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.LIST_NILAI_SISWA.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "ListNilaEkskul_SD",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.LIST_NILAI_EKSKUL.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.LIST_NILAI_EKSKUL.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "VolunteerSettings_SD",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.PENGATURAN_VOLUNTEER.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.PENGATURAN_VOLUNTEER.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "VolunteerSiswa_SD",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.VOLUNTEER.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.VOLUNTEER.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "CetakLTS_SD",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.CETAK_LTS.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.CETAK_LTS.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "LihatLedger_SD",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.LIHAT_LEDGER.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.LIHAT_LEDGER.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "PilihEkskul_SD",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.PILIH_EKSKUL.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.PILIH_EKSKUL.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "NilaiSiswaEkskul_SD",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_EKSKUL.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_EKSKUL.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "NilaiCatatanSiswa_SD",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.CATATAN_SISWA.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.CATATAN_SISWA.FILE
            );            
            RouteTable.Routes.MapPageRoute(
                "NilaiSikapSemesterSiswa_SD",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_SISWA_SIKAP_SEMESTER.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_SISWA_SIKAP_SEMESTER.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "CreateReport_SD",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.CREATE_REPORT.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.CREATE_REPORT.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "NilaiSiswaPrint_SD",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_SISWA_PRINT.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.NILAI_SISWA_PRINT.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "PengaturanRapor_SD",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.PENGATURAN_RAPOR_SD.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.PENGATURAN_RAPOR_SD.FILE
            );
            //end nilai siswa sd

            //nilai siswa smp
            RouteTable.Routes.MapPageRoute(
                "NilaiSiswa_SMP",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.NILAI_SISWA.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.NILAI_SISWA.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "NilaiSiswaLTS_SMP",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.NILAI_SISWA_LTS.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.NILAI_SISWA_LTS.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "ProsesRapor_SMP",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.PROSES_RAPOR.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.PROSES_RAPOR.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "ListNilaiSiswa_SMP",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.LIST_NILAI_SISWA.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.LIST_NILAI_SISWA.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "CetakLTS_SMP",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.CETAK_LTS.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.CETAK_LTS.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "NilaiCatatanSiswa_SMP",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.CATATAN_SISWA.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.CATATAN_SISWA.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "NilaiKepribadianSiswa_SMP",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.KEPRIBADIAN_SISWA.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.KEPRIBADIAN_SISWA.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "VolunteerSiswa_SMP",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.VOLUNTEER.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.VOLUNTEER.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "LihatLedger_SMP",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.LIHAT_LEDGER.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.LIHAT_LEDGER.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "CreateReport_SMP",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.CREATE_REPORT.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.CREATE_REPORT.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "DownloadReportRapor_SMP",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.DOWNLOAD_REPORT_RAPOR.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.DOWNLOAD_REPORT_RAPOR.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "NilaiSiswaPrint_SMP",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.NILAI_SISWA_PRINT.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.NILAI_SISWA_PRINT.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "NilaiSiswaSikap_SMP",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.NILAI_SISWA_SIKAP.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.NILAI_SISWA_SIKAP.FILE
            );
            //end nilai siswa smp

            //nilai siswa sma
            RouteTable.Routes.MapPageRoute(
                "DesainRapor_SMA",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.DESAIN_RAPOR.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.DESAIN_RAPOR.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "NilaiSiswa_SMA",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "NilaiSiswaEkskul_SMA",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA_EKSKUL.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA_EKSKUL.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "NilaiSiswaSikap_SMA",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA_SIKAP.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA_SIKAP.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "LedgerNilai_SMA",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.LIHAT_LEDGER.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.LIHAT_LEDGER.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "NilaiSiswaLTS_SMA",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA_LTS.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA_LTS.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "NilaiSiswaPrint_SMA",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA_PRINT.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.NILAI_SISWA_PRINT.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "NilaiCatatanSiswa_SMA",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.CATATAN_SISWA.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.CATATAN_SISWA.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "VolunteerSiswa_SMA",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.VOLUNTEER.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.VOLUNTEER.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "PengaturanRapor_SMA",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.PENGATURAN_RAPOR_SMA.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.PENGATURAN_RAPOR_SMA.FILE
            );
            //end nilai siswa sma

            //end guru

            //apis
            RouteTable.Routes.MapPageRoute(
                "API_Absensi_Siswa",
                Routing.URL.APIS._GENERAL.ABSENSI_SISWA.DO_SAVE.RouteName,
                Routing.URL.APIS._GENERAL.ABSENSI_SISWA.DO_SAVE.FILE
            );
            //end apis

            //loader
            RouteTable.Routes.MapPageRoute(
                "Loader_BukaSemester_File",
                Routing.URL.APPLIACTION_MODULES.LOADER.BUKA_SEMESTER.RouteName,
                Routing.URL.APPLIACTION_MODULES.LOADER.BUKA_SEMESTER.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Loader_BukaSemesterInfo_File",
                Routing.URL.APPLIACTION_MODULES.LOADER.BUKA_SEMESTER_INFO.RouteName,
                Routing.URL.APPLIACTION_MODULES.LOADER.BUKA_SEMESTER_INFO.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Loader_BukaSemesterError_File",
                Routing.URL.APPLIACTION_MODULES.LOADER.BUKA_SEMESTER_ERROR.RouteName,
                Routing.URL.APPLIACTION_MODULES.LOADER.BUKA_SEMESTER_ERROR.FILE
            );

            RouteTable.Routes.MapPageRoute(
                "Loader_SD_Nilai_Siswa",
                Routing.URL.APPLIACTION_MODULES.LOADER.SD.NILAI_SISWA.RouteName,
                Routing.URL.APPLIACTION_MODULES.LOADER.SD.NILAI_SISWA.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Loader_SD_BukaSemester_File",
                Routing.URL.APPLIACTION_MODULES.LOADER.SD.BUKA_SEMESTER.RouteName,
                Routing.URL.APPLIACTION_MODULES.LOADER.SD.BUKA_SEMESTER.FILE
            );

            RouteTable.Routes.MapPageRoute(
                "Loader_SMP_BukaSemester_File",
                Routing.URL.APPLIACTION_MODULES.LOADER.SMP.BUKA_SEMESTER.RouteName,
                Routing.URL.APPLIACTION_MODULES.LOADER.SMP.BUKA_SEMESTER.FILE
            );

            RouteTable.Routes.MapPageRoute(
                "Loader_SMA_Nilai_Siswa",
                Routing.URL.APPLIACTION_MODULES.LOADER.SMA.NILAI_SISWA.RouteName,
                Routing.URL.APPLIACTION_MODULES.LOADER.SMA.NILAI_SISWA.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Loader_SMA_BukaSemester_File",
                Routing.URL.APPLIACTION_MODULES.LOADER.SMA.BUKA_SEMESTER.RouteName,
                Routing.URL.APPLIACTION_MODULES.LOADER.SMA.BUKA_SEMESTER.FILE
            );

            RouteTable.Routes.MapPageRoute(
                "Loader_Absen_Siswa",
                Routing.URL.APPLIACTION_MODULES.LOADER.ABSEN_SISWA.RouteName,
                Routing.URL.APPLIACTION_MODULES.LOADER.ABSEN_SISWA.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Loader_Absen_Mapel_Siswa",
                Routing.URL.APPLIACTION_MODULES.LOADER.ABSEN_MAPEL_SISWA.RouteName,
                Routing.URL.APPLIACTION_MODULES.LOADER.ABSEN_MAPEL_SISWA.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Loader_Download_File",
                Routing.URL.APPLIACTION_MODULES.LOADER.ALL_UNIT.DOWNLOAD_FILE.RouteName,
                Routing.URL.APPLIACTION_MODULES.LOADER.ALL_UNIT.DOWNLOAD_FILE.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Loader_Delete_File",
                Routing.URL.APPLIACTION_MODULES.LOADER.ALL_UNIT.DELETE_FILE.RouteName,
                Routing.URL.APPLIACTION_MODULES.LOADER.ALL_UNIT.DELETE_FILE.FILE
            );
            //end loader

            //guru
            RouteTable.Routes.MapPageRoute(
                "SMP_Guru_MateriPembelajaran",
                Routing.URL.APPLIACTION_MODULES.GURU.SMP.MATERI_PEMBELAJARAN.RouteName,
                Routing.URL.APPLIACTION_MODULES.GURU.SMP.MATERI_PEMBELAJARAN.FILE
            );
            //end guru

            //masters
            RouteTable.Routes.MapPageRoute(
                "Master_Divisi",
                Routing.URL.APPLIACTION_MODULES.MASTER.DIVISI.RouteName,
                Routing.URL.APPLIACTION_MODULES.MASTER.DIVISI.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_Siswa",
                Routing.URL.APPLIACTION_MODULES.MASTER.SISWA.RouteName,
                Routing.URL.APPLIACTION_MODULES.MASTER.SISWA.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_Siswa_Input",
                Routing.URL.APPLIACTION_MODULES.MASTER.SISWA_INPUT.RouteName,
                Routing.URL.APPLIACTION_MODULES.MASTER.SISWA_INPUT.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_Pegawai",
                Routing.URL.APPLIACTION_MODULES.MASTER.PEGAWAI.RouteName,
                Routing.URL.APPLIACTION_MODULES.MASTER.PEGAWAI.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_Pegawai_Input",
                Routing.URL.APPLIACTION_MODULES.MASTER.PEGAWAI_INPUT.RouteName,
                Routing.URL.APPLIACTION_MODULES.MASTER.PEGAWAI_INPUT.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_NonSekolah",
                Routing.URL.APPLIACTION_MODULES.MASTER.UNIT_NON_SEKOLAH.RouteName,
                Routing.URL.APPLIACTION_MODULES.MASTER.UNIT_NON_SEKOLAH.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_Sekolah",
                Routing.URL.APPLIACTION_MODULES.MASTER.UNIT_SEKOLAH.RouteName,
                Routing.URL.APPLIACTION_MODULES.MASTER.UNIT_SEKOLAH.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_Kelas",
                Routing.URL.APPLIACTION_MODULES.MASTER.KELAS.RouteName,
                Routing.URL.APPLIACTION_MODULES.MASTER.KELAS.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_Mapel",
                Routing.URL.APPLIACTION_MODULES.MASTER.MAPEL.RouteName,
                Routing.URL.APPLIACTION_MODULES.MASTER.MAPEL.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_RuangKelas",
                Routing.URL.APPLIACTION_MODULES.MASTER.RUANG_KELAS.RouteName,
                Routing.URL.APPLIACTION_MODULES.MASTER.RUANG_KELAS.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_FormasiGuruKelas",
                Routing.URL.APPLIACTION_MODULES.MASTER.FORMASI_GURU_KELAS.RouteName,
                Routing.URL.APPLIACTION_MODULES.MASTER.FORMASI_GURU_KELAS.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_FormasiGuruMapel",
                Routing.URL.APPLIACTION_MODULES.MASTER.FORMASI_GURU_MAPEL.RouteName,
                Routing.URL.APPLIACTION_MODULES.MASTER.FORMASI_GURU_MAPEL.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_FormasiWaliKelas",
                Routing.URL.APPLIACTION_MODULES.MASTER.FORMASI_WALI_KELAS.RouteName,
                Routing.URL.APPLIACTION_MODULES.MASTER.FORMASI_WALI_KELAS.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_UserManagement",
                Routing.URL.APPLIACTION_MODULES.MASTER.USER_MANAGEMENT.RouteName,
                Routing.URL.APPLIACTION_MODULES.MASTER.USER_MANAGEMENT.FILE
            );

            RouteTable.Routes.MapPageRoute(
                "Master_PengaturanSD",
                Routing.URL.APPLIACTION_MODULES.MASTER.PENGATURAN_SD.RouteName,
                Routing.URL.APPLIACTION_MODULES.MASTER.PENGATURAN_SD.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_PengaturanSMP",
                Routing.URL.APPLIACTION_MODULES.MASTER.PENGATURAN_SMP.RouteName,
                Routing.URL.APPLIACTION_MODULES.MASTER.PENGATURAN_SMP.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_PengaturanSMA",
                Routing.URL.APPLIACTION_MODULES.MASTER.PENGATURAN_SMA.RouteName,
                Routing.URL.APPLIACTION_MODULES.MASTER.PENGATURAN_SMA.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_AspekPenilaian_SD",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.ASPEK_PENILAIAN.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.ASPEK_PENILAIAN.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_KompetensiDasar_SD",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.KOMPETENSI_DASAR.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.KOMPETENSI_DASAR.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_KomponenPenilaian_SD",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.KOMPONEN_PENILAIAN.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.KOMPONEN_PENILAIAN.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_StrukturPenilaian_SD",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.STRUKTUR_PENILAIAN.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.STRUKTUR_PENILAIAN.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Desain_Rapor_SD",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.DESAIN_RAPOR.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.DESAIN_RAPOR.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Desain_Rapor_LTS_SD",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.DESAIN_RAPOR_LTS.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.DESAIN_RAPOR_LTS.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Rapor_LTS_SD",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.RAPOR_LTS.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SD.RAPOR_LTS.FILE
            );

            RouteTable.Routes.MapPageRoute(
                "Master_AspekPenilaian_SMP",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.ASPEK_PENILAIAN.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.ASPEK_PENILAIAN.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_KompetensiDasar_SMP",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.KOMPETENSI_DASAR.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.KOMPETENSI_DASAR.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_KomponenPenilaian_SMP",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.KOMPONEN_PENILAIAN.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.KOMPONEN_PENILAIAN.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_StrukturPenilaian_SMP",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.STRUKTUR_PENILAIAN.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.STRUKTUR_PENILAIAN.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_FormasiSiswaEkskul_SMP",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.FORMASI_EKSKUL.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.FORMASI_EKSKUL.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Desain_Rapor_LTS_SMP",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.DESAIN_RAPOR_LTS.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.DESAIN_RAPOR_LTS.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Desain_Rapor_SMP",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.DESAIN_RAPOR.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMP.DESAIN_RAPOR.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_MapelJadwal_SMP",
                Routing.URL.APPLIACTION_MODULES.MASTER.JADWAL_MAPEL.RouteName,
                Routing.URL.APPLIACTION_MODULES.MASTER.JADWAL_MAPEL.FILE
            );

            RouteTable.Routes.MapPageRoute(
                "Master_AspekPenilaian_SMA",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.ASPEK_PENILAIAN.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.ASPEK_PENILAIAN.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_KompetensiDasar_SMA",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.KOMPETENSI_DASAR.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.KOMPETENSI_DASAR.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_KomponenPenilaian_SMA",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.KOMPONEN_PENILAIAN.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.KOMPONEN_PENILAIAN.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_StrukturPenilaian_SMA",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.STRUKTUR_PENILAIAN.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.STRUKTUR_PENILAIAN.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_FormasiSiswaEkskul_SMA",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.FORMASI_EKSKUL.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.FORMASI_EKSKUL.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Desain_Rapor_LTS_SMA",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.DESAIN_RAPOR_LTS.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.DESAIN_RAPOR_LTS.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Desain_Rapor_SMA",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.DESAIN_RAPOR.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.DESAIN_RAPOR.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "ProsesRapor_SMA",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.PROSES_RAPOR.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.PROSES_RAPOR.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "ListNilaiSiswa_SMA",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.LIST_NILAI_SISWA.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.SMA.LIST_NILAI_SISWA.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Master_MapelJadwal_SMA",
                Routing.URL.APPLIACTION_MODULES.MASTER.JADWAL_MAPEL.RouteName,
                Routing.URL.APPLIACTION_MODULES.MASTER.JADWAL_MAPEL.FILE
            );
            //end masters

            //library
            RouteTable.Routes.MapPageRoute(
                "Library_MasterPengaturanKunjunganPerpustakaan",
                Routing.URL.APPLIACTION_MODULES.LIBRARY.PENGATURAN_KUNJUNGAN_PERPUSTAKAAN.RouteName,
                Routing.URL.APPLIACTION_MODULES.LIBRARY.PENGATURAN_KUNJUNGAN_PERPUSTAKAAN.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Library_ListKunjunganPerpustakaan",
                Routing.URL.APPLIACTION_MODULES.LIBRARY.LIST_KUNJUNGAN_PERPUSTAKAAN.RouteName,
                Routing.URL.APPLIACTION_MODULES.LIBRARY.LIST_KUNJUNGAN_PERPUSTAKAAN.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Library_KunjunganPerpustakaan",
                Routing.URL.APPLIACTION_MODULES.LIBRARY.KUNJUNGAN_PERPUSTAKAAN.RouteName,
                Routing.URL.APPLIACTION_MODULES.LIBRARY.KUNJUNGAN_PERPUSTAKAAN.FILE
            );
            RouteTable.Routes.MapPageRoute(
                "Library_PengaturanKunjunganPerpustakaanRutin",
                Routing.URL.APPLIACTION_MODULES.LIBRARY.PENGATURAN_KUNJUNGAN_PERPUSTAKAAN_RUTIN.RouteName,
                Routing.URL.APPLIACTION_MODULES.LIBRARY.PENGATURAN_KUNJUNGAN_PERPUSTAKAAN_RUTIN.FILE
            );
            //end library

            RouteTable.Routes.MapPageRoute(
                "Preview_LTS_And_Rapor",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.ALL.PREVIEW_LTS_DAN_RAPOR.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.ALL.PREVIEW_LTS_DAN_RAPOR.FILE
            );

            RouteTable.Routes.MapPageRoute(
                "Link_Opener",
                Routing.URL.APPLIACTION_MODULES.LINK_OPENER.RouteName,
                Routing.URL.APPLIACTION_MODULES.LINK_OPENER.FILE
            );

            RouteTable.Routes.MapPageRoute(
                "Penilaian_All_File_Rapor",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.ALL.FILE_RAPOR.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.ALL.FILE_RAPOR.FILE
            );

            RouteTable.Routes.MapPageRoute(
                "Penilaian_All_File_Rapor_View",
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.ALL.FILE_RAPOR_VIEW.RouteName,
                Routing.URL.APPLIACTION_MODULES.EDUCATION.Penilaian.ALL.FILE_RAPOR_VIEW.FILE
            );

            RouteTable.Routes.MapPageRoute(
               "CBT_Mapel",
               Routing.URL.APPLIACTION_MODULES.CBT.MAPEL.RouteName,
               Routing.URL.APPLIACTION_MODULES.CBT.MAPEL.FILE
           );

            RouteTable.Routes.MapPageRoute(
               "CBT_BankSoal",
               Routing.URL.APPLIACTION_MODULES.CBT.SOAL.RouteName,
               Routing.URL.APPLIACTION_MODULES.CBT.SOAL.FILE
           );
            RouteTable.Routes.MapPageRoute(
              "CBT_BankSoal_INPUT",
              Routing.URL.APPLIACTION_MODULES.CBT.SOAL_INPUT.RouteName,
              Routing.URL.APPLIACTION_MODULES.CBT.SOAL_INPUT.FILE
          );
            RouteTable.Routes.MapPageRoute(
              "CBT_RUMAH_SOAL_SMP",
              Routing.URL.APPLIACTION_MODULES.CBT.RUMAH_SOAL_SMP.RouteName,
              Routing.URL.APPLIACTION_MODULES.CBT.RUMAH_SOAL_SMP.FILE
          );
            RouteTable.Routes.MapPageRoute(
              "CBT_RUMAH_SOAL_SMA",
              Routing.URL.APPLIACTION_MODULES.CBT.RUMAH_SOAL_SMA.RouteName,
              Routing.URL.APPLIACTION_MODULES.CBT.RUMAH_SOAL_SMA.FILE
          );
            RouteTable.Routes.MapPageRoute(
             "CBT_RUMAH_SOAL_INPUT",
             Routing.URL.APPLIACTION_MODULES.CBT.RUMAH_SOAL_INPUT.RouteName,
             Routing.URL.APPLIACTION_MODULES.CBT.RUMAH_SOAL_INPUT.FILE
         );
            RouteTable.Routes.MapPageRoute(
             "CBT_RUMAH_DESIGN_SOAL",
             Routing.URL.APPLIACTION_MODULES.CBT.DESIGN_SOAL.RouteName,
             Routing.URL.APPLIACTION_MODULES.CBT.DESIGN_SOAL.FILE
         );
            RouteTable.Routes.MapPageRoute(
             "CBT_SOAL_VIEW",
             Routing.URL.APPLIACTION_MODULES.CBT.SOAL_VIEW.RouteName,
             Routing.URL.APPLIACTION_MODULES.CBT.SOAL_VIEW.FILE
         );

            RouteTable.Routes.MapHubs();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}