using System.Diagnostics;

namespace BatchVideoEditing
{
	public partial class Form1 : Form
	{
		private List<string> videoPaths;
		private string audioFolderPath = @"C:\path\to\audio"; // Thay bằng đường dẫn tới folder chứa file âm thanh
		private string outputFolderPath = "output";

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
			progressBar.Value = 0;
			lblProgress.Text = "Total Progress: 0%";

			if (videoPaths.Count == 0)
			{
				MessageBox.Show("Please import videos first.");
				return;
			}
			btnProcess.Enabled = false;
			await ProcessVideosAsync(videoPaths);

			progressBar.Value = 100;
			lblProgress.Text = "Total Progress: 100%";
			btnGoToPath.Visible = true;
			btnProcess.Enabled = true;
			lstVideos.Items.Clear();
			// Xóa hết mọi thứ trong thư mục temp
			string tempFolderPath = Path.Combine(outputFolderPath, "temp");
			if (Directory.Exists(tempFolderPath))
			{
				Directory.Delete(tempFolderPath, true);
			}
		}

		private async Task ProcessVideosAsync(List<string> videos)
		{
			List<(ProgressBar progressBar, Label label)> progressControls = new List<(ProgressBar, Label)>();

			foreach (var video in videos)
			{
				ProgressBar progressBarVideo = new ProgressBar { Maximum = 100, Width = 250 };
				Label labelVideo = new Label { Text = $"{Path.GetFileName(video)}: 0%", Width = 250 };

				flowLayoutPanelProgress.Controls.Add(labelVideo);
				flowLayoutPanelProgress.Controls.Add(progressBarVideo);
				progressControls.Add((progressBarVideo, labelVideo));
			}

			List<Task> tasks = new List<Task>();

			for (int i = 0; i < videos.Count; i++)
			{
				var video = videos[i];
				var (progressBarVideo, labelVideo) = progressControls[i];

				var task = Task.Run(() =>
				{
					try
					{
						string outputPath = Path.Combine(outputFolderPath, Path.GetFileNameWithoutExtension(video) + "_processed.mp4");

						// Thực hiện các bước xử lý video
						ProcessVideo(video, outputPath);
						UpdateVideoProgress(progressBarVideo, labelVideo, 100);
					}
					catch (Exception ex)
					{
						MessageBox.Show($"Error processing {Path.GetFileName(video)}: {ex.Message}");
					}
				});

				tasks.Add(task);
			}

			await Task.WhenAll(tasks);
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

		private void ProcessVideo(string inputPath, string outputPath)
		{
			// Đảm bảo thư mục đầu ra tồn tại
			if (!Directory.Exists(outputFolderPath))
				Directory.CreateDirectory(outputFolderPath);

			// Tạo thư mục temp và final nếu chưa tồn tại
			string tempFolderPath = Path.Combine(outputFolderPath, "temp");
			string finalFolderPath = Path.Combine(outputFolderPath, "final");
			Directory.CreateDirectory(tempFolderPath);
			Directory.CreateDirectory(finalFolderPath);

			// 1. Crop video lại sao cho chiều dài giảm xuống còn 80% và thêm nền với hiệu ứng blur
			string cropAndBlurPath = Path.Combine(tempFolderPath, "cropped_blurred_" + Path.GetFileName(inputPath));
			CropAndBlurVideo(inputPath, cropAndBlurPath);

			// 2. Cắt video đã chỉnh sửa thành 5 phần
			List<string> splitVideos = SplitVideoIntoParts(cropAndBlurPath, 5);

			// 3. Horizontal flip cho các phần 1, 3, 5
			for (int i = 0; i < splitVideos.Count; i++)
			{
				if (i % 2 == 0)
				{
					string flippedPath = Path.Combine(tempFolderPath, "flipped_" + Path.GetFileName(splitVideos[i]));
					FlipHorizontal(splitVideos[i], flippedPath);
					splitVideos[i] = flippedPath; // Cập nhật đường dẫn sau khi flip
				}
			}

			// 4. Ghép các phần video lại thành 1 video trong temp
			string combinedPath = Path.Combine(tempFolderPath, "combined_" + Path.GetFileNameWithoutExtension(inputPath) + ".mp4");
			CombineVideos(splitVideos, combinedPath);

			// 5. Chèn text vào video và lưu vào thư mục final
			OverlayText(combinedPath, Path.GetFileNameWithoutExtension(inputPath));
			string finalOutputPath = Path.Combine(finalFolderPath, Path.GetFileNameWithoutExtension(inputPath) + "_final.mp4");
			File.Move(combinedPath, finalOutputPath); // Di chuyển video đã hoàn thành sang thư mục final

			// Xóa video tạm sau khi đã hoàn thành (đã di chuyển tất cả video final vào final)
			foreach (var splitVideo in splitVideos)
			{
				if (File.Exists(splitVideo))
				{
					File.Delete(splitVideo);
				}
			}
		}

		// Cập nhật CropAndBlurVideo để chỉ lưu file tạm trong temp
		private void CropAndBlurVideo(string inputPath, string outputPath)
		{
			// Tạo video background với hiệu ứng blur
			string blurredBackgroundPath = Path.Combine(outputFolderPath, "temp", "blurred_background.mp4");
			string blurCommand = $"-i \"{inputPath}\" -vf \"scale=iw:ih,boxblur=50:1\" -c:a copy \"{blurredBackgroundPath}\"";
			ExecuteFFmpegCommand(blurCommand);

			// Cắt video lại còn 80% chiều dài
			string cropPath = Path.Combine(outputFolderPath, "temp", "cropped_" + Path.GetFileName(inputPath));
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

			// Lấy duration chia hết cho 5 và nhỏ hơn hoặc bằng số hiện tại
			double roundedDuration = Math.Floor(duration / 5) * 5;

			// Tính toán số phần
			double durationPart;
			if (roundedDuration > 0 && roundedDuration == duration)
			{
				durationPart = roundedDuration / 5;
			}
			else
			{
				// Chia thành 4 phần bằng nhau và phần còn lại sẽ là phần thứ 5
				durationPart = roundedDuration / 4;
			}

			for (int i = 0; i < parts; i++)
			{
				string partPath = Path.Combine(outputFolderPath, "temp", $"{Path.GetFileNameWithoutExtension(videoPath)}_part{i + 1}.mp4");
				splitParts.Add(partPath);

				double startTime = i * durationPart;

				// Nếu đang chia thành 5 phần thì sử dụng durationPart
				// Nếu đang chia thành 4 phần, phần cuối sẽ là phần còn lại
				double splitDuration = (i < 4) ? durationPart : (duration - (durationPart * 4));

				string splitCommand = $"-i \"{videoPath}\" -ss {startTime} -t {splitDuration} -c copy \"{partPath}\"";
				ExecuteFFmpegCommand(splitCommand);
			}

			return splitParts;
		}


		private void OverlayText(string videoPath, string text)
		{
			string overlayCommand = $"-i \"{videoPath}\" -vf \"drawtext=text='{text}':fontcolor=white:fontsize=24:x=(w-text_w)/2:y=h-text_h-30\" -c:a copy \"{videoPath}\"";
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

		private void CombineVideos(List<string> videos, string outputPath)
		{
			// Tạo đường dẫn tới file concat_list.txt
			string tempFolderPath = Path.Combine(outputFolderPath, "temp");
			string concatFilePath = Path.Combine(tempFolderPath, "concat_list.txt");

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
			string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg_errors.txt");
			File.AppendAllText(filePath, $"{DateTime.Now}: {error}{Environment.NewLine}");
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
			Process.Start(new ProcessStartInfo
			{
				FileName = outputFolderPath,
				UseShellExecute = true,
				Verb = "open"
			});
		}

		private void btnClearAll_Click(object sender, EventArgs e)
		{
			lstVideos.Items.Clear();
			flowLayoutPanelProgress.Controls.Clear();
			videoPaths.Clear();
			progressBar.Value = 0;
			lblProgress.Text = "Progress: 0%";
			btnGoToPath.Visible = false;
		}
	}
}
