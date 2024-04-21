using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using BazaDeDateFeroviara.DataAccess;

namespace BazaDeDateFeroviara.Forms
{
    public partial class MainForm : Form
    {
        private DataGridView dataGridView1;
        private ComboBox classComboBox;
        private Button loadButton;
        private Button refreshButton;
        private Button insertButton;
        private Button updateButton;
        private Button deleteButton;
        private Button showReservationsButton; 
        private Button backButton;
        private ComboBox queryComboBox;
        private Button executeQueryButton;
        private ComboBox startStationComboBox;
        private ComboBox endStationComboBox;
        private Button traseuButton;

        private int selectedId;

        private string connectionString;

        public MainForm(string connectionString)
        {
            this.connectionString = connectionString;

            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.Font = new Font("Times New Roman", 12F);

            dataGridView1 = new DataGridView
            {
                Size = new Size(800, 400),
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter },
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter },
            };
            dataGridView1.CellClick += dataGridView1_CellClick;
            Controls.Add(dataGridView1);

            classComboBox = new ComboBox
            {
                Size = new Size(200, 30),
                Location = new Point((this.ClientSize.Width - 200) / 2, dataGridView1.Bottom + 10), 
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            classComboBox.Items.AddRange(new string[] { "Pasageri", "Trenuri", "Trenuri_Angajati", "Angajati", "Rute", "Gari", "Rezervari" });
            Controls.Add(classComboBox);

            loadButton = new Button
            {
                Size = new Size(100, 30),
                Text = "Incarca tabel",
                Location = new Point(classComboBox.Right + 10, classComboBox.Top),
            };
            loadButton.Click += new EventHandler((sender, e) => btnLoadData_Click(sender, e, connectionString));
            Controls.Add(loadButton);

            refreshButton = new Button
            {
                Size = new Size(100, 30),
                Text = "Refresh",
                Location = new Point(loadButton.Right + 10, classComboBox.Top),
            };
            refreshButton.Click += new EventHandler((sender, e) => btnRefreshData_Click(sender, e, connectionString));
            Controls.Add(refreshButton);

            insertButton = new Button
            {
                Size = new Size(100, 30),
                Text = "Inserează",
                Location = new Point(refreshButton.Right + 10, classComboBox.Top),
                Visible = false,
            };
            insertButton.Click += new EventHandler((sender, e) => btnInsert_Click(sender, e, connectionString));
            Controls.Add(insertButton);

            updateButton = new Button
            {
                Size = new Size(100, 30),
                Text = "Actualizează",
                Location = new Point(insertButton.Right + 10, classComboBox.Top),
                Visible = false,
            };
            updateButton.Click += new EventHandler((sender, e) => btnUpdate_Click(sender, e, connectionString));
            Controls.Add(updateButton);

            deleteButton = new Button
            {
                Size = new Size(100, 30),
                Text = "Șterge",
                Location = new Point(updateButton.Right + 10, classComboBox.Top),
                Visible = false,
            };
            deleteButton.Click += new EventHandler((sender, e) => btnDelete_Click(sender, e, connectionString));
            Controls.Add(deleteButton);

            showReservationsButton = new Button
            {
                Size = new Size(120, 30),
                Text = "Rezervări (pentru pasagerul selectat)",
                Location = new Point(deleteButton.Right + 10, classComboBox.Top),
                Visible = false,
            };
            showReservationsButton.Click += ShowReservationsButton_Click;
            Controls.Add(showReservationsButton);

            backButton = new Button
            {
                Size = new Size(120, 30),
                Text = "Înapoi",
                Location = new Point(deleteButton.Right + 10, classComboBox.Top),
                Visible = false,
            };
            backButton.Click += BackButton_Click;
            Controls.Add(backButton);

            classComboBox.SelectedIndexChanged += classComboBox_SelectedIndexChanged;

            queryComboBox = new ComboBox
            {
                Size = new Size(300, 30),
                Location = new Point((this.ClientSize.Width - 300) / 2, classComboBox.Bottom + 10),
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            queryComboBox.Items.AddRange(new string[]
            {
            "Informații despre pasageri, rezervări și trenuri",
            "Capetele traseelor trenurilor",
            "Gări și ora plecării fiecărui tren",
            "Angajați pe trenuri",
            "Venit pentru fiecare tren",
            "Informații despre trenul cu cele mai multe rezervări",
            "Numărul de locuri libere în fiecare tren",
            "Nr. angajați de la gara cea mai frecventată"
            });
            Controls.Add(queryComboBox);

            executeQueryButton = new Button
            {
                Size = new Size(150, 30),
                Text = "Încarcă diverse informații",
                Location = new Point(queryComboBox.Right + 10, queryComboBox.Top),
            };
            executeQueryButton.Click += ExecuteQueryButton_Click;
            Controls.Add(executeQueryButton);

            startStationComboBox = new ComboBox
            {
                Size = new Size(150, 30),
                Location = new Point((this.ClientSize.Width - 300) / 2, queryComboBox.Bottom + 10),
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            PopulateStations(startStationComboBox);
            Controls.Add(startStationComboBox);

            endStationComboBox = new ComboBox
            {
                Size = new Size(150, 30),
                Location = new Point(startStationComboBox.Right + 10, queryComboBox.Bottom + 10),
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            PopulateStations(endStationComboBox);
            Controls.Add(endStationComboBox);

            traseuButton = new Button
            {
                Size = new Size(100, 30),
                Text = "Venit traseu",
                Location = new Point(endStationComboBox.Right + 10, queryComboBox.Bottom + 10),
            };
            traseuButton.Click += TraseuButton_Click;
            Controls.Add(traseuButton);
        }

        private void classComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedClassName = classComboBox.SelectedItem as string;

            if (selectedClassName != null)
            {
                UpdateButtonVisibility(selectedClassName);
            }
        }

        private void UpdateButtonVisibility(string className)
        {
            insertButton.Visible = className == "Angajati" || className == "Pasageri" || className == "Trenuri"; ;
            updateButton.Visible = className == "Angajati" || className == "Pasageri";
            deleteButton.Visible = className == "Angajati" || className == "Pasageri" || className == "Trenuri";
            showReservationsButton.Visible = className == "Pasageri" ;
            backButton.Visible = false;

        }

        private void btnLoadData_Click(object sender, EventArgs e, string connectionString)
        {
            try
            {
                string className = classComboBox.SelectedItem as string;

                if (className != null)
                {
                    DataTable classData = GetDataByClassName(className, connectionString);

                    dataGridView1.DataSource = classData;

                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        if (column.HeaderText.EndsWith("ID"))
                        {
                            column.Visible = false;
                        }
                    }

                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void btnRefreshData_Click(object sender, EventArgs e, string connectionString)
        {
            try
            {
                string className = classComboBox.SelectedItem as string;

                if (className != null)
                {
                    DataTable classData = GetDataByClassName(className, connectionString);

                    dataGridView1.DataSource = classData;

                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        if (column.HeaderText.EndsWith("ID"))
                        {
                            column.Visible = false;
                        }
                    }

                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnInsert_Click(object sender, EventArgs e, string connectionString)
        {
            string className = classComboBox.SelectedItem as string;
            InsertForm insertForm = new InsertForm(className, connectionString);
            insertForm.Show();
        }

        private void btnUpdate_Click(object sender, EventArgs e, string connectionString)
        {
            try
            {
                if (selectedId > 0)
                {
                    string className = classComboBox.SelectedItem as string;
                    UpdateForm updateForm = new UpdateForm(className, connectionString, this);
                    updateForm.SetRecordID(selectedId);
                    updateForm.Show();
                }
                else
                {
                    MessageBox.Show("Selectați o înregistrare pentru a actualiza.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la încărcarea datelor pentru actualizare: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e, string connectionString)
        {
            try
            {
                if (selectedId > 0)
                {
                    string className = classComboBox.SelectedItem as string;

                    if (className != null)
                    {
                        switch (className)
                        {
                            case "Angajati":
                                Angajati angajati = new Angajati(connectionString);
                                angajati.DeleteAngajat(selectedId);
                                break;
                            case "Pasageri":
                                Pasageri pasageri = new Pasageri(connectionString);
                                pasageri.DeletePasager(selectedId); 
                                break;
                            case "Trenuri":
                                Trenuri trenuri = new Trenuri(connectionString);
                                trenuri.DeleteTren(selectedId);
                                break;
                            // Add cases for other classes as needed
                            default:
                                break;
                        }

                        MessageBox.Show($"Din tabelul {className} s-a șters cu succes.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnRefreshData_Click(sender, e, connectionString); 
                    }
                }
                else
                {
                    MessageBox.Show("Selectați o înregistrare pentru a șterge.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la ștergerea datelor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable GetDataByClassName(string className, string connectionString)
        {
            switch (className)
            {
                case "Pasageri":
                    Pasageri pasageri = new Pasageri(connectionString);
                    pasageri.OpenConnection();
                    DataTable pasageriData = pasageri.GetAllPasageri();
                    pasageri.CloseConnection();
                    return pasageriData;

                case "Trenuri":
                    Trenuri trenuri = new Trenuri(connectionString);
                    trenuri.OpenConnection();
                    DataTable trenuriData = trenuri.GetAllTrenuri();
                    trenuri.CloseConnection();
                    return trenuriData;

                case "Angajati":
                    Angajati angajati = new Angajati(connectionString);
                    angajati.OpenConnection();
                    DataTable angajatiData = angajati.GetAllAngajati();
                    angajati.CloseConnection();
                    return angajatiData;

                case "Trenuri_Angajati":
                    Trenuri_Angajati trenuriAngajati = new Trenuri_Angajati(connectionString);
                    trenuriAngajati.OpenConnection();
                    DataTable trenuriAngajatiData = trenuriAngajati.GetAllTrenuriAngajati();
                    trenuriAngajati.CloseConnection();
                    return trenuriAngajatiData;

                case "Rute":
                    Rute rute = new Rute(connectionString);
                    rute.OpenConnection();
                    DataTable ruteData = rute.GetAllRute();
                    rute.CloseConnection();
                    return ruteData;

                case "Gari":
                    Gari gari = new Gari(connectionString);
                    gari.OpenConnection();
                    DataTable gariData = gari.GetAllGari();
                    gari.CloseConnection();
                    return gariData;

                case "Rezervari":
                    Rezervari rezervari = new Rezervari(connectionString);
                    rezervari.OpenConnection();
                    DataTable rezervariData = rezervari.GetAllRezervari();
                    rezervari.CloseConnection();
                    return rezervariData;
                default:
                    throw new ArgumentException($"Class {className} not supported.");
            }
        }

        private void ShowReservationsButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedId > 0)
                {
                    DataTable reservationsData = GetRezervariByPasagerId(selectedId);
                    dataGridView1.DataSource = reservationsData;

                    dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                    backButton.Visible = true;
                    showReservationsButton.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la încărcarea rezervărilor pasagerului selectat: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            btnLoadData_Click(sender, e, connectionString);
            backButton.Visible = false;
            showReservationsButton.Visible = true;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                string className = classComboBox.SelectedItem as string;

                switch (className)
                {
                    case "Pasageri":
                        selectedId = Convert.ToInt32(row.Cells["Pasager_ID"].Value);
                        break;
                    case "Trenuri":
                        selectedId = Convert.ToInt32(row.Cells["Tren_ID"].Value);
                        break;
                    case "Angajati":
                        selectedId = Convert.ToInt32(row.Cells["Angajat_ID"].Value);
                        break;
                    // Add cases for other classes as needed
                    default:
                        selectedId = -1;
                        break;
                }
            }
        }

        private DataTable GetRezervariByPasagerId(int pasagerID)
        {
            string query = @"SELECT P.Nume, P.Prenume, R.Data, T.Nume AS NumeTren, R.Numar_loc, R.Pret_bilet, R.Vagon
                             FROM Pasageri P
                             JOIN Rezervari R ON P.Pasager_ID = R.Pasager_ID
                             JOIN Trenuri T ON R.Tren_ID = T.Tren_ID
                             WHERE P.Pasager_ID = @PasagerID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.Parameters.AddWithValue("@PasagerID", pasagerID);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        private void ExecuteQueryButton_Click(object sender, EventArgs e)
        {
            string selectedQuery = queryComboBox.SelectedItem as string;

            if (selectedQuery != null)
            {
                switch (selectedQuery)
                {
                    case "Informații despre pasageri, rezervări și trenuri":
                        LoadQueryData("SELECT P.Nume, P.Prenume, R.Data, T.Nume AS NumeTren, R.Numar_loc " +
                                     "FROM Pasageri P " +
                                     "JOIN Rezervari R ON P.Pasager_ID = R.Pasager_ID " +
                                     "JOIN Trenuri T ON R.Tren_ID = T.Tren_ID;");
                        break;

                    case "Capetele traseelor trenurilor":
                        LoadQueryData("SELECT T.Tren_ID, G1.Nume AS 'GaraInițială', G2.Nume AS 'GaraFinală' " +
                                     "FROM Trenuri T " +
                                     "JOIN Rute R1 ON T.Tren_ID = R1.Tren_ID " +
                                     "JOIN Gari G1 ON R1.Gara_ID = G1.Gara_ID " +
                                     "JOIN Rute R2 ON T.Tren_ID = R2.Tren_ID " +
                                     "JOIN Gari G2 ON R2.Gara_ID = G2.Gara_ID " +
                                     "WHERE R1.Ora_stationare = (SELECT MIN(Ora_stationare) FROM Rute WHERE Tren_ID = T.Tren_ID) " +
                                     "AND R2.Ora_stationare = (SELECT MAX(Ora_stationare) FROM Rute WHERE Tren_ID = T.Tren_ID);");
                        break;

                    case "Gări și ora plecării fiecărui tren":
                        LoadQueryData("SELECT Trenuri.Nume AS Nume_Tren, " +
                                     "DATEADD(MINUTE, CAST(Rute.Timp_Stationare AS INT), Rute.Ora_Stationare) AS Ora_Plecare, " +
                                     "Gari.Nume AS Nume_Gara " +
                                     "FROM Trenuri " +
                                     "JOIN Rute ON Trenuri.Tren_ID = Rute.Tren_ID " +
                                     "JOIN Gari ON Rute.Gara_ID = Gari.Gara_ID;");
                        break;

                    case "Angajați pe trenuri":
                        LoadQueryData("SELECT A.Nume AS Nume_Angajat, " +
                                     "A.Prenume AS Prenume_Angajat, " +
                                     "T.Nume AS Nume_Tren, " +
                                     "A.Functie AS Rol " +
                                     "FROM Angajati AS A " +
                                     "JOIN Trenuri_Angajati AS TA ON A.Angajat_ID = TA.Angajat_ID " +
                                     "JOIN Trenuri AS T ON TA.Tren_ID = T.Tren_ID;");
                        break;

                    case "Venit pentru fiecare tren":
                        LoadQueryData("SELECT T.Nume AS Nume_Tren, SUM(R.Pret_bilet) AS Venit_Total " +
                                      "FROM Trenuri T " +
                                      "JOIN Rezervari R ON T.Tren_ID = R.Tren_ID " +
                                      "GROUP BY T.Nume;");
                        break;

                    case "Informații despre trenul cu cele mai multe rezervări":
                        LoadQueryData("SELECT Nume, Tip, Capacitate " +
                                     "FROM Trenuri " +
                                     "WHERE Tren_ID = (SELECT TOP 1 Tren_ID FROM Rezervari GROUP BY Tren_ID ORDER BY COUNT(Rezervare_ID) DESC);");
                        break;

                    case "Numărul de locuri libere în fiecare tren":
                        LoadQueryData("SELECT T.Nume, T.Capacitate - COALESCE(R.RezervariCount, 0) AS LocuriLibere " +
                                      "FROM Trenuri T " +
                                      "LEFT JOIN (SELECT Tren_ID, COUNT(Rezervare_ID) AS RezervariCount " +
                                      "           FROM Rezervari GROUP BY Tren_ID) AS R ON T.Tren_ID = R.Tren_ID;");
                        break;

                    case "Nr. angajați de la gara cea mai frecventată":
                        LoadQueryData("SELECT TOP 1 G.Nume AS Nume_Gara, G.Gara_ID, " +
                                         "(SELECT COUNT(DISTINCT REZ.Pasager_ID) " +
                                         " FROM Rute R " +
                                         " JOIN Trenuri T ON R.Tren_ID = T.Tren_ID " +
                                         " JOIN Gari G1 ON R.Gara_ID = G1.Gara_ID " +
                                         " JOIN Rezervari REZ ON T.Tren_ID = REZ.Tren_ID " +
                                         " WHERE G1.Gara_ID = G.Gara_ID) AS Numar_Pasageri_Rezervari, " +
                                         "(SELECT COUNT(Angajat_ID) " +
                                         " FROM Angajati A " +
                                         " WHERE A.Gara_ID = G.Gara_ID) AS Numar_Angajati " +
                                         " FROM Gari G " +
                                         " ORDER BY Numar_Pasageri_Rezervari DESC;");
                        break;

                    default:
                        break;
                }
            }
        }


        private void LoadQueryData(string query)
        {
            try
            {
                DataTable queryData = ExecuteQuery(query);
                dataGridView1.DataSource = queryData;
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable ExecuteQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable dataTable = new DataTable();
                connection.Open();
                adapter.Fill(dataTable);
                connection.Close();
                return dataTable;
            }
        }

        private void PopulateStations(ComboBox comboBox)
        {
            try
            {
                Gari gari = new Gari(connectionString);
                gari.OpenConnection();
                DataTable gariData = gari.GetAllGari();
                gari.CloseConnection();

                foreach (DataRow row in gariData.Rows)
                {
                    comboBox.Items.Add(row["Nume"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading stations: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TraseuButton_Click(object sender, EventArgs e)
        {
            string startStation = startStationComboBox.SelectedItem as string;
            string endStation = endStationComboBox.SelectedItem as string;

            if (!string.IsNullOrEmpty(startStation) && !string.IsNullOrEmpty(endStation))
            {
                string query = "SELECT T.Nume AS Nume_Tren, ISNULL(SUM(R.Pret_bilet), 0) AS Venit_Total " +
                               "FROM Trenuri T " +
                               "LEFT JOIN Rezervari R ON T.Tren_ID = R.Tren_ID " +
                               "WHERE T.Tren_ID IN (" +
                                   "SELECT DISTINCT T.Tren_ID " +
                                   "FROM Trenuri T " +
                                   "JOIN Rute R1 ON T.Tren_ID = R1.Tren_ID " +
                                   "JOIN Rute R2 ON T.Tren_ID = R2.Tren_ID " +
                                   "JOIN Gari G1 ON R1.Gara_ID = G1.Gara_ID " +
                                   "JOIN Gari G2 ON R2.Gara_ID = G2.Gara_ID " +
                                   $"WHERE G1.Nume = '{startStation}' AND G2.Nume = '{endStation}' AND R1.Kilometrul < R2.Kilometrul" +
                               ") " +
                               "GROUP BY T.Nume;";

                LoadQueryData(query);
            }
            else
            {
                MessageBox.Show("Please select start and end stations.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(1033, 576);
            this.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.Name = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }
    }
}