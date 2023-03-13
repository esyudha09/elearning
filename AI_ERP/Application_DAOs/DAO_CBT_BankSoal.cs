using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities;
using AI_ERP.APIs.Master;

namespace AI_ERP.Application_DAOs
{
    public static class DAO_CBT_BankSoal
    {
        public const string SP_SELECT_ALL_BY_MAPEL_BY_KELAS = "CBT_BANK_SOAL_SELECT_ALL_BY_MAPEL_BY_KELAS";
        public const string SP_SELECT_ALL_BY_MAPEL_BY_KELAS_FOR_SEARCH = "CBT_BANK_SOAL_SELECT_ALL_BY_MAPEL__BY_KELAS_FOR_SEARCH";
        public const string SP_SELECT_BY_ID = "CBT_BANK_SOAL_SELECT_BY_ID";
        public const string SP_SELECT_JWB_GANDA_BY_HEADER = "CBT_BANK_SOAL_JWB_GANDA_SELECT_BY_HEADER";

        public const string SP_SELECT_BY_GURU_BY_TA = "Mapel_SELECT_BY_GURU_BY_TA";
        public const string SP_SELECT_BY_GURU_BY_TA_FOR_SEARCH = "Mapel_SELECT_BY_GURU_BY_TA_FOR_SEARCH";
        public const string SP_SELECT_MAPEL_SISWA_BY_ID = "CBT_MAPEL_SISWA_SELECT_BY_ID";

        //public const string SP_SELECT_ALL = "CBT_BankSoal_SELECT_ALL";       
        //public const string SP_SELECT_ALL_FOR_SEARCH = "CBT_BankSoal_SELECT_ALL_FOR_SEARCH";
        //public const string SP_SELECT_ALL_BY_UNIT_FOR_SEARCH = "CBT_BankSoal_SELECT_ALL_BY_UNIT_FOR_SEARCH";
        //public const string SP_SELECT_BY_SEKOLAH = "CBT_BankSoal_SELECT_BY_SEKOLAH";
        //public const string SP_SELECT_BY_STRUKTUR_NILAI = "CBT_BankSoal_SELECT_BY_STRUKTUR_NILAI_SD";
        //public const string SP_SELECT_BY_GURU = "CBT_BankSoal_SELECT_BY_GURU";
        //public const string SP_SELECT_BY_GURU_BY_TA = "CBT_BankSoal_SELECT_BY_GURU_BY_TA";
        //public const string SP_SELECT_BY_GURU_BY_TA_FOR_SEARCH = "CBT_BankSoal_SELECT_BY_GURU_BY_TA_FOR_SEARCH";        
        //public const string SP_SELECT_DISTINCT_ABSEN_BY_TA_BY_UNIT_BY_SEMESTER = "CBT_BankSoal_SELECT_DISTINCT_ABSEN_BY_TA_BY_UNIT_BY_SEMESTER";

        public const string SP_INSERT = "CBT_BANK_SOAL_INSERT";

        public const string SP_JWB_GANDA_INSERT = "CBT_BANK_SOAL_JWB_GANDA_INSERT";      
        public const string SP_JWB_GANDA_UPDATE = "CBT_BANK_SOAL_JWB_GANDA_UPDATE";

        public const string SP_UPDATE = "CBT_BANK_SOAL_UPDATE";

        public const string SP_DELETE = "CBT_BANK_SOAL_DELETE";
        public const string SP_DELETE_JWB_GANDA = "CBT_BANK_SOAL_JWB_GANDA_DELETE";

        public static class NamaField
        {
            public const string Kode = "Kode";
            public const string Rel_Mapel = "Rel_Mapel";
            public const string Rel_Kelas = "Rel_Kelas";
            public const string Rel_Guru = "Rel_Guru";
            public const string Nama = "Nama";
            public const string Rel_Rapor_AspekPenilaian = "Rel_Rapor_AspekPenilaian";
            public const string Soal = "Soal";
            public const string Jenis = "Jenis";
            public const string JwbEssay = "JwbEssay";
            public const string Rel_JwbGanda = "Rel_JwbGanda";
            public const string FileImage = "FileImage";
            public const string FileAudio = "FileAudio";
            public const string FileVideo = "FileVideo";


            public const string Rel_BankSoal = "Rel_BankSoal";
            public const string Jawaban = "Jawaban";
            public const string Urut = "Urut";

        }

        public static CBT_BankSoal GetEntityFromDataRow(DataRow row)
        {
            return new CBT_BankSoal
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Rel_Mapel = row[NamaField.Rel_Mapel].ToString(),
                Rel_Guru = row[NamaField.Rel_Guru].ToString(),
                Nama = row[NamaField.Nama].ToString(),
                Rel_Rapor_AspekPenilaian = row[NamaField.Rel_Rapor_AspekPenilaian].ToString(),
                Soal = row[NamaField.Soal].ToString(),
                Jenis = row[NamaField.Jenis].ToString(),
                JwbEssay = row[NamaField.JwbEssay].ToString(),
                Rel_JwbGanda = row[NamaField.Rel_JwbGanda].ToString(),
                FileImage = row[NamaField.FileImage].ToString(),
                FileAudio = row[NamaField.FileAudio].ToString(),
                FileVideo = row[NamaField.FileVideo].ToString()
            };
        }

