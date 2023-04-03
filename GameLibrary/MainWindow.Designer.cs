namespace GameLibrary
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageStore = new System.Windows.Forms.TabPage();
            this.tabPageCollection = new System.Windows.Forms.TabPage();
            this.tabPageCommunity = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageStore);
            this.tabControl1.Controls.Add(this.tabPageCollection);
            this.tabControl1.Controls.Add(this.tabPageCommunity);
            this.tabControl1.Font = new System.Drawing.Font("Century Gothic", 14.25F);
            this.tabControl1.Location = new System.Drawing.Point(1, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1530, 948);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageStore
            // 
            this.tabPageStore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(158)))), ((int)(((byte)(140)))));
            this.tabPageStore.Location = new System.Drawing.Point(4, 31);
            this.tabPageStore.Name = "tabPageStore";
            this.tabPageStore.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageStore.Size = new System.Drawing.Size(1522, 913);
            this.tabPageStore.TabIndex = 0;
            this.tabPageStore.Text = "Магазин";
            // 
            // tabPageCollection
            // 
            this.tabPageCollection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(158)))), ((int)(((byte)(140)))));
            this.tabPageCollection.Location = new System.Drawing.Point(4, 31);
            this.tabPageCollection.Name = "tabPageCollection";
            this.tabPageCollection.Size = new System.Drawing.Size(1522, 913);
            this.tabPageCollection.TabIndex = 1;
            this.tabPageCollection.Text = "Мои игры";
            // 
            // tabPageCommunity
            // 
            this.tabPageCommunity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(158)))), ((int)(((byte)(140)))));
            this.tabPageCommunity.Location = new System.Drawing.Point(4, 31);
            this.tabPageCommunity.Name = "tabPageCommunity";
            this.tabPageCommunity.Size = new System.Drawing.Size(1522, 913);
            this.tabPageCommunity.TabIndex = 2;
            this.tabPageCommunity.Text = "Сообщество";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(158)))), ((int)(((byte)(140)))));
            this.ClientSize = new System.Drawing.Size(1529, 942);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "GameBox";
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageStore;
        private System.Windows.Forms.TabPage tabPageCollection;
        private System.Windows.Forms.TabPage tabPageCommunity;
    }
}