namespace Task3
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
            this.comboDataType = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.zedGraph = new ZedGraph.ZedGraphControl();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // comboDataType
            // 
            this.comboDataType.FormattingEnabled = true;
            this.comboDataType.Location = new System.Drawing.Point(827, 333);
            this.comboDataType.Name = "comboDataType";
            this.comboDataType.Size = new System.Drawing.Size(235, 24);
            this.comboDataType.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(720, 424);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(151, 47);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Сохранить результаты";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(550, 424);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(155, 46);
            this.btnRun.TabIndex = 3;
            this.btnRun.Text = "Запустить тесты";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // zedGraph
            // 
            this.zedGraph.Location = new System.Drawing.Point(41, 13);
            this.zedGraph.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.zedGraph.Name = "zedGraph";
            this.zedGraph.ScrollGrace = 0D;
            this.zedGraph.ScrollMaxX = 0D;
            this.zedGraph.ScrollMaxY = 0D;
            this.zedGraph.ScrollMaxY2 = 0D;
            this.zedGraph.ScrollMinX = 0D;
            this.zedGraph.ScrollMinY = 0D;
            this.zedGraph.ScrollMinY2 = 0D;
            this.zedGraph.Size = new System.Drawing.Size(733, 384);
            this.zedGraph.TabIndex = 5;
            this.zedGraph.UseExtendedPrintDialog = true;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(41, 439);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(282, 31);
            this.progressBar.TabIndex = 6;
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(828, 53);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(232, 238);
            this.txtOutput.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1092, 505);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.zedGraph);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.comboDataType);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboDataType;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRun;
        private ZedGraph.ZedGraphControl zedGraph;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox txtOutput;
    }
}

