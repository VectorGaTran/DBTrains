using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using BazaDeDateFeroviara.Forms;

namespace BazaDeDateFeroviara
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string connectionString = @"Data Source=VICTOR-TRANDAFI\SQLEXPRESS;Initial Catalog=CFR;Integrated Security=True;";

            LoginForm loginForm = new LoginForm(connectionString);

            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                MainForm mainForm = new MainForm(connectionString);
                Application.Run(mainForm);
            }
            else
            {
                ShowCustomMessageBox("Exiting application.", "Success");
                Application.Exit();
            }
        }
        private static void ShowCustomMessageBox(string message, string caption)
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
    }
}


