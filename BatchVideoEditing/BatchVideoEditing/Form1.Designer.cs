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
		private System.Windows.Forms.ProgressBar totalprogressBar;
		private System.Windows.Forms.Label totallblProgress;
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
			totalprogressBar = new ProgressBar();
			totallblProgress = new Label();
			label1 = new Label();
			flowLayoutPanelProgress = new FlowLayoutPanel();
			label2 = new Label();
			label3 = new Label();
			comboBox1 = new ComboBox();
			label4 = new Label();
			label5 = new Label();
			label6 = new Label();
			label7 = new Label();
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
			btnGoToPath.Click += btnGoToPath_Click;
			// 
			// btnClearAll
			// 
			btnClearAll.Cursor = Cursors.Hand;
			btnClearAll.Location = new Point(523, 106);
			btnClearAll.Name = "btnClearAll";
			btnClearAll.Size = new Size(150, 31);
			btnClearAll.TabIndex = 9;
			btnClearAll.Text = " Reset all";
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
			// totalprogressBar
			// 
			totalprogressBar.Location = new Point(703, 585);
			totalprogressBar.Name = "totalprogressBar";
			totalprogressBar.Size = new Size(586, 23);
			totalprogressBar.TabIndex = 2;
			// 
			// totallblProgress
			// 
			totallblProgress.AutoSize = true;
			totallblProgress.Location = new Point(703, 561);
			totallblProgress.Name = "totallblProgress";
			totallblProgress.Size = new Size(129, 20);
			totallblProgress.TabIndex = 1;
			totallblProgress.Text = "Total Progress: 0%";
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
			// label3
			// 
			label3.AutoSize = true;
			label3.ForeColor = SystemColors.AppWorkspace;
			label3.Location = new Point(12, 644);
			label3.Name = "label3";
			label3.Size = new Size(94, 20);
			label3.TabIndex = 11;
			label3.Text = "Version: 2.1.3";
			// 
			// comboBox1
			// 
			comboBox1.FormattingEnabled = true;
			comboBox1.Items.AddRange(new object[] { " Xử lý đồng thời", " Xử lý tuần tự" });
			comboBox1.Location = new Point(1138, 115);
			comboBox1.Name = "comboBox1";
			comboBox1.Size = new Size(151, 28);
			comboBox1.TabIndex = 12;
			comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
			label4.Location = new Point(35, 482);
			label4.Name = "label4";
			label4.Size = new Size(551, 19);
			label4.TabIndex = 13;
			label4.Text = "Xử lý tuần tự sẽ thích hợp với máy yếu, chậm nhưng không giới hạn số video import vào";
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
			label5.Location = new Point(35, 514);
			label5.Name = "label5";
			label5.Size = new Size(559, 19);
			label5.TabIndex = 14;
			label5.Text = "Xử lý đồng thời tốn nhiều tài nguyên tại 1 thời điểm hơn, đổi lại sẽ tiết kiệm thời gian hơn";
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
			label6.Location = new Point(35, 546);
			label6.Name = "label6";
			label6.Size = new Size(306, 19);
			label6.TabIndex = 15;
			label6.Text = "Xử lý đồng thời có thể xử lý tối đa 20 video 1 lúc";
			// 
			// label7
			// 
			label7.AutoSize = true;
			label7.Font = new Font("Segoe UI", 8F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point);
			label7.Location = new Point(35, 453);
			label7.Name = "label7";
			label7.Size = new Size(88, 19);
			label7.TabIndex = 16;
			label7.Text = "Tips && Trick";
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1331, 673);
			Controls.Add(label7);
			Controls.Add(label6);
			Controls.Add(label5);
			Controls.Add(label4);
			Controls.Add(comboBox1);
			Controls.Add(label3);
			Controls.Add(label2);
			Controls.Add(label1);
			Controls.Add(totallblProgress);
			Controls.Add(totalprogressBar);
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
			Load += Form1_Load;
			ResumeLayout(false);
			PerformLayout();
		}

		private Label label1;
		private Label label2;
		private Label label3;
		private ComboBox comboBox1;
		private Label label4;
		private Label label5;
		private Label label6;
		private Label label7;
	}
}
