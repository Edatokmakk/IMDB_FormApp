namespace LAST
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lstBoxTitles = new System.Windows.Forms.ListBox();
            this.actorListBox = new System.Windows.Forms.ListBox();
            this.writerListBox = new System.Windows.Forms.ListBox();
            this.searchTxtBox = new System.Windows.Forms.TextBox();
            this.personNameTxtBox = new System.Windows.Forms.TextBox();
            this.jobTitleTxtBox = new System.Windows.Forms.TextBox();
            this.personPicBox = new System.Windows.Forms.PictureBox();
            this.picBox = new System.Windows.Forms.PictureBox();
            this.searchbtn = new System.Windows.Forms.Button();
            this.saveMovieBtn = new System.Windows.Forms.Button();
            this.directorListBox = new System.Windows.Forms.ListBox();
            this.descTxtBox = new System.Windows.Forms.TextBox();
            this.bioTxtBox = new System.Windows.Forms.TextBox();
            this.birthTxtBox = new System.Windows.Forms.TextBox();
            this.castListBox = new System.Windows.Forms.ListBox();
            this.rateTxtBox = new System.Windows.Forms.TextBox();
            this.yearTxtBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.genreLstBox = new System.Windows.Forms.ListBox();
            this.Genres = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.personPicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).BeginInit();
            this.SuspendLayout();
            // 
            // lstBoxTitles
            // 
            this.lstBoxTitles.FormattingEnabled = true;
            resources.ApplyResources(this.lstBoxTitles, "lstBoxTitles");
            this.lstBoxTitles.Name = "lstBoxTitles";
            this.lstBoxTitles.SelectedIndexChanged += new System.EventHandler(this.lstBoxTitles_SelectedIndexChanged);
            // 
            // actorListBox
            // 
            this.actorListBox.FormattingEnabled = true;
            resources.ApplyResources(this.actorListBox, "actorListBox");
            this.actorListBox.Name = "actorListBox";
            this.actorListBox.SelectedIndexChanged += new System.EventHandler(this.actorListBox_SelectedIndexChanged);
            // 
            // writerListBox
            // 
            this.writerListBox.FormattingEnabled = true;
            resources.ApplyResources(this.writerListBox, "writerListBox");
            this.writerListBox.Name = "writerListBox";
            this.writerListBox.SelectedIndexChanged += new System.EventHandler(this.writerListBox_SelectedIndexChanged);
            // 
            // searchTxtBox
            // 
            resources.ApplyResources(this.searchTxtBox, "searchTxtBox");
            this.searchTxtBox.Name = "searchTxtBox";
            this.searchTxtBox.TextChanged += new System.EventHandler(this.searchTxtBox_TextChanged);
            // 
            // personNameTxtBox
            // 
            resources.ApplyResources(this.personNameTxtBox, "personNameTxtBox");
            this.personNameTxtBox.Name = "personNameTxtBox";
            // 
            // jobTitleTxtBox
            // 
            resources.ApplyResources(this.jobTitleTxtBox, "jobTitleTxtBox");
            this.jobTitleTxtBox.Name = "jobTitleTxtBox";
            // 
            // personPicBox
            // 
            resources.ApplyResources(this.personPicBox, "personPicBox");
            this.personPicBox.Name = "personPicBox";
            this.personPicBox.TabStop = false;
            // 
            // picBox
            // 
            resources.ApplyResources(this.picBox, "picBox");
            this.picBox.Name = "picBox";
            this.picBox.TabStop = false;
            // 
            // searchbtn
            // 
            resources.ApplyResources(this.searchbtn, "searchbtn");
            this.searchbtn.Name = "searchbtn";
            this.searchbtn.UseVisualStyleBackColor = true;
            this.searchbtn.Click += new System.EventHandler(this.searchbtn_Click);
            // 
            // saveMovieBtn
            // 
            resources.ApplyResources(this.saveMovieBtn, "saveMovieBtn");
            this.saveMovieBtn.Name = "saveMovieBtn";
            this.saveMovieBtn.UseVisualStyleBackColor = true;
            this.saveMovieBtn.Click += new System.EventHandler(this.saveMovieBtn_Click);
            // 
            // directorListBox
            // 
            this.directorListBox.FormattingEnabled = true;
            resources.ApplyResources(this.directorListBox, "directorListBox");
            this.directorListBox.Name = "directorListBox";
            this.directorListBox.SelectedIndexChanged += new System.EventHandler(this.directorLstBox_SelectedIndexChanged);
            // 
            // descTxtBox
            // 
            resources.ApplyResources(this.descTxtBox, "descTxtBox");
            this.descTxtBox.Name = "descTxtBox";
            // 
            // bioTxtBox
            // 
            resources.ApplyResources(this.bioTxtBox, "bioTxtBox");
            this.bioTxtBox.Name = "bioTxtBox";
            // 
            // birthTxtBox
            // 
            resources.ApplyResources(this.birthTxtBox, "birthTxtBox");
            this.birthTxtBox.Name = "birthTxtBox";
            // 
            // castListBox
            // 
            this.castListBox.FormattingEnabled = true;
            resources.ApplyResources(this.castListBox, "castListBox");
            this.castListBox.Name = "castListBox";
            // 
            // rateTxtBox
            // 
            resources.ApplyResources(this.rateTxtBox, "rateTxtBox");
            this.rateTxtBox.Name = "rateTxtBox";
            // 
            // yearTxtBox
            // 
            resources.ApplyResources(this.yearTxtBox, "yearTxtBox");
            this.yearTxtBox.Name = "yearTxtBox";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // genreLstBox
            // 
            this.genreLstBox.FormattingEnabled = true;
            resources.ApplyResources(this.genreLstBox, "genreLstBox");
            this.genreLstBox.Name = "genreLstBox";
            // 
            // Genres
            // 
            resources.ApplyResources(this.Genres, "Genres");
            this.Genres.ForeColor = System.Drawing.Color.Red;
            this.Genres.Name = "Genres";
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Genres);
            this.Controls.Add(this.genreLstBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.yearTxtBox);
            this.Controls.Add(this.rateTxtBox);
            this.Controls.Add(this.castListBox);
            this.Controls.Add(this.birthTxtBox);
            this.Controls.Add(this.bioTxtBox);
            this.Controls.Add(this.descTxtBox);
            this.Controls.Add(this.directorListBox);
            this.Controls.Add(this.saveMovieBtn);
            this.Controls.Add(this.searchbtn);
            this.Controls.Add(this.picBox);
            this.Controls.Add(this.personPicBox);
            this.Controls.Add(this.jobTitleTxtBox);
            this.Controls.Add(this.personNameTxtBox);
            this.Controls.Add(this.searchTxtBox);
            this.Controls.Add(this.writerListBox);
            this.Controls.Add(this.actorListBox);
            this.Controls.Add(this.lstBoxTitles);
            this.Name = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.personPicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstBoxTitles;
        private System.Windows.Forms.ListBox actorListBox;
        private System.Windows.Forms.ListBox writerListBox;
        private System.Windows.Forms.TextBox searchTxtBox;
        private System.Windows.Forms.TextBox personNameTxtBox;
        private System.Windows.Forms.TextBox jobTitleTxtBox;
        private System.Windows.Forms.PictureBox personPicBox;
        private System.Windows.Forms.PictureBox picBox;
        private System.Windows.Forms.Button searchbtn;
        private System.Windows.Forms.Button saveMovieBtn;
        private System.Windows.Forms.ListBox directorListBox;
        private System.Windows.Forms.TextBox descTxtBox;
        private System.Windows.Forms.TextBox bioTxtBox;
        private System.Windows.Forms.TextBox birthTxtBox;
        private System.Windows.Forms.ListBox castListBox;
        private System.Windows.Forms.TextBox rateTxtBox;
        private System.Windows.Forms.TextBox yearTxtBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ListBox genreLstBox;
        private System.Windows.Forms.Label Genres;
    }
}

