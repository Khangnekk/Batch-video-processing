using System.Diagnostics;
using System.Text;

namespace BatchVideoEditing
{
	public partial class Form1 : Form
	{
		private List<string> videoPaths;
		private string audioFolderPath = @"C:\path\to\audio"; // Thay bằng đường dẫn tới folder chứa file âm thanh
		private string outputFolderPath = "output";
		private int type = 0;
		private int totalVideoImportAsync = 0;
		private int completeVideoAsync = 0;

		public Form1()
		{
			InitializeComponent();
			videoPaths = new List<string>();
		}

		private void btnImport_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog ofd = new OpenFileDialog())
			{
				ofd.Multiselect = true;
				ofd.Filter = "Video Files|*.mp4;*.avi;*.mov";
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					videoPaths.AddRange(ofd.FileNames);
					lstVideos.Items.AddRange(ofd.SafeFileNames);
				}
			}
		}

		private async void btnProcess_Click(object sender, EventArgs e)
		{
			totalprogressBar.Value = 0;
			totallblProgress.Text = "Total Progress: 0%";

			if (videoPaths.Count == 0)
			{
				MessageBox.Show("Please import videos first.");
				return;
			}
			btnProcess.Enabled = false;
			comboBox1.Enabled = false;
			btnImport.Enabled = false;
			switch (type)
			{
				case 0:
					// xử lý đồng thời
					await ProcessVideosAsync(videoPaths);
					break;
				case 1:
					// xử lý tuần tự
					await ProcessVideosAsyncType1(videoPaths);
					break;
				default:
					break;
			}

			totalprogressBar.Value = 100;
			totallblProgress.Text = "Total Progress: 100%";
			btnGoToPath.Visible = true;
			btnProcess.Enabled = true;
			comboBox1.Enabled = true;
			btnImport.Enabled = true;
			lstVideos.Items.Clear();
		}

		private async Task ProcessVideosAsync(List<string> videos)
		{
			int totalVideos = videos.Count;
			totalVideoImportAsync = totalVideos;
			int completedVideos = 0;
			int maxDegreeOfParallelism = 20; // Giới hạn số lượng video xử lý cùng lúc, bạn có thể điều chỉnh số này
			using (SemaphoreSlim semaphore = new SemaphoreSlim(maxDegreeOfParallelism))
			{
				var tasks = new List<Task>();

				foreach (var video in videos)
				{
					await semaphore.WaitAsync(); // Đợi cho đến khi có tài nguyên trống

					// Xử lý video trong Task.Run để không làm nghẽn UI thread
					var videoProcessingTask = Task.Run(async () =>
					{
						try
						{
							int i = videos.IndexOf(video) + 1;
							ProgressBar progressBarVideo = new ProgressBar { Maximum = 100, Width = 500 };
							Label labelVideo = new Label { Text = $"{Path.GetFileName(video)}: 0%", Width = 500 };

							flowLayoutPanelProgress.Invoke(new Action(() =>
							{
								flowLayoutPanelProgress.Controls.Add(labelVideo);
								flowLayoutPanelProgress.Controls.Add(progressBarVideo);
							}));

							await ProcessVideoAsync(video, progressBarVideo, labelVideo, i);

							// Cập nhật tiến độ tổng sau khi hoàn thành từng video
							Interlocked.Increment(ref completedVideos); // Tăng completedVideos một cách an toàn trong môi trường đa luồng
							int totalProgressPercentage = (completedVideos * 100) / totalVideos;

							// Cập nhật tổng thanh tiến độ và nhãn
							totalprogressBar.Invoke(new Action(() =>
							{
								totalprogressBar.Value = totalProgressPercentage;
								totallblProgress.Text = $"Total Progress: {totalProgressPercentage}%";
							}));
						}
						finally
						{
							semaphore.Release(); // Giải phóng tài nguyên để task khác có thể chạy
						}
					});

					tasks.Add(videoProcessingTask);
				}

				// Chờ tất cả các video xử lý xong
				await Task.WhenAll(tasks);
			}
		}

		private async Task ProcessVideosAsyncType1(List<string> videos)
		{
			int totalVideos = videos.Count;
			int completedVideos = 0;

			foreach (var video in videos)
			{
				int i = videos.IndexOf(video) + 1;
				ProgressBar progressBarVideo = new ProgressBar { Maximum = 100, Width = 500 };
				Label labelVideo = new Label { Text = $"{Path.GetFileName(video)}: 0%", Width = 500 };

				flowLayoutPanelProgress.Invoke(new Action(() =>
				{
					flowLayoutPanelProgress.Controls.Add(labelVideo);
					flowLayoutPanelProgress.Controls.Add(progressBarVideo);
				}));

				// Process each video sequentially
				await ProcessVideoAsyncType1(video, progressBarVideo, labelVideo, i);

				// Cập nhật tổng tiến độ sau khi hoàn thành từng video
				completedVideos++;
				int totalProgressPercentage = (completedVideos * 100) / totalVideos;

				// Cập nhật tổng thanh tiến độ và nhãn
				totalprogressBar.Value = totalProgressPercentage;
				totallblProgress.Text = $"Total Progress: {totalProgressPercentage}%";
			}
		}


		private async Task ProcessVideoAsyncType1(string inputPath, ProgressBar progressBar, Label label, int indexVideo)
		{
			try
			{
				string outputPath = Path.Combine(outputFolderPath, Path.GetFileNameWithoutExtension(inputPath) + "_processed.mp4");

				// Perform video processing synchronously
				await Task.Run(() => ProcessVideo(inputPath, outputPath, progressBar, label, indexVideo));
			}
			catch (Exception ex)
			{
				this.Invoke(new Action(() =>
				{
					// Show error message if there is an error processing the video
					MessageBox.Show($"Error processing {Path.GetFileName(inputPath)}: {ex.Message}");
				}));
			}
		}

		private async Task ProcessVideoAsync(string inputPath, ProgressBar progressBar, Label label, int i)
		{
			try
			{
				string outputPath = Path.Combine(outputFolderPath, Path.GetFileNameWithoutExtension(inputPath) + i + "_processed.mp4");

				// Xử lý video không đồng bộ
				await Task.Run(() => ProcessVideo(inputPath, outputPath, progressBar, label, i));
			}
			catch (Exception ex)
			{
				this.Invoke(new Action(() =>
				{
					// Hiển thị thông báo lỗi nếu có lỗi khi xử lý video
					MessageBox.Show($"Error processing {Path.GetFileName(inputPath)}: {ex.Message}");
				}));
			}
		}




		private void UpdateVideoProgress(ProgressBar progressBar, Label label, int progressPercentage)
		{
			if (progressBar.InvokeRequired || label.InvokeRequired)
			{
				progressBar.Invoke(new Action(() => progressBar.Value = progressPercentage));
				label.Invoke(new Action(() => label.Text = $"{label.Text.Split(':')[0]}: {progressPercentage}%"));
			}
			else
			{
				progressBar.Value = progressPercentage;
				label.Text = $"{label.Text.Split(':')[0]}: {progressPercentage}%";
			}
		}

		private void ProcessVideo(string inputPath, string outputPath, ProgressBar progressBar, Label label, int videoIndex)
		{
			UpdateVideoProgress(progressBar, label, 10);
			// Đảm bảo thư mục đầu ra tồn tại
			if (!Directory.Exists(outputFolderPath))
				Directory.CreateDirectory(outputFolderPath);

			// Tạo thư mục temp và final nếu chưa tồn tại
			string tempFolderPath = Path.Combine(outputFolderPath, "temp");
			string finalFolderPath = Path.Combine(outputFolderPath, "final");
			Directory.CreateDirectory(tempFolderPath);
			Directory.CreateDirectory(finalFolderPath);

			// 1. Crop video lại sao cho chiều dài giảm xuống còn 80% và thêm nền với hiệu ứng blur
			string cropAndBlurPath = Path.Combine(tempFolderPath, "cropped_blurred_" + videoIndex + "_" + Path.GetFileName(inputPath));
			CropAndBlurVideo(inputPath, cropAndBlurPath, videoIndex);
			UpdateVideoProgress(progressBar, label, 20); // Cập nhật tiến độ sau khi hoàn thành bước 1

			// 2. Cắt video đã chỉnh sửa thành 5 phần
			List<string> splitVideos = SplitVideoIntoParts(cropAndBlurPath, 5);
			UpdateVideoProgress(progressBar, label, 40); // Cập nhật tiến độ sau khi hoàn thành bước 2

			// 3. Horizontal flip cho các phần 1, 3, 5
			for (int i = 0; i < splitVideos.Count; i++)
			{
				if (i % 2 == 0)
				{
					string flippedPath = Path.Combine(tempFolderPath, "flipped_" + videoIndex + Path.GetFileName(splitVideos[i]));
					FlipHorizontal(splitVideos[i], flippedPath);
					splitVideos[i] = flippedPath; // Cập nhật đường dẫn sau khi flip
				}
			}
			UpdateVideoProgress(progressBar, label, 60); // Cập nhật tiến độ sau khi hoàn thành bước 3

			// 4. Ghép các phần video lại thành 1 video trong temp
			string combinedPath = Path.Combine(tempFolderPath, "combined_" + videoIndex + Path.GetFileNameWithoutExtension(inputPath) + ".mp4");
			CombineVideos(splitVideos, combinedPath, videoIndex);
			UpdateVideoProgress(progressBar, label, 80); // Cập nhật tiến độ sau khi hoàn thành bước 4

			// 5. Chèn text vào video và lưu vào thư mục final
			string overlayOutputPath = Path.Combine(finalFolderPath, "overlaytext_" + videoIndex + Path.GetFileNameWithoutExtension(inputPath) + ".mp4");
			OverlayText(combinedPath, Path.GetFileNameWithoutExtension(inputPath), overlayOutputPath);
			UpdateVideoProgress(progressBar, label, 95);

			// 6. Tăng sáng video lên 15% và lưu với tên gọi là tên ban đầu + "_final.mp4"
			string finalOutputPath = Path.Combine(finalFolderPath, Path.GetFileNameWithoutExtension(inputPath) + videoIndex + "_final.mp4");
			BrightenVideo(overlayOutputPath, finalOutputPath);
			UpdateVideoProgress(progressBar, label, 100); // Cập nhật tiến độ sau khi hoàn thành bước 6

			if (type == 0)
			{
				++completeVideoAsync;
				int totalProgressPercentage = (completeVideoAsync * 100) / totalVideoImportAsync;
				totalprogressBar.Value = totalProgressPercentage;
				totallblProgress.Text = $"Total Progress: {totalProgressPercentage}%";
			}

			// 7.Xóa tệp overlayOutputPath
			if (File.Exists(overlayOutputPath))
			{
				File.Delete(overlayOutputPath);
			}

			// 8. Xóa mọi thứ trong thư mục temp sau khi hoàn thành
			foreach (string file in Directory.GetFiles(tempFolderPath))
			{
				if (File.Exists(file))
					File.Delete(file);
			}

			// 9. Nếu bạn cũng muốn xóa các thư mục con (nếu có), bạn có thể sử dụng
			foreach (string dir in Directory.GetDirectories(tempFolderPath))
			{
				Directory.Delete(dir, true);
			}
		}

		private void BrightenVideo(string inputPath, string outputPath)
		{
			string brightenCommand = $"-i \"{inputPath}\" -vf \"eq=brightness=0.10\" -c:a copy \"{outputPath}\"";
			ExecuteFFmpegCommand(brightenCommand);
		}

		// Cập nhật CropAndBlurVideo để chỉ lưu file tạm trong temp
		private void CropAndBlurVideo(string inputPath, string outputPath, int videoIndex)
		{
			// Tạo video background với hiệu ứng blur
			string blurredBackgroundPath = Path.Combine(outputFolderPath, "temp", "blurred_background" + videoIndex + ".mp4");
			string blurCommand = $"-i \"{inputPath}\" -vf \"scale=iw:ih,boxblur=50:1\" -c:a copy \"{blurredBackgroundPath}\"";
			ExecuteFFmpegCommand(blurCommand);

			// Cắt video lại còn 80% chiều dài
			string cropPath = Path.Combine(outputFolderPath, "temp", "cropped_" + videoIndex + Path.GetFileName(inputPath));
			string cropCommand = $"-i \"{inputPath}\" -vf \"crop=iw:ih*0.9\" -c:a copy \"{cropPath}\"";
			ExecuteFFmpegCommand(cropCommand);

			// Ghép video đã cắt với video background và lưu trong temp
			string finalOutputCommand = $"-i \"{blurredBackgroundPath}\" -i \"{cropPath}\" -filter_complex \"[0:v][1:v] overlay=(W-w)/2:(H-h)/2\" -c:a copy \"{outputPath}\"";
			ExecuteFFmpegCommand(finalOutputCommand);
		}

		private void FlipHorizontal(string videoPath, string outputPath)
		{
			string flipCommand = $"-i \"{videoPath}\" -vf \"hflip\" -c:a copy \"{outputPath}\"";
			ExecuteFFmpegCommand(flipCommand);
		}


		private List<string> SplitVideoIntoParts(string videoPath, int parts)
		{
			List<string> splitParts = new List<string>();
			double duration = GetVideoDuration(videoPath);

			// Tính toán thời lượng cho mỗi phần
			double durationPart = duration / parts;

			for (int i = 0; i < parts; i++)
			{
				string partPath = Path.Combine(outputFolderPath, "temp", $"{Path.GetFileNameWithoutExtension(videoPath)}_part{i + 1}.mp4");
				splitParts.Add(partPath);

				double startTime = i * durationPart;

				// Lệnh FFmpeg để cắt từng phần, không dùng "-c copy" để tránh lỗi keyframes
				string splitCommand = $"-i \"{videoPath}\" -ss {startTime} -t {durationPart} -c:v libx264 -crf 18 -preset veryfast -c:a aac \"{partPath}\"";
				ExecuteFFmpegCommand(splitCommand);
			}

			return splitParts;
		}



		private void OverlayText(string videoPath, string text, string outputPath)
		{
			string wrappedText = WrapText(text, 21); // Giả sử maxLineLength là 21
			string fontColor = GetRandomColor(); // Lấy màu ngẫu nhiên

			// Thay thế newline với "\n" cho FFmpeg
			string overlayCommand = $"-i \"{videoPath}\" -vf \"drawtext=text='{wrappedText.Replace("\n", "\\")}':fontcolor={fontColor}:fontsize=36:shadowcolor=white:shadowx=1:shadowy=1:x=(w-text_w)/2:y=h-350:line_spacing=0.01\" -c:a copy \"{outputPath}\"";
			ExecuteFFmpegCommand(overlayCommand);
		}

		private string GetRandomColor()
		{
			// Tạo màu ngẫu nhiên giữa đỏ và vàng đậm
			Random rand = new Random();
			return rand.Next(2) == 0 ? "red" : "gold"; // "gold" là màu vàng đậm
		}

		private string WrapText(string text, int maxLineLength)
		{
			var words = text.Split(' ');
			var wrappedText = new StringBuilder();
			var currentLine = new StringBuilder();

			foreach (var word in words)
			{
				if (currentLine.Length + word.Length + 1 > maxLineLength)
				{
					wrappedText.Append(currentLine.ToString().Trim()); // Thêm dòng đã cắt
					wrappedText.AppendLine(); // Thêm dòng mới
					currentLine.Clear();
				}
				currentLine.Append(word + " ");
			}

			wrappedText.Append(currentLine.ToString().Trim()); // Thêm dòng cuối cùng

			return wrappedText.ToString().Trim(); // Trả về văn bản đã được bao bọc và không có khoảng trắng ở đầu và cuối
		}




		private void OverlayText(string videoPath)
		{
			// Kiểm tra và loại bỏ phần '\output\temp\' trong videoPath nếu nó tồn tại
			string tempFolderPath = Path.Combine(outputFolderPath, "temp");

			if (videoPath.Contains(tempFolderPath))
			{
				// Lấy tên file từ videoPath
				videoPath = Path.GetFileName(videoPath);
			}

			// Chèn chữ "abc" đơn giản vào video
			string overlayCommand = $"-i \"{videoPath}\" -vf \"drawtext=text='abc'\" -c:a copy \"{videoPath}\"";
			ExecuteFFmpegCommand(overlayCommand);
		}

		private double GetVideoDuration(string videoPath)
		{
			string ffprobeCommand = $"-v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 \"{videoPath}\"";
			string output = ExecuteFFprobeCommand(ffprobeCommand);

			if (double.TryParse(output.Trim(), out double duration))
			{
				return duration;
			}

			return 0; // Trả về 0 nếu không lấy được duration
		}

		private string ExecuteFFprobeCommand(string arguments)
		{
			ProcessStartInfo startInfo = new ProcessStartInfo
			{
				FileName = "ffprobe",
				Arguments = arguments,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true
			};

			using (Process process = new Process())
			{
				process.StartInfo = startInfo;
				process.Start();

				string output = process.StandardOutput.ReadToEnd();
				string error = process.StandardError.ReadToEnd();
				process.WaitForExit();

				if (!string.IsNullOrEmpty(error))
				{
					LogErrorToFile(error); // Log lỗi nếu có
				}

				return output;
			}
		}

		private void CombineVideos(List<string> videos, string outputPath, int videoIndex)
		{
			// Tạo đường dẫn tới file concat_list.txt
			string tempFolderPath = Path.Combine(outputFolderPath, "temp");
			string concatFilePath = Path.Combine(tempFolderPath, "concat_list" + videoIndex + ".txt");

			// Thay thế "output/temp/" trong đường dẫn video bằng ""
			var replacedVideos = videos.Select(v => v.Replace(tempFolderPath + Path.DirectorySeparatorChar, "")).ToList();

			// Ghi các đường dẫn video vào concat_list.txt theo format yêu cầu của FFmpeg
			File.WriteAllLines(concatFilePath, replacedVideos.Select(v => $"file '{v}'"));

			// Lệnh FFmpeg để nối các video đã thay thế đường dẫn
			string combineCommand = $"-f concat -safe 0 -i \"{concatFilePath}\" -c copy \"{outputPath}\"";
			ExecuteFFmpegCommand(combineCommand);
		}



		private void LogErrorToFile(string error)
		{
			//string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg_errors.txt");
			//File.AppendAllText(filePath, $"{DateTime.Now}: {error}{Environment.NewLine}");
		}

		private string ExecuteFFmpegCommand(string arguments, bool returnOutput = false)
		{
			ProcessStartInfo startInfo = new ProcessStartInfo
			{
				FileName = "ffmpeg",
				Arguments = arguments,
				RedirectStandardOutput = returnOutput,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true
			};

			using (Process process = new Process())
			{
				process.StartInfo = startInfo;
				process.Start();

				string output = returnOutput ? process.StandardOutput.ReadToEnd() : string.Empty;
				string error = process.StandardError.ReadToEnd();
				process.WaitForExit();

				if (!string.IsNullOrEmpty(error))
				{
					LogErrorToFile(error);
				}

				return output;
			}
		}

		private void btnGoToPath_Click(object sender, EventArgs e)
		{
			string tempFolderPath = Path.Combine(outputFolderPath, "final");
			Process.Start(new ProcessStartInfo
			{
				FileName = tempFolderPath,
				UseShellExecute = true,
				Verb = "open"
			});
		}

		private void btnClearAll_Click(object sender, EventArgs e)
		{
			totalVideoImportAsync = 0;
			completeVideoAsync = 0;
			lstVideos.Items.Clear();
			flowLayoutPanelProgress.Controls.Clear();
			videoPaths.Clear();
			totalprogressBar.Value = 0;
			totallblProgress.Text = "Total Progress: 0%";
			comboBox1.Enabled = true;
			btnImport.Enabled = true;
			btnProcess.Enabled = true;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			comboBox1.SelectedIndex = 0;
			totalVideoImportAsync = 0;
			completeVideoAsync = 0;
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			int index = comboBox1.SelectedIndex;
			switch (index)
			{
				case 0:
					type = 0;
					break;
				case 1:
					type = 1;
					break;
				default: break;
			}
		}
	}
}
