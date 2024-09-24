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
			SuspendLayout();
			// 
			// btnImport
			// 
			btnImport.Location = new Point(27, 77);
			btnImport.Name = "btnImport";
			btnImport.Size = new Size(150, 32);
			btnImport.TabIndex = 6;
			btnImport.Text = "Import Videos";
			btnImport.UseVisualStyleBackColor = true;
			btnImport.Click += btnImport_Click;
			// 
			// btnProcess
			// 
			btnProcess.Location = new Point(515, 315);
			btnProcess.Name = "btnProcess";
			btnProcess.Size = new Size(150, 31);
			btnProcess.TabIndex = 5;
			btnProcess.Text = "Chỉnh sửa ngay";
			btnProcess.UseVisualStyleBackColor = true;
			btnProcess.Click += btnProcess_Click;
			// 
			// btnGoToPath
			// 
			btnGoToPath.Location = new Point(27, 483);
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
			btnClearAll.Location = new Point(27, 315);
			btnClearAll.Name = "btnClearAll";
			btnClearAll.Size = new Size(150, 31);
			btnClearAll.TabIndex = 9;
			btnClearAll.Text = "Xóa tất cả";
			btnClearAll.UseVisualStyleBackColor = true;
			btnClearAll.Click += btnClearAll_Click;
			// 
			// lstVideos
			// 
			lstVideos.FormattingEnabled = true;
			lstVideos.ItemHeight = 20;
			lstVideos.Location = new Point(27, 115);
			lstVideos.Name = "lstVideos";
			lstVideos.Size = new Size(638, 184);
			lstVideos.TabIndex = 3;
			// 
			// progressBar
			// 
			progressBar.Location = new Point(27, 365);
			progressBar.Name = "progressBar";
			progressBar.Size = new Size(350, 23);
			progressBar.TabIndex = 2;
			// 
			// lblProgress
			// 
			lblProgress.AutoSize = true;
			lblProgress.Location = new Point(27, 405);
			lblProgress.Name = "lblProgress";
			lblProgress.Size = new Size(129, 20);
			lblProgress.TabIndex = 1;
			lblProgress.Text = "Total Progress: 0%";
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point);
			label1.ForeColor = SystemColors.ActiveCaptionText;
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
			flowLayoutPanelProgress.Location = new Point(703, 115);
			flowLayoutPanelProgress.Name = "flowLayoutPanelProgress";
			flowLayoutPanelProgress.Size = new Size(586, 523);
			flowLayoutPanelProgress.TabIndex = 8;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1331, 673);
			Controls.Add(label1);
			Controls.Add(lblProgress);
			Controls.Add(progressBar);
			Controls.Add(lstVideos);
			Controls.Add(btnProcess);
			Controls.Add(btnImport);
			Controls.Add(btnGoToPath);
			Controls.Add(flowLayoutPanelProgress);
			Controls.Add(btnClearAll);
			Name = "Form1";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Batch video editing";
			ResumeLayout(false);
			PerformLayout();
		}

		private Label label1;
	}
}
