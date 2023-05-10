namespace GameLibrary
{
    partial class ReviewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReviewForm));
            this.textBoxScore = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelGName = new System.Windows.Forms.Label();
            this.labelScore = new System.Windows.Forms.Label();
            this.textBoxReview = new System.Windows.Forms.TextBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxScore
            // 
            this.textBoxScore.Font = new System.Drawing.Font("Century Gothic", 24F);
            this.textBoxScore.Location = new System.Drawing.Point(308, 119);
            this.textBoxScore.Name = "textBoxScore";
            this.textBoxScore.Size = new System.Drawing.Size(60, 47);
            this.textBoxScore.TabIndex = 22;
            this.textBoxScore.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxScore.TextChanged += new System.EventHandler(this.textBoxScore_TextChanged);
            this.textBoxScore.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxScore_KeyPress);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.Controls.Add(this.labelGName);
            this.panel1.Location = new System.Drawing.Point(-4, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(476, 78);
            this.panel1.TabIndex = 23;
            // 
            // labelGName
            // 
            this.labelGName.AutoEllipsis = true;
            this.labelGName.BackColor = System.Drawing.Color.Transparent;
            this.labelGName.Font = new System.Drawing.Font("Century Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelGName.ForeColor = System.Drawing.Color.White;
            this.labelGName.Location = new System.Drawing.Point(23, 16);
            this.labelGName.Name = "labelGName";
            this.labelGName.Size = new System.Drawing.Size(431, 52);
            this.labelGName.TabIndex = 22;
            this.labelGName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelScore
            // 
            this.labelScore.AutoEllipsis = true;
            this.labelScore.BackColor = System.Drawing.Color.Transparent;
            this.labelScore.Font = new System.Drawing.Font("Century Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelScore.ForeColor = System.Drawing.Color.Black;
            this.labelScore.Location = new System.Drawing.Point(12, 122);
            this.labelScore.Name = "labelScore";
            this.labelScore.Size = new System.Drawing.Size(254, 57);
            this.labelScore.TabIndex = 21;
            this.labelScore.Text = "Ваша оценка:";
            // 
            // textBoxReview
            // 
            this.textBoxReview.Font = new System.Drawing.Font("Century Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxReview.Location = new System.Drawing.Point(19, 182);
            this.textBoxReview.Multiline = true;
            this.textBoxReview.Name = "textBoxReview";
            this.textBoxReview.Size = new System.Drawing.Size(431, 208);
            this.textBoxReview.TabIndex = 24;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAdd.Location = new System.Drawing.Point(19, 402);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(431, 35);
            this.buttonAdd.TabIndex = 25;
            this.buttonAdd.Text = "Добавить отзыв";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // label1
            // 
            this.label1.AutoEllipsis = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(374, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 57);
            this.label1.TabIndex = 26;
            this.label1.Text = "/ 10";
            // 
            // ReviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(158)))), ((int)(((byte)(140)))));
            this.ClientSize = new System.Drawing.Size(470, 448);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.textBoxReview);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBoxScore);
            this.Controls.Add(this.labelScore);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ReviewForm";
            this.Text = "GameBox";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxScore;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelGName;
        private System.Windows.Forms.Label labelScore;
        private System.Windows.Forms.TextBox textBoxReview;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Label label1;
    }
}