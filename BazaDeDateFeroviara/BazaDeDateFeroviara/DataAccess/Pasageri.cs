using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace BazaDeDateFeroviara.DataAccess
{
    public class Pasageri
    {
        private string connectionString;
        private SqlConnection connection;


        public Pasageri(string connectionString)
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


        public DataTable GetAllPasageri()
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Pasageri";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dataTable);
                }
            }

            return dataTable;
        }

        public void InsertPasager(string nume, string prenume, string telefon, string email, string tip)
        {
            OpenConnection();

            if (telefon == null && email == null)
            {
                throw new ArgumentException("Trebuie dat ori un număr de telefon ori un email.");
            }

            string query = "INSERT INTO Pasageri (Nume, Prenume, Numar_telefon, Email, Tip) " +
                           "VALUES (@Nume, @Prenume, @Numar_telefon, @Email, @Tip)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Nume", nume);
                command.Parameters.AddWithValue("@Prenume", prenume);
                command.Parameters.AddWithValue("@Numar_telefon", (object)telefon ?? DBNull.Value); 
                command.Parameters.AddWithValue("@Email", (object)email ?? DBNull.Value);
                command.Parameters.AddWithValue("@Tip", (object)tip ?? DBNull.Value);

                command.ExecuteNonQuery();
            }

            CloseConnection();
        }

        public void UpdatePasager(int recordID, string newNume, string newPrenume, string newNumar_telefon, string newEmail, string tip)
        {
            OpenConnection();

            string query = "UPDATE Pasageri SET " +
                           "Nume = ISNULL(@NewNume, Nume), " +
                           "Prenume = ISNULL(@NewPrenume, Prenume), " +
                           "Numar_telefon = ISNULL(@NewNumar_telefon, Numar_telefon), " +
                           "Email = ISNULL(@NewEmail, Email), " +
                           "Tip = ISNULL(@Tip, Tip) " +
                           "WHERE Pasager_ID = @RecordID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RecordID", recordID);
                command.Parameters.AddWithValue("@NewNume", string.IsNullOrEmpty(newNume) ? DBNull.Value : (object)newNume);
                command.Parameters.AddWithValue("@NewPrenume", string.IsNullOrEmpty(newPrenume) ? DBNull.Value : (object)newPrenume);
                command.Parameters.AddWithValue("@NewNumar_telefon", string.IsNullOrEmpty(newNumar_telefon) ? DBNull.Value : (object)newNumar_telefon);
                command.Parameters.AddWithValue("@NewEmail", string.IsNullOrEmpty(newEmail) ? DBNull.Value : (object)newEmail);
                command.Parameters.AddWithValue("@Tip", string.IsNullOrEmpty(tip) ? DBNull.Value : (object)tip);

                command.ExecuteNonQuery();
            }

            CloseConnection();
        }


        public void DeletePasager(int id)
        {
            OpenConnection();

            string query = "DELETE FROM Pasageri WHERE Pasager_ID = @ID ";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ID", id);
                
                command.ExecuteNonQuery();
            }

            CloseConnection();
        }

        public string[] GetPasagerDataByID(int pasagerID)
        {
            string query = "SELECT Nume, Prenume, Numar_telefon, Email, Tip FROM Pasageri WHERE Pasager_ID = @PasagerID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PasagerID", pasagerID);
                OpenConnection();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string[] data = new string[reader.FieldCount];
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            data[i] = reader[i].ToString();
                        }
                        return data;
                    }
                    else
                    {
                        throw new InvalidOperationException($"Pasager with ID {pasagerID} not found.");
                    }
                }
            }
        }
        // Other methods related to the Pasageri table
    }
}