using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPKALE_Cryptography
{
    class CTF
    {
        public void showResult()
        {
            
            crypto c = new crypto();
            DataTable dt_results = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
            string query = "select * from tbl_CTF_Directory";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            
            da.Fill(dt_results);
            conn.Close();
            da.Dispose();

            DataTable dt_final = new DataTable();
            dt_final.Columns.AddRange(new DataColumn[1] { new DataColumn("vote", typeof(string)) });
            for (int i = 0; i < dt_results.Rows.Count; i++)
            {
                dt_final.Rows.Add(c.Decrypt(dt_results.Rows[i]["vote"].ToString(), dt_results.Rows[i]["ssn"].ToString(), true));
            }

           

           DataTable dt_cadidate = new DataTable();
             connString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
             query = "select * from tbl_candidate_master";

             conn = new SqlConnection(connString);
             cmd = new SqlCommand(query, conn);
            conn.Open();

            
             da = new SqlDataAdapter(cmd);
            
            da.Fill(dt_cadidate);
            conn.Close();
            da.Dispose();


            DataTable dt_display = new DataTable();
            dt_display.Columns.AddRange(new DataColumn[2] { new DataColumn("Name", typeof(string)),
                            new DataColumn("vote",typeof(string)) });
            int cad1 = 0;
            int cad2 = 0;
            for (int i = 0; i < dt_final.Rows.Count; i++)
            {

                if (Convert.ToInt32(dt_final.Rows[i]["vote"].ToString()) == 1)
                    cad1++;
                else if (Convert.ToInt32(dt_final.Rows[i]["vote"].ToString()) == 2)
                    cad2++;

            }
            dt_display.Rows.Add(dt_cadidate.Rows[0]["c_name"].ToString(), cad1);
            dt_display.Rows.Add(dt_cadidate.Rows[1]["c_name"].ToString(), cad2);

            for (int i = 0; i < dt_display.Rows.Count; i++)
            {
                Console.WriteLine( dt_display.Rows[i]["Name"].ToString() + " : " + dt_display.Rows[i]["vote"].ToString());
            }

        }
    }
}
