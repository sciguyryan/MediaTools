using System.Runtime.InteropServices;

namespace MediaTools
{
    internal class FileUtils(string basePath)
    {
        #region DLL Imports

        private const int FO_DELETE = 0x0003;
        private const int FOF_NOCONFIRMATION = 0x0010; // Show no confirmation dialog box to the user.
        private const int FOF_ALLOWUNDO = 0x0040; // Preserve undo information, if possible.

        // Struct which contains information that the SHFileOperation function uses to perform file operations.
        // See: https://learn.microsoft.com/en-us/windows/win32/api/shellapi/ns-shellapi-shfileopstructw
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SHFILEOPSTRUCTW
        {
            public IntPtr hWnd;
            [MarshalAs(UnmanagedType.U4)]
            public int wFunc;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pFrom;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pTo;
            [MarshalAs(UnmanagedType.U2)]
            public short fFlags;

            [MarshalAs(UnmanagedType.Bool)]
            public bool fAnyOperationsAborted;
            public IntPtr hNameMappings;
            public string lpszProgressTitle;
        }

        [DllImport("shell32.dll", EntryPoint = "SHFileOperationW", SetLastError = true)]
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
                Console.WriteLine(@$"Error! Source directory path '{sourceDir}' doesn't exist!");
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
                    Console.WriteLine(
                        @$"Error! Failed to create destination directory '{destDir}'!"
                    );
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
                    Console.WriteLine(@$"Error! Unable to move file '{file}' to '{destFile}'!");
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
                Console.WriteLine(@"Error! Unable to delete the directory!");
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
    }
}
