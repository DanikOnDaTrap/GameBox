using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GameLibrary
{
    public partial class ProfileForm : Form
    {
        string Username;
        string SenderUsername;
        int accessLevel;
        int currentPageMG = 0;
        int currentPageFollowing = 0;
        int MGincrementation = 0;
        int FollowingIncrementation = 0;
        SqlConnection connection;
        DataTable table;
        Label[] labelsMyGames;
        Label[] labelsFollowing;
        PictureBox[] picBoxesMyGames;
        PictureBox[] PBFollows;
        Panel[] panelsMyGames;
        Panel[] PanelsFollow;
        

        public ProfileForm(string usName, string usNameSender ,int Role, SqlConnection cn)
        {
            InitializeComponent();
            Username = usName;
            connection = cn;
            accessLevel = Role;
            SenderUsername = usNameSender;

            labelsMyGames = new[] { labelMP1, labelMP2, labelMP3, labelMP4, labelMP5, labelMP6 };
            picBoxesMyGames = new[] { pictureBoxMP1, pictureBoxMP2, pictureBoxMP3, pictureBoxMP4, pictureBoxMP5, pictureBoxMP6 };
            panelsMyGames = new[] { panel19, panel23, panel24, panel25, panel26, panel27 };
            PBFollows = new[] { pictureBoxFollows1, pictureBoxFollows2, pictureBoxFollows3, pictureBoxFollows4, pictureBoxFollows5, pictureBoxFollows6 };
            PanelsFollow = new[] { panelF1, panelF2, panelF3, panelF4, panelF5, panelF6 };
            labelsFollowing = new[] { label40, label41, label42, label43, label44, label45 };
            
            pictureBoxPrevMG.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pictureBoxNextMG.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);

            SetMyGames();
            SetMyProflePage();
            SetFollows();
            RoundElements();
        }

        private void GetTableByQuery(string sqlQ)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand(sqlQ, connection);
            table = new DataTable();

            adapter.SelectCommand = cmd;
            adapter.Fill(table);
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

        public Image ConvertByteArrayToImage(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                return
                    Image.FromStream(ms);
            }
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
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Silver;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
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

        private void pictureBoxMP1_Click(object sender, EventArgs e)
        {
            PictureBox selectedPB = sender as PictureBox;
            GetTableByQuery($"SELECT * FROM Game WHERE Name = '{labelsMyGames[Convert.ToInt32(selectedPB.Name.Substring(12)) - 1].Text}'");
            GamePageForm obj = new GamePageForm(table.Rows[0].Field<int>("ID"), Username, accessLevel, connection);
            obj.ShowDialog();
            SetMyGames();
        }

        private void pictureBoxFollows1_Click(object sender, EventArgs e)
        {
            PictureBox selectedPB = sender as PictureBox;
            ProfileForm obj = new ProfileForm(labelsFollowing[Convert.ToInt32(selectedPB.Name.Substring(17)) - 1].Text, SenderUsername, accessLevel, connection);
            obj.ShowDialog();
            SetFollows();
        }

        private void pictureBoxFollows4_MouseEnter(object sender, EventArgs e)
        {
            PictureBox selectedPB = sender as PictureBox;
            selectedPB.Size = new Size(selectedPB.Width - 1, selectedPB.Height - 1);
        }

        private void pictureBoxFollows4_MouseLeave(object sender, EventArgs e)
        {
            PictureBox selectedPB = sender as PictureBox;
            selectedPB.Size = new Size(selectedPB.Width + 1, selectedPB.Height + 1);
        }
        private void RoundElements()
        {
            SetRoundedShape(panelMyGames, 20);
            SetRoundedShape(pictureBoxMP, 20);
            SetRoundedShape(panelMyFollows, 20);
            SetRoundedShape(panel28, 15);
            SetRoundedShape(panel35, 15);
            for (int i = 0; i < 6; i++)
            {
                SetRoundedShape(PanelsFollow[i], 15);
                SetRoundedShape(PBFollows[i], 15);
            }
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

        private void buttonAbotMe_Click(object sender, EventArgs e)
        {
            if (buttonAbotMe.Text == "Отписаться")
            {
                using (SqlCommand cmd = new SqlCommand($"DELETE FROM Following WHERE HostID = '{Username}' AND FollowerID = '{SenderUsername}'", connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                buttonAbotMe.Text = "Подписаться";
            }
            else
            {
                using (SqlCommand cmd = new SqlCommand($"INSERT Following VALUES ('{Username}','{SenderUsername}')", connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                buttonAbotMe.Text = "Отписаться";
            }
        }

        private void ProfileForm_Load(object sender, EventArgs e)
        {
            GetTableByQuery($"SELECT * FROM Following WHERE HostID = '{Username}' AND FollowerID = '{SenderUsername}'");
            if (table.Rows.Count != 0)
            {
                buttonAbotMe.Text = "Отписаться";
            }
        }
    }
}