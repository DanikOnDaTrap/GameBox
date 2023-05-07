using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace GameLibrary
{
    public partial class RegistrationForm : Form
    {
        SqlConnection connection;
        DataTable table;

        public RegistrationForm(SqlConnection sqlCon)
        {
            InitializeComponent();
            connection = sqlCon;
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand($"INSERT [User] VALUES ('{textBoxUsername.Text}', '{textBoxLogin.Text}', '{textBoxPassword.Text}', 0,0,NULL,NULL,NULL,NULL) ", connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Учетная запись создана!", "Внимание");
                this.Close();
            }
            catch
            {
                GetTableByQuery("SELECT * FROM [USER]");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (textBoxUsername.Text == table.Rows[i].Field<string>("Username"))
                    {
                        MessageBox.Show("Имя пользователя занято!", "Ошибка");
                    }
                }
            }
        }
        private void GetTableByQuery(string sqlQ)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand(sqlQ, connection);
            table = new DataTable();

            adapter.SelectCommand = cmd;
            adapter.Fill(table);
        }
    }
}
