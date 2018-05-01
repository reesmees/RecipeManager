using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksamenM2E2017.DbAccess
{
    public abstract class CommonDB
    {
        public readonly string connectionString;

        public CommonDB(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool ExecuteNonQuery(string query)
        {
            bool success = true;
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand com = new SqlCommand(query, con))
            {
                con.Open();
                com.ExecuteNonQuery();
            }
            return success;
        }

        public DataSet ExecuteQuery(string query, CommandType type = CommandType.Text)
        {
            DataSet data = new DataSet();
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand com = new SqlCommand(query, con) { CommandType = type })
            {
                con.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(com);
                adapter.Fill(data);
            }
            return data;
        }
    }
}
