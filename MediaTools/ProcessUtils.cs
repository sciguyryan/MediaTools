using System.Diagnostics;
using System.Text.Json;

namespace MediaTools
{
    internal class ProcessUtils
    {
        public static async Task<FfProbeJson?> RunMediaInfoFull(string path)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    Arguments = $"-v quiet -print_format json -show_format -show_streams \"{path}\"",
                    CreateNoWindow = true,
                    FileName = Program.AppSettings.FfprobePath,
                    RedirectStandardOutput = true,
                },
            };

            process.Start();

            var output = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();

            try
            {
                // If an empty result is returned then this will fail,
                // triggering an exception.
                return JsonSerializer.Deserialize<FfProbeJson>(output);
            }
            catch
            {
                return null;
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
