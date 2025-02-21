using System.Diagnostics;

namespace MediaTools
{
    internal class ProcessUtils
    {
        public static async Task<int> RunMediaDuration(string path)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    Arguments = $"-show_entries format=duration -v quiet -of csv=\"p=0\" \"{path}\"",
                    CreateNoWindow = true,
                    FileName = Program.AppSettings.FfprobePath,
                    RedirectStandardOutput = true,
                },
            };

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

        public static async Task RunDownloader(string downloadUrl, string tempPath)
        {
            var tcs = new TaskCompletionSource<bool>();
            var process = new Process()
            {
                EnableRaisingEvents = true,
                StartInfo = new ProcessStartInfo()
                {
                    Arguments = downloadUrl,
                    FileName = Program.AppSettings.YtDlpPath,
                    WorkingDirectory = tempPath
                }
            };

            process.Exited += (_, _) =>
            {
                tcs.SetResult(true);
                process.Dispose();
            };

            process.Start();

            await tcs.Task;
        }
    }
}
