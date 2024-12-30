using System.Diagnostics;

namespace MediaTools
{
    internal class ProcessUtils
    {
        private const string FfProbePath =
            @"D:\Projects\Video Encoding\ffmpeg-7.1\bin\ffprobe.exe";

        public static async Task<int> RunMediaInfo(string path)
        {
            var process = new Process();
            process.StartInfo.FileName = FfProbePath;
            process.StartInfo.Arguments =
                $"-show_entries format=duration -v quiet -of csv=\"p=0\" \"{path}\"";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

            var output = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();

            if (!double.TryParse(output, out var duration))
            {
                return 0;
            }
            else
            {
                return (int)Math.Round(duration);
            }
        }

        public static async Task RunDownloader(string downloadUrl, string basePath, string tempPath)
        {
            var tcs = new TaskCompletionSource<bool>();
            var process = new Process();
            process.StartInfo.FileName = $"{basePath}\\yt-dlp.exe";
            process.StartInfo.Arguments = downloadUrl;
            process.StartInfo.WorkingDirectory = tempPath;
            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) =>
            {
                tcs.SetResult(true);
                process.Dispose();
            };

            process.Start();

            await tcs.Task;
        }
    }
}
