using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using AI_ERP.Application_Entities;

namespace AI_ERP.Application_DAOs
{
    public static class _DAO_MutasiKantin
    {
        public const string SP_ELEARNING_SELECT_MUTASI_KANTIN = "ELEARNING_SELECT_MUTASI_KANTIN";

        public static class NamaField
        {
            public const string NoKasir = "NoKasir";
            public const string Tanggal = "Tanggal";
            public const string SaldoAwal = "SaldoAwal";
            public const string Total = "Total";
            public const string SaldoAkhir = "SaldoAkhir";
            public const string Keterangan = "Keterangan";
        }

        private static _MutasiKantin GetEntityFromDataRow(DataRow row)
        {
            return new _MutasiKantin
            {
                NoKasir = row[NamaField.NoKasir].ToString(),
                Tanggal = Convert.ToDateTime(row[NamaField.Tanggal]),
                SaldoAwal = Convert.ToDecimal(row[NamaField.SaldoAwal]),
                Total = Convert.ToDecimal(row[NamaField.Total]),
                SaldoAkhir = Convert.ToDecimal(row[NamaField.SaldoAkhir]),
                Keterangan = row[NamaField.Keterangan].ToString()
            };
        }

        public static List<_MutasiKantin> GetByNIS_Entity(string nis)
        {
            List<_MutasiKantin> hasil = new List<_MutasiKantin>();
            SqlConnection conn = Application_Libs.Libs.GetConnection_ERP();
            SqlCommand comm = conn.CreateCommand();
            SqlDataAdapter sqlDA;

            try
            {
                conn.Open();
                comm.CommandTimeout = 1200;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = SP_ELEARNING_SELECT_MUTASI_KANTIN;
                comm.Parameters.AddWithValue("@NIS", nis);

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