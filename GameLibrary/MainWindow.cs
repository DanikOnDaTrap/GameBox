using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using static System.Net.Mime.MediaTypeNames;
//using static System.Net.Mime.MediaTypeNames;

namespace GameLibrary
{
    public partial class MainWindow : Form
    {
        string Username;
        SqlConnection connection;
        DataTable table;
        DataTable FullProductTable;
        // dt Following
        Label[] gameLabelsName;
        Label[] gameLabelsPrice;
        Label[] LabelTrending;
        Label[] LabelTrendingDesc;
        PictureBox[] picBoxes;
        PictureBox[] PBTrending;
        PictureBox[] PlatformFirst;
        PictureBox[] PlatformSecond;
        PictureBox[] PlatformThird;
        PictureBox[][] Platforms;
        Panel[] Panels;
        Panel[] PanelsTrends;
        int currentPage = 0;
        int minGameID = 0;
        int maxGameID = 18;
        public List<int> cart = new List<int>();
        bool EditingInProgress = false;

        public MainWindow(string UserID, SqlConnection sqlCon)
        {
            InitializeComponent();
            this.Username = UserID;
            connection = sqlCon;
            Panels = new[] { panel1, panel2, panel3, panel4, panel5, panel6, panel7, panel8, panel9, panel10, panel11, panel12, panel13, panel14, panel15, panel16, panel17, panel18, panel19, panel23, panel24, panel25, panel26, panel27 };
            gameLabelsName = new[] { label2, label4, label6, label8, label10, label12, label14, label16, label18, label20, label22, label24, label26, label28, label30, label32, label34, label36};
            gameLabelsPrice = new[] { label1, label3, label5, label7, label9, label11, label13, label15, label17, label19, label21, label23, label25, label27, label29, label31, label33, label35,};
            picBoxes = new[] { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12, pictureBox13, pictureBox14, pictureBox15, pictureBox16, pictureBox17, pictureBox18 };
            PBTrending = new[] { pictureBoxTrend1, pictureBoxTrend2, pictureBoxTrend3 };
            LabelTrending = new[] { labelTrend1, labelTrend2, labelTrend3 };
            LabelTrendingDesc = new[] { labelTrendDesc1, labelTrendDesc2, labelTrendDesc3 };
            PlatformFirst = new[] { pictureBoxPlat1, pictureBoxPlat2, pictureBoxPlat3 };
            PlatformSecond = new[] { pictureBoxPlat4, pictureBoxPlat5, pictureBoxPlat6 };
            PlatformThird = new[] { pictureBoxPlat7, pictureBoxPlat8, pictureBoxPlat9 };
            Platforms = new[] { PlatformFirst, PlatformSecond, PlatformThird };
            PanelsTrends = new[] { panel20, panel21, panel22 };

            SetRoundedShape(panelControl, 35);
            SetRoundedShape(panelMyGames, 20);
            SetRoundedShape(pictureBoxMP, 20);
            SetRoundedShape(panelMyFollows, 20);
            for (int i = 0; i < Panels.Length; i++)
            {
                SetRoundedShape(Panels[i], 10);
            }
            for (int i = 0; i < PanelsTrends.Length; i++)
            {
                SetRoundedShape(PanelsTrends[i], 20);
            }
            for (int i = 0; i < PBTrending.Length; i++)
            {
                SetRoundedShape(PBTrending[i], 15);
            }
            GetProfile(); 
            CatalogLoad();
            SetEllipsis();
            GetFullTable();
            CartInfo();
            GetTrends();
            SetMyProflePage();
        }

        private void GetProfile()
        {
            GetTableByQuery($"SELECT * FROM [User] WHERE Username = '{Username}'");
            labelUserName.Text = table.Rows[0].Field<string>("Username");
            pictureBoxProfile.Image = ConvertByteArrayToImage(table.Rows[0].Field<byte[]>("Photo"));
            labelBalance.Text = "На счету: " + Math.Round(table.Rows[0].Field<decimal>("Balance"),2).ToString();
        }

        private void CartInfo()
        {
            labelCart.Text = $"Количество товаров: {cart.Count} \nНа сумму {Math.Round(GetCartSum(), 2).ToString()} рублей";
        }

