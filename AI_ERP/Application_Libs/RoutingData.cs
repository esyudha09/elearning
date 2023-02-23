using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AI_ERP.Application_DAOs;
using AI_ERP.Application_Entities;

namespace AI_ERP.Application_Libs
{
    public class ERoutingData
    {
        public string TahunAjaran { set; get; }
        public string Semester { set; get; }
        public string Unit { set; get; }
        public string Level { set; get; }
        public string Kelas { set; get; }
        public string SubKelas { set; get; }
    }

    public static class RoutingData
    {
        public static ERoutingData GetRoutingByKelasDet(string rel_kelasdet)
        {
            ERoutingData m = new ERoutingData();
            string s_unit = "";
            string s_level = "";
            string s_kelas = "";
            string s_subkelas = "";

            KelasDet m_kelas_det = DAO_KelasDet.GetByID_Entity(rel_kelasdet);
            if (m_kelas_det != null)
            {
                if (m_kelas_det.Nama != null)
                {
                    //kelas
                    if (m_kelas_det.Nama.Trim().ToLower().IndexOf("Kepompong Merah Senin".ToLower()) >= 0) s_kelas = "1";
                    if (m_kelas_det.Nama.Trim().ToLower().IndexOf("Kepompong Kuning Selasa".ToLower()) >= 0) s_kelas = "2";

                    if (m_kelas_det.Nama.Trim().ToLower().IndexOf("Kupu-Kupu Biru Senin".ToLower()) >= 0) s_kelas = "1";
                    if (m_kelas_det.Nama.Trim().ToLower().IndexOf("Kupu-Kupu Biru Selasa".ToLower()) >= 0) s_kelas = "2";
                    if (m_kelas_det.Nama.Trim().ToLower().IndexOf("Kupu-Kupu Kuning Senin".ToLower()) >= 0) s_kelas = "3";
                    if (m_kelas_det.Nama.Trim().ToLower().IndexOf("Kupu-Kupu Kuning Selasa".ToLower()) >= 0) s_kelas = "4";
                    if (m_kelas_det.Nama.Trim().ToLower().IndexOf("Kupu-Kupu Merah Senin".ToLower()) >= 0) s_kelas = "5";
                    if (m_kelas_det.Nama.Trim().ToLower().IndexOf("Kupu-Kupu Merah Selasa".ToLower()) >= 0) s_kelas = "6";

                    if (m_kelas_det.Nama.Trim().ToLower().IndexOf("-1") >= 0) s_kelas = "1";
                    if (m_kelas_det.Nama.Trim().ToLower().IndexOf("-2") >= 0) s_kelas = "2";
                    if (m_kelas_det.Nama.Trim().ToLower().IndexOf("-3") >= 0) s_kelas = "3";
                    if (m_kelas_det.Nama.Trim().ToLower().IndexOf("-4") >= 0) s_kelas = "4";
                    if (m_kelas_det.Nama.Trim().ToLower().IndexOf("-5") >= 0) s_kelas = "5";

                    if (m_kelas_det.Nama.Trim().ToLower().IndexOf("-a") >= 0) s_kelas = "A";
                    if (m_kelas_det.Nama.Trim().ToLower().IndexOf("-b") >= 0) s_kelas = "B";
                    if (m_kelas_det.Nama.Trim().ToLower().IndexOf("-c") >= 0) s_kelas = "C";
                    if (m_kelas_det.Nama.Trim().ToLower().IndexOf("-d") >= 0) s_kelas = "D";
                    if (m_kelas_det.Nama.Trim().ToLower().IndexOf("-e") >= 0) s_kelas = "E";

                    if (m_kelas_det.Nama.Trim().ToLower().IndexOf("-mipa") >= 0)
                    {
                        s_kelas = "A";
                        s_subkelas = m_kelas_det.Nama.Trim().Substring(m_kelas_det.Nama.Trim().Length - 1);
                    }
                    if (m_kelas_det.Nama.Trim().ToLower().IndexOf("-ips") >= 0)
                    {
                        s_kelas = "S";
                        s_subkelas = m_kelas_det.Nama.Trim().Substring(m_kelas_det.Nama.Trim().Length - 1);
                    }
                    if (m_kelas_det.Nama.Trim().ToLower().IndexOf("-ibb") >= 0)
                    {
                        s_kelas = "B";
                        s_subkelas = m_kelas_det.Nama.Trim().Substring(m_kelas_det.Nama.Trim().Length - 1);
                    }
                    //end kelas

                    //level
                    Kelas m_kelas = DAO_Kelas.GetByID_Entity(m_kelas_det.Rel_Kelas.ToString());
                    if (m_kelas != null)
                    {
                        if (m_kelas.Nama != null)
                        {
                            switch (m_kelas.UrutanLevel)
                            {
                                case 1: //kepompong
                                    s_level = "KG";
                                    break;
                                case 2: //kupu-kupu
                                    s_level = "KK";
                                    break;
                                case 3: //TK-A
                                    s_level = "AA";
                                    break;
                                case 4: //TK-B
                                    s_level = "BB";
                                    break;
                                case 5: //Kelas I
                                    s_level = "01";
                                    break;
                                case 6: //Kelas II
                                    s_level = "02";
                                    break;
                                case 7: //Kelas III
                                    s_level = "03";
                                    break;
                                case 8: //Kelas IV
                                    s_level = "04";
                                    break;
                                case 9: //Kelas V
                                    s_level = "05";
                                    break;
                                case 10: //Kelas VI
                                    s_level = "06";
                                    break;
                                case 11: //Kelas VII
                                    s_level = "07";
                                    break;
                                case 12: //Kelas VIII
                                    s_level = "08";
                                    break;
                                case 13: //Kelas IX
                                    s_level = "09";
                                    break;
                                case 14: //Kelas X
                                    s_level = "10";
                                    break;
                                case 15: //Kelas XI
                                    s_level = "11";
                                    break;
                                case 16: //Kelas XII
                                    s_level = "12";
                                    break;
                                default:
                                    break;
                            }

                            //unit
                            Sekolah m_sekolah = DAO_Sekolah.GetByID_Entity(m_kelas.Rel_Sekolah.ToString());
                            if (m_sekolah != null)
                            {
                                if (m_sekolah.Nama != null)
                                {
                                    switch (m_sekolah.UrutanJenjang)
                                    {
                                        case 1:
                                            s_unit = "B";
                                            break;
                                        case 2:
                                            s_unit = "K";
                                            break;
                                        case 3:
                                            s_unit = "D";
                                            break;
                                        case 4:
                                            s_unit = "P";
                                            break;
                                        case 5:
                                            s_unit = "A";
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            //end unit
                        }
                    }
                }
            }

            m.Unit = s_unit;
            m.Level = s_level;
            m.Kelas = s_kelas;
            m.SubKelas = s_subkelas;

            return m;
        }
    }
}