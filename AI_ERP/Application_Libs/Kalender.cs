using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AI_ERP.Application_Libs
{
    public static class Kalender
    {
        public static string[] Arr_Warna = new string[] { "#8C36B4", "#01B5B5", "#0000FF", "#E0992F", "#FF0000" };
        public static string[] Arr_WarnaHeader = new string[] { "#446D8C", "#00C400", "#8C36B4", "#01B5B5", "#DD0076", "#E0992F", "#FF0000" };

        public static string GetHTMLKalender(int bulan, int tahun)
        {
            DateTime tgl_awal = new DateTime(tahun, bulan, 1);
            DateTime tgl_akhir = new DateTime(tahun, bulan, 1);
            DateTime tgl_akhir_blnsebelumnya = tgl_awal.AddDays(-1);
            DayOfWeek hari_pertama = tgl_awal.DayOfWeek;

            tgl_akhir = tgl_akhir.AddMonths(1);
            tgl_akhir = tgl_akhir.AddDays(-1);

            string css_header = " style=\"padding: 5px; font-weight: bold; border-style: solid; border-width: 1px; border-color: #e4dfdf; border-bottom-color: @bordercolor; border-bottom-width: 5px; text-align: center; background-color: white; font-weight: normal; padding: 10px; font-size: x-small; color: grey; padding-left: 3px; padding-right: 3px; border-left-style: none; border-right-style: none; border-top-style: none;\" ";
            string css_header_libur = " style=\"padding: 5px; font-weight: bold; color: red; border-style: solid; border-width: 1px; border-color: #e4dfdf; border-bottom-color: @bordercolor; border-bottom-width: 5px; text-align: center; background-color: white; font-weight: normal; padding: 10px; font-size: x-small; padding-left: 3px; padding-right: 3px; border-left-style: none; border-right-style: none; border-top-style: none;\" ";
            string html_kalender = "<table style=\"margin: 0px; width: 100%;\">" +
                                    "<tr>" +
                                        "<td " + css_header.Replace("@bordercolor", Arr_WarnaHeader[0]) + ">Senin</td>" +
                                        "<td " + css_header.Replace("@bordercolor", Arr_WarnaHeader[1]) + ">Selasa</td>" +
                                        "<td " + css_header.Replace("@bordercolor", Arr_WarnaHeader[2]) + ">Rabu</td>" +
                                        "<td " + css_header.Replace("@bordercolor", Arr_WarnaHeader[3]) + ">Kamis</td>" +
                                        "<td " + css_header.Replace("@bordercolor", Arr_WarnaHeader[4]) + ">Jumat</td>" +
                                        "<td " + css_header_libur.Replace("@bordercolor", Arr_WarnaHeader[5]) + ">Sabtu</td>" +
                                        "<td " + css_header_libur.Replace("@bordercolor", Arr_WarnaHeader[6]) + ">Minggu</td>" +
                                    "</tr>";

            string css_tanggal = " style=\"padding: 5px; font-weight: bold; border-style: solid; border-width: 1px; border-color: #e4dfdf; text-align: center; background-color: white; font-weight: normal; padding: 10px; font-size: small; color: grey; border-left-style: none; border-right-style: none; border-top-style: none;\" ";
            string css_tanggal_libur = " style=\"padding: 5px; font-weight: bold; color: red; border-style: solid; border-width: 1px; border-color: #e4dfdf; text-align: center; background-color: white; font-weight: normal; padding: 10px; font-size: small; border-left-style: none; border-right-style: none; border-top-style: none;\" ";
            string css_tanggal_noncurrent = " style=\"background-color: #F8F8F8; padding: 5px; font-weight: bold; border-style: solid; border-width: 1px; border-color: #e4dfdf; color: #bfbfbf; text-align: center; background-color: white; font-weight: normal; padding: 10px; font-size: small; border-left-style: none; border-right-style: none; border-top-style: none;\" ";
            string css_tanggal_hr_ini = " style=\"background-color: #F8F8F8; font-weight: bold; color: green; text-align: center; background-color: white; padding: 0px; font-size: small; border-color: green; border-width: 3px; border-style: solid; background-color: green; color: white; border-left-style: none; border-right-style: none; border-top-style: none;\" ";
            string html_eventcal = "";
            int jml_row = 1;
            
            for (int i = 1; i <= tgl_akhir.Day; i++)
            {
                DateTime tanggal = new DateTime(tahun, bulan, i);
                if ((int)hari_pertama >= 1)
                {
                    if ((i + (int)hari_pertama - 2) % 7 == 0 && i != 1) { html_kalender += "</tr>"; jml_row++; }
                    if ((i + (int)hari_pertama - 2) % 7 == 0 || i == 1) html_kalender += "<tr>";
                }
                else if ((int)hari_pertama == 0)
                {
                    if ((i + 5) % 7 == 0 && i != 1) { html_kalender += "</tr>"; jml_row++; }
                    if ((i + 5) % 7 == 0 || i == 1) html_kalender += "<tr>";
                }
                if (hari_pertama != DayOfWeek.Monday && hari_pertama != DayOfWeek.Sunday && i == 1)
                {
                    for (int j = (tgl_akhir_blnsebelumnya.Day - ((int)hari_pertama - 2)); j <= tgl_akhir_blnsebelumnya.Day; j++)
                    {
                        html_kalender += "<td " + css_tanggal_noncurrent + ">" + j.ToString() + "</td>";
                    }
                }
                else if (hari_pertama == DayOfWeek.Sunday && i == 1)
                {
                    for (int j = (tgl_akhir_blnsebelumnya.Day - 5); j <= tgl_akhir_blnsebelumnya.Day; j++)
                    {
                        html_kalender += "<td " + css_tanggal_noncurrent + ">" + j.ToString() + "</td>";
                    }
                }
                string tgl = i.ToString();
                if (tanggal.Date == DateTime.Now.Date)
                {
                    html_kalender += "<td title=\" Hari Ini \" " + css_tanggal_hr_ini + ">" + tgl + "<br /><span style=\"font-size: x-small; font-weight: normal\">HARI INI</span>" + "</td>";
                }
                else
                {
                    html_kalender += "<td " + (tanggal.DayOfWeek == DayOfWeek.Saturday || tanggal.DayOfWeek == DayOfWeek.Sunday ? css_tanggal_libur : css_tanggal) + ">" + tgl + "</td>";
                }                
            }
            int jml_cell = jml_row * 7;
            int sisa_tanggal = jml_cell - ((int)hari_pertama - 1);
            if (hari_pertama > 0)
            {
                sisa_tanggal = jml_cell - ((int)hari_pertama - 1);
            }
            else if (hari_pertama == 0)
            {
                sisa_tanggal = jml_cell - 6;
            }
            if (tgl_akhir.Day < sisa_tanggal)
            {
                for (int i = 1; i <= (sisa_tanggal - tgl_akhir.Day); i++)
                {
                    html_eventcal = "";
                    html_kalender += "<td " + css_tanggal_noncurrent + ">" + i.ToString() + html_eventcal + "</td>";
                }
            }

            html_kalender += "</table>";

            return html_kalender;
        }
    }
}