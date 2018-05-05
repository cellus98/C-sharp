namespace VideoLibrary
{
    partial class VideoLibrary
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoLibrary));
            this.DisplayAll = new System.Windows.Forms.Button();
            this.Remove = new System.Windows.Forms.Button();
            this.Search = new System.Windows.Forms.Button();
            this.Update = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // DisplayAll
            // 
            this.DisplayAll.BackColor = System.Drawing.Color.LightGray;
            this.DisplayAll.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.DisplayAll.Location = new System.Drawing.Point(57, 281);
            this.DisplayAll.Name = "DisplayAll";
            this.DisplayAll.Size = new System.Drawing.Size(143, 50);
            this.DisplayAll.TabIndex = 0;
            this.DisplayAll.Text = "Display All";
            this.DisplayAll.UseVisualStyleBackColor = false;
            this.DisplayAll.Click += new System.EventHandler(this.DisplayAll_Click);
            // 
            // Remove
            // 
            this.Remove.BackColor = System.Drawing.Color.LightGray;
            this.Remove.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Remove.Location = new System.Drawing.Point(57, 337);
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(143, 49);
            this.Remove.TabIndex = 1;
            this.Remove.Text = "Remove";
            this.Remove.UseVisualStyleBackColor = false;
            this.Remove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // Search
            // 
            this.Search.BackColor = System.Drawing.Color.LightGray;
            this.Search.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Search.Location = new System.Drawing.Point(300, 281);
            this.Search.Name = "Search";
            this.Search.Size = new System.Drawing.Size(143, 50);
            this.Search.TabIndex = 2;
            this.Search.Text = "Search";
            this.Search.UseVisualStyleBackColor = false;
            this.Search.Click += new System.EventHandler(this.Search_Click);
            // 
            // Update
            // 
            this.Update.BackColor = System.Drawing.Color.LightGray;
            this.Update.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Update.Location = new System.Drawing.Point(300, 337);
            this.Update.Name = "Update";
            this.Update.Size = new System.Drawing.Size(143, 48);
            this.Update.TabIndex = 3;
            this.Update.Text = "Update";
            this.Update.UseVisualStyleBackColor = false;
            this.Update.Click += new System.EventHandler(this.Update_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(57, 33);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(344, 166);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // VideoLibrary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.ClientSize = new System.Drawing.Size(505, 418);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Update);
            this.Controls.Add(this.Search);
            this.Controls.Add(this.Remove);
            this.Controls.Add(this.DisplayAll);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "VideoLibrary";
            this.Text = "Video Library";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button DisplayAll;
        private System.Windows.Forms.Button Remove;
        private System.Windows.Forms.Button Search;
        private System.Windows.Forms.Button Update;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

