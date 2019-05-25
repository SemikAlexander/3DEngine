namespace _3DEngine
{
    partial class AddObject
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddObject));
            this.AddObj = new System.Windows.Forms.Button();
            this.ColorPicker = new System.Windows.Forms.ColorDialog();
            this.ScaleZ = new System.Windows.Forms.NumericUpDown();
            this.ScaleY = new System.Windows.Forms.NumericUpDown();
            this.PositionZ = new System.Windows.Forms.NumericUpDown();
            this.ScaleX = new System.Windows.Forms.NumericUpDown();
            this.PositionY = new System.Windows.Forms.NumericUpDown();
            this.ScaleLabel = new System.Windows.Forms.Label();
            this.PositionX = new System.Windows.Forms.NumericUpDown();
            this.ZLabel = new System.Windows.Forms.Label();
            this.YLabel = new System.Windows.Forms.Label();
            this.XLabel = new System.Windows.Forms.Label();
            this.PositionLabel = new System.Windows.Forms.Label();
            this.RotateZ = new System.Windows.Forms.NumericUpDown();
            this.RotateY = new System.Windows.Forms.NumericUpDown();
            this.RotateX = new System.Windows.Forms.NumericUpDown();
            this.RotateLabel = new System.Windows.Forms.Label();
            this.ObjectNameLabel = new System.Windows.Forms.Label();
            this.ObjectName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateX)).BeginInit();
            this.SuspendLayout();
            // 
            // AddObj
            // 
            this.AddObj.BackColor = System.Drawing.SystemColors.HotTrack;
            this.AddObj.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.AddObj.Enabled = false;
            this.AddObj.FlatAppearance.BorderSize = 0;
            this.AddObj.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddObj.Location = new System.Drawing.Point(0, 150);
            this.AddObj.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.AddObj.Name = "AddObj";
            this.AddObj.Size = new System.Drawing.Size(288, 32);
            this.AddObj.TabIndex = 69;
            this.AddObj.Text = "Добавить";
            this.AddObj.UseVisualStyleBackColor = false;
            this.AddObj.Click += new System.EventHandler(this.Button1_Click);
            // 
            // ColorPicker
            // 
            this.ColorPicker.AnyColor = true;
            this.ColorPicker.Color = System.Drawing.Color.LightGray;
            this.ColorPicker.FullOpen = true;
            // 
            // ScaleZ
            // 
            this.ScaleZ.BackColor = System.Drawing.SystemColors.HotTrack;
            this.ScaleZ.DecimalPlaces = 1;
            this.ScaleZ.ForeColor = System.Drawing.Color.White;
            this.ScaleZ.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ScaleZ.Location = new System.Drawing.Point(218, 116);
            this.ScaleZ.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ScaleZ.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            65536});
            this.ScaleZ.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ScaleZ.Name = "ScaleZ";
            this.ScaleZ.Size = new System.Drawing.Size(63, 23);
            this.ScaleZ.TabIndex = 104;
            this.ScaleZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ScaleZ.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ScaleY
            // 
            this.ScaleY.BackColor = System.Drawing.SystemColors.HotTrack;
            this.ScaleY.DecimalPlaces = 1;
            this.ScaleY.ForeColor = System.Drawing.Color.White;
            this.ScaleY.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ScaleY.Location = new System.Drawing.Point(149, 116);
            this.ScaleY.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ScaleY.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            65536});
            this.ScaleY.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ScaleY.Name = "ScaleY";
            this.ScaleY.Size = new System.Drawing.Size(63, 23);
            this.ScaleY.TabIndex = 103;
            this.ScaleY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ScaleY.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // PositionZ
            // 
            this.PositionZ.BackColor = System.Drawing.SystemColors.HotTrack;
            this.PositionZ.DecimalPlaces = 1;
            this.PositionZ.ForeColor = System.Drawing.Color.White;
            this.PositionZ.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.PositionZ.Location = new System.Drawing.Point(218, 54);
            this.PositionZ.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PositionZ.Name = "PositionZ";
            this.PositionZ.Size = new System.Drawing.Size(63, 23);
            this.PositionZ.TabIndex = 100;
            this.PositionZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ScaleX
            // 
            this.ScaleX.BackColor = System.Drawing.SystemColors.HotTrack;
            this.ScaleX.DecimalPlaces = 1;
            this.ScaleX.ForeColor = System.Drawing.Color.White;
            this.ScaleX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ScaleX.Location = new System.Drawing.Point(80, 116);
            this.ScaleX.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ScaleX.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            65536});
            this.ScaleX.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ScaleX.Name = "ScaleX";
            this.ScaleX.Size = new System.Drawing.Size(63, 23);
            this.ScaleX.TabIndex = 101;
            this.ScaleX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ScaleX.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // PositionY
            // 
            this.PositionY.BackColor = System.Drawing.SystemColors.HotTrack;
            this.PositionY.DecimalPlaces = 1;
            this.PositionY.ForeColor = System.Drawing.Color.White;
            this.PositionY.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.PositionY.Location = new System.Drawing.Point(149, 54);
            this.PositionY.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PositionY.Name = "PositionY";
            this.PositionY.Size = new System.Drawing.Size(63, 23);
            this.PositionY.TabIndex = 99;
            this.PositionY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ScaleLabel
            // 
            this.ScaleLabel.AutoSize = true;
            this.ScaleLabel.ForeColor = System.Drawing.Color.White;
            this.ScaleLabel.Location = new System.Drawing.Point(28, 118);
            this.ScaleLabel.Name = "ScaleLabel";
            this.ScaleLabel.Size = new System.Drawing.Size(46, 17);
            this.ScaleLabel.TabIndex = 102;
            this.ScaleLabel.Text = "Scale:";
            this.ScaleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PositionX
            // 
            this.PositionX.BackColor = System.Drawing.SystemColors.HotTrack;
            this.PositionX.DecimalPlaces = 1;
            this.PositionX.ForeColor = System.Drawing.Color.White;
            this.PositionX.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.PositionX.Location = new System.Drawing.Point(80, 54);
            this.PositionX.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PositionX.Name = "PositionX";
            this.PositionX.Size = new System.Drawing.Size(63, 23);
            this.PositionX.TabIndex = 98;
            this.PositionX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ZLabel
            // 
            this.ZLabel.AutoSize = true;
            this.ZLabel.ForeColor = System.Drawing.Color.White;
            this.ZLabel.Location = new System.Drawing.Point(242, 33);
            this.ZLabel.Name = "ZLabel";
            this.ZLabel.Size = new System.Drawing.Size(14, 17);
            this.ZLabel.TabIndex = 97;
            this.ZLabel.Text = "Z";
            this.ZLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // YLabel
            // 
            this.YLabel.AutoSize = true;
            this.YLabel.ForeColor = System.Drawing.Color.White;
            this.YLabel.Location = new System.Drawing.Point(170, 33);
            this.YLabel.Name = "YLabel";
            this.YLabel.Size = new System.Drawing.Size(15, 17);
            this.YLabel.TabIndex = 96;
            this.YLabel.Text = "Y";
            this.YLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // XLabel
            // 
            this.XLabel.AutoSize = true;
            this.XLabel.ForeColor = System.Drawing.Color.White;
            this.XLabel.Location = new System.Drawing.Point(97, 33);
            this.XLabel.Name = "XLabel";
            this.XLabel.Size = new System.Drawing.Size(16, 17);
            this.XLabel.TabIndex = 95;
            this.XLabel.Text = "X";
            this.XLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PositionLabel
            // 
            this.PositionLabel.AutoSize = true;
            this.PositionLabel.ForeColor = System.Drawing.Color.White;
            this.PositionLabel.Location = new System.Drawing.Point(12, 56);
            this.PositionLabel.Name = "PositionLabel";
            this.PositionLabel.Size = new System.Drawing.Size(62, 17);
            this.PositionLabel.TabIndex = 94;
            this.PositionLabel.Text = "Position:";
            this.PositionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // RotateZ
            // 
            this.RotateZ.BackColor = System.Drawing.SystemColors.HotTrack;
            this.RotateZ.ForeColor = System.Drawing.Color.White;
            this.RotateZ.Location = new System.Drawing.Point(218, 85);
            this.RotateZ.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RotateZ.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.RotateZ.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.RotateZ.Name = "RotateZ";
            this.RotateZ.Size = new System.Drawing.Size(63, 23);
            this.RotateZ.TabIndex = 93;
            this.RotateZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RotateY
            // 
            this.RotateY.BackColor = System.Drawing.SystemColors.HotTrack;
            this.RotateY.ForeColor = System.Drawing.Color.White;
            this.RotateY.Location = new System.Drawing.Point(149, 85);
            this.RotateY.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RotateY.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.RotateY.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.RotateY.Name = "RotateY";
            this.RotateY.Size = new System.Drawing.Size(63, 23);
            this.RotateY.TabIndex = 92;
            this.RotateY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RotateX
            // 
            this.RotateX.BackColor = System.Drawing.SystemColors.HotTrack;
            this.RotateX.ForeColor = System.Drawing.Color.White;
            this.RotateX.Location = new System.Drawing.Point(80, 85);
            this.RotateX.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.RotateX.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.RotateX.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.RotateX.Name = "RotateX";
            this.RotateX.Size = new System.Drawing.Size(63, 23);
            this.RotateX.TabIndex = 90;
            this.RotateX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RotateLabel
            // 
            this.RotateLabel.AutoSize = true;
            this.RotateLabel.ForeColor = System.Drawing.Color.White;
            this.RotateLabel.Location = new System.Drawing.Point(18, 87);
            this.RotateLabel.Name = "RotateLabel";
            this.RotateLabel.Size = new System.Drawing.Size(56, 17);
            this.RotateLabel.TabIndex = 91;
            this.RotateLabel.Text = "Rotate:";
            this.RotateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ObjectNameLabel
            // 
            this.ObjectNameLabel.AutoSize = true;
            this.ObjectNameLabel.ForeColor = System.Drawing.Color.White;
            this.ObjectNameLabel.Location = new System.Drawing.Point(12, 9);
            this.ObjectNameLabel.Name = "ObjectNameLabel";
            this.ObjectNameLabel.Size = new System.Drawing.Size(52, 17);
            this.ObjectNameLabel.TabIndex = 88;
            this.ObjectNameLabel.Text = "Name:";
            this.ObjectNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ObjectName
            // 
            this.ObjectName.Location = new System.Drawing.Point(70, 6);
            this.ObjectName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ObjectName.Name = "ObjectName";
            this.ObjectName.Size = new System.Drawing.Size(211, 23);
            this.ObjectName.TabIndex = 89;
            this.ObjectName.TextChanged += new System.EventHandler(this.ObjectName_TextChanged);
            // 
            // AddObject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GrayText;
            this.ClientSize = new System.Drawing.Size(288, 182);
            this.Controls.Add(this.ScaleZ);
            this.Controls.Add(this.ScaleY);
            this.Controls.Add(this.PositionZ);
            this.Controls.Add(this.ScaleX);
            this.Controls.Add(this.PositionY);
            this.Controls.Add(this.ScaleLabel);
            this.Controls.Add(this.PositionX);
            this.Controls.Add(this.ZLabel);
            this.Controls.Add(this.YLabel);
            this.Controls.Add(this.XLabel);
            this.Controls.Add(this.PositionLabel);
            this.Controls.Add(this.RotateZ);
            this.Controls.Add(this.RotateY);
            this.Controls.Add(this.RotateX);
            this.Controls.Add(this.RotateLabel);
            this.Controls.Add(this.ObjectNameLabel);
            this.Controls.Add(this.ObjectName);
            this.Controls.Add(this.AddObj);
            this.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "AddObject";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Добавить объект";
            ((System.ComponentModel.ISupportInitialize)(this.ScaleZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateX)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button AddObj;
        private System.Windows.Forms.ColorDialog ColorPicker;
        private System.Windows.Forms.NumericUpDown ScaleZ;
        private System.Windows.Forms.NumericUpDown ScaleY;
        private System.Windows.Forms.NumericUpDown PositionZ;
        private System.Windows.Forms.NumericUpDown ScaleX;
        private System.Windows.Forms.NumericUpDown PositionY;
        private System.Windows.Forms.Label ScaleLabel;
        private System.Windows.Forms.NumericUpDown PositionX;
        private System.Windows.Forms.Label ZLabel;
        private System.Windows.Forms.Label YLabel;
        private System.Windows.Forms.Label XLabel;
        private System.Windows.Forms.Label PositionLabel;
        private System.Windows.Forms.NumericUpDown RotateZ;
        private System.Windows.Forms.NumericUpDown RotateY;
        private System.Windows.Forms.NumericUpDown RotateX;
        private System.Windows.Forms.Label RotateLabel;
        private System.Windows.Forms.Label ObjectNameLabel;
        private System.Windows.Forms.TextBox ObjectName;
    }
}