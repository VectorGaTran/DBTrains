using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BazaDeDateFeroviara.DataAccess;


namespace BazaDeDateFeroviara.Forms
{
    public partial class InsertForm : Form
    {
        private Label[] labels;
        private TextBox[] textBoxes;
        private Button insertButton;
        private Button closeButton;
        private string className;
        private string connectionString;

        public InsertForm(string className, string connectionString)
        {
            InitializeComponent();
            this.className = className;
            this.connectionString = connectionString;
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Text = $"Inserează {className}";
            this.Size = new System.Drawing.Size(400, 300);

            switch (className)
            {
                case "Angajati":
                    labels = new Label[] { new Label(), new Label(), new Label(), new Label(), new Label(), new Label(), new Label() };
                    textBoxes = new TextBox[] { new TextBox(), new TextBox(), new TextBox(), new TextBox(), new TextBox(), new TextBox(), new TextBox() };
                    labels[0].Text = "Nume (obligatoriu)";
                    labels[1].Text = "Prenume (obligatoriu)";
                    labels[2].Text = "CNP (obligatoriu)";
                    labels[3].Text = "Email";
                    labels[4].Text = "Numar telefon";
                    labels[5].Text = "Functie (obligatoriu)";
                    labels[6].Text = "Salariu (obligatoriu)";
                    break;

                case "Pasageri":
                    labels = new Label[] { new Label(), new Label(), new Label(), new Label(), new Label() };
                    textBoxes = new TextBox[] { new TextBox(), new TextBox(), new TextBox(), new TextBox(), new TextBox() };
                    labels[0].Text = "Nume (obligatoriu)";
                    labels[1].Text = "Prenume (obligatoriu)";
                    labels[2].Text = "Numar telefon";
                    labels[3].Text = "Email";
                    labels[4].Text = "Tip";
                    break;

                case "Trenuri":
                    labels = new Label[] { new Label(), new Label(), new Label() };
                    textBoxes = new TextBox[] { new TextBox(), new TextBox(), new TextBox() };
                    labels[0].Text = "Nume (obligatoriu)"; 
                    labels[1].Text = "Tip (obligatoriu)";
                    labels[2].Text = "Capacitate (obligatoriu)";
                    break;

                // Add cases for other classes as needed

                default:
                    break;
            }

            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].Size = new System.Drawing.Size(100, 20);
                labels[i].Location = new System.Drawing.Point(50, 30 + i * 30);
                textBoxes[i].Size = new System.Drawing.Size(200, 20);
                textBoxes[i].Location = new System.Drawing.Point(200, 30 + i * 30);
                this.Controls.Add(labels[i]);
                this.Controls.Add(textBoxes[i]);
            }

            insertButton = new Button
            {
                Size = new System.Drawing.Size(80, 30),
                Location = new System.Drawing.Point(50, 30 + labels.Length * 30 + 20),
                Text = "Inserează",
            };
            insertButton.Click += new EventHandler(btnInsert_Click);
            this.Controls.Add(insertButton);

            closeButton = new Button
            {
                Size = new System.Drawing.Size(80, 30),
                Location = new System.Drawing.Point(150, 30 + labels.Length * 30 + 20),
                Text = "Închide",
            };
            closeButton.Click += new EventHandler(btnClose_Click);
            this.Controls.Add(closeButton);
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                string[] values = new string[textBoxes.Length];
                for (int i = 0; i < textBoxes.Length; i++)
                {
                    values[i] = textBoxes[i].Text;
                }

                switch (className)
                {
                    case "Angajati":
                        if (string.IsNullOrWhiteSpace(values[0]) || string.IsNullOrWhiteSpace(values[1]) || string.IsNullOrWhiteSpace(values[2]) ||
                            string.IsNullOrWhiteSpace(values[5]) || string.IsNullOrWhiteSpace(values[6]))
                        {
                            MessageBox.Show("Nume, Prenume, CNP, Functie, și Salariu nu pot fi necompletate .", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        Angajati angajati = new Angajati(connectionString);
                        angajati.InsertAngajat(values[0], values[1], values[2], values[3], values[4], values[5], Convert.ToInt16(values[6]));
                        break;

                    case "Pasageri":
                        if (string.IsNullOrWhiteSpace(values[0]) || string.IsNullOrWhiteSpace(values[1]) ||
                            (string.IsNullOrWhiteSpace(values[2]) && string.IsNullOrWhiteSpace(values[3])))
                        {
                            MessageBox.Show("Nume, Prenume, și ori Numar telefon ori Email trebuie completate.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        Pasageri pasageri = new Pasageri(connectionString);
                        pasageri.InsertPasager(values[0], values[1], values[2], values[3], values[4]);
                        break;

                    case "Trenuri":
                        if (string.IsNullOrWhiteSpace(values[0]) || string.IsNullOrWhiteSpace(values[1]) || string.IsNullOrWhiteSpace(values[2]))
                        {
                            MessageBox.Show("Toate câmpurile trebuie completate.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        Trenuri trenuri = new Trenuri(connectionString);
                        if (short.TryParse(values[2], out short attribute3))
                        {
                            trenuri.InsertTren(values[0], values[1], attribute3);
                        }
                        else
                        {
                            MessageBox.Show("Valoare invalidă pentru capacitate.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        break;

                    // Add cases for other classes as needed

                    default:
                        break;
                }

                MessageBox.Show($"În tabelul {className} s-a inserat cu succes.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la inserarea datelor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // InsertForm
            // 
            this.ClientSize = new System.Drawing.Size(1088, 552);
            this.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.Name = "InsertForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.InsertForm_Load);
            this.ResumeLayout(false);

        }

        private void InsertForm_Load(object sender, EventArgs e)
        {

        }
    }
}
