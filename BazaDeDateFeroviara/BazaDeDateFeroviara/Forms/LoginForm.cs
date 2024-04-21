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
    public partial class LoginForm : Form
    {
        private Authentification authService;
        private TextBox textBoxNume;
        private TextBox textBoxPrenume;
        private Button btnLogin;

        private string connectionString;

        public LoginForm(string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;

            authService = new Authentification(connectionString);

            textBoxNume = new TextBox
            {
                Size = new System.Drawing.Size(150, 20),
                Location = new System.Drawing.Point((this.ClientSize.Width - 150) / 2, 50), 
                Tag = "Introduceti Nume"
            };
            textBoxNume.Enter += TextBox_Enter;
            textBoxNume.Leave += TextBox_Leave;
            TextBox_Leave(textBoxNume, EventArgs.Empty); 
            Controls.Add(textBoxNume);

            textBoxPrenume = new TextBox
            {
                Size = new System.Drawing.Size(150, 20),
                Location = new System.Drawing.Point((this.ClientSize.Width - 150) / 2, 80),
                Tag = "Introduceti Prenume"
            };
            textBoxPrenume.Enter += TextBox_Enter;
            textBoxPrenume.Leave += TextBox_Leave;
            TextBox_Leave(textBoxPrenume, EventArgs.Empty); 
            Controls.Add(textBoxPrenume);

            btnLogin = new Button
            {
                Size = new System.Drawing.Size(100, 30),
                Location = new System.Drawing.Point((this.ClientSize.Width - 100) / 2, 110), // Centered horizontally
                Text = "Login"
            };
            btnLogin.Click += new EventHandler(btnLogin_Click);
            Controls.Add(btnLogin);
        }

        private void TextBox_Enter(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == textBox.Tag?.ToString())
            {
                textBox.Text = string.Empty;
            }
        }

        private void TextBox_Leave(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = textBox.Tag?.ToString();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string nume = textBoxNume.Text;
                string prenume = textBoxPrenume.Text;

                bool isAuthenticated = authService.AuthenticatePasager(nume, prenume);

                if (isAuthenticated)
                {
                    ShowCustomMessageBox("Login reusit!", "Success");

                    this.Hide();
                    MainForm mainForm = new MainForm(connectionString);
                    mainForm.ShowDialog();
                    this.Close();
                }
                else
                {
                    ShowCustomMessageBox("Nume sau Prenume invalid", "Error");
                }
            }
            catch (Exception ex)
            {
                ShowCustomMessageBox("Error during login: " + ex.Message, "Error");
            }
        }
        private void ShowCustomMessageBox(string message, string caption)
        {
            Form messageForm = new Form();
            messageForm.Size = new System.Drawing.Size(300, 150);
            messageForm.Font = new System.Drawing.Font("Times New Roman", 12F);
            messageForm.StartPosition = FormStartPosition.CenterScreen;

            Label label = new Label();
            label.Text = message;
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.MiddleCenter;
            messageForm.Controls.Add(label);

            Button okButton = new Button();
            okButton.Text = "OK";
            okButton.Dock = DockStyle.Bottom;
            okButton.Click += (s, e) => messageForm.Close();
            messageForm.Controls.Add(okButton);

            messageForm.ShowDialog();
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // LoginForm
            // 
            this.ClientSize = new System.Drawing.Size(484, 258);
            this.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.ResumeLayout(false);

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // You can perform initialization tasks here
        }
    }
}
