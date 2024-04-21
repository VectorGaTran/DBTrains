using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace BazaDeDateFeroviara
{
    public class Authentification
    {
        private string connectionString;

        public Authentification(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool AuthenticatePasager(string nume, string prenume)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Pasageri WHERE Nume = @Nume AND Prenume = @Prenume";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nume", nume);
                    command.Parameters.AddWithValue("@Prenume", prenume);

                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
        }
    }
}