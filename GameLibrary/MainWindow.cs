using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GameLibrary
{
    public partial class MainWindow : Form
    {
        string Username;
        int accessLevel = 0;
        int MGincrementation = 0;
        int FollowingIncrementation = 0;
        int currentPage = 0;
        int currentPageMG = 0;
        int currentPageFollowing = 0;
        int minGameID = 0;
        int maxGameID = 18;
        bool EditingInProgress = false;
        SqlConnection connection;
        DataTable table;
        DataTable FullProductTable;
        DataTable TrendingTable;
        Label[] gameLabelsName;
        Label[] gameLabelsPrice;
        Label[] LabelTrending;
        Label[] LabelTrendingDesc;
        Label[] labelsMyGames;
        Label[] labelsFollowing;
        PictureBox[] picBoxes;
        PictureBox[] picBoxesMyGames;
        PictureBox[] PBTrending;
        PictureBox[] PBFollows;
        PictureBox[] PlatformFirst;
        PictureBox[] PlatformSecond;
        PictureBox[] PlatformThird;
        PictureBox[][] Platforms;
        Panel[] Panels;
        Panel[] PanelsTrends;
        Panel[] panelsMyGames;
        Panel[] PanelsFollow;
        

        public MainWindow(string UserID, int Role, SqlConnection sqlCon)
        {
            InitializeComponent();
            this.Username = UserID;
            connection = sqlCon;
            accessLevel = Role;
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
            labelsMyGames = new[] { labelMP1, labelMP2, labelMP3, labelMP4, labelMP5, labelMP6 };
            picBoxesMyGames = new[] { pictureBoxMP1, pictureBoxMP2, pictureBoxMP3, pictureBoxMP4, pictureBoxMP5, pictureBoxMP6 };
            panelsMyGames = new[] { panel19, panel23, panel24, panel25, panel26, panel27 };
            PBFollows = new[] { pictureBoxFollows1, pictureBoxFollows2, pictureBoxFollows3, pictureBoxFollows4, pictureBoxFollows5, pictureBoxFollows6 };
            PanelsFollow = new[] { panelF1, panelF2, panelF3, panelF4, panelF5, panelF6 };
            labelsFollowing = new[] { label40, label41, label42, label43, label44, label45 };

            pictureBoxPrevMG.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pictureBoxNextMG.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            TextBoxWatermarkExtensionMethod.SetWatermark(textBoxSearch, "Имя пользователя");

            GetProfile();
            SetMyGames();
            SetFollows();
            CatalogLoad();
            SetEllipsis();
            GetFullTable();
            GetTrends();
            SetMyProflePage();
            RoundElements();

            if (accessLevel == 1)
                buttonAddGame.Visible = true;
        }

        private void GetProfile()
        {
            GetTableByQuery($"SELECT * FROM [User] WHERE Username = '{Username}'");
            labelUserName.Text = table.Rows[0].Field<string>("Username");
            pictureBoxProfile.Image = ConvertByteArrayToImage(table.Rows[0].Field<byte[]>("Photo"));
        }

        private void RoundElements()
        {
            SetRoundedShape(panelControl, 35);
            SetRoundedShape(panel29, 35);
            SetRoundedShape(panelMyGames, 20);
            SetRoundedShape(pictureBoxMP, 20);
            SetRoundedShape(panelMyFollows, 20);
            SetRoundedShape(panel28, 15);
            SetRoundedShape(panel35, 15);
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
            for (int i = 0; i < 6; i++)
            {
                SetRoundedShape(PanelsFollow[i], 15);
                SetRoundedShape(PBFollows[i], 15);
            }
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

        private void GetFullTable()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Game", connection);
            FullProductTable = new DataTable();

            adapter.SelectCommand = cmd;
            adapter.Fill(FullProductTable);
        }
        
        private void GetTrendingTable()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Game ORDER BY UserScore DESC", connection);
            TrendingTable = new DataTable();

            adapter.SelectCommand = cmd;
            adapter.Fill(TrendingTable);
        }

        private void GetTrends()
        {
            GetTrendingTable();
            for (int i = 0; i < PBTrending.Length; i++)
            {
                PBTrending[i].Image = ConvertByteArrayToImage(TrendingTable.Rows[i].Field<byte[]>("Image"));
                LabelTrending[i].Text = TrendingTable.Rows[i].Field<string>("Name");
                LabelTrendingDesc[i].Text = TrendingTable.Rows[i].Field<string>("Description");
                if (TrendingTable.Rows[i].Field<bool>("WindowsAvailable"))
                {
                    Platforms[i][0].Visible = true;
                }
                if (TrendingTable.Rows[i].Field<bool>("LinuxAvailable"))
                {
                    Platforms[i][1].Visible = true;
                }
                if (TrendingTable.Rows[i].Field<bool>("MacAvailable"))
                {
                    if (TrendingTable.Rows[i].Field<bool>("LinuxAvailable") == false)
                    {
                        Platforms[i][2].Location = Platforms[i][1].Location;
                    }
                    Platforms[i][2].Visible = true;
                }
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
            try
            {
                using (MemoryStream ms = new MemoryStream(data))
                {
                    return
                    Image.FromStream(ms);
                }
            }
            catch
            {
                return null;
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
                Panels[i].Visible = false;
            }
        }

        private void MyGamesClear()
        {
            for (int i = 0; i < 6; i++)
            {
                panelsMyGames[i].Visible = false;
            }
        }

        private void MyFollowingClear()
        {
            for (int i = 0; i < 6; i++)
            {
                PanelsFollow[i].Visible = false;
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
            SetMyGames();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox selectedPB = sender as PictureBox;
            GamePageForm obj = new GamePageForm(Int32.Parse(selectedPB.Name.Substring(10))-1+(currentPage*18), Username, accessLevel, connection);
            obj.ShowDialog();
            SetMyGames();
            SetMyProflePage();
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
            GamePageForm obj = new GamePageForm(TrendingTable.Rows[Convert.ToInt32(selectedPB.Name.Substring(selectedPB.Name.Length - 1)) - 1].Field<int>("ID"), Username, accessLevel,connection);
            obj.ShowDialog();
            SetMyGames();
            SetMyProflePage();
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
            dataGridView1.Columns[1].Width = 405;
            dataGridView1.Columns[2].Width = 80;
            dataGridView1.Columns[3].Width = 90;
            dataGridView1.ReadOnly = true;
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

        private bool SetMyGames()
        {
            MyGamesClear();


            GetTableByQuery($"SELECT * FROM Purchase INNER JOIN Game ON GameID = Game.ID WHERE UserID = '{Username}'");
            for (int i = 0; i < 6; i++)
            {
                
                try
                {
                    picBoxesMyGames[i].Image = ConvertByteArrayToImage(table.Rows[i + MGincrementation].Field<byte[]>("Image"));
                    labelsMyGames[i].Text = table.Rows[i + MGincrementation].Field<string>("Name");
                    panelsMyGames[i].Visible = true;

                }
                catch
                {
                    return 
                        false;
                }
            }
            return
                true;
        }

        private void pictureBoxNextMG_Click(object sender, EventArgs e)
        {
            if (panel25.Visible == true)
            {
                MGincrementation += 6;
                currentPageMG += 1;
                labelMGCount.Text = (currentPageMG + 1).ToString();
                SetMyGames();
            }
        }

        private void pictureBoxPrevMG_Click(object sender, EventArgs e)
        {
            if (currentPageMG != 0)
            {
                MGincrementation -= 6;
                currentPageMG -= 1;
                labelMGCount.Text = (currentPageMG + 1).ToString();
                SetMyGames();
            }
        }

        private bool SetFollows()
        {
            MyFollowingClear();

            GetTableByQuery($"SELECT HostID, Photo FROM Following INNER JOIN [User] ON Username = HostID WHERE FollowerID = '{Username}'");
            for (int i = 0; i < 6; i++)
            {
                try
                {
                    PBFollows[i].Image = ConvertByteArrayToImage(table.Rows[i + FollowingIncrementation].Field<byte[]>("Photo"));
                    labelsFollowing[i].Text = table.Rows[i + FollowingIncrementation].Field<string>("HostID");
                    PanelsFollow[i].Visible = true;
                }
                catch
                {
                    return
                        false;
                }
            }
            return
                true;
        }

        private void pictureBoxFollowingPrev_Click(object sender, EventArgs e)
        {
            if (currentPageFollowing != 0)
            {
                FollowingIncrementation -= 6;
                currentPageFollowing -= 1;
                labelMyFollowsCount.Text = (currentPageFollowing + 1).ToString();
                SetFollows();
            }
        }

        private void pictureBoxFollowingNext_Click(object sender, EventArgs e)
        {
            if (panelF6.Visible == true)
            {
                FollowingIncrementation += 6;
                currentPageFollowing += 1;
                labelMyFollowsCount.Text = (currentPageFollowing + 1).ToString();
                SetFollows();
            }
        }

        private void pictureBoxFollows1_MouseEnter(object sender, EventArgs e)
        {
            PictureBox selectedPB = sender as PictureBox;
            selectedPB.Size = new Size(selectedPB.Width - 1, selectedPB.Height - 1);
        }

        private void pictureBoxFollows1_MouseLeave(object sender, EventArgs e)
        {
            PictureBox selectedPB = sender as PictureBox;
            selectedPB.Size = new Size(selectedPB.Width + 1, selectedPB.Height + 1);
        }

        private void pictureBoxFollows1_Click(object sender, EventArgs e)
        {
            PictureBox selectedPB = sender as PictureBox;
            ProfileForm obj = new ProfileForm(labelsFollowing[Convert.ToInt32(selectedPB.Name.Substring(17)) - 1].Text, Username, accessLevel,connection);
            obj.ShowDialog();
            SetFollows();
        }

        private void pictureBoxMP_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxMP.Size = new Size(pictureBoxMP.Width - 1, pictureBoxMP.Height - 1);
        }

        private void pictureBoxMP_MouseLeave(object sender, EventArgs e)
        {
            pictureBoxMP.Size = new Size(pictureBoxMP.Width + 1, pictureBoxMP.Height + 1);
        }

        private void pictureBoxMP1_Click(object sender, EventArgs e)
        {
            PictureBox selectedPB = sender as PictureBox;
            GetTableByQuery($"SELECT * FROM Game WHERE Name = '{labelsMyGames[Convert.ToInt32(selectedPB.Name.Substring(12)) - 1].Text}'");            
            GamePageForm obj = new GamePageForm(table.Rows[0].Field<int>("ID"), Username, accessLevel, connection);
            obj.ShowDialog();
            SetMyGames();
            SetMyProflePage();
        }

        private void pictureBoxMP1_MouseEnter(object sender, EventArgs e)
        {
            PictureBox selectedPB = sender as PictureBox;
            selectedPB.Size = new Size(selectedPB.Width - 1, selectedPB.Height - 1);
        }

        private void pictureBoxMP1_MouseLeave(object sender, EventArgs e)
        {
            PictureBox selectedPB = sender as PictureBox;
            selectedPB.Size = new Size(selectedPB.Width + 1, selectedPB.Height + 1);
        }

        private void buttonAddGame_Click(object sender, EventArgs e)
        {
            GameAddingForm obj = new GameAddingForm(connection);
            obj.ShowDialog();
        }

        private void pictureBoxProfile_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        private void pictureBoxLeft_MouseEnter(object sender, EventArgs e)
        {
            PictureBox selectedPB = sender as PictureBox;
            selectedPB.Size = new Size(selectedPB.Width - 1, selectedPB.Height - 1);
        }

        private void pictureBoxLeft_MouseLeave(object sender, EventArgs e)
        {
            PictureBox selectedPB = sender as PictureBox;
            selectedPB.Size = new Size(selectedPB.Width + 1, selectedPB.Height + 1);
        }

        private void pictureBoxSearch_Click(object sender, EventArgs e)
        {
            GetTableByQuery($"SELECT * FROM [User] WHERE Username = '{textBoxSearch.Text}'");
            if (table.Rows.Count != 0)
            {
                using (SqlCommand cmd = new SqlCommand($"INSERT Following VALUES ('{table.Rows[0].Field<string>("Username")}','{this.Username}') ", connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show($"Вы подписались на {table.Rows[0].Field<string>("Username")}", "");
                SetFollows();
            }
            else
            {
                MessageBox.Show("Пользователя с таким именем не существует!", "Ошибка!");
            }
        }
    }
}