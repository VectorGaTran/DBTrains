using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace BazaDeDateFeroviara.DataAccess
{
    public class Angajati
    {
        private string connectionString;
        private SqlConnection connection;

        public Angajati(string connectionString)
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
        public DataTable GetAllAngajati()
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Angajati";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dataTable);
                }
            }

            return dataTable;
        }

        public void InsertAngajat(string nume, string prenume, string cnp, string email, string numar_telefon, string functie, short salariu)
        {
            OpenConnection();

            try
            {

                string query = "INSERT INTO Angajati (Nume, Prenume, CNP, Email, Numar_telefon, Functie, Salariu) " +
                               "VALUES (@Nume, @Prenume, @CNP, @Email, @Numar_telefon, @Functie, @Salariu)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nume", nume);
                    command.Parameters.AddWithValue("@Prenume", prenume);
                    command.Parameters.AddWithValue("@CNP", cnp);
                    command.Parameters.AddWithValue("@Email", (object)email ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Numar_telefon", (object)numar_telefon ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Functie", functie);
                    command.Parameters.AddWithValue("@Salariu", salariu);

                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    throw new InvalidOperationException("Un angajat cu acest CNP există deja în baza de date.", ex);
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

        public void UpdateAngajat(int recordID, string newNume, string newPrenume, string newCnp, string newEmail, string newNumar_telefon, string newFunctie, short? newSalariu)
        {
            OpenConnection();

            try
            {
                string query = "UPDATE Angajati SET " +
                               "Nume = ISNULL(@NewNume, Nume), " +
                               "Prenume = ISNULL(@NewPrenume, Prenume), " +
                               "CNP = ISNULL(@NewCnp, CNP), " +
                               "Email = ISNULL(@NewEmail, Email), " +
                               "Numar_telefon = ISNULL(@NewNumar_telefon, Numar_telefon), " +
                               "Functie = @NewFunctie, " +
                               "Salariu = @NewSalariu " +
                               "WHERE Angajat_ID = @RecordID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RecordID", recordID);

                    command.Parameters.AddWithValue("@NewNume", string.IsNullOrEmpty(newNume) ? DBNull.Value : (object)newNume);
                    command.Parameters.AddWithValue("@NewPrenume", string.IsNullOrEmpty(newPrenume) ? DBNull.Value : (object)newPrenume);
                    command.Parameters.AddWithValue("@NewCnp", string.IsNullOrEmpty(newCnp) ? DBNull.Value : (object)newCnp);
                    command.Parameters.AddWithValue("@NewEmail", string.IsNullOrEmpty(newEmail) ? DBNull.Value : (object)newEmail);
                    command.Parameters.AddWithValue("@NewNumar_telefon", string.IsNullOrEmpty(newNumar_telefon) ? DBNull.Value : (object)newNumar_telefon);
                    command.Parameters.AddWithValue("@NewFunctie", string.IsNullOrEmpty(newFunctie) ? DBNull.Value : (object)newFunctie);
                    command.Parameters.AddWithValue("@NewSalariu", newSalariu.HasValue ? (object)newSalariu.Value : DBNull.Value);

                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    throw new InvalidOperationException("Un angajat cu acest CNP există deja în baza de date.", ex);
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


        public void DeleteAngajat(int id)
        {
            OpenConnection();


            string query = "DELETE FROM Angajati WHERE Angajat_ID = @ID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ID", id);

                command.ExecuteNonQuery();
            }


            CloseConnection();
        }
        public string[] GetAngajatDataByID(int angajatID)
        {
            string query = "SELECT Nume, Prenume, CNP, Email, Numar_telefon, Functie, Salariu FROM Angajati WHERE Angajat_ID = @AngajatID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@AngajatID", angajatID);
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
                        throw new InvalidOperationException($"Angajat with ID {angajatID} not found.");
                    }
                }
            }
        }

    }


    // Other methods related to the Angajati table
}
