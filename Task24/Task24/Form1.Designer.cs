namespace Task24
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.zedPut = new ZedGraph.ZedGraphControl();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.zedGet = new ZedGraph.ZedGraphControl();
            this.zedRemove = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // zedPut
            // 
            this.zedPut.Location = new System.Drawing.Point(53, 33);
            this.zedPut.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.zedPut.Name = "zedPut";
            this.zedPut.ScrollGrace = 0D;
            this.zedPut.ScrollMaxX = 0D;
            this.zedPut.ScrollMaxY = 0D;
            this.zedPut.ScrollMaxY2 = 0D;
            this.zedPut.ScrollMinX = 0D;
            this.zedPut.ScrollMinY = 0D;
            this.zedPut.ScrollMinY2 = 0D;
            this.zedPut.Size = new System.Drawing.Size(427, 532);
            this.zedPut.TabIndex = 0;
            this.zedPut.UseExtendedPrintDialog = true;
            this.zedPut.Load += new System.EventHandler(this.zedGraphControl1_Load);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(686, 585);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(183, 32);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click_1);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(896, 593);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(57, 16);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Process";
            // 
            // zedGet
            // 
            this.zedGet.Location = new System.Drawing.Point(549, 33);
            this.zedGet.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.zedGet.Name = "zedGet";
            this.zedGet.ScrollGrace = 0D;
            this.zedGet.ScrollMaxX = 0D;
            this.zedGet.ScrollMaxY = 0D;
            this.zedGet.ScrollMaxY2 = 0D;
            this.zedGet.ScrollMinX = 0D;
            this.zedGet.ScrollMinY = 0D;
            this.zedGet.ScrollMinY2 = 0D;
            this.zedGet.Size = new System.Drawing.Size(438, 531);
            this.zedGet.TabIndex = 3;
            this.zedGet.UseExtendedPrintDialog = true;
            // 
            // zedRemove
            // 
            this.zedRemove.Location = new System.Drawing.Point(1043, 33);
            this.zedRemove.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.zedRemove.Name = "zedRemove";
            this.zedRemove.ScrollGrace = 0D;
            this.zedRemove.ScrollMaxX = 0D;
            this.zedRemove.ScrollMaxY = 0D;
            this.zedRemove.ScrollMaxY2 = 0D;
            this.zedRemove.ScrollMinX = 0D;
            this.zedRemove.ScrollMinY = 0D;
            this.zedRemove.ScrollMinY2 = 0D;
            this.zedRemove.Size = new System.Drawing.Size(427, 532);
            this.zedRemove.TabIndex = 4;
            this.zedRemove.UseExtendedPrintDialog = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1523, 681);
            this.Controls.Add(this.zedRemove);
            this.Controls.Add(this.zedGet);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.zedPut);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ZedGraph.ZedGraphControl zedPut;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblStatus;
        private ZedGraph.ZedGraphControl zedGet;
        private ZedGraph.ZedGraphControl zedRemove;
    }
}

