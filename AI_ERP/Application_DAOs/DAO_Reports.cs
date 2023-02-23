using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Libs;
using AI_ERP.Application_Entities;

namespace AI_ERP.Application_DAOs
{
    public static class DAO_Reports
    {
        public static class AbsensiSiswa
        {
            public const string SP_SELECT_ABSEN ="SiswaAbsen_SELECT_ABSEN";
            
            public static class NamaField
            {
                public const string TahunAjaran ="TahunAjaran";
                public const string Semester ="Semester";
                public const string Rel_Unit ="Rel_Unit";
                public const string Unit ="Unit";
                public const string Rel_Guru ="Rel_Guru";
                public const string Guru ="Guru";
                public const string Rel_Mapel ="Rel_Mapel";
                public const string Mapel ="Mapel";
                public const string Rel_KelasDet = "Rel_KelasDet";
                public const string KelasDet ="KelasDet";
                public const string NIS ="NIS";
                public const string Rel_Siswa ="Rel_Siswa";
                public const string Nama ="Nama";
                public const string Tanggal ="Tanggal";
                public const string UrutanJenjang ="UrutanJenjang";
                public const string UrutanLevel ="UrutanLevel";
                public const string UrutanKelas ="UrutanKelas";
                public const string Kehadiran ="Kehadiran";
                public const string Sakit = "Sakit";
                public const string Izin = "Izin";
                public const string Alpa = "Alpa";
                public const string Is_Cat01 ="Is_Cat01";
                public const string Is_Cat02 ="Is_Cat02";
                public const string Is_Cat03 ="Is_Cat03";
                public const string Is_Cat04 ="Is_Cat04";
                public const string Is_Cat05 ="Is_Cat05";
                public const string Is_Cat06 ="Is_Cat06";
                public const string Is_Cat07 ="Is_Cat07";
                public const string Is_Cat08 ="Is_Cat08";
                public const string Is_Cat09 ="Is_Cat09";
                public const string Is_Cat10 ="Is_Cat10";
                public const string Is_Sakit_Keterangan ="Is_Sakit_Keterangan";
                public const string Is_Izin_Keterangan ="Is_Izin_Keterangan";
                public const string Is_Alpa_Keterangan ="Is_Alpa_Keterangan";
                public const string Is_Cat01_Keterangan ="Is_Cat01_Keterangan";
                public const string Is_Cat02_Keterangan ="Is_Cat02_Keterangan";
                public const string Is_Cat03_Keterangan ="Is_Cat03_Keterangan";
                public const string Is_Cat04_Keterangan ="Is_Cat04_Keterangan";
                public const string Is_Cat05_Keterangan ="Is_Cat05_Keterangan";
                public const string Is_Cat06_Keterangan ="Is_Cat06_Keterangan";
                public const string Is_Cat07_Keterangan ="Is_Cat07_Keterangan";
                public const string Is_Cat08_Keterangan ="Is_Cat08_Keterangan";
                public const string Is_Cat09_Keterangan ="Is_Cat09_Keterangan";
                public const string Is_Cat10_Keterangan ="Is_Cat10_Keterangan";
            }