        private void SetEllipsis()
        {
            for (int i = 0; i < gameLabelsName.Length; i++)
            {
                gameLabelsName[i].AutoSize = false;
                gameLabelsName[i].Size = new Size(203, 21);
                gameLabelsName[i].TextAlign = ContentAlignment.MiddleLeft;
                gameLabelsName[i].AutoEllipsis = true;
            }
        }

        private decimal GetCartSum()
        {
            decimal sum = 0;
            for (int i = 0; i < cart.Count; i++)
            {
                sum += FullProductTable.Rows[cart[i]].Field<decimal>("Price");
            }
            return sum;
        }

        private void GetFullTable()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Game", connection);
            FullProductTable = new DataTable();

            adapter.SelectCommand = cmd;
            adapter.Fill(FullProductTable);
        }

        private void GetTrends()
        {
            string querySQL = $"SELECT * FROM Game ORDER BY UserScore DESC";
            GetTableByQuery(querySQL);
            for (int i = 0; i < PBTrending.Length; i++)
            {
                PBTrending[i].Image = ConvertByteArrayToImage(table.Rows[i].Field<byte[]>("Image"));
                LabelTrending[i].Text = table.Rows[i].Field<string>("Name");
                LabelTrendingDesc[i].Text = table.Rows[i].Field<string>("Description");
                if (table.Rows[i].Field<bool>("WindowsAvailable"))
                {
                    Platforms[i][0].Visible = true;
                }
                if (table.Rows[i].Field<bool>("LinuxAvailable"))
                {
                    Platforms[i][1].Visible = true;
                }
                if (table.Rows[i].Field<bool>("MacAvailable"))
                {
                    if (table.Rows[i].Field<bool>("LinuxAvailable") == false)
                    {
                        Platforms[i][2].Location = Platforms[i][1].Location;
                    }
                    Platforms[i][2].Visible = true;
                }
            }
        }









