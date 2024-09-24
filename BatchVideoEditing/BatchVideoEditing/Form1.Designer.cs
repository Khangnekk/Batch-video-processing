namespace BatchVideoEditing
{
	partial class Form1
	{
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Button btnImport;
		private System.Windows.Forms.Button btnProcess;
		private System.Windows.Forms.Button btnGoToPath;
		private System.Windows.Forms.Button btnClearAll;
		private System.Windows.Forms.ListBox lstVideos;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Label lblProgress;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelProgress;  
		// Dispose method
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		// InitializeComponent
		private void InitializeComponent()
		{
			btnImport = new Button();
			btnProcess = new Button();
			btnGoToPath = new Button();
			btnClearAll = new Button();
			lstVideos = new ListBox();
			progressBar = new ProgressBar();
			lblProgress = new Label();
			label1 = new Label();
			flowLayoutPanelProgress = new FlowLayoutPanel();
			label2 = new Label();
			SuspendLayout();
			// 
			// btnImport
			// 
			btnImport.Cursor = Cursors.Hand;
			btnImport.Location = new Point(35, 106);
			btnImport.Name = "btnImport";
			btnImport.Size = new Size(150, 32);
			btnImport.TabIndex = 6;
			btnImport.Text = "Import Videos";
			btnImport.UseVisualStyleBackColor = true;
			btnImport.Click += btnImport_Click;
			// 
			// btnProcess
			// 
			btnProcess.Cursor = Cursors.Hand;
			btnProcess.Location = new Point(523, 363);
			btnProcess.Name = "btnProcess";
			btnProcess.Size = new Size(150, 31);
			btnProcess.TabIndex = 5;
			btnProcess.Text = "  Chỉnh sửa ngay";
			btnProcess.TextAlign = ContentAlignment.MiddleLeft;
			btnProcess.UseVisualStyleBackColor = true;
			btnProcess.Click += btnProcess_Click;
			// 
			// btnGoToPath
			// 
			btnGoToPath.Cursor = Cursors.Hand;
			btnGoToPath.Location = new Point(1139, 629);
			btnGoToPath.Name = "btnGoToPath";
			btnGoToPath.Size = new Size(150, 32);
			btnGoToPath.TabIndex = 7;
			btnGoToPath.Text = "Xem kết quả";
			btnGoToPath.UseVisualStyleBackColor = true;
			btnGoToPath.Visible = false;
			btnGoToPath.Click += btnGoToPath_Click;
			// 
			// btnClearAll
			// 
			btnClearAll.Cursor = Cursors.Hand;
			btnClearAll.Location = new Point(35, 363);
			btnClearAll.Name = "btnClearAll";
			btnClearAll.Size = new Size(150, 31);
			btnClearAll.TabIndex = 9;
			btnClearAll.Text = "  Clear All";
			btnClearAll.TextAlign = ContentAlignment.MiddleLeft;
			btnClearAll.UseVisualStyleBackColor = true;
			btnClearAll.Click += btnClearAll_Click;
			// 
			// lstVideos
			// 
			lstVideos.FormattingEnabled = true;
			lstVideos.ItemHeight = 20;
			lstVideos.Location = new Point(35, 153);
			lstVideos.Name = "lstVideos";
			lstVideos.Size = new Size(638, 184);
			lstVideos.TabIndex = 3;
			// 
			// progressBar
			// 
			progressBar.Location = new Point(703, 585);
			progressBar.Name = "progressBar";
			progressBar.Size = new Size(586, 23);
			progressBar.TabIndex = 2;
			// 
			// lblProgress
			// 
			lblProgress.AutoSize = true;
			lblProgress.Location = new Point(703, 561);
			lblProgress.Name = "lblProgress";
			lblProgress.Size = new Size(129, 20);
			lblProgress.TabIndex = 1;
			lblProgress.Text = "Total Progress: 0%";
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point);
			label1.ForeColor = SystemColors.GrayText;
			label1.Location = new Point(351, 9);
			label1.Name = "label1";
			label1.Size = new Size(607, 46);
			label1.TabIndex = 0;
			label1.Text = "Phần mềm chỉnh sửa hàng loạt video";
			// 
			// flowLayoutPanelProgress
			// 
			flowLayoutPanelProgress.AutoScroll = true;
			flowLayoutPanelProgress.BackColor = SystemColors.ButtonHighlight;
			flowLayoutPanelProgress.Location = new Point(703, 153);
			flowLayoutPanelProgress.Name = "flowLayoutPanelProgress";
			flowLayoutPanelProgress.Size = new Size(586, 389);
			flowLayoutPanelProgress.TabIndex = 8;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(703, 118);
			label2.Name = "label2";
			label2.Size = new Size(311, 20);
			label2.TabIndex = 10;
			label2.Text = "(*) Những video được xử lý sẽ xuất hiện ở đây";
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1331, 673);
			Controls.Add(label2);
			Controls.Add(label1);
			Controls.Add(lblProgress);
			Controls.Add(progressBar);
			Controls.Add(lstVideos);
			Controls.Add(btnProcess);
			Controls.Add(btnImport);
			Controls.Add(btnGoToPath);
			Controls.Add(flowLayoutPanelProgress);
			Controls.Add(btnClearAll);
			MaximizeBox = false;
			Name = "Form1";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Batch video editing";
			ResumeLayout(false);
			PerformLayout();
		}

		private Label label1;
		private Label label2;
	}
}
