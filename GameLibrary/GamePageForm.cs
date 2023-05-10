using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GameLibrary
{
    public partial class GamePageForm : Form
    {
        int accessLevel = 0;
        SqlConnection connection;
        DataTable table;
        DataTable tempTable;
        DataTable subTableCategory;
        DataTable subTableDeveloper;
        DataTable subTableReview;
        DataTable subTableCollection;
        int gameID;
        string Username;
        bool inCollection = false;

        public GamePageForm(int productID, string uname, int Role, SqlConnection cn)
        {
            InitializeComponent();
            gameID = productID;
            connection = cn;
            Username = uname;
            accessLevel = Role;
 
            LoadData();
            SetDesign();
        }

        private void GetTableByQuery(string sqlQ)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand(sqlQ, connection);
            tempTable = new DataTable();

            adapter.SelectCommand = cmd;
            adapter.Fill(tempTable);
        }

        private void SetDesign()
        {
            SetRoundedShape(labelDescription, 15);
            SetRoundedShape(labelSystem, 15);
            SetRoundedShape(labelPressScore, 15);
            SetRoundedShape(labelUserScore, 15);
            SetRoundedShape(labelDeveloper, 15);
            SetRoundedShape(labelCategory, 15);
            SetRoundedShape(pictureBoxLogo, 15);
        }

        private void CustomDataGrid()
        {
            dataGridView1.Font = new Font("Century Gothic", 12F);
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Gray;
            dataGridView1.RowTemplate.MinimumHeight = 50;
            dataGridView1.Columns[0].Width = 170;
            dataGridView1.Columns[1].Width = 653;
            dataGridView1.Columns[2].Width = 90;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Silver;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
            if (accessLevel == 0)
                dataGridView1.ReadOnly = true;
        }

        private void LoadData()
        {
            GetTableByQuery($"SELECT * FROM GAME WHERE ID = {gameID}"); table = tempTable;
            GetTableByQuery($"SELECT Category.Name FROM Game INNER JOIN Category ON Game.CategoryID = Category.ID WHERE Game.ID = {gameID}"); subTableCategory = tempTable;
            GetTableByQuery($"SELECT Developer.Name FROM Game INNER JOIN Developer ON Game.DeveloperID = Developer.ID WHERE Game.ID = {gameID}"); subTableDeveloper = tempTable;
            GetTableByQuery($"SELECT Username AS Пользователь, [Text] AS Отзыв, Score AS Оценка FROM Review INNER JOIN [User] ON [User].Username = Review.UserID WHERE GameID = {gameID}"); subTableReview = tempTable;
            GetTableByQuery($"SELECT * FROM Purchase WHERE UserID = '{Username}'"); subTableCollection = tempTable;

            labelName.Text = table.Rows[0].Field<string>("Name");
            this.Text = table.Rows[0].Field<string>("Name");
            labelDescription.Text = table.Rows[0].Field<string>("Description");
            labelAge.Text = table.Rows[0].Field<int>("AgeRating").ToString() + "+";
            labelCategory.Text = "Категория: " + subTableCategory.Rows[0].Field<string>("Name");
            labelDeveloper.Text = "Разработчик: " + subTableDeveloper.Rows[0].Field<string>("Name");
            labelReleaseDate.Text = "Дата выхода: " + table.Rows[0].Field<DateTime>("ReleaseDate").ToLongDateString();
            labelSystem.Text = table.Rows[0].Field<string>("SystemRequirements");
            pictureBoxLogo.Image = ConvertByteArrayToImage(table.Rows[0].Field<byte[]>("Image"));
            labelUserScore.Text = "Оценка игроков: " + table.Rows[0].Field<double>("UserScore").ToString() + " / 10";
            labelPressScore.Text = "Оценка прессы: " + table.Rows[0].Field<double>("PressScore").ToString() + " / 10";
            if (table.Rows[0].Field<Decimal>("Price") == 0)
            {
                labelPrice.Text = "Бесплатно";
            }       
            else
            {
                labelPrice.Text = Math.Round(table.Rows[0].Field<Decimal>("Price"), 2).ToString() + " RUB";
            }
            if (table.Rows[0].Field<bool>("WindowsAvailable"))
            {
                pictureBoxPlatform1.Visible = true;
            }
            if (table.Rows[0].Field<bool>("LinuxAvailable"))
            {
                pictureBoxPlatform2.Visible = true;
            }
            if (table.Rows[0].Field<bool>("MacAvailable"))
            {
                if (table.Rows[0].Field<bool>("LinuxAvailable") == false)
                {
                    pictureBoxPlatform3.Location = pictureBoxPlatform2.Location;
                }
                pictureBoxPlatform3.Visible = true;
            }
            dataGridView1.DataSource = subTableReview;
            CustomDataGrid();
        }

        public Image ConvertByteArrayToImage(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                return
                Image.FromStream(ms);
            }
        }

        private void buttonAddToCart_Click(object sender, EventArgs e)
        {
            if (inCollection == false)
            {
                CartAdd();
            }
            else
            {
                CartRemove();
            }
        }

        public void CartAdd()
        {
            GetTableByQuery("SELECT * FROM Purchase");
            using (SqlCommand cmd = new SqlCommand($"INSERT Purchase VALUES ('{Username}', '{gameID}','{DateTime.Now}')", connection))
            {
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            buttonAddToCart.Text = "В коллекции!";
            inCollection = true;
        }

        public void CartRemove()
        {
            using (SqlCommand cmd = new SqlCommand($"DELETE FROM Purchase WHERE UserID = '{Username}' AND GameID = {gameID}", connection))
            {
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            buttonAddToCart.Text = "Добавить в коллекцию";
            inCollection = false;
        }

        private void GamePageForm_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < subTableCollection.Rows.Count; i++)
            {
                if (subTableCollection.Rows[i].Field<int>("GameID") == gameID)
                    inCollection = true;
            }
            if (inCollection)
                buttonAddToCart.Text = "В коллекции!";
            else
                buttonAddToCart.Text = "Добавить в коллекцию";
        }

        static void SetRoundedShape(Control control, int radius)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddLine(radius, 0, control.Width - radius, 0);
            path.AddArc(control.Width - radius, 0, radius, radius, 270, 90);
            path.AddLine(control.Width, radius, control.Width, control.Height - radius);
            path.AddArc(control.Width - radius, control.Height - radius, radius, radius, 0, 90);
            path.AddLine(control.Width - radius, control.Height, radius, control.Height);
            path.AddArc(0, control.Height - radius, radius, radius, 90, 90);
            path.AddLine(0, control.Height - radius, 0, radius);
            path.AddArc(0, 0, radius, radius, 180, 90);
            control.Region = new Region(path);
        }

        private void buttonAddReview_Click(object sender, EventArgs e)
        {
            ReviewForm obj = new ReviewForm(connection, labelName.Text, gameID, Username);
            obj.ShowDialog();
            LoadData();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Удалить отзыв?", "Модерация", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                using (SqlCommand cmd = new SqlCommand($"DELETE FROM Review WHERE UserID = '{dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString()}' AND Text = '{dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[1].Value.ToString()}'", connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                LoadData();
            }
        }
    }
}