        public void SetPhoto()
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Image Files (*.png *.jpg *.jpeg) |*.png; *.jpg; *.jpeg", Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(ofd.FileName);
                }
            }
            //Insert(ConvertImageToBytes(pictureBox1.Image), Int32.Parse(textBox1.Text));
        }
        public void Insert(byte[] image, string currentID)
        {
            using (SqlCommand cmd = new SqlCommand("Update [User] Set Photo = @image Where Username = @currentID", connection))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@image", image);
                cmd.Parameters.AddWithValue("@currentID", currentID);
                cmd.ExecuteNonQuery();
            }
        }

        byte[] ConvertImageToBytes(Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        public Image ConvertByteArrayToImage(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                return
                Image.FromStream(ms);
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
        
        private void CatalogLoad()
        {
            string querySQL = $"SELECT * FROM Game WHERE ID >= {minGameID} AND ID < {maxGameID}";
            GetTableByQuery(querySQL);

            for (int i = 0; i < table.Rows.Count; i++)
            {
                try
                {
                    Panels[i].Visible = true;
                    picBoxes[i].Image = ConvertByteArrayToImage(table.Rows[i].Field<byte[]>("Image"));
                    gameLabelsName[i].Text = table.Rows[i].Field<string>("Name");
                    if (table.Rows[i].Field<Decimal>("Price") == 0)
                        gameLabelsPrice[i].Text = "Бесплатно";
                    else
                        gameLabelsPrice[i].Text = Math.Round(table.Rows[i].Field<Decimal>("Price"), 2).ToString() + " RUB";
                }
                catch
                {
                    MessageBox.Show("Внимание!", "Ошибка при загрузке каталога");
                }
            }
        }

        private void CatalogClear()
        {
            for (int i = 0; i < 18; i++)
            {
                picBoxes[i].Image = null;
                gameLabelsName[i].Text = "";
                gameLabelsPrice[i].Text = "";
                Panels[i].Visible = false;
            }
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            PictureBox selectedPB = sender as PictureBox;
            selectedPB.Location = new Point(selectedPB.Location.X - 2, selectedPB.Location.Y - 2);
            selectedPB.Size = new Size(selectedPB.Width + 5, selectedPB.Height + 5);
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            PictureBox selectedPB = sender as PictureBox;
            selectedPB.Location = new Point(selectedPB.Location.X + 2, selectedPB.Location.Y + 2);
            selectedPB.Size = new Size(selectedPB.Width - 5, selectedPB.Height - 5);
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox selectedPB = sender as PictureBox;
            GamePageForm obj = new GamePageForm(Int32.Parse(selectedPB.Name.Substring(10))-1+(currentPage*18),connection);
            obj.cart = this.cart;
            obj.ShowDialog();
            CartInfo();
        }

        private void pictureBoxRight_Click(object sender, EventArgs e)
        {
            if (pictureBox18.Image != null)
            {
                CatalogClear();

                currentPage++;
                labelCurrentPage.Text = (currentPage + 1).ToString();

                UpdateRange(18);
                CatalogLoad();
            }
        }

        private void pictureBoxLeft_Click(object sender, EventArgs e)
        {
            if (currentPage > 0)
            {
                CatalogClear();

                currentPage--;
                labelCurrentPage.Text = (currentPage + 1).ToString();

                UpdateRange(-18);
                CatalogLoad();
            }
        }

        private void UpdateRange(int value)
        {
            minGameID += value;
            maxGameID += value;
        }

        private void pictureBoxTrend1_Click(object sender, EventArgs e)
        {
            PictureBox selectedPB = sender as PictureBox;
            GamePageForm obj = new GamePageForm(table.Rows[Convert.ToInt32(selectedPB.Name.Substring(selectedPB.Name.Length - 1)) - 1].Field<int>("ID"), connection);
            obj.cart = this.cart;
            obj.ShowDialog();
            CartInfo();
        }

        private void pictureBoxTrend1_MouseEnter(object sender, EventArgs e)
        {
            PictureBox selectedPB = sender as PictureBox;
            selectedPB.Size = new Size(selectedPB.Width + 5, selectedPB.Height + 5);
        }

        private void pictureBoxTrend1_MouseLeave(object sender, EventArgs e)
        {
            PictureBox selectedPB = sender as PictureBox;
            selectedPB.Size = new Size(selectedPB.Width - 5, selectedPB.Height - 5);
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

        private void SetMyProflePage()
        {
            labelNameMP.Text = Username;
            GetTableByQuery($"SELECT [Name] AS Игра ,[Text] AS Комментарий, Score AS Оценка, [Date] AS Дата FROM Review INNER JOIN Game ON GameID = Game.ID WHERE UserID = '{Username}'");
            dataGridView1.DataSource = table;
            GetTableByQuery($"SELECT * FROM [User] WHERE Username = '{Username}'");
            textBoxAboutMe.Text = table.Rows[0].Field<string>("AboutMe");
            textBoxActualName.Text = table.Rows[0].Field<string>("Name");
            try
            {
                pictureBoxMP.Image = ConvertByteArrayToImage(table.Rows[0].Field<byte[]>("Photo"));
            }
            catch
            {
                MessageBox.Show("Ошибка при загрузке аватара", "Внимание!");
            }
            CustomDataGrid();        
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
            dataGridView1.Columns[0].Width = 200;
            dataGridView1.Columns[1].Width = 420;
            dataGridView1.Columns[2].Width = 80;
            dataGridView1.Columns[3].Width = 90;
            dataGridView1.Enabled = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Silver;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
        }

        private void pictureBoxMP_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Image Files (*.png *.jpg *.jpeg) |*.png; *.jpg; *.jpeg", Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBoxMP.Image = System.Drawing.Image.FromFile(ofd.FileName); //
                    using (SqlCommand cmd = new SqlCommand("Update [User] Set Photo = @image Where Username = @currentID", connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@image", ConvertImageToBytes(pictureBoxMP.Image));
                        cmd.Parameters.AddWithValue("@currentID", Username);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private void buttonAbotMe_Click(object sender, EventArgs e)
        {
            if (!EditingInProgress)
            {
                EditingInProgress = true;
                textBoxAboutMe.ReadOnly = false;
                textBoxActualName.ReadOnly = false;
                buttonAbotMe.Text = "Завершить редактирование";
            }
            else
            {
                using (SqlCommand cmd = new SqlCommand("Update [User] Set Name = @name, AboutMe = @about Where Username = @currentID", connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@name", textBoxActualName.Text);
                    cmd.Parameters.AddWithValue("@about", textBoxAboutMe.Text);
                    cmd.Parameters.AddWithValue("@currentID", Username);
                    cmd.ExecuteNonQuery();
                }
                EditingInProgress = false;
                textBoxAboutMe.ReadOnly = true;
                textBoxActualName.ReadOnly = true;
                buttonAbotMe.Text = "Редактировать данные профиля";
            }            
        }
    }    
}