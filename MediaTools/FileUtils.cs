using System.Runtime.InteropServices;

namespace MediaTools
{
    internal class FileUtils(string basePath)
    {
        #region DLL Imports

        private const uint FO_DELETE = 0x0003; // Trash a file.
        private const uint FOF_NOCONFIRMATION = 0x0010; // Show no confirmation dialog box to the user.
        private const uint FOF_ALLOWUNDO = 0x0040; // Preserve undo information, if possible.

        // Struct which contains information that the SHFileOperation
        // function uses to perform file operations.
        // See: https://learn.microsoft.com/en-us/windows/win32/api/shellapi/ns-shellapi-shfileopstructw
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SHFILEOPSTRUCTW
        {
            public IntPtr hWnd;
            [MarshalAs(UnmanagedType.U4)]
            public uint wFunc;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pFrom;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pTo;
            [MarshalAs(UnmanagedType.U4)]
            public uint fFlags;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fAnyOperationsAborted;
            public IntPtr hNameMappings;
            public string lpszProgressTitle;
        }

        [DllImport("shell32.dll", EntryPoint = "SHFileOperationW", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int SHFileOperationW(ref SHFILEOPSTRUCTW lpFileOp);

        #endregion

        public static int TrashPath(string path)
        {
            // Note that the specification requires a double null termination here.
            // See: https://learn.microsoft.com/en-us/windows/win32/api/shellapi/ns-shellapi-shfileopstructa
            var fileOp = new SHFILEOPSTRUCTW
            {
                wFunc = FO_DELETE,
                pFrom = $"{path}\0\0",
                fFlags = FOF_ALLOWUNDO | FOF_NOCONFIRMATION
            };

            return SHFileOperationW(ref fileOp);
        }

        public static bool TryUntrashPath(string path)
        {
            // I'll have to implement this myself, since the APIs and such aren't behaving.
        }

        public void EnsureTempExists()
        {
            var tempDir = GetTempPath();
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }
        }

        public void MoveTempFiles(string? destFolder)
        {
            // Wait 1.5 seconds to ensure things are fully finished before attempting
            // to move the files. This prevents incomplete files being moved.
            Thread.Sleep(1500);

            var mediaDir = GetMediaPath();
            if (destFolder is not null)
            {
                mediaDir = Path.Combine(mediaDir, destFolder);
            }

            MoveDirectoryContents(GetTempPath(), mediaDir);
        }

        private bool MoveDirectoryContents(string sourceDir, string destDir)
        {
            if (!Directory.Exists(sourceDir))
            {
                var builder = DisplayBuilders.ErrorSourceDirectoryMissing;
                Console.WriteLine(builder.Build([sourceDir]));
                return false;
            }

            if (!Directory.Exists(destDir))
            {
                try
                {
                    Directory.CreateDirectory(destDir);
                }
                catch
                {
                    var builder = DisplayBuilders.ErrorFailedCreateDirectory;
                    Console.WriteLine(builder.Build([destDir]));
                    return false;
                }
            }

            // Move subdirectories recursively.
            var directories = Directory.GetDirectories(sourceDir);
            foreach (var directory in directories)
            {
                var dirName = Path.GetFileName(directory);
                var destDirectory = Path.Combine(destDir, dirName);
                if (!MoveDirectoryContents(directory, destDirectory))
                {
                    return false;
                }
            }

            // Move the individual files.
            var files = Directory.GetFiles(sourceDir);
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var destFile = Path.Combine(destDir, fileName);

                try
                {
                    File.Move(file, destFile, true);
                }
                catch
                {

                    var builder = DisplayBuilders.ErrorFileMoveFailed;
                    Console.WriteLine(builder.Build([file, destFile]));
                    return false;
                }
            }

            try
            {
                if (sourceDir != GetTempPath())
                {
                    Directory.Delete(sourceDir);
                }
            }
            catch
            {
                var builder = DisplayBuilders.ErrorDeleteDirectoryFailed;
                Console.WriteLine(builder.Build([sourceDir]));
                return false;
            }

            return true;
        }

        public string GetTempPath()
        {
            return Path.Combine(basePath, "temp");
        }

        public string GetMediaPath()
        {
            return Path.GetFullPath(Path.Combine(basePath, "..\\"));
        }

        public static void TruncateFile(string path)
        {
            using var f = File.Open(path, FileMode.Truncate);
        }

        public static bool IsFileLocked(string path)
        {
            if (!File.Exists(path))
            {
                // A file that doesn't exist can't be locked.
                return false;
            }

            FileStream? stream = null;

            try
            {
                stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                stream?.Close();
            }

            return false;
        }

        public bool IsMediaInRoot(MediaFileEntry entry)
        {
            var entryDir = new FileInfo(entry.FullPath).DirectoryName!;

            var path1Trimmed =
                Path.GetFullPath(entryDir)
                    .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            var path2Trimmed =
                Path.GetFullPath(GetMediaPath())
                    .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            return string.Equals(path1Trimmed, path2Trimmed, StringComparison.OrdinalIgnoreCase);
        }
    }
}
