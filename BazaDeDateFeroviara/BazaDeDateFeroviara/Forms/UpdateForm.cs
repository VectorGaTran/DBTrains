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
    public partial class UpdateForm : Form
    {
        private Label[] labels;
        private TextBox[] textBoxes;
        private Button updateButton;
        private Button closeButton;
        private string className;
        private string connectionString;
        private MainForm mainForm;
        private int recordID;


        public UpdateForm(string className, string connectionString, MainForm mainForm)
        {
            InitializeComponent();
            this.className = className;
            this.connectionString = connectionString;
            this.mainForm = mainForm;
            InitializeUI();
        }

        public void SetRecordID(int id)
        {
            recordID = id;
        }

        private void InitializeUI()
        {
            this.Text = $"Actualizează {className}";
            this.Size = new System.Drawing.Size(400, 300);

            switch (className)
            {
                case "Angajati":
                    labels = new Label[] { new Label(), new Label(), new Label(), new Label(), new Label(), new Label(), new Label() };
                    textBoxes = new TextBox[] { new TextBox(), new TextBox(), new TextBox(), new TextBox(), new TextBox(), new TextBox(), new TextBox() };
                    labels[0].Text = "Nume ";
                    labels[1].Text = "Prenume ";
                    labels[2].Text = "CNP ";
                    labels[3].Text = "Email ";
                    labels[4].Text = "Numar telefon ";
                    labels[5].Text = "Functie ";
                    labels[6].Text = "Salariu ";

                    break;

                case "Pasageri":
                    labels = new Label[] { new Label(), new Label(), new Label(), new Label(), new Label() };
                    textBoxes = new TextBox[] { new TextBox(), new TextBox(), new TextBox(), new TextBox(), new TextBox() };
                    labels[0].Text = "Nume ";
                    labels[1].Text = "Prenume ";
                    labels[2].Text = "Numar telefon ";
                    labels[3].Text = "Email ";
                    labels[4].Text = "Tip ";
                    break;

                // Add cases for other classes as needed

                default:
                    break;
            }

            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].Size = new System.Drawing.Size(200, 20);
                labels[i].Location = new System.Drawing.Point(50, 40 + i * 30);
                textBoxes[i].Size = new System.Drawing.Size(200, 20);
                textBoxes[i].Location = new System.Drawing.Point(250, 40 + i * 30);
                this.Controls.Add(labels[i]);
                this.Controls.Add(textBoxes[i]);
            }

            updateButton = new Button
            {
                Size = new System.Drawing.Size(100, 30),
                Location = new System.Drawing.Point(50, 40 + labels.Length * 30 + 20),
                Text = "Actualizează",
            };
            updateButton.Click += new EventHandler(btnUpdate_Click);
            this.Controls.Add(updateButton);

            closeButton = new Button
            {
                Size = new System.Drawing.Size(100, 30),
                Location = new System.Drawing.Point(200, 40 + labels.Length * 30 + 20),
                Text = "Închide",
            };
            closeButton.Click += new EventHandler(btnClose_Click);
            this.Controls.Add(closeButton);
        }

        private void UpdateForm_Load(object sender, EventArgs e)
        {
            try
            {
                switch (className)
                {
                    case "Angajati":
                        Angajati angajati = new Angajati(connectionString);
                        string[] angajatiData = angajati.GetAngajatDataByID(recordID);

                        if (angajatiData.Length == textBoxes.Length)
                        {
                            for (int i = 0; i < textBoxes.Length; i++)
                            {
                                textBoxes[i].Text = angajatiData[i];
                            }
                        }
                        break;

                    case "Pasageri":
                        Pasageri pasageri = new Pasageri(connectionString);
                        string[] pasageriData = pasageri.GetPasagerDataByID(recordID);

                        if (pasageriData.Length == textBoxes.Length)
                        {
                            for (int i = 0; i < textBoxes.Length; i++)
                            {
                                textBoxes[i].Text = pasageriData[i];
                            }
                        }
                        break;

                    // Add cases for other classes as needed

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
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
                        
                        Angajati angajati = new Angajati(connectionString);
                        angajati.UpdateAngajat(recordID, values[0], values[1], values[2], values[3], values[4], values[5], Convert.ToInt16(values[6]));
                        break;

                    case "Pasageri":
                        
                        Pasageri pasageri = new Pasageri(connectionString);
                        pasageri.UpdatePasager(recordID, values[0], values[1], values[2], values[3], values[4]);
                        break;

                    // Add cases for other classes as needed

                    default:
                        break;
                }

                mainForm.btnRefreshData_Click(sender, e, connectionString);
                MessageBox.Show($"Tabelul {className} a fost actualizat cu succes.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la actualizarea datelor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            // UpdateForm
            // 
            this.ClientSize = new System.Drawing.Size(1013, 749);
            this.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.Name = "UpdateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.UpdateForm_Load);
            this.ResumeLayout(false);

        }

    }
}
