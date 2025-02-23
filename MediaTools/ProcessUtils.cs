using System.Diagnostics;
using System.Text.Json;

namespace MediaTools
{
    internal class ProcessUtils
    {
        public static async Task<FfProbeJson?> RunMediaInfo(string path)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    Arguments = $"-v quiet -print_format json -show_format -show_streams \"{path}\"",
                    CreateNoWindow = true,
                    FileName = Program.appSettings.FfprobePath,
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
                    FileName = Program.appSettings.YtDlpPath,
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

        public static void RunMediaPlayer(string? path)
        {
            Process process;

            var playerPath = Program.appSettings.MediaPlayerPath;
            if (File.Exists(playerPath))
            {
                process = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        Arguments = path,
                        FileName = playerPath,
                    }
                };
            }
            else
            {
                process = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = path,
                        UseShellExecute = true,
                    }
                };
            }

            process.Start();
        }
    }
}
