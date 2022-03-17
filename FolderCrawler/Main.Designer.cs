namespace FolderCrawler
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.panel1 = new System.Windows.Forms.Panel();
            this.directoryTextbox = new System.Windows.Forms.TextBox();
            this.dirButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.Controls.Add(this.directoryTextbox);
            this.panel1.Controls.Add(this.dirButton);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(361, 785);
            this.panel1.TabIndex = 0;
            // 
            // directoryTextbox
            // 
            this.directoryTextbox.AcceptsReturn = true;
            this.directoryTextbox.AcceptsTab = true;
            this.directoryTextbox.AllowDrop = true;
            this.directoryTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.directoryTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.directoryTextbox.Font = new System.Drawing.Font("Open Sans", 10.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.directoryTextbox.ForeColor = System.Drawing.Color.White;
            this.directoryTextbox.ImeMode = System.Windows.Forms.ImeMode.On;
            this.directoryTextbox.Location = new System.Drawing.Point(13, 320);
            this.directoryTextbox.MaxLength = 32;
            this.directoryTextbox.Name = "directoryTextbox";
            this.directoryTextbox.ReadOnly = true;
            this.directoryTextbox.Size = new System.Drawing.Size(335, 24);
            this.directoryTextbox.TabIndex = 4;
            this.directoryTextbox.TextChanged += new System.EventHandler(this.directoryTextbox_TextChanged);
            // 
            // dirButton
            // 
            this.dirButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dirButton.FlatAppearance.BorderSize = 0;
            this.dirButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dirButton.Font = new System.Drawing.Font("Open Sans", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dirButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dirButton.Image = ((System.Drawing.Image)(resources.GetObject("dirButton.Image")));
            this.dirButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.dirButton.Location = new System.Drawing.Point(13, 271);
            this.dirButton.Name = "dirButton";
            this.dirButton.Size = new System.Drawing.Size(335, 52);
            this.dirButton.TabIndex = 3;
            this.dirButton.TabStop = false;
            this.dirButton.Text = "Choose Directory";
            this.dirButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.dirButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.dirButton.UseVisualStyleBackColor = false;
            this.dirButton.Click += new System.EventHandler(this.dirButton_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Gray;
            this.panel2.Location = new System.Drawing.Point(13, 208);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(335, 10);
            this.panel2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Open Sans", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(13, 145);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 58);
            this.label1.TabIndex = 1;
            this.label1.Text = "Input";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(-19, -19);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(103, 20);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "radioButton1";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(1459, 785);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Main";
            this.Text = "  ";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Button dirButton;
        private System.Windows.Forms.TextBox directoryTextbox;
    }
}

