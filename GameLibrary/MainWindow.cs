﻿using System;
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

namespace GameLibrary
{
    public partial class MainWindow : Form
    {
        int UserID;
        SqlConnection connection;
        DataTable table;
        DataTable FullProductTable;
        Label[] gameLabelsName;
        Label[] gameLabelsPrice;
        PictureBox[] picBoxes;
        int currentPage = 0;
        int minGameID = 0;
        int maxGameID = 18;
        public List<int> cart = new List<int>();

        public MainWindow(int UserID, SqlConnection sqlCon)
        {
            InitializeComponent();
            this.UserID = UserID;
            connection = sqlCon;
            gameLabelsName = new[] { label2, label4, label6, label8, label10, label12, label14, label16, label18, label20, label22, label24, label26, label28, label30, label32, label34, label36};
            gameLabelsPrice = new[] { label1, label3, label5, label7, label9, label11, label13, label15, label17, label19, label21, label23, label25, label27, label29, label31, label33, label35,};
            picBoxes = new[] { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12, pictureBox13, pictureBox14, pictureBox15, pictureBox16, pictureBox17, pictureBox18 };

            GetProfile(); 
            CatalogLoad();
            GetFullTable();
            CartInfo();
        }

        private void GetProfile()
        {
            GetTableByQuery($"SELECT * FROM [User] INNER JOIN [Profile] ON ProfileID = [Profile].ID WHERE [User].ID = {UserID}");
            labelUserName.Text = table.Rows[0].Field<string>("Username");
            pictureBoxProfile.Image = ConvertByteArrayToImage(table.Rows[0].Field<byte[]>("Photo"));
            labelBalance.Text = "На счету: " + Math.Round(table.Rows[0].Field<decimal>("Balance"),2).ToString();
        }

        private void CartInfo()
        {
            labelCart.Text = $"Количество товаров: {cart.Count} \nНа сумму {Math.Round(GetCartSum(), 2).ToString()} рублей";
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
        public void Insert(byte[] image, int currentID)
        {
            using (SqlCommand cmd = new SqlCommand("Update Game Set Image = @image Where ID = @currentID", connection))
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
    }
}