        public static CBT_BankSoalJawabGanda GetEntityFromDataRow2(DataRow row)
        {
            return new CBT_BankSoalJawabGanda
            {
                Kode = new Guid(row[NamaField.Kode].ToString()),
                Jawaban = row[NamaField.Jawaban].ToString()
            };
        }



        public static CBT_BankSoal GetByID_Entity(string kode)
        {
            CBT_BankSoal hasil = new CBT_BankSoal();

            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            if (kode == null) return hasil;
            if (kode.Length <= 10) return hasil;
            try
            {
                conn.Open();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_SELECT_BY_ID;
                comm.Parameters.AddWithValue("@" + NamaField.Kode, kode);

                DataTable dtResult = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult);
                foreach (DataRow row in dtResult.Rows)
                {
                    hasil = GetEntityFromDataRow(row);
                }

                comm.Parameters.Clear();
                comm.CommandText = SP_SELECT_JWB_GANDA_BY_HEADER;
                comm.Parameters.AddWithValue("@" + NamaField.Rel_BankSoal, kode);
                DataTable dtResult2 = new DataTable();
                sqlDA = new SqlDataAdapter(comm);
                sqlDA.Fill(dtResult2);

                List<CBT_BankSoalJawabGanda> list_JwbGanda = new List<CBT_BankSoalJawabGanda>();
                foreach (DataRow row in dtResult2.Rows)
                {
                    CBT_BankSoalJawabGanda j = new CBT_BankSoalJawabGanda();
                    j = GetEntityFromDataRow2(row);
                    list_JwbGanda.Add(j);
                }

                hasil.ListJwbGanda = list_JwbGanda;
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

        public static void Delete(string Kode, string user_id)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_DELETE;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, Kode));
                comm.Parameters.Add(new SqlParameter("@user_id", user_id));
                comm.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (Exception ec)
            {
                transaction.Rollback();
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public static void Insert(CBT_BankSoal BankSoal, string user_id)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_INSERT;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, BankSoal.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Mapel, BankSoal.Rel_Mapel));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Kelas, BankSoal.Rel_Kelas));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Guru, BankSoal.Rel_Guru));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, BankSoal.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_AspekPenilaian, BankSoal.Rel_Rapor_AspekPenilaian));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Soal, BankSoal.Soal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Jenis, BankSoal.Jenis));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JwbEssay, BankSoal.JwbEssay));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_JwbGanda, BankSoal.Rel_JwbGanda));
                comm.Parameters.Add(new SqlParameter("@user_id", user_id));

                comm.CommandText = SP_INSERT;
                comm.ExecuteNonQuery();



                foreach (CBT_BankSoalJawabGanda b in BankSoal.ListJwbGanda)
                {
                    comm.Parameters.Clear();
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, b.Kode));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_BankSoal, BankSoal.Kode));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Jawaban, b.Jawaban));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Urut, b.Urut));
                    comm.Parameters.Add(new SqlParameter("@user_id", user_id));
                    comm.CommandText = SP_JWB_GANDA_INSERT;
                    comm.ExecuteNonQuery();
                }



                transaction.Commit();



            }
            catch (Exception ec)
            {
                transaction.Rollback();
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public static void Update(CBT_BankSoal BankSoal, string user_id)
        {
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlTransaction transaction = null;
            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_UPDATE;

                comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, BankSoal.Kode));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Nama, BankSoal.Nama));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_Rapor_AspekPenilaian, BankSoal.Rel_Rapor_AspekPenilaian));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Soal, BankSoal.Soal));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Jenis, BankSoal.Jenis));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.JwbEssay, BankSoal.JwbEssay));
                comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_JwbGanda, BankSoal.Rel_JwbGanda));
                comm.Parameters.Add(new SqlParameter("@user_id", user_id));
                comm.ExecuteNonQuery();


                //comm.Parameters.Clear();
                //comm.Parameters.Add(new SqlParameter("@" + NamaField.Rel_BankSoal, BankSoal.Kode));
                //comm.CommandText = SP_DELETE_JWB_GANDA;
                //comm.ExecuteNonQuery();

                foreach (CBT_BankSoalJawabGanda b in BankSoal.ListJwbGanda)
                {
                    comm.Parameters.Clear();
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Kode, b.Kode));                 
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Jawaban, b.Jawaban));
                    comm.Parameters.Add(new SqlParameter("@" + NamaField.Urut, b.Urut));
                    comm.Parameters.Add(new SqlParameter("@user_id", user_id));
                    comm.CommandText = SP_JWB_GANDA_UPDATE;
                    comm.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch (Exception ec)
            {
                transaction.Rollback();
                throw new Exception(ec.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }




    }
}