using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GameLibrary
{
    public partial class GameAddingForm : Form
    {
        SqlConnection connection;
        DataTable tableCategory;
        DataTable tableDeveloper;
        DataTable tableGame;
        int developerID;
        int categoryID;
        bool Win = false;
        bool Lin = false;
        bool Mac = false;


        public GameAddingForm(SqlConnection cn)
        {
            InitializeComponent();
            connection = cn;
        }

        private void GameAddingForm_Load(object sender, EventArgs e)
        {
            tableCategory = GetTableByQuery("SELECT * FROM Category");
            tableDeveloper = GetTableByQuery("SELECT * FROM Developer");

            for (int i = 0; i < tableCategory.Rows.Count; i++)
            {
                comboBoxCategory.Items.Add(tableCategory.Rows[i].Field<string>("Name"));
            }
            for (int i = 0; i < tableDeveloper.Rows.Count; i++)
            {
                comboBoxDeveloper.Items.Add(tableDeveloper.Rows[i].Field<string>("Name"));
            }
        }

        private DataTable GetTableByQuery(string sqlQ)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand(sqlQ, connection);
            DataTable table = new DataTable();

            adapter.SelectCommand = cmd;
            adapter.Fill(table);
            return
                table;
        }

        private void buttonPost_Click(object sender, EventArgs e)
        {
            if (myCheckBoxWindows.Checked)
                Win = true;
            if (myCheckBoxLinux.Checked)
                Lin = true;
            if (myCheckBoxMac.Checked)
                Mac = true;
            try
            {
                tableGame = GetTableByQuery("SELECT * FROM Game");
                using (SqlCommand cmd = new SqlCommand($"INSERT Game VALUES ({tableGame.Rows.Count}," +
                    $"'{textBoxName.Text}'," +
                    $"{developerID}," +
                    $"@image," +
                    $"{Convert.ToInt32(textBoxAge.Text)}," +
                    $"{categoryID}," +
                    $"{Convert.ToDecimal(textBoxPrice.Text)}," +
                    $"{Convert.ToInt32(textBoxUserScore.Text)}," +
                    $"{Convert.ToInt32(textBoxPressScore.Text)}," +
                    $"'{textBoxDesc.Text}'," +
                    $"'{textBoxSysReq.Text}'," +
                    $"'{dateTimePicker1.Value}'," +
                    $"{Convert.ToInt32(Win)}," +
                    $"{Convert.ToInt32(Lin)}," +
                    $"{Convert.ToInt32(Mac)}," +
                    $"NULL)", connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@image", ConvertImageToBytes(pictureBox1.Image));
                    cmd.ExecuteNonQuery();
                }
                this.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка при добавлении в каталог", "Внимание!");
            }            
        }

        private void comboBoxDeveloper_SelectedValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < tableDeveloper.Rows.Count; i++)
            {
                if (tableDeveloper.Rows[i].Field<string>("Name") == comboBoxDeveloper.Text)
                {
                    developerID = tableDeveloper.Rows[i].Field<int>("ID");
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

        private void buttonAddPhoto_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Image Files (*.png *.jpg *.jpeg) |*.png; *.jpg; *.jpeg", Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = System.Drawing.Image.FromFile(ofd.FileName);
                }
            }
        }

        private void comboBoxCategory_SelectedValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < tableCategory.Rows.Count; i++)
            {
                if (tableCategory.Rows[i].Field<string>("Name") == comboBoxCategory.Text)
                {
                    categoryID = tableCategory.Rows[i].Field<int>("ID");
                }
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
        }

        private void textBoxPressScore_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || (e.KeyChar == Convert.ToChar(",")) || e.KeyChar == '\b')
                return;
            else
                e.Handled = true;
        }
    }

    class MyCheckBox : CheckBox
    {
        public MyCheckBox()
        {
            this.TextAlign = ContentAlignment.MiddleRight;
        }
        public override bool AutoSize
        {
            get { return base.AutoSize; }
            set { base.AutoSize = false; }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            int h = this.ClientSize.Height - 2;
            Rectangle rc = new Rectangle(new Point(0, 1), new Size(h, h));
            ControlPaint.DrawCheckBox(e.Graphics, rc,
                this.Checked ? ButtonState.Checked : ButtonState.Normal);
        }
    }
}
