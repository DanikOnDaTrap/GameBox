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

namespace GameLibrary
{
    public partial class MainWindow : Form
    {
        int UserID;
        SqlConnection connection;

        public MainWindow(int UserID, SqlConnection sqlCon)
        {
            InitializeComponent();
            this.UserID = UserID;
            connection = sqlCon;
        }
    }
}
