using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace GameLibrary
{
    public partial class ReviewForm : Form
    {
        DataTable table;
        SqlConnection connection;
        string gameName;
        int gameID;
        string userID;

        public ReviewForm(SqlConnection cn, string name, int id, string usID)
        {
            InitializeComponent();
            connection = cn;
            gameName = name;
            gameID = id;
            userID = usID;

            labelGName.Text = gameName;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            GetTableByQuery("SELECT * FROM Review");
            try
            {
                using (SqlCommand cmd = new SqlCommand($"INSERT Review VALUES ('{userID}', {gameID}, '{DateTime.Now}', '{textBoxReview.Text}', {Convert.ToDecimal(textBoxScore.Text)})", connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                this.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка при добавлении отзыва", "Внимание");
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

        private void textBoxScore_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || (e.KeyChar == Convert.ToChar(",")) || e.KeyChar == '\b')
                return;
            else
                e.Handled = true;
        }

        private void textBoxScore_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDecimal(textBoxScore.Text) > 10 || Convert.ToDecimal(textBoxScore.Text) < 1)
                    buttonAdd.Enabled = false;
                else
                    buttonAdd.Enabled = true;
            }
            catch
            {

            }
        }
    }
}
