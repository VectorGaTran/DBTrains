using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace BazaDeDateFeroviara.DataAccess
{
    public class Trenuri
    {
        private string connectionString;
        private SqlConnection connection;

        public Trenuri(string connectionString)
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
        public DataTable GetAllTrenuri()
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Trenuri";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dataTable);
                }
            }

            return dataTable;

        }

        public void InsertTren(string nume, string tip, short capacitate)
        {
            try
            {
                OpenConnection();

                string query = "INSERT INTO Trenuri (Nume, Tip, Capacitate) VALUES (@Nume, @Tip, @Capacitate)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nume", nume);
                    command.Parameters.AddWithValue("@Tip", tip);
                    command.Parameters.AddWithValue("@Capacitate", capacitate);

                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    throw new InvalidOperationException("Un tren cu numele/indicatorul ăsta există deja în baza de date.", ex);
                }
                else
                {
                    throw new InvalidOperationException($"SQL Error: {ex.Message}", ex);
                }
            }
            finally
            {
                CloseConnection();
            }
        }

        public void DeleteTren(int id)
        {
            OpenConnection();

            string query = "DELETE FROM Trenuri WHERE Tren_ID = @ID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ID", id);

                command.ExecuteNonQuery();
            }

            CloseConnection();
        }

        // Other methods related to the Trenuri table
    }
}
