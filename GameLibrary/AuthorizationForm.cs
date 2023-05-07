using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace GameLibrary
{
    public partial class AuthorizationForm : Form
    {
        public SqlConnection cn;
        DataTable table;
        private string textCaptcha = String.Empty;
        bool captchaRequired = false;
        int secblocked = 0;

        public AuthorizationForm()
        {
            InitializeComponent();
            textBoxPassword.UseSystemPasswordChar = true;
            ConnectionBuild();
        }

        private void ConnectionBuild()
        {
            try
            {
                SqlConnectionStringBuilder connect = new SqlConnectionStringBuilder();
                connect.InitialCatalog = "GameLibDB";
                connect.DataSource = @"SHEVELEVPC\SQLEXPRESS";
                connect.ConnectTimeout = 5;
                connect.IntegratedSecurity = true;
                cn = new SqlConnection();
                cn.ConnectionString = connect.ConnectionString;
                cn.Open();
            }
            catch
            {
                MessageBox.Show("Соединение не установлено", "Ошибка");
            }
        }

        private void pictureBoxShow_Click(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = !textBoxPassword.UseSystemPasswordChar;
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            // username
            string querySQL = $"SELECT Username FROM [USER] WHERE Login = '{"shev"}' AND Password = '{"shev"}'";  // Авторизация

            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand(querySQL, cn);
            table = new DataTable();

            adapter.SelectCommand = cmd;
            adapter.Fill(table);

            if (captchaRequired == false)
            {
                if (table.Rows.Count == 0)
                {
                    MessageBox.Show("Данные введены неверно", "Ошибка!");
                    textBoxLogin.Text = "";
                    textBoxPassword.Text = "";
                    CaptchaFieldUPD();
                    pictureBox2.Image = this.CreateImage(pictureBox2.Width, pictureBox2.Height);
                }
                else
                {
                    MWOpen();
                }
            }
            else
            {
                if (table.Rows.Count != 0 && textBoxCaptcha.Text == textCaptcha)
                {
                    CaptchaFieldUPD();
                    MWOpen();
                }
                else
                {
                    pictureBox2.Image = this.CreateImage(pictureBox2.Width, pictureBox2.Height);
                    MessageBox.Show("Введены некорректные данные, возможность авторизации заблокирована на 10 секунд", "Ошибка!");
                    buttonConnect.Enabled = false;
                    timer1.Start();
                }
            }
        }
        private void CaptchaFieldUPD()
        {
            captchaRequired = !captchaRequired;
            panelCaptcha.Visible = !panelCaptcha.Visible;
        }

        private void MWOpen()
        {
            MainWindow obj = new MainWindow(table.Rows[0].Field<string>("Username"), cn);
            obj.ShowDialog();
        }
        private void labelRegistration_Click(object sender, EventArgs e)
        {
            RegistrationForm obj = new RegistrationForm(this.cn);
            obj.ShowDialog();
        }

        private Bitmap CreateImage(int Width, int Height)
        {
            Random rnd = new Random();
            Bitmap result = new Bitmap(Width, Height);

            int Xpos = rnd.Next(5, Width - 120);
            int Ypos = rnd.Next(20, Height - 30);

            Brush[] colors = { Brushes.Black,
                     Brushes.Red,
                     Brushes.RoyalBlue,
                     Brushes.Green };

            Graphics g = Graphics.FromImage((Image)result);

            g.Clear(Color.Gray);

            textCaptcha = String.Empty;
            string ALF = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
            for (int i = 0; i < 5; ++i)
                textCaptcha += ALF[rnd.Next(ALF.Length)];

            g.DrawString(textCaptcha,
                         new Font("Arial", 20),
                         colors[rnd.Next(colors.Length)],
                         new PointF(Xpos, Ypos));
            g.DrawLine(Pens.Black,
                       new Point(0, 0),
                       new Point(Width - 1, Height - 1));
            g.DrawLine(Pens.Black,
                       new Point(0, Height - 1),
                       new Point(Width - 1, 0));
            for (int i = 0; i < Width; ++i)
                for (int j = 0; j < Height; ++j)
                    if (rnd.Next() % 20 == 0)
                        result.SetPixel(i, j, Color.White);

            return result;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (secblocked >= 10)
            {
                buttonConnect.Enabled = true;
                timer1.Stop();
            }
            else
            {
                secblocked++;
            }
        }

        private void labelRegistration_MouseEnter(object sender, EventArgs e)
        {
            labelRegistration.Font = new Font(labelRegistration.Font, FontStyle.Underline);
        }

        private void labelRegistration_MouseLeave(object sender, EventArgs e)
        {
            labelRegistration.Font = new Font(labelRegistration.Font, FontStyle.Regular);
        }
    }
}