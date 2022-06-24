namespace Life.Clients.WinForms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ButtonFirstGeneration = new System.Windows.Forms.Button();
            this.NUDSizeCell = new System.Windows.Forms.NumericUpDown();
            this.NUDDensity = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ButtonStop = new System.Windows.Forms.Button();
            this.ButtonStart = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.TimerTicks = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUDSizeCell)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUDDensity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel1.Controls.Add(this.ButtonStop);
            this.splitContainer1.Panel1.Controls.Add(this.ButtonStart);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer1.Size = new System.Drawing.Size(1170, 539);
            this.splitContainer1.SplitterDistance = 246;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(231, 170);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.ButtonFirstGeneration);
            this.tabPage1.Controls.Add(this.NUDSizeCell);
            this.tabPage1.Controls.Add(this.NUDDensity);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(223, 142);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Рандом";
            // 
            // ButtonFirstGeneration
            // 
            this.ButtonFirstGeneration.Location = new System.Drawing.Point(6, 84);
            this.ButtonFirstGeneration.Name = "ButtonFirstGeneration";
            this.ButtonFirstGeneration.Size = new System.Drawing.Size(211, 33);
            this.ButtonFirstGeneration.TabIndex = 5;
            this.ButtonFirstGeneration.Text = "Генерация первого поколения";
            this.ButtonFirstGeneration.UseVisualStyleBackColor = true;
            this.ButtonFirstGeneration.Click += new System.EventHandler(this.ButtonFirstGeneration_Click);
            // 
            // NUDSizeCell
            // 
            this.NUDSizeCell.Location = new System.Drawing.Point(170, 6);
            this.NUDSizeCell.Name = "NUDSizeCell";
            this.NUDSizeCell.Size = new System.Drawing.Size(47, 23);
            this.NUDSizeCell.TabIndex = 1;
            this.NUDSizeCell.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.NUDSizeCell.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // NUDDensity
            // 
            this.NUDDensity.Location = new System.Drawing.Point(170, 35);
            this.NUDDensity.Name = "NUDDensity";
            this.NUDDensity.Size = new System.Drawing.Size(47, 23);
            this.NUDDensity.TabIndex = 2;
            this.NUDDensity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.NUDDensity.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Плотность населения";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Масштаб";
            // 
            // ButtonStop
            // 
            this.ButtonStop.Location = new System.Drawing.Point(16, 256);
            this.ButtonStop.Name = "ButtonStop";
            this.ButtonStop.Size = new System.Drawing.Size(127, 36);
            this.ButtonStop.TabIndex = 5;
            this.ButtonStop.Text = "Стоп";
            this.ButtonStop.UseVisualStyleBackColor = true;
            this.ButtonStop.Click += new System.EventHandler(this.ButtonStop_Click);
            // 
            // ButtonStart
            // 
            this.ButtonStart.Location = new System.Drawing.Point(16, 205);
            this.ButtonStart.Name = "ButtonStart";
            this.ButtonStart.Size = new System.Drawing.Size(127, 36);
            this.ButtonStart.TabIndex = 0;
            this.ButtonStart.Text = "Старт";
            this.ButtonStart.UseVisualStyleBackColor = true;
            this.ButtonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(920, 539);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // TimerTicks
            // 
            this.TimerTicks.Interval = 10;
            this.TimerTicks.Tick += new System.EventHandler(this.TimerTickRate);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1170, 539);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Жизнь";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUDSizeCell)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUDDensity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private SplitContainer splitContainer1;
        private Button ButtonStart;
        private PictureBox pictureBox1;
        private Label label2;
        private Label label1;
        private NumericUpDown NUDDensity;
        private NumericUpDown NUDSizeCell;
        private Button ButtonStop;
        private System.Windows.Forms.Timer TimerTicks;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private Button ButtonFirstGeneration;
    }
}