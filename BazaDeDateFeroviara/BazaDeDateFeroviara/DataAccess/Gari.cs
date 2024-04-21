using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace BazaDeDateFeroviara.DataAccess
{
    public class Gari
    {
        private string connectionString;
        private SqlConnection connection;

        public Gari(string connectionString)
        {
            this.connectionString = connectionString;
            this.connection = new SqlConnection(connectionString);

        }
        public void OpenConnection()
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
        public DataTable GetAllGari()
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Gari";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dataTable);
                }
            }

            return dataTable;
        }
        public int GetGaraIdByName(string garaName)
        {
            OpenConnection();

            try
            {
                string query = "SELECT Gara_ID FROM Gari WHERE Nume = @GaraName";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GaraName", garaName);

                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return (int)result;
                    }
                }

                return -1;
            }
            finally
            {
                CloseConnection();
            }
        }

        // Other methods related to the Gari table
    }
}