            public static string GetSQLVW_Absen(
                DateTime dari_tanggal, DateTime sampai_tanggal, string rel_unit, string rel_kelasdet, string rel_mapel, string rel_guru)
            {
                return  
                        "SELECT x.* FROM " +
                        "( " +
                        "        SELECT  " +
                        "                b.TahunAjaran, " +
                        "                a.Semester, " +
                        "                CONVERT(varchar(50), c.Kode) AS Rel_Unit, " +
                        "                c.Nama AS Unit, " +
                        "                CONVERT(varchar(50), g.Kode) AS Rel_Guru, " +
                        "                g.Nama AS Guru, " +
                        "                CONVERT(varchar(50), h.Kode) AS Rel_Mapel, " +
                        "                h.Nama AS Mapel, " +
                        "                CONVERT(varchar(50), d.Kode) AS Rel_KelasDet, " +
                        "                d.Nama AS KelasDet, " +
                        "                e.NIS, " +
                        "                CONVERT(varchar(50), e.Kode) AS Rel_Siswa, " +
                        "                UPPER(e.Nama) AS Nama, " +
                        "                a.Tanggal, " +
                        "                c.UrutanJenjang, " +
                        "                f.UrutanLevel, " +
                        "                d.UrutanKelas, " +
                        "                CASE  " +
                        "			        WHEN LEFT(ISNULL(a.Is_Hadir, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Hadir, ''))) <> '' THEN '100' " +
                        "	                ELSE '0' " +
                        "                END AS Kehadiran, " +
                        "                CASE  " +
                        "			        WHEN LEFT(ISNULL(a.Is_Sakit, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Sakit, ''))) <> '' THEN '100' " +
                        "	                ELSE '0' " +
                        "                END AS Sakit, " +
                        "                CASE  " +
                        "	                WHEN LEFT(ISNULL(a.Is_Izin, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Izin, ''))) <> '' THEN '100' " +
                        "	                ELSE '0' " +
                        "                END AS Izin, " +
                        "                CASE  " +
                        "	                WHEN LEFT(ISNULL(a.Is_Alpa, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Alpa, ''))) <> '' THEN '100' " +
                        "	                ELSE '0' " +
                        "                END AS Alpa, " +
                        "                CASE  " +
                        "	                WHEN LEFT(ISNULL(a.Is_Cat01, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Cat01, ''))) <> '' THEN '100' " +
                        "	                ELSE '0' " +
                        "                END AS Is_Cat01, " +
                        "                CASE  " +
                        "	                WHEN LEFT(ISNULL(a.Is_Cat02, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Cat02, ''))) <> '' THEN '100' " +
                        "	                ELSE '0' " +
                        "                END AS Is_Cat02, " +
                        "                CASE  " +
                        "	                WHEN LEFT(ISNULL(a.Is_Cat03, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Cat03, ''))) <> '' THEN '100' " +

                        "	                ELSE '0' " +
                        "                END AS Is_Cat03, " +
                        "                CASE  " +
                        "	                WHEN LEFT(ISNULL(a.Is_Cat04, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Cat04, ''))) <> '' THEN '100' " +
                        "	                ELSE '0' " +
                        "                END AS Is_Cat04, " +
                        "                CASE  " +
                        "	                WHEN LEFT(ISNULL(a.Is_Cat05, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Cat05, ''))) <> '' THEN '100' " +
                        "	                ELSE '0' " +
                        "                END AS Is_Cat05, " +
                        "                CASE  " +
                        "	                WHEN LEFT(ISNULL(a.Is_Cat06, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Cat06, ''))) <> '' THEN '100' " +
                        "	                ELSE '0' " +
                        "                END AS Is_Cat06, " +
                        "                CASE  " + 

                        "	                WHEN LEFT(ISNULL(a.Is_Cat07, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Cat07, ''))) <> '' THEN '100' " +
                        "	                ELSE '0' " +
                        "                END AS Is_Cat07, " +
                        "                CASE  " +
                        "	                WHEN LEFT(ISNULL(a.Is_Cat08, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Cat08, ''))) <> '' THEN '100' " +
                        "	                ELSE '0' " +
                        "                END AS Is_Cat08, " +
                        "                CASE  " +
                        "	                WHEN LEFT(ISNULL(a.Is_Cat09, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Cat09, ''))) <> '' THEN '100' " +
                        "	                ELSE '0' " +
                        "                END AS Is_Cat09, " +
                        "                CASE  " +
                        "	                WHEN LEFT(ISNULL(a.Is_Cat10, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Cat10, ''))) <> '' THEN '100'  " +
                        "	                ELSE '0'  " +
                        "                END AS Is_Cat10,  " +

                        "                ISNULL(a.Is_Sakit_Keterangan, '') AS Is_Sakit_Keterangan,  " +
                        "                ISNULL(a.Is_Izin_Keterangan, '') AS Is_Izin_Keterangan,  " +
                        "                ISNULL(a.Is_Alpa_Keterangan, '') AS Is_Alpa_Keterangan,  " +

                        "                ISNULL(a.Is_Cat01_Keterangan, '') AS Is_Cat01_Keterangan,  " +
                        "                ISNULL(a.Is_Cat02_Keterangan, '') AS Is_Cat02_Keterangan,  " +
                        "                ISNULL(a.Is_Cat03_Keterangan, '') AS Is_Cat03_Keterangan,  " +
                        "                ISNULL(a.Is_Cat04_Keterangan, '') AS Is_Cat04_Keterangan,  " +
                        "                ISNULL(a.Is_Cat05_Keterangan, '') AS Is_Cat05_Keterangan,  " +
                        "                ISNULL(a.Is_Cat06_Keterangan, '') AS Is_Cat06_Keterangan,  " +
                        "                ISNULL(a.Is_Cat07_Keterangan, '') AS Is_Cat07_Keterangan,  " +
                        "                ISNULL(a.Is_Cat08_Keterangan, '') AS Is_Cat08_Keterangan,  " +
                        "                ISNULL(a.Is_Cat09_Keterangan, '') AS Is_Cat09_Keterangan,  " +
                        "                ISNULL(a.Is_Cat10_Keterangan, '') AS Is_Cat10_Keterangan  " +
                            "        FROM SiswaAbsenMapel a   " +
                            "        LEFT JOIN LinimasaKelas b ON CONVERT(varchar(50), b.Kode) = CONVERT(varchar(50), a.Rel_Linimasa)  " +
                            "        LEFT JOIN Sekolah c ON CONVERT(varchar(50), c.Kode) = CONVERT(varchar(50), a.Rel_Sekolah)  " +
                            "        LEFT JOIN KelasDet d ON CONVERT(varchar(50), d.Kode) = CONVERT(varchar(50), a.Rel_KelasDet)  " +
                            "        LEFT JOIN Siswa e ON CONVERT(varchar(50), e.Kode) = CONVERT(varchar(50), a.Rel_Siswa)  " +
                            "        LEFT JOIN Kelas f ON CONVERT(varchar(50), f.Kode) = CONVERT(varchar(50), d.Rel_Kelas)  " +
                            "        LEFT JOIN Pegawai g ON CONVERT(varchar(50), g.Kode) = CONVERT(varchar(50), a.Rel_Guru)  " +
                            "        LEFT JOIN Mapel h ON CONVERT(varchar(50), h.Kode) = CONVERT(varchar(50), a.Rel_Mapel)  " +
                            "        WHERE ISNULL(b.TahunAjaran, '') <> '' AND  " +
                        "	                RTRIM(LTRIM(ISNULL(a.Absen, ''))) = '' AND  " +
                        "	                (  " +
                        "	                	a.Tanggal >= '" + dari_tanggal.ToString("yyyy-MM-dd") + "' AND  " +
                        "	                	a.Tanggal < DATEADD(DAY, 1, '" + sampai_tanggal.ToString("yyyy-MM-dd") + "')  " +
                        "	                )  " +
                                            (
                                                rel_unit.Trim() != ""
                                                ? "AND c.Kode = '" + rel_unit.Replace("'", "''") + "'"
                                                : ""
                                            ) +
                                            (
                                                rel_kelasdet.Trim() != ""
                                                ? "AND a.Rel_KelasDet = '" + rel_kelasdet.Replace("'", "''") + "'"
                                                : ""
                                            ) +
                                            (
                                                rel_mapel.Trim() != ""
                                                ? "AND a.Rel_Mapel = '" + rel_mapel.Replace("'", "''") + "'"
                                                : ""
                                            ) +
                                            (
                                                rel_guru.Trim() != ""
                                                ? "AND a.Rel_Guru = '" + rel_guru.Replace("'", "''") + "'"
                                                : ""
                                            ) +

                            "        UNION  " +

                        "            SELECT   " +
                        "                b.TahunAjaran,  " +
                        "                a.Semester,  " +
                        "                CONVERT(varchar(50), c.Kode) AS Rel_Unit, " +
                        "                c.Nama AS Unit,  " +
                        "                CONVERT(varchar(50), g.Kode) AS Rel_Guru,  " +
                        "                g.Nama AS Guru,  " +
                        "                '' AS Rel_Mapel,  " +
                        "                '' AS Mapel,  " +
                        "                CONVERT(varchar(50), d.Kode) AS Rel_KelasDet,  " +
                        "                d.Nama AS KelasDet,  " +
                        "                e.NIS,  " +
                        "                CONVERT(varchar(50), e.Kode) AS Rel_Siswa,  " +
                        "                UPPER(e.Nama) AS Nama,  " +
                        "                a.Tanggal,  " +
                        "                c.UrutanJenjang,  " +
                        "                f.UrutanLevel,  " +
                        "                d.UrutanKelas,  " +
                        "                CASE   " +
                        "	                WHEN LEFT(ISNULL(a.Is_Hadir, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Hadir, ''))) <> '' THEN '100'  " +
                        "	                ELSE '0'  " +
                        "                END AS Kehadiran,  " +
                        "                CASE   " +
                        "	                WHEN LEFT(ISNULL(a.Is_Sakit, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Sakit, ''))) <> '' THEN '100'  " +
                        "	                ELSE '0'  " +
                        "                END AS Sakit,  " +
                        "                CASE   " +
                        "	                WHEN LEFT(ISNULL(a.Is_Izin, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Izin, ''))) <> '' THEN '100'  " +
                        "	                ELSE '0'  " +
                        "                END AS Izin,  " +
                        "                CASE   " +
                        "	                WHEN LEFT(ISNULL(a.Is_Alpa, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Alpa, ''))) <> '' THEN '100'  " +
                        "	                ELSE '0'  " +
                        "                END AS Alpa,  " +
                        "                CASE   " +
                        "	                WHEN LEFT(ISNULL(a.Is_Cat01, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Cat01, ''))) <> '' THEN '100'  " +
                        "	                ELSE '0'  " +
                        "                END AS Is_Cat01,  " +
                        "                CASE   " +
                        "	                WHEN LEFT(ISNULL(a.Is_Cat02, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Cat02, ''))) <> '' THEN '100'  " +
                        "	                ELSE '0'  " +
                        "                END AS Is_Cat02,  " +
                        "                CASE   " +
                        "	                WHEN LEFT(ISNULL(a.Is_Cat03, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Cat03, ''))) <> '' THEN '100'  " +
                        "	                ELSE '0'  " +
                        "                END AS Is_Cat03,  " +
                        "                CASE   " +
                        "	                WHEN LEFT(ISNULL(a.Is_Cat04, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Cat04, ''))) <> '' THEN '100'  " +
                        "	                ELSE '0'  " +
                        "                END AS Is_Cat04,  " +
                        "                CASE   " +
                        "	                WHEN LEFT(ISNULL(a.Is_Cat05, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Cat05, ''))) <> '' THEN '100'  " +
                        "	                ELSE '0'  " +
                        "                END AS Is_Cat05,  " +
                        "                CASE   " +
                        "	                WHEN LEFT(ISNULL(a.Is_Cat06, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Cat06, ''))) <> '' THEN '100'  " +
                        "	                ELSE '0'  " +
                        "                END AS Is_Cat06,  " +
                        "                CASE   " +
                        "	                WHEN LEFT(ISNULL(a.Is_Cat07, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Cat07, ''))) <> '' THEN '100'  " +
                        "	                ELSE '0'  " +
                        "                END AS Is_Cat07,  " +
                        "                CASE   " +
                        "	                WHEN LEFT(ISNULL(a.Is_Cat08, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Cat08, ''))) <> '' THEN '100'  " +
                        "	                ELSE '0'  " +
                        "                END AS Is_Cat08,  " +
                        "                CASE   " +
                        "	                WHEN LEFT(ISNULL(a.Is_Cat09, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Cat09, ''))) <> '' THEN '100'  " +
                        "	                ELSE '0'  " +
                        "                END AS Is_Cat09,  " +
                        "                CASE   " +
                        "	                WHEN LEFT(ISNULL(a.Is_Cat10, ''), 2) <> '__' AND LTRIM(RTRIM(ISNULL(a.Is_Cat10, ''))) <> '' THEN '100'  " +
                        "	                ELSE '0'  " +
                        "                END AS Is_Cat10,  " +

                        "                ISNULL(a.Is_Sakit_Keterangan, '') AS Is_Sakit_Keterangan,  " +
                        "                ISNULL(a.Is_Izin_Keterangan, '') AS Is_Izin_Keterangan,  " +
                        "                ISNULL(a.Is_Alpa_Keterangan, '') AS Is_Alpa_Keterangan,  " +

                        "                ISNULL(a.Is_Cat01_Keterangan, '') AS Is_Cat01_Keterangan,  " +
                        "                ISNULL(a.Is_Cat02_Keterangan, '') AS Is_Cat02_Keterangan,  " +
                        "                ISNULL(a.Is_Cat03_Keterangan, '') AS Is_Cat03_Keterangan,  " +
                        "                ISNULL(a.Is_Cat04_Keterangan, '') AS Is_Cat04_Keterangan,  " +
                        "                ISNULL(a.Is_Cat05_Keterangan, '') AS Is_Cat05_Keterangan,  " +
                        "                ISNULL(a.Is_Cat06_Keterangan, '') AS Is_Cat06_Keterangan,  " +
                        "                ISNULL(a.Is_Cat07_Keterangan, '') AS Is_Cat07_Keterangan,  " +
                        "                ISNULL(a.Is_Cat08_Keterangan, '') AS Is_Cat08_Keterangan,  " +
                        "                ISNULL(a.Is_Cat09_Keterangan, '') AS Is_Cat09_Keterangan,  " +
                        "                ISNULL(a.Is_Cat10_Keterangan, '') AS Is_Cat10_Keterangan  " +
                            "        FROM SiswaAbsen a   " +
                            "        LEFT JOIN LinimasaKelas b ON CONVERT(varchar(50), b.Kode) = CONVERT(varchar(50), a.Rel_Linimasa)  " +
                            "        LEFT JOIN Sekolah c ON CONVERT(varchar(50), c.Kode) = CONVERT(varchar(50), a.Rel_Sekolah)  " +
                            "        LEFT JOIN KelasDet d ON CONVERT(varchar(50), d.Kode) = CONVERT(varchar(50), a.Rel_KelasDet)  " +
                            "        LEFT JOIN Siswa e ON CONVERT(varchar(50), e.Kode) = CONVERT(varchar(50), a.Rel_Siswa)  " +
                            "        LEFT JOIN Kelas f ON CONVERT(varchar(50), f.Kode) = CONVERT(varchar(50), d.Rel_Kelas)  " +
                            "        LEFT JOIN Pegawai g ON CONVERT(varchar(50), g.Kode) = CONVERT(varchar(50), a.Rel_Guru)  " +
                            "        WHERE ISNULL(b.TahunAjaran, '') <> '' AND  " +
                        "	                RTRIM(LTRIM(ISNULL(a.Absen, ''))) = '' AND  " +
                        "	                (  " +
                        "	                	a.Tanggal >= '" + dari_tanggal.ToString("yyyy-MM-dd") + "' AND  " +
                        "	                	a.Tanggal < DATEADD(DAY, 1, '" + sampai_tanggal.ToString("yyyy-MM-dd") + "')  " +
                        "	                )  " +
                                            (
                                                rel_unit.Trim() != ""
                                                ? "AND c.Kode = '" + rel_unit.Replace("'", "''") + "'"
                                                : ""
                                            ) +
                                            (
                                                rel_kelasdet.Trim() != ""
                                                ? "AND a.Rel_KelasDet = '" + rel_kelasdet.Replace("'", "''") + "'"
                                                : ""
                                            ) +
                                            (
                                                rel_guru.Trim() != ""
                                                ? "AND a.Rel_Guru = '" + rel_guru.Replace("'", "''") + "'"
                                                : ""
                                            ) +
                                ") x  " +

                                "ORDER BY x.TahunAjaran,  " +
                        "         x.Semester,  " +
                        "         x.UrutanJenjang,  " +
                        "         x.UrutanLevel,  " +
                        "         x.UrutanKelas,  " +
                        "         x.Tanggal,  " +
                        "         x.Nama;";
            }

            private static Reports.AbsensiSiswa GetEntityFromDataRow(DataRow row)
            {
                return new Reports.AbsensiSiswa
                {
                    TahunAjaran = row[NamaField.TahunAjaran].ToString(),
                    Semester = row[NamaField.Semester].ToString(),
                    Rel_Unit = row[NamaField.Rel_Unit].ToString(),
                    Unit = row[NamaField.Unit].ToString(),
                    Rel_Guru = row[NamaField.Rel_Guru].ToString(),
                    Guru = row[NamaField.Guru].ToString(),
                    Rel_Mapel = row[NamaField.Rel_Mapel].ToString(),
                    Mapel = row[NamaField.Mapel].ToString(),
                    Rel_KelasDet = row[NamaField.Rel_KelasDet].ToString(),
                    KelasDet = row[NamaField.KelasDet].ToString(),
                    NIS = row[NamaField.NIS].ToString(),
                    Rel_Siswa = row[NamaField.Rel_Siswa].ToString(),
                    Nama = row[NamaField.Nama].ToString(),
                    Tanggal = Convert.ToDateTime(row[NamaField.Tanggal]),
                    UrutanJenjang = Convert.ToInt16(row[NamaField.UrutanJenjang]),
                    UrutanLevel = Convert.ToInt16(row[NamaField.UrutanLevel]),
                    UrutanKelas = Convert.ToInt16(row[NamaField.UrutanKelas]),
                    Kehadiran = row[NamaField.Kehadiran].ToString(),
                    Sakit = row[NamaField.Sakit].ToString(),
                    Izin = row[NamaField.Izin].ToString(),
                    Alpa = row[NamaField.Alpa].ToString(),
                    Is_Cat01 = row[NamaField.Is_Cat01].ToString(),
                    Is_Cat02 = row[NamaField.Is_Cat02].ToString(),
                    Is_Cat03 = row[NamaField.Is_Cat03].ToString(),
                    Is_Cat04 = row[NamaField.Is_Cat04].ToString(),
                    Is_Cat05 = row[NamaField.Is_Cat05].ToString(),
                    Is_Cat06 = row[NamaField.Is_Cat06].ToString(),
                    Is_Cat07 = row[NamaField.Is_Cat07].ToString(),
                    Is_Cat08 = row[NamaField.Is_Cat08].ToString(),
                    Is_Cat09 = row[NamaField.Is_Cat09].ToString(),
                    Is_Cat10 = row[NamaField.Is_Cat10].ToString(),
                    Is_Sakit_Keterangan = row[NamaField.Is_Sakit_Keterangan].ToString(),
                    Is_Izin_Keterangan = row[NamaField.Is_Izin_Keterangan].ToString(),
                    Is_Alpa_Keterangan = row[NamaField.Is_Alpa_Keterangan].ToString(),
                    Is_Cat01_Keterangan = row[NamaField.Is_Cat01_Keterangan].ToString(),
                    Is_Cat02_Keterangan = row[NamaField.Is_Cat02_Keterangan].ToString(),
                    Is_Cat03_Keterangan = row[NamaField.Is_Cat03_Keterangan].ToString(),
                    Is_Cat04_Keterangan = row[NamaField.Is_Cat04_Keterangan].ToString(),
                    Is_Cat05_Keterangan = row[NamaField.Is_Cat05_Keterangan].ToString(),
                    Is_Cat06_Keterangan = row[NamaField.Is_Cat06_Keterangan].ToString(),
                    Is_Cat07_Keterangan = row[NamaField.Is_Cat07_Keterangan].ToString(),
                    Is_Cat08_Keterangan = row[NamaField.Is_Cat08_Keterangan].ToString(),
                    Is_Cat09_Keterangan = row[NamaField.Is_Cat09_Keterangan].ToString(),
                    Is_Cat10_Keterangan = row[NamaField.Is_Cat10_Keterangan].ToString()
                };
            }

            public static List<Reports.AbsensiSiswa> GetAbsenSiswa(
                DateTime dari_tanggal, DateTime sampai_tanggal, string rel_unit, string rel_kelasdet, string rel_mapel, string rel_guru)
            {
                List<Reports.AbsensiSiswa> hasil = new List<Reports.AbsensiSiswa>();
                SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
                SqlCommand comm = conn.CreateCommand();
                SqlDataAdapter sqlDA;

                try
                {
                    conn.Open();
                    comm.CommandTimeout = 12000;
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = GetSQLVW_Absen(dari_tanggal, sampai_tanggal, rel_unit, rel_kelasdet, rel_mapel, rel_guru);

                    DataTable dtResult = new DataTable();
                    sqlDA = new SqlDataAdapter(comm);
                    sqlDA.Fill(dtResult);
                    foreach (DataRow row in dtResult.Rows)
                    {
                        hasil.Add(GetEntityFromDataRow(row));
                    }
                }
                catch (Exception ec)
                {
                    throw new Exception(ec.Message.ToString());
                }
                finally
                {
                    conn.Close();
                }

                return hasil;
            }
        }
    }
}